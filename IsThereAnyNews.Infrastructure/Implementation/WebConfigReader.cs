namespace IsThereAnyNews.Infrastructure.Implementation
{
    using System.Configuration;

    using IsThereAnyNews.Web.Interfaces.Infrastructure;

    public class WebConfigReaderWrapper : IConfigurationReaderWrapper
    {
        public string FacebookAppId => this.GetStringValue("Facebook.AppId");

        public string FacebookAppSecret => this.GetStringValue("Facebook.AppSecret");

        public string GoogleClientId => this.GetStringValue("Google.ClientId");

        public string GoogleClientSecret => this.GetStringValue("Google.ClientSecret");

        public string TwitterConsumerKey => this.GetStringValue("Twitter.ConsumerKey");

        public string TwitterConsumerSecret => this.GetStringValue("Twitter.ConsumerSecret");

        private string GetStringValue(string key) => ConfigurationManager.AppSettings[key];
    }
}