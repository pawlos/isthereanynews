angular.module("itan")
.controller("itan.PublicRssChannelsCtrl", function ($scope, $http) {
    $scope.channels = {
        loaded: false
    };

    $scope.channel = {
        loaded: false
    };

    $http.get("/RssChannel/PublicChannels")
        .success(function (data) {
            $scope.channels.list = data;
            $scope.channels.loaded = true;
        });

    $scope.onChannelClick = function (channelId) {
        $http.get("/RssChannel/Public/" + channelId)
            .success(function (data) {
                $scope.channel.loaded = true;
                $scope.channel.entries = data;
            });
    };

    $scope.buttonSubscriptionClass = function (channel) {
        return channel.entries.SubscriptionInfo.IsSubscribed ? "btn-danger" : "btn-primary";
    }

    $scope.onSubscribeClick = function (channelId, isSubscribed) {
        if (isSubscribed) {
            this.unsubscribe(channelId);
        } else {
            this.subscribe(channelId);
        }
    };

    $scope.subscribe = function (channelId) {
        var httpOptions = {
            method: 'POST',
            url: "/RssChannel/Subscribe",
            data: {
                channelId: channelId
            }
        };
        $http(httpOptions)
            .success(function () {
                $scope.updateSubscriptionStatus(channelId, true);
            });
    };

    $scope.unsubscribe = function (channelId) {
        var httpOptions = {
            method: 'POST',
            url: "/RssChannel/Unsubscribe",
            data: {
                channelId: channelId
            }
        };
        $http(httpOptions)
            .success(function () {
                $scope.updateSubscriptionStatus(channelId, false);
            });
    };

    $scope.updateSubscriptionStatus = function (channelId, newstatus) {
        if ($scope.channel.entries.ChannelId === channelId) {
            $scope.channel.entries.SubscriptionInfo.IsSubscribed = newstatus;
        }
    }
});

