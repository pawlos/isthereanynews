namespace IsThereAnyNews.Services.Implementation
{
    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;

    public class RssItemActionService : IRssItemActionService
    {
        private readonly IRssActionRepository rssItemActionRepository;

        private readonly IUserAuthentication authentication;

        public RssItemActionService(IRssActionRepository rssItemActionRepository,
            IUserAuthentication authentication)
        {
            this.rssItemActionRepository = rssItemActionRepository;
            this.authentication = authentication;
        }

        public void CurrentVoteupForArticleByCurrentUser(RssActionModel model)
        {
            var userId = this.authentication.GetCurrentUserId();
            this.rssItemActionRepository.AddVoteUpRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void MarkRssItemAsNotReadByCurrentUser(RssActionModel model)
        {
            var userId = this.authentication.GetCurrentUserId();
            this.rssItemActionRepository.AddNotReadRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void ShareRssItem(RssActionModel model)
        {
            var userId = this.authentication.GetCurrentUserId();
            this.rssItemActionRepository.AddShareRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void AddCommentToRssItemByCurrentUser(RssActionModel model)
        {
            var userId = this.authentication.GetCurrentUserId();
            this.rssItemActionRepository.AddCommentRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void OpenFullArticle(RssActionModel model)
        {
            var userId = this.authentication.GetCurrentUserId();
            this.rssItemActionRepository.AddFullArticleRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void AddToReadLaterQueueForCurrentUser(RssActionModel model)
        {
            var userId = this.authentication.GetCurrentUserId();
            this.rssItemActionRepository.AddReadLaterRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void CurrentVotedownForArticleByCurrentUser(RssActionModel model)
        {
            var userId = this.authentication.GetCurrentUserId();
            this.rssItemActionRepository.AddVoteDownRequestByUserForArticle(userId, model.StreamType, model.Id);
        }
    }
}