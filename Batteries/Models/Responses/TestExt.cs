using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class TestExt : Test
    {
        public string testType { get; set; }
        public string operatorUsername { get; set; }
        public string researchGroupName { get; set; }
        public string researchGroupAcronym { get; set; }
        public string testEquipmentBrand { get; set; }
        public string testEquipmentModel { get; set; }
        public string testTypeSubcategory { get; set; }
        public string batteryComponentType { get; set; }
        public string materialName { get; set; }
        public string experimentSystemLabel { get; set; }
        public string experimentPersonalLabel { get; set; }
        public string batchSystemLabel { get; set; }
        public string batchPersonalLabel { get; set; }
        public TestExt(Test e = null)
        {
            if (e != null)
            {
                this.testId = e.testId;
                this.fkTestType = e.fkTestType;
                this.fkTestEquipmentModel = e.fkTestEquipmentModel;
                this.fkMeasurementLevelType = e.fkMeasurementLevelType;
                this.fkExperiment = e.fkExperiment;
                this.fkBatch = e.fkBatch;
                this.fkMaterial = e.fkMaterial;
                this.fkBatteryComponentType = e.fkBatteryComponentType;
                this.stepId = e.stepId;
                this.fkBatteryComponentContent = e.fkBatteryComponentContent;
                this.fkBatchContent = e.fkBatchContent;
                this.fkResearchGroup = e.fkResearchGroup;
                this.fkUser = e.fkUser;
                this.testLabel = e.testLabel;
                this.comment = e.comment;
                this.lastChange = e.lastChange;
                this.dateCreated = e.dateCreated;
            }
        }
    }
}