namespace IsThereAnyNews.Web.Interfaces.Infrastructure
{
    public interface IConfigurationReaderWrapper
    {
        string FacebookAppId { get; }

        string FacebookAppSecret { get; }

        string GoogleClientId { get; }

        string GoogleClientSecret { get; }

        string TwitterConsumerKey { get; }

        string TwitterConsumerSecret { get; }
    }
}