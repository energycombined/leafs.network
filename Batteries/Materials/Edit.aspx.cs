using Batteries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.FriendlyUrls;
using Batteries.Dal;
using Batteries.Models.Responses;
using Batteries.Helpers;
using System.Globalization;
using System.Configuration;

namespace Batteries.Materials
{
    public partial class Edit : System.Web.UI.Page
    {
        public int materialId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            var material = GetMaterial(GetMaterialIdFromUrl());
            if (material == null)
            {
                NotifyHelper.Notify("Error", NotifyHelper.NotifyType.danger, "");
                RedirectHelper.RedirectToReturnUrl("~/Materials/Default", Response);
            }
            materialId = (int) material.materialId;
            if (IsPostBack) return;
            LoadMaterialTypes();
            LoadStoredInTypes();
            LoadMeasurementUnits();
            LoadMaterialFunctions();
            LoadVendors();
            Fill(material);
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
        private void LoadVendors()
        {
            List<Vendor> vendorList = Dal.VendorDa.GetAllVendors();

            int index = 0;

            foreach (Vendor vendor in vendorList)
            {
                DdlVendor.Items.Insert(index, new ListItem(vendor.vendorName + " | " + vendor.vendorSite, vendor.vendorId.ToString()));
                index++;
            }

            DdlVendor.Items.Insert(0, new ListItem("", ""));
        }


        private int GetMaterialIdFromUrl()
        {
            IList<string> segments = Request.GetFriendlyUrlSegments();
            int pId = -1;
            if (segments.Count != 0)
                int.TryParse(segments[0], out pId);
            return pId;
        }
        private Material GetMaterial(int materialId)
        {
            var currentUser = UserHelper.GetCurrentUser();
            var material = MaterialDa.GetAllMaterialsWithQuantity(currentUser.fkResearchGroup, materialId);
            if (material != null)
                return material[0];
            else
                return null;
        }
        private void Fill(Material material)
        {
            TxtName.Text = material.materialName;
            TxtChemicalFormula.Text = material.chemicalFormula;
            TxtMaterialLabel.Text = material.materialLabel;
            DdlMaterialType.SelectedValue = material.fkMaterialType != (int?)null ? material.fkMaterialType.ToString() : "";
            DdlStoredInType.SelectedValue = material.fkStoredInType != (int?)null ? material.fkStoredInType.ToString() : "";
            DdlMaterialFunction.SelectedValue = material.fkFunction != (int?)null ? material.fkFunction.ToString() : "";
            TxtPercentageOfActive.Text = material.percentageOfActive != (double?)null ? material.percentageOfActive.ToString() : "";
            TxtDescription.Text = material.description;
            DdlMeasurementUnit.SelectedValue = material.fkMeasurementUnit.ToString();
            TxtPrice.Text = material.price.ToString();
            TxtBulkPrice.Text = material.bulkPrice.ToString();
            TxtDateBought.Text = material.dateBought.ToString() != "" ? DateTime.Parse(material.dateBought.ToString()).ToString(ConfigurationManager.AppSettings["dateFormat"]) : "";
            DdlVendor.SelectedValue = material.fkVendor != (int?)null ? material.fkVendor.ToString() : "";
            TxtReference.Text = material.reference;
            TxtCasNumber.Text = material.casNumber;
            TxtLotNumber.Text = material.lotNumber;            
        }

        protected void UpdateButton_OnClick(object sender, EventArgs e)
        {
            //try
            //{
            //    var result = Bl.UpdateUser(GetUserIdFromUrl(), TxtUsername.Text, TxtFirstname.Text, TxtLastname.Text, TxtPhone.Text,
            //        TxtEmail.Text, CbActive.Checked);
            //    if (result)
            //        Response.Redirect("../Default");
            //    else
            //        SetErrorMessage("User not saved");
            //}
            //catch (Exception ex)
            //{
            //    Logger.Error(ex);
            //    //SetErrorMessage("Грешка при менување на корисник");
            //    SetErrorMessage(ex.Message);
            //}


            try
            {
                var material = new Material
                {
                    materialId = GetMaterialIdFromUrl(),
                    materialName = TxtName.Text,
                    chemicalFormula = TxtChemicalFormula.Text,
                    fkMaterialType = DdlMaterialType.SelectedValue != "" ? int.Parse(DdlMaterialType.SelectedValue) : (int?)null,
                    fkStoredInType = DdlStoredInType.SelectedValue != "" ? int.Parse(DdlStoredInType.SelectedValue) : (int?)null,
                    fkFunction = (DdlMaterialFunction.SelectedValue != "") ? int.Parse(DdlMaterialFunction.SelectedValue) : (int?)null,
                    percentageOfActive = (TxtPercentageOfActive.Text != "") ? double.Parse(TxtPercentageOfActive.Text) : (double?)null,
                    fkOperator = UserHelper.GetCurrentUser().userId,
                    fkMeasurementUnit = int.Parse(DdlMeasurementUnit.SelectedValue),
                    materialLabel = TxtMaterialLabel.Text,
                    description = TxtDescription.Text,
                    dateBought = (TxtDateBought.Text != "") ? DateTime.ParseExact(TxtDateBought.Text, ConfigurationManager.AppSettings["dateFormat"], CultureInfo.InvariantCulture) : (DateTime?)null,

                    fkVendor = (DdlVendor.SelectedValue != "") ? int.Parse(DdlVendor.SelectedValue) : (int?)null,
                    price = (TxtPrice.Text != "") ? double.Parse(TxtPrice.Text) : (double?)null,
                    bulkPrice = (TxtBulkPrice.Text != "") ? double.Parse(TxtBulkPrice.Text) : (double?)null,
                    reference = TxtReference.Text,
                    casNumber = (TxtCasNumber.Text != "") ? TxtCasNumber.Text : null,
                    lotNumber = (TxtLotNumber.Text != "") ? TxtLotNumber.Text : null
                };

                //validate percentage of active
                if (material.percentageOfActive != null)
                {
                    if (material.percentageOfActive < 0 || material.percentageOfActive > 100)
                    {
                        Exception ex = new Exception("Invalid \'percentage of active\' value.");
                        throw ex;
                    }
                }

                var result = MaterialDa.UpdateMaterial(material);
                if (result == 0)
                {
                    NotifyHelper.Notify("Success", NotifyHelper.NotifyType.success, "");
                    RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
                }
                else
                    NotifyHelper.Notify("Material info not updated", NotifyHelper.NotifyType.danger, "");
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                NotifyHelper.Notify(ex.Message, NotifyHelper.NotifyType.danger, "");
            }
        }
        protected void CancelButton_OnClick(object sender, EventArgs e)
        {
            RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
        }

    }
}