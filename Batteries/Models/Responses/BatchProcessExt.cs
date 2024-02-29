using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class BatchProcessExt : BatchProcess
    {
        public string operatorUsername { get; set; }
        public int? fkProcessType { get; set; }
        public string processType { get; set; }
        public string subcategory { get; set; }
        public string processDatabaseType { get; set; }
        public int? fkEquipment { get; set; }
        public string equipmentName { get; set; }
        public int? equipmentModelId { get; set; }
        public string equipmentModelName { get; set; }
        public string modelBrand { get; set; }

        public BatchProcessExt(BatchProcess e = null)
        {
            if (e != null)
            {
                this.batchProcessId = e.batchProcessId;
                this.fkBatch = e.fkBatch;
                this.step = e.step;
                //this.fkProcessType = e.fkProcessType;
                this.processOrderInStep = e.processOrderInStep;
                this.fkProcess = e.fkProcess;
                this.label = e.label;
            }
        }
    }
}