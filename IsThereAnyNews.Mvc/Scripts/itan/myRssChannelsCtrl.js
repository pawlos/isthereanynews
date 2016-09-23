angular.module("itan")
.controller("itan.MyRssChannelsCtrl", function ($scope, $http) {
    $scope.channels = {
        loaded: false
    };

        $scope.channel = {
            loaded: false
        };

    $http.get("/RssChannel/MyChannelList")
        .success(function (data) {
            $scope.channels.list = data;
            $scope.channels.loaded = true;
        });

        $scope.onChannelClick = function(streamType, subscriptionId) {
            $http.get("/Stream/ReadAjax/" + streamType + "/" + subscriptionId)
                .success(function(data) {
                    $scope.channel.loaded = true;
                    $scope.channel.entries = data;
                    $(".nocss-rss-item-list").collapsible();
                });
        };

        $scope.markReadWithEvent = function(streamType, id) {
            var getOptions= {
                method:'POST',
                url: "/Stream/MarkReadWithEvent",
                data: {
                    StreamType:streamType,
                    DisplayedItems: id,
                    Id:id
                }
            }
            $http(getOptions)
                .success(function() {
                    console.log("successufully marked as read");
                });
        };
    });

