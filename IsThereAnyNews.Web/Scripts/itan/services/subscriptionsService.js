angular
    .module("itan")
    .factory('subscriptionsService', ['subscriptionsApi', function (subscriptionsApi) {
        return {
            loadSubscriptions: function (model) {
                subscriptionsApi.loadSubscriptions(function (data) {
                    model.channels.list = data;
                    model.channels.loaded = true;
                });
            },
            onChannelClick: function (model, channel, callback) {
                subscriptionsApi.onChannelClick(channel.streamType, channel.id, function (data) {
                    model.channels.current = channel;
                    model.channel.loaded = true;
                    model.channel.entries = data;
                    callback();
                });
            },
            isCurrent: function (model, channel) {
                return model.channels.current === channel;
            },
            loadMoreEntries:function(model){
                subscriptionsApi.loadEntries(model.channels.current.streamType, model.channels.current.id, model.channel.entries.rssEntryToReadViewModels.length, function(data){
                    model.channel.entries.rssEntryToReadViewModels = model.channel.entries.rssEntryToReadViewModels.concat(data.rssEntryToReadViewModels);
                });
            }
        };
    }]);
