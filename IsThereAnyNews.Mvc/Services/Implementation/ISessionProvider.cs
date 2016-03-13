namespace IsThereAnyNews.Mvc.Services.Implementation
{
    public interface ISessionProvider
    {
        void SetUserId(long userId);
        long GetCurrentUserId();
    }
}