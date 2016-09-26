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
        $http.get("/RssChannel/Public/"+channelId)
            .success(function (data) {
                $scope.channel.loaded = true;
                $scope.channel.entries = data;
                $(".nocss-rss-item-list").collapsible();
            });
    };
});

