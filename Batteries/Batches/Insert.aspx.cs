using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models;
using Batteries.Models.Requests;
using Batteries.Models.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Batteries.Batches
{
    public partial class Insert : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            //LoadTemplates();

        }
        private void LoadTemplates()
        {
            List<BatchExt> batchesList = BatchDa.GetAllCompleteBatches();
            int index = 0;
            if (batchesList != null)
            {
                foreach (Batch batch in batchesList)
                {
                    DdlTemplate.Items.Insert(index, new ListItem(batch.batchSystemLabel + " | " + batch.batchPersonalLabel + " | " + ((DateTime)batch.dateCreated).ToString(ConfigurationManager.AppSettings["dateFormat"]), batch.batchId.ToString()));
                    index++;
                }
            }
            DdlTemplate.Items.Insert(0, new ListItem("", ""));
        }

        protected void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                var currentUser = UserHelper.GetCurrentUser();
                var batch = new Batch
                {
                    //batchPersonalLabel = TxtBatchPersonalLabel.Text,
                    fkUser = currentUser.userId,
                    fkProject = int.Parse(HfProjectSelectedValue.Value),
                    //description = TxtBatchDescription.Text,
                    //batchOutput = (TxtBatchOutput.Text != "") ? double.Parse(TxtBatchOutput.Text) : (double?)null,
                    //fkMeasurementUnit = (DdlMeasurementUnit.SelectedValue != "") ? int.Parse(DdlMeasurementUnit.SelectedValue) : (int?)null,
                    //chemicalFormula = TxtBatchChemicalFormula.Text,
                    //fkMaterialType = (DdlMaterialType.SelectedValue != "") ? int.Parse(DdlMaterialType.SelectedValue) : (int?)null,
                    isComplete = false,
                    fkResearchGroup = (int)currentUser.fkResearchGroup
                };
                if (HfDdlTemplateSelectedValue.Value != "")
                {
                    int templateId = int.Parse(HfDdlTemplateSelectedValue.Value);
                    batch.fkTemplate = templateId;
                }
                var result = ResearchGroupDa.GetAllResearchGroups((int)currentUser.fkResearchGroup)[0];
                string acronym = result.acronym;
                int lastBatchNumber = (int)result.lastBatchNumber;
                int returnedBatchId = BatchDa.AddBatchGeneralInfo(batch, acronym, lastBatchNumber);
                if (returnedBatchId > 0)
                {
                    if (HfDdlTemplateSelectedValue.Value != "")
                    {
                        int templateId = int.Parse(HfDdlTemplateSelectedValue.Value);
                        //string selectedExperimentContent = Helpers.WebMethods.GetExperimentWithContent(templateId);

                        //BatchResponse templateBatch = GeneralHelper.GetBatchWithContent(templateId);                        
                        string templateBatch = new Helpers.WebMethods().GetBatchWithContent(templateId);
                        object submittedItem;
                        var dateFormat = "dd/MM/yyyy"; // your datetime format
                        var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };
                        submittedItem = JsonConvert.DeserializeObject<AddBatchRequest>(templateBatch, dateTimeConverter);

                        AddBatchRequest submittedItemClass = (AddBatchRequest)submittedItem;

                        //INSERT BATCH CONTENT
                        //BatchDa.AddBatchContent(req, returnedBatchID, researchGroupId, cmd);

                        //AddBatchRequest batchRequest = new AddBatchRequest();
                        ////batchRequest.batchInfo.fkUser = currentUser.UserId;
                        //batchRequest.batchContent = templateBatch.batchContent;
                        //batchRequest.batchProcesses = templateBatch.batchProcesses;
                        //batchRequest.measurements = templateBatch.measurements;

                        var res = BatchDa.AddBatchContent(submittedItemClass, returnedBatchId, (int)currentUser.fkResearchGroup, null);
                        if (res != 0)
                        {
                            throw new Exception("Error implementing selected template");
                        }
                    }

                    //NotifyHelper.Notify("Success", NotifyHelper.NotifyType.success, "");
                    RedirectHelper.RedirectToReturnUrl(ResolveUrl("~/Batches/BatchContents/Insert/") + returnedBatchId, Response);
                }
                else
                {
                    NotifyHelper.Notify("Batch general info not inserted", NotifyHelper.NotifyType.danger, "");
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                NotifyHelper.Notify(ex.Message, NotifyHelper.NotifyType.danger, "");
            }
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Batches/Default");
        }


    }
}