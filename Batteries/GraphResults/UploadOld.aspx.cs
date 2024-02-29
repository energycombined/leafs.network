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

namespace Batteries.GraphResults
{
    public partial class UploadOld : System.Web.UI.Page
    {
        public int experimentId;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Helpers.GeneralHelper.GetMassCalculations(10);
            experimentId = GetExperimentIdFromUrl();

            var currentUser = UserHelper.GetCurrentUser();
            List<ExperimentExt> experimentListObject = ExperimentDa.GetAllExperimentsGeneralData(experimentId);
            if (experimentListObject != null)
            {
                ExperimentExt experimentObject = experimentListObject[0];
                if (experimentObject.fkResearchGroup != currentUser.fkResearchGroup)
                {
                    //NotifyHelper.Notify("Error", NotifyHelper.NotifyType.danger, "");
                    RedirectHelper.RedirectToReturnUrl("~/Experiments/Default", Response);
                }
            }
            else
            {
                NotifyHelper.Notify("Error", NotifyHelper.NotifyType.danger, "");
                RedirectHelper.RedirectToReturnUrl("~/Experiments/Default", Response);
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

        protected void BtnInsert_Click(object sender, EventArgs e)
        {
            if (FuDoc.HasFile && FuDoc.PostedFile.ContentLength > 0 &&
                (Path.GetExtension(FuDoc.PostedFile.FileName).ToLower() == ".txt" ||
                 Path.GetExtension(FuDoc.PostedFile.FileName).ToLower() == ".csv"))
            {
                try
                {
                    var currentUser = UserHelper.GetCurrentUser();
                    int userId = (int)currentUser.userId;

                    string newFilename = Helpers.Files.GenerateUniqueFilename();
                    string path = HostingEnvironment.MapPath("~/Uploads/FileAttachments/");
                    Directory.CreateDirectory(path);

                    string extension = Path.GetExtension(FuDoc.PostedFile.FileName);

                    while (File.Exists(path + newFilename + extension))
                    {
                        newFilename = Helpers.Files.GenerateUniqueFilename();
                    }
                    //FuDoc.
                    //Da trgnam prv del od losh base64 enkoding
                    //fileBase64 = fileBase64.Substring(fileBase64.IndexOf(",") + 1);

                    //File.WriteAllBytes(path + newFilename + extension, Convert.FromBase64String(fileBase64));
                    File.WriteAllBytes(path + newFilename + extension, FuDoc.FileBytes);

                    FileAttachment fa = new FileAttachment
                    {
                        description = null,
                        elementType = "ChargeDischargeTestResults",
                        extension = extension,
                        elementId = 1,
                        filename = FuDoc.FileName,
                        serverFilename = newFilename + extension,
                        fkUploadedBy = userId,
                        fkType = 3
                    };

                    var result = FileAttachmentDa.AddFileAttachment(fa);




                    //var checkRes = new Helpers.WebMethods().CheckExperimentById(11);
                    //var res = new Helpers.WebMethods().ImportTestResultsData(FuDoc.FileBytes, "bla", 5);


                    using (var reader = new CsvReader(File.OpenText(path + newFilename + extension)))
                    {
                        reader.Configuration.RegisterClassMap<ChargeDischargeTestResultMap>();
                        var listRows = reader.GetRecords<ChargeDischargeTestResult>().ToList();

                        //fkTest default 1
                        //fkExperiment set

                        //var deleteResult = 
                        var insertResult = ChargeDischargeTestResultDa.AddChargeDischargeTestResults(listRows.ToList<ChargeDischargeTestResult>(), experimentId);
                        if (insertResult == 0)
                        {
                            //SET EXPERIMENT HAS TEST DOC BOOL TO TRUE     

                            NotifyHelper.Notify("Successful import", NotifyHelper.NotifyType.success, "");
                            //RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
                        }
                    }

                }
                catch (Exception ex)
                {
                    //resp.status = "error";
                    //resp.message = ex.Message;
                    //return JsonConvert.SerializeObject(resp);
                    //return ex.Message.ToString();
                    NotifyHelper.Notify(ex.Message, NotifyHelper.NotifyType.danger, "");
                }

            }
        }
    }
}