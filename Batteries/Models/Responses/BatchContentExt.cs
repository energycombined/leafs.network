using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class BatchContentExt : BatchContent
    {
        public string materialName { get; set; }
        public string batchSystemLabel { get; set; }
        public string batchPersonalLabel { get; set; }
        public string measurementUnitName { get; set; }
        public string measurementUnitSymbol { get; set; }
        public string chemicalFormula { get; set; }


        public string materialLabel { get; set; }
        public string description { get; set; }
        //public string storedInType { get; set; }
        public string price { get; set; }
        public string bulkPrice { get; set; }
        public string materialFunction { get; set; }
        public double? batchOutput { get; set; }

        public MeasurementsExt measurements { get; set; }

        public BatchContentExt(BatchContent e = null)
        {
            if (e != null)
            {
                this.batchContentId = e.batchContentId;
                this.fkBatch = e.fkBatch;
                this.step = e.step;
                this.fkStepMaterial = e.fkStepMaterial;
                this.fkStepBatch = e.fkStepBatch;
                this.weight = e.weight;
                this.fkFunction = e.fkFunction;
                this.orderInStep = e.orderInStep;
                this.isComplete = e.isComplete;
                this.percentageOfActive = e.percentageOfActive;
            }
        }
    }
}