using System.Collections.Generic;
using System.Linq;
using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.EntityFramework.Models.Entities;

namespace IsThereAnyNews.DataAccess.Implementation
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly ItanDatabaseContext database;

        public UserRoleRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public List<UserRole> GetRolesForUser(long currentUserId)
        {
            return this.database
                .UserRoles
                .Where(r => r.UserId == currentUserId)
                .ToList();
        }
    }
}