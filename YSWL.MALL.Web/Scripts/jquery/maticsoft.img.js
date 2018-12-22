/*
* File:        maticsoft.img.js
* Author:      yaoyuan@ys56.com
* Copyright © 2004-2013 YSWL. All Rights Reserved.
*/
;
/* id为需要处理的图片所在的容器节点的id，maxWidth即为需要限制的最大宽度 */
(function($) {
    //延迟加载并等比例缩放
    $.lazyScaleLoad = function(id, maxWidth, maxHeight, callback) {
        if (!id) return;
        var imgs = $(id).find('img');
        for (var i = 0; i < imgs.length; i++) {
            (function(target) {
                if ($(target).attr('src')) return;

                // 每个图片只加載一次, 防止重复加載
                if ($(target).attr('loaded')) return;

                //初始化
                $.scaleLoadInit(target, maxWidth, maxHeight);

                //延迟加载
                //skip_invisible: false, failure_limit: 10,
                $(target).lazyload({
                    effect: "fadeIn",
                    skip_invisible: false,
                    load: function(elements_left, settings) {
                        var sender = this;
                        imgReady($(sender).attr('data-original'), function() {
                            $.scaleImg(this, maxWidth, maxHeight, sender);
                            //图片报头尺寸就绪
                            if ($(sender).attr('loaded') != 'ERROR') {
                                $(sender).attr('loaded', 'NO');
                            }
                        }, function() {
                            if (callback) {
                                callback.call(sender);
                            }
                            if ($(sender).attr('loaded') != 'ERROR') {
                                $(sender).attr('loaded', 'OK');
                            }
                        }, function() {
                            var url404 = maticsoftImgGet404URL(maxWidth, maxHeight);
                            imgReady(url404, null, function() {
                                $(sender).attr('src', url404);
                                $.scaleImg(this, maxWidth, maxHeight, sender);
                                if (callback) {
                                    callback.call(sender);
                                }
                                $(sender).attr('loaded', 'ERROR');
                            });
                        });
                    }
                });
            })(imgs[i]);
        } //for end
    };
    //延迟加载并指定尺寸拉伸
    $.lazyFixedLoadImg = function(id, maxWidth, maxHeight, callback) {
        if (!id) return;
        var imgs = $(id).find('img');
        for (var i = 0; i < imgs.length; i++) {
            (function(target) {
                if ($(target).attr('src')) return;

                // 每个图片只加載一次, 防止重复加載
                if ($(target).attr('loaded')) return;

                //初始化
                $.scaleLoadInit(target, maxWidth, maxHeight);

                //延迟加载
                //skip_invisible: false, failure_limit: 10,
                $(target).lazyload({
                    effect: "fadeIn",
                    skip_invisible: false,
                    load: function(elements_left, settings) {
                        var sender = this;
                        imgReady($(sender).attr('data-original'), function() {
                            this.width = maxWidth;
                            this.height = maxHeight;
                            sender.width = maxWidth;
                            sender.height = maxHeight;
                            //图片报头尺寸就绪
                            if ($(sender).attr('loaded') != 'ERROR') {
                                $(sender).attr('loaded', 'NO');
                            }
                        }, function() {
                            if (callback) {
                                callback.call(sender);
                            }
                            if ($(sender).attr('loaded') != 'ERROR') {
                                $(sender).attr('loaded', 'OK');
                            }
                        }, function() {
                            var url404 = maticsoftImgGet404URL(maxWidth, maxHeight);
                            imgReady(url404, null, function() {
                                $(sender).attr('src', url404);
                                this.width = maxWidth;
                                this.height = maxHeight;
                                sender.width = maxWidth;
                                sender.height = maxHeight;
                                if (callback) {
                                    callback.call(sender);
                                }
                                $(sender).attr('loaded', 'ERROR');
                            });
                        });
                    }
                });
            })(imgs[i]);
        } //for end
    };

    $.scaleLoadInit = function(target, maxWidth, maxHeight) {
        $(target).attr('loaded', 'loading');

        //Loading
        //var loading = $("<img alt=\"加载中\" title=\"图片加载中\" src=\"/Content/themes/base/SNS/images/loadingImg.gif\" />");
        $(target).show();
        //$(target).after(loading);
        //loading.css('padding',maxWidth+'px'+' '+maxHeight+'px');
        var ref = $(target).attr('ref') ? $(target).attr('ref') : $(target).attr('src');
        //$(target).attr('data-ref', ref);

        if (!ref) {
            $(target).attr('data-original', maticsoftImgGet404URL(maxWidth, maxHeight));
            $(target).attr('loaded', 'ERROR');
        } else {
            $(target).attr('data-original', ref);
        }
        $(target).removeAttr('ref');

        // Remove all attributes and CSS rules
        target.removeAttribute("height");
        target.removeAttribute("width");
        target.style.height = target.style.width = "";

        //占位图片 防止IE出现默认占位
        $(target).attr('src', '/Content/themes/base/images/transparent.gif');

        //设置占位高度
        $(target).attr('width', maxWidth + 'px');
        $(target).attr('height', maxHeight > 300 ? '300px' : maxHeight + 'px'); //设置占位高度
    };

    //立即加载并等比例缩放
    $.scaleLoad = function(id, maxWidth, maxHeight, callback) {
        if (!id) return;
        var imgs = $(id).find('img');
        for (var i = 0; i < imgs.length; i++) {
            (function(target) {
                //if ($(target).attr('src')) return;

                // 每个图片只加載一次, 防止重复加載
                if ($(target).attr('loaded')) return;

                //初始化
                $.scaleLoadInit(target, maxWidth, maxHeight);

                var url = $(target).attr('data-original');
                imgReady(url, function() {
                    $.scaleImg(this, maxWidth, maxHeight, target);
                    //图片报头尺寸就绪
                    if ($(target).attr('loaded') != 'ERROR') {
                        $(target).attr('loaded', 'NO');
                    }
                }, function() {
                    $(target).attr('src', url);


                    if (callback) {
                        callback.call(target);
                    }
                    if ($(target).attr('loaded') != 'ERROR') {
                        $(target).attr('loaded', 'OK');
                    }
                }, function() {
                    var url404 = maticsoftImgGet404URL(maxWidth, maxHeight);
                    imgReady(url404, null, function() {
                        $(target).attr('src', url404);
                        $.scaleImg(this, maxWidth, maxHeight, target);
                        if (callback) {
                            callback.call(target);
                        }
                        $(target).attr('loaded', 'ERROR');
                    });
                });
            })(imgs[i]);
        } //for end
    };
    
    //立即加载并指定尺寸拉伸
    $.scaleFixedLoad = function(id, maxWidth, maxHeight, callback) {
        if (!id) return;
        var imgs = $(id).find('img');
        for (var i = 0; i < imgs.length; i++) {
            (function(target) {
                //if ($(target).attr('src')) return;

                // 每个图片只加載一次, 防止重复加載
                if ($(target).attr('loaded')) return;

                //初始化
                $.scaleLoadInit(target, maxWidth, maxHeight);

                var url = $(target).attr('data-original');
                imgReady(url, function() {
                    this.width = maxWidth;
                    this.height = maxHeight;
                    //图片报头尺寸就绪
                    if ($(target).attr('loaded') != 'ERROR') {
                        $(target).attr('loaded', 'NO');
                    }
                }, function() {
                    $(target).attr('src', url);


                    if (callback) {
                        callback.call(target);
                    }
                    if ($(target).attr('loaded') != 'ERROR') {
                        $(target).attr('loaded', 'OK');
                    }
                }, function() {
                    var url404 = maticsoftImgGet404URL(maxWidth, maxHeight);
                    imgReady(url404, null, function() {
                        $(target).attr('src', url404);
                                this.width = maxWidth;
                                this.height = maxHeight;
                        if (callback) {
                            callback.call(target);
                        }
                        $(target).attr('loaded', 'ERROR');
                    });
                });
            })(imgs[i]);
        } //for end
    };
    //延迟等比例缩放 -- 未完成 2012-11-14
//    $.lazyZoomImg = function(img, maxWidth, maxHeight, target) {
//        if (img.naturalWidth > 0 && img.naturalHeight > 0) {
//            if (img.naturalWidth / img.naturalHeight >= maxWidth / maxHeight) {
//                if (img.naturalWidth > maxWidth) {
//                    img.width = maxWidth;
//                    img.height = (img.naturalHeight * maxWidth) / img.naturalWidth;
//                } else {
//                    img.width = img.naturalWidth;
//                    img.height = img.naturalHeight;
//                }
//                //img.alt = img.width + "×" + img.height;
//            } else {
//                if (img.naturalHeight > maxHeight) {
//                    img.height = maxHeight;
//                    img.width = (img.naturalWidth * maxHeight) / img.naturalHeight;
//                } else {
//                    img.width = img.naturalWidth;
//                    img.height = img.naturalHeight;
//                }
//                //img.alt = img.width + "×" + img.height;
//            }
//        }
//        if (target) {
//            target.width = img.width;
//            target.height = img.height;
////        target.style.width = img.width + "px";
////        target.style.height = img.height + "px";
//        }
//        return img;
//    };

    //立即等比例缩放
    $.scaleImg = function(img, maxWidth, maxHeight, target) {
// BUG NO Support IE7,IE8 BEN fix DONE!
//        if (img.width > 0 && img.height > 0) {
//            if (img.width / img.height >= maxWidth / maxHeight) {
//                if (img.width > maxWidth) {
//                    img.width = maxWidth;
//                    img.height = (img.height * maxWidth) / img.width;
//                } else {
//                    img.width = img.width;
//                    img.height = img.height;
//                }
//                //img.alt = img.width + "×" + img.height;
//            } else {
//                if (img.height > maxHeight) {
//                    img.height = maxHeight;
//                    img.width = (img.width * maxHeight) / img.height;
//                } else {
//                    img.width = img.width;
//                    img.height = img.height;
//                }
//                //img.alt = img.width + "×" + img.height;
//            }
//        }

        var aspectRatio = 0;
        // Calculate aspect ratio now, if possible
        if (maxHeight && maxWidth) {
            aspectRatio = maxWidth / maxHeight;
        }

        var imgHeight = img.height, imgWidth = img.width, imgAspectRatio = imgWidth / imgHeight, bxHeight = maxHeight, bxWidth = maxWidth, bxAspectRatio = aspectRatio;

        // Work the magic!
        // If one parameter is missing, we just force calculate it
        if (!bxAspectRatio) {
            if (bxHeight) {
                bxAspectRatio = imgAspectRatio + 1;
            } else {
                bxAspectRatio = imgAspectRatio - 1;
            }
        }

        // Only resize the images that need resizing
        if ((bxHeight && imgHeight > bxHeight) || (bxWidth && imgWidth > bxWidth)) {

            if (imgAspectRatio > bxAspectRatio) {
                bxHeight = ~~(imgHeight / imgWidth * bxWidth);
            } else {
                bxWidth = ~~(imgWidth / imgHeight * bxHeight);
            }

            img.height = bxHeight;
            img.width = bxWidth;
        }

        if (target) {
            target.width = img.width;
            target.height = img.height;
//        target.style.width = img.width + "px";
//        target.style.height = img.height + "px";
        }
        return img;
    };

})(jQuery);

function maticsoftImgGet404URL(width, height) {
    var url404 = '/Content/themes/base/images/404/';
    if (width <= 80 || height <= 80) {
        url404 += '80.jpg';
    } else if (width <= 142 || height <= 142) {
        url404 += '142.jpg';
    } else if (width <= 224 || height <= 224) {
        url404 += '224.jpg';
    } else if (width <= 230 || height <= 230) {
        url404 += '230.jpg';
    } else if (width <= 260 || height <= 260) {
        url404 += '260.jpg';
    } else {
        url404 += '142.jpg';
    }
    return url404;
}

//-------------- 向下兼容区域 START -----------------
function resizeImg(id, maxWidth, maxHeight, callback) {
    $.lazyScaleLoad(id, maxWidth, maxHeight, callback);
}
//等比例缩放
function zoomImg(img, maxWidth, maxHeight) {
    return $.scaleImg(img, maxWidth, maxHeight);
}
//-------------- 向下兼容区域 END -----------------

/**
* imgReady javascript plugin for Ben IE fix 20121120
* Author:      yaoyuan@ys56.com
* Copyright © 2006-2012 YSWL. All Rights Reserved.
* 图片头数据加载就绪事件 - 更快获取图片尺寸
* @param	{String}	图片路径
* @param	{Function}	尺寸就绪
* @param	{Function}	加载完毕 (可选)
* @param	{Function}	加载错误 (可选)
* @example imgReady('http://www.google.com.hk/intl/zh-CN/images/logo_cn.png', function () {
alert('size ready: width=' + this.width + '; height=' + this.height);
});
*/
var imgReady = (function() {
    var list = [], intervalId = null,
        // 用来执行队列
        tick = function() {
            var i = 0;
            for (; i < list.length; i++) {
                list[i].end ? list.splice(i--, 1) : list[i]();
            }
            ;
            !list.length && stop();
        },
        // 停止所有定时器队列
        stop = function() {
            clearInterval(intervalId);
            intervalId = null;
        };

    return function(url, ready, load, error) {
        if (!url) return;

        var onready, width, height, newWidth, newHeight,
            img = new Image();


        // 图片尺寸就绪
        onready = function() {
            newWidth = img.width;
            newHeight = img.height;
            if (newWidth !== width || newHeight !== height ||
                // 如果图片已经在其他地方加载可使用面积检测
                newWidth * newHeight > 1024) {
                ready && ready.call(img);
                onready.end = true;
            }
            ;
        };

        // 完全加载完毕的事件
        img.onload = function() {
            // onload在定时器时间差范围内可能比onready快
            // 这里进行检查并保证onready优先执行
            !onready.end && onready();

            load && load.call(img);

            // IE gif动画会循环执行onload，置空onload即可
            img = img.onload = img.onerror = null;
        };

        // 加载错误后的事件
        img.onerror = function() {
            error && error.call(img);
            onready.end = true;
            img = img.onload = img.onerror = null;
        };

        //Ben fix IE7,IE8 BUG set width and height 0
        width = img.width;
        height = img.height;
        img.src = url;

        // 如果图片被缓存，则直接返回缓存数据
        if (img && img.complete) {
            ready && ready.call(img);
            load && load.call(img);
            return;
        }
        ;
        img && onready();


        // 加入队列中定期执行
        if (!onready.end) {
            list.push(onready);
            // 无论何时只允许出现一个定时器，减少浏览器性能损耗
            if (intervalId === null) intervalId = setInterval(tick, 40);
        }
        ;
    };
})();

/*------------------------ 三方内容 START ---------------------------*/

/*
* Lazy Load - jQuery plugin for lazy loading images
*
* Copyright (c) 2007-2012 Mika Tuupola
*
* Licensed under the MIT license:
*   http://www.opensource.org/licenses/mit-license.php
*
* Project home:
*   http://www.appelsiini.net/projects/lazyload
*
* Version:  1.8.1
*
*/
(function ($, window) {
    var $window = $(window);

    $.fn.lazyload = function (options) {
        var elements = this;
        var $container;
        var settings = {
            threshold: 0,
            failure_limit: 0,
            event: "scroll",
            effect: "show",
            container: window,
            data_attribute: "original",
            skip_invisible: true,
            appear: null,
            load: null
        };

        function update() {
            var counter = 0;
            elements.each(function () {
                var $this = $(this);
                if (settings.skip_invisible && !$this.is(":visible")) {
                    return;
                }
                if ($.abovethetop(this, settings) ||
                    $.leftofbegin(this, settings)) {
                    /* Nothing. */
                } else if (!$.belowthefold(this, settings) &&
                    !$.rightoffold(this, settings)) {
                    $this.trigger("appear");
                    /* if we found an image we'll load, reset the counter */
                    counter = 0;
                } else {
                    if (++counter > settings.failure_limit) {
                        return false;
                    }
                }
            });

        }

        if (options) {
            /* Maintain BC for a couple of versions. */
            if (undefined !== options.failurelimit) {
                options.failure_limit = options.failurelimit;
                delete options.failurelimit;
            }
            if (undefined !== options.effectspeed) {
                options.effect_speed = options.effectspeed;
                delete options.effectspeed;
            }

            $.extend(settings, options);
        }

        /* Cache container as jQuery as object. */
        $container = (settings.container === undefined ||
                      settings.container === window) ? $window : $(settings.container);

        /* Fire one scroll event per scroll. Not one scroll event per image. */
        if (0 === settings.event.indexOf("scroll")) {
            $container.bind(settings.event, function (event) {
                return update();
            });
        }

        this.each(function () {
            var self = this;
            var $self = $(self);

            self.loaded = false;

            /* When appear is triggered load original image. */
            $self.one("appear", function () {
                if (!this.loaded) {
                    if (settings.appear) {
                        var elements_left = elements.length;
                        settings.appear.call(self, elements_left, settings);
                    }
                    $("<img />")
                        .bind("load error", function () {
                            $self
                                .hide()
                                .attr("src", $self.data(settings.data_attribute))
                                [settings.effect](settings.effect_speed);
                            self.loaded = true;

                            /* Remove image from array so it is not looped next time. */
                            var temp = $.grep(elements, function (element) {
                                return !element.loaded;
                            });
                            elements = $(temp);

                            if (settings.load) {
                                var elements_left = elements.length;
                                settings.load.call(self, elements_left, settings);
                            }
                        })
                        .attr("src", $self.data(settings.data_attribute));
                }
            });

            /* When wanted event is triggered load original image */
            /* by triggering appear.                              */
            if (0 !== settings.event.indexOf("scroll")) {
                $self.bind(settings.event, function (event) {
                    if (!self.loaded) {
                        $self.trigger("appear");
                    }
                });
            }
        });

        /* Check if something appears when window is resized. */
        $window.bind("resize", function (event) {
            update();
        });

        /* Force initial check if images should appear. */
        $(document).ready(function () {
            update();
        });

        return this;
    };

    /* Convenience methods in jQuery namespace.           */
    /* Use as  $.belowthefold(element, {threshold : 100, container : window}) */

    $.belowthefold = function (element, settings) {
        var fold;

        if (settings.container === undefined || settings.container === window) {
            fold = $window.height() + $window.scrollTop();
        } else {
            fold = $(settings.container).offset().top + $(settings.container).height();
        }

        return fold <= $(element).offset().top - settings.threshold;
    };

    $.rightoffold = function (element, settings) {
        var fold;

        if (settings.container === undefined || settings.container === window) {
            fold = $window.width() + $window.scrollLeft();
        } else {
            fold = $(settings.container).offset().left + $(settings.container).width();
        }

        return fold <= $(element).offset().left - settings.threshold;
    };

    $.abovethetop = function (element, settings) {
        var fold;

        if (settings.container === undefined || settings.container === window) {
            fold = $window.scrollTop();
        } else {
            fold = $(settings.container).offset().top;
        }

        return fold >= $(element).offset().top + settings.threshold + $(element).height();
    };

    $.leftofbegin = function (element, settings) {
        var fold;

        if (settings.container === undefined || settings.container === window) {
            fold = $window.scrollLeft();
        } else {
            fold = $(settings.container).offset().left;
        }

        return fold >= $(element).offset().left + settings.threshold + $(element).width();
    };

    $.inviewport = function (element, settings) {
        return !$.rightoffold(element, settings) && !$.leftofbegin(element, settings) &&
                !$.belowthefold(element, settings) && !$.abovethetop(element, settings);
    };

    /* Custom selectors for your convenience.   */
    /* Use as $("img:below-the-fold").something() or */
    /* $("img").filter(":below-the-fold").something() which is faster */

    $.extend($.expr[':'], {
        "below-the-fold": function (a) { return $.belowthefold(a, { threshold: 0 }); },
        "above-the-top": function (a) { return !$.belowthefold(a, { threshold: 0 }); },
        "right-of-screen": function (a) { return $.rightoffold(a, { threshold: 0 }); },
        "left-of-screen": function (a) { return !$.rightoffold(a, { threshold: 0 }); },
        "in-viewport": function (a) { return $.inviewport(a, { threshold: 0 }); },
        /* Maintain BC for couple of versions. */
        "above-the-fold": function (a) { return !$.belowthefold(a, { threshold: 0 }); },
        "right-of-fold": function (a) { return $.rightoffold(a, { threshold: 0 }); },
        "left-of-fold": function (a) { return !$.rightoffold(a, { threshold: 0 }); }
    });

})(jQuery, window);
/*------------------------ 三方内容 END ---------------------------*/