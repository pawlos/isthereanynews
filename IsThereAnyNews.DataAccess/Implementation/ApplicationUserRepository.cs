using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.DataAccess.Implementation
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        public object FindById(string currentUserId)
        {
            throw new System.NotImplementedException();
        }

        public void SaveNewUserToDatabase(ItanUser applicationUser)
        {
            throw new System.NotImplementedException();
        }
    }
}