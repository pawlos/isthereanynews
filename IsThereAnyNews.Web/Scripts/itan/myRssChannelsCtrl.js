angular.module("itan")
.controller("itan.MyRssChannelsCtrl", function ($scope, $http) {
    $scope.channels = {
        loaded: false,
        list: {},
        current: {}
    };

    $scope.channel = {
        loaded: false,
        entries: {}
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
            $(".utils").height() -
            20; // missing px somewhere :)
        return h;
    };

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

        var h2 = $(".height2");
        h2.height($scope.maxHeight2());
        h2[0].style.overflowY = "auto";
        h2[0].style.overflowX = "hidden";
    }

    $scope.loadChannels = function () {
        $http.get("/Subscriptions/MyChannelList")
       .success(function (data) {
           $scope.channels.list = data;
           $scope.channels.loaded = true;
       });
    }

    $scope.isCurrent = function (channel) {
        var x = channel === $scope.channels.current;
        if (x) {
            return "btn-info";
        }
        return "";
    }

    $http.get("/Subscriptions/MyChannelList")
        .success(function (data) {
            $scope.channels.list = data;
            $scope.channels.loaded = true;
        });

    $scope.onChannelClick = function (channel) {
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
    };

    $scope.markEntriesRead = function (entries) {
        var ids = entries.DisplayedRss;
        var httpOptions = {
            method: 'POST',
            url: "/Entries/MarkEntriesSkipped",
            data: {
                StreamType: entries.StreamType,
                Entries: ids,
                SubscriptionId: entries.SubscriptionId
            }

        };
        $http(httpOptions)
        .success(function () {
            $scope.channels.current.Count = 0;
            $scope.channel.entries.RssEntryToReadViewModels = [];
        });
    }

    $scope.markReadWithEvent = function (streamType, item) {
        var httpOptions = {
            method: 'POST',
            url: "/Entries/MarkReadWithEvent",
            data: {
                StreamType: streamType,
                Id: item.RssEntryViewModel.Id,
                SubscriptionId: item.RssEntryViewModel.SubscriptionId
            }
        };
        $http(httpOptions)
            .success(function () {
                if (!item.wasRead) {
                    $scope.channels.current.Count--;
                }
                item.wasRead = true;
            });
    };

    $scope.onArticleBodyClicked = function (streamType, id, url) {
        var httpOptions = {
            method: 'POST',
            url: "/Entries/MarkClickedWithEvent",
            data: {
                Id: id
            }
        };
        $http(httpOptions)
            .success(function () {
            });
        window.open(url, "_blank");
    };

    $scope.onThumbsUpClicked = function (streamType, id) {
        var httpOptions = {
            method: "POST",
            url: "/Entries/VoteUp",
            data: {
                streamType: streamType,
                id: id
            }
        };
        $http(httpOptions);
    };
    $scope.onThumbsDownClick = function (streamType, id) {
        var httpOptions = {
            method: "POST",
            url: "/Entries/VoteDown",
            data: {
                streamType: streamType,
                id: id
            }
        };
        $http(httpOptions);
    };

    $scope.onMarkUnreadClicked = function (streamType, id) {
        var httpOptions = {
            method: "POST",
            url: "/Entries/MarkNotRead",
            data: {
                streamType: streamType,
                id: id
            }
        };
        $http(httpOptions);
    };

    $scope.onShareClicked = function (streamType, id) {
        var httpOptions = {
            method: "POST",
            url: "/Entries/Share",
            data: {
                streamType: streamType,
                id: id
            }
        };
        $http(httpOptions);
    };
    $scope.onCommentsClicked = function (streamType, id) {
        var httpOptions = {
            method: "POST",
            url: "/Entries/AddComment",
            data: {
                streamType: streamType,
                id: id
            }
        };
        $http(httpOptions);
    };

    $scope.onReadLaterClicked = function (streamType, id) {
        var httpOptions = {
            method: "POST",
            url: "/Entries/AddToReadLater",
            data: {
                streamType: streamType,
                id: id
            }
        };
        $http(httpOptions);
    };

    $scope.onAddChannelClicked = function (newChannel) {
        var options = {
            method: "POST",
            url: "/Home/AddChannel",
            data: {
                RssChannelLink: newChannel.link,
                RssChannelName: newChannel.title
            }
        };
        $http(options)
            .success(function () {
                $scope.loadChannels();
            });
    };

    $scope.setHeights();

});

