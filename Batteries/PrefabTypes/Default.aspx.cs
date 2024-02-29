using Batteries.Dal;
using Batteries.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Batteries.PrefabTypes
{
    public partial class Default : System.Web.UI.Page
    {
        public string componentType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            componentType = Request.QueryString["componentType"];
            //HyperLink insertNew = (HyperLink) sender.  FindControl("InsertNew");
            //insertNew.NavigateUrl = "";
            InsertNew.DataBind();
        }

        [WebMethod]
        public static string DeleteCommercialType(int commercialTypeId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd.MM.yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            try
            {
                var result = BatteryComponentCommercialTypeDa.DeleteBatteryComponentCommercialType(commercialTypeId);                
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