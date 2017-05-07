angular
    .module("itan")
    .factory("entriesService", ['entriesApi',function (entriesApi) {
        return {
        markEntriesRead:function($scope,entries) {
            entriesApi.markEntriesRead(entries.StreamType,entries.DisplayedRss,entries.SubscriptionId,function(){
                    $scope.channels.current.Count = 0;
                    $scope.channel.entries.RssEntryToReadViewModels = [];
                });
        },
        markReadWithEvent:function ($scope, streamType, item) {
            if (!item.IsRead) {
                entriesApi.markReadWithEvent(streamType,item.RssEntryViewModel.Id,item.RssEntryViewModel.SubscriptionId,
                function(){
                        if (!item.IsRead) {
                            $scope.channels.current.Count--;
                        }
                        item.IsRead = true;
                    });
            }
        },
        onArticleBodyClicked:function (streamType, item) {
            entriesApi.onArticleBodyClicked(streamType, item.RssEntryViewModel.Id, item.RssEntryViewModel.SubscriptionId, function(){
                window.open(item.RssEntryViewModel.Url, "_blank");
            });
        },
        onThumbsUpClicked:function ($http, streamType, id) {
            entriesApi.onThumbsUpClicked(id, function(){});
        },
        onThumbsDownClick:function ($http, streamType, id) {
            entriesApi.onThumbsDownClick(id, function(){});
        },
        onMarkUnreadClicked:function ($http, streamType, id) {
            entriesApi.onMarkUnreadClicked(id, function(){});
        },
        onShareClicked:function ($http, streamType, id) {
            entriesApi.onShareClicked(id, function(){});
        },
        onCommentsClicked:function ($http, streamType, id) {
            entriesApi.onCommentsClicked(id, function(){});
        },
        onReadLaterClicked:function ($http, streamType, id) {
            entriesApi.onReadLaterClicked(id, function(){});
        }
    }}]);
