using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models;
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

namespace Batteries.ResearchGroups
{
    public partial class Default : System.Web.UI.Page
    {
        public bool isAdmin;
        protected void Page_Load(object sender, EventArgs e)
        {
            isAdmin = UserHelper.IsAdmin();
        }
        [WebMethod]
        public static string FilterList(int? researchGroupId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            try
            {
                List<ResearchGroupExt> researchGroups = ResearchGroupDa.GetAllResearchGroups(null);
                return JsonConvert.SerializeObject(researchGroups, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp, jsonSettings);
            }
        }
        [WebMethod]
        public static string DeleteResearchGroup(int researchGroupId)
        {

            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            try
            {
                var result = ResearchGroupDa.DeleteResearchGroup(researchGroupId);
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