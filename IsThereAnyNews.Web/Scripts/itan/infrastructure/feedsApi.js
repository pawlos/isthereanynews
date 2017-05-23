angular
    .module('itan')
    .factory('feedsApi', ['$http', function ($http) {
        return {
            loadFeedChannel: function (id, callback) {
                var data = {
                    url: '/feeds/entries',
                    method: 'get',
                    params: {
                        skip: 0,
                        take: 100,
                        feedId: id
                    }
                }
                $http(data).success(function (res) {
                    callback(res);
                });
            },
            loadFeeds: function (callback) {
                $http.get("/Feeds/Public")
                    .success(callback);
            },
            subscribe: function (feedId, callback) {
                var httpOptions = {
                    method: 'POST',
                    url: "/Feeds/SubscribeToChannel",
                    params: {
                        feedId: feedId
                    }
                };
                $http(httpOptions)
                    .success(callback);
            },
            unsubscribe: function (feedId, callback) {
                var httpOptions = {
                    method: 'POST',
                    url: "/Feeds/UnsubscribeFromChannel",
                    params: {
                        feedId: feedId
                    }
                };
                $http(httpOptions)
                    .success(callback);
            },
            loadMoreFeeds: function (toSkip, callback) {
                var data = {
                    url: '/Feeds/Public',
                    method: 'get',
                    params: {
                        skip: toSkip,
                    }
                }
                $http(data).success(function (res) {
                    callback(res);
                });
            }
        }
    }]);