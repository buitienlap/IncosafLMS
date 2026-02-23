namespace IncosafCMS.Web.Migrations
{
    using IncosafCMS.Core.DomainModels.Identity;
    using IncosafCMS.Web.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<IncosafCMS.Web.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Models.ApplicationDbContext context)
        {
            var manager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(
                    new ApplicationDbContext()));
            var user = new ApplicationUser()
            {
                UserName = "admin"
            };
            manager.Create(user, "admin@123");

            var roleMan = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(new ApplicationDbContext()));
            roleMan.Create(new ApplicationRole() { Name = "Admin" });
            

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
