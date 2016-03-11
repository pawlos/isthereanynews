using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.DataAccess
{
    public interface IApplicationUserRepository
    {
        object FindById(string currentUserId);
        void SaveNewUserToDatabase(ItanUser applicationUser);
    }
}