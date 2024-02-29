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

namespace Batteries.StoredInTypes
{
    public partial class Edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            var storedInType = GetStoredInType(GetStoredInTypeIdFromUrl());
            Fill(storedInType);
        }
        private int GetStoredInTypeIdFromUrl()
        {
            IList<string> segments = Request.GetFriendlyUrlSegments();
            int pId = -1;
            if (segments.Count != 0)
                int.TryParse(segments[0], out pId);
            return pId;
        }
        private StoredInType GetStoredInType(int storedInTypeId)
        {
            var storedInType = StoredInTypeDa.GetAllStoredInTypes(storedInTypeId);
            return storedInType[0];
        }
        private void Fill(StoredInType storedInType)
        {
            TxtStoredInType.Text = storedInType.storedInType;
        }
        protected void UpdateButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                var storedInType = new StoredInType
                {
                    storedInTypeId = GetStoredInTypeIdFromUrl(),
                    storedInType = TxtStoredInType.Text
                };
                var result = StoredInTypeDa.UpdateStoredInType(storedInType);
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