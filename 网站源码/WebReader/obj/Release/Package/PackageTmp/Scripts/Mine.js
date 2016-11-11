$("#bt_search").click(function () {
    var keyword = $("#input_search").val();
    if ("" == keyword)
        return;
    $.ajax({
        type: "POST",
        url: "/File/Search",
        data: {
            keyword: keyword
        },
        dataType: "html",
        success: function (data) {
            $("#all").html(data);
        }
    });
});

function UpdateShare(fid, access) {
    $.ajax({
        type: "POST",
        url: "/File/UpdateShare",
        data: {
            fileid: fid,
            access, access
        },
        dataType: "text",
        success: function (data) {
            alert(data);
            if (data == "修改成功") {
                if (access)
                    $("#" + fid).text("取消共享");
                else
                    $("#" + fid).text("共享");
            }
        }
    });
}
function DeleteFile(fid) {
    $.ajax({
        type: "POST",
        url: "/File/Delete",
        data: { fileid: fid },
        dataType: "text",
        success: function (data) {
            alert(data);
            if (data == "删除成功")
                $("#group_" + fid).remove();
        }
    });
}