using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class Batch
    {
        public int batchId { get; set; }        
        public int? fkUser { get; set; }
        public int? fkResearchGroup { get; set; }
        public string batchSystemLabel { get; set; }
        public string batchPersonalLabel { get; set; }
        public string description { get; set; }
        public DateTime? dateCreated { get; set; }
        public DateTime? lastChange { get; set; }
        public double? batchOutput { get; set; }
        public int? fkMeasurementUnit { get; set; }
        public string chemicalFormula { get; set; }
        public int? fkMaterialType { get; set; }
        public int? fkTemplate { get; set; }
        public Boolean? isComplete { get; set; }
        public int? fkEditedBy { get; set; }
        public int? fkProject { get; set; }
        public Boolean? hasTestResultsDoc { get; set; }
        public double? totalBatchOutput { get; set; } 
        public double? wasteAmount { get; set; }
        public string releasedAs { get; set; }
        public string wasteChemicalComposition { get; set; }
        public string wasteComment { get; set; }
    }
}