using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Mappings;
using Batteries.Models;
using Batteries.Models.TestResultsModels;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.FriendlyUrls;
using Batteries.Models.Responses;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using NLog;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.IO.Compression;

namespace Batteries.GraphResults
{
    public partial class Upload : System.Web.UI.Page
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public int experimentId = 0;
        public int testTypeId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            experimentId = GetExperimentIdFromUrl();

            var currentUser = UserHelper.GetCurrentUser();
            //List<ExperimentExt> experimentListObject = ExperimentDa.GetAllExperimentsGeneralData(experimentId);                      
        }
        private int GetExperimentIdFromUrl()
        {
            IList<string> segments = Request.GetFriendlyUrlSegments();
            int pId = -1;
            if (segments.Count != 0)
                int.TryParse(segments[0], out pId);
            return pId;
        }
        protected void BtnInsert_Click(object sender, EventArgs e)
        {
            if (TestResultsFiles.HasFile && TestResultsFiles.PostedFiles.Count > 0)
            //    ||
            //TestProcedureFile.HasFile && TestProcedureFile.PostedFile.ContentLength > 0 &&
            //    (Path.GetExtension(TestResultsFiles.PostedFile.FileName).ToLower() == ".txt" ||
            //     Path.GetExtension(TestResultsFiles.PostedFile.FileName).ToLower() == ".csv") ||
            //     (Path.GetExtension(TestProcedureFile.PostedFile.FileName).ToLower() == ".txt" ||
            //     Path.GetExtension(TestProcedureFile.PostedFile.FileName).ToLower() == ".csv"))
            {
                try
                {
                    string selected = "";
                    if (Request.Form["rb"] != null)
                    {
                        selected = Request.Form["rb"].ToString();
                    }
                    int? selectedExperiment = HfExperimentSelectedValue.Value != "" ? int.Parse(HfExperimentSelectedValue.Value) : (int?)null;
                    int? selectedBatch = HfBatchSelectedValue.Value != "" ? int.Parse(HfBatchSelectedValue.Value) : (int?)null;
                    int? selectedMaterial = HfMaterialSelectedValue.Value != "" ? int.Parse(HfMaterialSelectedValue.Value) : (int?)null;
                    int? selectedTestType = HfTestTypeSelectedValue.Value != "" ? int.Parse(HfTestTypeSelectedValue.Value) : (int?)null;
                    int? selectedTestEquipmentModel = HfTesEquipmentModelSelectedValue.Value != "" ? int.Parse(HfTesEquipmentModelSelectedValue.Value) : (int?)null;
                    if (selected == "experiment" && selectedExperiment == null)
                    {
                        throw new Exception("Please select experiment");
                    }
                    if (selected == "batch" && selectedBatch == null)
                    {
                        throw new Exception("Please select batch");
                    }
                    if (selected == "material" && selectedMaterial == null)
                    {
                        throw new Exception("Please select material");
                    }
                    var currentUser = UserHelper.GetCurrentUser();
                    int userId = (int)currentUser.userId;

                    int? measurementLevelType = null;
                    if (selected == "experiment" && selectedExperiment != null)
                    {
                        measurementLevelType = 6;
                    }
                    if (selected == "batch" && selectedBatch != null)
                    {
                        measurementLevelType = 5;
                    }
                    if (selected == "material" && selectedMaterial != null)
                    {
                        measurementLevelType = 7;
                    }


                    TestType testType = TestTypeDa.GetTestTypeById((int)selectedTestType);
                    TestEquipmentModel testEquipmentModel = TestEquipmentModelDa.GetTestEquipmentModelById((int)selectedTestEquipmentModel);

                    var test = new Test
                    {
                        fkTestType = selectedTestType,
                        fkTestEquipmentModel = selectedTestEquipmentModel,
                        fkMeasurementLevelType = measurementLevelType,
                        fkExperiment = selectedExperiment,
                        fkBatch = selectedBatch,
                        fkMaterial = selectedMaterial,
                        //fkBatteryComponentType = ,
                        //stepId = ,
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
                            fkType = 3 //Test results doc type
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
                                fkType = 4 //Test procedure doc type
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
                    //var insertedTestId = TestDa.AddTest(test); moved

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




        /**
                    //MANUAL CONVERT TO JSON
                    if (bool.Parse(ConfigurationManager.AppSettings["uploadTestMode"]))
                    {
                        string extensionC = Path.GetExtension(TestResultsFiles.PostedFiles[0].FileName);
                        string newFilenameC = Helpers.Files.GenerateUniqueFilename();
                        Directory.CreateDirectory(path);
                        while (File.Exists(path + newFilenameC + extensionC))
                        {
                            newFilenameC = Helpers.Files.GenerateUniqueFilename();
                        }
                        File.WriteAllBytes(path + newFilenameC + extensionC, TestResultsFiles.FileBytes);

                        using (var reader = new CsvReader(File.OpenText(path + newFilenameC + extensionC)))
                        {
                            reader.Configuration.RegisterClassMap<ChargeDischargeTestResultMap>();
                            var listRows = reader.GetRecords<ChargeDischargeTestResult>().ToList();
                            var dateFormat = "dd/MM/yyyy"; // your datetime format
                            var jsonSettings = new JsonSerializerSettings();
                            jsonSettings.DateFormatString = dateFormat;
                            string jsonb = JsonConvert.SerializeObject(listRows, jsonSettings);
                            //fkTest default 1
                            //fkExperiment set

                            //var deleteResult = 
                            var insertedMeasurementDataId = ChargeDischargeTestResultDa.AddMeasurementsDataOld(jsonb, selectedExperiment, selectedBatch, selectedTestType, selectedTestEquipmentModel);
                            //var insertedMeasurementDataId2 = ChargeDischargeTestResultDa.AddMeasurementsData(jsonb, selectedExperiment, selectedBatch, selectedTestType, selectedTestEquipmentModel);
                            //var insertResult = ChargeDischargeTestResultDa.AddChargeDischargeTestResults(listRows.ToList<ChargeDischargeTestResult>(), experimentId);
                            if (insertedMeasurementDataId != 0)
                            {
                                //SET EXPERIMENT HAS TEST DOC BOOL TO TRUE     
                                ClientScript.RegisterStartupScript(this.GetType(), "randomtext", "successMessage()", true);
                                //NotifyHelper.Notify("Successful import", NotifyHelper.NotifyType.success, "");
                                //RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
                            }

                            //FileAttachment fa = new FileAttachment
                            //{
                            //    description = null,
                            //    elementType = "ChargeDischargeTestResults",
                            //    extension = extension,
                            //    elementId = insertedMeasurementDataId,
                            //    filename = TestResultsFiles.FileName,
                            //    serverFilename = newFilename + extension,
                            //    fkUploadedBy = userId,
                            //    fkType = 3
                            //};

                            //var result = FileAttachmentDa.AddFileAttachment(fa);

                            ////save na multiple files od TestProcedureFiles
                            //if (TestProcedureFile.HasFiles)
                            //{
                            //    foreach (HttpPostedFile uploadedFile in TestProcedureFile.PostedFiles)
                            //    {
                            //        FileAttachment fa2 = new FileAttachment
                            //        {
                            //            description = null,
                            //            elementType = "TestResultTestProcedureFiles",
                            //            extension = extension2,
                            //            elementId = insertedMeasurementDataId,
                            //            filename = uploadedFile.FileName,
                            //            serverFilename = newFilename2 + extension2,
                            //            fkUploadedBy = userId,
                            //            fkType = 6
                            //        };
                            //        var result2 = FileAttachmentDa.AddFileAttachment(fa2);
                            //    }
                            //}
                        }

                    }

                    **/
    }
}