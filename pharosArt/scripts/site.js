function increaseStatistic(id) {
    var imgor = '/umbraco/surface/tracking/updatestatistic';
    //var data = id.toString();
    //alert(data);
    $.ajax({
        type: "GET",
        url: imgor + "?idMedia=" + id.toString(),
        contentType: false,
        processData: false,
        //data: data,
        success: function (result) {
            console.log(result);
            $("#responseMessage").text("Successfully uploaded.");
        },
        error: function (xhr, status, p3, p4) {
            alert('error: ' + id);
            var err = "Error " + " " + status + " " + p3 + " " + p4;
            if (xhr.responseText && xhr.responseText[0] == "{")
                err = JSON.parse(xhr.responseText).Message;
            $("#responseMessage").text("Error - Couldn't uploaded.");
            console.log(err);
        }
    });
}

function hoverFix() {
    var el = this;
    var par = el.parentNode;
    var next = el.nextSibling;
    par.removeChild(el);
    setTimeout(function () { par.insertBefore(el, next); }, 0)
}