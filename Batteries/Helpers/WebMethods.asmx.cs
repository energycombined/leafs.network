using Batteries.Dal;
using Batteries.Dal.Base;
using Batteries.Dal.EquipmentDal;
using Batteries.Dal.ProcessesDal;
using Batteries.Mappings;
using Batteries.Models;
using Batteries.Models.Requests;
using Batteries.Models.Responses;
using Batteries.Models.Responses.ProcessModels;
using Batteries.Models.TestResultsModels;
using CsvHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Services;
using System.Web.Services;

namespace Batteries.Helpers
{
    /// <summary>
    /// Summary description for WebMethods
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebMethods : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetExperimentMassCalculations(int experimentId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();

            try
            {
                dynamic calculations = Helpers.GeneralHelper.GetMassCalculations(experimentId);

                //List<MaterialExt> materials = MaterialDa.GetAllMaterialsWithQuantity(currentUser.fkResearchGroup, materialId, materialType);
                return JsonConvert.SerializeObject(calculations, jsonSettings);
            }
            catch (Exception e)
            {
                //return "Error! " + e.Message;
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetExperimentChargeDischargeTestResults(int experimentId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();

            try
            {
                List<ChargeDischargeTestResult> results = ChargeDischargeTestResultDa.GetAllChargeDischargeTestData(experimentId);
                return JsonConvert.SerializeObject(results, jsonSettings);
            }
            catch (Exception e)
            {
                //return "Error! " + e.Message;
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetAllResearchGroupList(int? researchGroupId = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();

            try
            {
                List<ResearchGroupExt> researchGroup = ResearchGroupDa.GetAllResearchGroups(researchGroupId);
                return JsonConvert.SerializeObject(researchGroup, jsonSettings);
            }
            catch (Exception e)
            {
                //return "Error! " + e.Message;
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetAllMaterialsList(int? materialId = null, int? materialType = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();

            try
            {
                List<MaterialExt> materials = MaterialDa.GetAllMaterialsWithQuantity(currentUser.fkResearchGroup, materialId, materialType);
                return JsonConvert.SerializeObject(materials, jsonSettings);
            }
            catch (Exception e)
            {
                //return "Error! " + e.Message;
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetMaterials(string search, int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                //BatchExt batch = BatchDa.GetBatchWithContent(batchId);
                List<MaterialExt> resultList = MaterialDa.GetMaterialsByName(search, currentUser.fkResearchGroup, page);
                //Array json;
                //json.items = resultList.ToString;
                //JArray array = new JArray();
                //array.Add("items");

                //JObject rss;
                //if (resultList != null)
                //{
                //     rss =
                //     new JObject(
                //        //new JProperty("items",
                //        //new JObject(
                //        //new JProperty("title", "James Newton-King")
                //     new JProperty("results",
                //        new JArray(
                //            from m in resultList
                //            orderby m.materialName
                //            select new JObject(
                //                new JProperty("id", m.materialId),
                //                new JProperty("text", m.materialName),
                //                new JProperty("chemicalFormula", m.chemicalFormula)
                //                //)
                //                //)
                //                )
                //     )
                //     ),
                //     new JProperty("pagination", "true")

                //     );


                //return JsonConvert.SerializeObject(resultList, jsonSettings);

                //var res = JsonConvert.SerializeObject(rss, jsonSettings);
                //return res;
                //}
                //else
                //{
                //     rss =
                //     new JObject(
                //     new JProperty("results",
                //        new JArray())
                //     );
                //}

                //var res = JsonConvert.SerializeObject(rss, jsonSettings);
                //return res;

                dynamic response = new ExpandoObject();
                response.results = resultList;
                response.pagination = new ExpandoObject();
                response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);

                //return rss.ToString();
                //string a = "{  \"results\": [   {\"id\": 1,\"text\": \"Troilite\"}  ],  \"pagination\": \"true\"}";
                //return a;

            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetMaterialsWithQuantity(string search, int? materialFunction = null, int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            //List<MaterialExt> materials = MaterialDa.GetAllMaterialsWithQuantity(currentUser.fkResearchGroup, materialId, materialType);

            try
            {
                List<MaterialExt> resultList = MaterialDa.GetMaterialsByNameWithQuantity(search, materialFunction, currentUser.fkResearchGroup, page);
                dynamic response = new ExpandoObject();
                response.results = resultList;
                response.pagination = new ExpandoObject();
                response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);

            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetAllExperimentsList(int? experimentId = null, int? researchGroupId = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                List<ExperimentExt> experiments = ExperimentDa.GetAllExperimentsListForResearchGroup((int)currentUser.fkResearchGroup, experimentId);
                return JsonConvert.SerializeObject(experiments, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetAllPublicExperimentsList()
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            //var currentUser = UserHelper.GetCurrentUser();
            try
            {
                List<ExperimentExt> experiments = ExperimentDa.GetAllPublicExperimentsList();
                return JsonConvert.SerializeObject(experiments, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetExperimentsListFromOtherRG(int? experimentId = null, int? researchGroupId = null, int? otherResearchGroupId = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                List<ExperimentExt> experiments = ExperimentDa.GetExperimentsListFromOtherRG((int)currentUser.fkResearchGroup, experimentId, otherResearchGroupId);
                return JsonConvert.SerializeObject(experiments, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetExperimentsByIdForCharts(int[] experimentIdArray = null, int? researchGroupId = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                List<ExperimentExt> experiments = ExperimentDa.GetExperimentsByIdForCharts((int)currentUser.fkResearchGroup, experimentIdArray);
                return JsonConvert.SerializeObject(experiments, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetExperimentById(int experimentId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                ExperimentExt experiment = ExperimentDa.GetExperimentByIdAndRGCreator(experimentId, (int)currentUser.fkResearchGroup);
                return JsonConvert.SerializeObject(experiment, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetUnfinishedExperimentsList(int? experimentId = null, int? researchGroupId = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                List<ExperimentExt> experiments = ExperimentDa.GetUnfinishedExperimentsList(experimentId, currentUser.fkResearchGroup);
                return JsonConvert.SerializeObject(experiments, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetAllExperimentsInTestGroup(int? testGroupId = null, int? researchGroupId = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                List<TestGroupExperimentExt> experiments = TestGroupExperimentDa.GetAllTestGroupExperiments(testGroupId, null, null, null);
                return JsonConvert.SerializeObject(experiments, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetAllExperimentsInProject(int projectId, int? researchGroupId = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                List<ExperimentExt> experiments = ExperimentDa.GetAllExperimentsByProject(projectId, null);
                return JsonConvert.SerializeObject(experiments, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetAllBatchesInProject(int? projectId = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                List<Batch> batches = BatchDa.GetDistinctBatchesInProject(projectId);
                return JsonConvert.SerializeObject(batches, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetAllRGInProject(int projectId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                List<ProjectResearchGroupExt> projectBatches = ProjectResearchGroupDa.GetAllProjectResearchGroups(projectId);
                return JsonConvert.SerializeObject(projectBatches, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetExperimentsForCharts(int? researchGroupIdCreator = null, int? operatorId = null, int? projectId = null, int? testGroupId = null, int? testTypeId = null, string search = "", int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                int currentResearchGroupId = (int)currentUser.fkResearchGroup;
                List<ExperimentExt> experiments = ExperimentDa.GetExperimentsForCharts(currentResearchGroupId, researchGroupIdCreator, operatorId, projectId, testGroupId, testTypeId, search, page);

                dynamic response = new ExpandoObject();
                response.results = experiments;
                response.pagination = new ExpandoObject();
                response.pagination.more = experiments != null ? experiments.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        //Not used currently
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetExperimentsFilteredPaged(string search, int? operatorId = null, int? projectId = null, int? testGroupId = null, int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                //researchGroupId = (int)currentUser.fkResearchGroup;
                List<ExperimentExt> experiments = ExperimentDa.GetExperimentsFilteredPaged((int)currentUser.fkResearchGroup, operatorId, projectId, testGroupId, search, page);

                dynamic response = new ExpandoObject();
                response.results = experiments;
                response.pagination = new ExpandoObject();
                response.pagination.more = experiments != null ? experiments.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetAllExperimentsOutsideTestGroupFromProject(int testGroupId, string search, int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                List<TestGroupExt> testGGeneralDataList = TestGroupDa.GetAllTestGroups(testGroupId);
                TestGroupExt testGGeneralData = testGGeneralDataList[0];

                int projectId = (int)testGGeneralData.fkProject;
                List<ExperimentExt> experiments = ExperimentDa.GetExperimentsOutsideTestGroupByProjectPaged(testGroupId, projectId, null, search, page);
                dynamic response = new ExpandoObject();
                response.results = experiments;
                response.pagination = new ExpandoObject();
                response.pagination.more = experiments != null ? experiments.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetResearchGroupsOutsideProject(int projectId, string search, int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                List<ResearchGroupExt> experiments = ResearchGroupDa.GetResearchGroupsOutsideProjectPaged(projectId, search, page);

                dynamic response = new ExpandoObject();
                response.results = experiments;
                response.pagination = new ExpandoObject();
                response.pagination.more = experiments != null ? experiments.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetAllTestGroupsByExperiment(int? experimentId = null, int? researchGroupId = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                List<TestGroupExperimentExt> experiments = TestGroupExperimentDa.GetAllTestGroupExperiments(null, null, experimentId, null);
                return JsonConvert.SerializeObject(experiments, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetAllTestGroupsByExperimentNotInside(string search, int experimentId, int? researchGroupId = null, int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                //FROM OWN RESEARCH GROUP ONLY
                //List<TestGroupExt> testgroups = TestGroupDa.GetTestGroupsByExperimentExcluded(null, experimentId, currentUser.fkResearchGroup, page);
                Experiment experimentInfo = ExperimentDa.GetExperimentById(experimentId, (int)currentUser.fkResearchGroup);
                int projectId = (int)experimentInfo.fkProject;
                List<TestGroupExt> testgroups = TestGroupDa.GetTestGroupsByProjectExperimentExcluded(null, experimentId, projectId, null, page);
                //return JsonConvert.SerializeObject(experiments, jsonSettings);
                dynamic response = new ExpandoObject();
                response.results = testgroups;
                response.pagination = new ExpandoObject();
                response.pagination.more = testgroups != null ? testgroups.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetProjects(string search, int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                List<ProjectExt> resultList = ProjectDa.GetProjectsByRGCreatorPaged(search, currentUser.fkResearchGroup, page);


                dynamic response = new ExpandoObject();
                response.results = resultList;
                response.pagination = new ExpandoObject();
                response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);


            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetAllTestsDoneByExperiment(int? experimentId = null, int? testTypeId = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                List<TestExt> tests = TestDa.GetAllTests(null, experimentId, testTypeId);
                return JsonConvert.SerializeObject(tests, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        //To be removed
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string AddTest(string formData)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                dynamic dynData = JsonConvert.DeserializeObject(formData);
                object submittedItem;

                submittedItem = JsonConvert.DeserializeObject<Test>(formData, dateTimeConverter);
                Test submittedItemClass = (Test)submittedItem;
                submittedItemClass.fkUser = currentUser.userId;

                //Batteries.Dal.TestDa.AddTest(submittedItemClass, null);
            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }

            return JsonConvert.SerializeObject(resp, jsonSettings);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetAllBatchesList(int? batchId = null, int? materialType = null, int? researchGroupId = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            //researchGroupId = currentUser.fkResearchGroup;

            try
            {
                List<BatchExt> batches = BatchDa.GetAllCompleteBatchesWithQuantity((int)currentUser.fkResearchGroup, batchId, materialType);
                return JsonConvert.SerializeObject(batches, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetBatchesListFromOtherRG(int? batchId = null, int? materialType = null, int? researchGroupId = null, int? otherResearchGroupId = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            //researchGroupId = currentUser.fkResearchGroup;

            try
            {
                List<BatchExt> batches = BatchDa.GetBatchesFromOtherRG(currentUser.fkResearchGroup, otherResearchGroupId, batchId, materialType);
                return JsonConvert.SerializeObject(batches, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetUnfinishedBatchesList(int? batchId = null, int? materialType = null, int? researchGroupId = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();

            try
            {
                List<BatchExt> batches = BatchDa.GetUnfinishedBatches(currentUser.fkResearchGroup, batchId, materialType);
                return JsonConvert.SerializeObject(batches, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetAllEquipmentList(int? equipmentId = null, int? processType = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();

            try
            {
                List<EquipmentExt> equipment = EquipmentDa.GetAllEquipment(processType, equipmentId);
                return JsonConvert.SerializeObject(equipment);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetAllEquipmentModelsList(int? equipmentId = null, int? equipmentModelId = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();

            try
            {
                List<EquipmentModelExt> equipmentModels = EquipmentModelDa.GetAllEquipmentModels(equipmentId, equipmentModelId);
                return JsonConvert.SerializeObject(equipmentModels);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetResearchGroups(string search, int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            //int researchGroupId = (int)currentUser.fkResearchGroup;
            try
            {
                List<ResearchGroup> resultList = ResearchGroupDa.GetResearchGroupsByName(search, null, page);
                dynamic response = new ExpandoObject();
                response.results = resultList;
                response.pagination = new ExpandoObject();
                response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);
                //return JsonConvert.SerializeObject(resultList, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetResearchGroupsByProjectId(string search, int? page = null, int? projectId = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            //int researchGroupId = (int)currentUser.fkResearchGroup;
            try
            {
                List<ResearchGroup> resultList = ResearchGroupDa.GetResearchGroupsByProjectId(search, null, page, projectId);
                dynamic response = new ExpandoObject();
                response.results = resultList;
                response.pagination = new ExpandoObject();
                response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);
                //return JsonConvert.SerializeObject(resultList, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetOtherResearchGroups(string search, int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;
            try
            {
                List<ResearchGroup> resultList = ResearchGroupDa.GetOtherResearchGroupsByName(researchGroupId, search, null, page);
                dynamic response = new ExpandoObject();
                response.results = resultList;
                response.pagination = new ExpandoObject();
                response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);
                //return JsonConvert.SerializeObject(resultList, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetTestGroups(string search, int? researchGroupId = null, int? projectId = null, int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                List<TestGroupExt> resultList = TestGroupDa.GetTestGroupsByResearchGroupCreator(search, researchGroupId, page);
                dynamic response = new ExpandoObject();
                response.results = resultList;
                response.pagination = new ExpandoObject();
                response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);
                //return JsonConvert.SerializeObject(resultList, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetTestGroupsForResearchGroup(string search, int? researchGroupId = null, int? projectId = null, int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                List<TestGroupExt> resultList = TestGroupDa.GetTestGroupsForResearchGroup(search, currentUser.fkResearchGroup, researchGroupId, projectId, page);
                dynamic response = new ExpandoObject();
                response.results = resultList;
                response.pagination = new ExpandoObject();
                response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);
                //return JsonConvert.SerializeObject(resultList, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetTestGroupsFromOwnRG(string search, int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;
            try
            {
                List<TestGroupExt> resultList = TestGroupDa.GetTestGroupsByResearchGroupCreator(search, researchGroupId, page);
                dynamic response = new ExpandoObject();
                response.results = resultList;
                response.pagination = new ExpandoObject();
                response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetTestGroupsFromProject(string search, int? page = null, int? projectId = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;
            try
            {
                List<TestGroupExt> resultList = TestGroupDa.GetAllTestGroupsByProjectForDropdown(search, page, projectId);
                dynamic response = new ExpandoObject();
                response.results = resultList;
                response.pagination = new ExpandoObject();
                response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }





        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetProjectsFromOwnRG(string search, int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;
            try
            {
                List<ProjectExt> resultList = ProjectDa.GetProjectsByRGCreatorPaged(search, researchGroupId, page);
                dynamic response = new ExpandoObject();
                response.results = resultList;
                response.pagination = new ExpandoObject();
                response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetProjectsForResearchGroup(string search, int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;
            try
            {
                List<ProjectExt> resultList = ProjectDa.GetProjectsByRGParticipantPaged(search, researchGroupId, page);
                dynamic response = new ExpandoObject();
                response.results = resultList;
                response.pagination = new ExpandoObject();
                response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetCompleteExperimentsPaged(string search, int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;
            try
            {
                List<ExperimentExt> resultList = ExperimentDa.GetCompleteExperimentsPaged(researchGroupId, search, page);
                dynamic response = new ExpandoObject();
                response.results = resultList;
                response.pagination = new ExpandoObject();
                response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetBatchForMeasurementsDropdown(string search, int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;
            try
            {
                List<Batch> resultList = BatchDa.GetBatchForMeasurementsDropdown(search, page, researchGroupId);
                dynamic response = new ExpandoObject();
                response.results = resultList;
                response.pagination = new ExpandoObject();
                response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetBatchesForCharts(int? projectId = null, int? operatorId = null, int? researchGroupIdCreator = null, int? testTypeId = null, string search = null, int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            int currentResearchGroupId = (int)currentUser.fkResearchGroup;
            try
            {
                List<DropdownItem> resultList = BatchDa.GetBatchesForCharts(currentResearchGroupId, researchGroupIdCreator, operatorId, projectId, testTypeId, search, page);
                dynamic response = new ExpandoObject();
                response.results = resultList;
                response.pagination = new ExpandoObject();
                response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public string GetProjectsFromSharedRG(string search, int? page = null)
        //{
        //    var resp = new ResponseWrapper { status = "ok", response = null };
        //    var dateFormat = "dd/MM/yyyy"; // your datetime format
        //    var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

        //    var jsonSettings = new JsonSerializerSettings();
        //    jsonSettings.DateFormatString = dateFormat;

        //    var currentUser = UserHelper.GetCurrentUser();
        //    int researchGroupId = (int)currentUser.fkResearchGroup;
        //    try
        //    {
        //        List<ProjectExt> resultList = ProjectDa.GetProjectsByRG(search, researchGroupId, page);
        //        dynamic response = new ExpandoObject();
        //        response.results = resultList;
        //        response.pagination = new ExpandoObject();
        //        response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
        //        return JsonConvert.SerializeObject(response, jsonSettings);
        //    }
        //    catch (Exception e)
        //    {
        //        resp.status = "error";
        //        resp.message = e.Message;
        //        return JsonConvert.SerializeObject(resp, jsonSettings);
        //    }
        //}



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetTestGroupsByProject(string search, int? projectId = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;
            try
            {
                List<ProjectTestGroupExt> resultList = ProjectTestGroupDa.GetProjectsByName(search, researchGroupId, projectId);
                dynamic response = new ExpandoObject();
                response.results = resultList;
                response.pagination = new ExpandoObject();
                response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetAllTestGroupsList(int? testGroupId = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;
            jsonSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;

            var currentUser = UserHelper.GetCurrentUser();
            //if (researchGroupId == null)
            //{

            //}
            int researchGroupId = (int)currentUser.fkResearchGroup;

            try
            {
                List<TestGroupExt> testGroups = TestGroupDa.GetTestGroupListForResearchGroup(researchGroupId, testGroupId);
                return JsonConvert.SerializeObject(testGroups, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetAllProjectsList(int? projectId = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;
            jsonSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;

            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;
            try
            {
                List<ProjectExt> projects = ProjectDa.GetProjectsByRGParticipant(researchGroupId, projectId);
                return JsonConvert.SerializeObject(projects, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetAllProjectsTGList(int? projectTestGroupId = null, int? testGroupId = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;
            jsonSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;

            //var currentUser = UserHelper.GetCurrentUser();
            //if (researchGroupId == null)
            //{
            //    researchGroupId = currentUser.fkResearchGroup;
            //}

            try
            {
                List<ProjectTestGroupExt> projectsTG = ProjectTestGroupDa.GetAllTGProjects(projectTestGroupId, testGroupId);
                return JsonConvert.SerializeObject(projectsTG, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }


        //[WebMethod(EnableSession = true)]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public string SubmitProjectExperiment(string formData)
        //{
        //    var resp = new ResponseWrapper { status = "ok", response = null };
        //    var dateFormat = "dd/MM/yyyy"; // your datetime format
        //    var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

        //    var jsonSettings = new JsonSerializerSettings();
        //    jsonSettings.DateFormatString = dateFormat;

        //    var currentUser = UserHelper.GetCurrentUser();
        //    try
        //    {
        //        dynamic dynData = JsonConvert.DeserializeObject(formData);
        //        object submittedItem;
        //        //object returnElement;
        //        submittedItem = JsonConvert.DeserializeObject<ProjectExperiment>(formData, dateTimeConverter);
        //        ProjectExperiment submittedItemClass = (ProjectExperiment)submittedItem;

        //        int result = Batteries.Dal.ProjectExperimentDa.AddProjectExperiment(submittedItemClass);
        //        if (result == 0)
        //        {
        //            //NotifyHelper.Notify("Experiment added!", NotifyHelper.NotifyType.success, "");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resp.status = "error";
        //        resp.message = ex.Message;
        //        return JsonConvert.SerializeObject(resp, jsonSettings);
        //    }
        //    return JsonConvert.SerializeObject(resp, jsonSettings);
        //}


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SubmitProjectBatch(string formData)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                dynamic dynData = JsonConvert.DeserializeObject(formData);
                object submittedItem;
                //object returnElement;
                submittedItem = JsonConvert.DeserializeObject<ProjectBatch>(formData, dateTimeConverter);
                ProjectBatch submittedItemClass = (ProjectBatch)submittedItem;
                submittedItemClass.addedManually = true;
                submittedItemClass.fkUser = currentUser.userId;

                if (!ProjectBatchDa.BatchExistsInProject((int)submittedItemClass.fkBatch, (int)submittedItemClass.fkProject))
                {
                    int result = Batteries.Dal.ProjectBatchDa.AddProjectBatch(submittedItemClass, null);
                    if (result != 0)
                    {
                        throw new Exception("Error while adding batch to project!");
                    }
                }
                else
                {
                    throw new Exception("Batch is already added to the project!");
                }
            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
            return JsonConvert.SerializeObject(resp, jsonSettings);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string AddProjectParticipant(string formData)
        {

            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                int researchGroupId = (int)currentUser.fkResearchGroup;
                int userId = (int)currentUser.userId;
                dynamic dynData = JsonConvert.DeserializeObject(formData);
                object submittedItem;
                //object returnElement;
                submittedItem = JsonConvert.DeserializeObject<ProjectResearchGroup>(formData, dateTimeConverter);
                ProjectResearchGroup submittedItemClass = (ProjectResearchGroup)submittedItem;
                submittedItemClass.fkUser = currentUser.userId;

                int result = ProjectResearchGroupDa.AddProjectResearchGroup(submittedItemClass);
                if (result == 0)
                {
                    int? projectId = submittedItemClass.fkProject;
                    int result2 = ExperimentDa.UpdateExperimentStatus((int)projectId);
                    //NotifyHelper.Notify("Batch to Experiment added!", NotifyHelper.NotifyType.success, "");
                }
            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
            return JsonConvert.SerializeObject(resp, jsonSettings);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SubmitTestGroupExperiment(string formData)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                dynamic dynData = JsonConvert.DeserializeObject(formData);
                object submittedItem;
                submittedItem = JsonConvert.DeserializeObject<TestGroupExperiment>(formData, dateTimeConverter);
                TestGroupExperiment submittedItemClass = (TestGroupExperiment)submittedItem;
                submittedItemClass.fkUser = currentUser.userId;

                int result = Batteries.Dal.TestGroupExperimentDa.AddTestGroupExperiment(submittedItemClass);
                if (result == 0)
                {
                    //NotifyHelper.Notify("Experiment added!", NotifyHelper.NotifyType.success, "");
                }
            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
            return JsonConvert.SerializeObject(resp, jsonSettings);
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string UpdateStatusExperiment(string formData)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                int researchGroupId = (int)currentUser.fkResearchGroup;
                int userId = (int)currentUser.userId;
                dynamic dynData = JsonConvert.DeserializeObject(formData);
                object submittedItem;
                //object returnElement;
                submittedItem = JsonConvert.DeserializeObject<Experiment>(formData, dateTimeConverter);
                Experiment submittedItemClass = (Experiment)submittedItem;
                int result = Batteries.Dal.ExperimentDa.UpdateStatusOfExperimentAsPublic(submittedItemClass);
                if (result == 0)
                {
                    //NotifyHelper.Notify("Batch to Experiment added!", NotifyHelper.NotifyType.success, "");
                }
            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
            return JsonConvert.SerializeObject(resp, jsonSettings);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string DeleteTestGroupExperiment(int testGroupExperimentId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd.MM.yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            try
            {
                var result = TestGroupExperimentDa.DeleteTestGroupExperiment(testGroupExperimentId);
            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp);
            }
            return JsonConvert.SerializeObject(resp, jsonSettings);
        }

        //NOT USED
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string DeleteBatchFromProject(int projectBatchId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd.MM.yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            try
            {
                var result = ProjectBatchDa.DeleteProjectBatch(projectBatchId);
            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp);
            }
            return JsonConvert.SerializeObject(resp, jsonSettings);
        }

        //NOT USED
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string DeleteRGFromProject(int projectResearchGroupId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd.MM.yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            try
            {
                var result = ProjectResearchGroupDa.DeleteProjectResearchGroup(projectResearchGroupId);
            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp);
            }
            return JsonConvert.SerializeObject(resp, jsonSettings);
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string UpdateTestGroupExperiment(int testGroupExperimentId, string experimentHypothesis, string conclusion)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                //dynamic dynData = JsonConvert.DeserializeObject(formData);
                //object submittedItem;
                ////object returnElement;
                //submittedItem = JsonConvert.DeserializeObject<TestGroupExperiment>(formData, dateTimeConverter);
                //TestGroupExperiment submittedItemClass = (TestGroupExperiment)submittedItem;

                //int result = Batteries.Dal.TestGroupExperimentDa.AddTestGroupExperiment(submittedItemClass);
                int result = Batteries.Dal.TestGroupExperimentDa.UpdateTestGroupExperimentConclusion(testGroupExperimentId, experimentHypothesis, conclusion);
                if (result == 0)
                {
                    //NotifyHelper.Notify("Success!", NotifyHelper.NotifyType.success, "");
                }
            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
            return JsonConvert.SerializeObject(resp, jsonSettings);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SubmitBatchWithContent(string formData)
        {
            //formData = "{\"batch\":{\"label\":\"testbatch\",\"suggestedLabel\":\"\",\"description\":\"testdescr\"},\"batchContent\":[{\"fkBatch\":1,\"step\":1,\"fkStepMaterial\":1,\"fkStepBatch\":null,\"weight\":9.55,\"fkFunction\":null,\"fkStoredInType\":null,\"orderInStep\":1},{\"fkBatch\":1,\"step\":1,\"fkStepMaterial\":13,\"fkStepBatch\":null,\"weight\":6.3,\"fkFunction\":null,\"fkStoredInType\":null,\"orderInStep\":2}]}";
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                dynamic dynData = JsonConvert.DeserializeObject(formData);
                object submittedItem;
                //object returnElement;
                submittedItem = JsonConvert.DeserializeObject<AddBatchRequest>(formData, dateTimeConverter);
                //foreach (dynamic processJson in submittedItem.)
                //{

                //}
                //return submittedItem;
                //submittedItem.batch.fkUser = currentUser.userId;
                AddBatchRequest submittedItemClass = (AddBatchRequest)submittedItem;


                //var res = ValidationHelper.IsModelValidWithErrors(submittedItemClass);
                //if (res.Count != 0)
                //{
                //    throw new Exception(res[0].ErrorMessage);
                //}

                submittedItemClass.batchInfo.fkUser = currentUser.userId;
                int researchGroupId = (int)currentUser.fkResearchGroup;
                submittedItemClass.batchInfo.fkResearchGroup = (int)currentUser.fkResearchGroup;

                int? projectId = Batteries.Dal.ExperimentDa.GetAllExperimentsGeneralData(submittedItemClass.fkExperiment)[0].fkProject;
                //foreach(BatchContent batchContentItem in submittedItemClass.batchContent)
                //{
                //    batchContentItem.
                //}
                //int result = Batteries.Dal.BatchDa.AddBatchWithContentAndReturn(submittedItemClass, researchGroupId);
                var result = Batteries.Dal.BatchDa.AddBatchWithContentAndReturn(submittedItemClass, researchGroupId);
                if (result == null)
                {
                    throw new Exception("Error submitting batch");
                }

                int batchId = result.batchId;
                List<BatchExt> batchGeneralDataList = BatchDa.GetAllBatchesGeneralData(batchId);
                BatchExt batchGeneralData = new BatchExt();
                if (batchGeneralDataList != null)
                {
                    batchGeneralData = batchGeneralDataList[0];
                    //projectId = batchGeneralData.fkProject;
                }

                //ADD THIS BATCH TO THE PROJECT
                List<ProjectBatch> projectBatchList = new List<ProjectBatch>();
                ProjectBatch projectBatch = new ProjectBatch();
                projectBatch.fkBatch = batchId;
                projectBatch.fkProject = projectId;
                projectBatch.fkUser = currentUser.userId;
              
                projectBatchList.Add(projectBatch);
                //ADD ALL OTHER BATCHES INSIDE THIS BATCH TO THE PROJECT
                List<int> batchIdList = GeneralHelper.GetBatchIdsInsideBatch(batchId);

                foreach (int btcId in batchIdList)
                {
                    ProjectBatch projectBatch1 = new ProjectBatch();
                    projectBatch1.fkBatch = btcId;
                    projectBatch1.fkComingBatch = batchId;
                    projectBatch1.fkProject = projectId;
                    projectBatch1.fkUser = currentUser.userId;
                    projectBatchList.Add(projectBatch1);
                }

                ProjectBatchDa.AddProjectBatchList(projectBatchList);


                if (result != null)
                {
                    //batch info in response
                    resp.response = result;
                    //NotifyHelper.Notify("Batch successfully created", NotifyHelper.NotifyType.success, "");
                }
                //if(result == 0)
                //{
                //    throw new Exception("Not enough in stock");
                //}

                //returnElement = Batteries.Dal.BatchDa.GetAllBatchesWithContent(((Batch)submittedItem).batchId);

            }
            //catch (ValidationException ve)
            //{
            //    //do whatever
            //    //throw new Exception(ve.Message);
            //    resp.status = "error";
            //    resp.message = ve.Message;
            //    return JsonConvert.SerializeObject(resp, jsonSettings);
            //}
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }

            return JsonConvert.SerializeObject(resp, jsonSettings);
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SubmitProcessSequence(string formData)
        {
            //formData = "{\"batch\":{\"label\":\"testbatch\",\"suggestedLabel\":\"\",\"description\":\"testdescr\"},\"batchContent\":[{\"fkBatch\":1,\"step\":1,\"fkStepMaterial\":1,\"fkStepBatch\":null,\"weight\":9.55,\"fkFunction\":null,\"fkStoredInType\":null,\"orderInStep\":1},{\"fkBatch\":1,\"step\":1,\"fkStepMaterial\":13,\"fkStepBatch\":null,\"weight\":6.3,\"fkFunction\":null,\"fkStoredInType\":null,\"orderInStep\":2}]}";
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                dynamic dynData = JsonConvert.DeserializeObject(formData);
                object submittedItem;
                submittedItem = JsonConvert.DeserializeObject<AddSequenceRequest>(formData, dateTimeConverter);

                AddSequenceRequest submittedItemClass = (AddSequenceRequest)submittedItem;

                submittedItemClass.sequenceInfo.fkUser = currentUser.userId;
                submittedItemClass.sequenceInfo.fkResearchGroup = (int)currentUser.fkResearchGroup;


                var result = Batteries.Dal.ProcessSequenceDa.SubmitProcessSequence(submittedItemClass);
                if (result != null)
                {
                    //batch info in response
                    resp.response = result;

                }

            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }

            return JsonConvert.SerializeObject(resp, jsonSettings);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string UpdateBatchGeneralData(string formData, int batchId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                dynamic dynData = JsonConvert.DeserializeObject(formData);
                object submittedItem;
                submittedItem = JsonConvert.DeserializeObject<AddBatchRequest>(formData, dateTimeConverter);

                //submittedItem.batch.fkUser = currentUser.userId;
                AddBatchRequest submittedItemClass = (AddBatchRequest)submittedItem;

                submittedItemClass.batchInfo.fkUser = currentUser.userId;
                int researchGroupId = (int)currentUser.fkResearchGroup;
                var result = Batteries.Dal.BatchDa.UpdateBatchGeneralData(batchId, submittedItemClass, researchGroupId);

                if (result != 0)
                {
                    throw new Exception("Error updating batch general information");
                }

            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
            string datetime = DateTime.Now.ToString("dd/MM/yyyy 'at' HH:mm:ss");
            resp.message = datetime;
            return JsonConvert.SerializeObject(resp, jsonSettings);
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string UpdateBatchGeneralDataAndFinish(string formData, int batchId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                dynamic dynData = JsonConvert.DeserializeObject(formData);
                object submittedItem;
                submittedItem = JsonConvert.DeserializeObject<AddBatchRequest>(formData, dateTimeConverter);

                //submittedItem.batch.fkUser = currentUser.userId;
                AddBatchRequest submittedItemClass = (AddBatchRequest)submittedItem;

                int userId = (int)currentUser.userId;
                submittedItemClass.batchInfo.fkUser = userId;
                int researchGroupId = (int)currentUser.fkResearchGroup;

                var result = Batteries.Dal.BatchDa.UpdateBatchGeneralDataAndSetComplete(batchId, submittedItemClass, researchGroupId, userId);
                if (result != 0)
                {
                    throw new Exception("Error updating batch general information");
                }
                else
                {
                    NotifyHelper.Notify("Batch general information updated!", NotifyHelper.NotifyType.info, "");
                }

            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
            string datetime = DateTime.Now.ToString("dd/MM/yyyy 'at' HH:mm:ss");
            resp.message = datetime;
            return JsonConvert.SerializeObject(resp, jsonSettings);
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string UpdateBatchContent(string formData, int batchId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                dynamic dynData = JsonConvert.DeserializeObject(formData);
                object submittedItem;
                submittedItem = JsonConvert.DeserializeObject<AddBatchRequest>(formData, dateTimeConverter);

                AddBatchRequest submittedItemClass = (AddBatchRequest)submittedItem;

                submittedItemClass.batchInfo.fkUser = currentUser.userId;
                int researchGroupId = (int)currentUser.fkResearchGroup;
                var result = Batteries.Dal.BatchDa.UpdateBatchContent(batchId, submittedItemClass, researchGroupId);
                //if (result != null)
                //{
                //    resp.response = result;
                //    //NotifyHelper.Notify("Batch successfully created", NotifyHelper.NotifyType.success, "");
                //}
                if (result != 0)
                {
                    throw new Exception("Error updating batch content");
                }

            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
            string datetime = DateTime.Now.ToString("dd/MM/yyyy 'at' HH:mm:ss");
            resp.message = datetime;
            return JsonConvert.SerializeObject(resp, jsonSettings);
        }

        //[WebMethod(EnableSession = true)]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public string SubmitBatchEditWithContent(string formData, int batchId)
        //{
        //    var resp = new ResponseWrapper { status = "ok", response = null };
        //    var dateFormat = "dd/MM/yyyy"; // your datetime format
        //    var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

        //    var jsonSettings = new JsonSerializerSettings();
        //    jsonSettings.DateFormatString = dateFormat;

        //    var currentUser = UserHelper.GetCurrentUser();
        //    try
        //    {
        //        dynamic dynData = JsonConvert.DeserializeObject(formData);
        //        object submittedItem;
        //        submittedItem = JsonConvert.DeserializeObject<AddBatchRequest>(formData, dateTimeConverter);
        //        AddBatchRequest submittedItemClass = (AddBatchRequest)submittedItem;
        //        submittedItemClass.batchInfo.fkUser = currentUser.userId;
        //        int researchGroupId = (int)currentUser.fkResearchGroup;
        //        var result = Batteries.Dal.BatchDa.UpdateBatchWithContentAndReturn(submittedItemClass, batchId, researchGroupId);
        //        if (result != null)
        //        {
        //            //batch info in response
        //            resp.response = result;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        resp.status = "error";
        //        resp.message = ex.Message;
        //        return JsonConvert.SerializeObject(resp, jsonSettings);
        //    }
        //    return JsonConvert.SerializeObject(resp, jsonSettings);
        //}
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string FinishBatchCreation(string formData, int batchId, bool editing)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                dynamic dynData = JsonConvert.DeserializeObject(formData);
                object submittedItem;
                submittedItem = JsonConvert.DeserializeObject<AddBatchRequest>(formData, dateTimeConverter);
                AddBatchRequest submittedItemClass = (AddBatchRequest)submittedItem;
                submittedItemClass.batchInfo.fkUser = currentUser.userId;
                int researchGroupId = (int)currentUser.fkResearchGroup;
                int userId = (int)currentUser.userId;

                var result = Batteries.Dal.BatchDa.UpdateBatchWithContentAndReturn(submittedItemClass, batchId, researchGroupId, userId, editing);

                if (result != 0)
                {
                    throw new Exception("Error submitting batch");
                }

                int? projectId = null;

                List<BatchExt> batchGeneralDataList = BatchDa.GetAllBatchesGeneralData(batchId);
                BatchExt batchGeneralData = new BatchExt();
                if (batchGeneralDataList != null)
                {
                    batchGeneralData = batchGeneralDataList[0];
                    projectId = batchGeneralData.fkProject;
                }

                //ADD THIS BATCH TO THE PROJECT
                List<ProjectBatch> projectBatchList = new List<ProjectBatch>();
                ProjectBatch projectBatch = new ProjectBatch();
                projectBatch.fkBatch = batchId;
                projectBatch.fkProject = projectId;
                projectBatch.fkUser = currentUser.userId;

                //don't add the same batch if we are editing it
                if (!ProjectBatchDa.BatchExistsInProject(batchId, (int)projectId))
                    projectBatchList.Add(projectBatch);
                //ADD ALL OTHER BATCHES INSIDE THIS BATCH TO THE PROJECT
                List<int> batchIdList = GeneralHelper.GetBatchIdsInsideBatch(batchId);

                foreach (int btcId in batchIdList)
                {
                    ProjectBatch projectBatch1 = new ProjectBatch();
                    projectBatch1.fkBatch = btcId;
                    projectBatch1.fkComingBatch = batchId;
                    projectBatch1.fkProject = projectId;
                    projectBatch1.fkUser = currentUser.userId;
                    projectBatchList.Add(projectBatch1);
                }

                ProjectBatchDa.AddProjectBatchList(projectBatchList);

                NotifyHelper.Notify("Batch saved", NotifyHelper.NotifyType.success, "");
            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
            return JsonConvert.SerializeObject(resp, jsonSettings);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string DiscardBatch(int batchId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                int researchGroupId = (int)currentUser.fkResearchGroup;
                 var result = Batteries.Dal.BatchDa.DiscardBatch(batchId, researchGroupId);
                if (result != 0)
                {
                    throw new Exception("So error occured");
                }
                {
                    NotifyHelper.Notify("Batch removed", NotifyHelper.NotifyType.info, "");
                }

            }
            catch (Exception ex)
            {
                //NotifyHelper.Notify(ex.Message, NotifyHelper.NotifyType.info, "");
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
            return JsonConvert.SerializeObject(resp, jsonSettings);
        }


        //EXPERIMENT METHODS
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string UpdateExperimentGeneralData(string formData, int experimentId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                dynamic dynData = JsonConvert.DeserializeObject(formData);
                object submittedItem;
                submittedItem = JsonConvert.DeserializeObject<AddExperimentRequest>(formData, dateTimeConverter);

                //submittedItem.batch.fkUser = currentUser.userId;
                AddExperimentRequest submittedItemClass = (AddExperimentRequest)submittedItem;

                submittedItemClass.experimentInfo.fkUser = currentUser.userId;
                int researchGroupId = (int)currentUser.fkResearchGroup;
                var result = Batteries.Dal.ExperimentDa.UpdateExperimentGeneralData(experimentId, submittedItemClass, researchGroupId);

                if (result != 0)
                {
                    throw new Exception("Error updating experiment general information");
                }

            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
            string datetime = DateTime.Now.ToString("dd/MM/yyyy 'at' HH:mm:ss");
            resp.message = datetime;
            return JsonConvert.SerializeObject(resp, jsonSettings);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string UpdateExperimentGeneralDataAndFinish(string formData, int experimentId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            int userId = (int)currentUser.userId;
            int researchGroupId = (int)currentUser.fkResearchGroup;

            try
            {
                dynamic dynData = JsonConvert.DeserializeObject(formData);
                object submittedItem;
                submittedItem = JsonConvert.DeserializeObject<AddExperimentRequest>(formData, dateTimeConverter);

                //submittedItem.batch.fkUser = currentUser.userId;
                AddExperimentRequest submittedItemClass = (AddExperimentRequest)submittedItem;

                submittedItemClass.experimentInfo.fkUser = userId;

                var result = Batteries.Dal.ExperimentDa.UpdateExperimentGeneralDataAndSetComplete(experimentId, submittedItemClass, researchGroupId, userId);

                if (result != 0)
                {
                    throw new Exception("Error updating experiment general information");
                }
                else
                {
                    NotifyHelper.Notify("Experiment general information updated!", NotifyHelper.NotifyType.info, "");
                }

            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
            string datetime = DateTime.Now.ToString("dd/MM/yyyy 'at' HH:mm:ss");
            resp.message = datetime;
            return JsonConvert.SerializeObject(resp, jsonSettings);
        }



        //Submit component method
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SubmitExperimentComponent(string formData, int experimentId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;
            //jsonSettings.Culture = CultureInfo.InvariantCulture;
            string componentType;
            //int experimentId;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                dynamic dynData = JsonConvert.DeserializeObject(formData);


                //experimentId = dynData.experimentId;
                componentType = dynData.componentType;

                //int componentTypeId = 0;
                //switch (componentType)
                //{
                //    case "Anode":
                //        componentTypeId = 1;
                //        break;
                //    case "Cathode":
                //        componentTypeId = 2;
                //        break;
                //    case "Separator":
                //        componentTypeId = 3;
                //        break;
                //    case "Electrolyte":
                //        componentTypeId = 4;
                //        break;
                //    case "ReferenceElectrode":
                //        componentTypeId = 5;
                //        break;
                //    case "Casing":
                //        componentTypeId = 6;
                //        break;

                //    default:
                //        throw new Exception("Invalid component type");
                //}

                AddBatteryComponentRequest addBatteryComponentRequest = new AddBatteryComponentRequest();

                List<AddBatteryComponentStepRequest> addBatteryComponentStepRequestList = new List<AddBatteryComponentStepRequest>();

                foreach (object step in dynData.componentSteps)
                {
                    object submittedComponentItem = JsonConvert.DeserializeObject<AddBatteryComponentStepRequest>(step.ToString(), dateTimeConverter);
                    AddBatteryComponentStepRequest submittedItemComponentClass = (AddBatteryComponentStepRequest)submittedComponentItem;
                    //submittedItemComponentClass.stepNumber = (int)step;

                    var res = ValidationHelper.IsModelValidWithErrors(submittedItemComponentClass);
                    if (res.Count != 0)
                    {
                        throw new Exception(res[0].ErrorMessage);
                    }

                    addBatteryComponentStepRequestList.Add(submittedItemComponentClass);

                    //foreach (object stepContent in step.stepContent)
                    //{
                    //    submittedStepContentItem = JsonConvert.DeserializeObject<BatteryComponentExt>(stepContent.ToString(), dateTimeConverter);
                    //    BatteryComponentExt submittedStepContentItemClass = (BatteryComponentExt)submittedStepContentItem;
                    //switch (componentType)
                    //{
                    //    case "Anode":
                    //        submittedStepContentItemClass.fkBatteryComponentType = 1;
                    //        break;
                    //    case "Cathode":
                    //        submittedStepContentItemClass.fkBatteryComponentType = 2;
                    //        break;
                    //    case "Separator":
                    //        submittedStepContentItemClass.fkBatteryComponentType = 3;
                    //        break;
                    //    case "Electrolyte":
                    //        submittedStepContentItemClass.fkBatteryComponentType = 4;
                    //        break;
                    //    case "ReferenceElectrode":
                    //        submittedStepContentItemClass.fkBatteryComponentType = 5;
                    //        break;
                    //    case "Casing":
                    //        submittedStepContentItemClass.fkBatteryComponentType = 6;
                    //        break;

                    //    default:
                    //        throw new Exception("Process type not defined");
                    //}
                    //    batteryComponentsList.Add(submittedStepContentItemClass);
                    //}

                    //foreach (object stepProcess in step.stepProcesses)
                    //{
                    //    submittedStepProcessItem = JsonConvert.DeserializeObject<ExperimentProcessExt>(stepProcess.ToString(), dateTimeConverter);
                    //    ExperimentProcessExt submittedStepProcessItemClass = (ExperimentProcessExt)submittedStepProcessItem;

                    //}

                }
                addBatteryComponentRequest.componentStepsContentList = addBatteryComponentStepRequestList;
                addBatteryComponentRequest.componentType = componentType;
                addBatteryComponentRequest.userId = currentUser.userId;
                addBatteryComponentRequest.experimentId = experimentId;
                addBatteryComponentRequest.componentEmpty = (bool)dynData.componentEmpty;
                //addBatteryComponentRequest.operatorId = (int)currentUser.userId;
                //submittedItemClass.batch.fkUser = currentUser.userId;

                //if ((object)dynData.measurements != null)

                object submittedMeasurementsItem = JsonConvert.DeserializeObject<MeasurementsExt>(dynData.measurements.ToString(), dateTimeConverter);
                MeasurementsExt submittedMeasurementsItemClass = (MeasurementsExt)submittedMeasurementsItem;
                addBatteryComponentRequest.measurements = submittedMeasurementsItemClass;

                int researchGroupId = (int)currentUser.fkResearchGroup;
                int result = Batteries.Dal.ExperimentDa.AddBatteryComponent(experimentId, addBatteryComponentRequest, researchGroupId);

            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
            string datetime = DateTime.Now.ToString("dd/MM/yyyy 'at' HH:mm:ss");
            resp.message = datetime;
            return JsonConvert.SerializeObject(resp, jsonSettings);
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SubmitExperimentComponentCommercialType(string formData)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;
            string componentType;
            int experimentId;
            int? commercialTypeId;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                dynamic dynData = JsonConvert.DeserializeObject(formData);
                componentType = dynData.componentType;
                int componentTypeId = 0;
                experimentId = dynData.experimentId;
                commercialTypeId = dynData.commercialTypeId;

                switch (componentType)
                {
                    case "Anode":
                        componentTypeId = 1;
                        break;
                    case "Cathode":
                        componentTypeId = 2;
                        break;
                    case "Separator":
                        componentTypeId = 3;
                        break;
                    case "Electrolyte":
                        componentTypeId = 4;
                        break;
                    case "ReferenceElectrode":
                        componentTypeId = 5;
                        break;
                    case "Casing":
                        componentTypeId = 6;
                        break;

                    default:
                        throw new Exception("Invalid component type");
                }


                int researchGroupId = (int)currentUser.fkResearchGroup;
                int result = Batteries.Dal.ExperimentDa.AddBatteryComponentCommercialType(experimentId, componentTypeId, commercialTypeId, researchGroupId);
            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
            string datetime = DateTime.Now.ToString("dd/MM/yyyy 'at' HH:mm:ss");
            resp.message = datetime;
            return JsonConvert.SerializeObject(resp, jsonSettings);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string FinishExperimentCreation(string formData, int experimentId, bool editing)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            int userId = (int)currentUser.userId;
            int researchGroupId = (int)currentUser.fkResearchGroup;

            string componentType;
            try
            {
                //int researchGroupId = (int)currentUser.fkResearchGroup;
                List<int> componentTypeIdList = ExperimentDa.GetAllExperimentComponentsCompletedIds(experimentId);
                List<int> invalidComponentTypeIdList = new List<int>();
                List<string> invalidComponentTypeList = new List<string>();

                if (!componentTypeIdList.Contains(1))
                {
                    invalidComponentTypeList.Add("Anode");
                }
                if (!componentTypeIdList.Contains(2))
                {
                    invalidComponentTypeList.Add("Cathode");
                }

                if (!componentTypeIdList.Contains(4))
                {
                    invalidComponentTypeList.Add("Electrolyte");
                }
                if (!componentTypeIdList.Contains(6))
                {
                    invalidComponentTypeList.Add("Casing");
                }
                if (invalidComponentTypeList.Count != 0)
                {
                    object incompleteComponents = new object();
                    incompleteComponents = invalidComponentTypeList;


                    resp.status = "incompleteComponents";
                    resp.message = "Please complete components marked in red.";
                    resp.response = invalidComponentTypeList;
                    return JsonConvert.SerializeObject(resp, jsonSettings);
                }

                //all good, INSERT ALL STOCK DATA AND FINISH EXPERIMENT
                int? projectId = null;

                List<ExperimentExt> experimentGeneralDataList = ExperimentDa.GetAllExperimentsGeneralData(experimentId);
                ExperimentExt experimentGeneralData = new ExperimentExt();
                if (experimentGeneralDataList != null)
                {
                    experimentGeneralData = experimentGeneralDataList[0];

                    projectId = experimentGeneralData.fkProject;
                }

                dynamic dynData = JsonConvert.DeserializeObject(formData);

                object submittedExperimentInfoItem = JsonConvert.DeserializeObject<ExperimentExt>(dynData.experimentInfo.ToString(), dateTimeConverter);
                ExperimentExt experimentInfo = (ExperimentExt)submittedExperimentInfoItem;

                List<AddBatteryComponentRequest> addBatteryComponentRequestList = new List<AddBatteryComponentRequest>();

                foreach (dynamic component in dynData.batteryComponents)
                {
                    componentType = component.componentType;
                    int componentTypeId = 0;

                    switch (componentType)
                    {
                        case "Anode":
                            componentTypeId = 1;
                            break;
                        case "Cathode":
                            componentTypeId = 2;
                            break;
                        case "Separator":
                            componentTypeId = 3;
                            break;
                        case "Electrolyte":
                            componentTypeId = 4;
                            break;
                        case "ReferenceElectrode":
                            componentTypeId = 5;
                            break;
                        case "Casing":
                            componentTypeId = 6;
                            break;

                        default:
                            throw new Exception("Invalid component type");
                    }

                    AddBatteryComponentRequest addBatteryComponentRequest = new AddBatteryComponentRequest();
                    List<AddBatteryComponentStepRequest> addBatteryComponentStepRequestList = new List<AddBatteryComponentStepRequest>();

                    foreach (object step in component.componentSteps)
                    {
                        object submittedComponentItem = JsonConvert.DeserializeObject<AddBatteryComponentStepRequest>(step.ToString(), dateTimeConverter);
                        AddBatteryComponentStepRequest submittedItemComponentClass = (AddBatteryComponentStepRequest)submittedComponentItem;

                        var res = ValidationHelper.IsModelValidWithErrors(submittedItemComponentClass);
                        if (res.Count != 0)
                        {
                            throw new Exception(res[0].ErrorMessage);
                        }

                        addBatteryComponentStepRequestList.Add(submittedItemComponentClass);
                    }
                    addBatteryComponentRequest.componentStepsContentList = addBatteryComponentStepRequestList;
                    addBatteryComponentRequest.componentType = componentType;
                    addBatteryComponentRequest.userId = currentUser.userId;
                    addBatteryComponentRequest.experimentId = experimentId;

                    addBatteryComponentRequestList.Add(addBatteryComponentRequest);
                    //int result = Batteries.Dal.BatteryComponentDa.AddBatteryComponent(addBatteryComponentRequest, researchGroupId);
                }

                AddExperimentRequest addExperimentRequest = new AddExperimentRequest();
                addExperimentRequest.experimentInfo = experimentInfo;
                addExperimentRequest.batteryComponents = addBatteryComponentRequestList;

                var result = Batteries.Dal.ExperimentDa.FinishExperimentCreation(addExperimentRequest, experimentId, researchGroupId, userId, editing);
                if (result != 0)
                {
                    throw new Exception("Error submitting experiment");
                }

                //INSERT EXPERIMENT SUMMARY
                var summaryRes = Batteries.Dal.ExperimentDa.InsertExperimentSummary(experimentId, researchGroupId, userId);
                if (summaryRes != 0)
                {
                    throw new Exception("Error saving experiment calculations");
                }

                //SET EXPERIMENT STATUS
                List<ProjectResearchGroupExt> projectRgGeneralList = ProjectResearchGroupDa.GetAllProjectResearchGroups((int)projectId);
                if (projectRgGeneralList.Count <= 1)
                {
                    ExperimentDa.SetStatusOfExperimentPrivate(experimentId);
                }
                else
                {
                    ExperimentDa.SetStatusOfExperimentShared(experimentId);
                }

                //ADD ALL BATCHES INSIDE THE EXPERIMENT TO THE PROJECT
                List<int> batchIdList = GeneralHelper.GetBatchIdsInsideExperiment(experimentId);

                List<ProjectBatch> projectBatchList = new List<ProjectBatch>();

                foreach (int batchId in batchIdList)
                {
                    ProjectBatch projectBatch = new ProjectBatch();
                    projectBatch.fkBatch = batchId;
                    projectBatch.fkComingExperiment = experimentId;
                    projectBatch.fkProject = projectId;
                    projectBatch.fkUser = currentUser.userId;
                    projectBatchList.Add(projectBatch);
                }
                ProjectBatchDa.AddProjectBatchList(projectBatchList);

                NotifyHelper.Notify("Experiment saved", NotifyHelper.NotifyType.success, "");
            }
            catch (Exception ex)
            {
                //UNMARK EXPERIMENT COMPLETE
                var unmarkResult = ExperimentDa.UnMarkExperimentComplete(experimentId);

                if (ex.Data.Count > 0)
                {
                    int faultyComponentId = (int)ex.Data["faultyComponentId"];
                    if (faultyComponentId != 0)
                    {
                        resp.status = "invalidStock";

                        List<string> invalidComponentTypeList = new List<string>();
                        if (faultyComponentId == 1)
                        {
                            invalidComponentTypeList.Add("Anode");
                        }
                        if (faultyComponentId == 2)
                        {
                            invalidComponentTypeList.Add("Cathode");
                        }
                        if (faultyComponentId == 3)
                        {
                            invalidComponentTypeList.Add("Separator");
                        }
                        if (faultyComponentId == 4)
                        {
                            invalidComponentTypeList.Add("Electrolyte");
                        }
                        if (faultyComponentId == 5)
                        {
                            invalidComponentTypeList.Add("ReferenceElectrode");
                        }
                        if (faultyComponentId == 6)
                        {
                            invalidComponentTypeList.Add("Casing");
                        }
                        resp.response = invalidComponentTypeList;
                    }
                }

                else
                {
                    resp.status = "error";
                }
                resp.message = ex.Message;

            }

            return JsonConvert.SerializeObject(resp, jsonSettings);
        }

        //za dorabotka
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetBillOfMaterials(int experimentId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            object empty = new Object();
            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;

            try
            {
                //var result = BatteryComponentDa.GetAllMaterialsInExperiment(experimentId);
                var result = "";
                return JsonConvert.SerializeObject(result, jsonSettings);

                //presmetaj i vrakaj json kao string so trite vrednosti
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        //NOT USED, old
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetMaterialDataForCsv(int? experimentId = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            object empty = new Object();
            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;

            try
            {
                var result = BatteryComponentDa.GetAllMaterialsInExperimentForCsv();

                //Object resultObject = new Object();
                string resultString = "Experiment Id,Material,Weight,Measurement Unit,Time,Width,Length,Conductivity,Thickness,Weight";
                resultString += System.Environment.NewLine;
                foreach (BatteryComponentExt o in result)
                {
                    string id = "";
                    string material = "";
                    string weight = "";
                    string measurementUnit = "";
                    string time = "";
                    string width = "";
                    string length = "";
                    string conductivity = "";
                    string thickness = "";
                    string MWeight = "";

                    id = o.fkExperiment.ToString();
                    weight = o.weight.ToString();
                    measurementUnit = o.measurementUnitSymbol.ToString();
                    if (o.measurements != null)
                    {
                        time = o.measurements.measuredTime.ToString();
                        width = o.measurements.measuredWidth.ToString();
                        length = o.measurements.measuredLength.ToString();
                        conductivity = o.measurements.measuredConductivity.ToString();
                        thickness = o.measurements.measuredThickness.ToString();
                        MWeight = o.measurements.measuredWeight.ToString();
                    }

                    if (o.fkStepMaterial != null)
                    {
                        material = o.materialName + ",";
                    }
                    else
                    {
                        material = o.batchSystemLabel + ",";
                    }

                    resultString += id + "," + material + "," + weight + "," + measurementUnit + "," + time + "," + width + "," + length + "," + conductivity + "," + thickness + "," + MWeight;
                    resultString += System.Environment.NewLine;

                }


                var json = JsonConvert.SerializeObject(result, jsonSettings);
                return JsonConvert.SerializeObject(result, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetBatchWithContent(int batchId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            object empty = new Object();

            try
            {
                BatchResponse batch = GeneralHelper.GetBatchWithContent(batchId);


                if (batch != null)
                {
                    return JsonConvert.SerializeObject(batch, jsonSettings);
                }
                else return "";
                //return JsonConvert.SerializeObject(batch, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetBatchInProgressWithContent(int batchId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            object empty = new Object();

            try
            {
                BatchResponse batch = GeneralHelper.GetBatchInProgressWithContent(batchId);


                //if (batch != null)
                //{
                //    return JsonConvert.SerializeObject(batch, jsonSettings);
                //}
                //else return JsonConvert.SerializeObject(empty, jsonSettings);
                return JsonConvert.SerializeObject(batch, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetBatchById(int batchId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                BatchExt batchGeneralData = BatchDa.GetBatchById(batchId, (int)currentUser.fkResearchGroup);
                return JsonConvert.SerializeObject(batchGeneralData, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetLastUsedProject()
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                Project projectGeneralData = ProjectDa.GetLastUsedProject( (int)currentUser.userId);
                return JsonConvert.SerializeObject(projectGeneralData, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetLastUsedTestGroup()
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                TestGroup testGroup = TestGroupDa.GetLastUsedTestGroup((int)currentUser.userId);
                return JsonConvert.SerializeObject(testGroup, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetBatchesByIdForCharts(int[] batchIdArray = null, int? researchGroupId = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                List<BatchExt> list = BatchDa.GetBatchesByIdForCharts((int)currentUser.fkResearchGroup, batchIdArray);
                return JsonConvert.SerializeObject(list, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetMaterialById(int materialId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy HH:mm"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                MaterialExt materialGeneralData = MaterialDa.GetAllMaterialsGeneralData(materialId)[0];
                return JsonConvert.SerializeObject(materialGeneralData, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetAllPreviousExperimentComponents(string search, int experimentId, int componentTypeId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            object empty = new Object();
            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;

            try
            {
                string results = ExperimentDa.GetAllFinishedBatteryComponentsListJsonForDropdown(researchGroupId, experimentId, componentTypeId);

                //string a = JsonConvert.SerializeObject(results, jsonSettings);
                //return JsonConvert.SerializeObject(new Object(), jsonSettings);
                return results;


                //ExperimentResponse experiment = new ExperimentResponse();
                //List<BatteryComponentResponse> batteryComponentResponseList = new List<BatteryComponentResponse>();
                //ExperimentExt experimentGeneralInfo = ExperimentDa.GetAllExperimentsGeneralData(experimentId)[0];
                //experiment.experimentInfo = experimentGeneralInfo;

                //List<int> componentTypeIds = ExperimentDa.GetAllExperimentComponentsCompletedIds(experimentId);
                //foreach (int componentTypeId in componentTypeIds)
                //{
                //    BatteryComponentResponse batteryComponentResponse = GeneralHelper.GetExperimentComponentWithContent(experimentId, componentTypeId);
                //    batteryComponentResponseList.Add(batteryComponentResponse);
                //}
                //experiment.batteryComponents = batteryComponentResponseList;

                //BatteryComponentResponse batteryComponentResponse = GeneralHelper.GetExperimentComponentWithContent(experimentId, componentTypeId);

                //return JsonConvert.SerializeObject(batteryComponentResponse, jsonSettings);

            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }

        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetExperimentComponent(int experimentId, int componentTypeId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            object empty = new Object();
            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;

            try
            {
                ExperimentResponse experiment = new ExperimentResponse();
                List<BatteryComponentResponse> batteryComponentResponseList = new List<BatteryComponentResponse>();
                ExperimentExt experimentGeneralInfo = ExperimentDa.GetAllExperimentsGeneralData(experimentId)[0];
                experiment.experimentInfo = experimentGeneralInfo;

                BatteryComponentResponse batteryComponentResponse = GeneralHelper.GetExperimentComponentWithContent(experimentId, componentTypeId);

                return JsonConvert.SerializeObject(batteryComponentResponse, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }

        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetExperimentWithContent(int experimentId)
        {
            //FOR COMPLETED EXPERIMENTS
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            object empty = new Object();
            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;
            try
            {
                ExperimentResponse experiment = new ExperimentResponse();
                List<BatteryComponentResponse> batteryComponentResponseList = new List<BatteryComponentResponse>();

                List<ExperimentExt> experimentGeneralDataList = ExperimentDa.GetAllCompleteExperimentsGeneralData(experimentId);
                ExperimentExt experimentGeneralData = new ExperimentExt();
                if (experimentGeneralDataList != null)
                {
                    experimentGeneralData = experimentGeneralDataList[0];
                    experiment.experimentInfo = experimentGeneralData;
                }
                else
                {
                    return "";
                }


                List<int> componentTypeIds = ExperimentDa.GetAllExperimentComponentsCompletedIds(experimentId);
                foreach (int componentTypeId in componentTypeIds)
                {
                    BatteryComponentResponse batteryComponentResponse = GeneralHelper.GetExperimentComponentWithContent(experimentId, componentTypeId);
                    batteryComponentResponseList.Add(batteryComponentResponse);
                }
                experiment.batteryComponents = batteryComponentResponseList;
                //BatchExt batch = BatchDa.GetBatchWithContent(batchId, researchGroupId);
                //if (batch != null)
                //{
                //    return JsonConvert.SerializeObject(batch, jsonSettings);
                //}
                //else return JsonConvert.SerializeObject(empty, jsonSettings);
                //return "";
                // string a = JsonConvert.SerializeObject(experiment, jsonSettings);
                return JsonConvert.SerializeObject(experiment, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetExperimentWithContentsPublic(int experimentId)
        {
            //FOR COMPLETED EXPERIMENTS
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            object empty = new Object();
            //var currentUser = UserHelper.GetCurrentUser();
            //int researchGroupId = (int)currentUser.fkResearchGroup;
            try
            {
                ExperimentResponse experiment = new ExperimentResponse();
                List<BatteryComponentResponse> batteryComponentResponseList = new List<BatteryComponentResponse>();

                List<ExperimentExt> experimentGeneralDataList = ExperimentDa.GetAllCompleteExperimentsGeneralData(experimentId);
                ExperimentExt experimentGeneralData = new ExperimentExt();
                if (experimentGeneralDataList != null)
                {
                    experimentGeneralData = experimentGeneralDataList[0];
                    experiment.experimentInfo = experimentGeneralData;
                }
                else
                {
                    return "";
                }


                List<int> componentTypeIds = ExperimentDa.GetAllExperimentComponentsCompletedIds(experimentId);
                foreach (int componentTypeId in componentTypeIds)
                {
                    BatteryComponentResponse batteryComponentResponse = GeneralHelper.GetExperimentComponentWithContent(experimentId, componentTypeId);
                    batteryComponentResponseList.Add(batteryComponentResponse);
                }
                experiment.batteryComponents = batteryComponentResponseList;
                //BatchExt batch = BatchDa.GetBatchWithContent(batchId, researchGroupId);
                //if (batch != null)
                //{
                //    return JsonConvert.SerializeObject(batch, jsonSettings);
                //}
                //else return JsonConvert.SerializeObject(empty, jsonSettings);
                //return "";
                // string a = JsonConvert.SerializeObject(experiment, jsonSettings);
                return JsonConvert.SerializeObject(experiment, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetExperimentInProgressWithContent(int experimentId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            object empty = new Object();
            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;
            try
            {
                ExperimentResponse experiment = new ExperimentResponse();
                List<BatteryComponentResponse> batteryComponentResponseList = new List<BatteryComponentResponse>();
                ExperimentExt experimentGeneralInfo = ExperimentDa.GetAllExperimentsGeneralData(experimentId)[0];
                experiment.experimentInfo = experimentGeneralInfo;

                List<int> componentTypeIds = ExperimentDa.GetAllExperimentComponentsCompletedIds(experimentId);
                foreach (int componentTypeId in componentTypeIds)
                {
                    BatteryComponentResponse batteryComponentResponse = GeneralHelper.GetExperimentComponentWithContent(experimentId, componentTypeId);
                    batteryComponentResponseList.Add(batteryComponentResponse);
                }
                experiment.batteryComponents = batteryComponentResponseList;

                //Checking if null is done previously on page
                return JsonConvert.SerializeObject(experiment, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetBatteryComponentCommercialTypes(string search, string componentType)
        {
            int componentTypeId = 0;
            switch (componentType)
            {
                case "Anode":
                    componentTypeId = 1;
                    break;
                case "Cathode":
                    componentTypeId = 2;
                    break;
                case "Separator":
                    componentTypeId = 3;
                    break;
                case "Electrolyte":
                    componentTypeId = 4;
                    break;
                case "ReferenceElectrode":
                    componentTypeId = 5;
                    break;
                case "Casing":
                    componentTypeId = 6;
                    break;
            }
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;
            try
            {
                List<BatteryComponentCommercialTypeExt> resultList = BatteryComponentCommercialTypeDa.GetBatteryComponentCommercialTypesByName(search, componentTypeId, researchGroupId);
                return JsonConvert.SerializeObject(resultList, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetAllBatteryComponentCommercialTypesList(string componentType, int? commercialTypeId = null)
        {
            int componentTypeId = 0;
            switch (componentType)
            {
                case "Anode":
                    componentTypeId = 1;
                    break;
                case "Cathode":
                    componentTypeId = 2;
                    break;
                case "Separator":
                    componentTypeId = 3;
                    break;
                case "Electrolyte":
                    componentTypeId = 4;
                    break;
                case "ReferenceElectrode":
                    componentTypeId = 5;
                    break;
                case "Casing":
                    componentTypeId = 6;
                    break;
            }
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;
            try
            {
                List<BatteryComponentCommercialTypeExt> resultList = BatteryComponentCommercialTypeDa.GetBatteryComponentCommercialTypes(commercialTypeId, componentTypeId, researchGroupId);
                return JsonConvert.SerializeObject(resultList, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetAllMaterialsUsedInExperiment(int experimentId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            object empty = new Object();
            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;

            List<BatteryComponentExt> allMaterials = new List<BatteryComponentExt>();

            try
            {
                List<BatteryComponentExt> allStepsContentList = BatteryComponentDa.GetAllContentInExperiment(experimentId, null, null);

                allMaterials.AddRange(allStepsContentList);
                foreach (BatteryComponentExt e in allStepsContentList)
                {
                    if (e.fkStepBatch != null)
                    {
                        List<BatchContentExt> allBatchContent = BatchContentDa.GetAllBatchContents(e.fkStepBatch);
                    }
                }

                string a = JsonConvert.SerializeObject(allStepsContentList, jsonSettings);
                return JsonConvert.SerializeObject(allStepsContentList, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }




        //not used
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetMaterialFunctions(string search)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            try
            {
                List<MaterialFunction> resultList = MaterialFunctionDa.GetMaterialFunctionsByName(search, null);
                return JsonConvert.SerializeObject(resultList, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetVendors(string search)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            try
            {
                List<Vendor> resultList = VendorDa.GetVendorsByName(search, null);
                return JsonConvert.SerializeObject(resultList, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public string GetTestGroupsForDrop(string search, int? page = null, int? projectId = null)
        //{
        //    var resp = new ResponseWrapper { status = "ok", response = null };
        //    var dateFormat = "dd/MM/yyyy"; // your datetime format
        //    var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

        //    var jsonSettings = new JsonSerializerSettings();
        //    jsonSettings.DateFormatString = dateFormat;

        //    var currentUser = UserHelper.GetCurrentUser();
        //    int researchGroupId = (int)currentUser.fkResearchGroup;
        //    try
        //    {
        //        List<TestGroupExt> TestGroupList = TestGroupDa.GetAllTestGroupsForDrop(researchGroupId, projectId);
        //        //List<TestGroupExt> resultList = TestGroupDa.GetTestGroupsByName(search, researchGroupId, page);
        //        dynamic response = new ExpandoObject();
        //        //response.results = resultList;
        //        response.pagination = new ExpandoObject();
        //        //response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
        //        return JsonConvert.SerializeObject(response, jsonSettings);
        //    }
        //    catch (Exception e)
        //    {
        //        resp.status = "error";
        //        resp.message = e.Message;
        //        return JsonConvert.SerializeObject(resp, jsonSettings);
        //    }
        //}

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetBatches(string search, int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                int researchGroupId = (int)currentUser.fkResearchGroup;
                //BatchExt batch = BatchDa.GetBatchWithContent(batchId);
                List<BatchExt> resultList = BatchDa.GetBatchesByName(search, researchGroupId, page);

                dynamic response = new ExpandoObject();
                response.results = resultList;
                response.pagination = new ExpandoObject();
                response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);

                //JObject rss;
                //if (resultList != null)
                //{
                //    rss =
                //    new JObject(
                //    new JProperty("results",
                //       new JArray(
                //           from b in resultList
                //           orderby b.batchLabel
                //           select new JObject(
                //               new JProperty("id", b.batchId),
                //               new JProperty("text", b.batchLabel)
                //               //new JProperty("chemicalFormula", b.chemicalFormula)
                //               )
                //    )
                //    ),
                //    new JProperty("pagination", "true")
                //    );
                //}
                //else
                //{
                //    rss =
                //    new JObject(
                //    new JProperty("results",
                //       new JArray())
                //    );
                //}
                //var res = JsonConvert.SerializeObject(rss, jsonSettings);
                //return res;

            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetBatchesOutsideProject(string search, int projectId, int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                int researchGroupId = (int)currentUser.fkResearchGroup;
                List<BatchExt> resultList = BatchDa.GetBatchesOutsideProject(search, researchGroupId, projectId, page);

                dynamic response = new ExpandoObject();
                response.results = resultList;
                response.pagination = new ExpandoObject();
                response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetDocumentTypes(string search)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            try
            {
                List<DocumentTypeExt> resultList = DocumentTypeDa.GetDocumentTypesByName(search);

                return JsonConvert.SerializeObject(resultList, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetProcessTypes(string search, int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            try
            {
                List<ProcessTypeExt> resultList = ProcessTypeDa.GetProcessTypesByName(search, page);

                dynamic response = new ExpandoObject();
                response.results = resultList;
                response.pagination = new ExpandoObject();
                response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetProcessSequence(string search, int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            try
            {
                List<SequenceExt> resultList = ProcessSequenceDa.GetProcessSequence(search, page);

                dynamic response = new ExpandoObject();
                response.results = resultList;
                response.pagination = new ExpandoObject();
                response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetEquipment(string search, int? fkProcess = null, int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            try
            {
                List<Equipment> resultList = EquipmentDa.GetEquipmentByProcessId(search, fkProcess, page);

                dynamic response = new ExpandoObject();
                response.results = resultList;
                response.pagination = new ExpandoObject();
                response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetEquipmentModels(string search, int? processTypeId = null, int? equipmentId = null, int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            try
            {
                List<EquipmentModelExt> resultList = EquipmentModelDa.GetEquipmentModelsForDropdown(search, processTypeId, equipmentId, page);

                dynamic response = new ExpandoObject();
                response.results = resultList;
                response.pagination = new ExpandoObject();
                response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetMaterialTypes(string search)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;
            try
            {
                List<MaterialType> resultList = MaterialTypeDa.GetMaterialTypesByName(search);
                return JsonConvert.SerializeObject(resultList, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetStoredInTypes(string search)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;
            try
            {
                List<StoredInType> resultList = StoredInTypeDa.GetStoredInTypesByName(search);
                return JsonConvert.SerializeObject(resultList, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetMeasurementUnits(string search)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;
            try
            {
                List<MeasurementUnit> resultList = MeasurementUnitDa.GetMeasurementUnitsByName(search);
                return JsonConvert.SerializeObject(resultList, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetTestTypes(string search)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            try
            {
                //var testType = new TestTypeExt();

                //if (testType.supportGraphing == false)
                //{
                //    NotifyHelper.Notify("The viewer doesn’t support these file formats yet. But you can upload the raw data and later download them in the experiments list.", NotifyHelper.NotifyType.danger, "");
                //}

                List<TestTypeExt> resultList = TestTypeDa.GetAllTestTypes(search);

                return JsonConvert.SerializeObject(resultList, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetTestTypesForCharts( string search, int[] experimentIds = null, int[] batchIds = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            try
            {
                List<TestTypeExt> resultList = TestTypeDa.GetTestTypesByExperimentOrBatchIds(search, true, experimentIds, batchIds);

                return JsonConvert.SerializeObject(resultList, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetTestEquipmentModels(string search, int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            try
            {
                List<TestEquipmentModel> resultList = TestEquipmentModelDa.GetTestEquipmentModelsPaged(search, page);

                dynamic response = new ExpandoObject();
                response.results = resultList;
                response.pagination = new ExpandoObject();
                response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);
                //return JsonConvert.SerializeObject(resultList, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string AddBatchStockTransaction(string formData)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                dynamic dynData = JsonConvert.DeserializeObject(formData);
                object submittedItem;
                //object returnElement;

                submittedItem = JsonConvert.DeserializeObject<StockTransaction>(formData, dateTimeConverter);
                //return submittedItem;
                //submittedItem.batch.fkUser = currentUser.userId;
                StockTransaction submittedItemClass = (StockTransaction)submittedItem;
                //submittedItemClass.batch.fkUser = currentUser.userId;


                Batteries.Dal.StockTransactionDa.AddBatchStockTransaction(submittedItemClass);
                //returnElement = Batteries.Dal.BatchDa.GetAllBatchesWithContent(((Batch)submittedItem).batchId);
                //resp.response = new ListObject { list = returnElement, type = new ListObjectTypeWaterSupply("PipeDamage") };

            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }

            return JsonConvert.SerializeObject(resp, jsonSettings);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string RecreateBatch(int batchId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;
            try
            {
                int result = Batteries.Dal.BatchDa.RecreateBatch(batchId, researchGroupId);
                if (result == 0)
                {
                    NotifyHelper.Notify("Batch quantity successfully recreated", NotifyHelper.NotifyType.success, "");
                }
            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
            return JsonConvert.SerializeObject(resp, jsonSettings);
        }

        //PROCESSES
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetProcessAttributes(string processType, int batchProcessId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            //object empty = new Object();
            var currentUser = UserHelper.GetCurrentUser();
            //int researchGroupId = (int)currentUser.fkResearchGroup;

            try
            {
                object resultObject = new Object();
                switch (processType)
                {
                    case "Milling":
                        MillingExt milling = MillingDa.GetAllMillings(null, null, batchProcessId)[0];
                        resultObject = milling;
                        break;
                    case "Mixing":
                        MixingExt mixing = MixingDa.GetAllMixings(null, null, batchProcessId)[0];
                        resultObject = mixing;
                        break;
                    case "Heating":
                        HeatingExt heating = HeatingDa.GetAllHeatings(null, null, batchProcessId)[0];
                        resultObject = heating;
                        break;
                    case "Pressing":
                        PressingExt pressing = PressingDa.GetAllPressings(null, null, batchProcessId)[0];
                        resultObject = pressing;
                        break;
                    case "Calendering":
                        CalenderingExt calendering = CalenderingDa.GetAllCalenderings(null, null, batchProcessId)[0];
                        resultObject = calendering;
                        break;
                    case "PhaseInversion":
                        PhaseInversionExt phaseInversion = PhaseInversionDa.GetAllPhaseInversions(null, null, batchProcessId)[0];
                        resultObject = phaseInversion;
                        break;


                    default:
                        throw new Exception("Process type not defined");
                }

                resp.response = resultObject;
                return JsonConvert.SerializeObject(resp, jsonSettings);

            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetRecentlyUsedProcessSettings(string processType)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();


            try
            {
                object resultObject = new Object();
                switch (processType)
                {
                    case "Milling":
                        //return JsonConvert.SerializeObject(resultList, jsonSettings);
                        //List<MillingExt> resultList = MillingDa.GetRecentlyUsedMillings();
                        //resultObject = resultList;
                        resultObject = MillingDa.GetRecentlyUsedMillings();
                        break;
                    case "Mixing":
                        resultObject = MixingDa.GetRecentlyUsedMixings();
                        break;
                    case "Heating":
                        resultObject = HeatingDa.GetRecentlyUsedHeatings();
                        break;
                    case "Pressing":
                        resultObject = PressingDa.GetRecentlyUsedPressings();
                        break;
                    case "Calendering":
                        resultObject = CalenderingDa.GetRecentlyUsedCalenderings();
                        break;
                    case "PhaseInversion":
                        resultObject = PhaseInversionDa.GetRecentlyUsedPhaseInversions();
                        break;
                    case "AcidDissolution":
                        resultObject = AcidDissolutionDa.GetRecentlyUsedAcidDissolutions();
                        break;
                    case "Annealing":
                        resultObject = AnnealingDa.GetRecentlyUsedAnnealings();
                        break;
                    case "Assembling":
                        resultObject = AssemblingDa.GetRecentlyUsedAssemblings();
                        break;
                    case "AtomicLayerDeposition":
                        resultObject = AtomicLayerDepositionDa.GetRecentlyUsedAtomicLayerDepositions();
                        break;
                    case "Centrifuging":
                        resultObject = CentrifugingDa.GetRecentlyUsedCentrifugings();
                        break;
                    case "Coating":
                        resultObject = CoatingDa.GetRecentlyUsedCoatings();
                        break;
                    case "Cooling":
                        resultObject = CoolingDa.GetRecentlyUsedCoolings();
                        break;
                    case "CoPrecipitation":
                        resultObject = CoPrecipitationDa.GetRecentlyUsedCoPrecipitations();
                        break;
                    case "Decanting":
                        resultObject = DecantingDa.GetRecentlyUsedDecantings();
                        break;
                    case "Decomposing":
                        resultObject = DecomposingDa.GetRecentlyUsedDecomposings();
                        break;
                    case "Depositing":
                        resultObject = DepositingDa.GetRecentlyUsedDepositings();
                        break;
                    case "Dropcasting":
                        resultObject = DropcastingDa.GetRecentlyUsedDropcastings();
                        break;
                    case "Drying":
                        resultObject = DryingDa.GetRecentlyUsedDryings();
                        break;
                    case "Electrodepositing":
                        resultObject = ElectrodepositingDa.GetRecentlyUsedElectrodepositings();
                        break;
                    case "ElectrodeWinding":
                        resultObject = ElectrodeWindingDa.GetRecentlyUsedElectrodeWindings();
                        break;
                    case "ElectrolyteDiffusionDegassing":
                        resultObject = ElectrolyteDiffusionDegassingDa.GetRecentlyUsedElectrolyteDiffusionDegassings();
                        break;
                    case "Electroplating":
                        resultObject = ElectroplatingDa.GetRecentlyUsedElectroplatings();
                        break;
                    case "Etching":
                        resultObject = EtchingDa.GetRecentlyUsedEtchings();
                        break;
                    case "Filtrating":
                        resultObject = FiltratingDa.GetRecentlyUsedFiltratings();
                        break;
                    case "Formation":
                        resultObject = FormationDa.GetRecentlyUsedFormations();
                        break;
                    case "Galvanizing":
                        resultObject = GalvanizingDa.GetRecentlyUsedGalvanizings();
                        break;
                    case "Impregnating":
                        resultObject = ImpregnatingDa.GetRecentlyUsedImpregnatings();
                        break;
                    case "Lithiation":
                        resultObject = LithiationDa.GetRecentlyUsedLithiations();
                        break;
                    case "Ozone":
                        resultObject = OzoneDa.GetRecentlyUsedOzones();
                        break;
                    case "Pasting":
                        resultObject = PastingDa.GetRecentlyUsedPastings();
                        break;
                    case "Purifying":
                        resultObject = PurifyingDa.GetRecentlyUsedPurifyings();
                        break;
                    case "Recrystalizing":
                        resultObject = RecrystalizingDa.GetRecentlyUsedRecrystalizings();
                        break;
                    case "Screenprinting":
                        resultObject = ScreenprintingDa.GetRecentlyUsedScreenprintings();
                        break;
                    case "Sealing":
                        resultObject = SealingDa.GetRecentlyUsedSealings();
                        break;
                    case "Sieving":
                        resultObject = SievingDa.GetRecentlyUsedSievings();
                        break;
                    case "Sintering":
                        resultObject = SinteringDa.GetRecentlyUsedSinterings();
                        break;
                    case "Slitting":
                        resultObject = SlittingDa.GetRecentlyUsedSlittings();
                        break;
                    case "Sonicating":
                        resultObject = SonicatingDa.GetRecentlyUsedSonicatings();
                        break;
                    case "SprayPyrrolysis":
                        resultObject = SprayPyrrolysisDa.GetRecentlyUsedSprayPyrrolysiss();
                        break;
                    case "Stacking":
                        resultObject = StackingDa.GetRecentlyUsedStackings();
                        break;
                    case "Washing":
                        resultObject = WashingDa.GetRecentlyUsedWashings();
                        break;
                    case "Welding":
                        resultObject = WeldingDa.GetRecentlyUsedWeldings();
                        break;
                    case "WetImpregnating":
                        resultObject = WetImpregnatingDa.GetRecentlyUsedWetImpregnatings();
                        break;


                    default:
                        throw new Exception("Process type not defined");
                }

                resp.response = resultObject;
                return JsonConvert.SerializeObject(resp, jsonSettings);

            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetRecentlyUsedProcess(int processId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();


            try
            {
                object resultObject = new Object();
                resultObject = EquipmentSettingsDa.GetRecentlyUsedProcess(processId);
                resp.response = resultObject;
                //jsonSettings = new JsonSerializerSettings();

                return JsonConvert.SerializeObject(resp, jsonSettings);

            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetEquipmentSettingsByProcess(int experimentOrBatchProcessId, bool comingFromBatch)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();


            try
            {
                object resultObject = new Object();
                resultObject = EquipmentSettingsDa.GetEquipmentSettingsByProcess(experimentOrBatchProcessId, comingFromBatch);
                resp.response = resultObject;
                return JsonConvert.SerializeObject(resp, jsonSettings);

            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetValuesBySequenceId(int processSequenceId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                SequenceResponse sequence = GeneralHelper.GetProcessSequence(processSequenceId);
                resp.response = sequence;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetEquipmentSettingsByEquipmentId(int? equipmentId = null, int? processId = null, int? equipmentModelId = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();


            try
            {
                object resultObject = new Object();
                resultObject = EquipmentSettingsDa.GetEquipmentSettingsByEquipmentId(equipmentId, processId, equipmentModelId);
                resp.response = resultObject;
                return JsonConvert.SerializeObject(resp, jsonSettings);

            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetRecentlyUsedEquipmentSettings(int equipmentId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();

            try
            {
                object resultObject = new Object();
                switch (equipmentId)
                {
                    case 1:
                        resultObject = MillingBallMillDa.GetRecentlyUsedMillingBallMills();
                        break;
                    case 2:
                        resultObject = MixingPlanetaryMixerDa.GetRecentlyUsedMixingPlanetaryMixers();
                        break;
                    case 3:
                        resultObject = HeatingPlateDa.GetRecentlyUsedHeatingPlates();
                        break;
                    case 4:
                        resultObject = HeatingTubeFurnaceDa.GetRecentlyUsedTubeFurnaces();
                        break;
                    case 5:
                        resultObject = PressingManualHydraulicPressDa.GetRecentlyUsedPressingManualHydraulicPresses();
                        break;
                    case 6:
                        resultObject = CalenderingHeatRollerPressDa.GetRecentlyUsedCalenderingHeatRollerPresses();
                        break;
                    case 7:
                        resultObject = CalenderingManualPressDa.GetRecentlyUsedCalenderingManualPresses();
                        break;
                    case 8:
                        resultObject = HeatingOvenDa.GetRecentlyUsedHeatingOvens();
                        break;
                    case 9:
                        resultObject = HeatingManualDa.GetRecentlyUsedHeatingManuals();
                        break;
                    case 10:
                        resultObject = PressingManualDa.GetRecentlyUsedPressingManuals();
                        break;
                    case 11:
                        resultObject = CalenderingManualDa.GetRecentlyUsedCalenderingManuals();
                        break;
                    case 12:
                        resultObject = MillingMortarAndPestleDa.GetRecentlyUsedMillingMortarAndPestles();
                        break;
                    case 13:
                        resultObject = MixingBallMillDa.GetRecentlyUsedMixingBallMills();
                        break;
                    case 14:
                        resultObject = MixingHotPlateStirrerDa.GetRecentlyUsedMixingHotPlateStirrers();
                        break;


                    default:
                        throw new Exception("Equipment type not defined");
                }

                resp.response = resultObject;
                return JsonConvert.SerializeObject(resp, jsonSettings);

            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public string BatchInsert(Batch batch)
        //{
        //    var resp = new ResponseWrapper { status = "ok", response = null };
        //    var dateFormat = "dd/MM/yyyy | H:mm"; // your datetime format
        //    var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

        //    var jsonSettings = new JsonSerializerSettings();
        //    jsonSettings.DateFormatString = dateFormat;

        //    //var currentUser = UserHelper.GetCurrentUser();


        //}


        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public string GetMaterialAvailableQuantity(int materialId)
        //{
        //    var resp = new ResponseWrapper { status = "ok", response = null };
        //    var dateFormat = "dd/MM/yyyy"; // your datetime format
        //    var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

        //    var jsonSettings = new JsonSerializerSettings();
        //    jsonSettings.DateFormatString = dateFormat;

        //    var currentUser = UserHelper.GetCurrentUser();

        //    try
        //    {
        //        List<MaterialExt> materials = MaterialDa.GetAllMaterialsWithQuantity(currentUser.fkResearchGroup, materialId, materialType);
        //        return JsonConvert.SerializeObject(materials);
        //    }
        //    catch (Exception e)
        //    {
        //        //return "Error! " + e.Message;
        //        resp.status = "error";
        //        resp.message = e.Message;
        //        return JsonConvert.SerializeObject(resp, jsonSettings);
        //    }
        //}

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetFileAttachments(string elementName, string id)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            try
            {
                resp.response = FileAttachmentDa.GetFileAttachments(null, elementName, long.Parse(id));

                var jsonSettings = new JsonSerializerSettings();
                jsonSettings.DateFormatString = "dd/MM/yyyy";

                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetFileAttachmentsMeasurements(int? testTypeId = null, int? experimentId = null, int? batchId = null, int? materialId = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            try
            {
                resp.response = FileAttachmentDa.GetFileAttachmentsMeasurementData(testTypeId, experimentId, batchId, materialId);

                var jsonSettings = new JsonSerializerSettings();
                jsonSettings.DateFormatString = "dd/MM/yyyy";

                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetFileAttachmentsMeasurementsLowLevel(int? testTypeId = null, int? experimentId = null, int? componentTypeId = null, int? stepId = null)
        {
            if (componentTypeId == 0) componentTypeId = null;
            if (stepId == 0) stepId = null;
            var resp = new ResponseWrapper { status = "ok", response = null };
            try
            {
                resp.response = FileAttachmentDa.GetFileAttachmentsMeasurementData(testTypeId, experimentId, null, null, componentTypeId, stepId);

                var jsonSettings = new JsonSerializerSettings();
                jsonSettings.DateFormatString = "dd/MM/yyyy";

                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SubmitFileAttachment(string fileBase64, string filename, string description, string elementName, int elementId, int documentTypeId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };

            //if (Helpers.UserHelper.IsViewer())
            //{
            //    resp.status = "error";
            //    resp.message = DbRes.T("You do not have the user privileges to upload files.", "Resources");
            //    return JsonConvert.SerializeObject(resp);
            //}

            try
            {
                var currentUser = UserHelper.GetCurrentUser();
                int userId = (int)currentUser.userId;

                string newFilename = Helpers.Files.GenerateUniqueFilename();
                string path = HostingEnvironment.MapPath("~/Uploads/FileAttachments/");
                Directory.CreateDirectory(path);

                string extension = Path.GetExtension(filename);

                while (File.Exists(path + newFilename + extension))
                {
                    newFilename = Helpers.Files.GenerateUniqueFilename();
                }

                //Da trgnam prv del od losh base64 enkoding
                fileBase64 = fileBase64.Substring(fileBase64.IndexOf(",") + 1);

                File.WriteAllBytes(path + newFilename + extension, Convert.FromBase64String(fileBase64));

                FileAttachment fa = new FileAttachment
                {
                    description = description,
                    elementType = elementName,
                    extension = extension,
                    elementId = elementId,
                    filename = filename,
                    serverFilename = newFilename + extension,
                    fkUploadedBy = userId,
                    fkType = documentTypeId
                };

                FileAttachmentDa.AddFileAttachment(fa);

            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp);
            }

            return JsonConvert.SerializeObject(resp);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string DeleteFileAttachment(long fileAttachmentId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };

            //if (Helpers.UserHelper.IsViewer())
            //{
            //    resp.status = "error";
            //    resp.message = DbRes.T("You do not have the user privileges make changes.", "Resources");
            //    return JsonConvert.SerializeObject(resp);
            //}

            try
            {
                var fileAttachment = FileAttachmentDa.GetFileAttachments(fileAttachmentId)[0];
                Helpers.Files.DeleteFile(fileAttachment.serverFilename);
                FileAttachmentDa.DeleteFileAttachment(fileAttachmentId);
            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp);
            }

            return JsonConvert.SerializeObject(resp);
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetFileAttachmentsExperimentByStep(string elementName, string experimentId, string componentTypeId, int stepId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            try
            {
                if (stepId == 0)
                {
                    resp.response = FileAttachmentExperimentDa.GetFileAttachmentExperiments(null, elementName, long.Parse(experimentId), int.Parse(componentTypeId), null, null);
                }
                else
                {
                    resp.response = FileAttachmentExperimentDa.GetFileAttachmentExperiments(null, elementName, long.Parse(experimentId), int.Parse(componentTypeId), stepId, null);
                }

                var jsonSettings = new JsonSerializerSettings();
                jsonSettings.DateFormatString = "dd/MM/yyyy";

                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetFileAttachmentsExperimentAllSteps(string elementName, string experimentId, string componentTypeId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            try
            {
                resp.response = FileAttachmentExperimentDa.GetFileAttachmentExperimentForComponentSteps(elementName, long.Parse(experimentId), int.Parse(componentTypeId), null);

                var jsonSettings = new JsonSerializerSettings();
                jsonSettings.DateFormatString = "dd/MM/yyyy";

                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SubmitFileAttachmentExperiment(string fileBase64, string filename, string description, string elementName, int experimentId, int componentTypeId, int stepId, int documentTypeId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };

            //if (Helpers.UserHelper.IsViewer())
            //{
            //    resp.status = "error";
            //    resp.message = DbRes.T("You do not have the user privileges to upload files.", "Resources");
            //    return JsonConvert.SerializeObject(resp);
            //}


            try
            {
                var currentUser = UserHelper.GetCurrentUser();
                int userId = (int)currentUser.userId;

                string newFilename = Helpers.Files.GenerateUniqueFilename();
                string path = HostingEnvironment.MapPath("~/Uploads/FileAttachments/");
                Directory.CreateDirectory(path);

                string extension = Path.GetExtension(filename);

                while (File.Exists(path + newFilename + extension))
                {
                    newFilename = Helpers.Files.GenerateUniqueFilename();
                }

                //Da trgnam prv del od losh base64 enkoding
                fileBase64 = fileBase64.Substring(fileBase64.IndexOf(",") + 1);

                File.WriteAllBytes(path + newFilename + extension, Convert.FromBase64String(fileBase64));


                FileAttachmentExperiment fa = new FileAttachmentExperiment
                {
                    description = description,
                    elementType = elementName,
                    extension = extension,
                    experimentId = experimentId,
                    componentTypeId = componentTypeId,
                    stepId = stepId,
                    filename = filename,
                    serverFilename = newFilename + extension,
                    fkUploadedBy = userId,
                    fkType = documentTypeId
                };
                if (stepId == 0)
                {
                    fa.stepId = null;
                }

                FileAttachmentExperimentDa.AddFileAttachmentExperiment(fa);

            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp);
            }

            return JsonConvert.SerializeObject(resp);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string DeleteFileAttachmentExperiment(long fileAttachmentId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };

            //if (Helpers.UserHelper.IsViewer())
            //{
            //    resp.status = "error";
            //    resp.message = DbRes.T("You do not have the user privileges make changes.", "Resources");
            //    return JsonConvert.SerializeObject(resp);
            //}

            try
            {
                var fileAttachment = FileAttachmentExperimentDa.GetFileAttachmentExperiments(fileAttachmentId)[0];
                Helpers.Files.DeleteFile(fileAttachment.serverFilename);
                FileAttachmentExperimentDa.DeleteFileAttachmentExperiment(fileAttachmentId);
            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp);
            }

            return JsonConvert.SerializeObject(resp);
        }


        //GRAPH METHODS


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetExperimentsSummaryList(int[] experimentIds)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            object empty = new Object();
            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;

            try
            {
                var experimentsData = Helpers.GeneralHelper.GetExperimentsSummaryList(experimentIds);
                return JsonConvert.SerializeObject(experimentsData, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetBatchSummaryList(int[] batchIds)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            object empty = new Object();
            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;

            try
            {
                var experimentsData = Helpers.GeneralHelper.GetBatchSummaryList(batchIds, researchGroupId);
                return JsonConvert.SerializeObject(experimentsData, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetTestDataForCharts(int testTypeId, int? experimentId, int? batchId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            object empty = new Object();
            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;

            try
            {
                //resp.response = FileAttachmentExperimentDa.GetFileAttachmentExperimentForComponentSteps(elementName, long.Parse(experimentId), int.Parse(componentTypeId), null);

                //var jsonSettings = new JsonSerializerSettings();
                //jsonSettings.DateFormatString = "dd/MM/yyyy";

                //add expriment info
                //add batch info
                //add all test info
                var test = TestDataDa.GetTestDataForCharts(testTypeId, experimentId, batchId);
                //var maxCalculations = ChargeDischargeTestResultDa.GetChargeDischargeMaxValues(experimentId);
                //var capacityData = ChargeDischargeTestResultDa.GetCapacityMaxValuesByCycleIndex(experimentId, batchId);
                object capacityData = null;
                var massCalculations = new object();
                if (experimentId != null && experimentId != 0)
                {
                    massCalculations = Helpers.GeneralHelper.GetMassCalculations(experimentId);
                }


                dynamic result = new ExpandoObject();
                result.test = test;
                result.capacityData = capacityData;
                result.massCalculations = massCalculations;

                resp.response = result;
                return JsonConvert.SerializeObject(resp, jsonSettings);

            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetUsers(string search, int? researchGroupId = null, int? page = null)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            //var currentUser = UserHelper.GetCurrentUser();

            try
            {
                List<UserExt> resultList = UserDa.GetUsersByName(null, null, researchGroupId, search, page);
                dynamic response = new ExpandoObject();
                response.results = resultList;
                response.pagination = new ExpandoObject();
                response.pagination.more = resultList != null ? resultList.Count() == 10 : false;
                return JsonConvert.SerializeObject(response, jsonSettings);

            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }

    }
}
