namespace IsThereAnyNews.DataAccess
{
    public interface IContactAdministrationEventRepository
    {
        void SaveContactAdministrationEventEventToDatabase(long contactId);
    }
}