function nTabs(thisObj, Num) {
    if (thisObj.className == "active") return;
    var tabObj = thisObj.parentNode.id;
    var tabList = document.getElementById(tabObj).getElementsByTagName("li");
    for (i = 0; i < tabList.length; i++) {
        if (i == Num) {
            thisObj.className = "active";
            try {
                document.getElementById(tabObj + "_Content" + i).style.display = "block";
            } catch (e) { }
        } else {
            tabList[i].className = "normal";
            try {
                document.getElementById(tabObj + "_Content" + i).style.display = "none";
            } catch (e) { }
        }
    }
}