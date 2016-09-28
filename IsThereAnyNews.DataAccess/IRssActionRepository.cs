namespace IsThereAnyNews.DataAccess
{
    using IsThereAnyNews.SharedData;

    public interface IRssActionRepository
    {
        void AddVoteUpRequestByUserForArticle(long userId, StreamType modelStreamType, long id);
        void AddNotReadRequestByUserForArticle(long userId, StreamType modelStreamType, long id);
        void AddShareRequestByUserForArticle(long userId, StreamType modelStreamType, long id);
        void AddCommentRequestByUserForArticle(long userId, StreamType modelStreamType, long id);
        void AddFullArticleRequestByUserForArticle(long userId, StreamType modelStreamType, long id);
        void AddReadLaterRequestByUserForArticle(long userId, StreamType modelStreamType, long id);
        void AddVoteDownRequestByUserForArticle(long userId, StreamType modelStreamType, long id);
    }
}