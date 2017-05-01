angular
    .module("itan")
    .factory("feedsService", ['feedsApi',function (feedsApi) {
        return {
            onChannelClick: function ($scope, channel, callback) {
                feedsApi.loadFeedChannel(channel.Id, function(data){
                        $scope.channel.loaded = true;
                        $scope.channel.entries = data;
                        $scope.channels.current = channel;
                        callback();
                    });
            },
            loadFeeds: function ($scope) {
                feedsApi.loadFeeds(function(data){
                        $scope.channels.list = data;
                        $scope.channels.loaded = true;
                    });
            },
            isCurrent: function ($scope, channel) {
                var x = channel === $scope.channels.current;
                if (x) {
                    return "btn-info";
                }
                return "";
            },
            buttonSubscriptionClass: function (channel) {
                return channel.entries.SubscriptionInfo.IsSubscribed ? "btn-danger" : "btn-primary";
            },
            onSubscribeClick: function ($scope, channelId, isSubscribed) {
                if (isSubscribed) {
                    this.unsubscribe(channelId);
                } else {
                    this.subscribe(channelId);
                }
            },
            subscribe: function ($scope, channelId) {
                feedsApi.subscribe(channelId,function(){
                        $scope.updateSubscriptionStatus(channelId, true);
                    });
            },
            unsubscribe: function ($scope, channelId) {
                feedsApi.unsubscribe(channelId, function(){
                        $scope.updateSubscriptionStatus(channelId, false);
                    });
            },
            updateSubscriptionStatus: function ($scope, channelId, newstatus) {
                if ($scope.channel.entries.ChannelId === channelId) {
                    $scope.channel.entries.SubscriptionInfo.IsSubscribed = newstatus;
                }
            }
        };
    }]);
