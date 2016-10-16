namespace IsThereAnyNews.Services
{
    using IsThereAnyNews.ViewModels;

    public interface IAdminService
    {
        ItanApplicationConfigurationViewModel ReadConfiguration();
    }
}