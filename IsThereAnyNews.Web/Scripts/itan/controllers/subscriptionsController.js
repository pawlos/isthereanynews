angular
    .module("itan")
    .controller("itan.subscriptionsController", 
               ['$scope','$http', 'subscriptionService','entryService','windowSizeHandler',
        function ($scope, $http, subscriptionService,entryService,windowSizeHandler) {
            $scope.channels = {
                loaded: false,
                list: {},
                current: {}
            };

            $scope.channel = {
                loaded: false,
                entries: {}
            };

            subscriptionService.loadSubscriptions($scope,$http);

            $scope.onChannelClick =  function (channel) {
                subscriptionService.onChannelClick($scope,$http,channel);
            };
            $scope.markEntriesRead = function(entries) {
                entryService.markEntriesRead($scope, $http,entries);
            };
            $scope.markReadWithEvent = function(streamType, item) {
                entryService.markReadWithEvent($scope, $http, streamType, item);
            };
            $scope.onArticleBodyClicked = function(streamType, id, url) {
                entryService.onArticleBodyClicked($http, streamType, id, url);
            };
            $scope.onThumbsUpClicked = function(streamType, id) {
                entryService.onThumbsUpClicked($http, streamType, id);
            };
            $scope.onThumbsDownClick = function(streamType, id) {
                entryService.onThumbsDownClick($http, streamType, id);
            };
            $scope.onMarkUnreadClicked = function(streamType, id) {
                entryService.onMarkUnreadClicked($http, streamType, id);
            };
            $scope.onShareClicked = function(streamType, id) {
                entryService.onShareClicked($http, streamType, id);
            };
            $scope.onCommentsClicked = function(streamType, id) {
                entryService.onCommentsClicked($http, streamType, id);
            };
            $scope.onReadLaterClicked = function(streamType, id) {
                entryService.onReadLaterClicked($http, streamType, id);
            };
            $scope.onShareClicked = function(streamType, id) {
                entryService.onShareClicked($http, streamType, id);

            };

           $(window).on("resize.doResize", function () {
                $scope.$apply(function () {
                    windowSizeHandler.setHeights();
                });
            });

            $scope.isCurrent = function (channel) {
                if(subscriptionService.isCurrent($scope, channel)){
                    return "btn-info";
                }
                return "";
            }
        }]);

