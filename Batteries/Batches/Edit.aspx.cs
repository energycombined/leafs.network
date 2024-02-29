using Batteries.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.FriendlyUrls;
using Batteries.Models.Responses;
using Batteries.Helpers;

namespace Batteries.Batches
{
    public partial class Edit : System.Web.UI.Page
    {
        public int batchId;
        public string batch;
        public string materialFunctionsJson;
        
        private int GetBatchIdFromUrl()
        {
            IList<string> segments = Request.GetFriendlyUrlSegments();
            int pId = -1;
            if (segments.Count != 0)
                int.TryParse(segments[0], out pId);
            return pId;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            batchId = GetBatchIdFromUrl();
            //batch = new WebMethods().GetBatchWithContent(batchId);


            
            
            
            if (IsPostBack) return;
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Batches/Default");
        }
    }
}