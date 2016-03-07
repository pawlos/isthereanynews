using System.Collections.Generic;

namespace IsThereAnyNews.Mvc.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using Models;

    public interface IItanDatabase
    {
        List<ApplicationUser> ApplicationUsers { get; }
        Task SaveAsync();
    }
}