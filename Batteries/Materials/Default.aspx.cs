using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models;
using Batteries.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Microsoft.AspNet.FriendlyUrls;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Converters;


namespace Batteries.Materials
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadMaterialTypes();
        }
        private void LoadMaterialTypes()
        {
            List<MaterialType> materialTypeList = Dal.MaterialTypeDa.GetAllMaterialTypes();

            int index = 0;

            foreach (MaterialType materialType in materialTypeList)
            {
                ddlMaterialType.Items.Insert(index, new ListItem(materialType.materialType, materialType.materialTypeId.ToString()));
                index++;
            }

            ddlMaterialType.Items.Insert(0, new ListItem("", ""));
        }
        [WebMethod]
        public static string FilterList(int? materialType)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd.MM.yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();

            try
            {
                List<MaterialExt> materials = MaterialDa.GetAllMaterialsWithQuantity(currentUser.fkResearchGroup, null, materialType);
                return JsonConvert.SerializeObject(materials);
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
        public static string DeleteMaterial(int materialId)
        {
            //try
            //{
            //    var result = MaterialDa.DeleteMaterial(materialId);
            //    if (result == 0)
            //    {
            //        NotifyHelper.Notify("Successfully deleted Material", NotifyHelper.NotifyType.success, "");
            //        //RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
            //        //Response.Redirect("~/Materials/Default");
            //    }
            //    else
            //        NotifyHelper.Notify("Error deleting Material", NotifyHelper.NotifyType.danger, "");
            //}
            //catch (System.Threading.ThreadAbortException)
            //{
            //}
            //catch (Exception ex)
            //{
            //    NotifyHelper.Notify(ex.Message, NotifyHelper.NotifyType.danger, "");
            //}

            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd.MM.yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            try
            {
                var result = MaterialDa.DeleteMaterial(materialId);
                /*
                if (result != 0)
                {
                    //NotifyHelper.Notify("Successfully deleted Material", NotifyHelper.NotifyType.success, "");
                    //return "";
                    return "Error! Something went wrong";
                }
                //return "";
                 * */
            }
            catch (Exception ex)
            {
                //NotifyHelper.Notify(ex.Message, NotifyHelper.NotifyType.danger, "");
                //return "Error! " + ex.Message;
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp);
            }
            return JsonConvert.SerializeObject(resp, jsonSettings);
        }
    }
}