using Batteries.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Forms.DataSources
{
    /// <summary>
    /// Summary description for Equipment
    /// </summary>
    public class Equipment : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int processType = int.Parse(context.Request.QueryString["processType"]);

            var json = EquipmentDa.GetAllEquipmentJsonForDropdown(processType);

            context.Response.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}