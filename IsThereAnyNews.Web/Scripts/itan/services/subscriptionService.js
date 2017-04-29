angular
    .module("itan")
    .factory('subscriptionService', [function () {
        return {
            loadSubscriptions: function ($scope, $http) {
                $http.get("/Subscriptions/MyChannelList")
                    .success(function (data) {
                        $scope.channels.list = data;
                        $scope.channels.loaded = true;
                    });
            },
            onChannelClick: function ($scope, $http,channel) {
                $scope.channels.current = channel;
                $http.get("/Feeds/ReadAjax?streamType=" + channel.StreamType + "&id=" + channel.Id)
                    .success(function (data) {
                        $scope.channel.loaded = true;
                        $scope.channel.entries = data;
                        $(".nocss-rss-item-list")
                            .collapse({
                                toggle: false
                            });
                    });
            },
            isCurrent:function ($scope,channel) {
                return channel === $scope.channels.current;
            }
        };
    }]);
