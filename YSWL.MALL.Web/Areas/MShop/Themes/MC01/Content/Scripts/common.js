
//获取购物车商品数量
function getCartCount($el) {
    $.ajax({
        type: 'Get',
        url: $YSWL.BasePath + 'ShoppingCart/GetCartCount',
        dataType: "text",
        success: function (data){
            var count = parseInt(data);
            if (count > 0){
                if (count > 99){
                    $el.text('99+').show();
                }else{
                    $el.text(count).show();
                }
            }else{
                $el.hide();
            }
        }
    });
}

//检查是否登录
var CheckUserState = function () {
    var islogin;
    $.ajax({
        url: $YSWL.BasePath + "Account/AjaxIsLogin",
        type: 'post',
        dataType: 'text',
        async: false,
        success: function (resultData) {
            if (resultData != "True") {
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

//去登陆页面
function gotoLoginPage() {
    location.href = $YSWL.BasePath + "a/l?returnUrl=" + $.getUrlMiddle();
}


//判断是否含有禁用词
function ContainsDisWords(desc) {
    var isContain = false;
    $.ajax({
        url: $YSWL.BasePath + "Partial/ContainsDisWords",
        type: 'post', dataType: 'text', timeout: 10000,
        async: false,
        data: { Desc: desc },
        success: function (resultData) {
            if (resultData == "True") {
                isContain = true;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ShowFailTip("操作失败：" + errorThrown);
        }
    });
    return isContain;
}


//页面内容超过一屏时  底部按钮hold住 content$ 内容元素    but$ button元素   hold的类名
function holdBottomButton(content$, but$,className){
    var windowH = $(window).height();
    var myContent = content$.height();
    if (windowH < myContent) {
        but$.addClass(className);
    } else {
        but$.removeClass(className);
    }
}

//获取未读消息  呈现元素
function getNotRead(element$) {
    $.ajax({
        type: "POST",
        dataType: "text",
        url: $YSWL.BasePath + "Partial/GetNotRead",
        success: function(result) {
            if (result > 0) {
                element$.addClass('hideDot');
            }
        }
    });
}