using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class PreviousProcessResponse
    {
        public long? experimentProcessId { get; set; }
        public int? fkProcessType { get; set; }
        public string label { get; set; }
        public int? fkEquipment { get; set; }
        public DateTime? dateCreated { get; set; }
        public int? batchProcessId { get; set; }
        public string processType { get; set; }
        public string equipmentName { get; set; }
        public string subcategory { get; set; }
        public int? fkEquipmentModel { get; set; }
        public string equipmentModelName { get; set; }
        public string modelBrand { get; set; }
        public int? fkProcess { get; set;}
    }
}