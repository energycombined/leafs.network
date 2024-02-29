using Batteries.Dal;
using Batteries.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Batteries.Experiments
{
    public partial class UpdateCalculations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            List<ExperimentExt> experiments = ExperimentDa.GetAllCompleteExperimentsGeneralData(null, null, false);
            if (experiments != null)
            {
                foreach (ExperimentExt experiment in experiments)
                {
                    var summaryRes = Batteries.Dal.ExperimentDa.InsertExperimentSummary((int)experiment.experimentId, (int)experiment.fkResearchGroup, (int)experiment.fkUser);
                }
            }

            successLabel.Visible = true;
        }
    }
}