var registerType = 'Mail';
var regs = /^[A-Za-z0-9]{6,30}$/;
var focusmsg = '请填写密码（6-30位数字或字母）';
var errormsg = '密码6-30位，支持“数字、字母”';
var mailStatus = true;
var nicknameStatus = true;
var pwdStatus = true;
var vpwdStatus = true;
var phoneStatus = true;
var codeStatus = false;
var agreementStatus = true;
var checkOK = true;

var InviteStatus = true; //邀请人号

var isOK = true;
var smsSeconds = 60;
var intervaSMS;
var validateOnce = {
    Email: "",
    Exists: false
};

$(function () {
    var regStr = $('#hfRegisterToggle').val(); //注册方式
    var isOpen = $("#hfSMSIsOpen").val();
    if (regStr == 'Phone') {
        if (isOpen == "True") {
            $(".txtphone").show();
        }
    }
    //刷新页面时获取时间
    var time = $("#hfSeconds").val();
    if (time > 0) {
        smsSeconds = time;
        $("#btnSendSMS").attr("value", "请在(" + smsSeconds + ")秒后重新发送");
        intervaSMS = setInterval("CountDown()", 1000);
    }
    //点击回复触发
    $("#aUserAgreement").click(function () {
        $("#divUserAgreement").dialog(dialogOpts); //弹出‘用户协议’层  
    });

    //dialog层中项的设置
    var dialogOpts = {
        title: "用户注册协议",
        width: 700,
        height: 600,
        modal: true,
        buttons: {
            "同意": function () {
                $("#chkAgreement").attr("checked", "checked"); //选中同意
                $(this).dialog("close"); //关闭层
            },
            "取消": function () {
                $(this).dialog("close"); //关闭层
            }
        }
    };


    //注册按钮
    $("#btnEmailRegister").click(function () {
        $("#divRegTip").removeClass().html("");
        if (regStr == 'Phone') {
            if (!codeStatus && isOpen == "True") {
                $("#divVerifyCodeTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("手机校验码不正确！");
                return;
            }
        }
        if (CheckRegister()) {
            $('#registerf').submit();
            //$(".form-regi").submit();
        }
    });

    $("#btnSendSMS").click(function () {
        CheckPhone($("#phone"));
        if (phoneStatus == false) {
            return;
        }
        var phone = $("#phone").val();
        var imageCode = $('#imageCode').val();
        if (imageCode == "") {
            $("#divImageCodeTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("请输入验证码！");
            return;
        }
        if (phone == "") {
            $("#divPhoneTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("请输入手机号码！");
            return;
        }
 
        if (phoneStatus) {
            //发送短信
            $.ajax({
                url: $YSWL.BasePath + "Account/SendSMS",
                type: 'post',
                dataType: 'json',
                timeout: 10000,
                async: false,
                data: {
                    Action: "post", Phone: phone, ImageCode: imageCode
                },
                success: function (resultData) {
                    
                    switch (resultData.STATUS) {
                        case "SUCCESS":
                            ShowSuccessTip("发送短信成功");
                            smsSeconds = 60;
                            //console.log(resultData.rand);
                            $("#hfPhoneNumber").val(resultData.DATA);
                            $("#btnSendSMS").attr("value", "请在(" + smsSeconds + ")秒后重新发送");
                            intervaSMS = setInterval("CountDown()", 1000);
                            break;
                        case "PHONEISNULL": //手机号为空
                            $("#divPhoneTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("<i class=\"msg-ico\"></i><p>请输入手机号码！</p>");
                            ShowFailTip("");
                            break;
                        case "IMAGECODEISINULL": //图形验证码为空
                            $("#divImageCodeTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("<i class=\"msg-ico\"></i><p>请输入验证码！</p>");
                            break;
                        case "IMAGECODEISEXPIRED": //图形验证码已失效
                            $("#divImageCodeTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("<i class=\"msg-ico\"></i><p>验证码已过期,请重新输入！</p>");
                            //刷新图形验证码
                            ChangeImageCode();
                            break;
                        case "IMAGECODEISERROR": //验证码错误
                            $("#divImageCodeTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("<i class=\"msg-ico\"></i><p>验证码有误,请重新输入！</p>");
                            //ShowFailTip("验证码有误,请重新输入");
                            break;
                        case "SENDSMSFREQUENT": //发送短信频繁
                            ShowFailTip("发送短信频繁，请稍后重试！");
                            //刷新图形验证码
                            ChangeImageCode();
                            break;
                        case "FAILED": //发送验证码失败
                            ShowFailTip("短信验证码发送失败");
                            break
                        default:
                            ShowFailTip("服务器没有返回数据，可能服务器忙，请稍候再试！");
                            break;
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    ShowFailTip("服务器没有返回数据，可能服务器忙，请稍候再试！");

                }

            });
        }
    });

    $("#imageCode").blur(function () {
        if ($(this).val() != "") {
            $("#divImageCodeTip").removeClass("msg msg-ok msg-naked msg-info msg-err").html('');
            return;
        }
    });
    
    $("#checkCode").focus(function () {
        $("#divVerifyCodeTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-err").addClass("msg msg-info").html("请输入短信校验码！");
    }).blur(function () {
        var code = $(this).val();
        if (code == "") {
            $("#divVerifyCodeTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("请输入短信校验码！");
            codeStatus = false;
            return;
        }
        var phone = $("#phone").val();
        if (phone != $("#hfPhoneNumber").val()) {
            $("#divPhoneTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("请输入一致的手机号码！");
            codeStatus = false;
            return;
        }
        //验证注册邮箱是否存在
        $.ajax({
            url: $YSWL.BasePath + "Account/VerifiyCode",
            type: 'post',
            dataType: 'text',
            timeout: 10000,
            async: false,
            data: {
                Action: "post", SMSCode: code, Phone: phone
            },
            success: function (resultData) {

                if (resultData == "False") {
                    $("#divVerifyCodeTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("短信校验码不正确！");
                } else {

                   $("#divVerifyCodeTip").removeClass("msg msg-err").removeClass("msg msg-info").addClass("msg msg-ok msg-naked").html("<i class=\"msg-ico\"></i>&nbsp;");
                    codeStatus = true;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $("#divVerifyCodeTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("服务器没有返回数据，可能服务器忙，请稍候再试！");
                codeStatus = false;
            }

        });
    });

    $("#email").focus(function () {
         $("#divEmailTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-err").addClass("msg msg-info").html("请填写有效的Email地址！");
    }).keypress(function (event) {
        if (event.which == 13) {
            $("#btnEmailRegister").trigger("click");
        }
    }).blur(function () {
        CheckEmail($(this));
    });

    $("#nickname").focus(function () {
        $("#divNicknameTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-err").addClass("msg msg-info").html("请填写昵称！");
    }).keypress(function (event) {
        if (event.which == 13) {
            $("#btnEmailRegister").trigger("click");
        }
    }).blur(function () {
        CheckNickname($(this));
    });

    $("#pwd").focus(function () {
        $("#divPwdTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-err").addClass("msg msg-info").html(focusmsg );
    }).keypress(function (event) {
        if (event.which == 13) {
            $("#btnEmailRegister").trigger("click");
        }
    }).blur(function () {
        CheckPwd($(this));
    });
    $("#vpwd").focus(function () {
        $("#divVPwdTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-err").addClass("msg msg-info").html("请再次填写密码，两次输入必须一致！");
    }).keypress(function (event) {
        if (event.which == 13) {
            $("#btnEmailRegister").trigger("click");
        }
    }).blur(function () {
        CheckVPwd($(this));
    });

    $("#phone").focus(function () {
        $("#divPhoneTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-err").addClass("msg msg-info").html("请填写手机号码！");
    }).keypress(function (event) {
        if (event.which == 13) {
            $("#btnEmailRegister").trigger("click");
        }
    }).blur(function () {
        CheckPhone($(this));
    });

    $("#chkAgreement").click(function () {
        CheckAgreement($(this));
    });

    //验证推荐人
    $("#InviteUserId").focus(function () {
        $("#divTuiJianren").removeClass("msg msg-err").removeClass("msg msg-info").addClass("msg msg-ok msg-naked").html("<i class=\"msg-ico\"></i>&nbsp;");
    }).keypress(function (event) {
        if (event.which == 13) {
            $("#divTuiJianren").trigger("click");
        }
    }).blur(function () {
        CheckInvite($(this));
    });

});

function CheckRegister() {
    var regStr = $('#hfRegisterToggle').val();
    var userNameStatus;
    if (regStr == "Phone") {
        CheckPhone($("#phone"));
        userNameStatus = phoneStatus;
    } else {
        CheckEmail($("#email"));
        userNameStatus = mailStatus;
    }
    CheckNickname($("#nickname"));
    CheckPwd($("#pwd"));
    CheckVPwd($("#vpwd"));
    CheckAgreement($("#chkAgreement"));
   // CheckInvite("#InviteUserId");
    CheckInvite($("#InviteUserId"));
    if (!userNameStatus || !pwdStatus || !vpwdStatus || !nicknameStatus || !agreementStatus || !InviteStatus) {
        checkOK = false;
    } else {
        checkOK = true;
    }
    return checkOK;
}

//验证邮箱
function CheckEmail(obj) {
    var regs = /^[\w-]+(\.[\w-]+)*\@[A-Za-z0-9]+((\.|-|_)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/;
    var emailval = obj.val();
    if (emailval != "") {
        if (!regs.test(emailval)) {
            $("#divEmailTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("请填写有效的Email地址！");
            mailStatus = false;
        } else {
//验证注册邮箱是否存在
        $.ajax({
            url: $YSWL.BasePath + "Account/IsExistUserName",
            type: 'post',
            dataType: 'text',
            timeout: 10000,
            async: false,
            data: {
                Action: "post", userName: emailval
            },
            success: function (resultData) {
                if (resultData == "true") {
                    $("#divEmailTip").removeClass("msg msg-err").removeClass("msg msg-info").addClass("msg msg-ok msg-naked").html("<i class=\"msg-ico\"></i>&nbsp;");
                    mailStatus = true;
                }
                else {
                    $("#divEmailTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("该Email已存在，请使用其他Email地址。使用该地址<a href=$YSWL.BasePath + 'Account/Login'>登录</a>，忘记密码请点击<a href=$YSWL.BasePath + 'Account/FindPwd' >找回密码</a>");
                    mailStatus = false;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                ShowServerBusyTip("服务器没有返回数据，可能服务器忙，请稍候再试！");
                mailStatus = false;
            }

        });
        }
    } else {
        $("#divEmailTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("请填写有效的Email地址！");
        mailStatus = false;
    }
    return;
}
 
function CheckPhone(obj) {
    //var regs = /^1([38][0-9]|4[57]|5[^4])\d{8}$/;  13[0-9]|14[0-9]|15[0-9]|17[0-9]|18[0-9]|19[0-9]
    //var regs = /^(13[0-9]|14[0-9]|19[0-9]|15[0-9]|17[0-9]|18[0-9]|19[0-9])[0-9]{8}$/;
    var regs = /^1[0-9][0-9][0-9]{8}$/;
    var phoneval = obj.val();
    if (phoneval != "") {
        var phonevaltel = "";
        if (phoneval.length == 12) {
            phonevaltel = phoneval.substring(0, 11);
        }
        else {
            phonevaltel = phoneval;
        }
        if (!regs.test(phonevaltel)) {
            $("#divPhoneTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("请填写有效的手机号码！");
            phoneStatus = false;
        } 
        else {
            //验证手机是否存在
            $.ajax({
                url: $YSWL.BasePath + "Account/IsExistUserName",
                type: 'post',
                dataType: 'text',
                timeout: 10000,
                async: false,
                data: {
                    Action: "post", userName: phoneval
                },
                success: function (resultData) {
                    if (resultData == "true") {
                        $("#divPhoneTip").removeClass("msg msg-err").removeClass("msg msg-info").addClass("msg msg-ok msg-naked").html("<i class=\"msg-ico\"></i>&nbsp;");
                        phoneStatus = true;
                    }
                    else {
                        $("#divPhoneTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("该手机号码已被注册。使用该地址<a href='http://xxshop.lvbangtuan.com/Account/Login'>登录</a>，忘记密码请点击<a href='http://xxshop.lvbangtuan.com/Account/FindPwd' >找回密码</a>");
                        phoneStatus = false;
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    ShowServerBusyTip("服务器没有返回数据，可能服务器忙，请稍候再试！");
                    phoneStatus = false;
                }

            });
        }
    } else {
        $("#divPhoneTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("请填写手机号码！");
        phoneStatus = false;
    }
    return;
}

//验证昵称
function CheckNickname(obj) {
    var i = 0;
    var niclnamevalue = obj.val();
    if (niclnamevalue.indexOf(";") > -1 || niclnamevalue.indexOf(",") > -1 || niclnamevalue.indexOf("'") > -1) {
        ShowFailTip('大神，请您手下留情！');
        $(this).val("");
        i++;
        if (i >= 3) {
            ShowFailTip('别玩了，这样有意思吗？');
        }
        nicknameStatus = false;
        return;
    }
    if (niclnamevalue != "") {
        //验证昵称是否存在
        $.ajax({
            url:  $YSWL.BasePath +"Account/IsExistNickName" ,
            type: 'post',
            dataType: 'text',
            timeout: 10000,
            async: false,
            data: {
                Action: "post",
                nickName: niclnamevalue
            },
            success: function (resultData) {
                if (resultData == "true") {
                    $("#divNicknameTip").removeClass("msg msg-err").removeClass("msg msg-info").addClass("msg msg-ok msg-naked").html("<i class=\"msg-ico\"></i>&nbsp;");
                    nicknameStatus = true;
                } else {
                    $("#divNicknameTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("该昵称已被其他用户抢先使用，换一个试试~");
                    nicknameStatus = false;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                ShowServerBusyTip("服务器没有返回数据，可能服务器忙，请稍候再试！");
                nicknameStatus = false;
            }
        });
    } else {
        $("#divNicknameTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("请填写昵称！");
        nicknameStatus = false;
    }
    return;
}

//验证密码
function CheckPwd(obj) {
    var pwdval = obj.val();
    if (pwdval.length == 0) {
        $("#divPwdTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("请填写密码！");
        pwdStatus = false;
        return;
    }
    if (!regs.test(pwdval)) {
        $("#divPwdTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html( errormsg );
        pwdStatus = false;
    } else {
        $("#divPwdTip").removeClass("msg msg-err").removeClass("msg msg-info").addClass("msg msg-ok msg-naked").html("<i class=\"msg-ico\"></i>&nbsp;");
        pwdStatus = true;
    }
}

//验证确认密码
function CheckVPwd(obj) {
    if (obj.val() != "") {
        if (obj.val() != $("#pwd").val()) {
            $("#divVPwdTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("两次填写的不一致，请重新填写！");
            vpwdStatus = false;
        } else {
            $("#divVPwdTip").removeClass("msg msg-err").removeClass("msg msg-info").addClass("msg msg-ok msg-naked").html("<i class=\"msg-ico\"></i>&nbsp;");
            vpwdStatus = true;
        }
    } else {
        $("#divVPwdTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("请再次填写密码，两次输入必须一致！");
        vpwdStatus = false;
    }
}

//验证协议
function CheckAgreement(obj) {
    if (obj.attr("checked")) {
        $("#divAgreementTip").removeClass("msg msg-err").removeClass("msg msg-info").html("");
        agreementStatus = true;
    } else {
        $("#divAgreementTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("请先阅读并同意《用户服务协议》");
        agreementStatus = false;
    }
}



function CountDown() {
    if (smsSeconds < 0) {
        //                $("[id$='txtPhone']").removeAttr("disabled");
        isOK = true;
        $("#btnSendSMS").removeAttr("disabled").val('重新获取校验码');
        clearInterval(intervaSMS);
        //刷新图形验证码
        ChangeImageCode();
    }
    else {
        $("#btnSendSMS").attr("value", "请在(" + smsSeconds + ")秒后重新发送");
        $("#btnSendSMS").attr("disabled", "disabled");
        //                $("[id$='txtPhone']").attr("disabled", "disabled");
        isOK = false;
        smsSeconds--;
    }
}

function ChangeImageCode() {
    var myImg = document.getElementById("ImageCheck");
    myImg.src = "/ValidateCode.aspx?flag=" + new Date();
    return false;
}

///验证推荐人是否存在
function CheckInvite(obj) {
    var Inviteval = obj.val();
    if (Inviteval != "") {
        //验证推荐人是否存在
        $.ajax({
            url: $YSWL.BasePath + "Account/IsExistInvite",
            type: 'post',
            dataType: 'text',
            timeout: 10000,
            async: false,
            data: {
                Action: "post", Invite: Inviteval
            },
            success: function (resultData) {
                if (resultData.substring(1, 5) == "true") {
                    $("#divTuiJianren").removeClass("msg msg-err").removeClass("msg msg-info").addClass("msg msg-ok msg-naked").html(resultData.substring(5, resultData.length - 1));
                    InviteStatus = true;
                }
                else {
                    $("#divTuiJianren").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("该邀请人不存在。请更换邀请人");
                    InviteStatus = false;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                ShowServerBusyTip("服务器没有返回数据，可能服务器忙，请稍候再试！");
                InviteStatus = false;
            }

        });
    } else {
        // $("#divTuiJianren").removeClass("msg msg-err").removeClass("msg msg-info").addClass("msg msg-ok msg-naked").html("<i class=\"msg-ico\"></i>&nbsp;");
        // InviteStatus = true;
        $("#divTuiJianren").removeClass("msg msg-err").removeClass("msg msg-info").addClass("msg msg-err").html("请填写邀请人");
        InviteStatus = false;
    }
    return InviteStatus;
}