angular
    .module("itan")
    .controller("itan.feedsController", ['$scope', '$http', 'feedsService', 'windowSizeHandler',
        function ($scope, $http, feedsService, windowSizeHandler) {
            $scope.feedsModel = {
                numberOfAllFeeds: 0,
                feedsLoaded: false,
                feeds: [],
                entries: [],
                currentFeed: {
                    isSubscribed: false
                },
                feedLoaded: false
            }


            $(window).on("resize.doResize", function () {
                $scope.$apply(function () {
                    windowSizeHandler.setHeights();
                });
            });

            $scope.init = function (numberOfAllFeeds) {
                $scope.numberOfAllFeeds = numberOfAllFeeds;
            }

            $scope.onFeedClick = function (feed) {
                $scope.feedsModel.currentFeed = feed;
                feedsService.onClick($scope.feedsModel, feed, function () {
                    windowSizeHandler.setHeights();
                    $scope.feedsModel.feedLoaded = true;
                });
            };

            $scope.isCurrent = function (feed) {
                return feedsService.isCurrent($scope.feedsModel, feed);
            };
            $scope.buttonSubscriptionClass = function (feed) {
                return feedsService.buttonSubscriptionClass(feed);
            };
            $scope.onClickLoadMore = function () {
                feedsService.loadMoreFeeds($scope.feedsModel);
            };
            $scope.onSubscribeClick = function (channelId, isSubscribed) {
                feedsService.onSubscribeClick($scope.feedsModel, channelId, isSubscribed);
            };
            feedsService.loadFeeds($scope.feedsModel);
            windowSizeHandler.setHeights();
        }
    ]);