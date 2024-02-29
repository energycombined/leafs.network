using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class StockTransactionExt : StockTransaction
    {
        public string materialName { get; set; }
        public string materialType { get; set; }
        public string materialChemicalFormula { get; set; }
        public string vendorName { get; set; }
        
        public string operatorUsername { get; set; }
        public string measurementUnitName { get; set; }
        public string measurementUnitSymbol { get; set; }

        public string batchSystemLabel { get; set; }
        public string batchPersonalLabel { get; set; }
        public string batchChemicalFormula { get; set; }

        public StockTransactionExt(StockTransaction e)
        {
            this.stockTransactionId = e.stockTransactionId;
            this.fkMaterial = e.fkMaterial;
            this.fkBatch = e.fkBatch;
            this.stockTransactionElementType = e.stockTransactionElementType;
            this.amount = e.amount;
            
            this.fkVendor = e.fkVendor;
            this.fkOperator = e.fkOperator;
            this.fkResearchGroup = e.fkResearchGroup;
            this.fkExperimentComing = e.fkExperimentComing;
            this.fkBatchComing = e.fkBatchComing;
            
            this.transactionDirection = e.transactionDirection;            
            this.fkBatteryComponentType = e.fkBatteryComponentType;
            this.dateBought = e.dateBought;
            this.dateCreated = e.dateCreated;
        }
    }
}