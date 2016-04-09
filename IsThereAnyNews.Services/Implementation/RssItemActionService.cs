using IsThereAnyNews.DataAccess;

namespace IsThereAnyNews.Services.Implementation
{
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

        public void CurrentVoteupForArticleByCurrentUser(long id)
        {
            var userId = this.sessionProvider.GetCurrentUserId();
            this.rssItemActionRepository.AddVoteUpRequestByUserForArticle(userId, id);
        }

        public void MarkRssItemAsNotReadByCurrentUser(long id)
        {
            var userId = this.sessionProvider.GetCurrentUserId();
            this.rssItemActionRepository.AddNotReadRequestByUserForArticle(userId, id);
        }

        public void ShareRssItem(long id)
        {
            var userId = this.sessionProvider.GetCurrentUserId();
            this.rssItemActionRepository.AddShareRequestByUserForArticle(userId, id);
        }

        public void AddCommentToRssItemByCurrentUser(long id)
        {
            var userId = this.sessionProvider.GetCurrentUserId();
            this.rssItemActionRepository.AddCommentRequestByUserForArticle(userId, id);
        }

        public void OpenFullArticle(long id)
        {
            var userId = this.sessionProvider.GetCurrentUserId();
            this.rssItemActionRepository.AddFullArticleRequestByUserForArticle(userId, id);
        }

        public void AddToReadLaterQueueForCurrentUser(long id)
        {
            var userId = this.sessionProvider.GetCurrentUserId();
            this.rssItemActionRepository.AddReadLaterRequestByUserForArticle(userId, id);
        }

        public void CurrentVotedownForArticleByCurrentUser(long id)
        {
            var userId = this.sessionProvider.GetCurrentUserId();
            this.rssItemActionRepository.AddVoteDownRequestByUserForArticle(userId, id);
        }
    }
}