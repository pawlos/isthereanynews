namespace IsThereAnyNews.DataAccess
{
    using IsThereAnyNews.SharedData;

    public interface IApplicationSettingsRepository
    {
        RegistrationSupported GetCurrentRegistrationStatus();

        bool CanRegisterWithinLimits();
    }
}