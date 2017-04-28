namespace IsThereAnyNews.Autofac
{
    using System.Linq;
    using System.Reflection;
    using System.Web.Compilation;
    using System.Web.Mvc;

    using AutoMapper;

    using global::Autofac;
    using global::Autofac.Integration.Mvc;

    using IsThereAnyNews.Automapper;
    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.HtmlStrip;
    using IsThereAnyNews.Infrastructure.Implementation;
    using IsThereAnyNews.Infrastructure.Import.Opml;
    using IsThereAnyNews.Infrastructure.Web;
    using IsThereAnyNews.RssChannelUpdater;
    using IsThereAnyNews.Services;
    using IsThereAnyNews.Services.Handlers;
    using IsThereAnyNews.Services.Handlers.Implementation;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.Web.Interfaces.Infrastructure;
    using IsThereAnyNews.Web.Interfaces.Services;

    public static class IsThereAnyNewsAutofac
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray();

            builder.RegisterControllers(assemblies);

            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(assemblies)
            .Where(t => t.Name.EndsWith("Wrapper"))
            .AsImplementedInterfaces();

            builder.RegisterType<ItanDatabaseContext>().InstancePerLifetimeScope();
            builder.RegisterType<RssUpdateJob>().InstancePerLifetimeScope();

            builder.RegisterType<HtmlStripper>().As<IHtmlStripper>();
            builder.RegisterType<Infrastructure>().As<IInfrastructure>();
            builder.RegisterType<ImportOpml>().As<IImportOpml>();
            builder.RegisterType<WebConfigReaderWrapper>().As<IConfigurationReaderWrapper>();

            builder.RegisterType<UserAuthentication>().As<IUserAuthentication>();

            builder.RegisterType<RssSubscriptionHandler>().Keyed<ISubscriptionHandler>(StreamType.Rss);
            builder.RegisterType<PersonSubscriptionHandler>().Keyed<ISubscriptionHandler>(StreamType.Person);
            builder.RegisterType<ChannelUpdatesSubscriptionHandler>().Keyed<ISubscriptionHandler>(StreamType.Channel);
            builder.RegisterType<ExceptionSubscriptionHandler>().Keyed<ISubscriptionHandler>(StreamType.Exception);

            builder.RegisterType<SubscriptionHandlerFactory>().As<ISubscriptionHandlerFactory>();

            builder.Register(c => IsThereAnyNewsAutomapper.ConfigureMapper()).As<IMapper>()
                .InstancePerLifetimeScope();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
