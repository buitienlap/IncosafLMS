using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncosafCMS.Web.Controllers
{
    [Authorize(Roles = "Accountant")]
    public class FinanceController : Controller
    {
        // GET: Finance
        public ActionResult Index()
        {
            return View();
        }
    }
}