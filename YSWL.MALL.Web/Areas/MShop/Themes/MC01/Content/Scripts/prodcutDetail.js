$(function () {
    $('#productDetail').css('min-height', $(window).height() - 84);

    $('#body').addClass('m_home');

    //向商品详情首页添加一条评论
    var commtStr = $('#comment_list .personDetail:eq(0)');
    if (commtStr.length > 0) {
        $('#detailEvaluation').append('<div class="personDetail" >' + commtStr.html() + '</div>');
    }

    //商品详情图片
    $.scaleLoad('#prodcutdetail', $(window).width(), 1280);

    //商品图片滑动  2张或2张以上才可以滑动
    if ( parseInt( $('#swiper1').attr('count'))>1) {
        var mySwiper = new Swiper('#swiper1', {
            direction: 'horizontal',
            loop: true,
            pagination: '.swiper-pagination',
            paginationType: 'fraction',
            autoplay: 5000
        });
    }

    //选项卡切换
    myTabsSwiper();

    //点击加入购物车 显示层
    $("#addShopping").on('click', function () {
        $("html,body").addClass("nooverflow");
        if (!$(this).hasClass('addShopping')) {//已置灰，不能购买
            return;
        }
        $("#bg").css({height: $(document).height()}).show();
        $(".specificationWrapper").show();
    });

    //促销活动  收起展示切换
    $("#activ_preMore").on('click', function () {
        if ($("#activityList .showTxt").is(":visible") == true) {//是否是显示状态
            $("#activityList .showTxt").hide();
            $("#activityList .showTxt2").show();
            $(this).addClass("preMore-bp2");
            $(this).removeClass("preMore-bp");
            $("#swiperMb").addClass('mb-c'); 
            $('#swiperMb').css({ height: $('.productIndex').height()});
        } else {
            $("#activityList .showTxt").show();
            $("#activityList .showTxt2").hide();
            $(this).addClass("preMore-bp");
            $(this).removeClass("preMore-bp2");
            $("#swiperMb").removeClass('mb-c');
            //alert(2222);
            $('#swiperMb').css({ height: ($('.productIndex').height() + 44 )});
        }
    });

    //批发优惠 收起展示切换
    $("#whole_preMore").on('click',function () {
        if ($("#wholeList .showTxt").is(":visible") == true) {//是否是显示状态
            $("#wholeList .showTxt").hide();
            $("#wholeList .showTxt2").show();
            $(this).addClass("preMore-bp2");
            $(this).removeClass("preMore-bp");
            $("#swiperMb").addClass('mb-c');
            $('#swiperMb').css({ height: $('.productIndex').height() });
        } else {
            $("#wholeList .showTxt").show();
            $("#wholeList .showTxt2").hide();
            $(this).addClass("preMore-bp");
            $(this).removeClass("preMore-bp2");
            $("#swiperMb").removeClass('mb-c');
            $('#swiperMb').css({ height: ($('.productIndex').height() + 44) });
        }
    });

    //详情页   介绍、规格参数切换
    $('#prodcutdetailTab li').on('click', function () {
        if ($(this).hasClass('select')) {
            return;
        }       
        if ($(this).index() === 1) {
            var hh = $('.floor').height() + $('#attributes_cont').outerHeight() + 180;                  
            $('#swiperMb').css({ height: hh });           
        } else {
            var hh = $('.floor').height() + $('#prodcutdetail').height() + 120;
            $('#swiperMb').css({ height: hh });
       } 
        $(this).addClass('select').siblings().removeClass('select');
        $('#prodcutdetail,#attributes_cont').hide();
        $('#' + $(this).attr('item')).show();
    });

    //收藏操作
    $("#btnProductFav").click(function () {
        if (!CheckUserState()) {
            //未登录  跳转到登陆页 
            gotoLoginPage();
            return;
        }
        var productId = $("#hdProductId").val();
        if ($(this).hasClass('collect')) {//已收藏
            //取消收藏
            $.ajax({
                type: "POST",
                dataType: "text",
                url: $YSWL.BasePath + "u/DelFav",
                async: false,
                data: { pId: productId ,type:1},
                success: function (data) {
                   if (data == "True") {
                       $("#btnProductFav").removeClass('collect').addClass('collect2');
                        ShowSuccessTip('取消成功');
                    } else if (data == "False") {
                        ShowFailTip('取消失败，请稍候再试！');
                    } else {
                        ShowFailTip('服务器繁忙，请稍候再试！');
                    }
                }
            });
        } else {
            //添加收藏
            $.ajax({
                type: "POST",
                dataType: "text",
                url: $YSWL.BasePath + "u/AjaxAddFav",
                async: false,
                data: { ProductId: productId },
                success: function (data) {
                    if (data == "Rep") {
                        $("#btnProductFav").removeClass('collect2').addClass('collect');
                        ShowFailTip('已收藏，请不要重复收藏');
                    } else if (data == "True") {
                        $("#btnProductFav").removeClass('collect2').addClass('collect');
                        ShowSuccessTip('收藏成功');
                    } else {
                        ShowFailTip('服务器繁忙，请稍候再试！');
                    }
                }
            });
        }
    });

    //获取购物车商品数量
    getCartCount($('#shoppingCount'));

    GetPvCount($("#hdProductId").val());

    //是否添加过收藏
    IsAddedFav($("#hdProductId").val());

    //关闭
    $("#bg").on('click', function () {
        $("#bg,.specificationWrapper").hide();
        $("html,body").removeClass("nooverflow");
    });

    //关闭
    $(".close").on('click', function () {
        $("#bg,.specificationWrapper").hide();
        $("html,body").removeClass("nooverflow");
    });

    //加
    $(document).on('click', '#plus', function () {
        var count = parseInt($("#productCount").val()) + 1;
        $("#productCount").val(count);
    });

    //减
    $(document).on('click', '#subtract', function () {
        var count = parseInt($("#productCount").val());
        if (count > 1) {
            count = count - 1;
        }
        $("#productCount").val(count);
    });


    // 点击 无货图片时 提示无货
    $(".none").on("click", function () {
        $(".no-shop").css({
            left: ($(window).width() - $(".no-shop").width()) / 2 + "px",
            top: ($(window).height() - $(".no-shop").height()) / 2 + $(window).scrollTop() + "px"
        });
        $(".no-shop").fadeIn(200);
        setTimeout(function () {
            $(".no-shop").fadeOut(500);
        }, 2000);
    });


    //点击已选择 同点击底部加入购物车 作用一致
    $('#divSelectInfo').click(function () {
        $('#addShopping').click();
    });

});

//加密用户名
function encryption(userNameClassName) {
    $('.' + userNameClassName).each(function () {
        var self = $(this);
        var self_length = self.text().trim().length;
        var self_text = self.text().trim();
        if (self_length >= 7) {
            self.text(self_text.substring(0, 3) + "****" + self_text.substring(7, self_length));
        } else if (self_length > 3) {
            self.text(self_text.substring(0, 2) + "****");
        } else {
            self.text('****');
        }
        $(this).removeClass(userNameClassName);
        self.show();
    });
}

function showAddCartSuccessTip() {
    //显示加入购物车提示
    $(".addSuccess")
        .css({
            //设置弹出层距离左边的位置
            left: ($(window).width() - $(".addSuccess").width()) / 2 + "px",
            //设置弹出层距离上面的位置
            top: ($(window).height() - $(".addSuccess").height()) / 2 + $(window).scrollTop() + "px"
            //,display: "block"
        });
    $(".addSuccess").fadeIn(200);
    setTimeout(function () {
        $(".addSuccess").fadeOut(500);
    },
        2000);
}

function swiperMbHeight() {
    //选择规格层 加入购物车
    if ($(".already").is(":visible") === true) {
        $('#swiperMb').css({ height: ($('.productIndex').height() + 100) });
    } else {
        $('#swiperMb').css({ height: $('.productIndex').height() });
    }
}
function GetPvCount(pid) {
    $.ajax({
        url: $YSWL.BasePath + "Product/GetPvCount",
        type: 'post',
        dataType: 'json',
        timeout: 10000,
        data: {
            pid: pid
        },
        success: function (jsondata) {
            if (jsondata.STATUS == "SUCCESS") {
                //jsondata.DATA; 将访问数展示到页面上  预留 
            }
        },
        error: function (messsage) {
        }
    });
};

//是否添加过收藏
function IsAddedFav(productId) {
    if (!CheckUserState()) {
        //未登录 返回
        return;
    }
    $.ajax({
        type: "POST",
        dataType: "text",
        url: $YSWL.BasePath + "u/IsAddedFav",
        async: false,
        data: { id: productId,type:1 },
        success: function (data) {
            if (data == "True") {
                $('#btnProductFav').addClass('collect').removeClass('collect2');
            } else {
                $('#btnProductFav').removeClass('collect').addClass('collect2');
            }
        }
    });
}

//点击标题切换内容
function myTabsSwiper() {
    var tabsSwiper = new Swiper('#tabs-container', {
        scrollbar: null,
        freeMode: false,
        freeModeMomentum: true,
        scrollbarSnapOnRelease: false,
        autoHeight: true,
        speed: 500,
        onSlideChangeStart: function () {
            window.scrollTo(0, 0);
            $("#detailTitleTab .act").removeClass('act');
            $("#detailTitleTab li").eq(tabsSwiper.activeIndex).addClass('act');
            $("#detailTitleTab li").eq(tabsSwiper.activeIndex).siblings().removeClass('act');
        }
    });
    $("#detailTitleTab li").on('touchstart mousedown', function (e) {
        e.preventDefault();
        $("#detailTitleTab .act").removeClass('act');
        $(this).addClass('act');
        tabsSwiper.slideTo($(this).index());
    });
    $("#detailTitleTab li").on("click", function (e) {
        e.preventDefault()
    });

    //查看详情
    $("#upTo").on("click", function () {
        $("#detailTitleTab li").eq(1).addClass('act').siblings().removeClass('act');
        tabsSwiper.slideTo(1);
    });


    //查看评价
    $("#moreComment").on('click', function () {
        $("#detailTitleTab li").eq(2).addClass('act').siblings().removeClass('act');
        tabsSwiper.slideTo(2);
    });

}

function pullUpAction() {
    setTimeout(function () {
        var tabsSwiper = new Swiper('#tabs-container', {
            scrollbar: null,
            freeMode: false,
            freeModeMomentum: true,
            scrollbarSnapOnRelease: false,
            autoHeight: true,
            speed: 500,
            onSlideChangeStart: function () {
                window.scrollTo(0, 0);
                $(".tabs1 .active").removeClass('act');
                $(".tabs1 li").eq(tabsSwiper.activeIndex).addClass('act');
                $(".tabs1 li").eq(tabsSwiper.activeIndex).siblings().removeClass('act');
            }
        });
        $(".tabs1 li").on('touchstart mousedown', function (e) {
            e.preventDefault();
            $(".tabs1 .act").removeClass('act');
            $(this).addClass('act');
            tabsSwiper.slideTo($(this).index());
        });
        var x = $(".tabs1 li").eq(1).index();
        $(".tabs1 .act").removeClass('act');
        $(".tabs1 li").eq(1).addClass('act');
        tabsSwiper.slideTo(x);
        myScroll.refresh();
    }, 1000);
}

//上拉查看商品详情
function loaded() {
    pullUpEl = document.getElementById('pullUp');
    proWrapperEl = document.getElementById('pro-wrapper');
    pullUpOffset = pullUpEl.offsetHeight;
    myScroll = new iScroll('pro-wrapper', {
        scrollbarClass: 'myScrollbar',
        useTransition: false,
        onRefresh: function () {
            if (pullUpEl.className.match('loading')) {
                pullUpEl.className = '';
                pullUpEl.querySelector('.pullUpLabel').innerHTML = '上拉查看图文详情';
                proWrapperEl.style.overflow = 'auto';
            }
        },
        onScrollMove: function () {
            if (this.y < (this.maxScrollY - 5) && !pullUpEl.className.match('flip')) {
                pullUpEl.className = 'flip';
                pullUpEl.querySelector('.pullUpLabel').innerHTML = '松手开始更新...';
                this.maxScrollY = this.maxScrollY;
                proWrapperEl.style.overflow = 'auto';
            } else if (this.y > (this.maxScrollY + 5) && pullUpEl.className.match('flip')) {
                pullUpEl.className = '';
                this.maxScrollY = pullUpOffset;
                proWrapperEl.style.overflow = 'auto';
            }
        },
        onScrollEnd: function () {
            if (pullUpEl.className.match('flip')) {
                pullUpEl.className = 'loading';
                pullUpEl.querySelector('.pullUpLabel').innerHTML = '加载中...';
                proWrapperEl.style.overflow = 'auto';
                pullUpAction(); // Execute custom function (ajax call?)
            }
        }
    });
    setTimeout(function () { document.getElementById('pro-wrapper').style.left = '0%'; }, 800);
}

//初始化绑定iScroll控件
document.addEventListener('DOMContentLoaded', loaded, false);