<%@ Page Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="AddGroupBuy.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.PromoteSales.AddGroupBuy" %>
<%@ Register TagPrefix="YSWL" TagName="AjaxRegion" Src="~/Controls/AjaxRegion.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
            <link href="/Scripts/jqueryui/jquery-ui-1.8.19.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jqueryui/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <script src="/admin/js/jqueryui/JqueryDataPicker4CN.js" type="text/javascript"></script>
      <link href="/Admin/js/select2-2.1/select2.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/select2-2.1/select2.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery/regionjs.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("[id$='ddlProduct']").select2();
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
            $("[id$='txtLimitCount']").OnlyNum();
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="团购活动管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal6" runat="server" Text="您可以进行新增团购商品操作" />
                    </td>
                </tr>
            </table>
        </div>

        <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
            <tr>
                <td class="tdbg">
                    <table cellspacing="0" cellpadding="3" width="100%" border="0">
                        <tr id="txtCategory" >
                            <td class="td_class" >
                                <asp:Literal ID="Literal3" runat="server" Text="商品分类" />：
                            </td>
                            <td height="25">
                              <asp:DropDownList ID="ddlCateList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCateList_Changed" Width="352px">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlCateList2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCateList2_Changed" Width="352px" Visible="False">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr >
                            <td class="td_class">
                                <asp:Literal ID="Literal9" runat="server" Text="商品"  />：
                            </td>
                            <td height="25">
                                <asp:DropDownList ID="ddlProduct" runat="server" Width="352px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                      
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal7" runat="server" Text="开始时间" />：
                            </td>
                            <td height="25">
                                 <asp:TextBox ID="txtStartDate" runat="server" Width="352" MaxLength="30" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal14" runat="server" Text="结束时间" />：
                            </td>
                            <td height="25">
                                 <asp:TextBox ID="txtEndDate" runat="server" Width="352" MaxLength="30" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal15" runat="server" Text="团购价格" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtPrice" runat="server" Width="352px" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                            <tr>
                                    <td class="td_class">
                                             <asp:Literal ID="Literal12" runat="server" Text="所在地" />：
                                    </td>
                                    <td height="25">
                                        <YSWL:AjaxRegion runat="server" ID="ajaxRegion" />
                                    </td>
                                </tr>
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal8" runat="server" Text="商品限购总数量" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtMaxCount" runat="server" Width="352px" MaxLength="30" Text="0"></asp:TextBox>
                                      <asp:RangeValidator ID="RangeValidator1" Display="Dynamic" Type="Integer" ControlToValidate="txtMaxCount"
                                    MinimumValue="0" MaximumValue="1000000" runat="server" ErrorMessage="请填写数字"></asp:RangeValidator>
                            </td>
                        </tr>

                                                <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal13" runat="server" Text="单次购买限购数量" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtLimitCount" runat="server" Width="352px" MaxLength="30" Text="1"></asp:TextBox>
                            </td>
                        </tr>


                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal10" runat="server" Text="违约金" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtFinePrice" runat="server" Width="352px" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal11" runat="server" Text="团购满足数量" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtGroupCount" runat="server" Width="352px" MaxLength="30" Text="0"></asp:TextBox>
                                      <asp:RangeValidator ID="RangeValidator2" Display="Dynamic" Type="Integer" ControlToValidate="txtGroupCount"
                                    MinimumValue="0" MaximumValue="1000000" runat="server" ErrorMessage="请填写数字"></asp:RangeValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal4" runat="server" Text="显示顺序" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtSequence" runat="server" Width="352px" MaxLength="30"></asp:TextBox>
                                <asp:RangeValidator ID="RangeValidator3" Display="Dynamic" Type="Integer" ControlToValidate="txtSequence"
                                    MinimumValue="0" MaximumValue="1000000" runat="server" ErrorMessage="请填写数字"></asp:RangeValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal2" runat="server" Text="" />
                            </td>
                            <td height="25">
                                  <asp:CheckBox ID="chkStatus" Text="上架" runat="server" Checked="True" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal5" runat="server" Text="活动说明" />
                            </td>
                            <td height="25">
                                     <asp:TextBox ID="txtDesc" runat="server" Width="352px"  TextMode="MultiLine" Rows="3"> </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                            </td>
                            <td height="25">
                                <asp:Button ID="btnSave" runat="server" Text="确定"
                                    OnClick="btnSave_Click" class="adminsubmit_short"></asp:Button>&nbsp;&nbsp;
                            </td>
                        </tr>
                        
                    </table>
                </td>
            </tr>
        </table>
        <br />
    </div>
</asp:Content>


