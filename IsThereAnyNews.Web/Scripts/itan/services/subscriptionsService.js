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
            }
        };
    }]);
