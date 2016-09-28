namespace IsThereAnyNews.Services.Implementation
{
    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;

    public class RssItemActionService : IRssItemActionService
    {
        private readonly ISessionProvider sessionProvider;
        private readonly IRssActionRepository rssItemActionRepository;

        public RssItemActionService(
            ISessionProvider sessionProvider,
            IRssActionRepository rssItemActionRepository)
        {
            this.sessionProvider = sessionProvider;
            this.rssItemActionRepository = rssItemActionRepository;
        }

        public void CurrentVoteupForArticleByCurrentUser(RssActionModel model)
        {
            var userId = this.sessionProvider.GetCurrentUserId();
            this.rssItemActionRepository.AddVoteUpRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void MarkRssItemAsNotReadByCurrentUser(RssActionModel model)
        {
            var userId = this.sessionProvider.GetCurrentUserId();
            this.rssItemActionRepository.AddNotReadRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void ShareRssItem(RssActionModel model)
        {
            var userId = this.sessionProvider.GetCurrentUserId();
            this.rssItemActionRepository.AddShareRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void AddCommentToRssItemByCurrentUser(RssActionModel model)
        {
            var userId = this.sessionProvider.GetCurrentUserId();
            this.rssItemActionRepository.AddCommentRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void OpenFullArticle(RssActionModel model)
        {
            var userId = this.sessionProvider.GetCurrentUserId();
            this.rssItemActionRepository.AddFullArticleRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void AddToReadLaterQueueForCurrentUser(RssActionModel model)
        {
            var userId = this.sessionProvider.GetCurrentUserId();
            this.rssItemActionRepository.AddReadLaterRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void CurrentVotedownForArticleByCurrentUser(RssActionModel model)
        {
            var userId = this.sessionProvider.GetCurrentUserId();
            this.rssItemActionRepository.AddVoteDownRequestByUserForArticle(userId, model.StreamType, model.Id);
        }
    }
}