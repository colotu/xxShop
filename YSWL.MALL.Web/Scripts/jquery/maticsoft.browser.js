/*
* File:        maticsoft.browser.js
* Author:      huhy@ys56.com
* Copyright © 2004-2016 YSWL. All Rights Reserved.
*/
/**
* 1.jquery的继承机制 对jquery 1.9.0. 版本 进行扩展 使其支持 $.browser 方法，已达到兼容之前组件的目的.(maticsoft.jqueryui.dialog.js)
*/
;
jQuery.extend({
    browser: function () {
        var
        rwebkit = /(webkit)\/([\w.]+)/,
        ropera = /(opera)(?:.*version)?[ \/]([\w.]+)/,
        rmsie = /(msie) ([\w.]+)/,
        rmozilla = /(mozilla)(?:.*? rv:([\w.]+))?/,
        browser = {},
        ua = window.navigator.userAgent,
        browserMatch = uaMatch(ua);

        if (browserMatch.browser) {
            browser[browserMatch.browser] = true;
            browser.version = browserMatch.version;
        }
        return { browser: browser };
    },
});

function uaMatch(ua) {
    ua = ua.toLowerCase();

    var match = rwebkit.exec(ua)
                || ropera.exec(ua)
                || rmsie.exec(ua)
                || ua.indexOf("compatible") < 0 && rmozilla.exec(ua)
                || [];

    return {
        browser: match[1] || "",
        version: match[2] || "0"
    };
}
 