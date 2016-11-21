namespace IsThereAnyNews.Autofac
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Web.Compilation;
    using System.Web.Mvc;

    using AutoMapper;

    using EntityFramework;

    using global::Autofac;
    using global::Autofac.Integration.Mvc;

    using Infrastructure.ConfigurationReader;
    using Infrastructure.ConfigurationReader.Implementation;

    using IsThereAnyNews.Automapper;
    using IsThereAnyNews.HtmlStrip;
    using IsThereAnyNews.RssChannelUpdater;
    using IsThereAnyNews.SharedData;

    using Services;
    using Services.Implementation;

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

            builder.RegisterType<ItanDatabaseContext>().InstancePerLifetimeScope();
            builder.RegisterType<RssUpdateJob>().InstancePerLifetimeScope();
            builder.RegisterType<RssReadersJob>().InstancePerLifetimeScope();

            builder.RegisterType<WebConfigReader>().As<IConfigurationReader>();
            builder.RegisterType<OpmlReader>().As<IOpmlReader>();
            builder.RegisterType<HtmlStripper>().As<IHtmlStripper>();

            builder.RegisterType<UserAuthentication>().As<IUserAuthentication>();

            builder.RegisterType<RssSubscriptionHandler>().Keyed<ISubscriptionHandler>(StreamType.Rss);
            builder.RegisterType<PersonSubscriptionHandler>().Keyed<ISubscriptionHandler>(StreamType.Person);

            builder.RegisterType<SubscriptionHandlerFactory>().As<ISubscriptionHandlerFactory>();
            builder.RegisterType<SyndicationFeedAdapter>().As<ISyndicationFeedAdapter>();

            builder.Register(c => IsThereAnyNewsAutomapper.ConfigureMapper()).As<IMapper>()
                .InstancePerLifetimeScope();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
