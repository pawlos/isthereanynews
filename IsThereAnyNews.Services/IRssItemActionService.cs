namespace IsThereAnyNews.Services
{
    public interface IRssItemActionService
    {
        void CurrentVoteupForArticleByCurrentUser(long id);
        void MarkRssItemAsNotReadByCurrentUser(long id);
        void ShareRssItem(long id);
        void AddCommentToRssItemByCurrentUser(long id);
        void OpenFullArticle(long id);
        void AddToReadLaterQueueForCurrentUser(long id);
        void CurrentVotedownForArticleByCurrentUser(long id);
    }
}