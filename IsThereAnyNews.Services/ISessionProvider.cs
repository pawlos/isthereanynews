namespace IsThereAnyNews.Services
{
    public interface ISessionProvider
    {
        void SetUserId(long userId);
        long GetCurrentUserId();
    }
}