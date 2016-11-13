﻿namespace IsThereAnyNews.Services
{
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.ViewModels;

    public interface IRssChannelsService
    {
        RssChannelsIndexViewModel LoadAllChannels();
        RssChannelsMyViewModel LoadAllChannelsOfCurrentUser();
        RssChannelIndexViewModel GetViewModelFormChannelId(long id);
        void CreateNewChannelIfNotExists(AddChannelDto dto);
    }
}