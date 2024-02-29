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

namespace Batteries.EquipmentPanel.EquipmentModels
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadEquipment();
        }
        private void LoadEquipment()
        {
            List<EquipmentExt> equipmentList = Dal.EquipmentDa.GetAllEquipment();

            int index = 0;

            foreach (Equipment equipment in equipmentList)
            {
                ddlEquipment.Items.Insert(index, new ListItem(equipment.equipmentName, equipment.equipmentId.ToString()));
                index++;
            }

            ddlEquipment.Items.Insert(0, new ListItem("", ""));
        }

        [WebMethod]
        public static string DeleteEquipmentModel(int equipmentModelId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd.MM.yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            try
            {
                var result = EquipmentModelDa.DeleteEquipmentModel(equipmentModelId);
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