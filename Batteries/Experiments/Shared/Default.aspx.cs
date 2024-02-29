using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Batteries.Experiments.Shared
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RedirectHelper.RedirectToReturnUrl(ResolveUrl("~/Experiments/"), Response);
        }
        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static string ViewExperiment(int experimentId)
        //{

        //    var resp = new ResponseWrapper { status = "ok", response = null };
        //    var dateFormat = "dd.MM.yyyy"; // your datetime format
        //    var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };
        //    var jsonSettings = new JsonSerializerSettings();
        //    jsonSettings.DateFormatString = dateFormat;


        //    List<ExperimentExt> experimentGeneralDataList = ExperimentDa.GetAllExperimentsGeneralData(experimentId);
        //    ExperimentExt experimentGeneralData = new ExperimentExt();
        //    var currentUser = UserHelper.GetCurrentUser();
        //    int? projectId = experimentGeneralData.fkProject;

        //    List<ProjectBatchExt> projectBatchGeneralDataList = ProjectBatchDa.GetAllProjectBatchesExperiment(projectId, experimentId);
        //    ProjectBatchExt projectBatchGeneralData = new ProjectBatchExt();
        //    projectBatchGeneralData = projectBatchGeneralDataList[0];
        //    var projectBatchId = projectBatchGeneralData.projectBatchId;

        //    experimentGeneralData = experimentGeneralDataList[0];
        //    if (currentUser.fkResearchGroup == experimentGeneralData.fkResearchGroup)
        //    {
        //        try
        //        {
        //            int userId = (int)currentUser.userId;
        //            var result = ExperimentDa.EditExperiment(experimentId, projectBatchId, userId);
        //        }

        //        catch (Exception ex)
        //        {
        //            resp.status = "error";
        //            resp.message = ex.Message;
        //            return JsonConvert.SerializeObject(resp);
        //        }
        //        return JsonConvert.SerializeObject(resp, jsonSettings);
        //    }
        //    else
        //    {
        //        return JsonConvert.SerializeObject(resp);
        //        //return null;
        //    }

        //}
    }
}