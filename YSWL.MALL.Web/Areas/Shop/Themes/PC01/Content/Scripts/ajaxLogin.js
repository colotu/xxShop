//检查是否登录  如果未登录  弹出登录层并返回false
                        //如果已登录 返回true
var CheckUserState = function () {
    var dialogOpts = {
        title: "登录",
        width: 400,
        modal: true,
        resizable: false,
        buttons: {
            "确定": function () {
                submitAjaxLogin();
            },
            "取消": function () {
                //  $(this).dialog("close"); //关闭层
                $("#divAjaxLogin").dialog("close");
            }
        }
    };
    var islogin;
    $.ajax({
        url: $YSWL.BasePath + "Account/AjaxIsLogin",
        type: 'post',
        dataType: 'text',
        async: false,
        success: function (resultData) {
            if (resultData != "True") {
                $("#divAjaxLogin").dialog(dialogOpts);
                //dialog层中项的设置
                islogin = false;
                return false;
            } else {
                islogin = true;
                return true;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
    return islogin;
};

function submitAjaxLogin() {
    var userName = $('#txtEmail').val();
    var pwd = $('#txtPwd').val();
    var str = '';
    var regStr = $('#hfRegisterToggle').val(); //注册方式
    if (regStr == "Phone") {
        str = '手机号码';
    } else {
        str = '邮箱地址';
    }
    if (userName == '') {
        ShowFailTip("请输入" + str + "!");
        return false;
    }
    if (regStr == "Phone") {//手机登录
        //var regs = /^1([38][0-9]|4[57]|5[^4])\d{8}$/;
        //if (!regs.test(userName)) {
        //    ShowFailTip("请填写有效的手机号码");
        //    return false;
        //}
    } else {//邮箱登录
        var regsEmail = /^[\w-]+(\.[\w-]+)*\@[A-Za-z0-9]+((\.|-|_)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/;
        if (!regsEmail.test(userName)) {
            ShowFailTip("请填写有效的Email地址");
            return false;
        }
    }

    if (pwd == '') {
        ShowFailTip("请输入密码！");
        return false;
    }

    $.ajax({
        type: "POST",
        dataType: "text",
        url: $YSWL.BasePath + "Account/AjaxLogin",
        async: false,
        data: { UserName: userName, UserPwd: pwd },
        success: function (data) {
            if (parseInt(data) == -1) {
                ShowFailTip('该功能已被管理员关闭，如有疑问，请联系网站管理员');
                return false;
            } else if (data == "NotActivity") {
                ShowFailTip('您的账户已被冻结，如有疑问，请联系网站管理员');
                return false;
            }
            if (parseInt(data.split("|")[0]) > 0) {
                $("#divAjaxLogin").dialog("close");
               if ($('#loginLayer').length > 0) {
                $('#loginLayer').load($YSWL.BasePath + 'Partial/Login');
               }
                location.replace(location.href);
                return true;
            }
            else {
                ShowFailTip('用户名或者密码不正确，请重试');
            }
        }
    });
}

var CheckLogin = function () {
    var islogin;
    $.ajax({
        url: $YSWL.BasePath + "Account/AjaxIsLogin",
        type: 'post',
        dataType: 'text',
        timeout: 10000,
        async: false,
        success: function (resultData) {
            if (resultData != "True") {
                islogin = false;
                return false;
            } else {
                islogin = true;
                return true;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        }
    });
    return islogin;
};
 
