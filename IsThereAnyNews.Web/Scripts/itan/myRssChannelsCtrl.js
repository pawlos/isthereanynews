angular
    .module("itan")
    .controller("itan.MyRssChannelsCtrl", 
               ['$scope','$http', 'subscriptionService','entryService',
        function ($scope, $http, subscriptionService,entryService) {
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


            $scope.maxHeight = function () {
                var h = document.documentElement.clientHeight -
                    $(".navbar-fixed-top").height() -
                    $(".navbar-fixed-bottom").height() -
                    20; // missing px somewhere :)
                return h;
            };

            $scope.maxHeight2 = function () {
                var h = document.documentElement.clientHeight -
                    $(".navbar-fixed-top").height() -
                    $(".navbar-fixed-bottom").height() -
                    $(".title").height() -
                    $(".utils").height() -
                    20; // missing px somewhere :)
                return h;
            };

            $(window).on("resize.doResize", function () {
                $scope.$apply(function () {
                    $scope.setHeights();
                });
            });

            $scope.setHeights = function () {
                var h = $(".height");
                h.height($scope.maxHeight());
                h[0].style.overflowY = "auto";
                h[0].style.overflowX = "hidden";

                var h2 = $(".height2");
                h2.height($scope.maxHeight2());
                h2[0].style.overflowY = "auto";
                h2[0].style.overflowX = "hidden";
            }

            $scope.isCurrent = function (channel) {
                if(subscriptionService.isCurrent($scope, channel)){
                    return "btn-info";
                }
                return "";
            }
        }]);

