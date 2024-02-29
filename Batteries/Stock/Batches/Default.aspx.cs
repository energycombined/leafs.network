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

namespace Batteries.Stock.Batches
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadBatches();
        }
        private void LoadBatches()
        {
            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;
            List<BatchExt> batchList = Dal.BatchDa.GetAllCompleteBatchesWithQuantity(researchGroupId);

            int index = 0;
            if (batchList != null)
            {
                foreach (BatchExt batch in batchList)
                {
                    ddlBatchPersonalLabel.Items.Insert(index, new ListItem(String.Concat("Batch_" + batch.batchId + " | " + batch.batchPersonalLabel, " | ", batch.chemicalFormula), batch.batchId.ToString()));
                    index++;
                }

                ddlBatchPersonalLabel.Items.Insert(0, new ListItem("", ""));
            }
        }
        [WebMethod]
        public static string FilterList(int? batchId)
        {
            //, int? researchGroupId
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd.MM.yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;
            try
            {
                List<StockTransactionExt> stockTransactions = StockTransactionDa.GetAllBatchStockTransactions(null, batchId, researchGroupId);
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