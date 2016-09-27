namespace IsThereAnyNews.DataAccess
{
    using System.Collections.Generic;

    using IsThereAnyNews.EntityFramework.Models.Entities;

    public interface IUserRoleRepository
    {
        List<UserRole> GetRolesForUser(long currentUserId);

        void AssignUserRole(long currentUserId);
    }
}