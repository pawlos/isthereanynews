namespace IsThereAnyNews.ViewModels
{
    using System;

    using IsThereAnyNews.SharedData;

    public class SocialLoginViewModel
    {
        public AuthenticationTypeProvider AuthenticationTypeProvider { get; set; }

        public DateTime Registered { get; set; }
    }
}