
using System.Collections.Generic;
using Microsoft.Owin.Security;

namespace IsThereAnyNews.ViewModels.Login
{
    public class AuthorizationIndexViewModel
    {
        public List<AuthenticationDescription> Providers { get; set; }
    }
}