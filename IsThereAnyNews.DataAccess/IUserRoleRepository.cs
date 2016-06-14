using System.Collections.Generic;
using IsThereAnyNews.EntityFramework.Models.Entities;

namespace IsThereAnyNews.DataAccess
{
    public interface IUserRoleRepository
    {
        List<UserRole> GetRolesForUser(long currentUserId);
    }
}