namespace IsThereAnyNews.DataAccess
{
    public interface IRssActionRepository
    {
        void AddVoteUpRequestByUserForArticle(long userId, long id);
        void AddNotReadRequestByUserForArticle(long userId, long id);
        void AddShareRequestByUserForArticle(long userId, long id);
        void AddCommentRequestByUserForArticle(long userId, long id);
        void AddFullArticleRequestByUserForArticle(long userId, long id);
        void AddReadLaterRequestByUserForArticle(long userId, long id);
        void AddVoteDownRequestByUserForArticle(long userId, long id);
    }
}