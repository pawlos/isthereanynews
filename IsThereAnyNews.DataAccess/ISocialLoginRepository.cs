namespace IsThereAnyNews.DataAccess
{
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.SharedData;

    public interface ISocialLoginRepository
    {
        void SaveToDatabase(SocialLogin socialLogin);
        SocialLogin FindSocialLogin(string socialLoginId, AuthenticationTypeProvider provider);

        bool UserIsRegistered(AuthenticationTypeProvider authenticationTypeProvider, string userId);
    }
}