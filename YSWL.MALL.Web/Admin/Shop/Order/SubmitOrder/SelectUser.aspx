 
<%@ Page Title="用户代下单功能" Language="C#" MasterPageFile="~/Admin/Basic.Master" CodeBehind="SelectUser.aspx.cs"
    Inherits="YSWL.MALL.Web.Admin.Shop.Order.SubmitOrder.SelectUser" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <link href="/Admin/js/select2-3.4.6/select2.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/select2-3.4.6/select2.min.js" type="text/javascript" charset="utf-8"></script>
    <script src="/Scripts/jquery/jquery.guid.js" type="text/javascript"></script>
     <link href="/Admin/js/colorbox/colorbox.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/colorbox/jquery.colorbox-min.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $(".iframe").colorbox({ iframe: true, width: "840", height: "330", overlayClose: false, href: "AddUser.aspx" });

        $(".select2-container").css("vertical-align", "middle");

        $("#txtUserId").select2({
            placeholder: "输入用户手机号",
            minimumInputLength: 1,
            formatInputTooShort: "请输入至少一个字符",
            formatNoMatches: "没有匹配项",
            formatSearching: "正在查询......",
            ajax: {
                url: "/User.aspx",
                type: "POST",
                dataType: 'json',
                quietMillis: 100,
                data: function (term, page) { // page is the one-based page number tracked by Select2
                    return {
                        Action: "GetUserList",
                        q: term, //search term
                        page_limit: 10, // page size
                        page: page // page number
                    };
                },
                results: function (data, page) {
                    var more = (page * 10) < data.total; // whether or not there are more results available
                    return { results: data.List, more: more };
                }
            },
            formatResult: Format, // omitted for brevity, see the source of this page
            escapeMarkup: function (m) { return m; } // we do not want to escape markup since we are displaying html in results

        });

        //选中事件
        $("#txtUserId").change(function () { 
            $.ajax({
                url: ("SelectUser.aspx?timestamp={0}").format(new Date().getTime()),
                type: 'POST', dataType: 'json', timeout: 10000,
                data: { Action: "GetUserInfo", Callback: "true", UserId: $(this).val() },
                success: function (resultData) {
                    $('#table_content').show();
                    if (resultData.STATUS == "SUCCESS") {
                        var data=resultData.DATA;                       
                        $('[id$="lblName"]').text(data.userName);
                        $('[id$="txtshipName"]').val(data.shipName);
                        $('[id$="txtshipPhone"]').val(data.shipCelPhone);
                        $('[id$="txtAddress"]').val(data.shipAddress);
                        $('#hidRegionId').val(data.shipRegionId);
                        if ($('[id$="hfSelectedNode"]').val() != data.shipRegionId) {
                            //重新加载地区
                            loadRegion(data.shipRegionId);
                        }
                    }
                    else {
                        ShowFailTip('服务器繁忙，请稍候再试！');
                    }
                }
            });
        });
        //保存收货地址
        $('#btnsave').click(function () {
            $(this).attr({ "disabled": "disabled" });
            var userId=parseInt($("#txtUserId").val());
            if (isNaN(userId) || userId <=0) {
                ShowFailTip("请先选择用户！");
                $(this).removeAttr("disabled");
                return;
            }
           var shipname = $('[id$="txtshipName"]').val();
           if (shipname == "") {
               ShowFailTip("请填写收货人！");
               $(this).removeAttr("disabled");
               return;
           }
           var shipphone = $('[id$="txtshipPhone"]').val();
           if (shipphone == "") {
               ShowFailTip("请填写收货人手机号码！");
               $(this).removeAttr("disabled");
               return;
           }
           var regionId = parseInt($('[id$="hfSelectedNode"]').val());
           if (isNaN( regionId)  ||  regionId<=0) {
               ShowFailTip("请选择收货地区！");
               $(this).removeAttr("disabled");
               return;
           }
           var address = $('[id$="txtAddress"]').val();
           if (address == "") {
               ShowFailTip("请填写收货地址！");
               $(this).removeAttr("disabled");
               return;
           }     
           $.ajax({
               url: ("SelectUser.aspx?timestamp={0}").format(new Date().getTime()),
               type: 'POST', dataType: 'json', timeout: 10000,
               data: { Action: "UpdateInfo", Callback: "true", userId: userId, shipname: shipname, shipphone: shipphone, regionId: regionId, address: address },
               success: function (resultData) {
                   switch (resultData.STATUS) {
                       case "SUCCESS":
                           $('#hidRegionId').val(regionId);
                           location.href = "productskulist.aspx";                          
                           break;
                       case "FAILED":
                           if (resultData.DATA == "DepotIsNot") {
                               ShowFailTip("该地区下未设置仓库！");
                           } else if (resultData.DATA == "UserIdIsNULL") {
                               ShowFailTip("请先选择用户！");         
                           }
                           break;
                       default:
                           ShowFailTip('保存失败！');
                           break;
                   }
               },
               error: function (XMLHttpRequest, textStatus, errorThrown) {
           }
           });
           $(this).removeAttr("disabled");
        });
 
 
   });
  
    function Format(data) {
        return data.text;
    }
    function loadRegion(regionId) {
        $('[id$="hfSelectedNode"]').val(regionId);
        $('#ship_address select').remove();
       $('#region_h').empty().append("<script src=\"/Scripts/jquery/maticsoft.selectregion.js\" handle=\"/RegionHandle.aspx\" isnull=\"true\" type=\"text/javascript\">\<\/script>");
    }

</script>
    </asp:Content>
 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      
    <div class="newslistabout">
       
            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="height:100px;">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                    <asp:Literal ID="LiteralSupplier" runat="server" Text="选择用户" />：
                       <input id="txtUserId" type="text" style="width: 280px" />
                        <input id="hidRegionId" type="hidden"   />
                      <%--  <input type="button"  class="adminsubmit" id="butCenter"  value="确定"  />--%>                     
                        <input type="button"  class="adminsubmit  iframe" id="butAdd" style="display:none"  value="新增用户"  />
                    </td>
                </tr>
            </table>
        <br />
            <table style="width: 100%;display:none;" cellpadding="0" cellspacing="0" class="border" id="table_content">
            <tr>
                <td class="tdbg">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">                       
                        <tr>
                            <td class="td_class">
                                用户：
                            </td>
                            <td  >
                                <asp:Label ID="lblName" runat="server"></asp:Label>
                            </td>                     
                            <td class="td_class">
                             
                            </td>
                            <td>
                              
                            </td>
                        </tr>
                          <tr>
                            <td class="td_class">
                                <span class="red">* </span>收货人 ：
                            </td>
                            <td>
                                  <asp:TextBox   ID="txtshipName" runat="server"></asp:TextBox>
                            
                            </td>
                             <td class="td_class">
                               <span class="red">* </span>收货人手机号码 ：
                            </td>
                            <td  colspan="2">
                                <asp:TextBox   ID="txtshipPhone" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <span class="red">* </span>收货地区 ：
                            </td>
                            <td  colspan="3" >
                                  <div   id="ship_address" style="display: inline-block;">
                                  <input type="hidden" id="hfSelectedNode" value="" />
                                    <div id="region_h">   
                                    <script src="/Scripts/jquery/maticsoft.selectregion.js" handle="/RegionHandle.aspx" isnull="true" type="text/javascript"></script>
                                   </div>  
                                  </div>                     
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <span class="red">* </span>详细地址 ：
                            </td>
                            <td  colspan="3" >
                                <asp:TextBox   ID="txtAddress" runat="server" style="width: 400px;"></asp:TextBox>
                            </td>
                          </tr>                        
                        <tr>                           
                            <td class="td_class"  colspan="4"  style=" text-align: center;">
                                <input type="button"  value="下一步"  id="btnsave" class="adminsubmit_short" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
 
        </div>
</asp:Content>
