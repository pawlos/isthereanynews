using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.Services
{
    public interface ISubscriptionHandlerFactory
    {
        ISubscriptionHandler GetProvider(StreamType streamType);
    }
}