using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(IncosafCMS.WebApi.Startup))]
namespace IncosafCMS.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //app.Use(
        //(context, next) =>
        //{
        //    if (context.Request.Path.HasValue && context.Request.Path.Value.Contains("/Workflow"))
        //    {
        //        if (context.Request.User != null && context.Request.User.IsInRole("Employee"))
        //        {
        //            return context.Response.WriteAsync("This feature is currently unavailable.");
        //        }
        //    }

        //    return next.Invoke();
        //});
        }
    }
}
