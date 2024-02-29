using Batteries.Models.TestResultsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class Test
    {
        public long testId { get; set; }
        public int? fkTestType { get; set; }
        public int? fkTestEquipmentModel { get; set; }
        public int? fkMeasurementLevelType { get; set; }
        public int? fkExperiment { get; set; }
        public int? fkBatch { get; set; }
        public int? fkMaterial { get; set; }
        public int? fkBatteryComponentType { get; set; }        
        public int? stepId { get; set; }
        public int? fkBatteryComponentContent { get; set; }
        public int? fkBatchContent { get; set; }
        public int? fkResearchGroup { get; set; }
        public int? fkUser { get; set; }
        public string testLabel { get; set; }
        public string comment { get; set; }
        public DateTime? dateCreated { get; set; }
        public DateTime? lastChange { get; set; }
        public TestData testData { get; set; }

    }
}