using System.Web.Mvc;
using IncosafCMS.Core.Data;
using IncosafCMS.Core.Logging;
using IncosafCMS.Core.Services;
using IncosafCMS.Data;
using IncosafCMS.Infrastructure.Logging;
using IncosafCMS.Services;
using Autofac;
using Autofac.Integration.Mvc;
using IncosafCMS.Core.Identity;
using IncosafCMS.Data.Identity;
using IncosafCMS.WebApi;
using Autofac.Integration.WebApi;
using System.Web.Http;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(IocConfig), "RegisterDependencies")]

namespace IncosafCMS.WebApi
{
    public class IocConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            const string nameOrConnectionString = "name=AppContext";
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterApiControllers(typeof(MvcApplication).Assembly);
            builder.RegisterModule<AutofacWebTypesModule>();
            builder.RegisterGeneric(typeof(EntityRepository<>)).As(typeof(IRepository<>)).InstancePerRequest();
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerRequest();
            builder.RegisterType(typeof(UnitOfWork)).As(typeof(IUnitOfWork)).InstancePerRequest();

            builder.Register<IEntitiesContext>(b =>
            {
                var logger = b.Resolve<ILogger>();
                var context = new IncosafCMSContext(nameOrConnectionString, logger);
                return context;
            }).InstancePerRequest();
            builder.Register(b => NLogLogger.Instance).SingleInstance();
            builder.RegisterModule(new IdentityModule());

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
