 
<%@ Page Title="增加用户" Language="C#" MasterPageFile="~/Admin/BasicNoFoot.Master" CodeBehind="AddUser.aspx.cs"
    Inherits="YSWL.MALL.Web.Admin.Shop.Order.SubmitOrder.AddUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    $(function () {
        //保存收货地址
        $('#btnsave').click(function () {
            $(this).attr({ "disabled": "disabled" });
            var username = $('[id$="txtuserName"]').val();
            if (username == "") {
                ShowFailTip("请填写用户手机号！");
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
           if (isNaN(regionId) || regionId <= 0) {
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
               url: ("AddUser.aspx?timestamp={0}").format(new Date().getTime()),
               type: 'POST', dataType: 'json', timeout: 10000,
               data: { Action: "AddInfo", Callback: "true", username: username, shipname: shipname, shipphone: shipphone, regionId: regionId, address: address },
               success: function (resultData) {
                   if (resultData.STATUS == "SUCCESS") {
                       if (resultData.DATA == "DepotIsNot") {//没有仓库信息
                           alert("保存成功！但该地区下未设置仓库. \n确认后将回到选择用户页面....");
                           javascript: parent.$.colorbox.close();//关闭弹窗
                           var $parentDocument = $(window.parent.document);//显示页面内容
                           $parentDocument.find('id=table_content').show();
                           $parentDocument.find('[id$="lblName"]').text(username);
                           $parentDocument.find('[id$="txtPhone"]').val(phone);
                           $parentDocument.find('[id$="txtshipName"]').val(shipname);
                           $parentDocument.find('[id$="txtshipPhone"]').val(shipphone);
                           $parentDocument.find('[id$="txtAddress"]').val(address);
                           $parentDocument.find('#hidRegionId').val(regionId);
                           //用户id
                           $parentDocument.find('#txtUserId').val(resultData.UserId);
                           $parentDocument.find('#select2-chosen-1').text(username);                         
                           if ($parentDocument.find('#hfSelectedNode').val() != regionId) {                             
                             //重新加载地区
                             window.parent.window.loadRegion(regionId);
                           }                         
                       } else {
                               window.parent.location.href = "ProductSKUList.aspx";
                       }                     
                   }
                   else {
                       switch (resultData.DATA) {
                           case "UserNameIsNull":
                               ShowFailTip('用户名不能为空！');
                               break;
                           case "HasUserName":
                               ShowFailTip('用户名已存在！');
                               break;
                           case "ADDFAILED":
                               ShowFailTip('新增失败！');
                               break;                               
                       }
                   }
                   $('#btnsave').removeAttr("disabled");
               }
           });
           $(this).removeAttr("disabled");
        });


   });
 
</script>
    </asp:Content>
 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      
    <div class="newslistabout">
               <table  style="width: 100%;    padding-top: 15px;"  cellspacing="0" cellpadding="0" width="100%" border="0"  class="border">                       
                        <tr>
                            <td class="td_class">
                               <span class="red">* </span>用户手机号码：
                            </td>
                            <td  >
                                 <asp:TextBox   ID="txtuserName" runat="server"></asp:TextBox>     
                            </td>                     
                            <td class="td_class">                                
                            </td>
                        </tr>
                          <tr>
                            <td class="td_class">
                               <span class="red">* </span>收货人 ：
                            </td>
                            <td  >
                                  <asp:TextBox   ID="txtshipName" runat="server"></asp:TextBox>
                            
                            </td>
                             <td class="td_class">
                               <span class="red">* </span>收货人手机号码  ：
                            </td>
                            <td  colspan="2">
                                <asp:TextBox   ID="txtshipPhone" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <span class="red">* </span>收货地区 ：
                            </td>
                            <td  colspan="3"  id="ship_address">
                               <div id="region_h">   
                                    <script src="/Scripts/jquery/jquery.guid.js" type="text/javascript"></script>
                                    <input type="hidden" id="hfSelectedNode" value="" />
                                    <script src="/Scripts/jquery/maticsoft.selectregion.js" handle="/RegionHandle.aspx" isnull="true" type="text/javascript"></script>
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
                                <input type="button"  value="保存"  id="btnsave" class="adminsubmit_short" />
                            </td>
                        </tr>
                    </table>
            <div id="txtDiv">
                </div>
        </div>
</asp:Content>
