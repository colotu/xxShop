/*提示框1*/
window.ZENG = window.ZENG || {};

ZENG.dom = {
    getById: function (id) {
        return document.getElementById(id);
    }, get: function (e) {
        return (typeof (e) == "string") ? document.getElementById(e) : e;
    }, createElementIn: function (tagName, elem, insertFirst, attrs) {
        var _e = (elem = ZENG.dom.get(elem) || document.body).ownerDocument.createElement(tagName || "div"), k;
        if (typeof (attrs) == 'object') {
            for (k in attrs) {
                if (k == "class") {
                    _e.className = attrs[k];
                } else if (k == "style") {
                    _e.style.cssText = attrs[k];
                } else {
                    _e[k] = attrs[k];
                }
            }
        }
        insertFirst ? elem.insertBefore(_e, elem.firstChild) : elem.appendChild(_e);
        return _e;
    }, getStyle: function (el, property) {
        el = ZENG.dom.get(el);
        if (!el || el.nodeType == 9) {
            return null;
        }
        var w3cMode = document.defaultView && document.defaultView.getComputedStyle, computed = !w3cMode ? null : document.defaultView.getComputedStyle(el, ''), value = "";
        switch (property) {
            case "float":
                property = w3cMode ? "cssFloat" : "styleFloat";
                break;
            case "opacity":
                if (!w3cMode) {
                    var val = 100;
                    try {
                        val = el.filters['DXImageTransform.Microsoft.Alpha'].opacity;
                    } catch (e) {
                        try {
                            val = el.filters('alpha').opacity;
                        } catch (e) {
                        }
                    }
                    return val / 100;
                } else {
                    return parseFloat((computed || el.style)[property]);
                }
                break;
            case "backgroundPositionX":
                if (w3cMode) {
                    property = "backgroundPosition";
                    return ((computed || el.style)[property]).split(" ")[0];
                }
                break;
            case "backgroundPositionY":
                if (w3cMode) {
                    property = "backgroundPosition";
                    return ((computed || el.style)[property]).split(" ")[1];
                }
                break;
        }
        if (w3cMode) {
            return (computed || el.style)[property];
        } else {
            return (el.currentStyle[property] || el.style[property]);
        }
    }, setStyle: function (el, properties, value) {
        if (!(el = ZENG.dom.get(el)) || el.nodeType != 1) {
            return false;
        }
        var tmp, bRtn = true, w3cMode = (tmp = document.defaultView) && tmp.getComputedStyle, rexclude = /z-?index|font-?weight|opacity|zoom|line-?height/i;
        if (typeof (properties) == 'string') {
            tmp = properties;
            properties = {};
            properties[tmp] = value;
        }
        for (var prop in properties) {
            value = properties[prop];
            if (prop == 'float') {
                prop = w3cMode ? "cssFloat" : "styleFloat";
            } else if (prop == 'opacity') {
                if (!w3cMode) {
                    prop = 'filter';
                    value = value >= 1 ? '' : ('alpha(opacity=' + Math.round(value * 100) + ')');
                }
            } else if (prop == 'backgroundPositionX' || prop == 'backgroundPositionY') {
                tmp = prop.slice(-1) == 'X' ? 'Y' : 'X';
                if (w3cMode) {
                    var v = ZENG.dom.getStyle(el, "backgroundPosition" + tmp);
                    prop = 'backgroundPosition';
                    typeof (value) == 'number' && (value = value + 'px');
                    value = tmp == 'Y' ? (value + " " + (v || "top")) : ((v || 'left') + " " + value);
                }
            }
            if (typeof el.style[prop] != "undefined") {
                el.style[prop] = value + (typeof value === "number" && !rexclude.test(prop) ? 'px' : '');
                bRtn = bRtn && true;
            } else {
                bRtn = bRtn && false;
            }
        }
        return bRtn;
    }, getScrollTop: function (doc) {
        var _doc = doc || document;
        return Math.max(_doc.documentElement.scrollTop, _doc.body.scrollTop);
    }, getClientHeight: function (doc) {
        var _doc = doc || document;
        return _doc.compatMode == "CSS1Compat" ? _doc.documentElement.clientHeight : _doc.body.clientHeight;
    }
};

ZENG.string = {
    RegExps: { trim: /^\s+|\s+$/g, ltrim: /^\s+/, rtrim: /\s+$/, nl2br: /\n/g, s2nb: /[\x20]{2}/g, URIencode: /[\x09\x0A\x0D\x20\x21-\x29\x2B\x2C\x2F\x3A-\x3F\x5B-\x5E\x60\x7B-\x7E]/g, escHTML: { re_amp: /&/g, re_lt: /</g, re_gt: />/g, re_apos: /\x27/g, re_quot: /\x22/g }, escString: { bsls: /\\/g, sls: /\//g, nl: /\n/g, rt: /\r/g, tab: /\t/g }, restXHTML: { re_amp: /&amp;/g, re_lt: /&lt;/g, re_gt: /&gt;/g, re_apos: /&(?:apos|#0?39);/g, re_quot: /&quot;/g }, write: /\{(\d{1,2})(?:\:([xodQqb]))?\}/g, isURL: /^(?:ht|f)tp(?:s)?\:\/\/(?:[\w\-\.]+)\.\w+/i, cut: /[\x00-\xFF]/, getRealLen: { r0: /[^\x00-\xFF]/g, r1: /[\x00-\xFF]/g }, format: /\{([\d\w\.]+)\}/g }, commonReplace: function (s, p, r) {
        return s.replace(p, r);
    }, format: function (str) {
        var args = Array.prototype.slice.call(arguments), v;
        str = String(args.shift());
        if (args.length == 1 && typeof (args[0]) == 'object') {
            args = args[0];
        }
        ZENG.string.RegExps.format.lastIndex = 0;
        return str.replace(ZENG.string.RegExps.format, function (m, n) {
            v = ZENG.object.route(args, n);
            return v === undefined ? m : v;
        });
    }
};


ZENG.object = {
    routeRE: /([\d\w_]+)/g,
    route: function (obj, path) {
        obj = obj || {};
        path = String(path);
        var r = ZENG.object.routeRE, m;
        r.lastIndex = 0;
        while ((m = r.exec(path)) !== null) {
            obj = obj[m[0]];
            if (obj === undefined || obj === null) {
                break;
            }
        }
        return obj;
    }
};



var ua = ZENG.userAgent = {}, agent = navigator.userAgent;
ua.ie = 9 - ((agent.indexOf('Trident/5.0') > -1) ? 0 : 1) - (window.XDomainRequest ? 0 : 1) - (window.XMLHttpRequest ? 0 : 1);



if (typeof (ZENG.msgbox) == 'undefined') {
    ZENG.msgbox = {};
}
ZENG.msgbox._timer = null;
ZENG.msgbox.loadingAnimationPath = ZENG.msgbox.loadingAnimationPath || ("/Admin/js/msgbox/images/loading.gif");
ZENG.msgbox.show = function (msgHtml, type, timeout, opts) {
    if (typeof (opts) == 'number') {
        opts = { topPosition: opts };
    }
    opts = opts || {};
    var _s = ZENG.msgbox,
	 template = '<span class="zeng_msgbox_layer" style="display:none;z-index:10000;" id="mode_tips_v2"><span class="gtl_ico_{type}"></span><span class="gtl_txt">{loadIcon}{msgHtml}</span></span>', loading = '<img src="" alt="" />', typeClass = [0, 0, 0, 0, "succ", "fail", "clear"], mBox, tips;
    _s._loadCss && _s._loadCss(opts.cssPath);
    mBox = ZENG.dom.get("q_Msgbox") || ZENG.dom.createElementIn("div", document.body, false, { className: "zeng_msgbox_layer_wrap" });
    mBox.id = "q_Msgbox";
    mBox.style.display = "";
    mBox.innerHTML = ZENG.string.format(template, { type: typeClass[type] || "hits", msgHtml: msgHtml || "", loadIcon: type == 6 ? loading : "" });
    _s._setPosition(mBox, timeout, opts.topPosition);
};
ZENG.msgbox._setPosition = function (tips, timeout, topPosition) {
    timeout = timeout /*|| 5000*/;
    var _s = ZENG.msgbox, bt = ZENG.dom.getScrollTop(), ch = ZENG.dom.getClientHeight(), t = Math.floor(ch / 2) - 40;
    ZENG.dom.setStyle(tips, "top", ((document.compatMode == "BackCompat" || ZENG.userAgent.ie < 7) ? bt : 0) + ((typeof (topPosition) == "number") ? topPosition : t) + "px");
    clearTimeout(_s._timer);
    tips.firstChild.style.display = "";
    timeout && (_s._timer = setTimeout(_s.hide, timeout));
};
ZENG.msgbox.hide = function (timeout) {
    var _s = ZENG.msgbox;
    if (timeout) {
        clearTimeout(_s._timer);
        _s._timer = setTimeout(_s._hide, timeout);
    } else {
        _s._hide();
    }
};
ZENG.msgbox._hide = function () {
    var _mBox = ZENG.dom.get("q_Msgbox"), _s = ZENG.msgbox;
    clearTimeout(_s._timer);
    if (_mBox) {
        var _tips = _mBox.firstChild;
        ZENG.dom.setStyle(_mBox, "display", "none");
    }
};

//成功
function ShowSuccessTip(msg) {
    ZENG.msgbox.show(msg, 4, 3000);
}

//失败
function ShowFailTip(msg) {
    ZENG.msgbox.show(msg, 5, 3000);
}

//加载中
function ShowLoadingTip(msg) {
    ZENG.msgbox.show(msg, 6, 3000);
}

//提示框2

window = window || {};

dom = {
    getById: function (id) {
        return document.getElementById(id);
    }, get: function (e) {
        return (typeof (e) == "string") ? document.getElementById(e) : e;
    }, createElementIn: function (tagName, elem, insertFirst, attrs) {
        var _e = (elem = dom.get(elem) || document.body).ownerDocument.createElement(tagName || "div"), k;
        if (typeof (attrs) == 'object') {
            for (k in attrs) {
                if (k == "class") {
                    _e.className = attrs[k];
                } else if (k == "style") {
                    _e.style.cssText = attrs[k];
                } else {
                    _e[k] = attrs[k];
                }
            }
        }
        insertFirst ? elem.insertBefore(_e, elem.firstChild) : elem.appendChild(_e);
        return _e;
    }, getStyle: function (el, property) {
        el = dom.get(el);
        if (!el || el.nodeType == 9) {
            return null;
        }
        var w3cMode = document.defaultView && document.defaultView.getComputedStyle, computed = !w3cMode ? null : document.defaultView.getComputedStyle(el, ''), value = "";
        switch (property) {
            case "float":
                property = w3cMode ? "cssFloat" : "styleFloat";
                break;
            case "opacity":
                if (!w3cMode) {
                    var val = 100;
                    try {
                        val = el.filters['DXImageTransform.Microsoft.Alpha'].opacity;
                    } catch (e) {
                        try {
                            val = el.filters('alpha').opacity;
                        } catch (e) {
                        }
                    }
                    return val / 100;
                } else {
                    return parseFloat((computed || el.style)[property]);
                }
                break;
            case "backgroundPositionX":
                if (w3cMode) {
                    property = "backgroundPosition";
                    return ((computed || el.style)[property]).split(" ")[0];
                }
                break;
            case "backgroundPositionY":
                if (w3cMode) {
                    property = "backgroundPosition";
                    return ((computed || el.style)[property]).split(" ")[1];
                }
                break;
        }
        if (w3cMode) {
            return (computed || el.style)[property];
        } else {
            return (el.currentStyle[property] || el.style[property]);
        }
    }, setStyle: function (el, properties, value) {
        if (!(el = dom.get(el)) || el.nodeType != 1) {
            return false;
        }
        var tmp, bRtn = true, w3cMode = (tmp = document.defaultView) && tmp.getComputedStyle, rexclude = /z-?index|font-?weight|opacity|zoom|line-?height/i;
        if (typeof (properties) == 'string') {
            tmp = properties;
            properties = {};
            properties[tmp] = value;
        }
        for (var prop in properties) {
            value = properties[prop];
            if (prop == 'float') {
                prop = w3cMode ? "cssFloat" : "styleFloat";
            } else if (prop == 'opacity') {
                if (!w3cMode) {
                    prop = 'filter';
                    value = value >= 1 ? '' : ('alpha(opacity=' + Math.round(value * 100) + ')');
                }
            } else if (prop == 'backgroundPositionX' || prop == 'backgroundPositionY') {
                tmp = prop.slice(-1) == 'X' ? 'Y' : 'X';
                if (w3cMode) {
                    var v = dom.getStyle(el, "backgroundPosition" + tmp);
                    prop = 'backgroundPosition';
                    typeof (value) == 'number' && (value = value + 'px');
                    value = tmp == 'Y' ? (value + " " + (v || "top")) : ((v || 'left') + " " + value);
                }
            }
            if (typeof el.style[prop] != "undefined") {
                el.style[prop] = value + (typeof value === "number" && !rexclude.test(prop) ? 'px' : '');
                bRtn = bRtn && true;
            } else {
                bRtn = bRtn && false;
            }
        }
        return bRtn;
    }, getScrollTop: function (doc) {
        var _doc = doc || document;
        return Math.max(_doc.documentElement.scrollTop, _doc.body.scrollTop);
    }, getClientHeight: function (doc) {
        var _doc = doc || document;
        return _doc.compatMode == "CSS1Compat" ? _doc.documentElement.clientHeight : _doc.body.clientHeight;
    }
};

string = {
    RegExps: { trim: /^\s+|\s+$/g, ltrim: /^\s+/, rtrim: /\s+$/, nl2br: /\n/g, s2nb: /[\x20]{2}/g, URIencode: /[\x09\x0A\x0D\x20\x21-\x29\x2B\x2C\x2F\x3A-\x3F\x5B-\x5E\x60\x7B-\x7E]/g, escHTML: { re_amp: /&/g, re_lt: /</g, re_gt: />/g, re_apos: /\x27/g, re_quot: /\x22/g }, escString: { bsls: /\\/g, sls: /\//g, nl: /\n/g, rt: /\r/g, tab: /\t/g }, restXHTML: { re_amp: /&amp;/g, re_lt: /&lt;/g, re_gt: /&gt;/g, re_apos: /&(?:apos|#0?39);/g, re_quot: /&quot;/g }, write: /\{(\d{1,2})(?:\:([xodQqb]))?\}/g, isURL: /^(?:ht|f)tp(?:s)?\:\/\/(?:[\w\-\.]+)\.\w+/i, cut: /[\x00-\xFF]/, getRealLen: { r0: /[^\x00-\xFF]/g, r1: /[\x00-\xFF]/g }, format: /\{([\d\w\.]+)\}/g }, commonReplace: function (s, p, r) {
        return s.replace(p, r);
    }, format: function (str) {
        var args = Array.prototype.slice.call(arguments), v;
        str = String(args.shift());
        if (args.length == 1 && typeof (args[0]) == 'object') {
            args = args[0];
        }
        string.RegExps.format.lastIndex = 0;
        return str.replace(string.RegExps.format, function (m, n) {
            v = object.route(args, n);
            return v === undefined ? m : v;
        });
    }
};


object = {
    routeRE: /([\d\w_]+)/g,
    route: function (obj, path) {
        obj = obj || {};
        path = String(path);
        var r = object.routeRE, m;
        r.lastIndex = 0;
        while ((m = r.exec(path)) !== null) {
            obj = obj[m[0]];
            if (obj === undefined || obj === null) {
                break;
            }
        }
        return obj;
    }
};



var ua = userAgent = {}, agent = navigator.userAgent;
ua.ie = 9 - ((agent.indexOf('Trident/5.0') > -1) ? 0 : 1) - (window.XDomainRequest ? 0 : 1) - (window.XMLHttpRequest ? 0 : 1);



if (typeof (msgbox) == 'undefined') {
    msgbox = {};
}
msgbox._timer = null;
msgbox.loadingAnimationPath = msgbox.loadingAnimationPath || ("/Admin/js/msgbox/images/loading.gif");
msgbox.show = function (msgHtml, type, timeout, opts) {
    if (typeof (opts) == 'number') {
        opts = { topPosition: opts };
    }
    opts = opts || {};
    var _s = msgbox,
     template2 = '<span class="judgement_msgbox" style="display:none;z-index:10000;" id="mode_tips_v2"><span class="gtl_con"></span><span class="gtl_txt">{loadIcon}{msgHtml}</span><span class="gtl_bottom_btn"><span class="gtl_btn msg_butCancel">取消</span><span class="gtl_btn msg_butCenter">确认</span></span></span><div class="black_overlay"></div>', typeClass = [0, 0, 0, 0], mBox, tips;
    _s._loadCss && _s._loadCss(opts.cssPath);
    mBox = dom.get("q_Msgbox2") || dom.createElementIn("div", document.body, false, { className: "msgbox_wrap" });
    mBox.id = "q_Msgbox2";
    mBox.style.display = "";
    mBox.innerHTML = string.format(template2, { type: typeClass[type] || "hits", msgHtml: msgHtml || "", loadIcon: type == 6 ? loading : "" });
    _s._setPosition(mBox, timeout, opts.topPosition);
};
msgbox._setPosition = function (tips, timeout, topPosition) {
    timeout = timeout/* || 5000*/;
    var _s = msgbox, bt = dom.getScrollTop(), ch = dom.getClientHeight(), t = Math.floor(ch / 2) - 40;
    dom.setStyle(tips, "top", ((document.compatMode == "BackCompat" || userAgent.ie < 7) ? bt : 0) + ((typeof (topPosition) == "number") ? topPosition : t) + "px");
    clearTimeout(_s._timer);
    tips.firstChild.style.display = "";
    timeout && (_s._timer = setTimeout(_s.hide, timeout));
};
msgbox.hide = function (timeout) {
    var _s = msgbox;
    if (timeout) {
        clearTimeout(_s._timer);
        _s._timer = setTimeout(_s._hide, timeout);
    } else {
        _s._hide();
    }
};
msgbox._hide = function () {
    var _mBox = dom.get("q_Msgbox2"), _s = msgbox;
    clearTimeout(_s._timer);
    if (_mBox) {
        var _tips = _mBox.firstChild;
        dom.setStyle(_mBox, "display", "none");
    }
};


//确认框
function ShowConfirm(msg, centerCallback, cancelCallBack) {
    msgbox.show(msg, 1/*, 3000*/);
    $(".black_overlay").show();

    //取消
    $(".msg_butCancel").click(function () {
        if (typeof cancelCallBack === "function") {
            cancelCallBack();
        }
        $(".msgbox_wrap").hide();
    });

    //确定
    $('.msg_butCenter').click(function () {
        if (typeof centerCallback === "function") {
            centerCallback();
        }
        $(".msgbox_wrap").hide();
    });
}


function clickhide() {
    msgbox._hide();
}

function msgAlert(type, msg) {
    $('.msg_' + type).html(msg);
    $('.msg_' + type).animate({ 'top': 0 }, 500);
    setTimeout(function () { $('.msg_' + type).animate({ 'top': '-3rem' }, 500) }, 2000);
}
