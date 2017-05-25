angular
.module('itan')
.factory('subscriptionsApi', ['$http', function($http){
    return {
        loadSubscriptions: function (callback) {
                $http.get("/Subscriptions/MyChannelList")
                    .success(callback);
            },
            onChannelClick: function (streamType, id,callback) {
                var data = {
                    url: '/feeds/ReadAjax',
                    method: 'get',
                    params: {
                        skip: 0,
                        take: 100,
                        feedId: id,
                        streamType:streamType
                    }
                }
                $http(data).success(function (res) {
                    callback(res);
                });
            }
    }
}])