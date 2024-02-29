using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Batteries.Experiments
{
    public partial class Default : System.Web.UI.Page
    {
        int? projectId = null;
        public int currentRG;
        public int? statusId;
        int noRecord = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            var currentUser = UserHelper.GetCurrentUser();
            currentRG = (int)currentUser.fkResearchGroup;

            NoOfProjects();
            NoOfTestGroups();
            NoOfResearchGroups();
            NoOfBatches();
            //Helpers.WebMethods.GetExperimentMaterials(experimentId);
            //Helpers.WebMethods.GetMaterialDataForCsv(null);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string EditExperiment(int experimentId)
        {

            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd.MM.yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };
            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            List<ExperimentExt> experimentGeneralDataList = ExperimentDa.GetAllExperimentsGeneralData(experimentId);
            ExperimentExt experimentGeneralData = new ExperimentExt();
            var currentUser = UserHelper.GetCurrentUser();
            int? projectId = experimentGeneralData.fkProject;

            //List<ProjectBatchExt> projectBatchGeneralDataList = ProjectBatchDa.GetAllProjectBatchesExperiment(projectId,  experimentId);
            //ProjectBatchExt projectBatchGeneralData = new ProjectBatchExt();
            //projectBatchGeneralData = projectBatchGeneralDataList[0];
            //var projectBatchId = projectBatchGeneralData.projectBatchId;

            experimentGeneralData = experimentGeneralDataList[0];
            if (currentUser.fkResearchGroup == experimentGeneralData.fkResearchGroup)
            {
                try
                {
                    int userId = (int)currentUser.userId;
                    var result = ExperimentDa.EditExperiment(experimentId, userId);
                }

                catch (Exception ex)
                {
                    resp.status = "error";
                    resp.message = ex.Message;
                    return JsonConvert.SerializeObject(resp);
                }
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
            else
            {
                return JsonConvert.SerializeObject(resp);
                //return null;
            }
        }

        void NoOfProjects()
        {
            List<ProjectExt> projectDataList = ProjectDa.CountAllProjects();
            if (projectDataList != null)
            {
                Int32 rows_count = Convert.ToInt32(projectDataList.Count);
                noProjects.InnerText = rows_count.ToString();
            }
            else
            {
                noProjects.InnerText = noRecord.ToString();
            }
        }

        void NoOfTestGroups()
        {
            List<TestGroupExt> testGroupDataList = TestGroupDa.GetAllTestGroups();
            if (testGroupDataList != null)
            {
                Int32 rows_count = Convert.ToInt32(testGroupDataList.Count);
                noTestGroups.InnerText = rows_count.ToString();
            }
            else
            {
                noTestGroups.InnerText = noRecord.ToString();
            }
        }

        void NoOfResearchGroups()
        {
            List<ResearchGroupExt> RGDataList = ResearchGroupDa.GetAllResearchGroups();
            if (RGDataList != null)
            {
                Int32 rows_count = Convert.ToInt32(RGDataList.Count);
                noResearchGrop.InnerText = rows_count.ToString();
            }
            else
            {
                noTestGroups.InnerText = noRecord.ToString();
            }
        }

        void NoOfBatches()
        {
            List<BatchExt> batchDataList = BatchDa.GetAllCompleteBatches();
            if (batchDataList != null)
            {
                Int32 rows_count = Convert.ToInt32(batchDataList.Count);
                noBatch.InnerText = rows_count.ToString();
            }
            else
            {
                noBatch.InnerText = noRecord.ToString();
            }
        }
    }

}