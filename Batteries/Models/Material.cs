using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class Material
    {
        public long materialId { get; set; }
        public string materialName { get; set; }
        public string materialLabel { get; set; }
        public string description { get; set; }
        public string chemicalFormula { get; set; }
        public int? fkMaterialType { get; set; }
        public int? fkStoredInType { get; set; }
        public int? fkOperator { get; set; }
        public int? fkMeasurementUnit { get; set; }       
        public int? fkVendor { get; set; }        
        public double? price { get; set; }
        public double? bulkPrice { get; set; }
        public string reference { get; set; }
        public int? fkFunction { get; set; }
        public string casNumber { get; set; }
        public string lotNumber { get; set; }
        public double? percentageOfActive { get; set; }
        public DateTime? dateCreated { get; set; }
        public DateTime? dateBought { get; set; }
        public DateTime? firstUse { get; set; }
        public DateTime? lastChange { get; set; }

    }
}