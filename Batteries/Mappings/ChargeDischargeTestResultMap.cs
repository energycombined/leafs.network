using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CsvHelper.Configuration;
using Batteries.Models.TestResultsModels;
using System.Globalization;


namespace Batteries.Mappings
{
    public sealed class ChargeDischargeTestResultMap : ClassMap<ChargeDischargeTestResult>
    {
        public ChargeDischargeTestResultMap()
        {
            //item.measurements.measuredTime != null ? item.measurements.measuredTime.ToString(CultureInfo.InvariantCulture) : "";
            Map(m => m.fkTestType).Name("Test_ID").Index(0);
            Map(m => m.dataPoint).Name("Data_Point").Index(1);
            Map(m => m.testTime).Name("Test_Time").Index(2).TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);
            Map(m => m.stepTime).Name("Step_Time").Index(4).TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);
            Map(m => m.dateTime).Name("DateTime").Index(3).TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);
            Map(m => m.stepIndex).Name("Step_Index").Index(5);
            Map(m => m.cycleIndex).Name("Cycle_Index").Index(6);
            Map(m => m.isFcData).Name("Is_FC_Data").Index(7);
            Map(m => m.current).Name("Current").Index(8).TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);
            Map(m => m.voltage).Name("Voltage").Index(9).TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);
            Map(m => m.chargeCapacity).Name("Charge_Capacity").Index(10).TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);
            Map(m => m.dischargeCapacity).Name("Discharge_Capacity").Index(11).TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);
            Map(m => m.chargeEnergy).Name("Charge_Energy").Index(12).TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);
            Map(m => m.dischargeEnergy).Name("Discharge_Energy").Index(13).TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);
            Map(m => m.dvDt).Name("dV/dt").Index(14).TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);
            Map(m => m.internalResistance).Name("Internal_Resistance").Index(15).TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);
            Map(m => m.acImpedance).Name("AC_Impedance").Index(16).TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);
            Map(m => m.aciPhaseAngle).Name("ACI_Phase_Angle").Index(17).TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);

            //Map(m => m.fkTestType).Name("Test_ID");
            //Map(m => m.dataPoint).Name("Data_Point");
            //Map(m => m.testTime).Name("Test_Time");
            //Map(m => m.dateTime).Name("Step_Time");
            //Map(m => m.stepTime).Name("DateTime");
            //Map(m => m.stepIndex).Name("Step_Index");
            //Map(m => m.cycleIndex).Name("Cycle_Index");
            //Map(m => m.isFcData).Name("Is_FC_Data");
            //Map(m => m.current).Name("Current");
            //Map(m => m.voltage).Name("Voltage");
            //Map(m => m.chargeCapacity).Name("Charge_Capacity");
            //Map(m => m.dischargeCapacity).Name("Discharge_Capacity");
            //Map(m => m.chargeEnergy).Name("Charge_Energy");
            //Map(m => m.dischargeEnergy).Name("Discharge_Energy");
            //Map(m => m.dvDt).Name("dV/dt");
            //Map(m => m.internalResistance).Name("Internal_Resistance");
            //Map(m => m.acImpedance).Name("AC_Impedance");
            //Map(m => m.aciPhaseAngle).Name("ACI_Phase_Angle");


            



            //"Test_ID","Data_Point","Test_Time","Step_Time","DateTime","Step_Index","Cycle_Index","Is_FC_Data","Current","Voltage","Charge_Capacity","Discharge_Capacity","Charge_Energy","Discharge_Energy","dV/dt","Internal_Resistance","AC_Impedance","ACI_Phase_Angle"


        }
    }
}
