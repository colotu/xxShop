
function History(key) {
    this.limit = 10;  // 最多10条记录
    this.key = key || 'y_his';  // 键值
    this.jsonData = null;  // 数据缓存
    this.cacheTime = 168;  // 24 小时
    this.path = '/';  // cookie path
}
History.prototype = {
    constructor: History
    //设置cookie //name 值 过期时间 
   , setCookie: function (name, value, expiresHours) {
       //options = options || {};
       var cookieString = name + '=' + encodeURIComponent(value) + ';path=' + this.path;
       //判断是否设置过期时间
       if (undefined != expiresHours) {
           var date = new Date();
           date.setTime(date.getTime() + expiresHours * 3600 * 1000);
           cookieString = cookieString + '; expires=' + date.toUTCString() + ';path=' + this.path;
       }
       document.cookie = cookieString;
   }
    //获取cookie
   , getCookie: function (name) {
       // cookie 的格式是个个用分号空格分隔
       var arrCookie = document.cookie ? document.cookie.split('; ') : [],
           val = '',
           tmpArr = '';

       for (var i = 0; i < arrCookie.length; i++) {
           tmpArr = arrCookie[i].split('=');
           tmpArr[0] = tmpArr[0].replace(' ', '');  // 去掉空格
           if (tmpArr[0] == name) {
               val = decodeURIComponent(tmpArr[1]);
               break;
           }
       }
       return val.toString();
   }
    //删除cookie
   , deleteCookie: function (name) {
       this.setCookie(name, '', -1, { "path": this.path });
       //console.log(document.cookie);
   }
    //初始化行
   , initRow: function ( link) {
       return '{ "link":"' + link + '"}';
   }
    //转换成json
   , parse2Json: function (jsonStr) {
       var json = [];
       try {
           json = JSON.parse(jsonStr);
       } catch (e) {
           //alert('parse error');return;
           json = eval(jsonStr);
       }

       return json;
   }

    // 添加记录
   , add: function (link) {
       var jsonStr = this.getCookie(this.key);
       //alert(jsonStr); return;

       if ("" != jsonStr) {
           this.jsonData = this.parse2Json(jsonStr);

           // 排重
           for (var x = 0; x < this.jsonData.length; x++) {
               if (link == this.jsonData[x]['link']) {
                   return false;
               }
           }
           // 重新赋值 组装 json 字符串
           jsonStr = '[' + this.initRow(link) + ',';
           for (var i = 0; i < this.limit - 1; i++) {
               if (undefined != this.jsonData[i]) {
                   jsonStr += this.initRow( this.jsonData[i]['link']) + ',';
               } else {
                   break;
               }
           }
           jsonStr = jsonStr.substring(0, jsonStr.lastIndexOf(','));
           jsonStr += ']';

       } else {
           jsonStr = '[' + this.initRow( link) + ']';
       }

       //alert(jsonStr);
       this.jsonData = this.parse2Json(jsonStr);
       this.setCookie(this.key, jsonStr, this.cacheTime, { "path": this.path });
   }
    // 得到记录
   , getList: function () {
       // 有缓存直接返回
       if (null != this.jsonData) {
           return this.jsonData;  // Array
       }
       // 没有缓存从 cookie 取
       var jsonStr = this.getCookie(this.key);
       if ("" != jsonStr) {
           this.jsonData = this.parse2Json(jsonStr);
       }

       return this.jsonData;
   }
    // 清空历史
   , clearHistory: function () {
       this.deleteCookie(this.key);
       this.jsonData = null;
   }
};
