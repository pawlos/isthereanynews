using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.Mvc.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        public new ItanRole[] Roles
        {
            get
            {
                var roles = base.Roles.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                var itanRoles = roles
                    .Select(r => Enum.Parse(typeof(ItanRole), r))
                    .Cast<ItanRole>()
                    .ToArray();
                return itanRoles;
            }
            set { base.Roles = String.Join(",", value); }
        }
    }
}