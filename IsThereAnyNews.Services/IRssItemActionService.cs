namespace IsThereAnyNews.Services
{
    using IsThereAnyNews.Dtos;

    public interface IRssItemActionService
    {
        void CurrentVoteupForArticleByCurrentUser(RssActionModel id);

        void MarkRssItemAsNotReadByCurrentUser(RssActionModel id);

        void ShareRssItem(RssActionModel id);

        void AddCommentToRssItemByCurrentUser(RssActionModel id);

        void AddToReadLaterQueueForCurrentUser(RssActionModel id);

        void CurrentVotedownForArticleByCurrentUser(RssActionModel id);
    }
}