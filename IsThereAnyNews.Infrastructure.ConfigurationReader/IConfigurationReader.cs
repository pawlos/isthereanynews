namespace IsThereAnyNews.Infrastructure.ConfigurationReader
{
    public interface IConfigurationReader
    {
        string FacebookAppId { get; }
        string FacebookAppSecret { get; }

        string TwitterConsumerKey { get; }
        string TwitterConsumerSecret { get; }

        string GoogleClientId { get; }
        string GoogleClientSecret { get; }
    }
}