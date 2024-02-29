using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class StockTransaction
    {
        public long stockTransactionId { get; set; }
        public long? fkMaterial { get; set; }
        public int? fkBatch { get; set; }
        public short? stockTransactionElementType { get; set; }
        public double? amount { get; set; }
        public int? fkVendor { get; set; }
        public int? fkOperator { get; set; }
        public int? fkResearchGroup { get; set; }
        public int? fkExperimentComing { get; set; }
        public int? fkBatchComing { get; set; }
        public short? transactionDirection { get; set; }
        public int? fkBatteryComponentType { get; set; }
        public DateTime? dateBought { get; set; }
        public DateTime? dateCreated { get; set; }

    }
}