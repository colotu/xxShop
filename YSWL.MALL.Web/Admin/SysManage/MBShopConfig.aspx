<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/Basic.Master"
    CodeBehind="MBShopConfig.aspx.cs" Inherits="YSWL.MALL.Web.Admin.SysManage.MBShopConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/admin/js/jquery.uploadify/uploadify-v2.1.0/uploadify.css" rel="stylesheet"
        type="text/css" />
    <script src="../js/jquery.uploadify/uploadify-v2.1.0/swfobject.js" type="text/javascript"></script>
    <script src="/admin/js/jquery.uploadify/uploadify-v2.1.0/jquery.uploadify.v2.1.0.min.js"
        type="text/javascript"></script>
    <script src="/Scripts/jquery/maticsoft.img.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("[id$='txtRechargeRatio']").OnlyFloat();
            if ($("[id$='imgIndexLogo']").attr("src")) {
                $("[id$='imgIndexLogo']").attr("ref", $("[id$='imgIndexLogo']").attr("src"));
                $("[id$='imgIndexLogo']").removeAttr("src");
                $.scaleLoad('#pic_load', 200, 200);
            }
            //            $("[id$='lnkDelete']").hide();
            $("#uploadify").uploadify({
                'uploader': '/admin/js/jquery.uploadify/uploadify-v2.1.0/uploadify.swf',
                'script': '/WebLogo.aspx',
                'cancelImg': '/admin/js/jquery.uploadify/uploadify-v2.1.0/cancel.png',
                'buttonImg': '/admin/images/uploadfile.jpg',
                'folder': 'UploadFile',
                'queueID': 'fileQueue',
                'auto': true,
                'multi': true,
                'width': 76,
                'height': 25,
                'fileExt': '*.jpg;*.gif;*.png;*.bmp',
                'fileDesc': 'Image Files (.JPG, .GIF, .PNG)',
                'queueSizeLimit': 1,
                'sizeLimit': 1024 * 1024 * 10,
                'onInit': function () {
                },
                'onSelect': function (e, queueID, fileObj) {
                },
                'onComplete': function (event, queueId, fileObj, response, data) {
                    if (response.split('|')[0] == "1") {
                        $("[id$='imgIndexLogo']").attr("ref", response.split('|')[1].format(''));
                        $("[id$='imgIndexLogo']").removeAttr("src");
                        $("[id$='imgIndexLogo']").removeAttr('loaded');
                        $.scaleLoad('#pic_load', 200, 200);
                        $("[id$='hfs_ICOPath']").val(response.split('|')[1]);
                    } else {
                        alert("图片上传失败！");
                    }
                }
            });
        });
        $(document).ready(function () {
            $("[id$='txtRechargeRatio']").OnlyFloat();
            if ($("[id$='imgLoginLogo']").attr("src")) {
                $("[id$='imgLoginLogo']").attr("ref", $("[id$='imgLoginLogo']").attr("src"));
                $("[id$='imgLoginLogo']").removeAttr("src");
                $.scaleLoad('#pic_loads', 200, 200);
            }
            //            $("[id$='lnkDelete']").hide();
            $("#uploadifys").uploadify({
                'uploader': '/admin/js/jquery.uploadify/uploadify-v2.1.0/uploadify.swf',
                'script': '/WebLogo.aspx',
                'cancelImg': '/admin/js/jquery.uploadify/uploadify-v2.1.0/cancel.png',
                'buttonImg': '/admin/images/uploadfile.jpg',
                'folder': 'UploadFile',
                'queueID': 'fileQueues',
                'auto': true,
                'multi': true,
                'width': 76,
                'height': 25,
                'fileExt': '*.jpg;*.gif;*.png;*.bmp',
                'fileDesc': 'Image Files (.JPG, .GIF, .PNG)',
                'queueSizeLimit': 1,
                'sizeLimit': 1024 * 1024 * 10,
                'onInit': function () {
                },
                'onSelect': function (e, queueID, fileObj) {
                },
                'onComplete': function (event, queueId, fileObj, response, data) {
                    if (response.split('|')[0] == "1") {
                        $("[id$='imgLoginLogo']").attr("ref", response.split('|')[1].format(''));
                        $("[id$='imgLoginLogo']").removeAttr("src");
                        $("[id$='imgLoginLogo']").removeAttr('loaded');
                        $.scaleLoad('#pic_loads', 200, 200);
                        $("[id$='hfs_LoginPath']").val(response.split('|')[1]);
                    } else {
                        alert("图片上传失败！");
                    }
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
                        <asp:Literal ID="Literal1" runat="server" Text="云订货设置" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal2" runat="server" Text="您可以对云订货进行相关设置" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="HiddenField_ID" runat="server" />
        <table style="width: 100%;" cellpadding="2" cellspacing="1">
            <tr>
                <td class="tdbg">
                    <table cellspacing="0" cellpadding="3" width="100%" border="0">
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal3" runat="server" Text="渠分销名称" />：
                            </td>
                            <td>
                                <asp:TextBox ID="txtMBShopName" runat="server" Width="318"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal5" runat="server" Text="首页Logo" />：
                            </td>
                            <td height="34">
                                <asp:HiddenField ID="hfs_ICOPath" runat="server" />
                                <div id="fileQueue">
                                </div>
                                <input type="file" name="uploadify" id="uploadify" />
                                <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:SysManage,lblWerSitePrompt%>" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                            </td>
                            <td height="25" id="pic_load">
                            <asp:Image ID="imgIndexLogo" runat="server" Width="120" Height="100" /></td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal4" runat="server" Text="登录页Logo" />：
                            </td>
                            <td height="25">
                                <asp:HiddenField ID="hfs_LoginPath" runat="server" />
                                <div id="fileQueues">
                                </div>
                                <input type="file" name="uploadifys" id="uploadifys" />
                                <asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:SysManage,lblWerSitePrompt%>" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                            </td>
                            <td height="25" id="pic_loads">
                            <asp:Image ID="imgLoginLogo" runat="server" Width="145" Height="60" /></td>
                        </tr>
                        <tr style="display: none">
                            <td class="td_class">
                                <asp:Literal ID="Litera21" runat="server" Text="一键拨号" />
                                ：
                            </td>
                            <td>
                                <asp:TextBox ID="txtMShopTel" runat="server" Width="400" Height="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal8" runat="server" Text="是否开启登录" />
                                ：
                            </td>
                            <td>
                                <asp:CheckBox runat="server" ID="OpenLogin"/>&nbsp;是
                                <br />
                                <asp:Literal ID="Literal9" runat="server" Text="客户需要先登录订货端才能继续订货" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal10" runat="server" Text="是否开启注册" />
                                ：
                            </td>
                            <td>
                                <asp:CheckBox runat="server" ID="OpenRegister"/>&nbsp;是
                                <br />
                                <asp:Literal ID="Literal11" runat="server" Text="客户可以通过渠分销APP自主注册账号,需要联系客服开通渠分销独立安装包" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                            </td>
                            <td height="25">
                                <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Site, btnSaveText %>"
                                    class="add-btn mar-t10" OnClientClick="confirmRegMode();" OnClick="btnSave_Click">
                                </asp:Button>
                              
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
    </div>
</asp:Content>
