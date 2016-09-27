namespace IsThereAnyNews.DataAccess.Implementation
{
    using System.Collections.Generic;
    using System.Linq;

    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.SharedData;

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

        public void AssignUserRole(long currentUserId)
        {
            var userRole = new UserRole() { RoleType = ItanRole.User, UserId = currentUserId };
            this.database.UserRoles.Add(userRole);
            this.database.SaveChanges();
        }
    }
}