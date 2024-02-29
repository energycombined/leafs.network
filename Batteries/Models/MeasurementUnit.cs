using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class MeasurementUnit
    {
        public int measurementUnitId { get; set; }
        public string measurementUnitName { get; set; }
        public string measurementUnitSymbol { get; set; }
        public DateTime? lastChange { get; set; }
    }
}