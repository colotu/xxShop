using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YSWL.MALL.BLL.Shop.Trial;

namespace YSWL.MALL.Web.Areas.Shop.Controllers
{
    public class TrialController : ShopControllerBase
    {
        //
        // GET: /Shop/Trial/
        private  YSWL.MALL.BLL.Shop.Trial.TrialReports reportBll=new TrialReports();
        private  YSWL.MALL.BLL.Shop.Trial.Trials trialBll=new Trials();
        public ActionResult Detail(int id=0)
        {
            YSWL.MALL.Model.Shop.Trial.Trials trialModel = trialBll.GetModel(id);

            ViewBag.MoreLinkUrl = BLL.SysManage.ConfigSystem.GetValue("Shop_Trial_LinkMoreUrl");

            //从导航进来
            if (trialModel == null)
            {
                string strWhere = "  EndDate >'" + DateTime.Now.ToString("yyyy-MM-dd") + "' and StartDate< '" +
                                  DateTime.Now.ToString("yyyy-MM-dd") + "'";
                List<YSWL.MALL.Model.Shop.Trial.Trials> trialsList= trialBll.GetTopList(1, strWhere, " TrialId desc");
                if (trialsList != null && trialsList.Count > 0)
                {
                    trialModel = trialsList[0];
                }
            }
            if (trialModel!=null)
            {
                //判断该试用状态
                if (DateTime.Now.CompareTo(trialModel.StartDate) >= 0 && DateTime.Now.CompareTo(trialModel.EndDate) <= 0)
                {
                    trialModel.TrialStatus = 1;
                }
                if (DateTime.Now.CompareTo(trialModel.StartDate) < 0)
                {
                    trialModel.TrialStatus = 0;
                }
                if (DateTime.Now.CompareTo(trialModel.EndDate) > 0)
                {
                    trialModel.TrialStatus = 2;
                }
            }
          

            #region SEO优化
            ViewBag.Title =trialModel==null?"试用详细页面": trialModel.TrialName +" - 试用详细页面";
            #endregion
            return View(trialModel);
        }
        //试用
        public PartialViewResult UseTrial( int Top=4,string viewName = "_UseTrial")
        {
            List<YSWL.MALL.Model.Shop.Trial.Trials> trialList = trialBll.GetTopList(Top, " TrialStatus=1 ",
                                                                                    " TrialId Desc");
            return PartialView(viewName, trialList);
        }
        //试用报告
        public PartialViewResult TrialReport(int Top = 6, string viewName = "_TrialReport")
        {
            List<YSWL.MALL.Model.Shop.Trial.TrialReports> reportList = reportBll.GetTopList(Top, " ",
                                                                                    " ReportId Desc");
            return PartialView(viewName, reportList);
        }
        //更多试用
        public PartialViewResult TrialPart(int Top = 8, string viewName = "_TrialPart")
        {
            List<YSWL.MALL.Model.Shop.Trial.Trials> trialList = trialBll.GetTopList(Top, "",
                                                                                    " TrialId Desc");
            ViewBag.MoreLinkUrl = BLL.SysManage.ConfigSystem.GetValue("Shop_Trial_LinkMoreUrl");
            return PartialView(viewName, trialList);
        }
    }
}
