<%@ Page Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true" CodeBehind="Modify.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.Trials.Modify" Title="修改页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script type="text/javascript">
            window.UEDITOR_HOME_URL = "/ueditor/";
    </script>
    <script src="/ueditor/ueditor.config.js" type="text/javascript"></script>
    <script src="/ueditor/ueditor.all.min.js" type="text/javascript"></script>
    <link href="/ueditor/themes/default/ueditor.css" rel="stylesheet" type="text/css" />
    <link href="/Admin/js/jquery.uploadify/uploadify-v2.1.0/uploadify.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/jquery.uploadify/uploadify-v2.1.0/swfobject.js" type="text/javascript"></script>
    <script src="/Admin/js/jquery.uploadify/uploadify-v2.1.0/jquery.uploadify.v2.1.0.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery/maticsoft.jquery.min.js" type="text/javascript"></script>
    <link href="/Scripts/jqueryui/jquery-ui-1.8.19.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jqueryui/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <script src="/admin/js/jqueryui/JqueryDataPicker4CN.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            $.datepicker.setDefaults($.datepicker.regional['zh-CN']);
            $("[id$='txtStartDate']").prop("readonly", true).datepicker({
                yearRange: ("1949:" + new Date().getFullYear()),
                dateFormat: "yy-mm-dd",
                onClose: function (selectedDate) {
                    $("[id$='txtEndDate']").datepicker("option", "minDate", selectedDate);
                }
            });
            $("[id$='txtEndDate']").prop("readonly", true).datepicker({
                yearRange: ("1949:" + new Date().getFullYear()),
                dateFormat: "yy-mm-dd",
                onClose: function (selectedDate) {
                    $("[id$='txtStartDate']").datepicker("option", "maxDate", selectedDate);
                }
            });

            if ($("[id$='hfImageUrl']").val()) {
                $("[id$='imgTrial']").attr("src", $("[id$='hfImageUrl']").val());
            }

            $("#uploadify").uploadify({
                'uploader': '/admin/js/jquery.uploadify/uploadify-v2.1.0/uploadify.swf',
                'script': '/UploadNormalImg.aspx',
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
                        $("[id$='hfImageUrl']").val(response.split('|')[1]);
                        $("[id$='imgTrial']").attr("src", response.split('|')[1].format(''));
                        $("#imgShow").show();
                        $("[id$='hfIsModifyImage']").val("True");
                    } else {
                        alert("图片上传失败！");
                    }
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:HiddenField ID="hfIsModifyImage" runat="server" />
        <asp:HiddenField ID="hfOldImage" runat="server" />
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal2" runat="server" Text="Shop_Trials编辑" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        您可以编辑<asp:Literal ID="Literal3" runat="server" Text="Shop_Trials" />
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
                                <asp:TextBox ID="txtTrialName" runat="server" Width="200px"></asp:TextBox>
                                <asp:Label runat="server" ID="lblTrialId" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" class="td_class">
                                开始时间 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:TextBox ID="txtStartDate" runat="server" Width="80"></asp:TextBox>
                                -
                                <asp:TextBox ID="txtEndDate" runat="server" Width="80"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" class="td_class">
                                概要 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:TextBox ID="txtShortDescription" runat="server" Width="500px" Height="200px" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" class="td_class">
                                描述 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:TextBox ID="txtDescription" runat="server" Width="80%" TextMode="MultiLine" Text=""></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" class="td_class">
                                试用链接 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:TextBox ID="txtLinklUrl" runat="server" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" class="td_class">
                                状态：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:DropDownList runat="server" ID="dropTrialStatus">
                                    <asp:ListItem Value="0" Text="即将进行" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="进行中"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="已结束"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" class="td_class">
                                试用总数 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:TextBox ID="txtTrialCounts" runat="server" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td height="25" class="td_class">
                                显示顺序 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:TextBox ID="txtDisplaySequence" runat="server" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" class="td_class">
                                市场价 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:TextBox ID="txtMarketPrice" runat="server" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" class="td_class">
                                图片路径 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:HiddenField runat="server" ID="hfImageUrl"/>
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
                                        <img  id="imgTrial" src="" />
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
    <script type="text/javascript">
        var editor = new baidu.editor.ui.Editor({//实例化编辑器
            iframeCssUrl: 'ueditor/themes/default/iframe.css', toolbars: [

            ['fullscreen', 'source', '|', 'undo', 'redo', '|',
                'bold', 'italic', 'underline', 'strikethrough', '|', 'forecolor', 'backcolor', '|',
                             'superscript', 'subscript', '|', 'justifyleft', 'justifycenter', 'justifyright', 'justifyjustify', 'insertorderedlist', 'insertunorderedlist', '|', 'indent', '|', 'removeformat', 'formatmatch', 'autotypeset', '|', 'pasteplain', '|', 'customstyle',
                'paragraph', '|', 'rowspacingtop', 'rowspacingbottom', 'lineheight', '|', 'fontfamily', 'fontsize', '|', 'imagenone', 'imageleft', 'imageright',
                'imagecenter', '|', 'insertimage', 'insertvideo', 'map', 'horizontal', '|',
                'link', 'unlink', '|', 'inserttable', 'deletetable', 'insertparagraphbeforetable', 'insertrow', 'deleterow', 'insertcol', 'deletecol', 'mergecells', 'mergeright', 'mergedown', 'splittocells', 'splittorows', 'splittocols']
                 ],
            initialContent: '', autoHeightEnabled: false,
            initialFrameHeight: 220,
            pasteplain: false
         , wordCount: false
          , elementPathEnabled: false
 , autoClearinitialContent: false, imagePath: "/Upload/RTF/", imageManagerPath: "/"
        });
        editor.render('<%=txtDescription.ClientID %>'); //将编译器渲染到容器
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
