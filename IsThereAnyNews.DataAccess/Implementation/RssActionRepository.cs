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

        public void AddVoteUpRequestByUserForArticle(long userId, StreamType modelStreamType, long id)
        {
            var featureRequest = new FeatureRequest
            {
                UserId = userId,
                RssEntryId = id,
                StreamType = modelStreamType,
                Type = FeatureRequestType.Voteup
            };

            this.database.FeatureRequests.Add(featureRequest);
            this.database.SaveChanges();
        }

        public void AddNotReadRequestByUserForArticle(long userId, StreamType modelStreamType, long id)
        {
            var featureRequest = new FeatureRequest
            {
                UserId = userId,
                RssEntryId = id,
                StreamType = modelStreamType,
                Type = FeatureRequestType.NotRead
            };

            this.database.FeatureRequests.Add(featureRequest);
            this.database.SaveChanges();
        }

        public void AddShareRequestByUserForArticle(long userId, StreamType modelStreamType, long id)
        {
            var featureRequest = new FeatureRequest
            {
                UserId = userId,
                RssEntryId = id,
                StreamType = modelStreamType,
                Type = FeatureRequestType.Share
            };

            this.database.FeatureRequests.Add(featureRequest);
            this.database.SaveChanges();
        }

        public void AddCommentRequestByUserForArticle(long userId, StreamType modelStreamType, long id)
        {
            var featureRequest = new FeatureRequest
            {
                UserId = userId,
                RssEntryId = id,
                StreamType = modelStreamType,
                Type = FeatureRequestType.AddComment
            };

            this.database.FeatureRequests.Add(featureRequest);
            this.database.SaveChanges();
        }

        public void AddFullArticleRequestByUserForArticle(long userId, StreamType modelStreamType, long id)
        {
            var featureRequest = new FeatureRequest
            {
                UserId = userId,
                RssEntryId = id,
                StreamType = modelStreamType,
                Type = FeatureRequestType.FullArticle
            };

            this.database.FeatureRequests.Add(featureRequest);
            this.database.SaveChanges();
        }

        public void AddReadLaterRequestByUserForArticle(long userId, StreamType modelStreamType, long id)
        {
            var featureRequest = new FeatureRequest
            {
                UserId = userId,
                RssEntryId = id,
                StreamType = modelStreamType,
                Type = FeatureRequestType.ReadLater
            };

            this.database.FeatureRequests.Add(featureRequest);
            this.database.SaveChanges();
        }

        public void AddVoteDownRequestByUserForArticle(long userId, StreamType modelStreamType, long id)
        {
            var featureRequest = new FeatureRequest
            {
                UserId = userId,
                RssEntryId = id,
                StreamType = modelStreamType,
                Type = FeatureRequestType.VoteDown
            };

            this.database.FeatureRequests.Add(featureRequest);
            this.database.SaveChanges();
        }
    }
}