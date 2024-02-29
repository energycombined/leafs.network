using Batteries.Dal;
using Batteries.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.FriendlyUrls;
using Batteries.Models.Responses;


namespace Batteries.Batches.BatchContents
{
    public partial class Insert : System.Web.UI.Page
    {
        public int batchId;
        public string batch;
        public string materialFunctionsJson;

        private int GetBatchIdFromUrl()
        {
            IList<string> segments = Request.GetFriendlyUrlSegments();
            int pId = -1;
            if (segments.Count != 0)
                int.TryParse(segments[0], out pId);
            return pId;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            batchId = GetBatchIdFromUrl();
            
            materialFunctionsJson = MaterialFunctionDa.GetAllMaterialFunctionsJsonForDropdown();

            if (batchId != -1)
            {
                List<BatchExt> batchGeneralDataList = BatchDa.GetAllBatchesGeneralData(batchId);
                BatchExt batchGeneralData = new BatchExt();

                if (batchGeneralDataList != null)
                {
                    batchGeneralData = batchGeneralDataList[0];                    
                }
                else
                {
                    //if null error page
                    NotifyHelper.Notify("Error", NotifyHelper.NotifyType.danger, "");
                    RedirectHelper.RedirectToReturnUrl("~/Batches/", Response);
                }

                var currentUser = UserHelper.GetCurrentUser();
                UserExt userCreatedBy = UserDa.GetUsers(batchGeneralData.fkUser)[0];

                if (userCreatedBy.fkResearchGroup != currentUser.fkResearchGroup)
                {
                    RedirectHelper.RedirectToReturnUrl("~/Batches/", Response);
                }

                if (batchGeneralData.isComplete == true)
                {
                    RedirectHelper.RedirectToReturnUrl("~/Batches/View" + '/' + batchGeneralData.batchId, Response);
                }
                

                var templateId = batchGeneralData.fkTemplate;
                if (templateId != null)
                {
                    BatchExt templateBatchGeneralData = BatchDa.GetAllBatchesGeneralData((int)templateId)[0];
                    string templateSystemLabel = templateBatchGeneralData.batchSystemLabel;
                    TxtTemplateBatchSystemLabel.Text = templateSystemLabel + " | " + templateBatchGeneralData.batchPersonalLabel;
                    TxtTemplateBatchSystemLabel.Visible = true;
                    LblTemplateBatchSystemLabel.Visible = true;
                }
                batch = new WebMethods().GetBatchInProgressWithContent(batchId);

                if (IsPostBack) return;

            }
            else
            {
                //some error page
                NotifyHelper.Notify("Error", NotifyHelper.NotifyType.danger, "");
                RedirectHelper.RedirectToReturnUrl("~/Batches/", Response);
            }
        }
        

    }
}