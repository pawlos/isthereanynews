namespace IsThereAnyNews.Autofac
{
    using System;
    using System.Reflection;
    using System.Web.Mvc;

    using AutoMapper;

    using EntityFramework;

    using global::Autofac;
    using global::Autofac.Integration.Mvc;

    using Infrastructure.ConfigurationReader;
    using Infrastructure.ConfigurationReader.Implementation;

    using IsThereAnyNews.Automapper;
    using IsThereAnyNews.RssChannelUpdater;
    using IsThereAnyNews.SharedData;

    using Services;
    using Services.Implementation;

    public static class IsThereAnyNewsAutofac
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetCallingAssembly());

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            builder.RegisterType<ItanDatabaseContext>().InstancePerLifetimeScope();
            builder.RegisterType<RssUpdateJob>().InstancePerLifetimeScope();
            builder.RegisterType<RssReadersJob>().InstancePerLifetimeScope();

            builder.RegisterType<WebConfigReader>().As<IConfigurationReader>();
            builder.RegisterType<SessionProvider>().As<ISessionProvider>();
            builder.RegisterType<OpmlReader>().As<IOpmlReader>();

            builder.RegisterType<SessionProvider>().As<ISessionProvider>();
            builder.RegisterType<UserAuthentication>().As<IUserAuthentication>();

            builder.RegisterType<RssSubscriptionHandler>().Keyed<ISubscriptionHandler>(StreamType.Rss);
            builder.RegisterType<PersonSubscriptionHandler>().Keyed<ISubscriptionHandler>(StreamType.Person);

            builder.RegisterType<SubscriptionHandlerFactory>().As<ISubscriptionHandlerFactory>();

            builder.Register(c => IsThereAnyNewsAutomapper.ConfigureMapper()).As<IMapper>()
                .InstancePerLifetimeScope();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
