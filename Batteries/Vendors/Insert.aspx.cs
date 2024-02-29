using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Batteries.Vendors
{
    public partial class Insert : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void BtnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                var vendor = new Vendor
                {
                    vendorName = TxtVendorName.Text,
                    vendorSite = TxtVendorSite.Text,
                    contactPerson = TxtContactPerson.Text,
                    phoneNumber = TxtPhoneNumber.Text,
                    comment = TxtComment.Text
                };
                var result = VendorDa.AddVendor(vendor);
                if (result != 0)
                {
                    NotifyHelper.Notify("Success", NotifyHelper.NotifyType.success, "");
                    RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
                }
                else
                    NotifyHelper.Notify("Vendor not inserted", NotifyHelper.NotifyType.danger, "");
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
            Response.Redirect("~/Vendors/Default");
        }
    }
}