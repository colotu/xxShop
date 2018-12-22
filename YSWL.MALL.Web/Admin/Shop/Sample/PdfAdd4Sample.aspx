<%@ Page Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="PdfAdd4Sample.aspx.cs" Inherits=" YSWL.Web.Admin.Shop.Sample.PdfAdd4Sample" Title="新增电子样本图片" %>

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
                        <asp:Literal ID="Literal2" runat="server" Text="新增电子样本图片" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal3" runat="server" Text="您可以根据您的需求新增不同的图片" />
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
                            <td class="td_class" valign="top">
                                电子样本 ：
                            </td>
                            <td height="25">
                                <asp:DropDownList ID="ddrSampleList" runat="server">
                                </asp:DropDownList>
                            </td>
                            
                        </tr>
                        

                        <tr>
                            <td class="td_class">
                                页面信息 ：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtTitle" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="必填信息" ControlToValidate="txtTitle"></asp:RequiredFieldValidator>
                              </td>
                        </tr>
                      
                        
                          
                        <tr >
                            <td class="td_class" valign="top">
                                选择类型 ：
                            </td>
                            <td height="25">
                                <asp:RadioButtonList ID="rdoType" runat="server" RepeatDirection="Horizontal" 
                                    AutoPostBack="True" onselectedindexchanged="rdoType_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Selected="True">本地上传</asp:ListItem>
                                    <asp:ListItem Value="1">远程地址</asp:ListItem>
                            </asp:RadioButtonList>

                                      </td>
                            
                        </tr>
                        
                        
                              
                        <tr id="trRemote" runat="server">
                            <td class="td_class" valign="top">
                                远程文件 ：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtRemoteUrl" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtRemoteUrl" runat="server" ErrorMessage="请填写地址"></asp:RequiredFieldValidator>
                      
                              </td>
                            
                        </tr>
                        

                        
                        <tr  id="trUploadPdf" runat="server">
                            <td class="td_class" valign="top">
                                上传PDF ：
                            </td>
                            <td height="25">
                                <asp:FileUpload ID="uploadPDF" runat="server" Width="235px" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="uploadPDF" runat="server" ErrorMessage="请选择正确的PDF文件" ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$"></asp:RegularExpressionValidator>
                            </td>
                            
                        </tr>
                        
                        
                        
                        
                        
                    </table>
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
