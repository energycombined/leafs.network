using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class EquipmentModel
    {
        public int equipmentModelId { get; set; }
        public int? fkEquipment { get; set; }
        public string equipmentModelName { get; set; }
        public string modelBrand { get; set; }

    }
}