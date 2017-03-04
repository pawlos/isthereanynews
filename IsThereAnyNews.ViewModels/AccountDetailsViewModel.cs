namespace IsThereAnyNews.ViewModels
{
    using System;
    using System.Collections.Generic;

    public class AccountDetailsViewModel
    {
        public string DisplayName { get; set; }

        public string Email { get; set; }

        public DateTime Registered { get; set; }

        public List<SocialLoginViewModel> SocialLogins { get; set; }
    }
}