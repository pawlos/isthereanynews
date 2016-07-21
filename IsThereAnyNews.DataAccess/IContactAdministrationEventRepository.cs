namespace IsThereAnyNews.DataAccess
{
    using IsThereAnyNews.EntityFramework.Models.Events;

    public interface IContactAdministrationEventRepository
    {
        void SaveToDatabase(ContactAdministrationEvent contactEvent);
    }
}