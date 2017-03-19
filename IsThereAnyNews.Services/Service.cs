namespace IsThereAnyNews.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Security.Claims;
    using System.ServiceModel.Syndication;
    using System.Web;
    using System.Xml;

    using AutoMapper;

    using Exceptionless;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.ProjectionModels.Mess;
    using IsThereAnyNews.Services.Handlers;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;
    using IsThereAnyNews.ViewModels.RssChannel;
    using IsThereAnyNews.Web.Interfaces.Services;

    public class Service : IService
    {
        private readonly IEntityRepository entityRepository;

        private readonly IMapper mapper;

        private readonly ISubscriptionHandlerFactory subscriptionHandlerFactory;

        public Service(IEntityRepository entityRepository, IMapper mapper, ISubscriptionHandlerFactory subscriptionHandlerFactory)
        {
            this.entityRepository = entityRepository;
            this.mapper = mapper;
            this.subscriptionHandlerFactory = subscriptionHandlerFactory;
        }

        public void AddCommentToRssItemByCurrentUser(RssActionModel model)
        {
            var userId = this.GetCurrentUserId();
            this.entityRepository.AddCommentRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void AddNewChannelsToGlobalSpace(List<RssSourceWithUrlAndTitle> channelList)
        {
            var loadUrlsForAllChannels = this.entityRepository.LoadUrlsForAllChannels();
            var channelsNewToGlobalSpace = channelList.Where(channel => !loadUrlsForAllChannels.Contains(channel.Url.ToLowerInvariant()))
                .ToList();
            this.entityRepository.SaveToDatabase(channelsNewToGlobalSpace);
        }

        public void AddToReadLaterQueueForCurrentUser(RssActionModel model)
        {
            var userId = this.GetCurrentUserId();
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
            var currentUserId = this.GetCurrentUserId();
            this.entityRepository.ChangeDisplayName(currentUserId, model.Displayname);
        }

        public void ChangeEmail(ChangeEmailModelDto model)
        {
            var currentUserId = this.GetCurrentUserId();
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
            if (!idByChannelUrl.Any())
            {
                this.CreateNewChannel(dto);
            }
        }

        public UserRssSubscriptionInfoViewModel CreateUserSubscriptionInfo(long id)
        {
            var userId = this.GetCurrentUserId();
            var subscriptionInfo = this.entityRepository.FindSubscriptionIdOfUserAndOfChannel(userId, id);
            var userRssSubscriptionInfoViewModel = this.mapper.Map<UserRssSubscriptionInfoViewModel>(subscriptionInfo);
            return userRssSubscriptionInfoViewModel;
        }

        public bool CurrentUserIsAuthenticated()
        {
            return HttpContext.Current.GetOwinContext()
                .Authentication.User.Identity.IsAuthenticated;
        }

        public void CurrentVotedownForArticleByCurrentUser(RssActionModel model)
        {
            var userId = this.GetCurrentUserId();
            this.entityRepository.AddVoteDownRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void CurrentVoteupForArticleByCurrentUser(RssActionModel model)
        {
            var userId = this.GetCurrentUserId();
            this.entityRepository.AddVoteUpRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public List<XmlNode> FilterOutInvalidOutlines(IEnumerable<XmlNode> outlines)
        {
            var validoutlines = outlines.Where(
                o => o.Attributes.GetNamedItem("xmlUrl") != null
                  && o.Attributes.GetNamedItem("title") != null
                  && !string.IsNullOrWhiteSpace(o.Attributes.GetNamedItem("xmlUrl").Value)
                  && !string.IsNullOrWhiteSpace(o.Attributes.GetNamedItem("title").Value));
            return validoutlines.ToList();
        }

        public AccountDetailsViewModel GetAccountDetailsViewModel()
        {
            var currentUserId = this.GetCurrentUserId();
            var userPrivateDetails = this.entityRepository.GetUserPrivateDetails(currentUserId);
            var accountDetailsViewModel = this.mapper.Map<AccountDetailsViewModel>(userPrivateDetails);
            return accountDetailsViewModel;
        }

        public RegistrationSupported GetCurrentRegistrationStatus()
        {
            return this.entityRepository.GetCurrentRegistrationStatus();
        }

        public ClaimsPrincipal GetCurrentUser()
        {
            return HttpContext.Current.GetOwinContext()
                .Authentication.User;
        }

        public long GetCurrentUserId()
        {
            long r;
            long.TryParse(
                this.GetCurrentUser()
                    .Claims.SingleOrDefault(claim => claim.Type == ItanClaimTypes.ApplicationIdentifier)
                    ?.Value,
                out r);
            return r;
        }

        public AuthenticationTypeProvider GetCurrentUserLoginProvider(ClaimsIdentity identity)
        {
            var issuer = identity.Claims.First(claim => !string.IsNullOrWhiteSpace(claim.Issuer))
                .Issuer;
            AuthenticationTypeProvider enumResult;
            Enum.TryParse(issuer, true, out enumResult);
            return enumResult;
        }

        public List<ItanRole> GetCurrentUserRoles()
        {
            var roles = this.GetCurrentUser()
                .Claims.Where(c => c.Type == ClaimTypes.Role)
                .Select(c => (ItanRole)Enum.Parse(typeof(ItanRole), c.Value))
                .ToList();
            return roles;
        }

        public IEnumerable<XmlNode> GetOutlines(Stream inputStream)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(inputStream);
            var outlines = xmlDocument.GetElementsByTagName("outline");
            return outlines.Cast<XmlNode>();
        }

        public string GetUserSocialIdFromIdentity(ClaimsIdentity identity)
        {
            var claim = identity.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
            return claim.Value;
        }

        public ContactViewModel GetViewModel()
        {
            return new ContactViewModel();
        }

        public RssChannelIndexViewModel GetViewModelFormChannelId(long id)
        {
            var rssChannel = this.entityRepository.LoadRssChannel(id);
            var rssChannelIndexViewModel = this.mapper.Map<RssChannelDTO, RssChannelIndexViewModel>(
                rssChannel,
                o => o.AfterMap(
                    (s, d) =>
                        {
                            d.Entries = d.Entries.OrderByDescending(item => item.PublicationDate)
                                .ToList();
                        }));
            if (!this.CurrentUserIsAuthenticated())
            {
                return rssChannelIndexViewModel;
            }

            rssChannelIndexViewModel.IsAuthenticatedUser = true;
            var userRssSubscriptionInfoViewModel = this.CreateUserSubscriptionInfo(id);
            rssChannelIndexViewModel.SubscriptionInfo = userRssSubscriptionInfoViewModel;
            return rssChannelIndexViewModel;
        }

        public void Import(OpmlImporterIndexDto dto)
        {
            var toRssChannelList = this.ParseToRssChannelList(dto);
            this.AddNewChannelsToGlobalSpace(toRssChannelList);
            var idByChannelUrl = this.entityRepository.GetIdByChannelUrl(
                toRssChannelList.Select(c => c.Url)
                    .ToList());
            var currentUserId = this.GetCurrentUserId();
            var channelsSubscribedByUser = this.entityRepository.GetChannelIdSubscriptionsForUser(currentUserId);
            channelsSubscribedByUser.ForEach(id => idByChannelUrl.Remove(id));
            idByChannelUrl.ForEach(c => this.entityRepository.CreateNewSubscriptionForUserAndChannel(currentUserId, c));
        }

        public bool IsUserRegistered(ClaimsIdentity identity)
        {
            var authenticationTypeProvider = this.GetCurrentUserLoginProvider(identity);
            var userId = this.GetUserSocialIdFromIdentity(identity);
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

        public List<ChannelEventViewModel> LoadAdminEvents()
        {
            return new List<ChannelEventViewModel>();
        }

        public RssChannelsIndexViewModel LoadAllChannels()
        {
            var allChannels = this.entityRepository.LoadAllChannelsWithStatistics();
            var viewmodel = this.mapper.Map<RssChannelsIndexViewModel>(allChannels);
            return viewmodel;
        }

        public RssChannelsMyViewModel LoadAllChannelsOfCurrentUser()
        {
            var currentUserId = this.GetCurrentUserId();
            var rssSubscriptions = this.entityRepository.LoadAllSubscriptionsForUser(currentUserId);

            // this.rssEntriesToReadRepository.CopyRssThatWerePublishedAfterLastReadTimeToUser(currentUserId, rssSubscriptions);
            var viewmodel = this.mapper.Map<List<RssChannelSubscriptionDTO>, RssChannelsMyViewModel>(rssSubscriptions);
            return viewmodel;
        }

        public List<ObservableUserEventsInformation> LoadAllObservableSubscription()
        {
            var now = DateTime.Now;
            var currentUserId = this.GetCurrentUserId();
            this.entityRepository.CopyAllUnreadElementsToUser(currentUserId);
            var loadNameAndCountForUser = this.entityRepository.LoadNameAndCountForUser(currentUserId);
            this.entityRepository.UpdateUserLastReadTime(currentUserId, now);
            var list = loadNameAndCountForUser.Select(this.ProjectToObservableUserEventsInformation)
                .ToList();
            return list;
        }

        public RssSubscriptionIndexViewModel LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(StreamType streamType, long subscriptionId, ShowReadEntries showReadEntries)
        {
            var provider = this.subscriptionHandlerFactory.GetProvider(streamType);
            var viewmodel = provider.GetSubscriptionViewModel(666666, subscriptionId, showReadEntries);
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

        public AdminEventsViewModel LoadEvents()
        {
            var roles = this.GetCurrentUserRoles();
            if (roles.Contains(ItanRole.SuperAdmin))
            {
                return this.LoadSuperAdminEvents();
            }

            return new AdminEventsViewModel();
        }

        public AdminEventsViewModel LoadSuperAdminEvents()
        {
            var updates = this.entityRepository.LoadUpdateEvents();
            var creations = this.entityRepository.LoadCreateEvents();
            var exceptions = this.entityRepository.LoadExceptionEvents();
            ChannelEventViewModel u = new ChannelEventUpdatesViewModel { Count = updates.UpdateCout.ToString(), Name = "Updates", Id = -1 };
            var c = new ChannelEventCreationViewModel { Count = creations.Count.ToString(), Name = "Creations", Id = -2 };
            var e = new ChannelEventExceptionViewModel { Count = exceptions.Count.ToString(), Name = "Exceptions", Id = -3 };
            var events = new AdminEventsViewModel { Updates = u, Creations = c, Exceptions = e };
            return events;
        }

        public UserDetailedPublicProfileViewModel LoadUserPublicProfile(long id)
        {
            var cui = this.GetCurrentUserId();
            var isUserAlreadySubscribed = this.entityRepository.IsUserSubscribedToUser(cui, id);
            var publicProfile = this.entityRepository.LoadUserPublicProfile(id);
            var userDetailedPublicProfileViewModel = this.mapper.Map<UserPublicProfileDto, UserDetailedPublicProfileViewModel>(publicProfile);
            userDetailedPublicProfileViewModel.IsUserAlreadySubscribed = isUserAlreadySubscribed;
            userDetailedPublicProfileViewModel.Events = userDetailedPublicProfileViewModel.Events.OrderByDescending(e => e.Viewed)
                .ToList();
            var loadNameAndCountForUser = this.entityRepository.LoadNameAndCountForUser(id);
            var publicProfileUsersInformations = this.mapper.Map<List<NameAndCountUserSubscription>, List<PublicProfileChannelInformation>>(loadNameAndCountForUser);
            userDetailedPublicProfileViewModel.Users = publicProfileUsersInformations;
            return userDetailedPublicProfileViewModel;
        }

        public void MarkAllRssReadForSubscription(MarkReadForSubscriptionDto dto)
        {
            var separator = new[]
                                {
                                    ";"
                                };
            var rssToMarkRead = dto.RssEntries.Split(separator, StringSplitOptions.None)
                .Select(long.Parse)
                .ToList();
            this.entityRepository.MarkAllReadForUserAndSubscription(dto.SubscriptionId, rssToMarkRead);
        }

        public void MarkClicked(MarkClickedDto dto)
        {
            var currentUserId = this.GetCurrentUserId();
            this.entityRepository.MarkClicked(dto.Id, currentUserId);
        }

        public void MarkEntriesRead(MarkReadDto dto)
        {
            var subscriptionHandler = this.subscriptionHandlerFactory.GetProvider(dto.StreamType);

            // var toberead = RssToMarkRead(dto.DisplayedItems);
            // subscriptionHandler.MarkRead(toberead);
        }

        public void MarkEntriesSkipped(MarkSkippedDto model)
        {
            var subscriptionHandler = this.subscriptionHandlerFactory.GetProvider(model.StreamType);
            var cui = this.GetCurrentUserId();
            var ids = RssToMarkRead(model.Entries);
            subscriptionHandler.MarkSkipped(model.SubscriptionId, ids);
            subscriptionHandler.AddEventSkipped(cui, model.Entries);
        }

        public void MarkRead(MarkReadDto dto)
        {
            var subscriptionHandler = this.subscriptionHandlerFactory.GetProvider(dto.StreamType);
            var cui = this.GetCurrentUserId();
            subscriptionHandler.MarkRead(cui, dto.Id, dto.SubscriptionId);
            subscriptionHandler.AddEventViewed(cui, dto.Id);
        }

        public void MarkRssItemAsNotReadByCurrentUser(RssActionModel model)
        {
            var userId = this.GetCurrentUserId();
            this.entityRepository.AddNotReadRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void OpenFullArticle(RssActionModel model)
        {
            var userId = this.GetCurrentUserId();
            this.entityRepository.AddFullArticleRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public List<RssSourceWithUrlAndTitle> ParseToRssChannelList(OpmlImporterIndexDto dto)
        {
            var outlines = this.GetOutlines(dto.ImportFile.InputStream);
            var ot = this.FilterOutInvalidOutlines(outlines);
            var urls = ot.Select(
                    o => new RssSourceWithUrlAndTitle(
                        o.Attributes.GetNamedItem("xmlUrl")
                            .Value,
                        o.Attributes.GetNamedItem("title")
                            .Value))
                .Distinct(new RssSourceWithUrlAndTitleComparer())
                .ToList();
            return urls;
        }

        public ItanApplicationConfigurationViewModel ReadConfiguration()
        {
            var appConfiguration = this.entityRepository.LoadApplicationConfiguration();
            var numberOfRegisteredUsers = this.entityRepository.GetNumberOfRegisteredUsers();
            var numberOfRssSubscriptions = this.entityRepository.GetNumberOfRssSubscriptions();
            var numberOfRssNews = this.entityRepository.GetNumberOfRssNews();
            var viewmodel = this.mapper.Map<ApplicationConfigurationDTO, ItanApplicationConfigurationViewModel>(appConfiguration);
            viewmodel.CurrentUsers = numberOfRegisteredUsers;
            viewmodel.Subscriptions = numberOfRssSubscriptions;
            viewmodel.RssNews = numberOfRssNews;
            return viewmodel;
        }

        public void RegisterIfNewUser(ClaimsIdentity identity)
        {
            var isUserRegistered = this.IsUserRegistered(identity);
            if (!isUserRegistered)
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
            var userId = this.GetCurrentUserId();
            this.entityRepository.AddShareRequestByUserForArticle(userId, model.StreamType, model.Id);
        }

        public void StoreCurrentUserIdInSession(ClaimsIdentity identity)
        {
            if (identity.Claims.Any(x => x.Type == ItanClaimTypes.ApplicationIdentifier))
            {
                return;
            }

            var currentUserSocialLoginId = this.GetUserSocialIdFromIdentity(identity);
            var currentUserLoginProvider = this.GetCurrentUserLoginProvider(identity);
            var userId = this.entityRepository.GetUserId(currentUserSocialLoginId, currentUserLoginProvider);
            identity.AddClaim(new Claim(ItanClaimTypes.ApplicationIdentifier, userId.ToString(), ClaimValueTypes.Integer64, "ITAN"));
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

        public void SubscribeCurrentUserToChannel(long channelId)
        {
            var currentUserId = this.GetCurrentUserId();
            var isUserSubscribedToChannelUrl = this.entityRepository.IsUserSubscribedToChannelId(currentUserId, channelId);
            if (!isUserSubscribedToChannelUrl)
            {
                this.entityRepository.Subscribe(channelId, currentUserId);
            }
        }

        public void SubscribeCurrentUserToChannel(AddChannelDto channelId)
        {
            var currentUserId = this.GetCurrentUserId();
            var isUserSubscribedToChannelUrl = this.entityRepository.IsUserSubscribedToChannelUrl(currentUserId, channelId.RssChannelLink);
            if (!isUserSubscribedToChannelUrl)
            {
                var idByChannelUrl = this.entityRepository.GetIdByChannelUrl(new List<string> { channelId.RssChannelLink })
                    .Single();
                this.entityRepository.Subscribe(idByChannelUrl, currentUserId, channelId.RssChannelName);
            }
        }

        public void SubscribeToUser(SubscribeToUserActivityDto model)
        {
            var currentUserId = this.GetCurrentUserId();
            this.entityRepository.CreateNewSubscription(currentUserId, model.ViewingUserId);
        }

        public void UnsubscribeCurrentUserFromChannelId(long id)
        {
            var userId = this.GetCurrentUserId();
            this.entityRepository.DeleteSubscriptionFromUser(id, userId);
        }

        public void UnsubscribeToUser(SubscribeToUserActivityDto model)
        {
            var cui = this.GetCurrentUserId();
            this.entityRepository.DeleteUserSubscription(cui, model.ViewingUserId);
        }

        public void UpdateChannel(RssChannelForUpdateDTO rssChannel, DateTime lastUpdate)
        {
            var syndicationEntries = this.Load(rssChannel.Url);
            var syndicationItemAdapters = syndicationEntries.Where(item => item.PublishDate > lastUpdate);
            var rssEntriesList = this.mapper.Map<IEnumerable<SyndicationItemAdapter>, List<NewRssEntryDTO>>(syndicationItemAdapters);
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
            catch (Exception e)
            {
                e.ToExceptionless()
                    .Submit();
                Debug.WriteLine(e.Message);
            }
        }

        private static List<long> RssToMarkRead(string model)
        {
            var separator = new[]
                                {
                                    ";",
                                    ","
                                };
            var rssToMarkRead = model.Split(separator, StringSplitOptions.None)
                .Select(long.Parse)
                .ToList();
            return rssToMarkRead;
        }

        private void CreateAndAssignNewSocialLoginForApplicationUser(Claim identifier, AuthenticationTypeProvider authenticationTypeProvider, long newUserId)
        {
            this.entityRepository.CreateNewSociaLogin(identifier.Value, authenticationTypeProvider, newUserId);
        }

        private long CreateNewApplicationUser(ClaimsIdentity identity)
        {
            var name = identity.Claims.Single(x => x.Type == ClaimTypes.Name);
            var email = identity.Claims.Single(x => x.Type == ClaimTypes.Email);
            return this.entityRepository.CreateNewUser(name.Value, email.Value);
        }

        private void CreateNewChannel(AddChannelDto dto)
        {
            var rssSourceWithUrlAndTitles = new List<RssSourceWithUrlAndTitle> { new RssSourceWithUrlAndTitle(dto.RssChannelLink, dto.RssChannelName) };
            this.entityRepository.SaveToDatabase(rssSourceWithUrlAndTitles);
            var urlsToChannels = new List<string> { dto.RssChannelLink };
            var listIds = this.entityRepository.GetIdByChannelUrl(urlsToChannels);
            var id = listIds.Single();
            this.entityRepository.SaveChannelCreatedEventToDatabase(id);
        }

        private Claim FindUserClaimNameIdentifier(ClaimsIdentity identity)
        {
            var identifier = identity.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
            return identifier;
        }

        private AuthenticationTypeProvider GetUserAuthenticationProviderFromAuthentication(ClaimsIdentity identity)
        {
            return this.GetCurrentUserLoginProvider(identity);
        }

        private ObservableUserEventsInformation ProjectToObservableUserEventsInformation(NameAndCountUserSubscription arg)
        {
            return new ObservableUserEventsInformation { Id = arg.Id, Name = arg.Name, Count = arg.Count.ToString() };
        }

        private UserPublicProfileViewModel ProjectToViewModel(UserPublicProfile model)
        {
            var viewModel = new UserPublicProfileViewModel { Id = model.Id, DisplayName = model.DisplayName, ChannelsCount = model.ChannelsCount };
            return viewModel;
        }

        private void RegisterCurrentSocialLogin(ClaimsIdentity identity)
        {
            var identifier = this.FindUserClaimNameIdentifier(identity);
            var newUserId = this.CreateNewApplicationUser(identity);
            var authenticationTypeProvider = this.GetUserAuthenticationProviderFromAuthentication(identity);
            this.CreateAndAssignNewSocialLoginForApplicationUser(identifier, authenticationTypeProvider, newUserId);
        }
    }
}