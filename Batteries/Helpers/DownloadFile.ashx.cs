using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Helpers
{
    /// <summary>
    /// Summary description for DownloadFile
    /// </summary>
    public class DownloadFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string fileAttachmentId = context.Request.QueryString["fileAttachmentId"];
            string folder = context.Request.QueryString["folder"];
            string serverFilename = "";
            string filename = "";

            var file = Dal.FileAttachmentDa.GetFileAttachments(long.Parse(fileAttachmentId))[0];
            serverFilename = file.serverFilename;
            filename = file.filename;            

            string path = context.Server.MapPath("~/Uploads/FileAttachments/" + serverFilename);

            context.Response.Clear();
            context.Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            //context.Response.ContentType = "image/png";
            context.Response.TransmitFile(path);
            context.Response.End();
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