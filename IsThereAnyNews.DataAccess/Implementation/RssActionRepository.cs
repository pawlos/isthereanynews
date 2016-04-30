using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.EntityFramework.Models;
using IsThereAnyNews.EntityFramework.Models.Entities;
using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.DataAccess.Implementation
{
    public class RssActionRepository : IRssActionRepository
    {
        private readonly ItanDatabaseContext database;

        public RssActionRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public void AddVoteUpRequestByUserForArticle(long userId, long id)
        {
            var featureRequest = new FeatureRequest
            {
                UserId = userId,
                RssEntryId = id,
                Type = FeatureRequestType.Voteup
            };

            this.database.FeatureRequests.Add(featureRequest);
            this.database.SaveChanges();
        }

        public void AddNotReadRequestByUserForArticle(long userId, long id)
        {
            var featureRequest = new FeatureRequest
            {
                UserId = userId,
                RssEntryId = id,
                Type = FeatureRequestType.NotRead
            };

            this.database.FeatureRequests.Add(featureRequest);
            this.database.SaveChanges();
        }

        public void AddShareRequestByUserForArticle(long userId, long id)
        {
            var featureRequest = new FeatureRequest
            {
                UserId = userId,
                RssEntryId = id,
                Type = FeatureRequestType.Share
            };

            this.database.FeatureRequests.Add(featureRequest);
            this.database.SaveChanges();
        }

        public void AddCommentRequestByUserForArticle(long userId, long id)
        {
            var featureRequest = new FeatureRequest
            {
                UserId = userId,
                RssEntryId = id,
                Type = FeatureRequestType.AddComment
            };

            this.database.FeatureRequests.Add(featureRequest);
            this.database.SaveChanges();
        }

        public void AddFullArticleRequestByUserForArticle(long userId, long id)
        {
            var featureRequest = new FeatureRequest
            {
                UserId = userId,
                RssEntryId = id,
                Type = FeatureRequestType.FullArticle
            };

            this.database.FeatureRequests.Add(featureRequest);
            this.database.SaveChanges();
        }

        public void AddReadLaterRequestByUserForArticle(long userId, long id)
        {
            var featureRequest = new FeatureRequest
            {
                UserId = userId,
                RssEntryId = id,
                Type = FeatureRequestType.ReadLater
            };

            this.database.FeatureRequests.Add(featureRequest);
            this.database.SaveChanges();
        }

        public void AddVoteDownRequestByUserForArticle(long userId, long id)
        {
            var featureRequest = new FeatureRequest
            {
                UserId = userId,
                RssEntryId = id,
                Type = FeatureRequestType.VoteDown
            };

            this.database.FeatureRequests.Add(featureRequest);
            this.database.SaveChanges();
        }
    }
}