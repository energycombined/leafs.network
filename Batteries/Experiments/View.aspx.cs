using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.FriendlyUrls;
using Batteries.Helpers;
using Batteries.Dal;
using Batteries.Models.Responses;

namespace Batteries.Experiments
{
    public partial class View : System.Web.UI.Page
    {
        public int experimentId;
        public string experiment;
        protected void Page_Load(object sender, EventArgs e)
        {
            experimentId = GetExperimentIdFromUrl();

            var currentUser = UserHelper.GetCurrentUser();

            //CHECK IF USER CAN VIEW EXPERIMENT
            List<ExperimentExt> experimentGeneralDataList = ExperimentDa.GetAllExperimentsGeneralData(experimentId);
            ExperimentExt experimentGeneralData = new ExperimentExt();
            if (experimentGeneralDataList != null)
            {
                experimentGeneralData = experimentGeneralDataList[0];
                if (experimentGeneralData.isComplete != true)
                {
                    RedirectHelper.RedirectToReturnUrl(ResolveUrl("~/Experiments/"), Response);
                }

                UserExt userCreatedBy = UserDa.GetUsers(experimentGeneralData.fkUser)[0];

                if (userCreatedBy.fkResearchGroup != currentUser.fkResearchGroup)
                {
                    RedirectHelper.RedirectToReturnUrl("~/Experiments/Shared/View/" + experimentId, Response);
                }

                //if (experimentGeneralData.fkResearchGroup != currentUser.fkResearchGroup)
                //{
                //    RedirectHelper.RedirectToReturnUrl(ResolveUrl("~/Experiments/"), Response);
                //}
            }


            experiment = Batteries.Helpers.WebMethods.GetExperimentWithContent(experimentId);
            if (experiment == "")
            {
                RedirectHelper.RedirectToReturnUrl(ResolveUrl("~/Experiments/"), Response);
            }
        }
        private int GetExperimentIdFromUrl()
        {
            IList<string> segments = Request.GetFriendlyUrlSegments();
            int pId = -1;
            if (segments.Count != 0)
                int.TryParse(segments[0], out pId);
            return pId;
        }
    }
}