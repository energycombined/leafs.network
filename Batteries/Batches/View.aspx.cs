using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.FriendlyUrls;
using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models.Responses;

namespace Batteries.Batches
{
    public partial class View : System.Web.UI.Page
    {
        public int batchId;
        public string batch;

        protected void Page_Load(object sender, EventArgs e)
        {            
            batchId = GetBatchIdFromUrl();
            //var batch = BatchDa.GetBatchWithContent(batchId);
            //batch = Batteries.Helpers.WebMethods.GetBatchWithContent(batchId);
            if (batchId != -1)
            {
                //CHECK IF USER CAN VIEW BATCH
                List<BatchExt> batchGeneralDataList = BatchDa.GetAllBatchesGeneralData(batchId);
                BatchExt batchGeneralData = new BatchExt();
                if (batchGeneralDataList != null)
                {
                    batchGeneralData = batchGeneralDataList[0];
                    if (batchGeneralData.isComplete != true)
                    {
                        RedirectHelper.RedirectToReturnUrl("~/Batches/", Response);
                    }

                    var currentUser = UserHelper.GetCurrentUser();
                    UserExt userCreatedBy = UserDa.GetUsers(batchGeneralData.fkUser)[0];

                    if (userCreatedBy.fkResearchGroup != currentUser.fkResearchGroup)
                    {
                        RedirectHelper.RedirectToReturnUrl("~/Batches/Shared/View/"+ batchId, Response);
                    }
                }

                batch = new WebMethods().GetBatchWithContent(batchId);
                if (batch == "")
                {
                    //some error page
                    NotifyHelper.Notify("Error", NotifyHelper.NotifyType.danger, "");
                    RedirectHelper.RedirectToReturnUrl("~/Batches/", Response);
                }
            }
            else
            {
                //some error page
                NotifyHelper.Notify("Error", NotifyHelper.NotifyType.danger, "");
                RedirectHelper.RedirectToReturnUrl("~/Batches/", Response);
            }

            //double batchTotal = BatchContentDa.GetBatchTotalWeight(batchId);
        }
        private int GetBatchIdFromUrl()
        {
            IList<string> segments = Request.GetFriendlyUrlSegments();
            int pId = -1;
            if (segments.Count != 0)
                int.TryParse(segments[0], out pId);
            return pId;
        }
        //BtnRecreateBatch_Click
        protected void BtnRecreateBatch_Click(object sender, EventArgs e)
        {
            //batchId
            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;

            try
            {
                int result = Batteries.Dal.BatchDa.RecreateBatch(batchId, researchGroupId);
                if (result == 0)
                {
                    NotifyHelper.Notify("Batch quantity successfully recreated", NotifyHelper.NotifyType.success, "");
                    RedirectHelper.RedirectToReturnUrl(Request.RawUrl, Response);
                }                
                //else
                //    NotifyHelper.Notify("Material not inserted", NotifyHelper.NotifyType.danger, "");
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                NotifyHelper.Notify(ex.Message, NotifyHelper.NotifyType.danger, "");
            }
        }
    }
}