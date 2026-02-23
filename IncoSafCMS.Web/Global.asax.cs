using DevExpress.Web;
using System;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace IncosafCMS.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Gọi cấu hình AutoMapper khi khởi động ứng dụng
            //AutoMapperConfig.Configure();

            //AreaRegistration.RegisterAllAreas();
            //RouteConfig.RegisterRoutes(RouteTable.Routes);


            /* edited by lapbt 16-mar-2022. Fix error report can not show before restart iis
             * guide: https://supportcenter.devexpress.com/ticket/details/t680495/internal-server-error-after-app-pool-reset-with-file-cache-enabled
            */
            // DevExpress.XtraReports.Web.WebDocumentViewer.Native.WebDocumentViewerBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Disabled;   // old code
            DevExpress.XtraReports.Web.WebDocumentViewer.DefaultWebDocumentViewerContainer.UseFileDocumentStorage(Server.MapPath("~/App_Data/PreviewCache"));
            DevExpress.XtraReports.Web.WebDocumentViewer.Native.WebDocumentViewerBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Required;
            DevExpress.XtraReports.Web.ASPxWebDocumentViewer.StaticInitialize();

            DevExpress.XtraReports.Web.QueryBuilder.Native.QueryBuilderBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Disabled;
            DevExpress.XtraReports.Web.ReportDesigner.Native.ReportDesignerBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Disabled;
            ASPxWebControl.BackwardCompatibility.DataControlAllowReadUnlistedFieldsFromClientApiDefaultValue = true;
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.DefaultBinder = new DevExpress.Web.Mvc.DevExpressEditorsBinder();
            ASPxWebControl.CallbackError += Application_Error;
        }
        private void Application_Error(object sender, EventArgs e)
        {
            Exception exception = HttpContext.Current.Server.GetLastError();
            if (exception is HttpUnhandledException)
                exception = exception.InnerException;
            AddToLog(exception.Message, exception.StackTrace);
        }
        void AddToLog(string message, string stackTrace)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(DateTime.Now.ToLocalTime().ToString());
            sb.AppendLine(message);
            sb.AppendLine();
            sb.AppendLine("Source File: " + HttpContext.Current.Request.RawUrl);
            sb.AppendLine();
            sb.AppendLine("Stack Trace: ");
            sb.AppendLine(stackTrace);
            for (int i = 0; i < 150; i++)
                sb.Append("-");
            sb.AppendLine();
            HttpContext.Current.Application["Log"] += sb.ToString();
            sb.AppendLine();
        }
    }
}
