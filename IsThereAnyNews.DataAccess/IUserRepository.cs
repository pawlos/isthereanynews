using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.DataAccess
{
    public interface IUserRepository
    {
        User CreateNewUser();
    }
}