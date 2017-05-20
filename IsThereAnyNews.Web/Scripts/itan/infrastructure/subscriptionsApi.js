angular
.module('itan')
.factory('subscriptionsApi', ['$http', function($http){
    return {
        loadSubscriptions: function (callback) {
                $http.get("/Subscriptions/MyChannelList")
                    .success(callback);
            },
            onChannelClick: function (streamType, id,callback) {
                $http.get("/Feeds/ReadAjax?streamType=" + streamType + "&id=" + id)
                    .success(callback);
            }
    }
}])