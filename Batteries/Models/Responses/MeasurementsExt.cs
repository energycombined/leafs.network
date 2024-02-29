using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class MeasurementsExt : Measurements
    {
        public MeasurementsExt(Measurements e = null)
        {
            if (e != null)
            {
                this.measurementsId = e.measurementsId;
                this.fkMeasurementLevelType = e.fkMeasurementLevelType;
                this.fkExperiment = e.fkExperiment;
                this.fkBatch = e.fkBatch;
                this.fkBatteryComponentType = e.fkBatteryComponentType;
                this.stepId = e.stepId;
                this.fkBatteryComponentContent = e.fkBatteryComponentContent;
                this.fkBatchContent = e.fkBatchContent;
                this.measuredTime = e.measuredTime;
                this.measuredWidth = e.measuredWidth;
                this.measuredLength = e.measuredLength;
                this.measuredConductivity = e.measuredConductivity;
                this.measuredThickness = e.measuredThickness;
                this.measuredWeight = e.measuredWeight;
            }
        }
    }
}