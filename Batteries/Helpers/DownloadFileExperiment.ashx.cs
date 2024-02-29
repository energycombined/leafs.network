using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Helpers
{
    /// <summary>
    /// Summary description for DownloadFileExperiment
    /// </summary>
    public class DownloadFileExperiment : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string fileAttachmentId = context.Request.QueryString["fileAttachmentId"];
            string folder = context.Request.QueryString["folder"];
            string serverFilename = "";
            string filename = "";

            var file = Dal.FileAttachmentExperimentDa.GetFileAttachmentExperiments(long.Parse(fileAttachmentId))[0];
            serverFilename = file.serverFilename;
            filename = file.filename;

            //string path = context.Server.MapPath("~/Uploads/FileAttachments/" + folder + "/" + serverFilename);
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