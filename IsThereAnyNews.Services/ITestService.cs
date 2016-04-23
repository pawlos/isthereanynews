namespace IsThereAnyNews.Services
{
    public interface ITestService
    {
        void GenerateUsers();
        void DuplicateChannels();
        void CreateSubscriptions();
        void CreateRssToRead();
        void CreateRssViewedEvent();
        void FixSubscriptions();
        void ReadRandomRssEvent();
    }
}
