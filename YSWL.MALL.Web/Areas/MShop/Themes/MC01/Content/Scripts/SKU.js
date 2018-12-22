 
//保存最后的组合结果信息
var SKUResult = {};
//获得对象的key

function getObjKeys(obj) {
    if (obj !== Object(obj)) throw new TypeError('Invalid object');
    var keys = [];
    for (var key in obj)
        if (Object.prototype.hasOwnProperty.call(obj, key))
            keys[keys.length] = key;
    return keys;
}

//把组合的key放入结果集SKUResult

function add2SKUResult(key, sku) {
    if (SKUResult[key]) { //SKU信息key属性·
        SKUResult[key].count += sku.count;
        SKUResult[key].prices.push(sku.price);
        SKUResult[key].rankprices.push(sku.rankprice);
        SKUResult[key].skus.push(sku.sku);
    } else {
        SKUResult[key] = {
            count: sku.count,
            prices: [sku.price],
            rankprices: [sku.rankprice],
            skus: [sku.sku]
        };
    }
}

//对一条SKU信息进行拆分组合

function combineSKU(skuKeyAttrs, cnum, sku) {
    var len = skuKeyAttrs.length;
    for (var i = 0; i < len; i++) {
        var key = skuKeyAttrs[i];
        for (var j = i + 1; j < len; j++) {
            if (j + cnum <= len) {
                var tempArr = skuKeyAttrs.slice(j, j + cnum); //安装组合个数获得属性值·
                var genKey = key + "," + tempArr.join(","); //得到一个组合key
                add2SKUResult(genKey, sku);
            }
        }
    }
}

var dataDefault;
//初始化得到结果集

function initSKU() {
    SKUResult = {}; //重置sku数据
    var data = $('#SKUOptions #SKUDATA').val();
    if (!data) return;
    data = jQuery.parseJSON(data);
    dataDefault = data.Default;
    var dataSKU = data.SKUDATA;

    var i, j, skuKeys = getObjKeys(dataSKU);
    for (i = 0; i < skuKeys.length; i++) {
        var skuKey = skuKeys[i]; //一条SKU信息key
        var sku = dataSKU[skuKey]; //一条SKU信息value
        var skuKeyAttrs = skuKey.split(","); //SKU信息key属性值数组
        var len = skuKeyAttrs.length;

        //Fix SKU KEY NO SORT BEN ADD 20131112
        skuKeyAttrs.sort(function (value1, value2) {
            return parseInt(value1) - parseInt(value2);
        });
        skuKey = skuKeyAttrs.join(',');
        //对每个SKU信息key属性值进行拆分组合
        for (j = 0; j < len; j++) {
            //单个属性值作为key直接放入SKUResult
            add2SKUResult(skuKeyAttrs[j], sku);
            //对本组SKU信息key属性进行组合，组合个数为j
            (j > 0 && j < len - 1) && combineSKU(skuKeyAttrs, j, sku);
        }

        //结果集接放入SKUResult
        SKUResult[skuKey] = {
            count: sku.count,
            prices: [sku.price],
            rankprices: [sku.rankprice],
            skus: [sku.sku]
        };
    }
}

//初始化用户选择事件
$(function () { 
    InitializationSku();
    $(document).on('click', '#SKUOptions .SkuItems', function () {
        var self = $(this);
        if (self.attr('disabled')) return false;

        var btnAddToCart = $('#btnAddToCart');
        self.toggleClass('select').siblings().removeClass('select');


        //if ($('#iteminfo .view-photo img').length > 0) {
        //    var imgSel = self.find('a img:eq(0)');
        //    if (self.hasClass('cur') && imgSel.length == 1) {
        //        //选择SKU图片时,将sku图片显示为商品的第一个主图
        //        var iteminfo_img = $('#iteminfo .view-photo img').eq(0);
        //        iteminfo_img.attr('src', imgSel.attr('largeurl'));
        //        $.scaleLoad('#iteminfo .view-photo', 175, 228);
        //    } else if (!self.hasClass('cur') && imgSel.length == 1) {
        //        //取消选择SKU图片时, 还原显示商品第一个主图
        //        var iteminfo_img = $('#iteminfo .view-photo img').eq(0);
        //        iteminfo_img.attr('src', iteminfo_img.attr('item'));
        //    }
        //}

        var selectedObjs = $('#SKUOptions .select');
        if (selectedObjs.length) {
            //获得组合key价格
            var selectedIds = [];
            var selectValueStr = '';//已选
           // var strV = '';
            selectedObjs.each(function () {
                selectedIds.push($(this).attr('AttrId'));
                selectValueStr += $(this).attr('alt') + '，';
            });

            //移除最后一个逗号
            if (selectValueStr.length > 0) {
                selectValueStr = selectValueStr.substring(0, selectValueStr.length - 1);
            }

            selectedIds.sort(function (value1, value2) {
                return parseInt(value1) - parseInt(value2);
            });
            var len = selectedIds.length;
            var skuItem = $('#btnAddToCart').removeAttr('disabled', 'disabled').removeClass('addShop-n');
            $('#btnAddToCart').text($('#btnAddToCart').attr('DefaultText'));
            if (!SKUResult[selectedIds.join(',')]) {
                var skuItem = $('#btnAddToCart').attr('disabled', 'disabled').addClass('addShop-n');
                $('#btnAddToCart').text('该商品已售罄');
                return;
            }
            var prices = SKUResult[selectedIds.join(',')].prices;
            var maxPrice = Math.max.apply(Math, prices);
            var minPrice = Math.min.apply(Math, prices);
            //会员价格
            var rankprices = SKUResult[selectedIds.join(',')].rankprices;
            var maxrankPrice = Math.max.apply(Math, rankprices);
            var minrankPrice = Math.min.apply(Math, rankprices);
            $('#stylePrice').text(
                    maxPrice > minPrice ? minPrice.toFixed(2) + "-" + maxPrice.toFixed(2) : maxPrice.toFixed(2));
            $('#rankPrice').text(
                    maxrankPrice > minrankPrice ? minrankPrice.toFixed(2) + "-" + maxrankPrice.toFixed(2) : maxrankPrice.toFixed(2));

            //用已选中的节点验证待测试节点 underTestObjs
            $('#SKUOptions .SkuItems').not(selectedObjs).not(self).each(function () {
                var siblingsSelectedObj = $(this).siblings('#SKUOptions .select');
                var testAttrIds = []; //从选中节点中去掉选中的兄弟节点
                if (siblingsSelectedObj.length) {
                    var siblingsSelectedObjId = siblingsSelectedObj.attr('AttrId');
                    for (var i = 0; i < len; i++) {
                        (selectedIds[i] != siblingsSelectedObjId) && testAttrIds.push(selectedIds[i]);
                    }
                } else {
                    testAttrIds = selectedIds.concat();
                }
                testAttrIds = testAttrIds.concat($(this).attr('AttrId'));
                testAttrIds.sort(function (value1, value2) {
                    return parseInt(value1) - parseInt(value2);
                });
                if (!SKUResult[testAttrIds.join(',')]) {
                    Status_SKUItem(this, false);
                } else {
                    Status_SKUItem(this, true);
                }
            });

            var skus = SKUResult[selectedIds.join(',')].skus;
            //添加购物车 设置 sku 数据
            if (selectedObjs.length == $('#SKUOptions .AttrItems').length &&
                    skus && skus.length == 1) {
                //显示库存
                if ($('#div_stock').length > 0) {
                    $('#div_stock').show().find('#stock_num').text(SKUResult[selectedIds.join(',')].count);
                }
                btnAddToCart.attr('itemid', skus[0]);
                //商品编码
                if ($('#prodcut_sku').length > 0) {
                    $('#prodcut_sku').text(skus[0]);
                }
            } else {
                //隐藏库存
                if ($('#div_stock').length > 0) {
                    $('#div_stock').hide().find('#stock_num').text('');
                }
                //商品编码
                if ($('#prodcut_sku').length > 0) {
                    $('#prodcut_sku').text($('#prodcut_sku').attr('productCode'));
                }
                btnAddToCart.attr('itemid', '');
            }
            //设置已选择
            $('#selectInfoText').html(selectValueStr);
            
        } else {
            //设置默认价格
            $('#stylePrice').text(
                    dataDefault.maxPrice > dataDefault.minPrice ?
                        dataDefault.minPrice.toFixed(2) + "-" + dataDefault.maxPrice.toFixed(2) :
                        dataDefault.maxPrice.toFixed(2));

            $('#rankPrice').text(
                    dataDefault.maxRankPrice > dataDefault.minRankPrice ?
                        dataDefault.minRankPrice.toFixed(2) + "-" + dataDefault.maxRankPrice.toFixed(2) :
                        dataDefault.maxRankPrice.toFixed(2));
            //设置属性状态
            $('#SKUOptions .SkuItems').each(function () {
                if (SKUResult[$(this).attr('AttrId')]) {
                    Status_SKUItem(this, true);
                } else {
                    Status_SKUItem(this, false);
                }
            });
            //清空已选择
            $('#selectInfoText').empty();
            btnAddToCart.attr('itemid', '');
        }
    });
});



function InitializationSku() {
    if ($('#SKUOptions #SKUDATA').length != 1) return;
    initSKU();
    $('#SKUOptions .SkuItems').each(function () {
        var self = $(this);
        var attrId = self.attr('AttrId');
        if (!SKUResult[attrId]) {
            Status_SKUItem(self, false);
        } else {
            Status_SKUItem(self, true);
        }
    }); 
 
    //无库存
    if ($('#SKUOptions .SkuItems').length === $('#SKUOptions .SkuItems[disabled]').length) {
        $('#btnAddToCart').attr('disabled', 'disabled').addClass('addShop-n').text('已售罄');
        $('#divBuyInfo').hide();
        $('#selectInfoText').empty();
    } else {  //有库存
        $('#btnAddToCart').removeAttr('disabled', 'disabled').removeClass('addShop-n').text($('#btnAddToCart').attr('DefaultText')).attr('itemid', '');
        $('#divBuyInfo').show();
        $('#selectInfoText').empty();
    }

    $('#stylePrice').text(
            dataDefault.maxPrice > dataDefault.minPrice ?
                dataDefault.minPrice.toFixed(2) + "-" + dataDefault.maxPrice.toFixed(2) :
                dataDefault.maxPrice.toFixed(2));
    $('#rankPrice').text(
            dataDefault.maxRankPrice > dataDefault.minRankPrice ?
                dataDefault.minRankPrice.toFixed(2) + "-" + dataDefault.maxRankPrice.toFixed(2) :
                dataDefault.maxRankPrice.toFixed(2));
};

function Status_SKUItem(target, enabled) {
                if (enabled) {
                    var skuItem = $(target).removeAttr('disabled').removeClass('none');
                    skuItem.attr('title', skuItem.attr('alt'));
                    skuItem.find('img:eq(0)').attr('title', skuItem.attr('alt'));
                } else {
                    var skuItem = $(target).attr('disabled', 'disabled').removeClass('select').addClass('none');
                    skuItem.attr('title', skuItem.attr('alt') + ' 已售罄');
                    skuItem.find('img:eq(0)').attr('title', skuItem.attr('alt') + ' 已售罄');
                }
};
 