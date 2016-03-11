using IsThereAnyNews.Mvc.Models;

namespace IsThereAnyNews.Mvc.Repositories
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