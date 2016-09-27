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
                $(".nocss-rss-item-list").collapsible();
            });
    };

    $scope.onSubscribeClick = function (channelId, isSubscribed) {
        if (isSubscribed) {
            unsubscribe(channelId);
        } else {
            subscribe(channelId);
        }
    };

        var subscribe = function(channelId) {
            var httpOptions = {
                method: 'POST',
                url: "/RssChannel/Subscribe",
                data: {
                    channelId: channelId
                }
            };
            $http(httpOptions)
                .success(function() {
                    console.log("subscribed");
                });
        };

        var unsubscribe = function (channelId) {
            var httpOptions = {
                method: 'POST',
                url: "/RssChannel/Unsubscribe",
                data: {
                    channelId: channelId
                }
            };
            $http(httpOptions)
                .success(function () {
                    console.log("unsubscribed");
                });
        };

    });

