angular.module("itan")
.controller("itan.PublicRssChannelsCtrl", function ($scope, $http) {
    $scope.channels = {
        loaded: false,
        list: {},
        current: {}
    };

    $scope.channel = {
        loaded: false,
        entries: {
            SubscriptionInfo: {
                IsSubscribed: ''
            }
        }
    };

    $scope.maxHeight = function () {
        var h = document.documentElement.clientHeight -
            $(".navbar-fixed-top").height() -
            $(".navbar-fixed-bottom").height() -
            20; // missing px somewhere :)
        return h;
    };

    $scope.maxHeight2 = function () {
        var h = document.documentElement.clientHeight -
            $(".navbar-fixed-top").height() -
            $(".navbar-fixed-bottom").height() -
            $(".title").height() -
            20; // missing px somewhere :)
        return h;
    };


    $http.get("/Home/PublicChannels")
        .success(function (data) {
            $scope.channels.list = data;
            $scope.channels.loaded = true;
        });

    $(window).on("resize.doResize", function () {
        $scope.$apply(function () {
            $scope.setHeights();
        });
    });

    $scope.setHeights = function () {
        var h = $(".height");
        h.height($scope.maxHeight());
        h[0].style.overflowY = "auto";
        h[0].style.overflowX = "hidden";

        h = $(".height2");
        h.height($scope.maxHeight2());
        h[0].style.overflowY = "auto";
        h[0].style.overflowX = "hidden";

    }

    $scope.onChannelClick = function (channel) {
        $http.get("/Home/Public/" + channel.Id)
            .success(function(data) {
                $scope.channel.loaded = true;
                $scope.channel.entries = data;
                $scope.channels.current = channel;
            })
            .then(function() {
                $scope.setHeights();
            });
    };

    $scope.isCurrent = function (channel) {
        var x = channel === $scope.channels.current;
        if (x) {
            return "btn-info";
        }
        return "";
    }

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
            url: "/Home/Subscribe",
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
            url: "/Home/Unsubscribe",
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

    $scope.setHeights();
});

