angular
.module('itan')
.factory('feedsApi', ['$http', function($http){
    return {
        loadFeedChannel:function(id, callback){
            $http.get("/Feeds/Feed/" + id)
                 .success(callback);
        },
        loadFeeds: function (callback) {
                $http.get("/Feeds/Public")
                    .success(callback);
        },
        subscribe: function (channelId, callback) {
                var httpOptions = {
                    method: 'POST',
                    url: "/Feeds/SubscribeToChannel",
                    data: {
                        channelId: channelId
                    }
                };
                $http(httpOptions)
                    .success(callback);
        },
        unsubscribe: function (channelId, callback) {
                var httpOptions = {
                    method: 'POST',
                    url: "/Feeds/UnsubscribeFromChannel",
                    data: {
                        channelId: channelId
                    }
                };
                $http(httpOptions)
                    .success(callback);
            },


    }
}]);