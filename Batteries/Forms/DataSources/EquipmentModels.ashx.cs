using Batteries.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Forms.DataSources
{
    /// <summary>
    /// Summary description for EquipmentModels
    /// </summary>
    public class EquipmentModels : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int equipmentId = int.Parse(context.Request.QueryString["equipmentId"]);

            var json = EquipmentModelDa.GetAllEquipmentModelsJsonForDropdown(equipmentId);

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