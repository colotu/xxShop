/*
* File:        maticsoft.jquery.js
* Author:      yaoyuan@ys56.com
* Copyright © 2004-2013 YSWL. All Rights Reserved.
*/
;
//设置jQuery Ajax 全局不使用缓存
$.ajaxSetup({ cache: false });
////占位替换函数
//String.prototype.format = function () {
//    if (arguments.length == 0) return this;
//    for (var s = this, i = 0; i < arguments.length; i++)
//        s = s.replace(new RegExp("\\{" + i + "\\}", "g"), arguments[i]);
//    return s;
//};

// 占位替换函数 高性能Beta版
//if (!String.prototype.format) {
String.prototype.format = function () {
    var args = arguments;
    return this.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] != 'undefined'
        ? args[number]
        : match
      ;
    });
};
//}
////计算字符串的长度（一个双字节字符长度计2，ASCII字符计1）
//String.prototype.len = function () { return this.replace(/[^\x00-\xff]/g, "aa").length; }

//截取字符串 包含中文处理，参数含义：(截取长度,是否增加…)
String.prototype.sub = function (len, hasDot) {
    var s2 = this.slice(0, len),
        i = s2.replace(/[^x00-xff]/g, "**").length;
    if (i <= len) return s2;
    var dot = hasDot ? '…' : '';
    i -= s2.length;
    switch (i) {
        case 0: return s2;
        case len: return this.slice(0, len >> 1) + dot;
        default:
            var k = len - i,
                s3 = this.slice(k, len),
                j = s3.replace(/[x00-xff]/g, "").length;
            return (j ? this.slice(0, k) + s3.sub(j) : this.slice(0, k)) + dot;
    }
};
//判断当前字符串是否以str开始
if (typeof String.prototype.startsWith != 'function') {
    String.prototype.startsWith = function (str) {
        return this.slice(0, str.length) == str;
    };
}
//判断当前字符串是否以str结束
if (typeof String.prototype.endsWith != 'function') {
    String.prototype.endsWith = function (str) {
        return this.slice(-str.length) == str;
    };
}

//获取URL参数 参数名忽略大小写
(function ($) {
    $.getUrlParam = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return decodeURIComponent(r[2]);
        return null;
    };

    (function(expCharsToEscape, expEscapedSpace, expNoStart, undefined) {
      /**
       * Modifies the given URL, returning it with the given parameter
       * changed to the given value.  The parameter is added if it didn't
       * already exist.  The parameter is removed if null or undefined is
       * specified as the value.
       * @param {string} url  The URL to be modified.
       * @param {string} paramName  The URL parameter whose value will be
       *     modified.
       * @param {string} paramValue  The value to assign.  This will be
       *     escaped using encodeURIComponent.
       * @return {string}  The updated URL.
       */
        $.setUrlParam = function(paramName, paramValue, url) {
            if (!url) url = window.location.href;
            paramValue = paramValue != undefined
                ? encodeURIComponent(paramValue).replace(expEscapedSpace, '+')
                : paramValue;
            var pattern = new RegExp(
                '([?&]'
                    + paramName.replace(expCharsToEscape, '\\$1')
                    + '=)[^&]*'
            );
            if (pattern.test(url)) {
                return url.replace(
                    pattern,
                    function($0, $1) {
                        return paramValue != undefined ? $1 + paramValue : '';
                    }
                ).replace(expNoStart, '$1?');
            } else if (paramValue != undefined) {
                return url + (url.indexOf('?') + 1 ? '&' : '?')
                    + paramName + '=' + paramValue;
            } else {
                return url;
            }
        };
    })(/([\\\/\[\]{}().*+?|^$])/g, /%20/g, /^([^?]+)&/);

    //获取当前页面名称(不带后缀名)
    $.PageName4NoEx = function () {
        var a = location.href;
        var b = a.split("/");
        var c = b.slice(b.length - 1, b.length).toString(String).split(".");
        return c.slice(0, 1);
    };

    //获取当前页面名称(带后缀名)
    $.PageName = function () {
        var strUrl = location.href;
        var arrUrl = strUrl.split("/");
        var strPage = arrUrl[arrUrl.length - 1];
        return strPage;
    };

    //获取指定URL(不带域名和参数)
    $.getUrlPart = function (level) {
        var strPage = "/" + location.href.split("?").slice(0, 1).toString().split("/").slice(3, level + 3).join("/");
        return strPage;
    };

    //获取中间URL(不带域名和参数)
    $.getUrlMiddle = function () {
        var strUrl = location.href;
        var arrUrl = strUrl.split("/");
        var strPage = "";
        for (var i = arrUrl.length - 1; i > 2; i--) {
            strPage = "/" + arrUrl[i] + strPage;
        }
        return strPage.split("?").slice(0, 1).toString(String);
    };

    //排序
    $.fn.sorted = function (customOptions) {
        /*
        $("ul li").sorted({
        by: function(v) {
        return parseInt(v.attr('data-id'));
        }
        });
        */
        var options = {
            reversed: false,
            by: function (a) { return a.text(); }
        };

        $.extend(options, customOptions);

        var $data = $(this);
        var arr = $data.get();
        arr.sort(function (a, b) {
            var valA = options.by($(a));
            var valB = options.by($(b));
            if (options.reversed) {
                return (valA < valB) ? 1 : (valA > valB) ? -1 : 0;
            } else {
                return (valA < valB) ? -1 : (valA > valB) ? 1 : 0;
            }
        });
        return $(arr);
    };
})(jQuery);

//日期格式化方法
Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(),    //day
        "h+": this.getHours(),   //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter
        "S": this.getMilliseconds() //millisecond
    };

    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }

    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
};
//日期格式化方法再次扩展 与C#统一
Date.prototype.toString = function (format, loc) {
    var time = {};
    time.Year = this.getFullYear();
    time.TYear = ("" + time.Year).substr(2);
    time.Month = this.getMonth() + 1;
    time.TMonth = time.Month < 10 ? "0" + time.Month : time.Month;
    time.Day = this.getDate();
    time.TDay = time.Day < 10 ? "0" + time.Day : time.Day;
    time.Hour = this.getHours();
    time.THour = time.Hour < 10 ? "0" + time.Hour : time.Hour;
    time.hour = time.Hour < 13 ? time.Hour : time.Hour - 12;
    time.Thour = time.hour < 10 ? "0" + time.hour : time.hour;
    time.Minute = this.getMinutes();
    time.TMinute = time.Minute < 10 ? "0" + time.Minute : time.Minute;
    time.Second = this.getSeconds();
    time.TSecond = time.Second < 10 ? "0" + time.Second : time.Second;
    time.Millisecond = this.getMilliseconds();
    time.Week = this.getDay();

    var MMMArrEn = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var MMMArr = ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"];
    var WeekArrEn = ["Sun", "Mon", "Tue", "Web", "Thu", "Fri", "Sat"];
    var WeekArr = ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"];

    var oNumber = time.Millisecond / 1000;

    if (format != undefined && format.replace(/\s/g, "").length > 0) {
        if (loc != undefined && loc == "en") {
            MMMArr = MMMArrEn.slice(0);
            WeekArr = WeekArrEn.slice(0);
        }
        format = format
            .replace(/yyyy/ig, time.Year)
            .replace(/yyy/ig, time.Year)
            .replace(/yy/ig, time.TYear)
            .replace(/y/ig, time.TYear)
            .replace(/MMM/g, MMMArr[time.Month - 1])
            .replace(/MM/g, time.TMonth)
            .replace(/M/g, time.Month)
            .replace(/dd/ig, time.TDay)
            .replace(/d/ig, time.Day)
            .replace(/HH/g, time.THour)
            .replace(/H/g, time.Hour)
            .replace(/hh/g, time.Thour)
            .replace(/h/g, time.hour)
            .replace(/mm/g, time.TMinute)
            .replace(/m/g, time.Minute)
            .replace(/ss/ig, time.TSecond)
            .replace(/s/ig, time.Second)
            .replace(/fff/ig, time.Millisecond)
            .replace(/ff/ig, oNumber.toFixed(2) * 100)
            .replace(/f/ig, oNumber.toFixed(1) * 10)
            .replace(/EEE/g, WeekArr[time.Week]);
    } else {
        format = time.Year + "-" + time.Month + "-" + time.Day + " " + time.Hour + ":" + time.Minute + ":" + time.Second;
    }
    return format;
};

/** 
* 合并两个json对象属性为一个对象 
* @param jsonbject1 
* @param jsonbject2 
* @returns resultJsonObject 
*/
$.mergeJsonObject = function (jsonbject1, jsonbject2) {
    var resultJsonObject = {};
    for (var attr in jsonbject1) {
        resultJsonObject[attr] = jsonbject1[attr];
    }
    for (var attr in jsonbject2) {
        resultJsonObject[attr] = jsonbject2[attr];
    }
    return resultJsonObject;
};

//数组去重方法
Array.prototype.distinct = function () {
    var arr = [],
        obj = {},
        i = 0,
        len = this.length,
        result;
    for (; i < len; i++) {
        result = this[i];
        if (obj[result] !== result) {
            arr.push(result);
            obj[result] = result;
        }
    }
    return arr;
};

/* 方法:Array.remove(dx) 
* 功能:删除数组元素. 
* 参数:dx删除元素的下标. 
* 返回:在原数组上修改数组 
*/
//经常用的是通过遍历,重构数组.
Array.prototype.remove = function (dx) {
    if (isNaN(dx) || dx > this.length) {
        return false;
    }
    for (var i = 0, n = 0; i < this.length; i++) {
        if (this[i] != this[dx]) {
            this[n++] = this[i];
        }
    }
    this.length -= 1;
};

//在数组中获取指定值的元素索引
Array.prototype.getIndexByValue = function (value) {
    var index = -1;
    for (var i = 0; i < this.length; i++) {
        if (this[i] == value) {
            index = i;
            break;
        }
    }
    return index;
};


$.fn.smartFloat = function () {
    var position = function (element) {
        var top = element.position().top, pos = element.css("position");
        $(window).scroll(function () {
            var scrolls = $(this).scrollTop();
            if (scrolls > top) {
                if (window.XMLHttpRequest) {
                    element.css({
                        position: "fixed",
                        top: 0
                    });
                } else {
                    element.css({
                        top: scrolls
                    });
                }
            } else {
                element.css({
                    position: pos,
                    top: top
                });
            }
        });
    };
    return $(this).each(function () {
        position($(this));
    });
};

$.fn.smartFloatIE6 = function () {
    var position = function (element) {
        var top = element.position().top,
        pos = element.css("position");
        $(window).scroll(function () {
            var scrolls = $(this).scrollTop();
            if (scrolls > top) {
                if (window.XMLHttpRequest) {
                    element.css({
                        position: "fixed",
                        top: 0
                    });
                } else {
                    element.css({
                        top: scrolls
                    });
                }
            } else {
                element.css({
                    position: "absolute",
                    top: top
                });
            }
        });
    };
    return $(this).each(function () {
        position($(this));
    });
};

//进一取整函数
function modFoat(v) {
    var _max = parseInt(v) + 1;
    if (_max - v < 1) {
        return _max;
    }
    return v;
}

(function ($) {
    //Jquery扩展截取字符串方法 性能较低 备用
    $.subStr = function (str, length) {
        var a = str.match(/[^\x00-\xff]|\w{1,2}/g);
        return a.length <= length ? str : a.slice(0, length).join("") + "…";
    };


    //导航函数, 自动对应IE6 和 Parent页面跳转
    $.navURL = function (url) {
        if (getIEVersion() > 0) {
            var tmpDocument;
            if (parent.length) {
                tmpDocument = parent.window.document;
            } else {
                tmpDocument = window.document;
            }
            var tempa = tmpDocument.createElement("a");
            tempa.href = url;
            tmpDocument.getElementsByTagName("body")[0].appendChild(tempa);
            tempa.click();
        } else {
            if (parent.length) {
                parent.window.location = url;
            } else {
                window.location = url;
            }
            //            //其它浏览器直接导航
            //            window.location = url;
        }
    };

    //导航函数 新窗口打开
    $.navURLNewWindow4A = function (url) {
        var vra = document.createElement('a');
        vra.target = '_blank';
        vra.href = url;
        document.body.appendChild(vra);
        vra.click();
    };
    //导航函数 新窗口打开
    $.navURLNewWindow4Form = function (url) {
        var formId = guidGenerator();
        var form = document.createElement('form');
        form.id = formId;
        form.action = url;
        form.method = 'get';
        form.target = '_blank';
        document.body.appendChild(form);
        form.submit();
        //        document.write('<form id="'+ formId +'" target="_blank" ' +
        //            'action="'+ url +'" method="get" ></form>');
        //        document.getElementById(formId).submit();
    };


    $.LockBodyScroll = function () {
        $('boby').attr('scroll', 'no');
    };

    $.UnlockBodyScroll = function () {
        $('boby').attr('scroll', 'auto');
    };

    $.LockKey = function () {
        //禁用右键、文本选择功能、复制按键  
        $(document).bind('contextmenu', function () { return false; });
        $(document).bind('selectstart', function () { return false; });
        $(document).keydown(function () { return loclkey(arguments[0]); });
        return 'OK';
    };

})(jQuery);

function getIEVersion() {
    var rv = -1; // Return value assumes failure.
    if (navigator.appName == "Microsoft Internet Explorer") {
        var ua = navigator.userAgent;
        var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
        if (re.exec(ua) != null)
            rv = parseFloat(RegExp.$1);
    }
    return rv;
}

//按键时提示警告  
function loclkey(e) {
    var keynum;
    if (window.event) {
        keynum = e.keyCode; // IE  
    } else if (e.which) {
        keynum = e.which; // Netscape/Firefox/Opera  
    }
    if (keynum == 17) {
        //alert('禁止复制内容！');  
        return false;
    }

    if ((window.event.altKey) &&
      ((window.event.keyCode == 37) ||            //屏蔽Alt+方向键←     
      (window.event.keyCode == 39))) {            //屏蔽Alt+方向键→  
        //        alert("不准你使用ALT+方向键前进或后退网页！");
        event.returnValue = false;
    } if (//(event.keyCode == 8) ||                    //屏蔽退格删除键      
    //(event.keyCode == 116) ||                   //屏蔽F5刷新键     
      (event.ctrlKey && event.keyCode == 82)) {   //Ctrl+R     
        event.keyCode = 0;
        event.returnValue = false;
    }
    if (event.keyCode == 122) { event.keyCode = 0; event.returnValue = false; }    //屏蔽F11     
    if (event.ctrlKey && event.keyCode == 78) event.returnValue = false;      //屏蔽Ctrl+n     
    if (event.shiftKey && event.keyCode == 121) event.returnValue = false;    //屏蔽shift+F10     
    if (window.event.srcElement.tagName == "A" && window.event.shiftKey)
        window.event.returnValue = false;       //屏蔽shift加鼠标左键新开一网页     
    if ((window.event.altKey) && (window.event.keyCode == 115)) {             //屏蔽Alt+F4      
        window.showModelessDialog("about:blank", "", "dialogWidth:1px;dialogheight:1px");
        return false;
    }
}


//判断输入的日期是否正确
function CheckDate(INDate) {
    if (INDate == "") { return true; }
    subYY = INDate.substr(0, 4)
    if (isNaN(subYY) || subYY <= 0) {
        return true;
    }
    //转换月份
    if (INDate.indexOf('-', 0) != -1) { separate = "-" }
    else {
        if (INDate.indexOf('/', 0) != -1) { separate = "/" }
        else { return true; }
    }
    area = INDate.indexOf(separate, 0)
    subMM = INDate.substr(area + 1, INDate.indexOf(separate, area + 1) - (area + 1))
    if (isNaN(subMM) || subMM <= 0) { return true; }
    if (subMM.length < 2) { subMM = "0" + subMM }
    //转换日
    area = INDate.lastIndexOf(separate)
    subDD = INDate.substr(area + 1, INDate.length - area - 1)
    if (isNaN(subDD) || subDD <= 0) {
        return true;
    }
    if (eval(subDD) < 10) { subDD = "0" + eval(subDD) }
    NewDate = subYY + "-" + subMM + "-" + subDD
    if (NewDate.length != 10) { return true; }
    if (NewDate.substr(4, 1) != "-") { return true; }
    if (NewDate.substr(7, 1) != "-") { return true; }
    var MM = NewDate.substr(5, 2);
    var DD = NewDate.substr(8, 2);
    if ((subYY % 4 == 0 && subYY % 100 != 0) || subYY % 400 == 0) { //判断是否为闰年
        if (parseInt(MM) == 2) {
            if (DD > 29) { return true; }
        }
    } else {
        if (parseInt(MM) == 2) {
            if (DD > 28) { return true; }
        }
    }
    var mm = new Array(1, 3, 5, 7, 8, 10, 12); //判断每月中的最大天数
    var flag = false;
    for (i = 0; i < mm.length; i++) {
        if (parseInt(MM, 10) == mm[i]) { flag = true; }
    }
    if (flag == true) {
        if (parseInt(DD) > 31) { return true; }
    } else {
        if (parseInt(DD) > 30) { return true; }
    }

    if (parseInt(MM) > 12) { return true; }
    return false;
}

function guidGenerator() {
    var S4 = function () {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    };
    return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
}

$.fn.DisableCtrlV = function () {
    $(this).keydown(function (e) {
        // 注意此处不要用keypress方法，否则不能禁用　Ctrl+V 与　Ctrl+V,具体原因请自行查找keyPress与keyDown区分，十分重要，请细查
        if ($.browser.msie) { // 判断浏览器
            if (((event.keyCode > 47) && (event.keyCode < 58)) || (event.keyCode == 8)) { // 判断键值  
                return true;
            } else {
                return false;
            }
        } else {
            if (((e.which > 47) && (e.which < 58)) || (e.which == 8) || (event.keyCode == 17)) {
                return true;
            } else {
                return false;
            }
        }
    }).focus(function () {
        this.style.imeMode = 'disabled'; // 禁用输入法,禁止输入中文字符
        // imeMode有四种形式，分别是：
        // active 代表输入法为中文
        // inactive 代表输入法为英文
        // auto 代表打开输入法 (默认)
        // disable 代表关闭输入法
    });
};

$.fn.OnlyNum = function() {
    $(this).live('keydown', function(event) {
        if (event.ctrlKey) return true;
        //        if (isNaN(event.value)) event.execCommand('undo'); if (event.keyCode == 32) event.execCommand('undo');
        //                return (event.keyCode >= 48 && event.keyCode <= 57);

        //8：退格键、9：Tab、46：delete、37-40： 方向键
        //48-57：小键盘区的数字、96-105：主键盘区的数字
        //110、190：小键盘区和主键盘区的小数
        //189、109：小键盘区和主键盘区的负号
        var e = e || window.event; //IE、FF下获取事件对象
        var cod = e.charCode || e.keyCode; //IE、FF下获取键盘码

        //        if ((cod != 8 && cod != 46 && (cod > 40) && (cod > 57) && (cod > 105)) || e.shiftKey) notValue(e);
        if (cod != 8 && cod != 9 && cod != 46 && (cod < 37 || cod > 40) && (cod < 48 || cod > 57) && (cod < 96 || cod > 105)) {
            e.preventDefault ? e.preventDefault() : e.returnValue = false;
            $(this).show('highlight', 150);
        }

    }).live('focus', function() {
        this.style.imeMode = 'disabled'; // 禁用输入法,禁止输入中文字符
        // imeMode有四种形式，分别是：
        // active 代表输入法为中文
        // inactive 代表输入法为英文
        // auto 代表打开输入法 (默认)
        // disable 代表关闭输入法
    }).live('blur', function() {
        $(this).val($(this).val().replace(/[^\d]/g, ''));
    });
    if (getIEVersion() > 0) {
        //IE
        $(this).bind('beforepaste', function(event) {
            clipboardData.setData('text', clipboardData.getData('text').replace(/[^\d]/g, ''));
        });
    } else {
//            //Chrome FireFox 4 ZeroClipboard BEN MODIFY 20131209
//            $(this).bind('paste', function (event) {
//                event.clipboardData.getData("text/plain").replace(/[^\d]/g, '');
//            });
    }
};


$.fn.OnlyFloat = function() {
    $(this).live('keydown', function(event) {
        if (event.ctrlKey) return true;
        //        var key = e.keyCode;
        //        if (key >= 48 || key <= 57) {
        //            return true;
        //        } else if (key == 46 && ($(this).val().indexOf(".") == -1 && $(this).val().length > 0)) {
        //            return true;
        //        }
        //        return false;
        //8：退格键、46：delete、37-40： 方向键
        //48-57：小键盘区的数字、96-105：主键盘区的数字
        //110、190：小键盘区和主键盘区的小数
        //189、109：小键盘区和主键盘区的负号
        var e = e || window.event; //IE、FF下获取事件对象
        var cod = e.charCode || e.keyCode; //IE、FF下获取键盘码
        //小数点处理
        if (cod == 110 || cod == 190) {
            ($(this).val().indexOf(".") >= 0 || !$(this).val().length) && notValue(this, e);
        } else {
            //            if ((cod != 8 && cod != 46 && (cod > 40) && (cod > 57) && (cod > 105)) || e.shiftKey) notValue(this,e);
            if (cod != 8 && cod != 9 && cod != 46 && (cod < 37 || cod > 40) && (cod < 48 || cod > 57) && (cod < 96 || cod > 105)) notValue(this, e);
        }

        function notValue(sender, e) {
            e.preventDefault ? e.preventDefault() : e.returnValue = false;
            $(sender).show('highlight', 150);
        }
    }).live('focus', function() {
        this.style.imeMode = 'disabled'; // 禁用输入法,禁止输入中文字符
        // imeMode有四种形式，分别是：
        // active 代表输入法为中文
        // inactive 代表输入法为英文
        // auto 代表打开输入法 (默认)
        // disable 代表关闭输入法
    }).live('blur', function() {
        $(this).val($(this).val().replace(/[^\d.]/g, '').replace(/^\./g, ''));
    });

    if (getIEVersion() > 0) {
        //IE
        $(this).bind('beforepaste', function(event) {
            clipboardData.setData('text', clipboardData.getData('text').replace(/(^[0-9]([.][0-9]{1,2})?$)|(^1[0-9]([.][0-9]{1,2})?$)|(^2[0-3]([.][0-9]{1,2})?$)|(^24([.]0{1,2})?$)/g, ''));
        });
    } else {
//            //Chrome FireFox 4 ZeroClipboard BEN MODIFY 20131209
//            $(this).bind('paste', function (event) {
//                event.clipboardData.getData("text/plain").replace(/(^[0-9]([.][0-9]{1,2})?$)|(^1[0-9]([.][0-9]{1,2})?$)|(^2[0-3]([.][0-9]{1,2})?$)|(^24([.]0{1,2})?$)/g, '');
//            });
    }
};

////禁用右键、文本选择功能、复制按键  
//$(document).bind("contextmenu", function () { return false; });
//$(document).bind("selectstart", function () { return false; });
//$(document).keydown(function () { return key(arguments[0]) });

////按键时提示警告  
//function key(e) {
//    var keynum;
//    if (window.event) // IE  
//    {
//        keynum = e.keyCode;
//    }
//    else if (e.which) // Netscape/Firefox/Opera  
//    {
//        keynum = e.which;
//    }
//    if (keynum == 17) { alert("禁止复制内容！"); return false; }
//}



////屏蔽鼠标右键、Ctrl+N、Shift+F10、F11、F5刷新、退格键       
//function document.oncontextmenu() { event.returnValue = false; } //屏蔽鼠标右键     
//function window.onhelp() { return false }       //屏蔽F1帮助     
//function document.onkeydown() {
//    if ((window.event.altKey) &&
//      ((window.event.keyCode == 37) ||            //屏蔽Alt+方向键←     
//      (window.event.keyCode == 39))) {            //屏蔽Alt+方向键→  
//        alert("不准你使用ALT+方向键前进或后退网页！");
//        event.returnValue = false;
//    } if ((event.keyCode == 8) ||                    //屏蔽退格删除键      
//      (event.keyCode == 116) ||                   //屏蔽F5刷新键     
//      (event.ctrlKey && event.keyCode == 82)) {   //Ctrl+R     
//        event.keyCode = 0;
//        event.returnValue = false;
//    }
//    if (event.keyCode == 122) { event.keyCode = 0; event.returnValue = false; }    //屏蔽F11     
//    if (event.ctrlKey && event.keyCode == 78) event.returnValue = false;      //屏蔽Ctrl+n     
//    if (event.shiftKey && event.keyCode == 121) event.returnValue = false;    //屏蔽shift+F10     
//    if (window.event.srcElement.tagName == "A" && window.event.shiftKey)
//        window.event.returnValue = false;       //屏蔽shift加鼠标左键新开一网页     
//    if ((window.event.altKey) && (window.event.keyCode == 115)) {             //屏蔽Alt+F4      
//        window.showModelessDialog("about:blank", "", "dialogWidth:1px;dialogheight:1px");
//        return false;
//    }
//}     






//// 快捷键响应   
//// targetObj: 目标对象，如果满足快捷键条件，触发目标对象的click事件  
//// ctrlKey: 是否按住了Ctrl组合键  
//// shiftKey: 是否按住了Shift组合键  
//// altKey: 是否按住了Alt组合键  
//// keycode: 按键对应的数值  
//function Hotkey(event, targetObj, ctrlKey, shiftKey, altKey, keycode){  
//if (  
//   targetObj  
//   && event.ctrlKey == ctrlKey   
//   && event.shiftKey == shiftKey   
//   && event.altKey == altKey   
//   && event.keyCode == keycode  
//   )  
//   targetObj.click();  
//}  
//  
//function fnKeyup(event)  
//{  
//var b = document.getElementById("myButton");  
//Hotkey(event, b, true, false, false, 13);  
//}  
//  
//// 捕获系统的Keyup事件  
//// 如果是Mozilla系列浏览器  
//if (document.addEventListener)  
//document.addEventListener("keyup",fnKeyup,true);  
//else  
//document.attachEvent("onkeyup",fnKeyup);  
//  
////-->

jQuery.Hashtable = function () {
    this.items = new Array();
    this.itemsCount = 0;
    this.add = function (key, value) {
        if (!this.containsKey(key)) {
            this.items[key] = value;
            this.itemsCount++;
        } else { //lee change this, allow overwrite
            this.items[key] = value;
        }
        //throw "key '"+key+"' allready exists."
    };
    this.get = function (key) {
        if (this.containsKey(key))
            return this.items[key];
        else
            return null;
    };

    this.remove = function (key) {
        if (this.containsKey(key)) {
            delete this.items[key];
            this.itemsCount--;
        } else
            throw "key '" + key + "' does not exists.";
    };
    this.containsKey = function (key) {
        return typeof (this.items[key]) != "undefined";
    };
    this.containsValue = function containsValue(value) {
        for (var item in this.items) {
            if (this.items[item] == value)
                return true;
        }
        return false;
    };
    this.contains = function (keyOrValue) {
        return this.containsKey(keyOrValue) || this.containsValue(keyOrValue);
    };
    this.clear = function () {
        this.items = new Array();
        itemsCount = 0;
    };
    this.size = function () {
        return this.itemsCount;
    };
    this.length = function () {
        return this.itemsCount;
    };
    this.isEmpty = function () {
        return this.size() == 0;
    };
};
