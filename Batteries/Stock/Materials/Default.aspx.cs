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

namespace Batteries.Stock.Materials
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadMaterials();
        }
        private void LoadMaterials()
        {
            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;

            List<MaterialExt> materialList = Dal.MaterialDa.GetAllMaterialsWithQuantity(researchGroupId);

            int index = 0;
            if (materialList != null)
            {

                foreach (MaterialExt material in materialList)
                {
                    ddlMaterialName.Items.Insert(index, new ListItem(String.Concat(material.materialName, " | ", material.chemicalFormula), material.materialId.ToString()));
                    index++;
                }

                ddlMaterialName.Items.Insert(0, new ListItem("", ""));
            }
        }

        [WebMethod]
        public static string FilterList(int? materialId)
        {
            //, int? researchGroupId
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;
            //int researchGroupId = 1;
            //fkOperator = UserHelper.GetCurrentUser().UserId
            try
            {

                List<StockTransactionExt> stockTransactions = StockTransactionDa.GetAllMaterialStockTransactions(null, materialId, researchGroupId);
                //if (stockTransactions != null)
                //{
                //    return JsonConvert.SerializeObject(stockTransactions, jsonSettings);
                //}
                //return "";
                return JsonConvert.SerializeObject(stockTransactions, jsonSettings);
            }
            catch (Exception e)
            {
                resp.status = "error";
                resp.message = e.Message;
                return JsonConvert.SerializeObject(resp);
            }
        }
    }
}