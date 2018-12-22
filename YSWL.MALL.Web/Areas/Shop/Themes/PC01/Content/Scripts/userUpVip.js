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

    /*验证推荐人编号*/
    $("#txtTjrUsernmae").focus(function () {
        $("#TjrUsernmaeTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-err").addClass("msg msg-info").html("<i class=\"msg-ico\"></i><p>填写推荐人编号</p>");
    }).blur(function() {
        checkTjrUsernmae();
    });
    /*验证推荐人编号*/

    /*填写用户姓名*/
    $("#txtNickName").focus(function () {
        $("#nciknameTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-err").addClass("msg msg-info").html("<i class=\"msg-ico\"></i><p>填写用户姓名</p>");
    }).blur(function() {
            
    });
    /*验证积分数量*/

    /*填写手机号*/
    $("#txtTelPhone").focus(function () {
        $("#txtTelPhoneTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-err").addClass("msg msg-info").html("<i class=\"msg-ico\"></i><p>填写手机号</p>");
    }).blur(function () {

    });
    /*验证积分数量*/

    /*填写店铺编号*/
    $("#txtshenghuoguan").focus(function () {
        $("#TjrshenghuoguanTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-err").addClass("msg msg-info").html("<i class=\"msg-ico\"></i><p>填写店铺编号2</p>");
    }).blur(function () {
        checkShenghuoguanUsernmae();
    });
    /*验证积分数量*/

});
// 验证推荐人编号
function checkTjrUsernmae() {
    var TjrnameVal = $.trim($('#txtTjrUsernmae').val());
    if (TjrnameVal == '') {
        $("#TjrUsernmaeTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("<i class=\"msg-ico\"></i><p>升级VIP会员推荐人编号不能为空！</p>");
        return false;
    }
    var errnum = 0;
    $.ajax({
        url: "/UserCenter/CheckTjrName",
        type: 'post',
        dataType: 'json',
        timeout: 10000,
        async: false,
        data: {
            Action: "post",
            TjrName: TjrnameVal
        },
        success: function(JsonData) {
            switch (JsonData.STATUS) {
            case "EXISTS":
                $("#TjrUsernmaeTip").removeClass("msg msg-err").removeClass("msg msg-info").addClass("msg msg-ok msg-naked").html("<i class=\"msg-ico\"></i><p>" + JsonData.msg + "</p>");
                break;
            case "NOTEXISTS":
                errnum++;
                $("#TjrUsernmaeTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("<i class=\"msg-ico\"></i><p>该推荐人在VIP系统不存在，请重新输入</p>");
                break; 
            case "NOTNULL":
                errnum++;
                $("#TjrUsernmaeTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("<i class=\"msg-ico\"></i><p>用户名不能为空！</p>");
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


//验证店铺编号是否存在
function checkShenghuoguanUsernmae() {
    var ShenghgVal = $.trim($('#txtshenghuoguan').val());
    if (ShenghgVal == '') {
        $("#TjrshenghuoguanTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("<i class=\"msg-ico\"></i><p>升级VIP会员店铺编号不能为空！</p>");
        return false;
    }
    var errnum = 0;
    $.ajax({
        url: "/UserCenter/ExistsWdbhfzr",
        type: 'post',
        dataType: 'json',
        timeout: 10000,
        async: false,
        data: {
            Action: "post",
            ShenghgName: ShenghgVal
        },
        success: function (JsonData) {
            switch (JsonData.STATUS) {
                case "EXISTS":
                    $("#TjrshenghuoguanTip").removeClass("msg msg-err").removeClass("msg msg-info").addClass("msg msg-ok msg-naked").html("<i class=\"msg-ico\"></i><p>" + JsonData.msg + "</p>");
                    break;
                case "NOTshenghg":
                    errnum++;
                    $("#TjrshenghuoguanTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("<i class=\"msg-ico\"></i><p>该店铺编号在VIP系统不存在，请重新输入</p>");
                    break;
                case "NOTEXISTS":
                    errnum++;
                    $("#TjrshenghuoguanTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("<i class=\"msg-ico\"></i><p>填写的不是店铺编号！</p>");
                    break;
                default:
                    errnum++;
                    ShowServerBusyTip("服务器没有返回数据，可能服务器忙，请稍候再试！");
                    break;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            errnum++;
            ShowServerBusyTip("服务器没有返回数据，可能服务器忙，请稍候再试！");
        }
    });

    return errnum == 0 ? true : false;
}



function submit() {

    var errnum = 0;

    if (!checkTjrUsernmae()) {
        errnum++;
    }

    if (! (errnum == 0 ? true: false)) {
        return false;
    } else {
        var username = $.trim($("#txtUseName").val());
        var nicktitle = $.trim($("#txtNickName").val());
        var telphone = $.trim($("#txtTelPhone").val());
        var passone = $.trim($("#passone").val());
        var passtwo = $.trim($("#passtwo").val());
        var userLeave = $.trim($("#userLeve").val());
        var tjruser = $.trim($("#txtTjrUsernmae").val());
        var shenghuoguan = $.trim($("#txtshenghuoguan").val());

        $.ajax({
            url: "/UserCenter/UserUpVipMsg",
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            async: false,
            data: {
                Action: "post",
                Username: username,
                Nicktitle: nicktitle,
                Telphone: telphone,
                Passone: passone,
                Passtwo: passtwo,
                UserLeave: userLeave,
                Tjruser: tjruser,
                Shenghuoguan: shenghuoguan,
            },
            success: function(JsonData) {

                switch (JsonData.STATUS) {
                case "NICKNAMENULL":
                        ShowServerBusyTip("用户姓名不能为空！");
                    break;
                    case "TjruserNULL":
                        ShowServerBusyTip("推荐人编号不能为空，请输入！");
                    break;
                    case "strUserTelNULL":
                        ShowServerBusyTip("手机号不能为空，请输入！");
                        break;
                    case "NOTEXISTS":
                        ShowServerBusyTip("推荐人编号在会员系统不存在！");
                        break;
                    case "NOTshenghg":
                        ShowServerBusyTip("店铺编号系统不存在！");
                        break;
                    case "RecommendUserNotActivation":
                        ShowServerBusyTip("推荐人未激活，不允许推荐注册！");
                        break;
                    case "SUCCNot":
                        ShowServerBusyTip("会员已升级为VIP，但没有激活，请联系公司！");
                        break;
                    case "LOGINNAME_ALREADY_REGISTERED":
                        ShowServerBusyTip("会员已经是VIP会员，如有问题请联系公司！");
                        break;
                case "Notjfbuzu":
                        ShowServerBusyTip("钱包余额不足升级费用！");
                    break;
                case "SUCC":
                    $("#nciknameTip").removeClass("msg msg-ok msg-naked");
                    $("#titleTip").removeClass("msg msg-ok msg-naked");
                    $("#txtNickName").val("");
                    $("#txtTelPhone").val("");
                    $("#txtTjrUsernmae").val("");
                    $("#txtshenghuoguan").val("");
                    ShowSuccessTip("升级VIP成功！");
                    break;
                case "FAIL":
                    ShowFailTip("升级VIP失败！");
                    break;
                default:
                    ShowServerBusyTip("服务器没有返回数据，可能服务器忙，请稍候再试！");
                    break;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                ShowServerBusyTip("服务器没有返回数据，可能服务器忙，请稍候再试！");
            }

        });
    }

}