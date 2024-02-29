using Batteries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.FriendlyUrls;
using Batteries.Dal;
using Batteries.Helpers;

namespace Batteries.MaterialFunctions
{
    public partial class Edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            var materialFunction = GetMaterialFunction(GetMaterialFunctionIdFromUrl());
            Fill(materialFunction);
        }
        private int GetMaterialFunctionIdFromUrl()
        {
            IList<string> segments = Request.GetFriendlyUrlSegments();
            int pId = -1;
            if (segments.Count != 0)
                int.TryParse(segments[0], out pId);
            return pId;
        }
        private MaterialFunction GetMaterialFunction(int materialFunctionId)
        {
            var materialFunction = MaterialFunctionDa.GetAllMaterialFunctions(materialFunctionId);
            return materialFunction[0];
        }
        private void Fill(MaterialFunction materialFunction)
        {
            TxtMaterialFunction.Text = materialFunction.materialFunctionName;
        }
        protected void UpdateButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                var materialFunction = new MaterialFunction
                {
                    materialFunctionId = GetMaterialFunctionIdFromUrl(),
                    materialFunctionName = TxtMaterialFunction.Text
                };
                var result = MaterialFunctionDa.UpdateMaterialFunction(materialFunction);
                if (result == 0)
                {
                    NotifyHelper.Notify("Success", NotifyHelper.NotifyType.success, "");
                    RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
                }
                else
                    NotifyHelper.Notify("Material function info not updated", NotifyHelper.NotifyType.danger, "");
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