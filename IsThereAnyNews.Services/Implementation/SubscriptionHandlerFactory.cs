namespace IsThereAnyNews.Services.Implementation
{
    using Autofac.Features.Indexed;

    using IsThereAnyNews.SharedData;

    public class SubscriptionHandlerFactory : ISubscriptionHandlerFactory
    {
        private readonly IIndex<StreamType, ISubscriptionHandler> handlers;

        public SubscriptionHandlerFactory(IIndex<StreamType, ISubscriptionHandler> handlers)
        {
            this.handlers = handlers;
        }

        public ISubscriptionHandler GetProvider(StreamType streamType)
        {
            return this.handlers[streamType];
        }
    }
}