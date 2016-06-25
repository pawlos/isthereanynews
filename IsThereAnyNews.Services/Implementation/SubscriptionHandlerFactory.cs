using Autofac.Features.Indexed;
using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.Services.Implementation
{
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