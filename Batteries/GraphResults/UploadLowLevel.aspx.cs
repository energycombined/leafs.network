using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models;
using Batteries.Models.Responses;
using Microsoft.AspNet.FriendlyUrls;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Batteries.GraphResults
{
    public partial class UploadLowLevel : System.Web.UI.Page
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public int experimentId = 0;
        //private int batchId = 0;
        public int componentTypeId = 0;
        public int stepId = 0;

        public int measurementLevelType = 0;

        //private int stepMaterialId = 0;
        //private int stepBatchId = 0;

        private ExperimentExt experiment;

        public int testTypeId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (IsPostBack) return;
            var currentUser = UserHelper.GetCurrentUser();
            GetParametersFromUrl();

            if (experimentId != 0)
            {
                if (stepId != 0 && componentTypeId != 0)
                {
                    measurementLevelType = 2; // steps
                    //Check if step is valid
                    if (!BatteryComponentDa.StepExists(experimentId, componentTypeId, stepId))
                    {
                        NotifyHelper.Notify("Invalid step number", NotifyHelper.NotifyType.danger, "");
                        BtnInsert.Enabled = false;
                    }
                }
                else if (stepId == 0 && componentTypeId != 0 && (componentTypeId >= 1 && componentTypeId <= 6))
                {
                    measurementLevelType = 3; // component
                }
                else
                {
                    NotifyHelper.Notify("Some error occured", NotifyHelper.NotifyType.danger, "");
                    BtnInsert.Enabled = false;
                }

                experiment = ExperimentDa.GetExperimentByIdAndRGCreator(experimentId, (int)currentUser.fkResearchGroup);
                if (experiment == null)
                {
                    NotifyHelper.Notify("Invalid experiment!", NotifyHelper.NotifyType.danger, "");
                    BtnInsert.Enabled = false;
                }
            }
        }

        protected void BtnInsert_Click(object sender, EventArgs e)
        {
            //Check if parameters OK (step id)
            if (TestResultsFiles.HasFile && TestResultsFiles.PostedFiles.Count > 0)
            {
                try
                {
                    int? selectedExperimentId = experimentId != 0 ? experimentId : (int?)null;
                    //int? selectedBatch = HfBatchSelectedValue.Value != "" ? int.Parse(HfBatchSelectedValue.Value) : (int?)null;
                    int? selectedBatteryComponentId = componentTypeId != 0 ? componentTypeId : (int?)null;
                    int? selectedStepId = stepId != 0 ? stepId : (int?)null;

                    int? selectedTestType = HfTestTypeSelectedValue.Value != "" ? int.Parse(HfTestTypeSelectedValue.Value) : (int?)null;
                    int? selectedTestEquipmentModel = HfTesEquipmentModelSelectedValue.Value != "" ? int.Parse(HfTesEquipmentModelSelectedValue.Value) : (int?)null;

                    var currentUser = UserHelper.GetCurrentUser();
                    int userId = (int)currentUser.userId;

                    int? selectedMeasurementLevelType = measurementLevelType != 0 ? measurementLevelType : (int?)null;

                    TestType testType = TestTypeDa.GetTestTypeById((int)selectedTestType);
                    TestEquipmentModel testEquipmentModel = TestEquipmentModelDa.GetTestEquipmentModelById((int)selectedTestEquipmentModel);

                    var test = new Test
                    {
                        fkTestType = selectedTestType,
                        fkTestEquipmentModel = selectedTestEquipmentModel,
                        fkMeasurementLevelType = selectedMeasurementLevelType,
                        fkExperiment = selectedExperimentId,
                        //fkBatch = selectedBatch,
                        //fkMaterial = selectedMaterial,
                        fkBatteryComponentType = selectedBatteryComponentId,
                        stepId = selectedStepId,
                        //fkBatteryComponentContent = ,
                        //fkBatchContent = ,
                        fkResearchGroup = currentUser.fkResearchGroup,
                        fkUser = currentUser.userId,
                        testLabel = TxtTestLabel.Text,
                        comment = TxtComment.Text,
                    };

                    var insertedTestId = TestDa.AddTest(test);

                    //Upload all file attachments first
                    string path = HostingEnvironment.MapPath("~/Uploads/FileAttachments/");

                    foreach (HttpPostedFile uploadedFile in TestResultsFiles.PostedFiles)
                    {
                        string extension = Path.GetExtension(uploadedFile.FileName);
                        string newFilename = Helpers.Files.GenerateUniqueFilename();
                        Directory.CreateDirectory(path);
                        while (File.Exists(path + newFilename + extension))
                        {
                            newFilename = Helpers.Files.GenerateUniqueFilename();
                        }
                        File.WriteAllBytes(path + newFilename + extension, TestResultsFiles.FileBytes);
                        FileAttachment fa = new FileAttachment
                        {
                            description = null,
                            elementType = "Test",
                            extension = extension,
                            elementId = insertedTestId,
                            filename = uploadedFile.FileName,
                            serverFilename = newFilename + extension,
                            fkUploadedBy = userId,
                            fkType = 3
                        };

                        var result = FileAttachmentDa.AddFileAttachment(fa);

                    }

                    if (TestProcedureFile.HasFile)
                    {
                        foreach (HttpPostedFile uploadedFile in TestProcedureFile.PostedFiles)
                        {
                            string newFilename2 = Helpers.Files.GenerateUniqueFilename();
                            string extension2 = Path.GetExtension(TestProcedureFile.PostedFile.FileName);
                            Directory.CreateDirectory(path);
                            while (File.Exists(path + newFilename2 + extension2))
                            {
                                newFilename2 = Helpers.Files.GenerateUniqueFilename();
                            }
                            File.WriteAllBytes(path + newFilename2 + extension2, TestProcedureFile.FileBytes);

                            FileAttachment fa2 = new FileAttachment
                            {
                                description = null,
                                elementType = "Test",//TestProcedureFile
                                extension = extension2,
                                elementId = insertedTestId,
                                filename = uploadedFile.FileName,
                                serverFilename = newFilename2 + extension2,
                                fkUploadedBy = userId,
                                fkType = 6
                            };
                            var result2 = FileAttachmentDa.AddFileAttachment(fa2);
                        }
                    }

                    NotifyHelper.Notify("File upload success", NotifyHelper.NotifyType.success, "");

                    string returnedJson = null;

                    if (testType.supportsGraphing)
                    {
                        if (!(bool.Parse(ConfigurationManager.AppSettings["uploadJsonDirectlyOption"])) ||
                            (bool.Parse(ConfigurationManager.AppSettings["uploadJsonDirectlyOption"]) && !CbJsonDirectly.Checked))
                        {

                            //CALL API TO GET THE JSON
                            //var apiUrl = "http://5.175.19.250:5000/upload_file";
                            var apiUrl = ConfigurationManager.AppSettings["pythonApiUrl"];

                            string tempDir = HostingEnvironment.MapPath("~/Uploads") + "\\TempUpload\\Tmp_";
                            // Create temp directory

                            string newDirName = Helpers.Files.GenerateUniqueFilename();
                            tempDir += newDirName;
                            while (Directory.Exists(tempDir))
                            {
                                newDirName = Helpers.Files.GenerateUniqueFilename();
                            }
                            Directory.CreateDirectory(tempDir);

                            foreach (HttpPostedFile uploadedFile in TestResultsFiles.PostedFiles)
                            {
                                uploadedFile.SaveAs(Path.Combine(tempDir, uploadedFile.FileName));
                            }
                            //tempDir += "qokse4iw2mw";
                            try
                            {
                                using (var httpClient = new HttpClient())
                                {
                                    using (var form = new MultipartFormDataContent())
                                    {
                                        string[] allFiles = Directory.GetFiles(tempDir);

                                        foreach (string filePath in allFiles)
                                        {
                                            //var filename = Path.GetFileName(newFilePath);
                                            //var newFilePath = Files.ConvertToGzip(uploadedFile.InputStream, uploadedFile.FileName, uploadedFile.ContentType, tempDir);
                                            var newFilePath = Files.ConvertToGzip(filePath, tempDir);
                                        }

                                        string[] allFilesGz = Directory.GetFiles(tempDir, "*.gz");
                                        foreach (string filePath in allFilesGz)
                                        {
                                            string fileName = Path.GetFileName(filePath);
                                            //form.Add(Files.CreateFileContent(File.ReadAllBytes(filePath), fileName, MimeMapping.GetMimeMapping(filePath)));
                                            var fileStream = File.OpenRead(filePath); //new FileStream(filePath, FileMode.Open);  
                                            form.Add(Files.CreateFileContent(fileStream, fileName, MimeMapping.GetMimeMapping(filePath)));
                                        }
                                        //form.Add(Files.CreateFileContent(uploadedFile.InputStream, uploadedFile.FileName, uploadedFile.ContentType));

                                        form.Add(new StringContent(testType.testType.ToUpper()), "test_type");
                                        form.Add(new StringContent(testType.testTypeSubcategory.ToUpper()), "test_type_subcategory");
                                        form.Add(new StringContent(testEquipmentModel.brandName.ToUpper()), "instrument_brand");
                                        form.Add(new StringContent(testEquipmentModel.testEquipmentModelName.ToUpper()), "instrument");

                                        var response = httpClient.PostAsync(apiUrl, form).Result;
                                        //response.EnsureSuccessStatusCode();
                                        if (response.IsSuccessStatusCode)
                                        {
                                            var contentStream = response.Content.ReadAsStreamAsync().Result;
                                            var responseBody = new System.IO.StreamReader(contentStream).ReadToEnd();
                                            JsonResponseWrapper responseObj = JsonConvert.DeserializeObject<JsonResponseWrapper>(responseBody);

                                            //Code 1 - error, Code 0 Success
                                            //{ "Code":1,"Message":"Only gz files allowed"}
                                            if (responseObj.code == 1)
                                            {
                                                logger.Error("Conversion api: " + responseObj.message);
                                                //throw new Exception("Conversion of files failed!");
                                                throw new Exception("Conversion of files failed! " + responseObj.message);
                                            }
                                            else if (responseObj.code == 0)
                                            {
                                                returnedJson = responseBody;
                                            }
                                        }
                                    }
                                }
                                if (Directory.Exists(tempDir))
                                    Directory.Delete(tempDir, true);
                            }
                            catch (Exception ex)
                            {
                                if (Directory.Exists(tempDir))
                                    Directory.Delete(tempDir, true);
                                throw ex;
                            }
                        }

                    }

                    //var insertedMeasurementDataId = 0;
                    //var insertedMeasurementDataId = ChargeDischargeTestResultDa.AddMeasurementsDataOld(jsonb, selectedExperiment, selectedBatch, selectedTestType, selectedTestEquipmentModel);
                    //var insertedTestId = TestDa.AddTest(test); moved up

                    if (testType.supportsGraphing)
                    {
                        //IF INSERTING JSON DIRECTLY                    
                        if (bool.Parse(ConfigurationManager.AppSettings["uploadJsonDirectlyOption"])
                        && CbJsonDirectly.Checked)
                        {
                            HttpPostedFile uploadedJsonFile = TestResultsFiles.PostedFiles.Where(x => x.ContentType == "application/json").FirstOrDefault();
                            if (uploadedJsonFile != null)
                            {
                                String json = new System.IO.StreamReader(uploadedJsonFile.InputStream).ReadToEnd();
                                var insertedTestDataId = TestDa.AddTestDataConvertedJson(json, insertedTestId, test);
                            }
                            else
                            {
                                throw new Exception("No json file uploaded!");
                            }
                        }
                        else
                        {
                            //INSERT JSON RETURNED FROM API
                            var insertedTestDataId = TestDa.AddTestDataConvertedJson(returnedJson, insertedTestId, test);
                        }
                    }                    

                    if (insertedTestId != 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "randomtext", "successMessage()", true);
                        //NotifyHelper.Notify("Successful import", NotifyHelper.NotifyType.success, "");
                        //RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
                    }

                    //Da trgnam prv del od losh base64 enkoding
                    //fileBase64 = fileBase64.Substring(fileBase64.IndexOf(",") + 1);

                    //File.WriteAllBytes(path + newFilename + extension, Convert.FromBase64String(fileBase64));                                        




                    //var checkRes = new Helpers.WebMethods().CheckExperimentById(11);
                    //var res = new Helpers.WebMethods().ImportTestResultsData(TestResultsFiles.FileBytes, "bla", 5);

                }

                catch (Exception ex)
                {
                    logger.Error(ex, ex.Message);
                    //resp.status = "error";
                    //resp.message = ex.Message;
                    //return JsonConvert.SerializeObject(resp);
                    //return ex.Message.ToString();
                    NotifyHelper.Notify(ex.Message, NotifyHelper.NotifyType.danger, "");
                }

                //string jScript = "<script> setTimeout(function() {window.close()}, 3000);</script>";
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", jScript);
                //RedirectHelper.RedirectToReturnUrl("~/Experiments/Default", Response);
            }

        }
        private void GetParametersFromUrl()
        {
            int.TryParse(Request.QueryString.Get("expId"), out experimentId);
            int.TryParse(Request.QueryString.Get("componentId"), out componentTypeId);
            int.TryParse(Request.QueryString.Get("step"), out stepId);
            //int.TryParse(Request.QueryString.Get("expIds"), out stepMaterialId);
            //int.TryParse(Request.QueryString.Get("expIds"), out stepBatchId);
        }
    }
}