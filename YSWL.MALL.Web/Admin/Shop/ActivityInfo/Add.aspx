<%@ Page Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="Add.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.ActivityInfo.Add"
    Title="增加页" %>
<%@ Register TagPrefix="YSWL" TagName="CategoriesDropList" Src="~/Controls/CategoriesDropList.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Scripts/jqueryui/jquery-ui-1.8.19.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jqueryui/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <script src="/admin/js/jqueryui/JqueryDataPicker4CN.js" type="text/javascript"></script>
    <link href="/Admin/js/select2-2.1/select2.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/select2-2.1/select2.min.js" type="text/javascript"></script>
    <link href="/Scripts/jqueryui/jquery-ui-timepicker-addon.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jqueryui/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("[id$='ddlBuyProductId']").select2();
            $("[id$='ddlProductId']").select2({ placeholder: "请选择" });
            $.datepicker.setDefaults($.datepicker.regional['zh-CN']);
            $("[id$='txtStartDate']").prop("readonly", true).datepicker({
                changeMonth: true,
                dateFormat: "yy-mm-dd",
                onClose: function (selectedDate) {
                    $("[id$='txtEndDate']").datepicker("option", "minDate", selectedDate);
                }
            });
            $("[id$='txtEndDate']").prop("readonly", true).datepicker({
                changeMonth: true,
                dateFormat: "yy-mm-dd",
                onClose: function (selectedDate) {
                    $("[id$='txtStartDate']").datepicker("option", "maxDate", selectedDate);
                    $("[id$='txtEndDate']").val($(this).val());
                }
            });

//            $("[id$='ddlRule']").click(function () {
//                var ruleId = $(this).val();
//                if (ruleId == 3) {
//                    $(".txtCoupon").show();
//                    $(".txtProduct").hide();
//                } else {
//                    $(".txtCoupon").hide();
//                    $(".txtProduct").show();
//                }
//            });

            $("[id$='txtMaxCount']").OnlyNum();
            $("[id$='txtCount']").OnlyNum();
            $("[id$='txtLimitPrice']").OnlyFloat();
            $("[id$='txtBuyCount']").OnlyFloat();



            $("[id$='ddlBuyProductId']").select2({ placeholder: "请选择" });

            $('#td_cate select').die('change').live('change', function () {
                GetProductList(false);
            });

            $('[id$="ddlBuyProductId"]').change(function () {
                $('[id$="hidbuyproductId"]').val($(this).val());
            });

            //回发
            if (IsPostBack()) {
                GetProductList(true); //回发后值下拉框中的值保持不变
            }
        });


        //IspostBack
        function IsPostBack() {
            var IsPostBack = "<%=IsPostBack%>";
            if (IsPostBack == "True") {
                return true;
            } else {
                return false;
            }
        }

        function GetProductList(isback) {
            var this_val = parseInt($('[id$="hfSelectedNode"]').val());
            if (this_val <= 0) {
                $('[id$="ddlBuyProductId"]').empty().append('<option></option>');
                $("[id$='ddlBuyProductId']").select2({ placeholder: "请选择" });
                $('[id$="hidbuyproductId"]').val(''); 
                return;
            }
            $.ajax({
                url: ("Add.aspx?timestamp={0}").format(new Date().getTime()),
                type: 'POST', dataType: 'json', timeout: 10000,
                data: { Action: "GetProductList", Callback: "true", CId: this_val },
                success: function (resultData) {
                    var json = eval(resultData);
                    var strValue = '';
                    if (resultData.STATUS == "Success") {
                        var list = json.List;
                        for (var i = 0; i < list.length; i++) {
                            strValue += '<option value="' + list[i].Id + '">' + list[i].Name + '</option>';
                        }
                    } else {
                        strValue = '<option>请选择</option>';
                        ShowFailTip('服务器繁忙，请稍候再试！');
                    }
                    $('[id$="ddlBuyProductId"]').empty();
                    $('[id$="ddlBuyProductId"]').append(strValue);
                    if (isback) {
                        $('[id$="ddlBuyProductId"]').val($('[id$="hidbuyproductId"]').val()).select2();
                    } else {
                        $("[id$='ddlBuyProductId']").select2({ placeholder: "请选择" });
                        $('[id$="hidbuyproductId"]').val(''); 
                    }
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="促销活动管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal6" runat="server" Text="您可以进行新增促销活动操作" />
                    </td>
                </tr>
            </table>
        </div>
        <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
            <tr>
                <td class="tdbg">
                    <table cellspacing="0" cellpadding="3" width="100%" border="0">
                        <tr>
                            <td class="td_class">
                                规则 ：
                            </td>
                            <td height="25">
                                <asp:DropDownList ID="ddlRule" runat="server"  AutoPostBack=true 
                                    onselectedindexchanged="ddlRule_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="txtCategory"  >
                            <td class="td_class">
                                <asp:Literal ID="Literal3" runat="server" Text="指定分类" />：
                            </td>
                            <td height="25" id="td_cate">
                                <YSWL:CategoriesDropList ID="ddlCateList" runat="server" IsNull="true" />
                            </td>
                        </tr>
                        <tr >
                            <td class="td_class">
                                购买指定的商品 ：
                            </td>
                            <td height="25">
                            <asp:HiddenField ID="hidbuyproductId" runat="server" />
                              <select id="ddlBuyProductId" style="width: 500px;">
                                    <option></option>
                                </select>
                                <span>(提示：先根据商品分类进行筛选商品)</span>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td class="td_class">
                                指定的商品SKU ：
                            </td>
                            <td height="25">
                                <label runat="server" id="lblBuySKU">
                                </label>
                            </td>
                        </tr>
                        <tr >
                            <td class="td_class">
                                指定数量 ：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtBuyCount" runat="server" Width="50px" >1</asp:TextBox>
                            </td>
                        </tr>
                        <tr   id="trgift"   runat="server"  visible="false">
                            <td class="td_class">
                                赠品 ：
                            </td>
                            <td height="25">
                                <asp:DropDownList ID="ddlProductId" runat="server" Width="500px">
                                </asp:DropDownList>
                                <span></span>
                            </td>
                        </tr>
                        <tr   runat="server" id="trCoupon"  visible="false">
                            <td class="td_class">
                                优惠券 ：
                            </td>
                            <td height="25">
                                <asp:DropDownList ID="ddlCoupon" runat="server">
                                </asp:DropDownList>
                                <span></span>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td class="td_class">
                                商品SKU ：
                            </td>
                            <td height="25">
                                <label runat="server" id="lblSKU">
                                </label>
                            </td>
                        </tr>
                        <tr runat="server" id="trCount">
                            <td class="td_class">
                                赠送数量 ：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtCount" runat="server" Width="50px">1</asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                消费金额区间 ：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtLimitPrice" runat="server" Width="50px"></asp:TextBox> -- <asp:TextBox ID="txtLimitMaxPrice" runat="server" Width="50px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td class="td_class">
                                促销总价 ：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtSalePrice" runat="server" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td class="td_class">
                                限售总数量 ：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtMaxCount" runat="server" Width="50px"></asp:TextBox>
                            </td>
                        </tr>
                        
                        <tr>
                            <td class="td_class">
                                有效时间 ：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtStartDate" runat="server" Width="100px"></asp:TextBox>
                                -- <asp:TextBox ID="txtEndDate" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                状态 ：
                            </td>
                            <td height="25">
                                <asp:CheckBox ID="checkboxStatus" Checked="true" runat="server" />是否启用
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                            </td>
                            <td height="25">
                                <asp:Button ID="btnCancle" runat="server" Text="取消" OnClick="btnCancle_Click" class="adminsubmit_short">
                                </asp:Button>&nbsp;&nbsp;
                                <asp:Button ID="btnSave" runat="server" Text="确定" OnClick="btnSave_Click" class="adminsubmit_short">
                                </asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
