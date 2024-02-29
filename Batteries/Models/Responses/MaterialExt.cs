using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class MaterialExt : Material
    {
        public string materialType { get; set; }
        public string storedInType { get; set; }
        public string operatorUsername { get; set; }
        public double? availableQuantity { get; set; }
        public string measurementUnitName { get; set; }
        public string measurementUnitSymbol { get; set; }
        public string materialFunction { get; set; }
        public string vendorName { get; set; }
        public string vendorSite { get; set; }
        public int? fkResearchGroup { get; set; }
        public MaterialExt(Material e)
        {
            this.materialId = e.materialId;
            this.materialName = e.materialName;
            this.materialLabel = e.materialLabel;
            this.description = e.description;
            this.chemicalFormula = e.chemicalFormula;
            this.fkMaterialType = e.fkMaterialType;
            this.fkStoredInType = e.fkStoredInType;            
            this.fkOperator = e.fkOperator;
            this.fkMeasurementUnit = e.fkMeasurementUnit;
            this.dateBought = e.dateBought;
            this.firstUse = e.firstUse;
            this.fkVendor = e.fkVendor;
            this.price = e.price;
            this.bulkPrice = e.bulkPrice;
            this.reference = e.reference;
            this.fkFunction = e.fkFunction;
            this.casNumber = e.casNumber;
            this.lotNumber = e.lotNumber;
            this.percentageOfActive = e.percentageOfActive;
            this.dateCreated = e.dateCreated;
            this.lastChange = e.lastChange;
        }
    }

}