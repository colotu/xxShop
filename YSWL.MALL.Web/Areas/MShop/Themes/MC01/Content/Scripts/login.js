var loginNameStatus = true;
var passwordStatus = true;
$(document).ready(function () {
    //登录按钮的单击事件
    $('#loginsubmit').click(function () {
        if (CheckLogin()) {//验证通过
            submitAjaxLogin();
        }
    });

    //微信用户绑定
    $("#bindsubmit").click(function () {
        if (CheckLogin()) {//验证通过
            $(this).attr("disabled", "disabled");
            var userName = $("#txtLogin").val();
            var pwd = $("#txtPwd").val();
            var user = $("#txtUser").val();
            var open = $("#txtOpenId").val();
            $.ajax({
                url: $YSWL.BasePath + "Account/AjaxBind",
                type: 'post',
                dataType: 'text',
                timeout: 10000,
                async: false,
                data: {
                    Action: "post", UserName: userName, UserPwd: pwd, User: user, OpenId: open
                },
                success: function (resultData) {
                    if (resultData == "1") {
                        ShowSuccessTip("绑定用户成功");
                    }
                    if (resultData == "2") {
                        ShowFailTip("该账户已被冻结");
                    }
                    if (resultData == "3") {
                        ShowFailTip("该账户已经绑定了其它帐号");
                    }

                    if (resultData == "0") {
                        ShowFailTip("用户名或者密码有误");
                        $("#bindsubmit").removeAttr("disabled");
                    }
                    if (resultData == "-1") {
                        ShowFailTip("服务器繁忙，请稍候再试");
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    ShowServerBusyTip("服务器繁忙，请稍候再试");
                }

            });
        }
    });
});


//验证登录
function CheckLogin() {
    var regStr = $('#hfRegisterToggle').val(); //注册方式
    
    if (regStr == "Phone") {
        CheckLoginPhoneName($("#txtLogin"));
    } else {
        CheckLoginEmailName($("#txtLogin"));
    }

    CheckPassword($("#txtPwd"));
    var checkOK = false;
    if (!loginNameStatus || !passwordStatus) {
        checkOK = false;
    }
    else {
        checkOK = true;
    }
    return checkOK;
}

////验证邮箱
function CheckLoginEmailName(obj) {
    var regsEmail = /^[\w-]+(\.[\w-]+)*\@[A-Za-z0-9]+((\.|-|_)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/;
    var val = obj.val();
    if (val == ""  ) {
        loginNameStatus = false;
        //$('#diverror').text('请填写邮箱');
        ShowFailTip('请填写邮箱');
        return;
    } else if (!regsEmail.test(val)) {
        ShowFailTip('请填写有效的Email地址');
        loginNameStatus = false;
        return;
    } else {
        loginNameStatus = true;
        $("#divLoginTip").html('');
        return;
    }
}

////验证手机
function CheckLoginPhoneName(obj) {
    //var regs = /^1([38][0-9]|4[57]|5[^4])\d{8}$/;
    //var val = obj.val();
    //if (val == "" ) {
    //    loginNameStatus = false;
    //    ShowFailTip('请填写手机号码');
    //} else if (!regs.test(val)) {
    //    ShowFailTip('手机号码无效');
    //    loginNameStatus = false;
    //} else {
        loginNameStatus = true;
    //}
}


//验证密码
function CheckPassword(obj){
    if (obj.val() != ""){
        passwordStatus = true;
    }else{
        passwordStatus = false;
        ShowFailTip('请填写密码');
    }
}



function submitAjaxLogin(){
    var userName = $('#txtLogin').val();
    var pwd = $('#txtPwd').val();
    var returnUrl = $('#returnUrl').val();
    $.ajax({
        type: "POST",
        dataType: "text",
        url: $YSWL.BasePath + "Account/AjaxLogin",
        async: false,
        data: { UserName: userName, UserPwd: pwd },
        success: function (data){
            if (parseInt(data) == -1){
                ShowFailTip('该功能已关闭');
                return false;
            }else if (data == "NotActivity"){
                ShowFailTip('您的账户已被冻结');
                return false;
            }
            if (parseInt(data.split("|")[0]) > 0) {
                if (returnUrl != "" && returnUrl.length > 0) {
                    location.replace(returnUrl);
                } else {
                    location.replace($YSWL.BasePath);
                }
                return true;
            }else {
                ShowFailTip('用户名或者密码有误');
            }
        }
    });
}
 