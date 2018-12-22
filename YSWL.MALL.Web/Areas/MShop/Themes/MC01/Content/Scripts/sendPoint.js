/**
* sendmessage.js
*
* 功 能：发送站内信
* 文件名称：sendmessage.js
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/09/25 12:00:00  蒋海滨    初版
*
* Copyright (c) 2013 JT Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：金泰科技　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

/*错误警告提示信息*/
var warningTip = "<p class=\"noticeWrap\"> <b class=\"ico-warning\"></b><span class=\"txt-err\" >{0}</span></p>";
/*成功的提示信息*/
var succTip = "<p class=\"noticeWrap\"><b class=\"ico-succ\"></b><span class=\"txt-succ\" style='color:#777171'>{0}</span></p>";
/*鼠标移上去*/
var mouseonTip = "<div class=\"txt-info-mouseon\"  style=\"display:none;\">{0}</div>";
/* 鼠标离开*/
var mouseoutTip = "<div class=\"txt-info-mouseout\"  style=\"display:none;\">{0}</div>";

 
$(function() {

    /*验证用户名开始*/
    $("#txtNickName").focus(function() {
       $("#nciknameTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-err").addClass("msg msg-info").html("<i class=\"msg-ico\"></i><p>填写接收人用户名</p>");
    }).blur(function() {
        checknickname();
    });
    /*验证用户名结束*/

    /*验证积分数量*/
    $("#txtTitle").focus(function() {
       $("#titleTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-err").addClass("msg msg-info").html("<i class=\"msg-ico\"></i><p>填写积分数量</p>");
    }).blur(function() {

        checktitle();
       
    });
    /*验证积分数量*/


});
// 验证用户昵称
function checknickname() {
    var nicknameVal = $.trim($('#txtNickName').val());
    if (nicknameVal == '') {
          $("#nciknameTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("<i class=\"msg-ico\"></i><p>接收用户名不能为空！</p>");
        return false;
    }
    var errnum = 0;
    $.ajax({
        url: $YSWL.BasePath + "/UserCenter/ExistsUserNameJF",
        type: 'post',
        dataType: 'json',
        timeout: 10000,
        async: false,
        data: {
            Action: "post",
            NickName: nicknameVal
        },
        success: function(JsonData) {
            switch (JsonData.STATUS) {
            case "EXISTS":
                $("#nciknameTip").removeClass("msg msg-err").removeClass("msg msg-info").addClass("msg msg-ok msg-naked").html("<i class=\"msg-ico\"></i><p>"+JsonData.msg+"</p>");
                break;
            case "NOTEXISTS":
                errnum++;
                $("#nciknameTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("<i class=\"msg-ico\"></i><p>用户名有问题或者不存在，请重新输入！！</p>");
                break; 
            case "NOTNULL":
                errnum++;
                 $("#nciknameTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("<i class=\"msg-ico\"></i><p>接收人用户名不能为空！</p>");
                break;
            default:
                errnum++;
                ShowServerBusyTip("服务器没有返回数据，可能服务器忙，请稍候再试！");
                break;
            }
        },
        error: function(XMLHttpRequest, textStatus, errorThrown) {
            errnum++;
            ShowServerBusyTip("服务器没有返回数据，可能服务器忙，请稍候再试！");
        }
    });

    return errnum == 0 ? true: false;
}

// 验证主题
function checktitle() {    
   var regs = /^\+?[1-9][0-9]*$/;//大于0的正整数
    var titleVal = $.trim($('#txtTitle').val());
    if (titleVal != "") {
            if (!regs.test(titleVal)) {
                $("#titleTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("请填写有效的积分数量！");
                mailStatus = false;
            } 
            else
            {  
                var errnum = 0;
                $.ajax({
                    url: $YSWL.BasePath + "/UserCenter/PointToB",
                    type: 'post',
                    dataType: 'json',
                    timeout: 10000,
                    async: false,
                    data: {
                        Action: "post",
                        Title: titleVal
                    },
                    success: function(JsonData) {
                        switch (JsonData.STATUS) {
                        case "EXISTS":
                            $("#titleTip").removeClass("msg msg-err").removeClass("msg msg-info").addClass("msg msg-ok msg-naked").html("<i class=\"msg-ico\"></i><p>&nbsp;</p>");
                            break;
                        case "NOTEXISTS":
                            errnum++;
                            $("#titleTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("<i class=\"msg-ico\"></i><p>积分不足！</p>");
                            break;
                        default:
                            errnum++;
                            ShowServerBusyTip("服务器没有返回数据，可能服务器忙，请稍候再试！");
                            break;
                        }
                    },
                    error: function(XMLHttpRequest, textStatus, errorThrown) {
                        errnum++;
                        ShowServerBusyTip("服务器没有返回数据，可能服务器忙，请稍候再试！");
                    }
                });
                return errnum == 0 ? true: false;
//                $("#titleTip").removeClass("msg msg-err").removeClass("msg msg-info").addClass("msg msg-ok msg-naked").html("<i class=\"msg-ico\"></i><p>&nbsp;</p>");
//                return true;
            }
     }
     else
    {
        $("#titleTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("<i class=\"msg-ico\"></i><p>积分不能为空！</p>");
        return false;
    }
}


function submit() {

    var errnum = 0;

    if (!checknickname()) {
        errnum++;
    }

    if (!checktitle()) {
        errnum++;
    }

    if (! (errnum == 0 ? true: false)) {
        return false;
    } else {
        var nickname = $.trim($("#txtNickName").val());
        var title = $.trim($("#txtTitle").val());

        $.ajax({
            url: $YSWL.BasePath + "/UserCenter/SendPointMsg",
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            async: false,
            data: {
                Action: "post",
                NickName: nickname,
                Title: title,
            },
            success: function(JsonData) {

                switch (JsonData.STATUS) {
                case "NICKNAMENULL":
                    ShowServerBusyTip("用户名不能为空！");
                    break;
                case "NICKNAMENOTEXISTS":
                    ShowServerBusyTip("用户名不存在，请重新输入！");
                    break;
                case "TITLENULL":
                    ShowServerBusyTip("积分数量不能为空！");
                    break;
                case "SUCC":
                    $("#nciknameTip").removeClass("msg msg-ok msg-naked");
                    $("#titleTip").removeClass("msg msg-ok msg-naked");
                    $("#txtNickName").val("");
                    $("#txtTitle").val("");
                    ShowSuccessTip("提交成功！");
                    break;
                case "FAIL":
                    ShowFailTip("提交失败！");
                    break;
                default:
                    ShowServerBusyTip("服务器没有返回数据，可能服务器忙，请稍候再试！");
                    break;
                }
            },
            error: function(XMLHttpRequest, textStatus, errorThrown) {
                ShowServerBusyTip("服务器没有返回数据，可能服务器忙，请稍候再试！");
            }

        });
    }

}