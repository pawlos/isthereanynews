namespace IsThereAnyNews.Mvc.Services
{
    using System.Threading.Tasks;

    public interface ILoginService
    {
        Task RegisterIfNewUser();
    }
}