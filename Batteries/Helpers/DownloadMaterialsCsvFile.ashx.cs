using Batteries.Dal;
using Batteries.Models.Responses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Batteries.Helpers
{
    /// <summary>
    /// Summary description for DownloadMaterialsCsvFile
    /// </summary>
    public class DownloadMaterialsCsvFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string fileAttachmentId = context.Request.QueryString["fileAttachmentId"];
            string folder = context.Request.QueryString["folder"];
            //string serverFilename = "";
            string filename = "MaterialsUsed_" + DateTime.Now.ToString("dd_MM_yyyy") + ".csv";

            //var file = Dal.FileAttachmentDa.GetFileAttachments(long.Parse(fileAttachmentId))[0];
            //serverFilename = file.serverFilename;
            //filename = file.filename;

            //string path = context.Server.MapPath("~/Uploads/FileAttachments/" + serverFilename);

            var currentUser = UserHelper.GetCurrentUser();
            int currentResearchGroupId = (int)currentUser.fkResearchGroup;

            int[] ignoredMaterialFunctionsArray = Helpers.GeneralHelper.GetMaterialFunctionsToIgnore();

            List<ExperimentExt> experimentGeneralDataList = ExperimentDa.GetAllExperimentsListForResearchGroup(currentResearchGroupId);

            //List<ProjectResearchGroupExt> projectResearchGroupGeneralDataList = ProjectResearchGroupDa.GetAllProjectRGroupsForMatList(researchGroupId);
            //ProjectResearchGroupExt projectGeneralData = new ProjectResearchGroupExt();
            //projectGeneralData = projectResearchGroupGeneralDataList[0];
            //int? projectId = projectGeneralData.fkProject;

            List<BatteryComponentExt> experimentContentList;
            List<BatchContentExt> allbatchesContentList;
            List<dynamic> allContent;

            experimentContentList = BatteryComponentDa.GetAllContentInExperimentByResearchGroup(currentResearchGroupId); //get all content in all experiments
            allbatchesContentList = new List<BatchContentExt>();
            allContent = new List<dynamic>();

            if (experimentContentList != null)
            {
                foreach (BatteryComponentExt bc in experimentContentList)
                {
                    List<BatchContentExt> list = new List<BatchContentExt>();

                    dynamic content = new ExpandoObject();
                    content.content = bc;
                    content.experimentId = bc.fkExperiment;
                    content.fkBatteryComponentType = bc.fkBatteryComponentType;
                    content.batteryComponentType = bc.batteryComponentType;

                    ExperimentExt experimentInfo = (ExperimentExt)(experimentGeneralDataList.Where(x => x.experimentId == bc.fkExperiment).ToList())[0];
                    content.experimentInfo = experimentInfo;

                    allContent.Add(content);

                    if (bc.fkStepBatch != null)
                    {
                        double usedWeight = double.Parse(bc.weight.ToString());
                        double batchOutput = double.Parse(bc.batchOutput.ToString());
                        double batchTotalWeight = BatchContentDa.GetBatchTotalWeight((int)bc.fkStepBatch, ignoredMaterialFunctionsArray);

                        double percentageUsed = (usedWeight / batchTotalWeight);

                        List<BatchContentExt> allBatchContent = BatchContentDa.GetAllContentInBatchRecursive(list, (int)bc.fkStepBatch, percentageUsed, ignoredMaterialFunctionsArray);
                        allbatchesContentList.AddRange(allBatchContent);

                        foreach (BatchContentExt batc in allBatchContent)
                        {
                            content = new ExpandoObject();
                            content.content = batc;
                            content.experimentId = bc.fkExperiment;
                            content.fkBatteryComponentType = bc.fkBatteryComponentType;
                            content.batteryComponentType = bc.batteryComponentType;
                            content.experimentInfo = experimentInfo;
                            allContent.Add(content);
                        }
                        //allContent.AddRange(allBatchContent);
                    }
                }
            
                foreach (dynamic i in allContent)
                {
                    dynamic c = i.content;

                    if (c.weight != null)
                        c.weight = Math.Round((double)c.weight, 5);
                }


                //var result = BatteryComponentDa.GetAllMaterialsInExperimentForCsv();

                //Object resultObject = new Object();
                string resultString = "\"Experiment Id\",\"Battery Component Type\",\"Material\",\"Prefab Type\",\"Prefab Model\",\"Weight\",\"Measurement Unit\",\"Function\",\"Percentage Of Active\",\"Time\",\"Width\",\"Length\",\"Conductivity\",\"Thickness\",\"Measured Weight\",\"Anode A.M.\",\"Anode A.M. percentage\",\"Cathode A.M.\",\"Cathode A.M. percentage\",\"Experiment personal label\",\"Experiment description\",\"Created by\"";
                resultString += System.Environment.NewLine;
                foreach (dynamic i in allContent)
                {
                    dynamic item = i.content;

                    string id = "";
                    string batteryComponentType = "";
                    string material = "";
                    string commercialComponentName = "";
                    string commercialComponentModel = "";
                    string weight = "";
                    string measurementUnit = "";
                    string functionInExperiment = "";
                    string percentageOfActive = "";

                    string measuredTime = "";
                    string measuredWidth = "";
                    string measuredLength = "";
                    string measuredConductivity = "";
                    string measuredThickness = "";
                    string measuredWeight = "";

                    //string anodeTotalActiveMaterial = "";
                    //string cathodeTotalActiveMaterial = "";

                    //Type. a = item.GetType().Attributes;
                    Type classType = (Type)item.GetType();
                    //if (item.GetType() == System.Reflection.TypeInfo.GetType("BatteryComponentExt"))
                    //{
                    //    var b = "";
                    //}
                    //if ((Type)item.GetType() == (Type)Batteries.Models.Responses.BatteryComponentExt)
                    //{
                    //    var c = "";
                    //}
                    //id = i.experimentId.ToString();
                    id = i.experimentInfo.experimentId.ToString();
                    batteryComponentType = "\"" + i.batteryComponentType + "\"";


                    double val = i.experimentInfo.anodeTotalActiveMaterials != "" ? double.Parse(i.experimentInfo.anodeTotalActiveMaterials) : 0;
                    string anodeTotalActiveMaterials = i.experimentInfo.anodeTotalActiveMaterials != "" ? Math.Round(double.Parse(i.experimentInfo.anodeTotalActiveMaterials), 5).ToString("N5", CultureInfo.InvariantCulture) : "\\";
                    //var anodeTotalActiveMaterials = i.experimentInfo.anodeTotalActiveMaterials != "" ? val.ToString("0.00000") : "\\";
                    string cathodeTotalActiveMaterials = i.experimentInfo.cathodeTotalActiveMaterials != "" ? Math.Round(double.Parse(i.experimentInfo.cathodeTotalActiveMaterials), 5).ToString("N5", CultureInfo.InvariantCulture) + "" : "\\";

                    string anodeActiveMaterials = i.experimentInfo.anodeActiveMaterials != "" ? "\"" + i.experimentInfo.anodeActiveMaterials + " = " + i.experimentInfo.anodeActivePercentages + " %" + "\"" : "\\";
                    string cathodeActiveMaterials = i.experimentInfo.cathodeActiveMaterials != "" ? "\"" + i.experimentInfo.cathodeActiveMaterials + " = " + i.experimentInfo.cathodeActivePercentages + " %" + "\"" : "\\";

                    //anodeActiveMaterials = "\"" + anodeActiveMaterials + "\"";
                    //cathodeActiveMaterials = "\"" + cathodeActiveMaterials + "\"";

                    functionInExperiment = item.materialFunction != "" ? "\"" + item.materialFunction + "\"" : "";
                    if (item.fkFunction != 1)
                    {
                        percentageOfActive = item.percentageOfActive != null ? "\"" + item.percentageOfActive + "\"" : "\\";
                    }
                    else
                    {
                        percentageOfActive = item.percentageOfActive != null ? "\"" + item.percentageOfActive + "\"" : "";
                    }

                    //if (classType.Name == "BatteryComponentExt")
                    //{
                    //    BatteryComponentExt bc = (BatteryComponentExt)item;
                    //    if (item.fkCommercialType != null) continue;
                    //}
                    //else if (classType.Name == "BatchContentExt")
                    //{
                    //    BatchContentExt bc = (BatchContentExt)item;
                    //    batteryComponentType = bc.batch != "" ? "\"" + bc.batteryComponentType + "\"" : ""; ;
                    //}

                    if (classType.Name == "BatteryComponentExt" && (item.fkCommercialType != null))
                    {
                        //continue;
                        int comTypeId = (int)item.fkCommercialType;
                        BatteryComponentCommercialTypeExt commercialType = BatteryComponentCommercialTypeDa.GetBatteryComponentCommercialTypes(comTypeId)[0];

                        commercialComponentName = "\"" + commercialType.batteryComponentCommercialType + "\"";
                        commercialComponentModel = "\"" + commercialType.model + "\"";
                    }
                    else
                    {
                        if (item.fkStepMaterial != null)
                        {
                            material = "\"" + item.materialName + "\"";
                        }
                        else
                        {
                            material = "\"" + item.batchSystemLabel + "\"";
                        }

                        //if (item.fkCommercialType == null)
                        //{
                        //    if (item.batteryComponentId != null)
                        //    {
                        //        batteryComponentType = item.batteryComponentType;
                        //        functionInExperiment = item.materialFunction;
                        //    }

                        //    if (item.fkStepMaterial != null)
                        //    {
                        //        material = item.materialName;
                        //    }
                        //    else
                        //    {
                        //        material = item.batchSystemLabel;
                        //    }
                        //}

                        //if (item.fkExperiment != null)
                        //    id = item.fkExperiment.ToString();

                        weight = item.weight.ToString(CultureInfo.InvariantCulture);
                    }

                    measurementUnit = "\"" + item.measurementUnitSymbol.ToString() + "\"";

                    if (item.measurements != null)
                    {
                        measuredTime = item.measurements.measuredTime != null ? item.measurements.measuredTime.ToString(CultureInfo.InvariantCulture) : "";
                        measuredWidth = item.measurements.measuredWidth != null ? item.measurements.measuredWidth.ToString(CultureInfo.InvariantCulture) : "";
                        measuredLength = item.measurements.measuredLength != null ? item.measurements.measuredLength.ToString(CultureInfo.InvariantCulture) : "";
                        measuredConductivity = item.measurements.measuredConductivity != null ? item.measurements.measuredConductivity.ToString(CultureInfo.InvariantCulture) : "";
                        measuredThickness = item.measurements.measuredThickness != null ? item.measurements.measuredThickness.ToString(CultureInfo.InvariantCulture) : "";
                        measuredWeight = item.measurements.measuredWeight != null ? item.measurements.measuredWeight.ToString(CultureInfo.InvariantCulture) : "";
                    }

                    string experimentDescription = "\"" + i.experimentInfo.experimentDescription + "\"";
                    string createdBy = "\"" + i.experimentInfo.operatorUsername + "\"";
                    string personalLabel = "\"" + i.experimentInfo.experimentPersonalLabel + "\"";

                    resultString += id + "," + batteryComponentType + "," + material + "," + commercialComponentName + "," + commercialComponentModel + "," + weight + "," + measurementUnit + "," + functionInExperiment + "," + percentageOfActive + "," + measuredTime + "," + measuredWidth + "," + measuredLength + "," + measuredConductivity + "," + measuredThickness + "," + measuredWeight + "," + anodeTotalActiveMaterials + "," + anodeActiveMaterials + "," + cathodeTotalActiveMaterials + "," + cathodeActiveMaterials + "," + personalLabel + "," + experimentDescription + "," + createdBy;
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
                string resultString = "\"Experiment Id\",\"Battery Component Type\",\"Material\",\"Prefab Type\",\"Prefab Model\",\"Weight\",\"Measurement Unit\",\"Function\",\"Percentage Of Active\",\"Time\",\"Width\",\"Length\",\"Conductivity\",\"Thickness\",\"Measured Weight\",\"Anode A.M.\",\"Anode A.M. percentage\",\"Cathode A.M.\",\"Cathode A.M. percentage\",\"Experiment personal label\",\"Experiment description\",\"Created by\"";
                resultString += System.Environment.NewLine;
                context.Response.Clear();
                context.Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                context.Response.Write(resultString);
                context.Response.End();
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