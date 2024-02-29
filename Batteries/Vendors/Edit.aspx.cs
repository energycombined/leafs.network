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

namespace Batteries.Vendors
{
    public partial class Edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            var vendor = GetVendor(GetVendorIdFromUrl());
            Fill(vendor);
        }
        private int GetVendorIdFromUrl()
        {
            IList<string> segments = Request.GetFriendlyUrlSegments();
            int pId = -1;
            if (segments.Count != 0)
                int.TryParse(segments[0], out pId);
            return pId;
        }
        private Vendor GetVendor(int vendorId)
        {
            var vendor = VendorDa.GetAllVendors(vendorId);
            return vendor[0];
        }
        private void Fill(Vendor vendor)
        {
            TxtVendorName.Text = vendor.vendorName;
            TxtVendorSite.Text = vendor.vendorSite;
            TxtContactPerson.Text = vendor.contactPerson;
            TxtPhoneNumber.Text = vendor.phoneNumber;
            TxtComment.Text = vendor.comment;

        }
        protected void UpdateButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                var vendor = new Vendor
                {
                    vendorId = GetVendorIdFromUrl(),
                    vendorName = TxtVendorName.Text,
                    vendorSite = TxtVendorSite.Text,
                    contactPerson = TxtContactPerson.Text,
                    phoneNumber = TxtPhoneNumber.Text,
                    comment = TxtComment.Text
                };
                var result = VendorDa.UpdateVendor(vendor);
                if (result == 0)
                {
                    NotifyHelper.Notify("Success", NotifyHelper.NotifyType.success, "");
                    RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
                }
                else
                    NotifyHelper.Notify("Vendor info not updated", NotifyHelper.NotifyType.danger, "");
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