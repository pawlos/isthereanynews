namespace IsThereAnyNews.Services.Handlers
{
    using IsThereAnyNews.SharedData;

    public interface ISubscriptionHandlerFactory
    {
        ISubscriptionHandler GetProvider(StreamType streamType);
    }
}