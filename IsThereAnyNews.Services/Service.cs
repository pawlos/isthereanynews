namespace IsThereAnyNews.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Claims;
    using System.ServiceModel.Syndication;
    using System.Xml;

    using AutoMapper;

    using Exceptionless;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.DataAccess.Implementation;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Dtos.Feeds;
    using IsThereAnyNews.Infrastructure.Import.Opml;
    using IsThereAnyNews.Infrastructure.Web;
    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.ProjectionModels.Mess;
    using IsThereAnyNews.Services.Handlers;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;
    using IsThereAnyNews.ViewModels.RssChannel;
    using IsThereAnyNews.ViewModels.Subscriptions;
    using IsThereAnyNews.Web.Interfaces.Services;

    public class Service: IService
    {
        private readonly IEntityRepository entityRepository;

        private readonly IImportOpml importer;

        private readonly IInfrastructure infrastructure;

        private readonly IMapper mapper;

        private readonly ISubscriptionHandlerFactory subscriptionHandlerFactory;

        public Service(
            IEntityRepository entityRepository,
            IMapper mapper,
            ISubscriptionHandlerFactory subscriptionHandlerFactory,
            IInfrastructure infrastructure,
            IImportOpml importer)
        {
            this.entityRepository = entityRepository;
            this.mapper = mapper;
            this.subscriptionHandlerFactory = subscriptionHandlerFactory;
            this.infrastructure = infrastructure;
            this.importer = importer;
        }

        private static string[] Separator
        {
            get
            {
                var separator = new[] { ";", "," };
                return separator;
            }
        }

        public void AddCommentToRssItemByCurrentUser(RssActionModel model)
        {
            var userId = this.infrastructure.GetCurrentUserId();
            this.entityRepository.AddCommentRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void AddNewChannelsToGlobalSpace(List<RssSourceWithUrlAndTitle> channelList)
        {
            var loadUrlsForAllChannels = this.entityRepository.LoadUrlsForAllChannels();
            var channelsNewToGlobalSpace = channelList
                    .Where(channel => !loadUrlsForAllChannels.Contains(channel.Url.ToLowerInvariant()))
                    .ToList();
            var cui = this.infrastructure.GetCurrentUserId();
            this.entityRepository.SaveToDatabase(cui, channelsNewToGlobalSpace);
        }

        public void AddToReadLaterQueueForCurrentUser(RssActionModel model)
        {
            var userId = this.infrastructure.GetCurrentUserId();
            this.entityRepository.AddReadLaterRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void AssignToUserRole(ClaimsIdentity identity)
        {
            var cui = long.Parse(
                    identity.Claims.Single(x => x.Type == ItanClaimTypes.ApplicationIdentifier)
                            .Value);
            this.entityRepository.AssignUserRole(cui);
        }

        public bool CanRegisterIfWithinLimits()
        {
            return this.entityRepository.CanRegisterWithinLimits();
        }

        public void ChangeDisplayName(ChangeDisplayNameModelDto model)
        {
            var currentUserId = this.infrastructure.GetCurrentUserId();
            this.entityRepository.ChangeDisplayName(currentUserId, model.Displayname);
        }

        public void ChangeEmail(ChangeEmailModelDto model)
        {
            var currentUserId = this.infrastructure.GetCurrentUserId();
            this.entityRepository.ChangeEmail(currentUserId, model.Email);
        }

        public void ChangeRegistration(ChangeRegistrationDto dto)
        {
            this.entityRepository.ChangeApplicationRegistration(dto.Status);
        }

        public void ChangeUsersLimit(ChangeUsersLimitDto dto)
        {
            this.entityRepository.ChangeUserLimit(dto.Limit);
        }

        public void CreateNewChannelIfNotExists(AddChannelDto dto)
        {
            var idByChannelUrl = this.entityRepository.GetIdByChannelUrl(new List<string> { dto.RssChannelLink });
            if(!idByChannelUrl.Any())
                this.CreateNewChannel(dto);
        }

        public UserRssSubscriptionInfoViewModel CreateUserSubscriptionInfo(long id)
        {
            var userId = this.infrastructure.GetCurrentUserId();
            var subscriptionInfo = this.entityRepository.FindSubscriptionIdOfUserAndOfChannel(userId, id);
            var userRssSubscriptionInfoViewModel = this.mapper.Map<UserRssSubscriptionInfoViewModel>(subscriptionInfo);
            return userRssSubscriptionInfoViewModel;
        }

        public void CurrentVotedownForArticleByCurrentUser(RssActionModel model)
        {
            var userId = this.infrastructure.GetCurrentUserId();
            this.entityRepository.AddVoteDownRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void CurrentVoteupForArticleByCurrentUser(RssActionModel model)
        {
            var userId = this.infrastructure.GetCurrentUserId();
            this.entityRepository.AddVoteUpRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public AccountDetailsViewModel GetAccountDetailsViewModel()
        {
            var currentUserId = this.infrastructure.GetCurrentUserId();
            var userPrivateDetails = this.entityRepository.GetUserPrivateDetails(currentUserId);
            var accountDetailsViewModel = this.mapper.Map<AccountDetailsViewModel>(userPrivateDetails);
            return accountDetailsViewModel;
        }

        public RegistrationSupported GetCurrentRegistrationStatus()
        {
            return this.entityRepository.GetCurrentRegistrationStatus();
        }

        public FeedEntriesViewModel GetFeedEntries(FeedsGetEntries model)
        {
            var feedEntries = this.entityRepository.GetFeedEntries(model.FeedId, model.Skip, model.Take);
            var feedEntriesViewModel = this.mapper.Map<FeedEntries, FeedEntriesViewModel>(feedEntries);
            return feedEntriesViewModel;
        }

        public ContactViewModel GetViewModel()
        {
            return new ContactViewModel();
        }

        public void Import(OpmlImporterIndexDto dto)
        {
            var toRssChannelList = this.ParseToRssChannelList(dto);
            this.AddNewChannelsToGlobalSpace(toRssChannelList);
            var idByChannelUrl = this.entityRepository.GetIdByChannelUrl(
                    toRssChannelList.Select(c => c.Url)
                                    .ToList());
            var currentUserId = this.infrastructure.GetCurrentUserId();
            var channelsSubscribedByUser = this.entityRepository.GetChannelIdSubscriptionsForUser(currentUserId);
            channelsSubscribedByUser.ForEach(id => idByChannelUrl.Remove(id));
            idByChannelUrl.ForEach(c => this.entityRepository.CreateNewSubscriptionForUserAndChannel(currentUserId, c));
        }

        public bool IsUserRegistered(ClaimsIdentity identity)
        {
            var authenticationTypeProvider = this.infrastructure.GetCurrentUserLoginProvider(identity);
            var userId = this.infrastructure.GetUserSocialIdFromIdentity(identity);
            var userIsRegistered = this.entityRepository.UserIsRegistered(authenticationTypeProvider, userId);
            return userIsRegistered;
        }

        public List<SyndicationItemAdapter> Load(string url)
        {
            var reader = XmlReader.Create(url);
            var feed = SyndicationFeed.Load(reader);
            var syndicationItems = feed.Items.ToList();
            var items = this.mapper.Map<List<SyndicationItem>, List<SyndicationItemAdapter>>(syndicationItems);
            return items;
        }

        public RssChannelsMyViewModel LoadAllChannelsOfCurrentUser()
        {
            var currentUserId = this.infrastructure.GetCurrentUserId();
            var rssSubscriptions = this.entityRepository.LoadAllSubscriptionsForUser(currentUserId);
            var listOfUsers = this.LoadAllObservableSubscription(currentUserId);
            var roles = this.infrastructure.GetCurrentUserRoles();

            var viewmodel = this.mapper.Map<List<RssChannelSubscriptionDTO>, RssChannelsMyViewModel>(rssSubscriptions);

            if(roles.Contains(ItanRole.SuperAdmin))
            {
                var updates = this.entityRepository.LoadUpdateEventsCount(currentUserId);
                var creations = this.entityRepository.LoadCreateEventsCount(currentUserId);
                var exceptions = this.entityRepository.LoadExceptionEventsCount(currentUserId);

                var u = new ChannelEventUpdatesViewModel { Count = updates.Count.ToString() };
                var c = new ChannelEventCreationViewModel { Count = creations.Count.ToString() };
                var e = new ChannelEventExceptionViewModel { Count = exceptions.Count.ToString() };

                viewmodel.Creations = c;
                viewmodel.Updates = u;
                viewmodel.Exceptions = e;
            }

            viewmodel.Users = listOfUsers;
            return viewmodel;
        }

        public List<ObservableUserEventsInformation> LoadAllObservableSubscription(long userId)
        {
            var loadNameAndCountForUser = this.entityRepository.LoadNameAndCountForUser(userId);
            var list = loadNameAndCountForUser.Select(this.ProjectToObservableUserEventsInformation)
                                              .ToList();
            return list;
        }

        public ISubscriptionContentIndexViewModel LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(
            FeedsGetRead input)
        {
            var provider = this.subscriptionHandlerFactory.GetProvider(input.StreamType);
            var currentUserId = this.infrastructure.GetCurrentUserId();
            var viewmodel = provider.GetSubscriptionViewModel(currentUserId, input);
            return viewmodel;
        }

        public AllUsersPublicProfilesViewModel LoadAllUsersPublicProfile()
        {
            var publicProfiles = this.entityRepository.LoadAllUsersPublicProfileWithChannelsCount();
            var list = publicProfiles.Select(this.ProjectToViewModel)
                                     .ToList();
            var viewmodel = new AllUsersPublicProfilesViewModel { Profiles = list };
            return viewmodel;
        }

        public FeedsIndexViewModel LoadPublicFeedsNumbers()
        {
            var readNumberOfAllRssFeeds = this.entityRepository.ReadNumberOfAllRssFeeds();
            return new FeedsIndexViewModel(readNumberOfAllRssFeeds.Count);
        }

        public RssChannelsIndexViewModel LoadPublicRssFeeds(FeedsGetPublic input)
        {
            var currentUserId = this.infrastructure.GetCurrentUserId();
            var allChannels =
                    this.entityRepository.LoadAllChannelsWithStatistics(currentUserId, input.Skip, input.Take);
            var viewmodel = this.mapper.Map<RssChannelsIndexViewModel>(allChannels);
            return viewmodel;
        }

        public UserDetailedPublicProfileViewModel LoadUserPublicProfile(long id)
        {
            var cui = this.infrastructure.GetCurrentUserId();
            var isUserAlreadySubscribed = this.entityRepository.IsUserSubscribedToUser(cui, id);
            var publicProfile = this.entityRepository.LoadUserPublicProfile(id);
            var userDetailedPublicProfileViewModel =
                    this.mapper.Map<UserPublicProfileDto, UserDetailedPublicProfileViewModel>(publicProfile);
            userDetailedPublicProfileViewModel.IsUserAlreadySubscribed = isUserAlreadySubscribed;
            userDetailedPublicProfileViewModel.Events = userDetailedPublicProfileViewModel
                    .Events.OrderByDescending(e => e.Viewed)
                    .ToList();
            var loadNameAndCountForUser = this.entityRepository.LoadNameAndCountForUser(id);
            var publicProfileUsersInformations = this
                    .mapper.Map<List<NameAndCountUserSubscription>, List<PublicProfileChannelInformation>>(
                    loadNameAndCountForUser);
            userDetailedPublicProfileViewModel.Users = publicProfileUsersInformations;
            return userDetailedPublicProfileViewModel;
        }

        public void MarkAllRssReadForSubscription(MarkReadForSubscriptionDto dto)
        {
            var rssToMarkRead = dto.RssEntries.Split(Separator, StringSplitOptions.None)
                                   .Select(long.Parse)
                                   .ToList();
            this.entityRepository.MarkAllReadForUserAndSubscription(dto.SubscriptionId, rssToMarkRead);
        }

        public void MarkClicked(EntryClickedDto dto)
        {
            var subscriptionHandler = this.subscriptionHandlerFactory.GetProvider(dto.StreamType);
            var cui = this.infrastructure.GetCurrentUserId();
            subscriptionHandler.MarkClicked(cui, dto.Id, dto.SubscriptionId);
        }

        public void MarkEntriesSkipped(EntriesSkippedDto model)
        {
            var subscriptionHandler = this.subscriptionHandlerFactory.GetProvider(model.StreamType);
            var cui = this.infrastructure.GetCurrentUserId();
            subscriptionHandler.MarkSkipped(cui, model.SubscriptionId, model.Entries);
        }

        public void MarkNavigated(EntryNavigatedDto dto)
        {
            var subscriptionHandler = this.subscriptionHandlerFactory.GetProvider(dto.StreamType);
            var cui = this.infrastructure.GetCurrentUserId();
            subscriptionHandler.MarkNavigated(cui, dto.Id, dto.SubscriptionId);
        }

        public void MarkRssItemAsNotReadByCurrentUser(RssActionModel model)
        {
            var userId = this.infrastructure.GetCurrentUserId();
            this.entityRepository.AddNotReadRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void OpenFullArticle(RssActionModel model)
        {
            var userId = this.infrastructure.GetCurrentUserId();
            this.entityRepository.AddFullArticleRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public List<RssSourceWithUrlAndTitle> ParseToRssChannelList(OpmlImporterIndexDto dto)
        {
            var outlines = this.importer.GetOutlines(dto.ImportFile.InputStream);
            var ot = this.importer.FilterOutInvalidOutlines(outlines);
            var urls = this.importer.SelectUrls(ot);
            return urls;
        }

        public ItanApplicationConfigurationViewModel ReadConfiguration()
        {
            var appConfiguration = this.entityRepository.LoadApplicationConfiguration();
            var numberOfRegisteredUsers = this.entityRepository.GetNumberOfRegisteredUsers();
            var numberOfRssSubscriptions = this.entityRepository.GetNumberOfRssSubscriptions();
            var numberOfRssNews = this.entityRepository.GetNumberOfRssNews();
            var viewmodel =
                    this.mapper.Map<ApplicationConfigurationDTO, ItanApplicationConfigurationViewModel>(
                            appConfiguration);
            viewmodel.CurrentUsers = numberOfRegisteredUsers;
            viewmodel.Subscriptions = numberOfRssSubscriptions;
            viewmodel.RssNews = numberOfRssNews;
            return viewmodel;
        }

        public void RegisterIfNewUser(ClaimsIdentity identity)
        {
            var isUserRegistered = this.IsUserRegistered(identity);
            if(!isUserRegistered)
            {
                this.RegisterCurrentSocialLogin(identity);
                this.StoreCurrentUserIdInSession(identity);
                this.AssignToUserRole(identity);
            }
        }

        public void SaveAdministrationContact(ContactAdministrationDto dto)
        {
            var contactId = this.entityRepository.SaveToDatabase(dto);
            this.entityRepository.SaveContactAdministrationEventEventToDatabase(contactId);
        }

        public void ShareRssItem(RssActionModel model)
        {
            var userId = this.infrastructure.GetCurrentUserId();
            this.entityRepository.AddShareRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void StoreCurrentUserIdInSession(ClaimsIdentity identity)
        {
            if(identity.Claims.Any(x => x.Type == ItanClaimTypes.ApplicationIdentifier))
            {
                return;
            }

            var currentUserSocialLoginId = this.infrastructure.GetUserSocialIdFromIdentity(identity);
            var currentUserLoginProvider = this.infrastructure.GetCurrentUserLoginProvider(identity);
            var userId = this.entityRepository.GetUserId(currentUserSocialLoginId, currentUserLoginProvider);
            identity.AddClaim(
                    new Claim(
                            ItanClaimTypes.ApplicationIdentifier,
                            userId.ToString(),
                            ClaimValueTypes.Integer64,
                            "ITAN"));
        }

        public void StoreItanRolesToSession(ClaimsIdentity identity)
        {
            var currentUserId = long.Parse(
                    identity.Claims.Single(x => x.Type == ItanClaimTypes.ApplicationIdentifier)
                            .Value);
            var itanRoles = this.entityRepository.GetRolesTypesForUser(currentUserId);
            var claims = this.mapper.Map<List<ItanRole>, List<Claim>>(itanRoles);
            identity.AddClaims(claims);
        }

        public void SubscribeCurrentUserToChannel(FeedsPostSubscription model)
        {
            var currentUserId = this.infrastructure.GetCurrentUserId();
            var isUserSubscribedToChannelUrl =
                    this.entityRepository.IsUserSubscribedToChannelId(currentUserId, model.FeedId);
            if(!isUserSubscribedToChannelUrl)
            {
                this.entityRepository.Subscribe(model.FeedId, currentUserId);
            }
        }

        public void SubscribeCurrentUserToChannel(AddChannelDto channelId)
        {
            var currentUserId = this.infrastructure.GetCurrentUserId();
            var isUserSubscribedToChannelUrl =
                    this.entityRepository.IsUserSubscribedToChannelUrl(currentUserId, channelId.RssChannelLink);
            if(!isUserSubscribedToChannelUrl)
            {
                var idByChannelUrl = this
                        .entityRepository.GetIdByChannelUrl(new List<string> { channelId.RssChannelLink })
                        .Single();
                this.entityRepository.Subscribe(idByChannelUrl, currentUserId, channelId.RssChannelName);
            }
        }

        public void SubscribeToUser(SubscribeToUserActivityDto model)
        {
            var currentUserId = this.infrastructure.GetCurrentUserId();
            this.entityRepository.CreateNewSubscription(currentUserId, model.ViewingUserId);
        }

        public void UnsubscribeCurrentUserFromChannelId(FeedsPostSubscription model)
        {
            var userId = this.infrastructure.GetCurrentUserId();
            this.entityRepository.DeleteSubscriptionFromUser(model.FeedId, userId);
        }

        public void UnsubscribeToUser(SubscribeToUserActivityDto model)
        {
            var cui = this.infrastructure.GetCurrentUserId();
            this.entityRepository.DeleteUserSubscription(cui, model.ViewingUserId);
        }

        public void UpdateChannel(RssChannelForUpdateDTO rssChannel, DateTime lastUpdate)
        {
            var syndicationEntries = this.Load(rssChannel.Url);
            var syndicationItemAdapters = syndicationEntries.Where(item => item.PublishDate > lastUpdate);
            var rssEntriesList =
                    this.mapper.Map<IEnumerable<SyndicationItemAdapter>, List<NewRssEntryDTO>>(syndicationItemAdapters);
            rssEntriesList.ForEach(r => r.RssChannelId = rssChannel.Id);
            this.entityRepository.SaveToDatabase(rssEntriesList);
            this.entityRepository.SaveEvent(rssChannel.Id);
        }

        public void UpdateGlobalRss()
        {
            try
            {
                var rssChannel = this.entityRepository.LoadChannelToUpdate();
                var lastUpdate = this.entityRepository.GetLatestUpdateDate(rssChannel.Id);
                this.entityRepository.SaveEvent(rssChannel.Id);
                this.UpdateChannel(rssChannel, lastUpdate);
            }
            catch(Exception e)
            {
                e.ToExceptionless()
                 .Submit();
                Debug.WriteLine(e.Message);
            }
        }

        private void CreateAndAssignNewSocialLoginForApplicationUser(
            Claim identifier,
            AuthenticationTypeProvider authenticationTypeProvider,
            long newUserId)
        {
            this.entityRepository.CreateNewSocialLogin(identifier.Value, authenticationTypeProvider, newUserId);
        }

        private long CreateNewApplicationUser(ClaimsIdentity identity)
        {
            var name = identity.Claims.Single(x => x.Type == ClaimTypes.Name);
            var email = identity.Claims.Single(x => x.Type == ClaimTypes.Email);
            return this.entityRepository.CreateNewUser(name.Value, email.Value);
        }

        private void CreateNewChannel(AddChannelDto dto)
        {
            var rssSourceWithUrlAndTitles =
                    new List<RssSourceWithUrlAndTitle>
                        {
                            new RssSourceWithUrlAndTitle(
                                    dto.RssChannelLink,
                                    dto.RssChannelName)
                        };
            var cui = this.infrastructure.GetCurrentUserId();
            this.entityRepository.SaveToDatabase(cui, rssSourceWithUrlAndTitles);
            var urlsToChannels = new List<string> { dto.RssChannelLink };
            var listIds = this.entityRepository.GetIdByChannelUrl(urlsToChannels);
            var id = listIds.Single();
            this.entityRepository.SaveChannelCreatedEventToDatabase(cui, id);
        }

        private AuthenticationTypeProvider GetUserAuthenticationProviderFromAuthentication(ClaimsIdentity identity)
        {
            return this.infrastructure.GetCurrentUserLoginProvider(identity);
        }

        private ObservableUserEventsInformation ProjectToObservableUserEventsInformation(
            NameAndCountUserSubscription arg)
        {
            return new ObservableUserEventsInformation
            {
                Id = arg.SubscriptionId,
                Name = arg.DisplayName,
                Count = arg.Count.ToString()
            };
        }

        private UserPublicProfileViewModel ProjectToViewModel(UserPublicProfile model)
        {
            var viewModel = new UserPublicProfileViewModel
            {
                Id = model.Id,
                DisplayName = model.DisplayName,
                ChannelsCount = model.ChannelsCount
            };
            return viewModel;
        }

        private void RegisterCurrentSocialLogin(ClaimsIdentity identity)
        {
            var identifier = this.infrastructure.FindUserClaimNameIdentifier(identity);
            var newUserId = this.CreateNewApplicationUser(identity);
            var authenticationTypeProvider = this.GetUserAuthenticationProviderFromAuthentication(identity);
            this.CreateAndAssignNewSocialLoginForApplicationUser(identifier, authenticationTypeProvider, newUserId);
        }
    }
}