namespace IsThereAnyNews.Infrastructure.ConfigurationReader.Implementation
{
    using System.Configuration;

    public class WebConfigReader : IConfigurationReader
    {
        public string FacebookAppId => this.GetStringValue("Facebook.AppId");
        public string FacebookAppSecret => this.GetStringValue("Facebook.AppSecret");
        public string TwitterConsumerKey => this.GetStringValue("Twitter.ConsumerKey");
        public string TwitterConsumerSecret => this.GetStringValue("Twitter.ConsumerSecret");
        public string GoogleClientId => this.GetStringValue("Google.ClientId");
        public string GoogleClientSecret => this.GetStringValue("Google.ClientSecret");

        private string GetStringValue(string key) => ConfigurationManager.AppSettings[key];
    }
}
