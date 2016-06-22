using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.Services.Implementation
{
    public interface ISubscriptionHandlerFactory
    {
        ISubscriptionHandler GetProvider(StreamType streamType);
    }
}