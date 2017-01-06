namespace IsThereAnyNews.DataAccess
{
    using IsThereAnyNews.ProjectionModels;

    public interface IUpdateRepository
    {
        RssChannelForUpdateDTO LoadChannelToUpdate();
    }
}