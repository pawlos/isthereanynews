namespace IsThereAnyNews.DataAccess
{
    public interface IContactAdministrationEventRepository
    {
        void SaveToDatabase(long contactId);
    }
}