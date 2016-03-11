using IsThereAnyNews.Mvc.Models;

namespace IsThereAnyNews.Mvc.Repositories
{
    internal interface IApplicationUserRepository
    {
        object FindById(string currentUserId);
        void SaveNewUserToDatabase(ItanUser applicationUser);
    }
}