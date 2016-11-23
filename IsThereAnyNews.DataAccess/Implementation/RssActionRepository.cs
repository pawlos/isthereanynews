namespace IsThereAnyNews.DataAccess.Implementation
{
    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.SharedData;

    public class RssActionRepository : IRssActionRepository
    {
        private readonly ItanDatabaseContext database;

        public RssActionRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public void AddCommentRequestByUserForArticle(long userId, StreamType modelStreamType, long id)
        {
            var featureRequest = new FeatureRequest(userId, id, modelStreamType, FeatureRequestType.AddComment);
            this.SaveFeatureRequestToDatabase(featureRequest);
        }

        public void AddFullArticleRequestByUserForArticle(long userId, StreamType modelStreamType, long id)
        {
            var featureRequest = new FeatureRequest(userId, id, modelStreamType, FeatureRequestType.FullArticle);
            this.SaveFeatureRequestToDatabase(featureRequest);
        }

        public void AddNotReadRequestByUserForArticle(long userId, StreamType modelStreamType, long id)
        {
            var featureRequest = new FeatureRequest(userId, id, modelStreamType, FeatureRequestType.NotRead);
            this.SaveFeatureRequestToDatabase(featureRequest);
        }

        public void AddReadLaterRequestByUserForArticle(long userId, StreamType modelStreamType, long id)
        {
            var featureRequest = new FeatureRequest(userId, id, modelStreamType, FeatureRequestType.ReadLater);
            this.SaveFeatureRequestToDatabase(featureRequest);
        }

        public void AddShareRequestByUserForArticle(long userId, StreamType modelStreamType, long id)
        {
            var featureRequest = new FeatureRequest(userId, id, modelStreamType, FeatureRequestType.Share);
            this.SaveFeatureRequestToDatabase(featureRequest);
        }

        public void AddVoteDownRequestByUserForArticle(long userId, StreamType modelStreamType, long id)
        {
            var featureRequest = new FeatureRequest(userId, id, modelStreamType, FeatureRequestType.VoteDown);
            this.SaveFeatureRequestToDatabase(featureRequest);
        }

        public void AddVoteUpRequestByUserForArticle(long userId, StreamType modelStreamType, long id)
        {
            var featureRequest = new FeatureRequest(userId, id, modelStreamType, FeatureRequestType.Voteup);
            this.SaveFeatureRequestToDatabase(featureRequest);
        }

        private void SaveFeatureRequestToDatabase(FeatureRequest featureRequest)
        {
            this.database.FeatureRequests.Add(featureRequest);
            this.database.SaveChanges();
        }
    }
}