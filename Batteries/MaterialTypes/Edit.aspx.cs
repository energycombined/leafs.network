using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.FriendlyUrls;
using Batteries.Models;
using Batteries.Dal;
using Batteries.Helpers;

namespace Batteries.MaterialTypes
{
    public partial class Edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            var materialType = GetMaterialType(GetMaterialTypeIdFromUrl());
            Fill(materialType);
        }
        private int GetMaterialTypeIdFromUrl()
        {
            IList<string> segments = Request.GetFriendlyUrlSegments();
            int pId = -1;
            if (segments.Count != 0)
                int.TryParse(segments[0], out pId);
            return pId;
        }
        private MaterialType GetMaterialType(int materialTypeId)
        {
            var materialType = MaterialTypeDa.GetAllMaterialTypes(materialTypeId);
            return materialType[0];
        }
        private void Fill(MaterialType materialType)
        {
            TxtMaterialType.Text = materialType.materialType;
        }
        protected void UpdateButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                var materialType = new MaterialType
                {
                    materialTypeId = GetMaterialTypeIdFromUrl(),
                    materialType = TxtMaterialType.Text
                };
                var result = MaterialTypeDa.UpdateMaterialType(materialType);
                if (result == 0)
                {
                    NotifyHelper.Notify("Success", NotifyHelper.NotifyType.success, "");
                    RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
                }
                else
                    NotifyHelper.Notify("Material type info not updated", NotifyHelper.NotifyType.danger, "");
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