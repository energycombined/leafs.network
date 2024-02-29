using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Batteries.DocumentTypes
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static string FilterList(int? documentTypeId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd.MM.yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            try
            {
                List<DocumentTypeExt> documentTypes = DocumentTypeDa.GetAllDocumentTypes(null);
                return JsonConvert.SerializeObject(documentTypes);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        public static string DeleteDocumentType(int documentTypeId)
        {

            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd.MM.yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            try
            {
                var result = DocumentTypeDa.DeleteDocumentType(documentTypeId);
            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp);
            }
            return JsonConvert.SerializeObject(resp, jsonSettings);
        }
    }
}