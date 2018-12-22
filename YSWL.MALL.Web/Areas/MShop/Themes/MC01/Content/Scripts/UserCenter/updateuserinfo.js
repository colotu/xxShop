

// 验证用户昵称
function checknickname() {
    var i = 0;
    var nicknameVal = $.trim($('#txtNickName').val());
    if (nicknameVal.indexOf(";") > -1) {
        ShowFailTip("昵称不能包含“；”");
        $(this).val("");
        i++;
        if (i >= 3) {
            ShowFailTip('别玩了，这样有意思吗？');
        }
        return false;
    }
    if (nicknameVal != "") {
        //验证昵称是否存在
        var errnum = 0;
        $.ajax({
            url: $YSWL.BasePath + "u/CheckNickName",
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            async: false,
            data: {
                Action: "post",
                NickName: nicknameVal
            },
            success: function (JsonData) {
                switch (JsonData.STATUS) {
                    case "OK":
                        $("#nciknameTip").removeClass("red").addClass("tipClass").html("&nbsp;");
                        break;
                    case "EXISTS":
                        errnum++;
                        $("#nciknameTip").removeClass("tipClass").addClass("red").html("该昵称已被其它用户抢先使用，换一个试试");
                        break;
                    case "NOTNULL":
                        errnum++;
                        $("#nciknameTip").removeClass("tipClass").addClass("red").html("昵称不能为空！");
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
    } else {
        $("#nciknameTip").removeClass("tipClass").addClass("red").html("昵称不能为空！");
        return false;
    }
}

// 验证用户性别
function checksex() {
    var sex = 2;
    if ($('#personal_sex').find('span').text() == '男') {
        sex = 1;
    }
    if ($('#personal_sex').find('span').text() == '女') {
        sex = 0;
    }
    if (sex == 2) {
        ShowFailTip("请选择您的性别！");
        return false;
    } else {
        
        return true;
    }
}




//短日期，形如 (2008-08-08)
function isDate(str) {
    var r = str.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/);
    if (r == null) return false;
    var d = new Date(r[1], r[3] - 1, r[4]);
    return (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4]);
}

// 验证用户生日
function checkbirthday() {

    var birthdayVal = $.trim($('#txtBirthday').val());
    if (birthdayVal != '') {
        if (!isDate(birthdayVal)) {
            ShowFailTip("生日格式错误！");
            return false;
        }
    }
    return true;

}




function submit() {

    if (!checknickname()) {
        return false;
    }
    if (!checksex()) {
        return false;
    }
    if (!checkbirthday()) {
        return false;
    }

        var nickname = $.trim($("#txtNickName").val());
        
        var birthday = $.trim($("#txtBirthday").val());
       
        var sex = -1;

        if ($('#personal_sex').find('span').text() == '男') {
            sex = 1;
        }
        if ($('#personal_sex').find('span').text() == '女') {
            sex = 0;
        }

        $.ajax({
            url: $YSWL.BasePath + "u/UpdateUserInfo",
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            async: false,
            data: {
                Action: "post",
                NickName: nickname,
                Sex: sex,
                Birthday: birthday     
            },
            success: function (JsonData) {

                switch (JsonData.STATUS) {
                    case "SUCC":
                        ShowSuccessTip("修改成功！");
                        setTimeout('window.location.href = $YSWL.BasePath + "u/Setting";', 2000);
                        break;
                    case "FAIL":
                        ShowFailTip("修改失败！");
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
    };
