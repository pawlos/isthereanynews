namespace IsThereAnyNews.Services.Implementation
{
    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;

    public class RssItemActionService : IRssItemActionService
    {
        private readonly IUserAuthentication authentication;
        private readonly IEntityRepository entityRepository;

        public RssItemActionService(
            IUserAuthentication authentication, IEntityRepository entityRepository)
        {
            this.authentication = authentication;
            this.entityRepository = entityRepository;
        }

        public void CurrentVoteupForArticleByCurrentUser(RssActionModel model)
        {
            var userId = this.authentication.GetCurrentUserId();
            this.entityRepository.AddVoteUpRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void MarkRssItemAsNotReadByCurrentUser(RssActionModel model)
        {
            var userId = this.authentication.GetCurrentUserId();
            this.entityRepository.AddNotReadRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void ShareRssItem(RssActionModel model)
        {
            var userId = this.authentication.GetCurrentUserId();
            this.entityRepository.AddShareRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void AddCommentToRssItemByCurrentUser(RssActionModel model)
        {
            var userId = this.authentication.GetCurrentUserId();
            this.entityRepository.AddCommentRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void OpenFullArticle(RssActionModel model)
        {
            var userId = this.authentication.GetCurrentUserId();
            this.entityRepository.AddFullArticleRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void AddToReadLaterQueueForCurrentUser(RssActionModel model)
        {
            var userId = this.authentication.GetCurrentUserId();
            this.entityRepository.AddReadLaterRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void CurrentVotedownForArticleByCurrentUser(RssActionModel model)
        {
            var userId = this.authentication.GetCurrentUserId();
            this.entityRepository.AddVoteDownRequestByUserForArticle(userId, model.StreamType, model.Id);
        }
    }
}