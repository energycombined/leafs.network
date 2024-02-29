using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.FriendlyUrls;
using Batteries.Models.Responses;
using System.Configuration;
using System.Globalization;

namespace Batteries.Materials
{
    public partial class View : System.Web.UI.Page
    {
        public int materialId = 0;
        public bool isFromOtherResearchGroup = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            var currentUser = UserHelper.GetCurrentUser();
            var material = GetMaterial(GetMaterialIdFromUrl());
            if (material != null && material.fkResearchGroup != currentUser.fkResearchGroup)
            {
                //NotifyHelper.Notify("Error", NotifyHelper.NotifyType.danger, "");
                //RedirectHelper.RedirectToReturnUrl("~/Materials/Default", Response);
                isFromOtherResearchGroup = true;
            }
            materialId = (int)material.materialId;
            if (IsPostBack) return;
            Fill(material);
        }
        private int GetMaterialIdFromUrl()
        {
            IList<string> segments = Request.GetFriendlyUrlSegments();
            int pId = -1;
            if (segments.Count != 0)
                int.TryParse(segments[0], out pId);
            return pId;
        }
        private MaterialExt GetMaterial(int materialId)
        {
            var currentUser = UserHelper.GetCurrentUser();
            //var material = MaterialDa.GetAllMaterialsWithQuantity(currentUser.fkResearchGroup, materialId);
            var material = MaterialDa.GetMaterialWithQuantity(currentUser.fkResearchGroup, materialId);

            if (material != null)
                return material[0];
            else
                return null;
        }

        private void Fill(MaterialExt material)
        {
            //DateTime date = DateTime.Parse(material.dateBought.ToString());
            //string inGermanFormat = date.ToString(CultureInfo.GetCultureInfo("en-EN"));

            LblMaterialName.Text = material.materialName;
            LblChemicalFormula.Text = material.chemicalFormula;
            LblMaterialLabel.Text = material.materialLabel;
            LblMaterialType.Text = material.materialType;
            LblStoredInType.Text = material.storedInType;
            LblFunctionInExperiment.Text = material.materialFunction;
            LblDescription.Text = material.description;
            LblAmount.Text = (material.availableQuantity != null ? material.availableQuantity.ToString() : "0") + " " + material.measurementUnitSymbol;
            LblMeasurementUnit.Text = material.measurementUnitName + "(" + material.measurementUnitSymbol + ")";
            LblPrice.Text = material.price != null ? material.price.ToString() + " €" : "";
            LblBulkPrice.Text = material.bulkPrice != null ? material.bulkPrice.ToString() + " €/kg" : "";
            LblDateBought.Text = material.dateBought.ToString() != "" ? DateTime.Parse(material.dateBought.ToString()).ToString(ConfigurationManager.AppSettings["dateFormat"]) : "";
            LblCasNumber.Text = material.casNumber;
            LblLotNumber.Text = material.lotNumber;
            LblVendorName.Text = material.vendorName;
            HlVendorSite.NavigateUrl = "http://" + material.vendorSite;
            HlVendorSite.Text = material.vendorSite;

            if (material.fkFunction == 1)
            {
                if (material.percentageOfActive != null)
                    LblPercentageOfActive.Text = material.percentageOfActive + " %";
            }
            else
                LblPercentageOfActive.Text = "/";
        }

        protected void CancelButton_OnClick(object sender, EventArgs e)
        {
            RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
        }
    }
}