using Batteries.Dal;
using Batteries.Dal.EquipmentDal;
using Batteries.Dal.ProcessesDal;
using Batteries.Models;
using Batteries.Models.Responses;
using Batteries.Models.Responses.EquipmentModels;
using Batteries.Models.Responses.ProcessModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Batteries.Helpers
{
    public class GeneralHelper
    {
        public static int[] GetMaterialFunctionsToIgnore()
        {
            int[] array = new int[] { 6, 8, 11 };

            return array;
        }
        public static List<BatchProcessResponse> GetAllBatchProcessesWithSettings(int batchId)
        {
            try
            {
                //List<BatchContentExt> allBatchContent = BatchContentDa.GetAllContentInBatch(e.fkStepBatch);
                List<BatchProcessExt> allBatchProcesses = BatchProcessDa.GetAllBatchProcesses(batchId);
                List<BatchProcessResponse> processResponseList = new List<BatchProcessResponse>();

                if (allBatchProcesses != null)
                {

                    foreach (BatchProcessExt batchProcess in allBatchProcesses)
                    {
                        BatchProcessResponse processResponse = new BatchProcessResponse();
                        processResponse.batchProcess = batchProcess;

                        string processType = batchProcess.processType;
                        int processTypeId = (int)batchProcess.fkProcessType;
                        long batchProcessId = batchProcess.batchProcessId;

                        //FILL PROCESS ATTRIBUTES AND EQUIPMENT SETTINGS
                        List<EquipmentSettingsValue> processAttributes = GetProcessAttributesAndEquipmentSettings(null, null, batchProcessId);
                        processResponse.processAttributes = processAttributes;
                        //processResponse.equipmentSettings = processAttributesAndEquipment.equipmentSettings;

                        processResponseList.Add(processResponse);
                    }

                    //allbatchesProcessList.AddRange(allBatchProcesses);
                    //allProcesses.AddRange(allBatchProcesses);
                    //allProcesses.AddRange(processResponseList);

                }

                return processResponseList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<ProcessResponse> GetAllExperimentProcessesWithSettings(int experimentId)
        {
            try
            {
                List<ExperimentProcessExt> allStepsProcessList = ExperimentProcessDa.GetAllExperimentProcesses(null, experimentId, null, null, null);
                List<ProcessResponse> processResponseList = new List<ProcessResponse>();

                if (allStepsProcessList != null)
                {

                    foreach (ExperimentProcessExt experimentProcess in allStepsProcessList)
                    {
                        ProcessResponse processResponse = new ProcessResponse();
                        processResponse.stepProcess = experimentProcess;

                        string processType = experimentProcess.processType;
                        int processTypeId = (int)experimentProcess.fkProcessType;
                        long experimentProcessId = experimentProcess.experimentProcessId;


                        //FILL PROCESS ATTRIBUTES AND EQUIPMENT SETTINGS         //fkExperiment, experimentProcessId, batchProcessId
                        List<EquipmentSettingsValue> processAttributes = GetProcessAttributesAndEquipmentSettings(experimentId, experimentProcessId, null);
                        processResponse.processAttributes = processAttributes;
                        //processResponse.equipmentSettings = processAttributesAndEquipment.equipmentSettings;

                        processResponseList.Add(processResponse);
                    }

                    //allbatchesProcessList.AddRange(allBatchProcesses);
                    //allProcesses.AddRange(allBatchProcesses);
                    //allProcesses.AddRange(processResponseList);

                }

                return processResponseList;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public static List<ExpandoObject> GetExperimentsSummaryList(int[] experimentIds)
        {
            List<ExpandoObject> result = new List<ExpandoObject>();
            dynamic experiment = new ExpandoObject();

            foreach (int experimentId in experimentIds)
            {
                experiment = new ExpandoObject();
                List<ExperimentExt> experimentGeneralDataList = ExperimentDa.GetAllCompleteExperimentsGeneralData(experimentId);
                ExperimentExt experimentGeneralData = new ExperimentExt();
                if (experimentGeneralDataList != null)
                {
                    experimentGeneralData = experimentGeneralDataList[0];
                    experiment.experimentInfo = experimentGeneralData;
                }

                List<ExperimentSummaryExt> experimentSummaryList = ExperimentSummaryDa.GetExperimentSummary(experimentId);

                dynamic calculations;
                calculations = new ExpandoObject();
                ExperimentSummaryExt anode = (ExperimentSummaryExt)(experimentSummaryList.Where(x => x.fkBatteryComponentType == 1).ToList())[0];
                ExperimentSummaryExt cathode = (ExperimentSummaryExt)(experimentSummaryList.Where(x => x.fkBatteryComponentType == 2).ToList())[0];
                ExperimentSummaryExt separator = (ExperimentSummaryExt)(experimentSummaryList.Where(x => x.fkBatteryComponentType == 3).ToList())[0];
                ExperimentSummaryExt electrolyte = (ExperimentSummaryExt)(experimentSummaryList.Where(x => x.fkBatteryComponentType == 4).ToList())[0];
                ExperimentSummaryExt referenceElectrode = (ExperimentSummaryExt)(experimentSummaryList.Where(x => x.fkBatteryComponentType == 5).ToList())[0];
                ExperimentSummaryExt casing = (ExperimentSummaryExt)(experimentSummaryList.Where(x => x.fkBatteryComponentType == 6).ToList())[0];

                calculations.anode = anode;
                calculations.cathode = cathode;
                calculations.separator = separator;
                calculations.electrolyte = electrolyte;
                calculations.referenceElectrode = referenceElectrode;
                calculations.casing = casing;

                experiment.calculations = calculations;

                result.Add(experiment);
            }

            return result;
        }
        public static List<ExpandoObject> GetBatchSummaryList(int[] batchIds, int? researchGroupId = null)
        {
            List<ExpandoObject> result = new List<ExpandoObject>();
            dynamic batch = new ExpandoObject();

            foreach (int batchId in batchIds)
            {
                batch = new ExpandoObject();
                List<BatchExt> batchGeneralDataList = BatchDa.GetAllCompleteBatchGeneralData(batchId, researchGroupId);
                BatchExt batchGeneralData = new BatchExt();
                if (batchGeneralDataList != null)
                {
                    batchGeneralData = batchGeneralDataList[0];
                    batch.batchInfo = batchGeneralData;
                }
                result.Add(batch);
            }

            return result;
        }
        public static dynamic GetMassCalculations(int? experimentId)
        {
            List<ExperimentSummaryExt> experimentSummaryList = ExperimentSummaryDa.GetExperimentSummary(experimentId);

            dynamic calculations;
            string calculationsJsonString;

            calculations = new ExpandoObject();

            ExperimentSummaryExt anode = (ExperimentSummaryExt)(experimentSummaryList.Where(x => x.fkBatteryComponentType == 1).ToList())[0];
            ExperimentSummaryExt cathode = (ExperimentSummaryExt)(experimentSummaryList.Where(x => x.fkBatteryComponentType == 2).ToList())[0];
            ExperimentSummaryExt separator = (ExperimentSummaryExt)(experimentSummaryList.Where(x => x.fkBatteryComponentType == 3).ToList())[0];
            ExperimentSummaryExt electrolyte = (ExperimentSummaryExt)(experimentSummaryList.Where(x => x.fkBatteryComponentType == 4).ToList())[0];
            ExperimentSummaryExt referenceElectrode = (ExperimentSummaryExt)(experimentSummaryList.Where(x => x.fkBatteryComponentType == 5).ToList())[0];
            ExperimentSummaryExt casing = (ExperimentSummaryExt)(experimentSummaryList.Where(x => x.fkBatteryComponentType == 6).ToList())[0];

            //dynamic commercialType = new ExpandoObject();

            //anode = GetMassCalculationsForComponent(allAnodeContent);
            //cathode = GetMassCalculationsForComponent(allCathodeContent);
            //separator = GetMassCalculationsForComponent(allSeparatorContent);
            //electrolyte = GetMassCalculationsForComponent(allElectrolyteContent);
            //referenceElectrode = GetMassCalculationsForComponent(allReferenceElectrodeContent);
            //casing = GetMassCalculationsForComponent(allCasingContent);

            double mass1 = anode.totalActiveMaterials ?? 0;
            double mass2 = anode.totalWeight ?? 0;
            double mass3 = cathode.totalActiveMaterials ?? 0;
            double mass4 = cathode.totalWeight ?? 0;
            double mass5 = anode.totalWeight ?? 0 + cathode.totalWeight ?? 0 + electrolyte.totalWeight ?? 0;
            double mass6 = anode.totalWeight ?? 0 + cathode.totalWeight ?? 0 + electrolyte.totalWeight ?? 0 + casing.totalWeight ?? 0;

            //calculations.mass1 = Math.Round(mass1, 5);
            //calculations.mass2 = Math.Round(mass2, 5);
            //calculations.mass3 = Math.Round(mass3, 5);
            //calculations.mass4 = Math.Round(mass4, 5);
            //calculations.mass5 = Math.Round(mass5, 5);
            //calculations.mass6 = Math.Round(mass6, 5);
            calculations.mass1 = mass1;
            calculations.mass2 = mass2;
            calculations.mass3 = mass3;
            calculations.mass4 = mass4;
            calculations.mass5 = mass5;
            calculations.mass6 = mass6;

            return calculations;
        }

        public static dynamic GetMassCalculationsForComponent(List<dynamic> allComponentContent)
        {
            //active materials total weight
            //total weight of component
            int[] ignoredMaterialFunctionsArray = GetMaterialFunctionsToIgnore();

            dynamic calculations = new ExpandoObject();

            double total = 0;
            double totalActiveMaterials = 0;

            if (allComponentContent.Count > 0)
            {
                //allComponentContent[0].GetType().GetProperty("fkCommercialType")
                //var a = allComponentContent[0].fkCommercialType;

                if (allComponentContent.Count() == 1 && allComponentContent[0].fkCommercialType != null)
                {
                    calculations.totalWeight = 0;
                    calculations.totalActiveMaterials = 0;
                }
                else
                {
                    double totalLabeled = 0;

                    foreach (dynamic content in allComponentContent)
                    {
                        if (content.fkStepMaterial != null)
                        {
                            if (content.fkFunction != null)
                            {
                                if (!ignoredMaterialFunctionsArray.Contains((int)content.fkFunction))
                                {
                                    //Solvent does not go in the total calculations 
                                    total += content.weight;
                                    totalLabeled += content.weight;
                                }
                                if (content.fkFunction == 1)
                                {
                                    double poa = 1;
                                    if (content.percentageOfActive != null)
                                        poa = content.percentageOfActive / 100;

                                    totalActiveMaterials += (content.weight * poa);
                                }

                            }
                            else
                            {
                                //if no function assigned
                                //total += content.weight;
                            }
                        }

                    }
                    //calculations.totalWeight = Math.Round((double)total, 3).ToString(CultureInfo.InvariantCulture) + " g";
                    //calculations.totalActiveMaterials = Math.Round(totalActiveMaterials, 3).ToString(CultureInfo.InvariantCulture) + " g";
                    calculations.totalWeight = total;
                    calculations.totalActiveMaterials = totalActiveMaterials;

                }
            }
            else
            {
                calculations.totalWeight = 0;
                calculations.totalActiveMaterials = 0;
            }

            return calculations;
        }

        public static dynamic GetExperimentSummary(int experimentId)
        {
            int[] ignoredMaterialFunctionsArray = GetMaterialFunctionsToIgnore();
            List<BatteryComponentExt> experimentContentList;
            List<BatchContentExt> allbatchesContentList;
            List<dynamic> allContent;

            string allContentJsonString;
            List<ExperimentProcessExt> experimentProcessList;
            List<BatchProcessExt> allbatchesProcessList;
            List<dynamic> allProcesses;

            string allProcessesJsonString;


            List<dynamic> allAnodeContent = new List<dynamic>();
            List<dynamic> allCathodeContent = new List<dynamic>();
            List<dynamic> allSeparatorContent = new List<dynamic>();
            List<dynamic> allElectrolyteContent = new List<dynamic>();
            List<dynamic> allReferenceElectrodeContent = new List<dynamic>();
            List<dynamic> allCasingContent = new List<dynamic>();



            experimentContentList = BatteryComponentDa.GetAllContentInExperiment(experimentId, null, null);

            allbatchesContentList = new List<BatchContentExt>();
            allContent = new List<dynamic>();

            allbatchesProcessList = new List<BatchProcessExt>();
            allProcesses = new List<dynamic>();

            List<ProcessResponse> experimentProcessResponseList = GetAllExperimentProcessesWithSettings(experimentId);
            allProcesses.AddRange(experimentProcessResponseList);
            if (experimentContentList != null)
            {
                foreach (BatteryComponentExt bc in experimentContentList)
                {
                    List<BatchContentExt> list = new List<BatchContentExt>();

                    dynamic content = new ExpandoObject();
                    content.content = bc;
                    content.fkBatteryComponentType = bc.fkBatteryComponentType;
                    content.batteryComponentType = bc.batteryComponentType;

                    allContent.Add(content);

                    switch ((int)bc.fkBatteryComponentType)
                    {
                        case 1:
                            //componentType = "Anode";
                            allAnodeContent.Add(bc);
                            break;
                        case 2:
                            //componentType = "Cathode";
                            allCathodeContent.Add(bc);
                            break;
                        case 3:
                            //componentType = "Separator";
                            allSeparatorContent.Add(bc);
                            break;
                        case 4:
                            //componentType = "Electrolyte";
                            allElectrolyteContent.Add(bc);
                            break;
                        case 5:
                            //componentType = "ReferenceElectrode";
                            allReferenceElectrodeContent.Add(bc);
                            break;
                        case 6:
                            //componentType = "Casing";
                            allCasingContent.Add(bc);
                            break;
                    }
                    if (bc.fkStepBatch != null)
                    {
                        double usedWeight = double.Parse(bc.weight.ToString());
                        //double batchOutput = double.Parse(bc.batchOutput.ToString());
                        double batchTotalWeight = BatchContentDa.GetBatchTotalWeight((int)bc.fkStepBatch, ignoredMaterialFunctionsArray);
                        double percentageUsed = batchTotalWeight > 0 ? (usedWeight / batchTotalWeight) : 0;

                        List<BatchContentExt> allBatchContent = BatchContentDa.GetAllContentInBatchRecursive(list, (int)bc.fkStepBatch, percentageUsed, ignoredMaterialFunctionsArray);
                        allbatchesContentList.AddRange(allBatchContent);


                        foreach (BatchContentExt batc in allBatchContent)
                        {
                            content = new ExpandoObject();
                            content.content = batc;
                            content.fkBatteryComponentType = bc.fkBatteryComponentType;
                            content.batteryComponentType = bc.batteryComponentType;
                            allContent.Add(content);
                        }
                        //allContent.AddRange(allBatchContent);

                        switch ((int)bc.fkBatteryComponentType)
                        {
                            case 1:
                                allAnodeContent.AddRange(allBatchContent);
                                break;
                            case 2:
                                allCathodeContent.AddRange(allBatchContent);
                                break;
                            case 3:
                                allSeparatorContent.AddRange(allBatchContent);
                                break;
                            case 4:
                                allElectrolyteContent.AddRange(allBatchContent);
                                break;
                            case 5:
                                allReferenceElectrodeContent.Add(allBatchContent);
                                break;
                            case 6:
                                allCasingContent.AddRange(allBatchContent);
                                break;
                        }
                        //get all processes in batches in experiment
                        List<BatchProcessResponse> result = Helpers.GeneralHelper.GetAllBatchProcessesWithSettings((int)bc.fkStepBatch);

                        foreach (BatchProcessResponse bpr in result)
                        {
                            bpr.batteryComponentType = bc.batteryComponentType;
                        }
                        allProcesses.AddRange(result);

                        foreach (BatchContentExt batc in allBatchContent)
                        {
                            if (batc.fkStepBatch != null)
                            {
                                List<BatchProcessResponse> resultB = Helpers.GeneralHelper.GetAllBatchProcessesWithSettings((int)bc.fkStepBatch);
                                foreach (BatchProcessResponse bpr in resultB)
                                {
                                    bpr.batteryComponentType = bc.batteryComponentType;
                                }
                                allProcesses.AddRange(resultB);
                            }
                        }
                    }
                }
            }
            //allContentJsonString = JsonConvert.SerializeObject(allContent);

            //bach in the previous for to Display Component Name
            //get all processes in batches inside batches
            //foreach (BatchContentExt bc in allbatchesContentList)
            //{
            //    if (bc.fkStepBatch != null)
            //    {
            //        List<BatchProcessResponse> result = Helpers.GeneralHelper.GetAllBatchProcessesWithSettings((int)bc.fkStepBatch);

            //        allProcesses.AddRange(result);
            //    }
            //}

            //allProcessesJsonString = JsonConvert.SerializeObject(allProcesses);


            //CALCULATIONS
            dynamic calculations;
            string calculationsJsonString;

            calculations = new ExpandoObject();
            dynamic anode = new ExpandoObject();
            dynamic cathode = new ExpandoObject();
            dynamic separator = new ExpandoObject();
            dynamic electrolyte = new ExpandoObject();
            dynamic referenceElectrode = new ExpandoObject();
            dynamic casing = new ExpandoObject();

            dynamic commercialType = new ExpandoObject();

            anode = GetAllCalculationsForComponent(allAnodeContent, 1);
            cathode = GetAllCalculationsForComponent(allCathodeContent, 2);
            separator = GetAllCalculationsForComponent(allSeparatorContent, 3);
            electrolyte = GetAllCalculationsForComponent(allElectrolyteContent, 4);
            referenceElectrode = GetAllCalculationsForComponent(allReferenceElectrodeContent, 5);
            casing = GetAllCalculationsForComponent(allCasingContent, 6);

            calculations.anode = anode;
            calculations.cathode = cathode;
            calculations.separator = separator;
            calculations.electrolyte = electrolyte;
            calculations.referenceElectrode = referenceElectrode;
            calculations.casing = casing;



            double mass1 = anode.totalActiveMaterials;
            double mass2 = anode.totalWeight;
            double mass3 = cathode.totalActiveMaterials;
            double mass4 = cathode.totalWeight;
            double mass5 = anode.totalWeight + cathode.totalWeight + electrolyte.totalWeight;
            double mass6 = anode.totalWeight + cathode.totalWeight + electrolyte.totalWeight + casing.totalWeight;

            //calculations.mass1 = Math.Round(mass1, 5);
            //calculations.mass2 = Math.Round(mass2, 5);
            //calculations.mass3 = Math.Round(mass3, 5);
            //calculations.mass4 = Math.Round(mass4, 5);
            //calculations.mass5 = Math.Round(mass5, 5);
            //calculations.mass6 = Math.Round(mass6, 5);
            calculations.mass1 = mass1;
            calculations.mass2 = mass2;
            calculations.mass3 = mass3;
            calculations.mass4 = mass4;
            calculations.mass5 = mass5;
            calculations.mass6 = mass6;


            dynamic resultObject = new ExpandoObject();
            dynamic summary = new ExpandoObject();
            summary.allContent = allContent;
            summary.allProcesses = allProcesses;
            resultObject.summary = summary;
            resultObject.calculations = calculations;

            return resultObject;
        }
        public static dynamic GetAllCalculationsForComponent(List<dynamic> allComponentContent, int componentTypeId, bool withActive = true)
        {
            int[] ignoredMaterialFunctionsArray = GetMaterialFunctionsToIgnore();

            dynamic calculations = new ExpandoObject();
            calculations.componentTypeId = componentTypeId;

            dynamic commercialType = new ExpandoObject();
            int? fkCommercialType = null;
            double total = 0;
            double totalActiveMaterials = 0;
            double activeMaterialsPercentage = 0;

            if (allComponentContent.Count > 0)
            {

                if (allComponentContent.Count() == 1 && allComponentContent[0].fkCommercialType != null)
                {
                    calculations.totalWeight = 0;
                    calculations.totalActiveMaterials = 0;

                    BatteryComponentCommercialTypeExt comType = BatteryComponentCommercialTypeDa.GetBatteryComponentCommercialTypes((int)allComponentContent[0].fkCommercialType)[0];
                    commercialType.fkCommercialType = ((int)comType.batteryComponentCommercialTypeId);
                    commercialType.commercialType = comType.batteryComponentCommercialType;
                    calculations.commercialType = commercialType;
                }
                else
                {
                    double totalLabeled = 0;
                    string materials = "";
                    string percentages = "";

                    string activeMaterials = "";
                    string activePercentages = "";

                    foreach (dynamic content in allComponentContent)
                    {
                        if (content.fkStepMaterial != null)
                        {
                            if (content.fkFunction != null)
                            {
                                if (!ignoredMaterialFunctionsArray.Contains((int)content.fkFunction))
                                {
                                    total += content.weight;
                                    totalLabeled += content.weight;
                                }
                                if (content.fkFunction == 1)
                                {
                                    double poa = 1;
                                    if (content.percentageOfActive != null)
                                        poa = content.percentageOfActive / 100;

                                    totalActiveMaterials += (content.weight * poa);
                                }

                            }
                            else
                            {
                                //if no function assigned
                                //total += content.weight;
                            }
                        }

                    }
                    //calculations.totalWeight = Math.Round((double)total, 3).ToString(CultureInfo.InvariantCulture) + " g";
                    //calculations.totalActiveMaterials = Math.Round(totalActiveMaterials, 3).ToString(CultureInfo.InvariantCulture) + " g";
                    //calculations.totalActiveMaterialsPercentage = Math.Round(activeMaterialsPercentage, 1).ToString(CultureInfo.InvariantCulture) + " %";

                    if (total != 0)
                        activeMaterialsPercentage = (totalActiveMaterials / total) * 100;

                    calculations.totalWeight = total;
                    calculations.totalActiveMaterials = totalActiveMaterials;
                    calculations.totalActiveMaterialsPercentage = activeMaterialsPercentage;
                    //DO ROUNDING IN JAVASCRIPT ON DISPLAY


                    foreach (dynamic content in allComponentContent)
                    {
                        if (content.fkStepMaterial != null && content.fkFunction != null)
                        {
                            if (!ignoredMaterialFunctionsArray.Contains((int)content.fkFunction))
                            {
                                materials += materials == "" ? content.chemicalFormula : " : " + content.chemicalFormula;
                                double percentageNumber = totalLabeled != 0 ? (content.weight / totalLabeled) : 0;
                                string percentage = Math.Round((percentageNumber) * 100, 2).ToString(CultureInfo.InvariantCulture);
                                percentages += percentages == "" ? percentage : " : " + percentage;
                            }
                            if (content.fkFunction == 1)
                            {
                                double poa = 1;
                                if (content.percentageOfActive != null)
                                    poa = content.percentageOfActive / 100;

                                activeMaterials += activeMaterials == "" ? content.chemicalFormula : " : " + content.chemicalFormula;
                                double activePercentageNumber = totalActiveMaterials != 0 ? ((content.weight * poa) / totalActiveMaterials) : 0;
                                string activePercentage = Math.Round((activePercentageNumber) * 100, 2).ToString(CultureInfo.InvariantCulture);
                                activePercentages += activePercentages == "" ? activePercentage : " : " + activePercentage;
                            }
                        }
                    }
                    if (percentages == "")
                        percentages += "0";
                    //percentages += "0 %";
                    //else
                    //    percentages += " %";

                    if (activePercentages == "")
                        activePercentages += "0";
                    //activePercentages += "0 %";
                    //else
                    //    activePercentages += " %";

                    calculations.activeMaterialsPercentages = activeMaterials + " = " + activePercentages;
                    calculations.activeMaterials = activeMaterials;
                    calculations.activePercentages = activePercentages;
                    //calculations.totalLabeledMaterials = Math.Round(totalLabeled, 3).ToString(CultureInfo.InvariantCulture) + " g";
                    calculations.totalLabeledMaterials = totalLabeled;
                    calculations.labeledMaterialsPercentages = materials + " = " + percentages;

                    calculations.labeledMaterials = materials;
                    calculations.labeledPercentages = percentages;
                }
                calculations.componentEmpty = false;
            }
            else
            {
                calculations.componentEmpty = true;
                calculations.totalWeight = 0;
                calculations.totalActiveMaterials = 0;
            }

            return calculations;
        }

        public static BatteryComponentResponse GetExperimentComponentWithContent(int experimentId, int componentTypeId)
        {
            try
            {
                string componentType = "";
                switch (componentTypeId)
                {
                    case 1:
                        componentType = "Anode";
                        break;
                    case 2:
                        componentType = "Cathode";
                        break;
                    case 3:
                        componentType = "Separator";
                        break;
                    case 4:
                        componentType = "Electrolyte";
                        break;
                    case 5:
                        componentType = "ReferenceElectrode";
                        break;
                    case 6:
                        componentType = "Casing";
                        break;
                }
                List<BatteryComponentStepResponse> batteryComponentStepResponseList = new List<BatteryComponentStepResponse>();

                List<BatteryComponentExt> allStepsContentList = BatteryComponentDa.GetAllBatteryComponents(null, experimentId, componentTypeId);
                List<BatteryComponentExt> stepContentList = new List<BatteryComponentExt>();
                List<ExperimentProcessExt> allStepsProcessList = ExperimentProcessDa.GetAllExperimentProcesses(null, experimentId, componentTypeId);


                List<ExperimentProcessExt> stepProcessList = new List<ExperimentProcessExt>();
                List<dynamic> stepProcessAttributesList = new List<dynamic>();

                List<ProcessResponse> processResponseList = new List<ProcessResponse>();
                //ProcessResponse processResponse = new ProcessResponse();
                List<List<ProcessResponse>> processResponseListByStep = new List<List<ProcessResponse>>();

                //List<List<ExperimentProcessExt>> processListByStep = new List<List<ExperimentProcessExt>>();


                int previousStep = 0;
                int currentStep = 0;
                if (allStepsProcessList != null)
                {
                    foreach (ExperimentProcessExt experimentProcess in allStepsProcessList)
                    {

                        currentStep = int.Parse(experimentProcess.step.ToString());
                        if (previousStep == 0)
                        {
                            previousStep = currentStep;
                        }
                        ProcessResponse processResponse = new ProcessResponse();
                        processResponse.stepProcess = experimentProcess;

                        //FILL PROCESS ATTRIBUTES AND EQUIPMENT SETTINGS
                        string processType = experimentProcess.processType;
                        int processTypeId = (int)experimentProcess.fkProcessType;
                        long experimentProcessId = experimentProcess.experimentProcessId;
                        int fkExperiment = (int)experimentProcess.fkExperiment;

                        List<EquipmentSettingsValue> processAttributes = GetProcessAttributesAndEquipmentSettings(fkExperiment, experimentProcessId, null);
                        processResponse.processAttributes = processAttributes;
                        //processResponse.equipmentSettings = processAttributesAndEquipment.equipmentSettings;

                        //moze i step number

                        if (currentStep == previousStep)
                        {
                            //stepProcessList.Add(experimentProcess);                                
                            //processResponse.stepProcess = experimentProcess;                                
                            processResponseList.Add(processResponse);


                            if (allStepsProcessList.IndexOf(experimentProcess) == allStepsProcessList.Count - 1)
                            {
                                //processListByStep.Add(stepProcessList);
                                processResponseListByStep.Add(processResponseList);
                                //ne treba clear
                                //processResponseList = new List<ProcessResponse>();
                            }
                        }
                        else
                        {
                            //processListByStep.Add(stepProcessList);
                            processResponseListByStep.Add(processResponseList);
                            //stepProcessList = new List<ExperimentProcessExt>();
                            processResponseList = new List<ProcessResponse>();
                            //stepProcessList.Add(experimentProcess);
                            processResponseList.Add(processResponse);
                            if (allStepsProcessList.IndexOf(experimentProcess) == allStepsProcessList.Count - 1)
                            {
                                //processListByStep.Add(stepProcessList);
                                processResponseListByStep.Add(processResponseList);
                            }
                        }
                        previousStep = currentStep;
                    }
                }


                previousStep = 0;
                currentStep = 0;
                BatteryComponentResponse batteryComponentResponse = new BatteryComponentResponse();

                if (allStepsContentList != null)
                {
                    foreach (BatteryComponentExt batteryComponent in allStepsContentList)
                    {
                        if (batteryComponent.fkCommercialType != null)
                        {
                            int comTypeId = (int)batteryComponent.fkCommercialType;
                            batteryComponentResponse.batteryComponentSteps = null;
                            batteryComponentResponse.isCommercialType = true;
                            BatteryComponentCommercialTypeExt commercialType = BatteryComponentCommercialTypeDa.GetBatteryComponentCommercialTypes(comTypeId)[0];
                            batteryComponentResponse.commercialType = commercialType;
                            continue;
                        }
                        else
                        {
                            bool isSavedAsBatch = false;
                            currentStep = int.Parse(batteryComponent.step.ToString());
                            if (batteryComponent.isSavedAsBatch == true)
                            {
                                isSavedAsBatch = true;
                            }
                            if (previousStep == 0)
                            {
                                previousStep = currentStep;
                            }
                            if (currentStep == previousStep)
                            {
                                stepContentList.Add(batteryComponent);
                                if (allStepsContentList.IndexOf(batteryComponent) == allStepsContentList.Count - 1)
                                {
                                    BatteryComponentStepResponse batteryComponentStepResponse = new BatteryComponentStepResponse();
                                    batteryComponentStepResponse.stepNumber = currentStep;
                                    batteryComponentStepResponse.stepContent = stepContentList;
                                    //batteryComponentStepResponse.stepProcesses = processListByStep[currentStep - 1];
                                    //batteryComponentStepResponse.stepProcesses = processResponseListByStep[currentStep - 1];

                                    if (isSavedAsBatch == true)
                                    {
                                        batteryComponentStepResponse.isSavedAsBatch = true;
                                        isSavedAsBatch = false;
                                    }
                                    MeasurementsExt measurements = MeasurementsDa.GetMeasurementsStepLevel(experimentId, null, componentTypeId, batteryComponentStepResponse.stepNumber);
                                    batteryComponentStepResponse.measurements = measurements;
                                    batteryComponentStepResponseList.Add(batteryComponentStepResponse);
                                    //stepContentList.Clear();
                                }
                            }
                            else
                            {
                                BatteryComponentStepResponse batteryComponentStepResponse = new BatteryComponentStepResponse();
                                batteryComponentStepResponse.stepNumber = previousStep;
                                batteryComponentStepResponse.stepContent = stepContentList;
                                //batteryComponentStepResponse.stepProcesses = processResponseListByStep[previousStep - 1];

                                if (isSavedAsBatch == true)
                                {
                                    batteryComponentStepResponse.isSavedAsBatch = true;
                                    isSavedAsBatch = false;
                                }

                                MeasurementsExt measurements = MeasurementsDa.GetMeasurementsStepLevel(experimentId, null, componentTypeId, batteryComponentStepResponse.stepNumber);
                                batteryComponentStepResponse.measurements = measurements;
                                batteryComponentStepResponseList.Add(batteryComponentStepResponse);
                                //stepContentList.Clear();
                                stepContentList = new List<BatteryComponentExt>();
                                stepContentList.Add(batteryComponent);
                                if (allStepsContentList.IndexOf(batteryComponent) == allStepsContentList.Count - 1)
                                {
                                    batteryComponentStepResponse = new BatteryComponentStepResponse();
                                    batteryComponentStepResponse.stepNumber = currentStep;
                                    batteryComponentStepResponse.stepContent = stepContentList;
                                    //batteryComponentStepResponse.stepProcesses = processResponseListByStep[currentStep - 1];

                                    if (isSavedAsBatch == true)
                                    {
                                        batteryComponentStepResponse.isSavedAsBatch = true;
                                        isSavedAsBatch = false;
                                    }

                                    measurements = new MeasurementsExt();
                                    measurements = MeasurementsDa.GetMeasurementsStepLevel(experimentId, null, componentTypeId, batteryComponentStepResponse.stepNumber);
                                    batteryComponentStepResponse.measurements = measurements;
                                    batteryComponentStepResponseList.Add(batteryComponentStepResponse);
                                    //stepContentList.Clear();
                                    //stepContentList = new List<BatteryComponentExt>();
                                }
                            }
                            previousStep = currentStep;

                        }
                    }
                }

                //FILL PROCESSES in steps (check for step without process or with only processes)
                foreach (List<ProcessResponse> pList in processResponseListByStep)
                {
                    var stepList = batteryComponentStepResponseList.Where(x => x.stepNumber == pList[0].stepProcess.step).ToList();
                    if (stepList.Count > 0)
                    {
                        var step = stepList[0];
                        step.stepProcesses = pList;
                    }
                    else
                    {
                        //it's a step that contains only processes
                        var bcStepResponse = new BatteryComponentStepResponse();
                        bcStepResponse.stepProcesses = pList;
                        bcStepResponse.stepNumber = pList[0].stepProcess.step.Value;

                        MeasurementsExt measurements = MeasurementsDa.GetMeasurementsStepLevel(experimentId, null, componentTypeId, pList[0].stepProcess.step.Value);
                        bcStepResponse.measurements = measurements;

                        batteryComponentStepResponseList.Add(bcStepResponse);
                    }
                }


                batteryComponentResponse.batteryComponentSteps = batteryComponentStepResponseList;
                batteryComponentResponse.componentType = componentType;

                MeasurementsExt measurementsComponent = MeasurementsDa.GetMeasurementsComponentLevel(experimentId, null, componentTypeId);
                batteryComponentResponse.measurements = measurementsComponent;


                return batteryComponentResponse;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static BatchResponse GetBatchWithContent(int batchId)
        {
            var currentUser = UserHelper.GetCurrentUser();
            int myResearchGroupId = (int)currentUser.fkResearchGroup;

            try
            {
                BatchResponse batch = new BatchResponse();
                //BatchExt batch = BatchDa.GetBatchWithContent(batchId, researchGroupId);
                BatchExt batchGeneralInfo = new BatchExt();

                //Does not matter what research group
                //List<BatchExt> batchGeneralInfoList = BatchDa.GetAllCompleteBatchesWithQuantity(myResearchGroupId, batchId);

                //myResearchGroup only for stock
                List<BatchExt> batchGeneralInfoList = BatchDa.GetCompleteBatchesWithQuantityNoRG(myResearchGroupId, batchId);

                if (batchGeneralInfoList != null)
                    batchGeneralInfo = batchGeneralInfoList[0];
                else return null;

                batch.batchInfo = batchGeneralInfo;

                List<BatchContentExt> batchContentList = BatchContentDa.GetAllBatchContents(batchId);
                List<BatchProcessExt> batchProcessList = BatchProcessDa.GetAllBatchProcesses(batchId);

                //List<dynamic> batchProcessAttributesList = new List<dynamic>();
                List<BatchProcessResponse> processResponseList = new List<BatchProcessResponse>();

                foreach (BatchProcessExt batchProcess in batchProcessList)
                {
                    BatchProcessResponse processResponse = new BatchProcessResponse();
                    processResponse.batchProcess = batchProcess;

                    string processType = batchProcess.processType;
                    int processTypeId = (int)batchProcess.fkProcessType;
                    long batchProcessId = batchProcess.batchProcessId;

                    //FILL PROCESS ATTRIBUTES AND EQUIPMENT SETTINGS
                    List<EquipmentSettingsValue> processAttributes = GetProcessAttributesAndEquipmentSettings(null, null, batchProcessId);
                    processResponse.processAttributes = processAttributes;
                    //processResponse.equipmentSettings = processAttributesAndEquipment.equipmentSettings;

                    processResponseList.Add(processResponse);
                }


                MeasurementsExt measurements = MeasurementsDa.GetMeasurementsBatchLevel(batchId);
                batch.measurements = measurements;

                batch.batchContent = batchContentList;
                batch.batchProcesses = processResponseList;

                //if (batch != null)
                //{
                //    return JsonConvert.SerializeObject(batch, jsonSettings);
                //}
                //else return JsonConvert.SerializeObject(empty, jsonSettings);
                //return JsonConvert.SerializeObject(batch, jsonSettings);
                return batch;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static BatchResponse GetBatchInProgressWithContent(int batchId)
        {
            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;

            try
            {
                BatchResponse batch = new BatchResponse();
                BatchExt batchGeneralInfo = BatchDa.GetAllBatchesGeneralData(batchId, researchGroupId)[0];
                batch.batchInfo = batchGeneralInfo;

                List<BatchContentExt> batchContentList = BatchContentDa.GetAllBatchContents(batchId);
                List<BatchProcessExt> batchProcessList = BatchProcessDa.GetAllBatchProcesses(batchId);

                //List<dynamic> batchProcessAttributesList = new List<dynamic>();
                List<BatchProcessResponse> processResponseList = new List<BatchProcessResponse>();

                if (batchProcessList != null)
                {
                    foreach (BatchProcessExt batchProcess in batchProcessList)
                    {
                        BatchProcessResponse processResponse = new BatchProcessResponse();
                        processResponse.batchProcess = batchProcess;

                        string processType = batchProcess.processType;
                        int processTypeId = (int)batchProcess.fkProcessType;
                        long batchProcessId = batchProcess.batchProcessId;

                        //FILL PROCESS ATTRIBUTES AND EQUIPMENT SETTINGS
                        List<EquipmentSettingsValue> processAttributes = GetProcessAttributesAndEquipmentSettings(null, null, batchProcessId);
                        processResponse.processAttributes = processAttributes;
                        //processResponse.equipmentSettings = processAttributesAndEquipment.equipmentSettings;

                        processResponseList.Add(processResponse);
                    }
                }


                MeasurementsExt measurements = MeasurementsDa.GetMeasurementsBatchLevel(batchId);
                batch.measurements = measurements;

                batch.batchContent = batchContentList;
                batch.batchProcesses = processResponseList;

                //if (batch != null)
                //{
                //    return JsonConvert.SerializeObject(batch, jsonSettings);
                //}
                //else return JsonConvert.SerializeObject(empty, jsonSettings);
                //return JsonConvert.SerializeObject(batch, jsonSettings);
                return batch;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static SequenceResponse GetProcessSequence(int processSequenceId)
        {
            var currentUser = UserHelper.GetCurrentUser();
            int researchGroupId = (int)currentUser.fkResearchGroup;

            try
            {

                SequenceResponse sequence = new SequenceResponse();
                // sequence.sequenceInfo = Get na generalno info(label)
                List<ProcessSequenceContentExt> processList = ProcessSequenceDa.GetSequenceContent(processSequenceId); //samo od content tabela
                List<SequenceProcessResponse> processResponseList = new List<SequenceProcessResponse>();
                if (processList != null)
                {
                    foreach (ProcessSequenceContentExt process in processList)
                    {
                        SequenceProcessResponse sequenceProcess = new SequenceProcessResponse();
                        sequenceProcess.sequenceContent = process;

                        string processType = process.processType;
                        int processTypeId = (int)process.fkProcessType;

                        //FILL PROCESS ATTRIBUTES AND EQUIPMENT SETTINGS
                        List<EquipmentSettingsValue> processAttributes = GetProcessAttributesAndEquipmentSettings(null, null, null, process.processSequenceContentId);
                        sequenceProcess.processAttributes = processAttributes;

                        processResponseList.Add(sequenceProcess);
                    }
                    sequence.processesList = processResponseList;
                }
                return sequence;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //da probam da ne e dynamic
        public static List<EquipmentSettingsValue> GetProcessAttributesAndEquipmentSettings(int? fkExperiment = null, long? experimentProcessId = null, long? batchProcessId = null, int? sequenceContentId = null)
        {
            //dynamic processResponse = new ExpandoObject();

            //FILL PROCESS ATTRIBUTES

            //ova da se vralkja od List<EquipmentSettingsValue>
            object resultObject = new Object();
            List<EquipmentSettingsValue> processAttributes = EquipmentSettingsDa.GetAllEquipmentSettingsValue(fkExperiment, experimentProcessId, batchProcessId, sequenceContentId);
            //resultObject = equipment;

            //processResponse.processAttributes = resultObject;

            //FILL EQUIPMENT SETTINGS

            //object settingsObject = new Object();

            //processResponse.equipmentSettings = settingsObject;

            return processAttributes;
        }

        public static List<int> GetBatchIdsInsideExperiment(int experimentId)
        {
            List<int> result = new List<int>();
            List<int> batchesFirstLevel = ExperimentDa.GetBatchesInExperiment(experimentId);
            batchesFirstLevel = batchesFirstLevel.Distinct().ToList();
            result.AddRange(batchesFirstLevel);

            foreach (int batchId in batchesFirstLevel)
            {
                List<int> batchIds = GetBatchIdsInsideBatch(batchId);
                result.AddRange(batchIds);
            }
            return result.Distinct().ToList();
        }

        public static List<int> GetBatchIdsInsideBatch(int batchId)
        {
            List<int> result = new List<int>();

            List<BatchContentExt> allBatchContent = new List<BatchContentExt>();
            BatchContentDa.GetAllContentInBatchRecursive(allBatchContent, batchId);

            List<BatchContentExt> onlyBatches = allBatchContent.Where(x => x.fkStepBatch != null).ToList();
            foreach (BatchContentExt batc in onlyBatches)
            {
                result.Add((int)batc.fkStepBatch);
            }
            return result.Distinct().ToList();
        }


        public static long AddTestWithFiles(Test test, IList<HttpPostedFile> testResultsFiles, HttpPostedFile testProcedureFile)
        {
            var insertedTestId = TestDa.AddTest(test);

            string path = HostingEnvironment.MapPath("~/Uploads/FileAttachments/");

            return 0;
        }

    }
}