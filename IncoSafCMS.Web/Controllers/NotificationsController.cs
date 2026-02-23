using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.Services;
using IncosafCMS.Data;
using IncosafCMS.Data.Notify;
using IncosafCMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncosafCMS.Web.Controllers
{
    public class NotificationsController : Controller
    {
        IService<Notification> service;
        IUnitOfWork uow;
        List<Notification> currentMessagesList;
        
        public NotificationsController(IService<Notification> _service, IUnitOfWork _uow)
        {
            service = _service;
            uow = _uow;
            NotificationRegister(_uow.Repository<Notification>());
        }
        private void NotificationRegister(IRepository<Notification> notiRepo)
        {
            return;
            //var notiRepo = uow.Repository<Notification>();
            notiRepo.StartChangesMonitor();
            notiRepo.NotificationRegister(null);
            notiRepo.OnChanged += NotificationsController_OnChanged;
            
            currentMessagesList = notiRepo.GetAll();
        }
        private void NotificationsController_OnChanged(object sender, EventArgs e)
        {
            //var myE = (OnDataChangedEventArg<Notification>)e;
            //var currentMessagesList = myE.Data.Where(item => item.targetUser.UserName == User.Identity.Name);
            //currentMessagesList = myE.Data;
            var rep = (EntityRepository<Notification>)sender;
            rep.OnChanged -= NotificationsController_OnChanged;
            rep.Dispose();
            rep = new EntityRepository<Notification>(new IncosafCMSContext("AppContext", new DebugLogger()));
            //var sev = new Services.Service<Notification>(new UnitOfWork(new IncosafCMSContext("AppContext", new DebugLogger())));
            currentMessagesList = rep.GetAll();
            //currentMessagesList = currentMessagesList.Where(m => m.targetUsers.Any(n => n.UserName == User.Identity.Name)).ToList();
            NotificationHub.SendMessages();
            //notiRepo.OnChanged -= NotificationsController_OnChanged;
            NotificationRegister(rep);
        }

        public ActionResult GetAllMessages()
        {
            return View(currentMessagesList);
        }
        public ActionResult GetMessagesCount()
        {
            return Content(currentMessagesList.Count.ToString());
        }
        public ActionResult RelativeDate(DateTime theDate)
        {
            Dictionary<long, string> thresholds = new Dictionary<long, string>();
            int minute = 60;
            int hour = 60 * minute;
            int day = 24 * hour;
            thresholds.Add(60, "{0} giây trước");
            thresholds.Add(minute * 2, "một phút trước");
            thresholds.Add(45 * minute, "{0} phút trước");
            thresholds.Add(120 * minute, "một giờ trước");
            thresholds.Add(day, "{0} giờ trước");
            thresholds.Add(day * 2, "hôm qua");
            thresholds.Add(day * 30, "{0} ngày trước");
            thresholds.Add(day * 365, "{0} tháng trước");
            thresholds.Add(long.MaxValue, "{0} năm trước");

            long since = (DateTime.Now.Ticks - theDate.Ticks) / 10000000;
            foreach (long threshold in thresholds.Keys)
            {
                if (since < threshold)
                {
                    TimeSpan t = new TimeSpan((DateTime.Now.Ticks - theDate.Ticks));
                    var content = string.Format(thresholds[threshold], (t.Days > 365 ? t.Days / 365 : (t.Days > 0 ? t.Days : (t.Hours > 0 ? t.Hours : (t.Minutes > 0 ? t.Minutes : (t.Seconds > 0 ? t.Seconds : 0))))).ToString());
                    return Content(content);
                }
            }
            return Content("");
        }
        [HttpPost]
        public ActionResult ReadNotification(int Id)
        {
            var notifi = service.GetById(Id);
            if (notifi != null)
            {
                notifi.read = true;
                service.Update(notifi);
            }
            return Content("Success");
        }
    }
}
