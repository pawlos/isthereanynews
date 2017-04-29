angular
    .module("itan")
    .factory("feedsService", [function () {
        return {
            onChannelClick: function ($scope, $http, channel) {
                $http.get("/Feeds/Feed/" + channel.Id)
                    .success(function (data) {
                        $scope.channel.loaded = true;
                        $scope.channel.entries = data;
                        $scope.channels.current = channel;
                    })
                    .then(function () {
                        $scope.setHeights();
                    });
            },

            loadFeeds: function ($scope, $http) {
                $http.get("/Feeds/Public")
                    .success(function (data) {
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

            onSubscribeClick: function ($scope, $http, channelId, isSubscribed) {
                if (isSubscribed) {
                    this.unsubscribe(channelId);
                } else {
                    this.subscribe(channelId);
                }
            },

            subscribe: function ($scope, $http, channelId) {
                var httpOptions = {
                    method: 'POST',
                    url: "/Feeds/SubscribeToChannel",
                    data: {
                        channelId: channelId
                    }
                };
                $http(httpOptions)
                    .success(function () {
                        $scope.updateSubscriptionStatus(channelId, true);
                    });
            },

            unsubscribe: function ($scope, $http, channelId) {
                var httpOptions = {
                    method: 'POST',
                    url: "/Feeds/UnsubscribeFromChannel",
                    data: {
                        channelId: channelId
                    }
                };
                $http(httpOptions)
                    .success(function () {
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
