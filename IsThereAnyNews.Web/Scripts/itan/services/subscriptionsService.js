angular
    .module("itan")
    .factory('subscriptionsService', ['subscriptionsApi',function (subscriptionsApi) {
        return {
            loadSubscriptions: function ($scope) {
                subscriptionsApi.loadSubscriptions(function(data){
                        $scope.channels.list = data;
                        $scope.channels.loaded = true;
                    });
            },
            onChannelClick: function ($scope,channel, callback) {
                subscriptionsApi.onChannelClick(channel.StreamType, channel.Id, function(data){
                        $scope.channels.current = channel;
                        $scope.channel.loaded = true;
                        $scope.channel.entries = data;
                        callback();
                    });
            },
            isCurrent:function ($scope,channel) {
                return channel === $scope.channels.current;
            }
        };
    }]);
