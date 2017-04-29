angular
    .module("itan")
    .factory("entryService", function () {
        return {
        markEntriesRead:function($scope, $http, entries) {
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
                .success(function() {
                    $scope.channels.current.Count = 0;
                    $scope.channel.entries.RssEntryToReadViewModels = [];
                });
        },

        markReadWithEvent:function ($scope, $http, streamType, item) {
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
        },

        onArticleBodyClicked:function ($http, streamType, id, url) {
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
        },

        onThumbsUpClicked:function ($http, streamType, id) {
            var httpOptions = {
                method: "POST",
                url: "/Entries/VoteUp",
                data: {
                    streamType: streamType,
                    id: id
                }
            };
            $http(httpOptions);
        },

        onThumbsDownClick:function ($http, streamType, id) {
            var httpOptions = {
                method: "POST",
                url: "/Entries/VoteDown",
                data: {
                    streamType: streamType,
                    id: id
                }
            };
            $http(httpOptions);
        },

        onMarkUnreadClicked:function ($http, streamType, id) {
            var httpOptions = {
                method: "POST",
                url: "/Entries/MarkNotRead",
                data: {
                    streamType: streamType,
                    id: id
                }
            };
            $http(httpOptions);
        },

        onShareClicked:function ($http, streamType, id) {
            var httpOptions = {
                method: "POST",
                url: "/Entries/Share",
                data: {
                    streamType: streamType,
                    id: id
                }
            };
            $http(httpOptions);
        },
        onCommentsClicked:function ($http, streamType, id) {
            var httpOptions = {
                method: "POST",
                url: "/Entries/AddComment",
                data: {
                    streamType: streamType,
                    id: id
                }
            };
            $http(httpOptions);
        },

        onReadLaterClicked:function ($http, streamType, id) {
            var httpOptions = {
                method: "POST",
                url: "/Entries/AddToReadLater",
                data: {
                    streamType: streamType,
                    id: id
                }
            };
            $http(httpOptions);
        }
    }});
