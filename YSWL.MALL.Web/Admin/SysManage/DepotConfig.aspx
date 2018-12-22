<%@ Page Title="多仓设置" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="DepotConfig.aspx.cs" Inherits="YSWL.MALL.Web.Admin.SysManage.DepotConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var IsMultiDepot = $("#ctl00_ContentPlaceHolder1_chk_OpenMultiDepot").attr("checked");
            if (IsMultiDepot) {
                $(".isOpenDepot").show();
            }
            $("#ctl00_ContentPlaceHolder1_chk_OpenMultiDepot").click(function () {
                var open = $(this).attr("checked");
                if (open) {
                    $(".isOpenDepot").show();
                } else {
                    $(".isOpenDepot").hide();
                }
            });

            //var isOMS = $("#ctl00_ContentPlaceHolder1_chk_ConOMS").attr("checked");
            //if (isOMS) {
            //    $("#ddlOMSUrl").show();
            //}
            //$("#ctl00_ContentPlaceHolder1_chk_ConOMS").click(function () {
            //    var open = $(this).attr("checked");
            //    if (open) {
            //        $("#ddlOMSUrl").show();
            //    } else {
            //        $("#ddlOMSUrl").hide();
            //    }
            //});

            var isPMS = $("#ctl00_ContentPlaceHolder1_chk_ConPMS").attr("checked");
            if (isPMS) {
                $("#spPMSUrl").show();
            }
            $("#ctl00_ContentPlaceHolder1_chk_ConPMS").click(function () {
                var open = $(this).attr("checked");
                if (open) {
                    $("#spPMSUrl").show();
                } else {
                    $("#spPMSUrl").hide();
                }
            });


            var isOpen = $("#ctl00_ContentPlaceHolder1_chk_OpenDepot").attr("checked");
            if (isOpen) {
                $("#ddlDefaultDepot").show();
            }
            $("#ctl00_ContentPlaceHolder1_chk_OpenDepot").click(function () {
                var open = $(this).attr("checked");
                if (open) {
                    $("#ddlDefaultDepot").show();
                } else {
                    $("#ddlDefaultDepot").hide();
                }
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input type="hidden" id="hidradiobutRegStr" runat="server" />
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="多仓设置" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal2" runat="server" Text="您可以对多仓库进行相关参数设置" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="HiddenField_ID" runat="server" />
        <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
            <tr>
                <td class="tdbg">
                    <table cellspacing="0" cellpadding="3" width="100%" border="0">
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal15" runat="server" Text="OMS系统" />：
                            </td>
                            <td height="25">
                                <asp:CheckBox ID="chk_ConOMS" runat="server" Text="开启" Checked="True" />
                                <span id="ddlOMSUrl" style="display: none">OMS地址：
                                     <asp:TextBox ID="txtOMSUrl" runat="server" Width="240" Height="30">http://</asp:TextBox>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class"></td>
                            <td height="25">
                                <span style="color: red">Tip：表示对接到OMS系统。如未对接OMS系统，请不要勾选</span>
                            </td>
                        </tr>

                        <tr style="display: none">
                            <td class="td_class">
                                <asp:Literal ID="Literal4" runat="server" Text="PMS系统" />：
                            </td>
                            <td height="25">
                                <asp:CheckBox ID="chk_ConPMS" runat="server" Text="开启" Checked="True" />
                                <span id="spPMSUrl" style="display: none">PMS地址：
                                     <asp:TextBox ID="txtPMSUrl" runat="server" Width="240" Height="30">http://</asp:TextBox>
                                </span>
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="td_class"></td>
                            <td height="25">
                                <span style="color: red">Tip：表示对接到PMS系统。如未对接PMS系统，请不要勾选</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal16" runat="server" Text="开启多分仓" />：
                            </td>
                            <td height="25">
                                <asp:CheckBox ID="chk_OpenMultiDepot" runat="server" Text="开启" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class"></td>
                            <td height="25">
                                <span style="color: red">Tip：开启多分仓以后，商品库存会自动切换到分仓库存</span>
                            </td>
                        </tr>

                        <tr class="isOpenDepot" style="display: none">
                            <td class="td_class">
                                <asp:Literal ID="Literal5" runat="server" Text="默认仓库" />：
                            </td>
                            <td height="25">
                                <asp:CheckBox ID="chk_OpenDepot" runat="server" Text="开启" />
                                <span id="ddlDefaultDepot" style="display: none">默认仓库：
                                    <asp:DropDownList ID="ddlDepot" runat="server">
                                    </asp:DropDownList>
                                </span>
                            </td>
                        </tr>

                        <tr class="isOpenDepot" style="display: none">
                            <td class="td_class"></td>
                            <td height="25">
                                <span style="color: red">Tip：默认仓库表示，关联了地区仓库之外地区的订单会自动分配至默认仓库</span>
                            </td>
                        </tr>

                        <tr style="display: none">
                            <td class="td_class">
                                <asp:Literal ID="Literal3" runat="server" Text="分仓商品过滤" />：
                            </td>
                            <td height="25">
                                <asp:CheckBox ID="chk_OpenDepotPro" runat="server" Text="开启" />
                            </td>
                        </tr>
                        <tr class="isOpenDepot" style="display: none">
                            <td class="td_class"></td>
                            <td height="25">
                                <span style="color: red">Tip：开启分仓商品表示前台商品展示会根据分仓商品来展示</span>
                            </td>
                        </tr>


                        <tr style="display: none">
                            <td class="td_class">
                                <asp:Literal ID="Literal6" runat="server" Text="小鸟科技SAASToken" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtToken" runat="server" Width="240" Height="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="td_class"></td>
                            <td height="25">
                               
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class"></td>
                            <td height="25">
                                <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Site, btnSaveText %>"
                                    class="adminsubmit_short" OnClientClick="return confirmRegMode();" OnClick="btnSave_Click"></asp:Button>
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
</asp:Content>
