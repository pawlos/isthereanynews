angular
.module('itan')
.factory('entriesApi',['$http', function($http){
    return {
        markEntriesRead:function(streamType, ids, subscriptionId,callback) {
            var httpOptions = {
                method: 'POST',
                url: "/Entries/MarkEntriesSkipped",
                data: {
                    StreamType: streamType,
                    Entries: ids,
                    SubscriptionId: subscriptionId
                }
            };
            $http(httpOptions)
                .success(callback);
        },

        markReadWithEvent:function (streamType, id, subscriptionId,callback) {
            var httpOptions = {
                method: 'POST',
                url: "/Entries/MarkClicked",
                data: {
                    StreamType: streamType,
                    Id: id,
                    SubscriptionId: subscriptionId
                }
            };
            $http(httpOptions)
                .success(callback);
        },

        onArticleBodyClicked: function (streamType, id, subscriptionId, callback) {
            var httpOptions = {
                method: 'POST',
                url: "/Entries/MarkNavigated",
                data: {
                    StreamType: streamType,
                    Id: id,
                    SubscriptionId: subscriptionId
                }
            };
            $http(httpOptions)
                .success(callback);
        },

        onThumbsUpClicked:function (streamType, id,callback) {
            var httpOptions = {
                method: "POST",
                url: "/Entries/VoteUp",
                data: {
                    streamType: streamType,
                    id: id
                }
            };
            $http(httpOptions).success(callback);
        },

        onThumbsDownClick:function (streamType, id,callback) {
            var httpOptions = {
                method: "POST",
                url: "/Entries/VoteDown",
                data: {
                    streamType: streamType,
                    id: id
                }
            };
            $http(httpOptions).success(callback);
        },

        onMarkUnreadClicked:function (streamType, id,callback) {
            var httpOptions = {
                method: "POST",
                url: "/Entries/MarkNotRead",
                data: {
                    streamType: streamType,
                    id: id
                }
            };
            $http(httpOptions).success(callback);
        },

        onShareClicked:function (streamType, id,callback) {
            var httpOptions = {
                method: "POST",
                url: "/Entries/Share",
                data: {
                    streamType: streamType,
                    id: id
                }
            };
            $http(httpOptions).success(callback);
        },
        onCommentsClicked:function (streamType, id,callback) {
            var httpOptions = {
                method: "POST",
                url: "/Entries/AddComment",
                data: {
                    streamType: streamType,
                    id: id
                }
            };
            $http(httpOptions).success(callback);
        },

        onReadLaterClicked:function (streamType, id,callback) {
            var httpOptions = {
                method: "POST",
                url: "/Entries/AddToReadLater",
                data: {
                    streamType: streamType,
                    id: id
                }
            };
            $http(httpOptions).success(callback);
        }
    }
}])