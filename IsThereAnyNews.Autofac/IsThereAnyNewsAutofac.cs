using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.DataAccess.Implementation;
using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.Infrastructure.ConfigurationReader;
using IsThereAnyNews.Infrastructure.ConfigurationReader.Implementation;
using IsThereAnyNews.Services;
using IsThereAnyNews.Services.Implementation;

namespace IsThereAnyNews.Autofac
{
    public static class IsThereAnyNewsAutofac
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetCallingAssembly()).InstancePerLifetimeScope();

            builder.RegisterType<RssChannelRepository>().As<IRssChannelRepository>();
            builder.RegisterType<RssChannelsRepository>().As<IRssChannelsRepository>();
            builder.RegisterType<RssChannelsSubscriptionsRepository>().As<IRssChannelsSubscriptionsRepository>();
            builder.RegisterType<SocialLoginRepository>().As<ISocialLoginRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();

            builder.RegisterType<ItanDatabaseContext>().InstancePerLifetimeScope();

            builder.RegisterType<WebConfigReader>().As<IConfigurationReader>();
            builder.RegisterType<SessionProvider>().As<ISessionProvider>();

            builder.RegisterType<ApplicationLoginService>().As<ILoginService>();
            builder.RegisterType<OpmlImporterService>().As<IOpmlImporterService>();
            builder.RegisterType<RssChannelService>().As<IRssChannelService>();
            builder.RegisterType<RssChannelsService>().As<IRssChannelsService>();
            builder.RegisterType<SessionProvider>().As<ISessionProvider>();
            builder.RegisterType<UserAuthentication>().As<IUserAuthentication>();


            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
