using System.Collections.Generic;

namespace IsThereAnyNews.Mvc.Services
{
    using System.Threading.Tasks;
    using Models;

    public interface IItanDatabase
    {
        List<ApplicationUser> ApplicationUsers { get; }
        List<RssChannel> RssChannels { get; }
        Task SaveAsync();
    }
}