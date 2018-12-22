var registerType = 'Mail';
var regs = /^[A-Za-z0-9]{6,30}$/;
var focusmsg = '请填写密码（6-30位数字或字母）';
var errormsg = '密码6-30位，支持“数字、字母”';
var mailStatus = true;
var nicknameStatus = true;
var pwdStatus = true;
var codeStatus = false;
var phoneStatus = false;
var vpwdStatus = true;
var InviteStatus = true; //邀请人号

var validateOnce = {
    Email: "",
    Exists: false
};

$(function () {
    var regStr = $('#hfRegisterToggle').val(); //注册方式
    var isOpen = $("#hfSMSIsOpen").val();
    //if (regStr == 'Phone') {
    //    if (isOpen == "True") {
    //        $(".txtphone").show();
    //    }
    //}
    //注册按钮
    $("#btnEmailRegister").click(function () {
        if (regStr == 'Phone') {
            if ($('#checkCode').val()== "") {
                ShowFailTip("请输入短信校验码");
                return;
            }
            if (!codeStatus && isOpen == "True") {
                ShowFailTip("短信效验码不正确");
                return;
            }
        }
        if (CheckRegister()) {
            $('#formregister').submit();
        }
    });

    $("#btnSendSMS").click(function () {
        CheckPhone($("#username"));
 	if (phoneStatus == false) {
            return;
        }
 	var phone = $("#username").val();
        var imageCode = $('#imageCode').val();
        if (imageCode == "") {
            ShowFailTip('请输入验证码');
            return;
        }
        if (phone == "") {
            ShowFailTip('请输入手机号码');
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
                    //mailStatus = false;
                    switch (resultData.STATUS) {
                        case "SUCCESS":
                            ShowSuccessTip("发送短信成功");
                            smsSeconds = 60;
                           // alert(resultData.rand);
                            //console.log(resultData.rand);
                            $("#hfPhoneNumber").val(resultData.DATA);
                            $("#btnSendSMS").attr("value", "请在(" + smsSeconds + ")秒后重新发送");
                            intervaSMS = setInterval("CountDown()", 1000);
                            break;
                        case "PHONEISNULL": //手机号为空
                            ShowFailTip('请输入手机号码');
                            break;
                        case "IMAGECODEISINULL": //图形验证码为空
                            ShowFailTip('请输入验证码');
                            break;
                        case "IMAGECODEISEXPIRED": //图形验证码已失效
                            ShowFailTip('验证码已过期,请重新输入');
                            //刷新图形验证码
                            ChangeImageCode();
                            break;
                        case "IMAGECODEISERROR": //验证码错误
                            ShowFailTip('验证码有误,请重新输入');
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
                            ShowFailTip("服务器没有返回数据，请稍候再试！");
                        phoneStatus = false;
                            break;
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    ShowFailTip("服务器没有返回数据，请稍候再试！");
                    phoneStatus = false;
                }
            });
        }
    });

    //微信新用户绑定
    $("#btnRegBind").click(function () {
        if (CheckRegister()) {
            $(this).attr("disabled", "disabled");
            var eamil = $("#username").val();
            var pwd = $("#pwd").val();
            var nick = $("#nickname").val();
            var user = $("#txtUser").val();
            var open = $("#txtOpenId").val();
            $.ajax({
                url: $YSWL.BasePath + "Account/AjaxRegBind",
                type: 'post',
                dataType: 'text',
                timeout: 10000,
                async: false,
                data: {
                    Action: "post", UserName: eamil, UserPwd: pwd, NickName: nick, User: user, OpenId: open
                },
                success: function (resultData) {
                    if (resultData == "1") {
                        ShowSuccessTip("绑定用户成功！");
                    }
                    if (resultData == "3") {
                        ShowFailTip("该账户已经绑定了其它帐号");
                    }
                    if (resultData == "0") {
                        ShowFailTip("服务器繁忙，请稍候再试");
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    ShowServerBusyTip("服务器繁忙，请稍候再试");
                }
            });
        }
    });
  
    //查看注册协议
    $('#lookAgreement').on('click', function () {
        $('#mainPage').hide();
        $('#userAgreementPage').show();
    });

    //关闭注册协议
    $('#back_mianPage').on('click', function () {
        $('#userAgreementPage').hide();
        $('#mainPage').show(); 
    });

    //验证推荐人
    $("#InviteUserId").focus(function () {
    }).blur(function () {
        CheckInvite($(this));
    });
 
});

function CheckRegister() {
//    var isOpen = $("#hfSMSIsOpen").val();
//    if (isOpen != "True") {
//        CheckEmail($("#email"));
//    }
    var regStr = $('#hfRegisterToggle').val();
    var userNameStatus;
    if (regStr == "Phone") {
        if(!CheckPhone($("#username"))){
            userNameStatus = phoneStatus;
        }
    } else {
        if (!CheckEmail($("#username"))) {
           userNameStatus = mailStatus;
        }
    }
    if (!userNameStatus) {
        return false;
    }
    CheckNickname($("#nickname"));
    if (!nicknameStatus) {
        return false;
    }
   
    if (!CheckPwd($("#pwd"))) {
        return false;
    }
    if (!CheckVPwd($("#vpwd"))) {
        return false;
    }

    if (!CheckInvite($("#InviteUserId"))) {
        return false;
    }

    ////验证推荐人
    //$("#InviteUserId").focus(function () {
    //}).blur(function () {
    //    CheckInvite($(this));
    //});

    return true;
}

//验证邮箱
function CheckEmail(obj) {
    var regs = /^[\w-]+(\.[\w-]+)*\@[A-Za-z0-9]+((\.|-|_)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/;
    var emailval = obj.val();
    if (emailval != "") {
        if (!regs.test(emailval)) {
            ShowFailTip('请填写有效的Email地址');
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
                   mailStatus = true;
                }else {
                    ShowFailTip('该Email已被注册,换个试试');
                    mailStatus = false;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                ShowServerBusyTip("服务器没有返回数据，请稍候再试！");
                 mailStatus = false;
            }
        });
        }
    } else {
        ShowFailTip('请填写Email地址');
        mailStatus = false;
    }
 return;
}
function CheckPhone(obj) {
    var regs = /^(13[0-9]|14[0-9]|15[0-9]|17[0-9]|18[0-9]|19[0-9])[0-9]{8}$/;
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
            ShowFailTip('请填写有效的手机号码');
 phoneStatus = false;
            return;

        } else {
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
                        $("#divPhoneTip").html("");
                       phoneStatus = true;
                    }
                    else {
                        ShowFailTip('该手机号码已被注册,换个试试');
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
        ShowFailTip('请填写手机号码');
        phoneStatus = false;
    }
return;
}
//验证昵称
function CheckNickname(obj) {
    var i = 0;
    var niclnamevalue = obj.val();
    if (niclnamevalue.indexOf(";") > -1) {
        ShowFailTip('用户名不能包含“；”');
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
                   nicknameStatus = true;
                } else {
                    ShowFailTip('该昵称已被使用，换一个试试');
                    nicknameStatus = false;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                ShowServerBusyTip("服务器没有返回数据，请稍候再试！");
                nicknameStatus = false;
            }
        });
    } else {
        ShowFailTip('请填写昵称');
         nicknameStatus = false;
    }
    return;
}

//验证密码
function CheckPwd(obj) {
    var pwdval = obj.val();
    if (pwdval.length == 0) {
        ShowFailTip('请填写密码');
        return false;
    }
    if (!regs.test(pwdval)) {
        ShowFailTip(errormsg);
        return false;
    } else {
        return true;
    }
}

//验证确认密码
function CheckVPwd(obj) {
    if (obj.val() != "") {
        if (obj.val() != $("#pwd").val()) {
            ShowFailTip('两次密码不一致,请确认！');
            return false;
        } else {
            return true;
        }
    } else {
        ShowFailTip('请填写确认密码');
 return false;

    }
}
 
function CountDown() {
    if (smsSeconds < 0) {
         isOK = true;
        $("#btnSendSMS").removeAttr("disabled").val('重新获取校验码');
        clearInterval(intervaSMS);
        //刷新图形验证码
        ChangeImageCode();
    } else {
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
                if (resultData.substring(1,5) == "true") {
                    $("#divTJRAME").removeClass("msg msg-err").removeClass("msg msg-info").addClass("msg msg-ok msg-naked").html(resultData.substring(5, resultData.length - 1));
                    InviteStatus = true;
                }
                else {
                    ShowFailTip("该邀请人不存在。请更换邀请人");
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
        ShowFailTip("请填写邀请人");
        InviteStatus = false;
    }
    return InviteStatus;
}