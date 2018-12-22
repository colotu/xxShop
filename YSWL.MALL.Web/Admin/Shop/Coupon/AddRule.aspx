<%@ Page Language="C#" MasterPageFile="~/Admin/BasicNoFoot.Master" AutoEventWireup="true"
    CodeBehind="AddRule.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.Coupon.AddRule" %>

<%@ Register TagPrefix="YSWL" TagName="CategoriesDropList" Src="~/Controls/CategoriesDropList.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery/maticsoft.jquery.min.js" type="text/javascript"></script>
    <link href="/Scripts/jqueryui/jquery-ui-1.8.19.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jqueryui/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <script src="/admin/js/jqueryui/JqueryDataPicker4CN.js" type="text/javascript"></script>
    <link href="/Admin/js/select2-2.1/select2.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/select2-2.1/select2.min.js" type="text/javascript"></script>
    <style type="text/css">
        #txtCategory div
        {
            display: initial;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            var cate = $("#ctl00_ContentPlaceHolder1_hfCategory").val();
            var sup = $("#ctl00_ContentPlaceHolder1_hfSupplier").val();
            if (cate.toLowerCase() == "true") {
                $(".txtCategory").show();
            }
            if (sup.toLowerCase() == "true") {
                $("#txtSupplier").show();
            }
            $.datepicker.setDefaults($.datepicker.regional['zh-CN']);
            $("#ctl00_ContentPlaceHolder1_txtStartDate").prop("readonly", true).datepicker({

                changeMonth: true,
                dateFormat: "yy-mm-dd",
                onClose: function (selectedDate) {
                    $("#ctl00_ContentPlaceHolder1_txtEndDate").datepicker("option", "minDate", selectedDate);
                }
            });
            $("#ctl00_ContentPlaceHolder1_txtEndDate").prop("readonly", true).datepicker({

                changeMonth: true,
                dateFormat: "yy-mm-dd",
                onClose: function (selectedDate) {
                    $("#ctl00_ContentPlaceHolder1_txtStartDate").datepicker("option", "maxDate", selectedDate);
                    $("#ctl00_ContentPlaceHolder1_txtEndDate").val($(this).val());
                }
            });
            $("[name$='rblType']").click(function () {
                var value = parseInt($(this).val());
                $(".claType").show();
                $(".claType3").hide();
                if (value == 1) {
                    $(".SendCount").hide();
                    $(".Point").show();
                }
                if (value == 0) {
                    $(".SendCount").show();
                    $(".Point").hide();
                }
                if (value == 2) {
                    $(".SendCount").hide();
                    $(".Point").hide();
                    $(".claType").hide();
                    $(".claType3").show();
                }
            });

            $("[id$='ddlProductId']").select2({ placeholder: "请选择" });

            $('#td_cate select').die('change').live('change', function () {
                GetProductList(false);
            });

            $('[id$="ddlProductId"]').change(function () {
                $('[id$="hidproductId"]').val($(this).val());
            });

            //回发
            if (IsPostBack()) {
                GetProductList(true); //回发后值下拉框中的值保持不变
            }
        })

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
                $('[id$="ddlProductId"]').empty().append('<option></option>');
                $("[id$='ddlProductId']").select2({ placeholder: "请选择" });
                return;
            }
            $.ajax({
                url: ("AddRule.aspx?timestamp={0}").format(new Date().getTime()),
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
                    $('[id$="ddlProductId"]').empty();
                    $('[id$="ddlProductId"]').append(strValue);
                    if (isback) {
                        $('[id$="ddlProductId"]').val($('[id$="hidproductId"]').val()).select2();
                    } else {
                        $("[id$='ddlProductId']").select2({ placeholder: "请选择" });
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
                        <asp:Literal ID="Literal1" runat="server" Text="优惠券活动管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal6" runat="server" Text="您可以进行设置优惠券条件操作" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hfCategory" runat="server" />
        <asp:HiddenField ID="hfSupplier" runat="server" />
        <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
            <tr>
                <td class="tdbg">
                    <table cellspacing="0" cellpadding="3" width="100%" border="0">
                        <tr class="txtCategory" style="display: none">
                            <td class="td_class">
                                <asp:Literal ID="Literal3" runat="server" Text="指定分类" />：
                            </td>
                            <td height="25" id="td_cate">
                                <YSWL:CategoriesDropList ID="ddlCateList" runat="server" IsNull="true" />
                            </td>
                        </tr>
                        <tr class="txtCategory" style="display: none">
                            <td class="td_class">
                                指定商品 ：
                            </td>
                            <td height="25">
                                <asp:HiddenField ID="hidproductId" runat="server" />
                                <select id="ddlProductId" style="width: 500px;">
                                    <option></option>
                                </select>
                                <span style="display: inline-block;">(提示：先指定商品分类 1)如果不指定商品，则以分类为准,2)如果不指定分类，则无限制</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal7" runat="server" Text="优惠券分类" />：
                            </td>
                            <td height="25">
                                <asp:DropDownList ID="ddlClass" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="txtSupplier" style="display: none">
                            <td class="td_class">
                                <asp:Literal ID="Literal9" runat="server" Text="商家" />：
                            </td>
                            <td height="25">
                                <asp:DropDownList ID="ddlSupplier" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal5" runat="server" Text="名称" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtName" runat="server" Width="250px" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal10" runat="server" Text="优惠券前缀" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtPreName" runat="server" Width="250px" MaxLength="30"></asp:TextBox>
                                <span style="color: red; padding-left: 8px">最好为大写字母</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal11" runat="server" Text="优惠券卡号长度" />：
                            </td>
                            <td height="25">
                                <asp:DropDownList ID="ddlLength" runat="server" Width="80">
                                    <asp:ListItem Value="10" Text="10位" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="11" Text="11位"></asp:ListItem>
                                    <asp:ListItem Value="12" Text="12位"></asp:ListItem>
                                    <asp:ListItem Value="13" Text="13位"></asp:ListItem>
                                </asp:DropDownList>
                                <span style="color: red; padding-left: 8px">（不包括优惠券前缀）</span>
                            </td>
                        </tr>
               
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal15" runat="server" Text="最低消费金额" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtLimitPrice" runat="server" Width="80px" MaxLength="30"></asp:TextBox>
                                &nbsp;&nbsp;优惠券面值：<asp:TextBox ID="txtPrice" runat="server" Width="80px" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal4" runat="server" Text="优惠券类型：" />
                            </td>
                            <td height="25">
                                <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" align="left">
                                    <asp:ListItem Value="0" Text="普通优惠券" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="兑换优惠券"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="赠送优惠券" ></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                         <tr class="claType">
                            <td class="td_class">
                                <asp:Literal ID="Literal14" runat="server" Text="有效时间" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtStartDate" runat="server" Width="100" MaxLength="30"></asp:TextBox> -- 
                                <asp:TextBox ID="txtEndDate" runat="server" Width="100" MaxLength="30"></asp:TextBox>
                                <asp:CheckBox ID="chkNoDate" runat="server" />无限期
                            </td>
                        </tr>
                           <tr class="claType3" style="display: none">
                            <td class="td_class">
                                <asp:Literal ID="Literal8" runat="server" Text="顺延时间" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtDeferDay" runat="server" Width="100" MaxLength="30"></asp:TextBox>天
                                
                                <asp:CheckBox ID="chkAvaType" runat="server" />次月生效
                            </td>
                        </tr>

                        <tr class="SendCount">
                            <td class="td_class">
                                <asp:Literal ID="Literal13" runat="server" Text="生成数量" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtSendCount" runat="server" Width="80px" MaxLength="30"></asp:TextBox>
                                <span style="color: red; padding-left: 8px">必须为数字</span>
                            </td>
                        </tr>
                        <tr class="Point" style="display: none">
                            <td class="td_class">
                                <asp:Literal ID="Literal17" runat="server" Text="兑换所需积分" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtPoint" runat="server" Width="80px" MaxLength="30"></asp:TextBox>
                                <span style="color: red; padding-left: 8px">必须为数字</span>
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="td_class">
                                <asp:Literal ID="Literal12" runat="server" Text="优惠券密码" />：
                            </td>
                            <td height="25">
                                <asp:DropDownList ID="ddlPwd" runat="server" Width="80">
                                    <asp:ListItem Value="6" Text="6位" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="7" Text="7位"></asp:ListItem>
                                    <asp:ListItem Value="8" Text="8位"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal2" runat="server" Text="" />
                            </td>
                            <td height="25">
                                <asp:CheckBox ID="chkIsPwd" Text="需要密码" runat="server" Visible="False" />
                                <asp:CheckBox ID="chkIsReuse" Text="可重复" runat="server" Visible="False" />
                                <asp:CheckBox ID="chkStatus" Text="启用" runat="server" Checked="True" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                            </td>
                            <td height="25">
                                <asp:Button ID="btnSave" runat="server" Text="生成" OnClick="btnSave_Click" class="adminsubmit_short">
                                </asp:Button>&nbsp;&nbsp;
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
