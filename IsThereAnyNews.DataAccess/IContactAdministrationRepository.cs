namespace IsThereAnyNews.DataAccess
{
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.EntityFramework;

    public interface IContactAdministrationRepository
    {
        long SaveToDatabase(ContactAdministrationDto entity);
    }
}