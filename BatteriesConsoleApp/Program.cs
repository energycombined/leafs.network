using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace FileValidationTool
{
    using System;
    using System.Timers;
    using System.Security.Cryptography;
    using System.Net;
    using System.Configuration;
    using System.Xml.Linq;
    using Newtonsoft.Json;

    class Program
    {
        private static string _experimentsourceFolder;
        private static string _experimentIdRequstURL;
        private static string _postRequestURL;

        public static bool IfIdExists;

        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            _experimentsourceFolder = ConfigurationManager.AppSettings["experimentsourceFolder"];
            _experimentIdRequstURL = ConfigurationManager.AppSettings["experimentIdRequstURL"];
            _postRequestURL = ConfigurationManager.AppSettings["postRequestURL"];

            ReadFilesGenerateHashCodes();

            //Timer x = new Timer(5000);
            //x.AutoReset = true;
            //x.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            //x.Start();

            //Console.Read();

        }

        /* Function that sends csv file to server using post request. Web POST Method */

        //private static void SendFileToServer(string experimentId,string filename,string filePath)
        //{
        //    try
        //    {
        //        var httpWebRequest = (HttpWebRequest)WebRequest.Create(_postRequestURL);
        //        httpWebRequest.ContentType = "application/json";
        //        httpWebRequest.Method = "POST";

        //        byte[] AsBytes = File.ReadAllBytes(@filePath);
        //        String AsBase64String = Convert.ToBase64String(AsBytes);

        //        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //            {
        //                var obj = new
        //                {
        //                    token = ConfigurationManager.AppSettings["token"],
        //                    experimentId = experimentId,
        //                    filename = filename,
        //                    fileBase64 = AsBase64String

        //                };

        //                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        //                streamWriter.Write(jsonString);

        //            }

        //        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //        {
        //            var result = streamReader.ReadToEnd();
        //            //Console.WriteLine(result);
        //            logger.Info("Response from Server:" + result);
        //        }


        //    }
        //    catch (WebException ex)
        //    {
        //        logger.Info("Error for file " + experimentId + ex.ToString());
        //    }

        //}


        public static async Task PostData(string experimentId, string filename, string filePath)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(_postRequestURL);
            httpWebRequest.Timeout = 1000000;
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            // Await the GetResponseAsync() call.
            byte[] AsBytes = File.ReadAllBytes(@filePath);
            String AsBase64String = Convert.ToBase64String(AsBytes);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var obj = new
                {
                    token = ConfigurationManager.AppSettings["token"],
                    experimentId = experimentId,
                    filename = filename,
                    fileBase64 = AsBase64String

                };

                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                streamWriter.Write(jsonString);

            }

            using (var response = await httpWebRequest.GetResponseAsync().ConfigureAwait(true))
            {
                // The following code will execute on the main thread.
                using (var streamResponse = response.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(streamResponse))
                    {
                        var result = streamReader.ReadToEnd();
                        //Console.WriteLine(result);
                        logger.Info("Response from Server:" + result);
                    }
                }
            }
        }


        /*Function for sending get request to server in order to check if the id of the csv file exists Web GET Method*/

        private static void CreateGetServiceFunction(int fileId)
        {
            string url = _experimentIdRequstURL + "&experimentId=" + fileId;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string strResponse = reader.ReadToEnd();

            logger.Info("Response from Server:" + strResponse);

            if (strResponse.Contains("true"))
            {
                IfIdExists = true;
            }
            else if (strResponse.Contains("false"))
            {
                IfIdExists = false;
            }

        }





        /* Function for validating files and sending them to server */

        private static void ReadFilesGenerateHashCodes()
        {
            try
            {

                string[] files = Directory.GetFiles(@_experimentsourceFolder, "*.txt");

                //logger.Info(" NUMBER OF FILES " + files.Length);
                foreach (string dir in files)
                {
                    //System.Threading.Thread.Sleep(2000);
                    string fileNameWithExtension = Path.GetFileName(dir);

                    string fileName = Path.GetFileNameWithoutExtension(dir);

                    //bool checkStart = checkIfIsDigit(fileName);

                    bool checkStringFile = validateString(fileName);
                    logger.Info(fileName + " - NAME " + (checkStringFile ? "ok" : "invalid"));
                    if (checkStringFile)
                    {
                        int experimentId = GetExperimentId(fileName);

                        CreateGetServiceFunction(experimentId);
                        if (IfIdExists)
                        {
                            logger.Info(experimentId + "-ID exists in the DB for that file!");
                            //Console.WriteLine("postoi id vo baza za toj file ");

                            string correspondingTxtFilePath = @_experimentsourceFolder + fileName + ".MD5";

                            Boolean txtFileWithSameName = File.Exists(correspondingTxtFilePath);

                            String fileHashCode = CalculateMD5(dir);

                            string stringFileIdValue = GetExperimentIdAsString(fileName);
                            // if the .txt file exists compare 
                            if (txtFileWithSameName)
                            {
                                CompareHashCodes(correspondingTxtFilePath, fileHashCode, stringFileIdValue, fileNameWithExtension, dir);
                            }
                            //ako ne postoi zapisi
                            // if the .txt file not exists then write the hash code
                            else
                            {
                                // send the file

                                // SendFileToServer(GetExperimentIdAsString(fileName), fileNameWithExtension, dir);
                                PostData(GetExperimentIdAsString(fileName), fileNameWithExtension, dir).Wait();
                                // SendFileToServer(GetExperimentIdAsString(fileName), fileNameWithExtension, dir);
                                //write the new md5
                                File.WriteAllText(correspondingTxtFilePath, fileHashCode);
                                logger.Info("There are no same files, generate HASH MD5 code");
                                // Console.WriteLine("Ne postojat isti zapisi, generiraj hash code");                                
                            }
                        }
                        else
                        {
                            logger.Info(experimentId + "-ID does not exist in the DB!");
                            // Console.WriteLine("Ne postoi zapis so takvo ID vo baza");
                        }


                    }
                    else
                    {
                        logger.Info("The name is not in valid format!");
                    }
                }
            }
            catch (Exception e)
            {
                logger.Info("Exception thrown - Message " + e.Message + " Stack trace " + e.StackTrace);
            }
        }





        private static Boolean validateString(String fileName)
        {
            if (fileName.Contains("_"))
            {
                string[] words = fileName.Split('_');

                String test = words[0];

                Boolean isDigit = test.All(char.IsDigit);
                logger.Info(test + " - is digit: " + (isDigit ? "yes" : "no") + " second word " + words[1]);

                if (isDigit && words[1].Equals("ES"))
                {
                    return true;
                }
            }

            return false;



        }

        /* Function for comparing the hash code values of the files */

        private static void CompareHashCodes(string correspondingTxtFilePath, string fileHashCode, string experimentId, string fileNameWithExtension, string dir)
        {
            string contentFromTxtFile = File.ReadAllText(correspondingTxtFilePath);

            if (fileHashCode == contentFromTxtFile)
            {
                logger.Info("The file hasn't changed!");
            }
            else
            {
                // SendFileToServer(experimentId, fileNameWithExtension, dir);
                PostData(experimentId, fileNameWithExtension, dir).Wait();
                File.WriteAllText(correspondingTxtFilePath, fileHashCode);
                logger.Info("The files are different!");
                logger.Info("Sending file to server");
            }
        }


        /* Function for generating hash md5 value of file */

        private static string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        /* Function that parses file name and returns the id integer value from the name */

        private static Boolean checkIfIsDigit(String fileName)
        {
            string[] words = fileName.Split('_');
            String test = words[0];
            Boolean isDigit = test.All(char.IsDigit);
            return isDigit;
        }

        private static int GetExperimentId(String fileName)
        {
            string[] words = fileName.Split('_');

            int fileId = int.Parse(words[0]);
            return fileId;
        }

        /* Function that parses file name and returns the id string value from the name */

        private static string GetExperimentIdAsString(String fileName)
        {
            string[] words = fileName.Split('_');
            string getExperimentIdAsString = words[0];
            return getExperimentIdAsString;
        }
    }
}

