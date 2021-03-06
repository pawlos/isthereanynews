namespace IsThereAnyNews.Web
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Web;
    using System.Web.Security;

    public class ItanRoleProvider : RoleProvider
    {
        public override string ApplicationName { get; set; }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            var claimsPrincipal = HttpContext.Current.User as ClaimsPrincipal;
            if (claimsPrincipal == null)
            {
                return new string[]
                           {
                              };
            }

            var claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;
            var rolesForUser = claimsIdentity.Claims.Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value)
                .ToArray();
            return rolesForUser;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var claimsPrincipal = HttpContext.Current.User as ClaimsPrincipal;
            if (claimsPrincipal == null)
            {
                return false;
            }

            var claimsIdentity = claimsPrincipal.Identities.SingleOrDefault(x => x.AuthenticationType == "ITAN");
            if (claimsIdentity == null)
            {
                return false;
            }

            return claimsIdentity.Claims.Where(x => x.Type == ClaimTypes.Role)
                .Any(x => x.Value == roleName);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}