using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.FriendlyUrls;

namespace Batteries.Batches.Shared
{
    public partial class View : System.Web.UI.Page
    {
        public int batchId;
        //int? projectId = null;
        public string batch;
        protected void Page_Load(object sender, EventArgs e)
        {
            var currentUser = UserHelper.GetCurrentUser();
            batchId = GetBatchIdFromUrl();
            if (batchId != -1)
            {
                //CHECK IF USER CAN VIEW BATCH

                //List<ProjectBatchExt> projectBatchGeneralDataList = ProjectBatchDa.GetAllBatches(batchId);
                //ProjectBatchExt projectBatchGeneralData = new ProjectBatchExt();

                List<BatchExt> batchGeneralDataList = BatchDa.GetAllBatchesGeneralData(batchId);
                BatchExt batchGeneralData = new BatchExt();

                if (batchGeneralDataList != null)
                {
                    batchGeneralData = batchGeneralDataList[0];

                    if (!BatchDa.HasViewPermission(batchId, (int)currentUser.fkResearchGroup))
                    {
                        RedirectHelper.RedirectToReturnUrl(ResolveUrl("~/Batches/"), Response);
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
                    //NotifyHelper.Notify("You are not participant in this Project/Batch", NotifyHelper.NotifyType.warning, "");
                    RedirectHelper.RedirectToReturnUrl("~/Batches/", Response);
                }
                //if (batchGeneralDataList != null)
                //{                    
                //    batchGeneralData = batchGeneralDataList[0];

                //    if (batchGeneralData.isComplete != true)
                //    {
                //        RedirectHelper.RedirectToReturnUrl("~/Batches/Shared", Response);
                //    }             
                //    UserExt userCreatedBy = UserDa.GetUsers(batchGeneralData.fkUser)[0];

                //    if (userCreatedBy.fkResearchGroup == currentUser.fkResearchGroup)
                //    {
                //        RedirectHelper.RedirectToReturnUrl("~/Batches/Shared", Response);
                //    }
                //}

            }
            else
            {
                //some error page
                NotifyHelper.Notify("Error", NotifyHelper.NotifyType.danger, "");
                //RedirectHelper.RedirectToReturnUrl("~/Batches/Shared", Response);
            }

        }
        private int GetBatchIdFromUrl()
        {
            IList<string> segments = Request.GetFriendlyUrlSegments();
            int pId = -1;
            if (segments.Count != 0)
                int.TryParse(segments[0], out pId);
            return pId;
        }
    }
}