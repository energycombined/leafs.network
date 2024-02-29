using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Hosting;

namespace Batteries.Helpers
{
    public class Files
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public static string GenerateUniqueFilename(string fileExtension = "", string prefix = "", string suffix = "")
        {
            string fileName = Path.GetRandomFileName();

            if (fileExtension != "")
            {
                //Za sekoj slucaj trganje na tocka od extenzija
                fileExtension = fileExtension.Replace(".", "");
            }

            //Posto generira i random ekstenzija, samo ja trgam tockata
            fileName = fileName.Replace(".", "");

            fileName = prefix + fileName + suffix + (fileExtension != "" ? '.' + fileExtension : "");

            return fileName;
        }

        /*
        public static void SaveFile()
        {
            string newFilename = Helpers.Files.GenerateUniqueFilename();
            string path = HostingEnvironment.MapPath("~/Uploads/FileAttachments/");
            string extension = Path.GetExtension(filename);

            while (File.Exists(path + newFilename + extension))
            {
                newFilename = Helpers.Files.GenerateUniqueFilename();
            }

            //Da trgnam prv del od losh base64 enkoding
            fileBase64 = fileBase64.Substring(fileBase64.IndexOf(",") + 1);

            File.WriteAllBytes(path + newFilename + extension, Convert.FromBase64String(fileBase64));
        }
         */

        public static bool DeleteFile(string serverFilename)
        {
            //string path = HostingEnvironment.MapPath("~/Uploads/FileAttachments/" + folder + "/");
            string path = HostingEnvironment.MapPath("~/Uploads/FileAttachments/");
            File.Delete(path + serverFilename);
            return true;
        }

        public static string ConvertToGzip(string filePath, string tempDir)//Stream stream, string fileName, string contentType, string tempDir)
        {
            string newFilePath = "";
            //string fileName = Path.GetFileName(filePath);
            //string tempFile = Path.Combine(tempDir, fileName);

            FileInfo fileToBeGZipped = new FileInfo(filePath);
            FileInfo gzipFileName = new FileInfo(string.Concat(fileToBeGZipped.FullName, ".gz"));

            try
            {
                using (FileStream fileToBeZippedAsStream = fileToBeGZipped.OpenRead())
                {
                    using (FileStream gzipTargetAsStream = gzipFileName.Create())
                    {
                        using (GZipStream gzipStream = new GZipStream(gzipTargetAsStream, CompressionMode.Compress))
                        {
                            try
                            {
                                fileToBeZippedAsStream.CopyTo(gzipStream);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }

            return newFilePath;
        }

        public static StreamContent CreateFileContent(Stream stream, string fileName, string contentType)
        {
            StreamContent streamContent = new StreamContent(stream);
            streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "\"files\"",
                FileName = "\"" + string.Concat(fileName) + "\""
            }; // the extra quotes are key here
            //streamContent.Headers.Add("Content-Disposition", string.Format("form-data; name=\"files\"; filename=\"{0}\"", fileName));

            streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            //streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            //fileContent.Headers.ContentEncoding.Add("gzip");
            return streamContent;
        }

        public static ByteArrayContent CreateFileContent(byte[] byteArr, string fileName, string contentType)
        {
            var byteArrayContent = new ByteArrayContent(byteArr);
            byteArrayContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "\"files\"",
                FileName = "\"" + string.Concat(fileName) + "\""
            }; // the extra quotes are key here
            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            //byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            //fileContent.Headers.ContentEncoding.Add("gzip");
            //byteArrayContent.Headers.ContentLength = byteArr.Length;
            return byteArrayContent;
        }
    }
}