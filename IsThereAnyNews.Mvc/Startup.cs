namespace IsThereAnyNews.Mvc
{
    using Infrastructure.ConfigurationReader.Implementation;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.Facebook;
    using Microsoft.Owin.Security.Google;
    using Microsoft.Owin.Security.Twitter;
    using Owin;
    using SharedData;

    public class Startup
    {
        public static void Configuration(IAppBuilder appBuilder)
        {
            var cfgReader = new WebConfigReader();
            var cookieAuthenticationOptions = new CookieAuthenticationOptions
            {
                AuthenticationType = ConstantStrings.AuthorizationCookieName,
                LoginPath = new PathString("/Login")
            };
            appBuilder.UseCookieAuthentication(cookieAuthenticationOptions);

            appBuilder.UseFacebookAuthentication(new FacebookAuthenticationOptions
            {
                AppId = cfgReader.FacebookAppId,
                AppSecret = cfgReader.FacebookAppSecret,
                SignInAsAuthenticationType = ConstantStrings.AuthorizationCookieName
            });

            appBuilder.UseTwitterAuthentication(new TwitterAuthenticationOptions
            {
                ConsumerKey = cfgReader.TwitterConsumerKey,
                ConsumerSecret = cfgReader.TwitterConsumerSecret,
                SignInAsAuthenticationType = ConstantStrings.AuthorizationCookieName,
                BackchannelCertificateValidator = null   // can be for demo purposes
            });


            appBuilder.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
            {
                ClientId = cfgReader.GoogleClientId,
                ClientSecret = cfgReader.GoogleClientSecret,
                SignInAsAuthenticationType = ConstantStrings.AuthorizationCookieName
            });
        }
    }
}