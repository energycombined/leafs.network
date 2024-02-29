using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models.Responses;
using Microsoft.AspNet.FriendlyUrls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Batteries.Experiments.Shared
{
    public partial class PublicView : System.Web.UI.Page
    {
        public int experimentId;
        public string experiment;
        protected void Page_Load(object sender, EventArgs e)
        {
            experimentId = GetExperimentIdFromUrl();

            List<ExperimentExt> experimentGeneralDataList = ExperimentDa.GetAllExperimentsGeneralData(experimentId);
            ExperimentExt experimentGeneralData = new ExperimentExt();

            if (experimentGeneralDataList != null)
            {
                experimentGeneralData = experimentGeneralDataList[0];
                if (experimentGeneralData.fkSharingType != 3)
                {
                    RedirectHelper.RedirectToReturnUrl(ResolveUrl("~/Default"), Response);
                }
            }

            experiment = Batteries.Helpers.WebMethods.GetExperimentWithContentsPublic(experimentId);
            if (experiment == "")
            {
                RedirectHelper.RedirectToReturnUrl(ResolveUrl("~/Default/"), Response);
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