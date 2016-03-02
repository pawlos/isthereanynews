using System.Configuration;

namespace IsThereAnyNews.Infrastructure.ConfigurationReader.Implementation
{
    public class WebConfigReader : IConfigurationReader
    {
        public string FacebookAppId => GetStringValue("Facebook.AppId");
        public string FacebookAppSecret => GetStringValue("Facebook.AppSecret");
        public string TwitterConsumerKey => GetStringValue("Twitter.ConsumerKey");
        public string TwitterConsumerSecret => GetStringValue("Twitter.ConsumerSecret");
        public string GoogleClientId => GetStringValue("Google.ClientId");
        public string GoogleClientSecret => GetStringValue("Google.ClientSecret");

        private string GetStringValue(string key) => ConfigurationManager.AppSettings[key];
    }

}
