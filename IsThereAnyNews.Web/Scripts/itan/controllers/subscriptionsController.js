angular
    .module("itan")
    .controller("itan.subscriptionsController", ['$scope', 'subscriptionsService', 'entriesService', 'windowSizeHandler',
        function ($scope, subscriptionsService, entriesService, windowSizeHandler) {
            $scope.subscriptionModel = {
                channels: {
                    loaded: false,
                    list: {},
                    current: {}
                },
                channel: {
                    loaded: false,
                    entries: {}
                }
            }

            subscriptionsService.loadSubscriptions($scope.subscriptionModel);

            $scope.onChannelClick = function (channel) {
                subscriptionsService.onChannelClick($scope.subscriptionModel, channel, function () {
                    $(".nocss-rss-item-list")
                        .collapse({
                            toggle: false
                        });
                });
            };

            $scope.unreadRss = function (entries) {
                var x = _.reject(entries, function (i) { return i.rssEntryViewModel.isRead });
                return x.length;
            }

            $scope.loadMoreEntries = function () {
                subscriptionsService.loadMoreEntries($scope.subscriptionModel)
            };
            $scope.markEntriesRead = function (entries) {
                entriesService.markEntriesRead($scope.subscriptionModel, entries);
            };
            $scope.markReadWithEvent = function (streamType, item) {
                entriesService.markReadWithEvent($scope.subscriptionModel, streamType, item);
            };
            $scope.onArticleBodyClicked = function (streamType, item) {
                entriesService.onArticleBodyClicked(streamType, item);
            };
            $scope.onThumbsUpClicked = function (streamType, id) {
                entriesService.onThumbsUpClicked(streamType, id);
            };
            $scope.onThumbsDownClick = function (streamType, id) {
                entriesService.onThumbsDownClick(streamType, id);
            };
            $scope.onMarkUnreadClicked = function (streamType, id) {
                entriesService.onMarkUnreadClicked(streamType, id);
            };
            $scope.onShareClicked = function (streamType, id) {
                entriesService.onShareClicked(streamType, id);
            };
            $scope.onCommentsClicked = function (streamType, id) {
                entriesService.onCommentsClicked(streamType, id);
            };
            $scope.onReadLaterClicked = function (streamType, id) {
                entriesService.onReadLaterClicked(streamType, id);
            };
            $scope.onShareClicked = function (streamType, id) {
                entriesService.onShareClicked(streamType, id);

            };

            $(window).on("resize.doResize", function () {
                $scope.$apply(function () {
                    windowSizeHandler.setHeights();
                });
            });

            $scope.isCurrent = function (channel) {
                if (subscriptionsService.isCurrent($scope.subscriptionModel, channel)) {
                    return "btn-info";
                }
                return "";
            }
        }
    ]);