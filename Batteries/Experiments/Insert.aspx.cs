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
using System.Configuration;
using System.Data;
using Batteries.Dal.Base;
using Npgsql;
using NpgsqlTypes;

namespace Batteries.Experiments
{
    public partial class Insert : System.Web.UI.Page
    {

        public int experimentId;
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
            if (IsPostBack) return;

            //if (!Page.IsPostBack)
            //{
            //    //LoadTestGroups();
            //    //getProject();
            //    LoadProject();

            //    //DdlTestGroup.Enabled = false;
            //}

            //LoadTemplates();
        }


        //private void LoadTestGroups()
        //{
        //    List<ProjectTestGroupExt> experimentsList = ProjectTestGroupDa.GetAllTGProjects();
        //    int index = 0;
        //    if (experimentsList != null)
        //    {
        //        foreach (ProjectTestGroup experiment in experimentsList)
        //        {
        //            DdlTestGroup.Items.Insert(index, new ListItem(experiment.fkTestGroup != (int?)null ? experiment.fkTestGroup.ToString() + " | " + experiment.fkUser + " | " + ((DateTime)experiment.dateCreated).ToString(ConfigurationManager.AppSettings["dateFormat"]) : "", experiment.projectTestGroupId.ToString()));
        //            index++;
        //        }
        //    }
        //    DdlTestGroup.Items.Insert(0, new ListItem("", ""));
        //}
        protected void BtnSelectTemplate_Click(object sender, EventArgs e)
        {

        }

        protected void BtnStartFresh_Click(object sender, EventArgs e)
        {

        }

        protected void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                var currentUser = UserHelper.GetCurrentUser();
                //int? p = HfProjectSelectedValue.Value != "" ? int.Parse(HfProjectSelectedValue.Value) : (int?)null;
                var experiment = new Experiment
                {
                    experimentPersonalLabel = TxtExperimentPersonalLabel.Text,
                    experimentDescription = TxtExperimentDescription.Text,
                    fkUser = currentUser.userId,
                    fkResearchGroup = currentUser.fkResearchGroup,
                    fkProject = int.Parse(HfProjectSelectedValue.Value),
                    isComplete = false,
                };
                if (HfDdlTemplateSelectedValue.Value != "")
                {
                    int templateId = int.Parse(HfDdlTemplateSelectedValue.Value);
                    experiment.fkTemplate = templateId;
                }
                //string acronym = ResearchGroupDa.GetAllResearchGroups((int)currentUser.fkResearchGroup)[0].acronym;
                var result = ResearchGroupDa.GetAllResearchGroups((int)currentUser.fkResearchGroup)[0];
                string acronym = result.acronym;
                int lastExperimentNumber = (int)result.lastExperimentNumber;
                int returnedExperimentId = ExperimentDa.AddExperimentGeneralInfo(experiment, acronym, lastExperimentNumber);

                experimentId = returnedExperimentId;
                if (returnedExperimentId != 0)
                {
                    if (HfTestGroupSelectedValue.Value != "")
                    {
                        var testGroupExperiment = new TestGroupExperiment
                        {
                            fkTestGroup = int.Parse(HfTestGroupSelectedValue.Value),
                            fkExperiment = returnedExperimentId,
                            fkProject = int.Parse(HfProjectSelectedValue.Value),
                            experimentHypothesis = TxtHypothesis.Text,
                            fkUser = currentUser.userId
                        };
                        TestGroupExperimentDa.AddTestGroupExperiment(testGroupExperiment);
                    }

                    //if (HfProjectSelectedValue.Value != "")
                    //{
                    //    var projectExperiment = new ProjectExperiment
                    //    {                            
                    //        fkTestGroup = int.Parse(DdlTestGroup.SelectedValue),
                    //        fkExperiment = returnedExperimentId,
                    //        fkProject = int.Parse(HfProjectSelectedValue.Value),
                    //        experimentHypothesis = TxtHypothesis.Text
                    //    };
                    //    ProjectExperimentDa.AddProjectExperiment(projectExperiment);
                    //}

                    if (HfDdlTemplateSelectedValue.Value != "")
                    {
                        int templateId = int.Parse(HfDdlTemplateSelectedValue.Value);
                        //string selectedExperimentContent = Helpers.WebMethods.GetExperimentWithContent(templateId);
                        int copyingResult = ExperimentDa.CopyExperimentContents(templateId, returnedExperimentId);
                        if (copyingResult != 0)
                        {
                            throw new Exception("Error implementing selected previous experiment template");
                        }
                    }

                    //RedirectHelper.RedirectToReturnUrl(Request.RawUrl + '/' + returnedExperimentId, Response);
                    RedirectHelper.RedirectToReturnUrl(ResolveUrl("~/Experiments/ExperimentContents/Insert/") + returnedExperimentId, Response);
                }
                else
                {
                    NotifyHelper.Notify("Experiment general info not inserted", NotifyHelper.NotifyType.danger, "");
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
            Response.Redirect("~/Experiments/Default");
        }

    }
}