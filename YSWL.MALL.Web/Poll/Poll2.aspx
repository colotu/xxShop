<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Poll2.aspx.cs" Inherits="YSWL.MALL.Web.Poll2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>参与海通问卷调查，赢取诺基亚全触屏智能手机大奖！</title>
    <link type="text/css" rel="stylesheet" href="../css/poll.css" />
   <script type="text/javascript" src="http://jqueryjs.googlecode.com/files/jquery-1.3.2.js"></script>

    <script type="text/javascript" src="http://dev.iceburg.net/jquery/jqModal/jqModal.js"></script>

    <script type="text/javascript">
       $().ready(function() {
          $('#dialog_date').jqm({trigger: 'a.dialog_date'});
          $('#dialog_rule').jqm({trigger: 'a.dialog_rule'});
          $('#dialog_award').jqm({trigger: 'a.dialog_award'});
          $('#dialog_draw').jqm({trigger: 'a.dialog_draw'});
          $('#dialog_office').jqm({trigger: 'a.dialog_office'});
          $('#dialog_mail').jqm({trigger: 'a.dialog_mail'});
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div class="layout">
        <div class="head">
            <a href="http://www.htsec.com" class="haitong" target="_blank" >&nbsp;</a>
            <a href="http://www.gupk.com" class="gupk" target="_blank">&nbsp;</a>
            </div>
        <div class="intro">
            <div class="introdetail">
                <p>
                    系统时间：<span class="red"><%=nowtime%></span></p>
                <p>
                    活动介绍： 为更好了解中国股民的网络使用情况，为股民朋友们提供更多更好的服务，海通证券和北京金网融通网络科技有限公司联合主办“参加海通问卷调查 赢取惊喜数码大奖”调查活动，参与本次活动的朋友将有机会获得丰厚的数码大奖！<br />
                </p>
                <div class="tip">
                    一等奖1名：奖诺基亚全触屏智能手机5800XM一部<br />
                    二等奖3名：奖苹果iPod shuffle3(4GB)一部<br />
                    三等奖5名：奖SanDisk Micro U3 8G U盘一部
                </div>
            </div>
            <div class="introctr">
                <div class="intbox">
                    <ul>
                        <li class="cs">活动客服热线：400-6509-777</li>
                        <li class="date"><a href="javascript:;" class="dialog_date">活动日期</a></li>
                        <li class="rule"><a href="javascript:;" class="dialog_rule">活动规则</a></li>
                        <li class="draw"><a href="javascript:;" class="dialog_draw">抽奖方式</a></li>
                        <li class="award"><a href="javascript:;" class="dialog_award">领奖方式</a></li>
                        <li class="office"><a href="http://www.gupk.com" target="_blank">活动官网</a></li>
                        <li class="mail"><a href="mailto:cs@gupk.com" >发送邮件</a></li>
                    </ul>
                    <br class="clear" />
                </div>

            </div>
        </div>
        <div class="per">
            <h2>
                调查问卷</h2>
            <div class="conbox">
            
            <asp:repeater id="rptVote" runat="server" onitemdatabound="rptVote_ItemDataBound"  
                                            >
											<ItemTemplate>
												<tr>
													<th>
													<b><%# DataBinder.Eval(Container, "DataItem.Title")%></b>
													</th>
												</tr>
												<tr>
													<td align="left" bgcolor="White">
														<asp:Label id="labID" Visible=False runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ID") %>'>
														</asp:Label>
														<asp:Label id="labType" Visible=False runat="server" text='<%# DataBinder.Eval(Container, "DataItem.Type") %>'>
														</asp:Label>
														<asp:CheckBoxList DataTextField="Name" DataValueField="id" id="cbChoice" runat="server" RepeatDirection=Vertical Visible="False"></asp:CheckBoxList>
														<asp:RadioButtonList DataTextField="Name" DataValueField="id" id="rbChoice" runat="server" RepeatDirection=Vertical  Visible="False"></asp:RadioButtonList>
														<asp:TextBox id="txtContent" runat="server" Visible="False" TextMode="MultiLine" Width="200"
															Height="50px"></asp:TextBox>
													</td>
												</tr>
											</ItemTemplate>
										</asp:repeater>
            
               
            </div>
        </div>
        <div class="submit">
            <asp:Button ID="btnOk" class="btn" runat="server" Text="提交问卷" 
                onclick="btnOk_Click1" />
        </div>
        <div class="foot">
            <p>
                活动客服热线：400-6509-777（客服时间：周一至周五上午9:30至下午17：30）<br />
                客服邮箱：cs@gupk.com
                <br />
                活动官方网站：www.gupk.com
            </p>
        </div>
    </div>
    
                <div class="jqmWindow" id="dialog_rule">
                <a href="javascript:;" class="jqmClose red">X</a><br class="clear" />
                <hr>
                活动期间，参与“股民网络行为调查”就有机会获得股战场为您提供的丰厚奖励。<br />
                活动举办时间：2009年5月25日开始，2009年6月5日结束<br />
                活动内容介绍：在调查活动结束后，会从参与“股民网络行为调查”活动的海通用户中进行抽取获奖用户，奖励相应奖品。<br />
                抽奖规则：在每位用户填写完成调查问卷并提交后，会随机提供给用户一行序列号，这个序列号作为兑奖的标识。抽奖时按序列号为准。<br />
                活动联合主办方：海通证券、北京金网融通网路科技有限公司<br />
                活动客服热线：400-6509-777（客服时间：周一至周五上午9:30至下午17：30）<br />
                活动官方网站：www.gupk.com<br />
                <br />
                奖品设置：<br />
                    一等奖1名：奖诺基亚全触屏智能手机5800XM一部<br />
                    二等奖3名：奖苹果iPod shuffle3(4GB)一部<br />
                    三等奖5名：奖SanDisk Micro U3 8G U盘一部
                <br />

                抽奖时间：<br />
                活动结束后一周内<br />
                <br />
                抽奖方式：<br />
                选择海通的一家营业部大厅进行现场抽取。<br />
                <br />
                领奖方式及凭证：<br />
				获奖者于抽奖当日即时产生,同时会在主办方网站进行公布。<br />
				中奖名单公布3个工作日内，主办方与获奖者取得联系，核实身份并协商领奖方式。<br />
				中奖用户本人不能领取奖品的，需委托他人代为领取的，受托人须持中奖用户本人的有效证明和以及受委托本人的有效证明、并且持有获奖序列号，经主办方工作人员核实情况属实后，即可领取奖品；<br />
				获奖奖品的保修及售后服务依照生产商的保修承诺实施，活动主办方不负责保修。<br />
            </div>
            <div class="jqmWindow" id="dialog_date">
                <a href="javascript:;" class="jqmClose red">X</a><br class="clear" />
                <hr>
                活动举办时间：2009年5月25日开始，2009年6月5日结束<br />
            </div>
            <div class="jqmWindow" id="dialog_award">
                <a href="javascript:;" class="jqmClose red">X</a><br class="clear" />
                <hr>
                领奖方式及凭证：<br />
				获奖者于抽奖当日即时产生,同时会在主办方网站进行公布。<br />
				中奖名单公布3个工作日内，主办方与获奖者取得联系，核实身份并协商领奖方式。<br />
				中奖用户本人不能领取奖品的，需委托他人代为领取的，受托人须持中奖用户本人的有效证明和以及受委托本人的有效证明、并且持有获奖序列号，经主办方工作人员核实情况属实后，即可领取奖品；<br />
				获奖奖品的保修及售后服务依照生产商的保修承诺实施，活动主办方不负责保修。<br />
            </div>
            <div class="jqmWindow" id="dialog_draw">
                <a href="javascript:;" class="jqmClose red">X</a><br class="clear" />
                <hr>
                抽奖方式：<br />
                在海通营业部大厅进行现场抽取。<br />
            </div>

    </form>
</body>
</html>
