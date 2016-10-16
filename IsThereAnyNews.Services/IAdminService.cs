namespace IsThereAnyNews.Services
{
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.ViewModels;

    public interface IAdminService
    {
        ItanApplicationConfigurationViewModel ReadConfiguration();

        void ChangeRegistration(ChangeRegistrationDto dto);

        void ChangeUsersLimit(ChangeUsersLimitDto dto);
    }
}