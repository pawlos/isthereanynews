namespace IsThereAnyNews.DataAccess
{
    using System.Collections.Generic;

    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.SharedData;

    public interface IUserRoleRepository
    {
        List<UserRole> GetRolesForUser(long currentUserId);

        void AssignUserRole(long currentUserId);

        List<ItanRole> GetRolesTypesForUser(long currentUserId);
    }
}