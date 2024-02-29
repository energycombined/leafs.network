using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models;
using Batteries.Models.Responses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Batteries.Stock.Materials
{
    public partial class Insert : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadMaterials();
            LoadVendors();
        }
        private void LoadMaterials()
        {
            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;
            List<MaterialExt> materialList = Dal.MaterialDa.GetAllMaterialsWithQuantity(researchGroupId);

            int index = 0;

            foreach (MaterialExt material in materialList)
            {
                DdlMaterialName.Items.Insert(index, new ListItem(String.Concat(material.materialName, " | ", material.chemicalFormula), material.materialId.ToString()));
                index++;
            }

            DdlMaterialName.Items.Insert(0, new ListItem("", ""));
        }
        private void LoadVendors()
        {
            List<Vendor> vendorList = Dal.VendorDa.GetAllVendors();

            int index = 0;

            foreach (Vendor vendor in vendorList)
            {
                DdlVendor.Items.Insert(index, new ListItem(vendor.vendorName, vendor.vendorId.ToString()));
                index++;
            }

            DdlVendor.Items.Insert(0, new ListItem("", ""));
        }
        protected void BtnInsert_Click(object sender, EventArgs e)
        {
            var currentUser = UserHelper.GetCurrentUser();
            try
            {
                if (double.Parse(TxtAmount.Text) <= 0)
                {
                    throw new Exception("Amount must be greater then zero!");
                }

                var stockTransaction = new StockTransaction
                {
                    fkMaterial = int.Parse(DdlMaterialName.SelectedValue),
                    amount = double.Parse(TxtAmount.Text),
                    //transactionDirection = 1,
                    transactionDirection = short.Parse(RbTransactionDirection.SelectedValue),
                    dateBought = (TxtDateBought.Text != "") ? DateTime.ParseExact(TxtDateBought.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null,
                    //dateBought = TxtDateBought.Text ? DateTime.Parse(TxtDateBought.Text).Date : null,
                    fkVendor = (DdlVendor.SelectedValue != "") ? int.Parse(DdlVendor.SelectedValue) : (int?)null,
                    fkOperator = currentUser.userId,
                    fkResearchGroup = currentUser.fkResearchGroup,
                };
                var result = StockTransactionDa.AddMaterialStockTransaction(stockTransaction);
                if (result != 0)
                {
                    NotifyHelper.Notify("Success", NotifyHelper.NotifyType.success, "");
                    //RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
                    RedirectHelper.RedirectToReturnUrl(ResolveUrl("View/" + result), Response);
                }
                else
                    NotifyHelper.Notify("Stock transaction data not inserted", NotifyHelper.NotifyType.danger, "");
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                NotifyHelper.Notify(ex.Message, NotifyHelper.NotifyType.danger, "");
            }
        }
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Stock/Materials/Default");
        }
    }
}