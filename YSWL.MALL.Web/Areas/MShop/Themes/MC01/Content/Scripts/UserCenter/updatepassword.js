/**
* updatepassword.js
*
* 功 能：修改密码
* 文件名称： updatepassword.js
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/06/18 12:00:00   HUHY  初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/




// 验证用户原密码
function checkpassword() {

    var errnum = 0;

    var passwordVal = $.trim($('#txtPwd').val());

    if (passwordVal == '') {
        ShowFailTip('原密码不能为空！');
        return false;
    } else {
        $.ajax({
            url:  $YSWL.BasePath +'u/CheckPassword' ,
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            async: false,
            data: {
                Action: "post",
                Password: passwordVal
            },
            success: function(JsonData) {
                if (JsonData.STATUS == "ERROR") {
                    errnum++;
                    ShowFailTip("原密码错误！");
                } else if (JsonData.STATUS == "OK") {
                   
                } else {
                    errnum++;
                    ShowServerBusyTip("服务器没有返回数据，可能服务器忙，请稍候再试！");
                }
            },
            error: function(XMLHttpRequest, textStatus, errorThrown) {
                errnum++;
                ShowServerBusyTip("服务器没有返回数据，可能服务器忙，请稍候再试！");
            }
        });
    }
    return errnum == 0 ? true: false;
}

// 验证用户新密码
function checknewpassword() {
    var newpasswordVal = $.trim($('#txtNewPwd').val());
    if (newpasswordVal == '') {
        ShowFailTip("新密码不能为空！");
        return false;
    }
    else if (newpasswordVal.length < 6 || newpasswordVal.length > 16) {
        ShowFailTip("新密码长度为6~16个字符！");
        return false;
    }
    else {
        return true;
    }
}

// 验证用户确认密码
function checkconfirmpassword() {

    var newpasswordVal = $.trim($('#txtNewPwd').val());
    var confirmpwdVal = $.trim($('#txtConfirmPwd').val());   
    if (confirmpwdVal == '') {
        ShowFailTip("确认密码不能为空！");
        return false;
    }
    else if (newpasswordVal != confirmpwdVal) {
        ShowFailTip("两次密码不一致,请确认！");
        return false;
    }
  
    return true;

}

function submit() {
    if (!checkpassword()) {
        return false;
    }
    if (!checknewpassword()) {
        return false;
    }
    if (!checkconfirmpassword()) {
        return false;
    }
    
    
        var newpasswordVal = $.trim($('#txtNewPwd').val());
        var confirmpwdVal = $.trim($('#txtConfirmPwd').val());
        $.ajax({
            url:  $YSWL.BasePath +'u/UpdateUserPassword' ,
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            async: false,
            data: {
                Action: "post",
                NewPassword: newpasswordVal,
                ConfirmPassword: confirmpwdVal
            },
            success: function (JsonData) {
                switch (JsonData.STATUS) {
                    case "FAIL":
                        ShowServerBusyTip("新密码和确认密码不一致！");
                        break;
                    case "UPDATESUCC":
                        ShowSuccessTip("修改成功！");
                        setTimeout('location.replace($YSWL.BasePath + "u/Setting");', 2000);
                        break;
                    case "UPDATEFAIL":
                        ShowFailTip("修改密码失败！");
                        break;
                    default:
                        ShowFailTip("服务器没有返回数据，可能服务器忙，请稍候再试！");
                        break;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                ShowServerBusyTip("服务器没有返回数据，可能服务器忙，请稍候再试！");
            }
        });

    
}