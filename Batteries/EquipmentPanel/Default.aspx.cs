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

namespace Batteries.EquipmentPanel
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadProcessTypes();
        }
        private void LoadProcessTypes()
        {
            List<ProcessTypeExt> processTypeList = Dal.ProcessTypeDa.GetAllProcessTypes();

            int index = 0;

            foreach (ProcessType processType in processTypeList)
            {
                ddlProcessType.Items.Insert(index, new ListItem(processType.processType, processType.processTypeId.ToString()));
                index++;
            }

            ddlProcessType.Items.Insert(0, new ListItem("", ""));
        }

        [WebMethod]
        public static string DeleteEquipment(int equipmentId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd.MM.yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;
            
            try
            {
                var result = EquipmentDa.DeleteEquipment(equipmentId);                
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