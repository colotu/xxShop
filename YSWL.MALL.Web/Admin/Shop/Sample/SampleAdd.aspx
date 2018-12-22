<%@ Page Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="SampleAdd.aspx.cs" Inherits=" YSWL.Web.Admin.Shop.Sample.SampleAdd" Title="电子样本管理" %>

<%@ Register Src="~/Controls/UCDroplistPermission.ascx" TagName="UCDroplistPermission"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
       <link href="/admin/css/tab.css" rel="stylesheet" type="text/css" charset="utf-8" />
    <script src="/admin/js/tab.js" type="text/javascript"></script>
    <link rel="stylesheet" href="/admin/js/validate/pagevalidator.css" type="text/css" />
    <script type="text/javascript" src="/admin/js/validate/pagevalidator.js"></script>
    <link href="/admin/css/gridviewstyle.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal2" runat="server" Text="电子样本增加" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal3" runat="server" Text="您可以根据您的需求新增电子样本" />
                    </td>
                </tr>
            </table>
        </div>

           <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
               <tr>
                            <td class="td_class">
                            </td>
                            <td height="25">
                                     <asp:Button ID="goback" runat="server" Text="返回"
                                    class="adminsubmit_short" CausesValidation="False" onclick="goback_Click"></asp:Button>
                                <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Site, btnSaveText %>"
                                    class="adminsubmit_short" OnClick="btnSave_Click"></asp:Button>
                                    
                                      <asp:Button ID="Button1" runat="server" Text="保存及新增"
                                    class="adminsubmit" OnClick="btnSave_Click"></asp:Button>
                                    
                                                        
                                <%--<asp:Button ID="btnCancle" runat="server" Text="<%$ Resources:Site, btnCancleText %>"
                                    class="adminsubmit_short"  CausesValidation="False" OnClick="btnCancle_Click"></asp:Button>--%>
                            </td>
                        </tr>
            </table>
        </div>
        
        <div class="nTab4">
            <div class="TabTitle">
                <ul id="myTab1">
                    <li class="active" onclick="nTabs(this,0);"><a href="javascript:void(0);">基本信息</a></li>
                    <li class="normal" onclick="nTabs(this,1);"><a href="javascript:void(0);">查询优化</a></li>
                </ul>
            </div>
        </div>
        
          <div class="TabContent formitem">
            <div id="myTab1_Content0" tabindex="0">
        <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
            <tr>
                <td class="tdbg">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td class="td_class">
                                样本名称 ：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtTiltle" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="必填信息" ControlToValidate="txtTiltle"></asp:RequiredFieldValidator>
                              </td>
                        </tr>
                        
                        <tr>
                            <td class="td_class" valign="top">
                                是否发布 ：
                            </td>
                            <td height="25">
                            <asp:CheckBox ID="chkStatus" runat="server"  />
                            </td>
                            
                        </tr>
                        
                        
                        
                        <tr>
                            <td class="td_class" valign="top">
                                JPG封面图片 ：
                            </td>
                            <td height="25">
                                <asp:FileUpload ID="uploadJPGCover" runat="server" Width="235px" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="uploadJPGCover" runat="server" ErrorMessage="请选择正确的JPG格式" ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.jpg|.JPG)$"></asp:RegularExpressionValidator>
                            </td>
                            
                        </tr>
                        
                        <tr>
                            <td class="td_class" valign="top">
                                PDF封面图片 ：
                            </td>
                            <td height="25">
                                <asp:FileUpload ID="uploadPDFCover" runat="server" Width="235px" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="uploadPDFCover" runat="server" ErrorMessage="请选择正确的PDF文件"  ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.jpg|.JPG)$"></asp:RegularExpressionValidator>
                            </td>
                            
                        </tr>
                        
                        <tr>
                            <td class="td_class" valign="top">
                                显示顺序 ：
                            </td>
                            <td height="25">
                                  <asp:TextBox ID="txtSequence" runat="server" Width="200px" MaxLength="50" ></asp:TextBox>
                    
                            </td>
                            
                            
                        </tr>
                        
                    </table>
                </td>
            </tr>
        </table>
        </div>
        
        <div id="myTab1_Content1" tabindex="0"  style=" display:none">
           <table style="width: 100%; border-bottom: none; border-top: none; float: left;" cellpadding="2"
                    cellspacing="1" class="border">
                    <tr>
                        <td class="td_class">
                            URL规则 ：
                        </td>
                        <td height="25">
                            <asp:TextBox ID="txtUrlRule" runat="server" Width="372px">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_class">
                            页面标题 ：
                        </td>
                        <td height="25">
                            <asp:TextBox ID="txtMeta_Title" runat="server" Width="372px">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_class">
                            页面描述 ：
                        </td>
                        <td height="25">
                            <asp:TextBox ID="txtMeta_Description" runat="server" Width="372px">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_class">
                            页面关键词 ：
                        </td>
                        <td height="25">
                            <asp:TextBox ID="txtMeta_Keywords" runat="server" Width="372px">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_class">
                            图片Alt信息 ：
                        </td>
                        <td height="25">
                            <asp:TextBox ID="txtSeoImageAlt" runat="server" Width="372px">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_class">
                            图片Title信息 ：
                        </td>
                        <td height="25">
                            <asp:TextBox ID="txtSeoImageTitle" runat="server" Width="372px">
                            </asp:TextBox>
                        </td>
                    </tr>
                    
                    
                </table>
        </div>
        </div>
    </div>
    <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
