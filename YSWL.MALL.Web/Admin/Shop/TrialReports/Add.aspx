<%@ Page Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.TrialReports.Add" Title="增加页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Admin/js/jquery.uploadify/uploadify-v2.1.0/uploadify.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/jquery.uploadify/uploadify-v2.1.0/swfobject.js" type="text/javascript"></script>
    <script src="/Admin/js/jquery.uploadify/uploadify-v2.1.0/jquery.uploadify.v2.1.0.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery/maticsoft.jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            $("#uploadify").uploadify({
                'uploader': '/admin/js/jquery.uploadify/uploadify-v2.1.0/uploadify.swf',
                'script': '/UploadNormalImg.aspx',
                'cancelImg': '/admin/js/jquery.uploadify/uploadify-v2.1.0/cancel.png',
                'buttonImg': '/admin/images/uploadfile.jpg',
                'folder': 'UploadFile',
                'queueID': 'fileQueue',
                'width': 76,
                'height': 25,
                'auto': true,
                'multi': true,
                'fileExt': '*.jpg;*.gif;*.png;*.bmp',
                'fileDesc': 'All Files',
                'queueSizeLimit': 1,
                'sizeLimit': 1024 * 1024 * 1024 * 1024 * 1024,
                'onInit': function () {
                },

                'onSelect': function (e, queueID, fileObj) {
                },
                'onComplete': function (event, queueId, fileObj, response, data) {

                    if (response.split('|')[0] == "1") {
                        $("[id$='hfImageUrl']").val(response.split('|')[1]);
                        $("#imgTrial").attr("src", response.split('|')[1].format(''));
                        $("#imgShow").show();
                    } else {
                        alert("图片上传失败！");
                    }
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal2" runat="server" Text="增加试用报告" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        您可以<asp:Literal ID="Literal3" runat="server" Text="增加试用报告" />
                    </td>
                </tr>
            </table>
        </div>
        <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
            <tr>
                <td class="tdbg">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td height="25" class="td_class">
                                名称 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:TextBox ID="txtTitle" runat="server" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" class="td_class">
                                链接 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:TextBox ID="txtLinkUrl" runat="server" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" class="td_class">
                                介绍 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:TextBox ID="txtShortDescription" runat="server" Width="500px" Height="200px" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" class="td_class">
                                发表者 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:TextBox ID="txtCreatedUserName" runat="server" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" class="td_class">
                                图片 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:HiddenField runat="server" ID="hfImageUrl" />
                                <div id="fileQueue">
                                </div>
                                <input type="file" name="uploadify" id="uploadify" /><br />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                            </td>
                            <td height="25">
                                <label class="msgNormal" style="width: 200px">
                                    <asp:Literal ID="Literal32" runat="server" Text="请选择有效的图片文件，建议将图片文件的大小限制在200KB以内。" /></label>
                            </td>
                        </tr>
                        <tr id="imgShow">
                            <td class="td_class">
                            </td>
                            <td height="25">
                                <img id="imgTrial" src="" />
                            </td>
                        </tr>
                        <tr>
                            <td height="25">
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                            </td>
                            <td height="25">
                                <asp:Button ID="btnCancle" runat="server" Text="<%$ Resources:Site, btnCancleText %>" class="adminsubmit_short" OnClick="btnCancle_Click"></asp:Button>
                                <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Site, btnSaveText %>" class="adminsubmit_short" OnClick="btnSave_Click"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
