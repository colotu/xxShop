<%@ Page Language="C#" MasterPageFile="~/admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="SubmitPoll.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Forms.SubmitPoll"
    Title="<%$Resources:Poll,ptOptionsIndex %>" %>

<%@ MasterType VirtualPath="~/admin/Basic.Master" %>
<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .color_red
        {
            color: Red;
        }
    </style>
    <script type="text/javascript">
        $(function () {

            $(".iframe").colorbox({ iframe: true, width: "800", height: "650", overlayClose: false });

            /*****问卷调查开始****/
            $('[id$="_surveytopic0"]').die().live('click', function () { //单选按钮的单击事件
                $(this).parent().parent().find('[name="hidtopics0"]').val($(this).val()); //把当前选中的按钮的值保存到当前按钮的父节点的父节点中名字为hidtopics0的元素的value中 
            });

            $('[id$="_surveytopic1"]').die().live('click', function () { //多选按钮的单击事件  
                var values = ""; //得到与当前按钮同一组的被选中的checkbox的值
                $("input[name='" + $(this).attr('name') + "']:checked").each(function (i, d) {
                    values += d.value + ",";
                });
                $(this).parent().parent().find('[name="hidtopics1"]').val(values.substring(0, values.length - 1)); //把当前选中的按钮的值保存到当前按钮的父节点的父节点中名字为hidtopics0的元素的value中 
            });
            /*****问卷调查结束****/
        });
        //提交投票
        var submitOptions = function () {
            var uid = $.getUrlParam('uid');
            if (uid == null || parseInt(uid) <= 0) {
                ShowFailTip("请先选择用户");
                return false;
            }
            var json = []; //声明json
            if ($('.topicsoptions').length <= 0) {
                ShowFailTip("目前没有题目，请不要投票");     //$('.topicsoptions').eq(i).attr('qnumber')
                return false;
            }
            for (var i = 0; i < $('.topicsoptions').length; i++) {
                var toticsoptionsval = $('.topicsoptions').eq(i).val();
                if (toticsoptionsval == "" || toticsoptionsval == "0") {
                    ShowFailTip("请填写第" + (i + 1) + "题");     //$('.topicsoptions').eq(i).attr('qnumber')
                    return false;
                }
                json.push({ "topicid": $('.topicsoptions').eq(i).attr('topicsid'), "topicvlaue": $('.topicsoptions').eq(i).val(), "type": $('.topicsoptions').eq(i).attr('topicstype') });
            }
            $.ajax({
                url: ("SubmitPoll.aspx?timestamp={0}").format(new Date().getTime()),
                type: 'POST',
                dataType: 'text',
                timeout: 10000,
                async: false,
                data: { Action: "SubmitPolls",
                    Callback: "true", TopicIDjson: JSON.stringify(json), uid: uid
                },
                success: function (resultData) {
                    switch (resultData) {
                        case "true":
                            ShowSuccessTip("投票成功！");
                            if ($('#IsContinuous:checked').length >= 1) {
                                //清空选择的内容
                                $('[id$="ltlCurrentUser"]').text('');
                                $('.topicsoptions').val('');
                                $('[id$="_surveytopic0"]:checked').attr('checked', false);
                                $('[id$="_surveytopic1"]:checked').attr('checked', false);
                            } else {
                                setTimeout(function () {
                                    window.location.href = "/admin/Poll/Options/showcount.aspx?fid=" + $.getUrlParam('fid');
                                }, 1000);
                            }
                            break;
                        case "false":
                            ShowServerBusyTip("投票失败");
                            break;
                        case "isnotnull":
                            ShowFailTip("不能重复投票！");
                            break;
                        case "NotSelectUser":
                            ShowFailTip("请选择用户！");
                            break;
                        case "Repeat":
                            ShowFailTip("该用户已提交过问卷，请重新选择用户！");
                            break;
                        case "NotSelectUser":
                            ShowFailTip("请选择用户！");
                            break;
                        default:
                            ShowServerBusyTip("投票失败");
                            break;
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrown) {
                    ShowServerBusyTip("服务器没有返回数据，可能服务器忙，请稍候再试！");
                }
            });
        };
 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="问卷调查" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal2" runat="server" Text="问卷调查" />
                    </td>
                </tr>
            </table>
        </div>
        <div align="center">
            <table cellspacing="0" cellpadding="5" width="100%" border="0" class="border">
                <tr>
                    <td>
                        <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:Poll,lblQuestionnaire %>" />
                        【<asp:Label ID="lblFormName" runat="server" Font-Bold="true" Text=""></asp:Label>】
                        <asp:Label ID="lblFormID" runat="server" Visible="false" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="padding: 20px;">
                        选择用户： <a id="linkSelectUser" href="/admin/Poll/Forms/UserList.aspx?fid=<%=Fid %>"
                            style="color: blue;" class=" iframe">选择 </a>
                        <asp:Literal ID="ltlCurrentUser" runat="server" Text="" />
                        <span class="color_red">
                            <asp:Literal ID="ltlNotSelectUser" runat="server" Text="" /></span>
                    </td>
                </tr>
                <%= GetStrList() %>
                <tr>
                    <td style="padding: 20px;">
                        <input type="checkbox" id="IsContinuous">
                        <label for="IsContinuous">
                            是否连续提交</label>
                        <input type="button" onclick="submitOptions();" value="提交" class="adminsubmit_short" />
                    </td>
                </tr>
            </table>
        </div>
        <style>
            td.tdbg
            {
                padding-left: 10px;
            }
        </style>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
