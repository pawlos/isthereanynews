namespace IsThereAnyNews.Mvc.Services
{
    using System.Security.Claims;

    public interface IUserAuthentication
    {
        string GetCurrentUserId();
        ClaimsPrincipal GetCurrentUser();
    }
}