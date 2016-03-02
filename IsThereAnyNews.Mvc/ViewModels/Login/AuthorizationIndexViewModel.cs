namespace IsThereAnyNews.Mvc.ViewModels.Login
{
    using System.Collections.Generic;
    using Microsoft.Owin.Security;

    public class AuthorizationIndexViewModel
    {
        public List<AuthenticationDescription> Providers { get; set; }
    }
}