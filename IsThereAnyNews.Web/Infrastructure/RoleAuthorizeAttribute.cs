namespace IsThereAnyNews.Mvc.Infrastructure
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using IsThereAnyNews.SharedData;

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

            set
            {
                base.Roles = String.Join(",", value);
            }
        }
    }
}