using IsThereAnyNews.EntityFramework.Models;
using IsThereAnyNews.EntityFramework.Models.Entities;
using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.DataAccess
{
    public interface ISocialLoginRepository
    {
        void SaveToDatabase(SocialLogin socialLogin);
        SocialLogin FindSocialLogin(string socialLoginId, AuthenticationTypeProvider provider);
    }
}