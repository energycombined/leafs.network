using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Batteries.Materials
{
    public partial class Insert : System.Web.UI.Page
    {
        //private User currentUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadMaterialTypes();
            LoadStoredInTypes();
            LoadMeasurementUnits();
            //LoadVendors();
            LoadMaterialFunctions();

            //currentUser = 
        }
        private void LoadMaterialTypes()
        {
            List<MaterialType> materialTypeList = Dal.MaterialTypeDa.GetAllMaterialTypes();

            int index = 0;

            foreach (MaterialType materialType in materialTypeList)
            {
                DdlMaterialType.Items.Insert(index, new ListItem(materialType.materialType, materialType.materialTypeId.ToString()));
                index++;
            }

            DdlMaterialType.Items.Insert(0, new ListItem("", ""));
        }
        private void LoadStoredInTypes()
        {
            List<StoredInType> storedInTypeList = Dal.StoredInTypeDa.GetAllStoredInTypes();

            int index = 0;

            foreach (StoredInType storedInType in storedInTypeList)
            {
                DdlStoredInType.Items.Insert(index, new ListItem(storedInType.storedInType, storedInType.storedInTypeId.ToString()));
                index++;
            }

            DdlStoredInType.Items.Insert(0, new ListItem("", ""));
        }        
        private void LoadMeasurementUnits()
        {
            List<MeasurementUnit> measurementUnitList = Dal.MeasurementUnitDa.GetAllMeasurementUnits();

            int index = 0;

            foreach (MeasurementUnit measurementUnit in measurementUnitList)
            {
                DdlMeasurementUnit.Items.Insert(index, new ListItem(String.Concat(measurementUnit.measurementUnitSymbol, " | ", measurementUnit.measurementUnitName), measurementUnit.measurementUnitId.ToString()));
                index++;
            }

            DdlMeasurementUnit.Items.Insert(0, new ListItem("", ""));
        }
        private void LoadMaterialFunctions()
        {
            List<MaterialFunction> materialFunctionList = Dal.MaterialFunctionDa.GetAllMaterialFunctions();

            int index = 0;

            foreach (MaterialFunction materialFunction in materialFunctionList)
            {
                DdlMaterialFunction.Items.Insert(index, new ListItem(materialFunction.materialFunctionName, materialFunction.materialFunctionId.ToString()));
                index++;
            }

            DdlMaterialFunction.Items.Insert(0, new ListItem("", ""));
        }
        //private void LoadVendors()
        //{
        //    List<Vendor> vendorList = Dal.VendorDa.GetAllVendors();

        //    int index = 0;

        //    foreach (Vendor vendor in vendorList)
        //    {
        //        DdlVendor.Items.Insert(index, new ListItem(vendor.vendorName + " | " + vendor.vendorSite, vendor.vendorId.ToString()));
        //        index++;
        //    }

        //    DdlVendor.Items.Insert(0, new ListItem("", ""));
        //}
        protected void BtnInsert_Click(object sender, EventArgs e)
        {
            string format = "dd/mm/yyyy";
            try
            {                
                //DateTime? dateBought = (TxtDateBought.Text != "") ? DateTime.ParseExact(TxtDateBought.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
                var material = new Material
                {
                    materialName = TxtName.Text,
                    chemicalFormula = TxtChemicalFormula.Text,
                    fkMaterialType = int.Parse(DdlMaterialType.SelectedValue),
                    fkStoredInType = int.Parse(DdlStoredInType.SelectedValue),
                    fkFunction = (DdlMaterialFunction.SelectedValue != "") ? int.Parse(DdlMaterialFunction.SelectedValue) : (int?)null,
                    percentageOfActive = (TxtPercentageOfActive.Text != "") ? double.Parse(TxtPercentageOfActive.Text) : (double?)null,
                    fkOperator = UserHelper.GetCurrentUser().userId,
                    fkMeasurementUnit = int.Parse(DdlMeasurementUnit.SelectedValue),
                    materialLabel = TxtMaterialLabel.Text,
                    description = TxtDescription.Text,
                    dateBought = (TxtDateBought.Text != "") ? DateTime.ParseExact(TxtDateBought.Text, ConfigurationManager.AppSettings["dateFormat"], CultureInfo.InvariantCulture) : (DateTime?)null,
                    //fkVendor = (DdlVendor.Value != "") ? int.Parse(DdlVendor.Value) : (int?)null,
                    fkVendor = Request.Form["DdlVendor"] != null ? int.Parse(Request.Form["DdlVendor"]) : (int?)null,
                    price = (TxtPrice.Text != "") ? double.Parse(TxtPrice.Text) : (double?)null,
                    bulkPrice = (TxtBulkPrice.Text != "") ? double.Parse(TxtBulkPrice.Text) : (double?)null,
                    //casNumber = (TxtCasNumber.Text != "") ? int.Parse(TxtCasNumber.Text) : (int?)null,
                    //lotNumber = (TxtLotNumber.Text != "") ? int.Parse(TxtLotNumber.Text) : (int?)null,
                    casNumber = (TxtCasNumber.Text != "") ? TxtCasNumber.Text : null,
                    lotNumber = (TxtLotNumber.Text != "") ? TxtLotNumber.Text : null,
                    reference = TxtReference.Text
                };
                double stockAmount = double.Parse(TxtAmount.Text);
                int researchGroupId = (int)UserHelper.GetCurrentUser().fkResearchGroup;

                //validate percentage of active
                if (material.percentageOfActive != null)
                {
                    if (material.percentageOfActive < 0 || material.percentageOfActive > 100)
                    {
                        Exception ex = new Exception("Invalid \'percentage of active\' value.");
                        throw ex;
                    }
                }

                var result = MaterialDa.AddMaterialWithStock(material, stockAmount, researchGroupId);
                if (result != 0)
                {
                    NotifyHelper.Notify("Success", NotifyHelper.NotifyType.success, "");
                    if (sender == BtnInsert)
                    {
                        RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
                    }
                    else if (sender == BtnSaveAndEdit)
                    {
                        RedirectHelper.RedirectToReturnUrl(ResolveUrl("Edit/" + result), Response);
                    }
                }
                else
                    NotifyHelper.Notify("Material not inserted", NotifyHelper.NotifyType.danger, "");
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
            Response.Redirect("~/Materials/Default");
        }

    }
}