using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.DomainModels.Identity;
using IncosafCMS.Data;
using IncosafCMS.Data.Identity;
using IncosafCMS.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IncosafCMS.Web
{
    /// <summary>
    /// Custom authorization attribute for setting per-method accessibility 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SetPermissionsAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// The name of each action that must be permissible for this method, separated by a comma.
        /// </summary>
        public string Permission { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var context = new IncosafCMSContext("name=AppContext", new DebugLogger());
            EntityRepository<AppPermission> permissionsRep = new EntityRepository<AppPermission>(context);

            var applicationUserManager = IdentityFactory.CreateUserManager(context);
            var roleManager = IdentityFactory.CreateRoleManager(context);

            bool isUserAuthorized = base.AuthorizeCore(httpContext);

            var perms = permissionsRep.FindBy(e => e.Name == Permission).FirstOrDefault();

            if (perms?.Roles?.Count() > 0)
            {
                foreach (var item in perms.Roles)
                {
                    var currentUserId = httpContext.User.Identity.GetUserId<int>();
                    var relatedPermisssionRole = roleManager.FindById(item.RoleId).Name;
                    if (applicationUserManager.IsInRole(currentUserId, relatedPermisssionRole))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
        }
    }
    public class DynamicRoleAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var controller = httpContext.Request.RequestContext
                .RouteData.GetRequiredString("controller");
            var action = httpContext.Request.RequestContext
                .RouteData.GetRequiredString("action");
            // feed the roles here
            //Roles = string.Join(",", _rolesProvider.Get(controller, action));
            return base.AuthorizeCore(httpContext);

            //[DynamicRoleAuthorize]
            //public ActionResult MyAction()
            //{

            //}
        }
    }
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {

        public CustomAuthorizeAttribute(string roleSelector)
        {

            Roles = GetRoles(roleSelector);
        }

        private string GetRoles(string roleSelector)
        {
            // Do something to get the dynamic list of roles instead of returning a hardcoded string
            var context = new IncosafCMSContext("name=AppContext", new DebugLogger());
            EntityRepository<AppPermission> permissions = new EntityRepository<AppPermission>(context);
            var alll = permissions.GetAll();
            //var pe = alll.Where(e => e.Name == roleSelector && e.Roles.Any(item => item.Name == "KDV"));
            return "Somerole";
        }

        //[Authorize(Roles = GetRoles("UpdateProduct"))]
    }
}
