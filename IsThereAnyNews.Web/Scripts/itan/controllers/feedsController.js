angular
    .module("itan")
    .controller("itan.feedsController",
    ['$scope', '$http', 'feedsService','windowSizeHandler',
        function ($scope, $http, feedsService, windowSizeHandler) {
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

            $(window).on("resize.doResize", function () {
                $scope.$apply(function () {
                    windowSizeHandler.setHeights();
                });
            });

            $scope.onChannelClick = function (channel) {
                feedsService.onChannelClick($scope, channel, function(){
                    windowSizeHandler.setHeights();
                });
            };

            $scope.isCurrent = function (channel) {
                if (feedsService.isCurrent($scope, channel)) {
                    return "btn-info";
                }
                return "";
            }

            $scope.buttonSubscriptionClass = function (channel) {
                return feedsService.buttonSubscriptionClass(channel);
            }

            $scope.onSubscribeClick = function (channelId, isSubscribed) {
                feedsService.onSubscribeClick($scope, channelId, isSubscribed);
            };

            feedsService.loadFeeds($scope);
            windowSizeHandler.setHeights();
        }]);

