using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.FriendlyUrls;
using Batteries.Models.Responses;

namespace Batteries.Experiments.ExperimentContents
{
    public partial class Insert : System.Web.UI.Page
    {
        public int experimentId;
        public int? projectId;
        public string experiment;
        public string materialFunctionsJson;
        private int GetExperimentIdFromUrl()
        {
            IList<string> segments = Request.GetFriendlyUrlSegments();
            int pId = -1;
            if (segments.Count != 0)
                int.TryParse(segments[0], out pId);
            return pId;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            var currentUser = UserHelper.GetCurrentUser();
            //WebMethods.GetAllExperimentComponents(9, 1);

            experimentId = GetExperimentIdFromUrl();
            

            //string resultJson = MaterialFunctionDa.GetAllMaterialFunctionsJsonForDropdown();
            //materialFunctionsJson = "{\"results\":" + resultJson + "}";
            materialFunctionsJson = MaterialFunctionDa.GetAllMaterialFunctionsJsonForDropdown();
            //var materialFunctionsObject = new 
          
            if (experimentId != -1)
            {
                List<ExperimentExt> experimentGeneralDataList = ExperimentDa.GetAllExperimentsGeneralData(experimentId);
                ExperimentExt experimentGeneralData = new ExperimentExt();
                if (experimentGeneralDataList != null)
                {                    
                    experimentGeneralData = experimentGeneralDataList[0];

                    projectId = experimentGeneralData.fkProject;
                                      
                    if (currentUser.fkResearchGroup != experimentGeneralData.fkResearchGroup)
                    {
                        NotifyHelper.Notify("Editing is not allowed, this Experiment is not created by your Research Group!", NotifyHelper.NotifyType.warning, "");
                        RedirectHelper.RedirectToReturnUrl("~/Experiments/", Response);
                    }

                    if (experimentGeneralData.isComplete == true)
                    {
                        //NotifyHelper.Notify("Experiment completed", NotifyHelper.NotifyType.danger, "");
                        RedirectHelper.RedirectToReturnUrl("~/Experiments/", Response);
                    }                    
                }
                else
                {
                    //if null error page
                    NotifyHelper.Notify("Error", NotifyHelper.NotifyType.danger, "");
                    RedirectHelper.RedirectToReturnUrl("~/Experiments/", Response);
                }

                if (experimentGeneralData.isComplete == true)
                {
                    RedirectHelper.RedirectToReturnUrl("~/Experiments/View" + '/' + experimentGeneralData.experimentId, Response);
                }
                
                var templateId = experimentGeneralData.fkTemplate;
                if (templateId != null)
                {
                    Experiment templateExperimentGeneralData = ExperimentDa.GetAllExperimentsGeneralData((int)templateId)[0];
                    string templateSystemLabel = templateExperimentGeneralData.experimentSystemLabel;
                    TxtTemplateExperimentSystemLabel.Text = templateSystemLabel + " | " + templateExperimentGeneralData.experimentPersonalLabel;
                    TxtTemplateExperimentSystemLabel.Visible = true;
                    LblTemplateExperimentSystemLabel.Visible = true;
                }
                experiment = Helpers.WebMethods.GetExperimentInProgressWithContent(experimentId);

                if (IsPostBack) return;

                //FillExperimentGeneralInfo(experimentGeneralData);

            }
            else
            {
                //some error page
                NotifyHelper.Notify("Error", NotifyHelper.NotifyType.danger, "");
                RedirectHelper.RedirectToReturnUrl("~/Experiments/", Response);
            }
        }
        //private void FillExperimentGeneralInfo(Experiment experiment)
        //{
        //    //TxtExperimentSystemLabel.Text = "EXP_" + experiment.experimentId;
        //    TxtExperimentSystemLabel.Text = experiment.experimentSystemLabel;
        //    TxtExperimentPersonalLabel.Text = experiment.experimentPersonalLabel;
        //    TxtExperimentDescription.Text = experiment.experimentDescription;
        //}

        //protected void UpdateExperimentInfoButton_OnClick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var experiment = new Experiment
        //        {
        //            experimentId = GetExperimentIdFromUrl(),
        //            experimentPersonalLabel = TxtExperimentPersonalLabel.Text,
        //            experimentDescription = TxtExperimentDescription.Text
        //        };
        //        var result = ExperimentDa.UpdateExperimentGeneralData(experiment);
        //        if (result == 0)
        //        {
        //            //NotifyHelper.Notify("Success", NotifyHelper.NotifyType.success, "");
        //            //RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
        //        }
        //        else
        //            NotifyHelper.Notify("Experiment info not updated", NotifyHelper.NotifyType.danger, "");
        //    }
        //    catch (System.Threading.ThreadAbortException)
        //    {
        //    }
        //    catch (Exception ex)
        //    {
        //        NotifyHelper.Notify(ex.Message, NotifyHelper.NotifyType.danger, "");
        //    }
        //}
    }
}