using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Mappings;
using Batteries.Models.Responses;
using Batteries.Models.TestResultsModels;
using CsvHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace Batteries.Service
{
    /// <summary>
    /// Summary description for WebMethodsPublic
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebMethodsPublic : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CheckExperimentById(string token, int experimentId)
        {
            
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            if (token != ConfigurationManager.AppSettings["token"])
            {
                //throw new Exception("Unauthorizes access");
                throw new HttpException(401, "Unauthorizes access");
            }

            try
            {               
                List<ExperimentExt> experimentsList = ExperimentDa.GetAllCompleteExperimentsGeneralData(experimentId, null);
                if (experimentsList != null)
                {
                    resp.response = true;
                }
                else
                {
                    resp.response = false;
                    resp.message = "Experiment ID does not exist in database";
                }

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
        public string ImportTestResultsData(string token, string fileBase64, string filename, int experimentId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            if (token != ConfigurationManager.AppSettings["token"])
            {
                throw new HttpException(401, "Unauthorized access");
            }
            try
            {
                byte[] fileBytes = Convert.FromBase64String(fileBase64);
                Stream stream = new MemoryStream(fileBytes);
                StreamReader sr = new StreamReader(stream);

                using (var reader = new CsvReader(sr))
                {
                    reader.Configuration.RegisterClassMap<ChargeDischargeTestResultMap>();
                    var listRows = reader.GetRecords<ChargeDischargeTestResult>().ToList();

                    var insertResult = ChargeDischargeTestResultDa.AddChargeDischargeTestResults(listRows.ToList<ChargeDischargeTestResult>(), experimentId);
                    if (insertResult == 0)
                    {
                        resp.response = true;
                    }
                    else
                    {
                        throw new Exception("Error importing data");
                    }

                }

                return JsonConvert.SerializeObject(resp, jsonSettings);

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
