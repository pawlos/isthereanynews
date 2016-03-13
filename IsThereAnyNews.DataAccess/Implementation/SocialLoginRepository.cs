using System.Linq;
using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.EntityFramework.Models;
using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.DataAccess.Implementation
{
    public class SocialLoginRepository : ISocialLoginRepository
    {
        private readonly ItanDatabaseContext itanDatabaseContext;

        public SocialLoginRepository(ItanDatabaseContext itanDatabaseContext)
        {
            this.itanDatabaseContext = itanDatabaseContext;
        }

        public void SaveToDatabase(SocialLogin socialLogin)
        {
            this.itanDatabaseContext.SocialLogins.Add(socialLogin);
            this.itanDatabaseContext.SaveChanges();
        }

        public SocialLogin FindSocialLogin(string socialLoginId, AuthenticationTypeProvider provider)
        {
            var socialLogin = this.itanDatabaseContext
                .SocialLogins
                .Where(login => login.SocialId == socialLoginId)
                .Where(login => login.Provider == provider)
                .SingleOrDefault();

            return socialLogin;
        }

        
    }
}