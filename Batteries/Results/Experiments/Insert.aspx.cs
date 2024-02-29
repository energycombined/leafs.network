using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.FriendlyUrls;
using Batteries.Dal;
using Batteries.Models.Responses;
using Newtonsoft.Json;
using System.Configuration;
using Newtonsoft.Json.Converters;
using System.Dynamic;
using System.Globalization;
using Batteries.Helpers;

namespace Batteries.Results.Experiments
{
    public partial class Insert1 : System.Web.UI.Page
    {
        public int experimentId;
        public string experiment;

        public List<BatteryComponentExt> experimentContentList;
        public List<BatchContentExt> allbatchesContentList;
        public List<dynamic> allContent;

        public string allContentJsonString;

        public List<ExperimentProcessExt> experimentProcessList;
        public List<BatchProcessExt> allbatchesProcessList;
        public List<dynamic> allProcesses;


        public string allProcessesJsonString;


        public List<dynamic> allAnodeContent;
        public List<dynamic> allCathodeContent;
        public List<dynamic> allSeparatorContent;
        public List<dynamic> allElectrolyteContent;
        public List<dynamic> allReferenceElectrodeContent;
        public List<dynamic> allCasingContent;

        //public double anodeTotal;
        //public double anodeTotalActiveMaterials;
        //public double anodeActiveMaterialsPercentage;
        //public double cathodeTotal;
        //public double cathodeTotalActiveMaterials;
        //public double cathodeActiveMaterialsPercentage;

        public dynamic experimentSummaryData;
        public dynamic calculations;
        public string calculationsJsonString;

        public int[] ignoredMaterialFunctionsArray;

        protected void Page_Load(object sender, EventArgs e)
        {
            //ignoredMaterialFunctionsArray = Helpers.GeneralHelper.GetMaterialFunctionsToIgnore();
            experimentId = GetExperimentIdFromUrl();
            var currentUser = UserHelper.GetCurrentUser();
            
            List<ExperimentExt> experimentListObject = ExperimentDa.GetAllExperimentsGeneralData(experimentId);
            if (experimentListObject != null)
            {
                ExperimentExt experimentObject = experimentListObject[0];
                if (experimentObject.isComplete != true)
                {
                    //NotifyHelper.Notify("Error", NotifyHelper.NotifyType.danger, "");
                    RedirectHelper.RedirectToReturnUrl(ResolveUrl("~/Experiments/"), Response);
                }
                if (!ExperimentDa.HasViewPermission(experimentId, (int)currentUser.fkResearchGroup))
                {
                    RedirectHelper.RedirectToReturnUrl(ResolveUrl("~/Experiments/"), Response);
                }
                experiment = JsonConvert.SerializeObject(experimentObject, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
            }
            else
            {
                NotifyHelper.Notify("Error", NotifyHelper.NotifyType.danger, "");
                RedirectHelper.RedirectToReturnUrl("~/Experiments/Default", Response);
            }
            //experiment.

            ViewExperimentLink.DataBind();

            experimentSummaryData = Helpers.GeneralHelper.GetExperimentSummary(experimentId);
            //var a = JsonConvert.SerializeObject(experimentSummaryData);

            allContent = experimentSummaryData.summary.allContent;
            allProcesses = experimentSummaryData.summary.allProcesses;

            allContentJsonString = JsonConvert.SerializeObject(allContent);
            allProcessesJsonString = JsonConvert.SerializeObject(allProcesses);

            calculations = experimentSummaryData.calculations;
            calculationsJsonString = JsonConvert.SerializeObject(calculations);

        }

        private int GetExperimentIdFromUrl()
        {
            IList<string> segments = Request.GetFriendlyUrlSegments();
            int pId = -1;
            if (segments.Count != 0)
                int.TryParse(segments[0], out pId);
            return pId;
        }

        protected void ViewExperimentButton_Click(object sender, EventArgs e)
        {

        }


    }
}