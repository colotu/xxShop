/*Msaas公共方法*/

/*新版密码强度验证*/

/*字符串长度>=8&&<=16 验证通过+1 否则直接返回0
含有大小写字母 验证通过+1
含有特殊字符 验证通过+1
含有数字 验证通过+1
*/

function pwdReg(str) {
    var regNum = /\d+/; //验证数字
    var regAlphabet = /[a-zA-Z]/;//验证字母
    var regSpecial = /^[a-zA-Z0-9]*$/;//验证特殊字符 包含为false

    var num = 0;
    if (str.length >= 8 && str.length <= 16) {
        num++;
    }
    else {
        return 0;
    }
    if (regNum.test(str)) {
        num++;
    }
    if (regAlphabet.test(str)) {
        num++;
    }
    if (!regSpecial.test(str)) {
        num++;
    }
    return num;
}

/*load状态模态框
* 并引用相应样式class
*/


function showLoad(msg) {
    if (!msg) {
        msg = '请等待...';
    }

    var templete = '<div class="modal-div" style=""><img style="width: 64px;position: relative;" src="/Areas/mshop/Themes/MC01/Content/images/loading.gif"><div style="position: static;color: #fff;text-align: center;margin: 0 auto;width: 80%;font-size: 16px;">' + msg + '</div></div>';
    $(document.body).append(templete);
    var h = Math.floor(document.documentElement.clientHeight / 2) - 64 + "px";
    var w = Math.floor(document.documentElement.clientWidth / 2) - 32 + "px";
    $(".modal-div img").css("margin-top", h);
    $(".modal-div img").css("margin-left", w);
    $(".modal-div").show();
    $('body').attr('style', 'overflow-y:hidden');
}

function showLoadNomsg() {
    var templete = '<div class="modal-div" style=""><div style="position: static;color: #fff;text-align: center;margin: 0 auto;width: 80%;font-size: 16px;"></div></div>';
    $(document.body).append(templete);
    var h = Math.floor(document.documentElement.clientHeight / 2) - 64 + "px";
    var w = Math.floor(document.documentElement.clientWidth / 2) - 32 + "px";
    $(".modal-div img").css("margin-top", h);
    $(".modal-div img").css("margin-left", w);
    $(".modal-div").show();
    $('body').attr('style', 'overflow-y:hidden');
}

function hideLoad() {
    $(".modal-div").hide();
    $(".modal-div").remove();
    $('body').attr('style', 'overflow-y:');
}

/*验证码*/
function ChangeImageCode() {
    $("#imgRandom").attr("src", "/ValidateCode.aspx?id=" + Math.random());
}

/*按钮状态*/
//启用
function enableBtn() {
    $(".btn_state").removeClass("btn_disabled");
    $(".btn_state").addClass("btn_active");
}
//禁用
function disableBtn() {
    $(".btn_state").removeClass("btn_active");
    $(".btn_state").addClass("btn_disabled"); 
}

//msg传递提示信息
//需在使用界面实现success()和fail()函数,并传入相对的函数名称
function modelSubmit(msg, title, button, success, fail) {
    if (!title) {
        title = '确定';
    }
    if (!button) {
        button = '确定';
    }
    if (!success) {
        success = 'success';
    }
    if (!fail) {
        fail = 'fail';
    }
    showLoadNomsg();
    //var tempstr =
    //    '<div id="modal-window" style="z-index: 1011;position: absolute;top: 50px;left: 30px;background: white;width: 250px;height: 187px;"><div style="text-align: center;margin-top: 15px;">' + msg + '</div><div ="margin-top: 30px;text-align: center;"><input type="button" value="确认" onclick="' + success + '();" style="border-color: blue;border: 1px solid;margin: 20px;" class="btn_blue btn_default_s"><input class="btn_blue btn_default_s" type="button" value="取消" onclick="' + fail + '();"></div></div>';
    var tempstr =
        '<div class="dialog-overlay"><div class="dialog"><div class="dialog-popup-title"><a href="javascript:void(0);" onclick="' + fail + '();" class="pay-close">x</a><span>' + title + '</span></div><div class="dialog-cont">' + msg + '<div class="btn_box"><a class="btn_default btn_active" onclick="' + success + '();">' + button + '</a></div></div></div></div>';
    $(document.body).append(tempstr);

    
}

//应用支付
function payWindow(name, price, success, fail) {
    var chilTemp = '<p class="name">' + name + '</p><p class="price">￥' + price + '</p><div class="pay-way"><span>余额支付</span></div>';
    modelSubmit(chilTemp, '支付', '支付', success, fail);
}


 
