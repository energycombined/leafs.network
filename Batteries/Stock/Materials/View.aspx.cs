using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.FriendlyUrls;


namespace Batteries.Stock.Materials
{
    public partial class View : System.Web.UI.Page
    {
        public int stockTransactionId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            var currentUser = UserHelper.GetCurrentUser();
            var stockTransaction = GetStockTransaction(GetStockTransactionIdFromUrl());
            if (stockTransaction == null || stockTransaction.fkResearchGroup != currentUser.fkResearchGroup)
            {
                RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
            }
            else
            {
                stockTransactionId = (int)stockTransaction.stockTransactionId;
                Fill(stockTransaction);
            }
            
            if (IsPostBack) return;
        }
        private int GetStockTransactionIdFromUrl()
        {
            IList<string> segments = Request.GetFriendlyUrlSegments();
            int pId = -1;
            if (segments.Count != 0)
                int.TryParse(segments[0], out pId);
            return pId;
        }
        private StockTransactionExt GetStockTransaction(int stockTransactionId)
        {
            var currentUser = UserHelper.GetCurrentUser();
            var stockTransaction = StockTransactionDa.GetAllMaterialStockTransactions(stockTransactionId);
            if (stockTransaction == null)
                return null;
            return stockTransaction[0];
        }

        private void Fill(StockTransactionExt stockTransaction)
        {
            LblStockTransactionId.Text = stockTransaction.stockTransactionId.ToString();
            LblMaterialName.Text = stockTransaction.materialName;
            LblOperator.Text = stockTransaction.operatorUsername;
            LblTransactionType.Text = stockTransaction.transactionDirection == 1 ? "Addition" : "Subtraction";
            LblAmount.Text =  stockTransaction.amount.ToString();
            LblChemicalFormula.Text = stockTransaction.materialChemicalFormula;
            LblMeasurementUnit.Text = stockTransaction.measurementUnitName + "(" + stockTransaction.measurementUnitSymbol + ")";
        }

        protected void CancelButton_OnClick(object sender, EventArgs e)
        {
            RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
        }
    }
}