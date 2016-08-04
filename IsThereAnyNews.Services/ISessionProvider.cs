namespace IsThereAnyNews.Services
{
    using System.Collections.Generic;
    using System.Security.Claims;

    public interface ISessionProvider
    {
        void SetUserId(long userId);
        long GetCurrentUserId();
        List<Claim> LoadClaims();
        void SaveClaims(IEnumerable<Claim> claims);
    }
}