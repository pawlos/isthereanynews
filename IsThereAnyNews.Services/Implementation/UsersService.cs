using System;
using System.Collections.Generic;
using System.Linq;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.DataAccess.Implementation;

namespace IsThereAnyNews.Services.Implementation
{
    public class UsersService : IUsersService
    {
        private readonly IUserRepository usersRepository;

        public UsersService(IUserRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public AllUsersPublicProfilesViewModel LoadAllUsersPublicProfile()
        {
            var publicProfiles = this.usersRepository.LoadAllUsersPublicProfileWithChannelsCount();
            var list = publicProfiles.Select(ProjectToViewModel).ToList();
            var viewmodel = new AllUsersPublicProfilesViewModel
            {
                Profiles = list
            };
            return viewmodel;
        }

        public UserDetailedPublicProfileViewModel LoadUserPublicProfile(long id)
        {
            var publicProfile = this.usersRepository.LoadUserPublicProfile(id);
            var userDetailedPublicProfileViewModel = new UserDetailedPublicProfileViewModel
            {
                DisplayName = publicProfile.DisplayName,
                ChannelsCount = publicProfile.RssSubscriptionList.Count,
                Channels = publicProfile.RssSubscriptionList.Distinct().Select(channelSubscription => new PublicProfileChannelInformation
                {
                    Id = channelSubscription.Id,
                    Name = channelSubscription.Title,
                }).ToList(),
                Events = publicProfile.EventsRssViewed.Select(e => new EventRssViewedViewModel
                {
                    Title = e.RssEntry.Title,
                    Viewed = e.Created,
                    RssId = e.RssEntryId
                }).ToList(),
                EventsCount = publicProfile.EventsRssViewed.Count(),
                ViewingUserId = id
            };
            return userDetailedPublicProfileViewModel;
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
    }

    public class EventRssViewedViewModel
    {
        public string Title { get; set; }
        public DateTime Viewed { get; set; }
        public long RssId { get; set; }
    }

    public class PublicProfileChannelInformation
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class UserDetailedPublicProfileViewModel
    {
        public string DisplayName { get; set; }
        public int ChannelsCount { get; set; }
        public List<PublicProfileChannelInformation> Channels { get; set; }
        public List<EventRssViewedViewModel> Events { get; set; }
        public int EventsCount { get; set; }
        public long ViewingUserId { get; set; }
    }

    public class AllUsersPublicProfilesViewModel
    {
        public List<UserPublicProfileViewModel> Profiles { get; set; }
    }

    public class UserPublicProfileViewModel
    {
        public long Id { get; set; }
        public string DisplayName { get; set; }
        public int ChannelsCount { get; set; }
    }
}