namespace IsThereAnyNews.Mvc
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using IsThereAnyNews.Infrastructure.ConfigurationReader.Implementation;

    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.Facebook;
    using Microsoft.Owin.Security.Google;
    using Microsoft.Owin.Security.Twitter;

    using Owin;

    using SharedData;

    public static class Startup
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
                Scope =
                    {
                        "email"
                    },
                SignInAsAuthenticationType = ConstantStrings.AuthorizationCookieName,
                BackchannelHttpHandler = new FacebookBackChannelHandler(),
                UserInformationEndpoint = "https://graph.facebook.com/v2.4/me?fields=id,name,email",
                Provider = new FacebookAuthenticationProvider
                {
                    OnAuthenticated = (context) =>
                    {
                        //context.Identity.AddClaim(new System.Security.Claims.Claim("FacebookAccessToken", context.AccessToken));
                        ////foreach (var claim in context.User)
                        ////{
                        //    //var claimType = string.Format("urn:facebook:{0}", claim.Key);
                        //    //string claimValue = claim.Value.ToString();
                        //    //if (!context.Identity.HasClaim(claimType, claimValue))
                        //    //{
                        //        //context.Identity.AddClaim(new System.Security.Claims.Claim(claimType, claimValue, "XmlSchemaString", "Facebook"));
                        //    //}

                        ////}

                        return System.Threading.Tasks.Task.FromResult(0);
                    }
                }
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

    public class FacebookBackChannelHandler : HttpClientHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            {
                // Replace the RequestUri so it's not malformed
                if (!request.RequestUri.AbsolutePath.Contains("/oauth"))
                {
                    request.RequestUri = new Uri(request.RequestUri.AbsoluteUri.Replace("?access_token", "&access_token"));
                }

                return await base.SendAsync(request, cancellationToken);
            }
        }
    }
}