namespace IsThereAnyNews.Web
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using IsThereAnyNews.Infrastructure.ConfigurationReader.Implementation;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.Web.Models;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.Facebook;
    using Microsoft.Owin.Security.Google;
    using Microsoft.Owin.Security.Twitter;

    using Owin;

    public partial class Startup
    {
        private WebConfigReader configurationReader;

        public Startup()
        {
            this.configurationReader = new WebConfigReader();
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Social/Login"),
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            app.UseFacebookAuthentication(new FacebookAuthenticationOptions
            {
                AppId = this.configurationReader.FacebookAppId,
                AppSecret = this.configurationReader.FacebookAppSecret,
                Scope =
                    {
                        "email"
                    },
                BackchannelHttpHandler = new FacebookBackChannelHandler(),
                UserInformationEndpoint = "https://graph.facebook.com/v2.4/me?fields=id,name,email",
                Provider = new FacebookAuthenticationProvider
                {
                    OnAuthenticated = (context) =>
                    {
                        context.Identity.AddClaim(new System.Security.Claims.Claim("FacebookAccessToken", context.AccessToken));
                        foreach (var claim in context.User)
                        {
                            var claimType = string.Format("urn:facebook:{0}", claim.Key);
                            string claimValue = claim.Value.ToString();
                            if (!context.Identity.HasClaim(claimType, claimValue))
                            {
                                context.Identity.AddClaim(new System.Security.Claims.Claim(claimType, claimValue, "XmlSchemaString", "Facebook"));
                            }

                        }

                        return Task.FromResult(0);
                    }
                }
            });

            app.UseTwitterAuthentication(new TwitterAuthenticationOptions
            {
                ConsumerKey = this.configurationReader.TwitterConsumerKey,
                ConsumerSecret = this.configurationReader.TwitterConsumerSecret,
                SignInAsAuthenticationType = ConstantStrings.AuthorizationCookieName,
                BackchannelCertificateValidator = null   // can be for demo purposes
            });


            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
            {
                ClientId = this.configurationReader.GoogleClientId,
                ClientSecret = this.configurationReader.GoogleClientSecret
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