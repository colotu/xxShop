 <%@ Page Language="C#" MasterPageFile="~/Admin/BasicNoFoot.Master" AutoEventWireup="true"
    CodeBehind="Apply.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.SubstituteReturn.Apply" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
    .reviewimg-upload span {
filter: alpha(opacity=50);
display: none;
line-height: 45px;
background-color: #333;
color: #ffffff;
width: 35px;
margin-left: -51px;
padding-left: 10px;
margin-right: 6px;
}
.reviewimg-upload img {
margin-right: 6px;
}
img {
-ms-interpolation-mode: bicubic;
vertical-align: bottom;
}
</style>
 <link href="/Scripts/jquery.upload/fineuploader-3.4.1.css" rel="stylesheet" type="text/css" />
 <script src="/Scripts/jquery.upload/jquery.fineuploader-3.4.1.min.js" type="text/javascript"></script> 
</asp:Content>
<%@ Register TagPrefix="cc1" Namespace="YSWL.MALL.Web.Controls" Assembly="YSWL.MALL.Web" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="新增退货申请" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal6" runat="server" Text="您可以进行新增退货申请操作" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="table_Items" style="">
        <cc1:GridViewEx ID="gridView" Width="100%" runat="server" AllowPaging="False" AllowSorting="True"
                                ShowToolBar="false" AutoGenerateColumns="False" OnBind="BindData"
                                PageSize="10" ShowExportExcel="False"
                                ShowExportWord="False" ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px"
                                ShowCheckAll="False" DataKeyNames="ItemId" Style="float: left;">
                                <Columns>
                                    <asp:TemplateField ControlStyle-Width="60" HeaderText="商品图片" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <a href="<%= YSWL.Components.MvcApplication.GetCurrentRoutePath(YSWL.Web.AreaRoute.Shop) %>product/Detail/<%# Eval("ProductId") %>" target="_blank">
                                                <asp:Image ID="Image1" runat="server" Width="60px" Height="60px" ImageAlign="Middle"
                                                    ImageUrl='<%# YSWL.MALL.Web.Components.FileHelper.GeThumbImage(Eval("ThumbnailsUrl").ToString(), "T128X130_")%>' />
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="商品名称" ItemStyle-HorizontalAlign="left">
                                        <ItemTemplate>
                                            <a href="<%= YSWL.Components.MvcApplication.GetCurrentRoutePath(YSWL.Web.AreaRoute.Shop) %>product/Detail/<%# Eval("ProductId") %>" target="_blank">
                                            <%#Eval("Name")%></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ControlStyle-Width="80" HeaderText="商品编号" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-Width="80">
                                        <ItemTemplate>
                                            <%#Eval("SKU")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ControlStyle-Width="80" HeaderText="商品属性" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-Width="150">
                                        <ItemTemplate>
                                          <%#GetSKUStr(Eval("SKU"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ControlStyle-Width="80" HeaderText="购买数量" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-Width="80">
                                        <ItemTemplate>
                                            <%# Eval("ShipmentQuantity")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ControlStyle-Width="80" HeaderText="是否退货" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-Width="80">
                                        <ItemTemplate>
                                          <input type="checkbox" id="checkboxprod_@(item.ItemId)"  itemId="<%#Eval("ItemId")%>"  prodType="<%# Eval("ProductType")%>" />  
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ControlStyle-Width="80" HeaderText="退货数量" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-Width="80" >
                                        <ItemTemplate>
                                            <input type="text"  style="width:25px;"  id="textcount_<%#Eval("ItemId")%>"   prodType="<%# Eval("ProductType")%>"  count="<%#Eval("ShipmentQuantity")%>" class="returnCount"  maxLength="2"  itemid="<%#Eval("ItemId")%>" />


                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle Height="25px" HorizontalAlign="Right" />
                                <HeaderStyle Height="35px" />
                                <PagerStyle Height="25px" HorizontalAlign="Right" />
                                <SortTip AscImg="~/Images/up.JPG" DescImg="~/Images/down.JPG" />
                                <RowStyle Height="25px" />
                                <SortDirectionStr>DESC</SortDirectionStr>
                            </cc1:GridViewEx>
        </div>
    <asp:HiddenField runat="server" ID="hidoid" />
 
        <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
            <tr>
                <td class="tdbg">
                    <table cellspacing="0" cellpadding="3" width="100%" border="0">
                     
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal3" runat="server" Text="退货类型" />：
                            </td>
                            <td height="25">
                               <input type="radio" name="returngoodstype" value="1" class="radio" id="returngoodstype_1">
                    <label style="cursor:pointer" value="1" for="returngoodstype_1">整单退</label>
                     <input type="radio" name="returngoodstype" checked="checked" value="2" class="radio" id="returngoodstype_2">
                    <label style="cursor:pointer" value="2"   for="returngoodstype_2">部分退</label>
                            </td>
                        </tr>

                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal7" runat="server" Text="服务类型" />：
                            </td>
                            <td height="25">
                               <input type="radio" name="serviceType" value="1" checked="checked" class="radio" id="serviceType_click_1">
                    <label style="cursor:pointer" name="serviceTypeLabel" value="1" for="serviceType_click_1">退货</label>
                     <input type="radio" name="serviceType" value="2" class="radio" id="serviceType_click_2" style="display:none;">
                    <label style="cursor:pointer;display:none;" name="serviceTypeLabel" value="2" for="serviceType_click_2"  >换货</label>
                     <input type="radio" name="serviceType" value="3" class="radio" id="serviceType_click_3" style="display:none;">
                    <label style="cursor:pointer;display:none;" name="serviceTypeLabel" value="3" for="serviceType_click_3" >维修</label>
                    
                            </td>
                        </tr>
     
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal8" runat="server" Text="退货原因" />：
                            </td>
                            <td height="25">
                                 <textarea  id="apply_Content"   style="resize: none;width:400px;height:100px" maxLength="500"  class="area f-txt-b f-txt"></textarea>
                            </td>
                        </tr>
                         <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal2" runat="server" Text="图片信息" />：
                            </td>
                            <td height="25">
                               <div class="reviewimg-upload">
                        <input type="hidden" value="" name="UploadPhotoPath"/>
                        <input type="hidden" value="" name="UploadPhotoNames"/>
                        <div name="UploadPhoto"  style="padding-left: 5px; width:70px; float:left;">
                        </div>
                           </div>
                            </td>
                        </tr>
                           <tr style="display:none;">
                            <td class="td_class">
                                <asp:Literal ID="Literal4" runat="server" Text="商品返回方式" />：
                            </td>
                            <td height="25">
                             <input type="radio" name="pickWareType" value="0" id="pickWareType_0" checked="checked"  class="radio">
                <label for="pickWareType_0">默认(暂未设置)</label>
                                 <input type="radio" name="pickWareType" value="1" id="pickWareType_1" class="radio">
                <label for="pickWareType_1">上门取件</label><div class="msg-text"></div>
                            </td>
                        </tr>
                           <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal5" runat="server" Text="取货地区" />：
                            </td>
                            <td height="25">
                                <script src="/Scripts/jquery/jquery.guid.js" type="text/javascript"></script>
                                <asp:HiddenField ID="hfSelectedNode" runat="server" Value="" />
                                        <input type="hidden" value="@(Model.RegionId.HasValue?Model.RegionId.Value:0)" />
                                        <script src="/Scripts/jquery/maticsoft.selectregion.js" handle="/RegionHandle.aspx" isnull="true" type="text/javascript"></script>
                            </td>
                             </tr>
   <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal9" runat="server" Text="详细地址" />：
                            </td>
                            <td height="25">
                            <asp:TextBox ID="txtPick_Address" runat="server"></asp:TextBox>
                           
                            </td>
                              </tr>
                       
   <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal10" runat="server" Text="联系人姓名" />：
                            </td>
                            <td height="25">
                              <asp:TextBox ID="txtApplyUserName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                           <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal11" runat="server" Text="手机号码" />：
                            </td>
                            <td height="25">
                              <asp:TextBox ID="txtApplyPhone" runat="server"></asp:TextBox>
                              
                            </td>
                        </tr>
                      
                        <tr>
                            <td class="td_class">
                            </td>
                            <td height="25">
                            <input type="button" id="btnSubmit" class="adminsubmit_short"  value="提交申请"/>
                         
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
<script type="text/javascript">
$(function () {
    $('.returnCount').OnlyNum();

   $('[id^="checkboxprod_"]').each(function(){
     if(parseInt($(this).attr('prodType'))==2){
        $(this).attr('checked','checked');
        $(this).attr('disabled','disabled');
        $(this).attr('prtype','2');
     }else{
        $(this).attr('prtype','1');
     }
   });
 
   $('[id^="textcount_"]').each(function(){
     if(parseInt($(this).attr('prodType'))==2){
        $(this).attr('disabled','disabled');
        $(this).val($(this).attr("count"));
     }
   });
   

    //点击退货类型
    $('[name="returngoodstype"]').click(function(){
        var _val=parseInt($(this).val());
        if(_val==1 ){
            $('#table_Items').hide();
        }else{
            $('#table_Items').show();
        }
    });

      //上传图片
     qqupload();
    //改变上传图片按钮的样式
     $('.qq-upload-button').css({'backgroundColor':'#A59D89','width':'55','padding':'5','margin-top':'7'});

    
    $('#btnSubmit').click(function () {

        //获取要退货的商品 (不包含赠品,赠品在后台获取)
        var oid = $('[id$="hidoid"]').val();
        if (isNaN(oid)) {
            //ShowFailTip("");
            return;
        }

        //退货类型
        var returngoodstype = parseInt($('[name="returngoodstype"]:checked').val());
        if (isNaN(returngoodstype) || returngoodstype <= 0) {
            ShowFailTip("请选择退货类型");
            return;
        }

        
         //如果只有赠品（没有正常销售的商品）就把退货类型改为整单退  
        if($('[id^="checkboxprod_"][prtype=1]').length<=0){
            returngoodstype=1;
        }

        var items = $('[id^="checkboxprod_"][prtype=1]:checked');
        var json = [];
        if(returngoodstype!=1){ //1是 整单退 在后台获取
             if (items.length <= 0) {
                 ShowFailTip("请选择商品");
                 return;
             }
      
             var itemId = 0;
             var count = 0;
             for (var i = 0; i < items.length; i++) {
                    itemId = parseInt(items.eq(i).attr('itemId'));
                    count = parseInt($('#textcount_' + itemId).val());
                    if (isNaN(count) || count <= 0) {
                          ShowFailTip("退货数量必须大于0");
                          return;
                     }
                     json.push({ "itemId": itemId, "count": count });
               }
         }

        var serviceType = parseInt($('[name="serviceType"]:checked').val());
        if (isNaN(serviceType) || serviceType <= 0) {
            ShowFailTip("请选择服务类型");
            return;
        }
        var content = $.trim($('#apply_Content').val());
        if (content.length <= 0) {
            ShowFailTip("请填写退货原因");
            return;
        }
        var pickWareType = parseInt($('[name="pickWareType"]:checked').val());
        if (isNaN(pickWareType) || pickWareType < 0) {
            ShowFailTip("请选择返回方式");
            return;
        }

        var regionId = parseInt($('[id$="hfSelectedNode"]').val());
        if (isNaN(regionId) || regionId <= 0) {
            ShowFailTip("请选择地区");
            return;
        }

        var address = $.trim($('[id$="txtPick_Address"]').val());
        if (address.length <= 0) {
            ShowFailTip("请填写地址");
            return;
        }

        var name = $.trim($('[id$="txtApplyUserName"]').val());
        if (name.length <= 0) {
            ShowFailTip("请填写联系人姓名");
            return;
        }

        var phone = $.trim($('[id$="txtApplyPhone"]').val());
        if (phone.length <= 0) {
            ShowFailTip("请填写联系人电话");
            return;
        }
        var regs = /^1([38][0-9]|4[57]|5[^4])\d{8}$/;
        if (!regs.test(phone)) {
            ShowFailTip("请填写有效的手机号码！");
            return;
        }

        var imagesurlPath = $('[name="UploadPhotoPath"]').val();
        var imagesurlName = $('[name="UploadPhotoNames"]').val();
 
        $.ajax({
            type: "POST",
            dataType: "text",
            timeout: 10000,         
            url: ("Apply.aspx?timestamp={0}").format(new Date().getTime()),
            data: { Action: "Apply", Callback: "true", oId: oid,RGoodsType:returngoodstype,Items: JSON.stringify(json), ServiceType: serviceType, Content: content, PickType: pickWareType, RegionId: regionId, Address: address, Name: name, Phone: phone,ImagesurlPath:imagesurlPath, ImagesurlName:imagesurlName},
            success: function (data) {
                switch (data) {
                    case "True":
                        ShowSuccessTip("提交成功");
                        $(parent.document).find('[id$=btnSearch]').click();
                        //window.location.replace("../ReturnOrder/List.aspx"); 
                        //跳转
                        break;
                    case "Fail":
                        ShowFailTip("提交失败");
                        break;
                    case "SELECTALLITEMS":
                        ShowFailTip("若要将全部商品退回,请使用【整单退】");
                        break;
                    case "IsNotMeetCondition":
                        //不满足退单条件 
                        break;
                    case "Illegal":
                        //数据重复
                        break;
                    case "COUNTISTOOBIG":
                        //数量超过购买数量
                        break;
                    case "ITEMSISNULL": ////项为空
                        break;
                    case "AMOUNTISNULL":
                        //总金额小于=0
                        break;
                    case "False":
                        //数据不完整
                        break;
                    default:
                        break;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                ShowFailTip("操作失败：" + errorThrown);
            }
        });
    });


    var self_val = 0;
    $('.returnCount').focus(function () {
        self_val = $(this).val();
    }).blur(function () {
        var count = $(this).attr('count');
        var _val = $(this).val();
        if (count < _val) {
            ShowFailTip("退货数量不能大于购买数量！");
            $(this).val(self_val);
            return false;
        }
    });

});

//上传图片按钮
    var qqupload = function() {
        var ulbtnparent = $("[name='UploadPhoto']").parent();
        new qq.FineUploader({
            element: $("[name='UploadPhoto']")[0],
            request: {
                endpoint: '/UploadMultipleFileHandler.aspx'
            },
            text: {
                uploadButton: '上传图片'
            },
            multiple: true,
            validation: {
                allowedExtensions: ['jpeg', 'jpg', 'gif', 'png'],
                itemLimit: 5,
                sizeLimit: 5242880, // 50 kB = 50 * 1024 bytes
            },
            callbacks: {
                onComplete: function(id, fileName, responseJSON) {
                    $(".qq-upload-list").hide();
                    if (responseJSON.success) {
                         ulbtnparent.append(('<div style="display:inline-block;line-height: 45px;"><img src="{0}"  width="45px" height="45px"/><span  onclick="nameDel(this);"  item="{0}" itemname="{1}"  >删除</span></div>').format(
                            responseJSON.path.format(responseJSON.names), responseJSON.names));
                        ShowSuccessTip('上传成功！');
                        ulbtnparent.find('[name="UploadPhotoPath"]').val(ulbtnparent.find('[name="UploadPhotoPath"]').val() + '|' + responseJSON.path.format(responseJSON.names));
                        ulbtnparent.find('[name="UploadPhotoNames"]').val(ulbtnparent.find('[name="UploadPhotoNames"]').val() + '|' + responseJSON.names);
                        imghover();

                    } else {
                        ShowFailTip("服务器没有返回数据，可能服务器忙，请稍候再试：");
                    }
                }
            }
        });
    };


      //鼠标移入移出图片
    var imghover = function() {
       $('.reviewimg-upload').find('img').parent('div').unbind('hover').hover(function () {  
          $(this).find('span').css('display','inline-block');
       }, function () {
          $(this).find('span').css('display','none');
       });
    };

     //删除图片
      function nameDel(sender) {
        var ulbtnparent=  $(sender).parents('.reviewimg-upload');
        var targetVal =  $(sender).attr('item');
        $(sender).parent('div').remove();
        var pathArray =ulbtnparent.find('[name="UploadPhotoPath"]').val().split('|');
        var index = pathArray.getIndexByValue(targetVal);
        pathArray.remove(index);
        ulbtnparent.find('[name="UploadPhotoPath"]').val(pathArray.join('|'));

        var nameVal = $(sender).attr('itemname');
        var nameArray = ulbtnparent.find('[name="UploadPhotoNames"]').val().split('|');
        var indexname = nameArray.getIndexByValue(nameVal);
        nameArray.remove(indexname);
        ulbtnparent.find('[name="UploadPhotoNames"]').val(nameArray.join('|'));
    }
</script>

    </asp:Content>