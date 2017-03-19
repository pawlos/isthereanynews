namespace IsThereAnyNews.RssChannelUpdater
{
    using Autofac;
    using Autofac.Integration.Mvc;

    using FluentScheduler;

    public class AutofacRegistry : IJobFactory
    {
        public IJob GetJobInstance<T>() where T : IJob
        {
            return AutofacDependencyResolver.Current.ApplicationContainer.Resolve<T>();
        }
    }
}