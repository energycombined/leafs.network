using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class ProcessSequenceContentExt : ProcessSequenceContent
    {
        public int? fkProcessType { get; set; }
        public string processType { get; set; }
        public string subcategory { get; set; }
        public int? fkEquipment { get; set; }
        public string equipmentName { get; set; }
        public int? fkEquipmentModel { get; set; }
        public string equipmentModelName { get; set; }
        public string modelBrand { get; set; }
        public ProcessSequenceContentExt(ProcessSequenceContent e = null)
        {
            if (e != null)
            {
                this.processSequenceContentId = e.processSequenceContentId;
                this.order = e.order;
                this.fkProcessSequence = e.fkProcessSequence;
                this.fkProcess = e.fkProcess;
                this.dateCreated = e.dateCreated;
                this.processLabel = e.processLabel;
            }
        }
    }
}