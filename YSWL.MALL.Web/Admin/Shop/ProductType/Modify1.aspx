<%@ Page Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="Modify1.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.ProductType.Modify1"
    Title="商品类型管理" %>
    
<%@ Register TagPrefix="YSWL" Namespace="YSWL.Web.Validator" Assembly="YSWL.Web.Validator" %>
<%@ Register Src="~/Controls/UCDroplistPermission.ascx" TagName="UCDroplistPermission"
    TagPrefix="uc2" %>
<%@ Register Assembly="YSWL.Controls" Namespace="YSWL.Controls" TagPrefix="YSWL" %>
<%@ Register TagPrefix="YSWL" Namespace="YSWL.MALL.Web.Controls" Assembly="YSWL.MALL.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/admin/js/validate/pagevalidator.css" type="text/css" />
    <script type="text/javascript" src="/admin/js/validate/pagevalidator.js"></script>
    <script src="/admin/js/tab.js" type="text/javascript"></script>
    <link href="/admin/css/tab.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            var Tid = $.getUrlParam("tid");
            $("#m1").attr("href", "Modify1.aspx?tid=" + Tid);
            $("#m2").attr("href", "Modify2.aspx?tid=" + Tid);
            $("#m3").attr("href", "Modify3.aspx?tid=" + Tid);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal2" runat="server" Text="编辑商品类型" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal3" runat="server" Text="商品类型是一系属性的组合，可以用来向顾客展示某些商品具有的特有的属性，一个商品类型下可新增多种属性.一种是供客户查看的扩展属性,如图书类型商品的作者，出版社等，一种是供客户可选的规格,如服装类型商品的颜色、尺码。 " />
                    </td>
                </tr>
            </table>
        </div>        
        <div class="nTab4">
            <div class="TabTitle">
                <ul id="myTab1">
                    <li class="active" ><a id="m1">基本设置</a></li>
                    <li class="normal" ><a id="m2">扩展属性</a></li>
                    <li class="normal"><a  id="m3">规格</a></li>
                </ul>
            </div>
     
        <div class="TabContent">
            <div id="myTab1_Content0">
        <table style="width: 100%;border-top:none;" cellpadding="2" cellspacing="1" class="border">
            <tr>
                <td class="tdbg">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td class="td_class">
                                商品类型名称 ：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtTypeName" runat="server" Width="372px"></asp:TextBox>
                            </td>
                        </tr>
                                <tr>
                                    <td class="td_class">
                                    </td>
                                    <td height="25">
                                        <div id="txtTypeNameTip" runat="server">
                                        </div>
                                        <YSWL:ValidateTarget ID="ValidateTargetName" runat="server"  OkMessage="输入正确！" Description="商品类型名称不能为空，长度限制在1-50个字符之间！"
                                            ControlToValidate="txtTypeName" ContainerId="ValidatorContainer">
                                            <Validators>
                                                <YSWL:InputStringClientValidator ErrorMessage="商品类型名称不能为空，长度限制在1-50个字符之间！" LowerBound="1"
                                                    UpperBound="50" />
                                            </Validators>
                                        </YSWL:ValidateTarget>
                                    </td>
                                </tr>
                                
                        <tr>
                            <td  style="height:8px;">
                            </td>
                            <td height="6">
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class" style="vertical-align:top;">
                                关联商品品牌 ：
                            </td>
                            <td height="25">
                                <YSWL:BrandsCheckBoxList ID="chkBrandsCheckBox" runat="server">
                                </YSWL:BrandsCheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                            </td>
                            <td height="25">
                                <label class="msgNormal" style="width: 200px">
                                    <asp:Literal ID="Literal1" runat="server" Text="选择与些商品类型关联的商品品牌" /></label>
                            </td>
                        </tr>
                        <tr>
                            <td  style="height:8px;">
                            </td>
                            <td height="6">
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                备注 ：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtRemark" runat="server" Width="372px" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                            </td>
                            <td height="25">
                                <asp:Button ID="btnCancle" runat="server" Text="<%$ Resources:Site, btnCancleText %>"
                                    class="adminsubmit_short" OnClick="btnCancle_Click" title="返回列表页"></asp:Button>
                                <asp:Button ID="btnSave" runat="server" Text="保存" title="保存基本设置"
                                    class="adminsubmit_short"  OnClientClick="return PageIsValid();" OnClick="btnSave_Click"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        </div>
    </div>
               </div>
    <YSWL:ValidatorContainer runat="server" ID="ValidatorContainer" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
