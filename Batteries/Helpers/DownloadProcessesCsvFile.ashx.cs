using Batteries.Dal;
using Batteries.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Helpers
{
    /// <summary>
    /// Summary description for DownloadProcessesCsvFile
    /// </summary>
    public class DownloadProcessesCsvFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string fileAttachmentId = context.Request.QueryString["fileAttachmentId"];
            string folder = context.Request.QueryString["folder"];
            //string serverFilename = "";
            string filename = "ProcessesUsed_" + DateTime.Now.ToString("dd_MM_yyyy") + ".csv";

            try
            {
                var currentUser = UserHelper.GetCurrentUser();
                int researchGroupId = (int)currentUser.fkResearchGroup;

                int[] ignoredMaterialFunctionsArray = Helpers.GeneralHelper.GetMaterialFunctionsToIgnore();
                List<dynamic> allContent;
                List<ExperimentProcessExt> experimentProcessList;
                List<dynamic> allProcesses;

                allContent = new List<dynamic>();
                allProcesses = new List<dynamic>();

                //List<ExperimentExt> allExperiments = ExperimentDa.GetAllCompleteExperimentsGeneralData(null, researchGroupId, false);
                List<ExperimentExt> allExperiments = ExperimentDa.GetAllExperimentsListForResearchGroup(researchGroupId, null, false);
                //List<ExperimentExt> allExperiments = ExperimentDa.GetAllFinishedExperiments();

                if (allExperiments != null)
                {
                    foreach (ExperimentExt e in allExperiments)
                    {
                        List<BatchContentExt> allbatchesContentList = new List<BatchContentExt>();

                        List<BatteryComponentExt> experimentContentList = BatteryComponentDa.GetAllContentInExperiment(e.experimentId);
                        //List<BatteryComponentExt> experimentContentList= BatteryComponentDa.GetAllContentInExperimentByResearchGroup(researchGroupId); //get all content in all experiments

                        List<ProcessResponse> experimentProcessResponseList = Helpers.GeneralHelper.GetAllExperimentProcessesWithSettings(e.experimentId);

                        allProcesses.AddRange(experimentProcessResponseList);

                        foreach (BatteryComponentExt bc in experimentContentList)
                        {
                            List<BatchContentExt> list = new List<BatchContentExt>();
                            //allContent.Add(bc);

                            if (bc.fkStepBatch != null)
                            {
                                List<BatchContentExt> allBatchContent = BatchContentDa.GetAllContentInBatchRecursive(list, (int)bc.fkStepBatch, 1, ignoredMaterialFunctionsArray);
                                allbatchesContentList.AddRange(allBatchContent);
                                //allContent.AddRange(allBatchContent);

                                //get all proceses in batch
                                List<BatchProcessResponse> result = Helpers.GeneralHelper.GetAllBatchProcessesWithSettings((int)bc.fkStepBatch);


                                foreach (BatchProcessResponse bpr in result)
                                {
                                    bpr.experimentId = e.experimentId;
                                    bpr.batteryComponentType = bc.batteryComponentType;
                                }
                                allProcesses.AddRange(result);

                                foreach (BatchContentExt b in allBatchContent)
                                {
                                    if (b.fkStepBatch != null)
                                    {
                                        List<BatchProcessResponse> resultB = Helpers.GeneralHelper.GetAllBatchProcessesWithSettings((int)b.fkStepBatch);
                                        foreach (BatchProcessResponse bpr in resultB)
                                        {
                                            bpr.experimentId = e.experimentId;
                                            bpr.batteryComponentType = bc.batteryComponentType;
                                        }
                                        allProcesses.AddRange(resultB);
                                    }
                                }
                            }
                        }
                    }                
                    //var a = JsonConvert.SerializeObject(allProcesses);
                    string resultString = "\"Experiment Id\",\"Battery Component Type\",\"Process\",\"Equipment\",\"Model\"";
                    resultString += System.Environment.NewLine;
                    foreach (dynamic item in allProcesses)
                    {
                        string id = "";
                        string batteryComponentType = "";
                        string process = "\"\"";
                        string equipment = "\"\"";
                        string equipmentModel = "\"\"";


                        Type classType = (Type)item.GetType();
                        if (classType.Name == "ProcessResponse")
                        {
                            ProcessResponse ep = (ProcessResponse)item;

                            id = ep.stepProcess.fkExperiment.ToString();
                            batteryComponentType = "\"" + ep.stepProcess.batteryComponentType + "\"";
                            process = "\"" + ep.stepProcess.processType + "\"";
                            equipment = "\"" + ep.stepProcess.equipmentName + "\"";
                            equipmentModel = "\"" + ep.stepProcess.equipmentModelName + "\"";
                        }
                        else
                        {
                            BatchProcessResponse bp = (BatchProcessResponse)item;

                            process = "\"" + bp.batchProcess.processType + "\"";
                            id = bp.experimentId.ToString();
                            batteryComponentType = "\"" + bp.batteryComponentType + "\"";
                            equipment = "\"" + bp.batchProcess.equipmentName + "\"";
                            equipmentModel = "\"" + bp.batchProcess.equipmentModelName + "\"";
                        }


                        ///*if (item.stepProcess.GetType().GetProperty("equipmentName") != null)
                        //{*/
                        //equipment = "\"" + item.stepProcess.equipmentName + "\"";
                        ////}
                        ////if (item.stepProcess.GetType().GetProperty("equipmentModelName") != null)
                        //    equipmentModel = "\"" + item.stepProcess.equipmentModelName + "\"";



                        resultString += id + "," + batteryComponentType + "," + process + "," + equipment + "," + equipmentModel;
                        resultString += System.Environment.NewLine;

                    }


                    context.Response.Clear();
                    context.Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                    context.Response.Write(resultString);
                    //context.Response.ContentType = "image/png";
                    //context.Response.TransmitFile(resultString);
                    context.Response.End();

                }
                else
                {
                    string resultString = "\"Experiment Id\",\"Battery Component Type\",\"Process\",\"Equipment\",\"Model\"";
                    resultString += System.Environment.NewLine;                   
                    context.Response.Clear();
                    context.Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                    context.Response.Write(resultString);
                    context.Response.End();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
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