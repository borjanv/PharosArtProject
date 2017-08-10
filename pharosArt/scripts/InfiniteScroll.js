var page = 0,
    inCallback = false,
    hasReachedEndOfInfiniteScroll = false;

var scrollHandler = function () {
    if (hasReachedEndOfInfiniteScroll == false &&
            ($(window).scrollTop() == $(document).height() - $(window).height())) {
        loadMoreToInfiniteScrollTable(moreRowsUrl);
    }
}

function loadMoreToInfiniteScrollTable(loadMoreRowsUrl, nodeId) {
    if (page > -1 && !inCallback) {
        inCallback = true;
        page++;
        //$("div#loading").show();
        $("#load-more-items").html("Loading...");
        $("#load-more-items").attr("disabled", true);
        $.ajax({
            type: 'GET',
            url: loadMoreRowsUrl,
            data: "pageNum=" + page,
            success: function (data) {
                if (data.length > 52) {
                    $("#home-grid").append(data);
                    $("#load-more-items").html("Load more items");
                    $("#load-more-items").attr("disabled", false);
                }
                else {
                    page = -1;
                    $("#load-more-items").html("No more items");
                    $("#load-more-items").attr("disabled", true);
                    showNoMoreRecords();
                }

                inCallback = false;
                $("div#loading").hide();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                var e = errorThrown;
            }
        });
    }
}
function showNoMoreRecords() {
    hasReachedEndOfInfiniteScroll = true;
}
