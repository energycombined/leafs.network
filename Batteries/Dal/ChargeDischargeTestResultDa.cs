using Batteries.Dal.Base;
using Batteries.Models.TestResultsModels;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace Batteries.Dal
{
    public class ChargeDischargeTestResultDa
    {
        //        public static int AddChargeDischargeTestResults(ChargeDischargeTestResult chargeDischargeTestResult, NpgsqlCommand cmd, int experimentId, int? testId = null)
        //        {
        //            try
        //            {
        //                if (cmd != null)
        //                {
        //                    cmd.Parameters.Clear();
        //                }
        //                else
        //                {
        //                    cmd = Db.CreateCommand();
        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                    {
        //                        cmd.Connection.Open();
        //                    }
        //                }

        //                //int returnedID = 0;

        //                cmd.CommandType = CommandType.Text;


        //                cmd.CommandText =
        //            @"INSERT INTO public.charge_discharge_test_data (
        //                    fk_experiment,
        //                    fk_test,
        //                    fk_test_type,
        //                    data_point,
        //                    test_time,
        //                    step_time,
        //                    date_time,
        //                    step_index,
        //                    cycle_index,
        //                    is_fc_data,
        //                    current,
        //                    voltage,
        //                    charge_capacity,
        //                    discharge_capacity,
        //                    charge_energy,
        //                    discharge_energy,
        //                    dv_dt,
        //                    internal_resistance,
        //                    ac_impedance,
        //                    aci_phase_angle
        //                    
        //                    )
        //                                        VALUES (:eid, :tid, 1, :dp, :tt, :st, :dt, :si, :ci, :ifd, :curr, :vol, :cc, :dc, :ce, :de, :dvdt, :ir, :ai, :apa)
        //                    ;";

        //                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
        //                Db.CreateParameterFunc(cmd, "@tid", testId, NpgsqlDbType.Integer);

        //                Db.CreateParameterFunc(cmd, "@dp", chargeDischargeTestResult.dataPoint, NpgsqlDbType.Integer);
        //                Db.CreateParameterFunc(cmd, "@tt", chargeDischargeTestResult.testTime, NpgsqlDbType.Double);
        //                Db.CreateParameterFunc(cmd, "@st", chargeDischargeTestResult.stepTime, NpgsqlDbType.Double);
        //                Db.CreateParameterFunc(cmd, "@dt", chargeDischargeTestResult.dateTime, NpgsqlDbType.Double);
        //                Db.CreateParameterFunc(cmd, "@si", chargeDischargeTestResult.stepIndex, NpgsqlDbType.Integer);
        //                Db.CreateParameterFunc(cmd, "@ci", chargeDischargeTestResult.cycleIndex, NpgsqlDbType.Integer);
        //                Db.CreateParameterFunc(cmd, "@ifd", chargeDischargeTestResult.isFcData, NpgsqlDbType.Integer);
        //                Db.CreateParameterFunc(cmd, "@curr", chargeDischargeTestResult.current, NpgsqlDbType.Double);
        //                Db.CreateParameterFunc(cmd, "@vol", chargeDischargeTestResult.voltage, NpgsqlDbType.Double);
        //                Db.CreateParameterFunc(cmd, "@cc", chargeDischargeTestResult.chargeCapacity, NpgsqlDbType.Double);
        //                Db.CreateParameterFunc(cmd, "@dc", chargeDischargeTestResult.dischargeCapacity, NpgsqlDbType.Double);
        //                Db.CreateParameterFunc(cmd, "@ce", chargeDischargeTestResult.chargeEnergy, NpgsqlDbType.Double);
        //                Db.CreateParameterFunc(cmd, "@de", chargeDischargeTestResult.dischargeEnergy, NpgsqlDbType.Double);
        //                Db.CreateParameterFunc(cmd, "@dvdt", chargeDischargeTestResult.dvDt, NpgsqlDbType.Double);
        //                Db.CreateParameterFunc(cmd, "@ir", chargeDischargeTestResult.internalResistance, NpgsqlDbType.Double);
        //                Db.CreateParameterFunc(cmd, "@ai", chargeDischargeTestResult.acImpedance, NpgsqlDbType.Double);
        //                Db.CreateParameterFunc(cmd, "@apa", chargeDischargeTestResult.aciPhaseAngle, NpgsqlDbType.Double);

        //                int returnedID = int.Parse(Db.ExecuteScalar(cmd, false));
        //                if (returnedID <= 0)
        //                {
        //                    throw new Exception("Error inserting test results data");
        //                }

        //                return returnedID;
        //            }
        //            catch (Exception ex)
        //            {
        //                throw new Exception("Error inserting test results data", ex);
        //            }
        //        }

        public static int AddChargeDischargeTestResults(List<ChargeDischargeTestResult> chargeDischargeTestResultList, int experimentId, int? testId = null)
        {
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 240;

            NpgsqlTransaction t = cmd.Connection.BeginTransaction();
            try
            {
                cmd.CommandText =
                    @"DELETE FROM public.charge_discharge_test_data
                                WHERE fk_experiment=:eid;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                Db.ExecuteNonQuery(cmd, false);

                foreach (ChargeDischargeTestResult chargeDischargeTestResult in chargeDischargeTestResultList)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText =
                @"INSERT INTO public.charge_discharge_test_data (
                            fk_experiment,
                            fk_test,
                            fk_test_type,
                            data_point,
                            test_time,
                            step_time,
                            date_time,
                            step_index,
                            cycle_index,
                            is_fc_data,
                            current,
                            voltage,
                            charge_capacity,
                            discharge_capacity,
                            charge_energy,
                            discharge_energy,
                            dv_dt,
                            internal_resistance,
                            ac_impedance,
                            aci_phase_angle
                            
                            )
                                                VALUES (:eid, :tid, 1, :dp, :tt, :st, :dt, :si, :ci, :ifd, :curr, :vol, :cc, :dc, :ce, :de, :dvdt, :ir, :ai, :apa)
                            RETURNING test_data_id;";

                    Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@tid", testId, NpgsqlDbType.Integer);

                    Db.CreateParameterFunc(cmd, "@dp", chargeDischargeTestResult.dataPoint, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@tt", chargeDischargeTestResult.testTime, NpgsqlDbType.Double);
                    Db.CreateParameterFunc(cmd, "@st", chargeDischargeTestResult.stepTime, NpgsqlDbType.Double);
                    Db.CreateParameterFunc(cmd, "@dt", chargeDischargeTestResult.dateTime, NpgsqlDbType.Double);
                    Db.CreateParameterFunc(cmd, "@si", chargeDischargeTestResult.stepIndex, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@ci", chargeDischargeTestResult.cycleIndex, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@ifd", chargeDischargeTestResult.isFcData, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@curr", chargeDischargeTestResult.current, NpgsqlDbType.Double);
                    Db.CreateParameterFunc(cmd, "@vol", chargeDischargeTestResult.voltage, NpgsqlDbType.Double);
                    Db.CreateParameterFunc(cmd, "@cc", chargeDischargeTestResult.chargeCapacity, NpgsqlDbType.Double);
                    Db.CreateParameterFunc(cmd, "@dc", chargeDischargeTestResult.dischargeCapacity, NpgsqlDbType.Double);
                    Db.CreateParameterFunc(cmd, "@ce", chargeDischargeTestResult.chargeEnergy, NpgsqlDbType.Double);
                    Db.CreateParameterFunc(cmd, "@de", chargeDischargeTestResult.dischargeEnergy, NpgsqlDbType.Double);
                    Db.CreateParameterFunc(cmd, "@dvdt", chargeDischargeTestResult.dvDt, NpgsqlDbType.Double);
                    Db.CreateParameterFunc(cmd, "@ir", chargeDischargeTestResult.internalResistance, NpgsqlDbType.Double);
                    Db.CreateParameterFunc(cmd, "@ai", chargeDischargeTestResult.acImpedance, NpgsqlDbType.Double);
                    Db.CreateParameterFunc(cmd, "@apa", chargeDischargeTestResult.aciPhaseAngle, NpgsqlDbType.Double);

                    long returnedID = long.Parse(Db.ExecuteScalar(cmd, false));
                    if (returnedID <= 0)
                    {
                        t.Rollback();
                        throw new Exception("Error inserting test results data");
                    }
                }

                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.experiment
                        SET has_test_results_doc=true, date_modified=now()::timestamp
                        WHERE experiment_id=:eid;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                var res = Db.ExecuteNonQuery(cmd, false);
                if (res <= 0)
                {
                    t.Rollback();
                    throw new Exception("Error updating experiment info");
                }

                t.Commit();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                t.Rollback();
                throw new Exception(ex.Message);
            }

            return 0;
        }

        public static long AddMeasurementsDataOld(string jsonb, int? experimentId = null, int? batchId = null, int? testTypeId = null, int? testEquipmentModel = null)
        {
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 240;

            long returnedMeasurementID = 0;

            NpgsqlTransaction t = cmd.Connection.BeginTransaction();
            try
            {
                if (experimentId != null)
                {
                    cmd.CommandText =
                    @"DELETE FROM public.measurement_data
                                WHERE fk_experiment=:eid
                                AND fk_test_type = :ttid
                    ;";

                    Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@ttid", testTypeId, NpgsqlDbType.Integer);
                    Db.ExecuteNonQuery(cmd, false);
                }
                else if (batchId != null)
                {
                    cmd.CommandText =
                   @"DELETE FROM public.measurement_data
                                WHERE fk_batch=:bid
                                AND fk_test_type = :ttid
                                ;";

                    Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@ttid", testTypeId, NpgsqlDbType.Integer);
                    Db.ExecuteNonQuery(cmd, false);
                }


                cmd.Parameters.Clear();
                cmd.CommandText =
            @"INSERT INTO public.measurement_data (
                            fk_experiment,
                            fk_batch,
                            fk_test_type,
                            data
                            )
                                                VALUES (:eid, :bid, :tt, :data)
                            RETURNING measurement_data_id;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@tt", testTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@data", jsonb, NpgsqlDbType.Jsonb);

                returnedMeasurementID = long.Parse(Db.ExecuteScalar(cmd, false));
                if (returnedMeasurementID <= 0)
                {
                    t.Rollback();
                    throw new Exception("Error inserting test results data");
                }

                if (experimentId != null)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText =
                        @"UPDATE public.experiment
                        SET has_test_results_doc=true, date_modified=now()::timestamp
                        WHERE experiment_id=:eid;";

                    Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                    var res = Db.ExecuteNonQuery(cmd, false);
                    if (res <= 0)
                    {
                        t.Rollback();
                        throw new Exception("Error updating experiment info");
                    }
                }
                else if (batchId != null)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText =
                        @"UPDATE public.batch
                        SET has_test_results_doc=true
                        WHERE batch_id=:bid;";

                    Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                    var res = Db.ExecuteNonQuery(cmd, false);
                    if (res <= 0)
                    {
                        t.Rollback();
                        throw new Exception("Error updating experiment info");
                    }
                }


                t.Commit();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                t.Rollback();
                throw new Exception(ex.Message);
            }

            return returnedMeasurementID;
        }

        public static List<ChargeDischargeTestResult> GetAllChargeDischargeTestData(int? experimentId = null, int? testDataId = null)
        {
            DataTable dt;
            //test type id = 1 

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"SELECT *
                    FROM charge_discharge_test_data t                        

                    WHERE (t.fk_experiment = :eid or :eid is null) and
                        (t.test_data_id = :tdid or :tdid is null)
                    ORDER BY t.test_data_id;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@tdid", testDataId, NpgsqlDbType.Bigint);

                dt = Db.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            List<ChargeDischargeTestResult> list = (from DataRow dr in dt.Rows select CreateObject(dr)).ToList();

            return list;
        }

        public static List<ChargeDischargeTestResult> GetChargeDischargeTestDataForGraphs(int? experimentId = null, int? testDataId = null)
        {
            DataTable dt;
            //test type id = 1 

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"SELECT  test_data_id, t.test_time , t.voltage, t.current, t.charge_capacity, t.discharge_capacity,
                                t.charge_energy, t.discharge_energy, t.cycle_index
                              FROM charge_discharge_test_data t
                              WHERE
                                    (t.fk_experiment = :eid or :eid is null) 
                              ORDER BY t.cycle_index, t.test_time
                          ;";
                //and (t.current != 0)
                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@tdid", testDataId, NpgsqlDbType.Bigint);

                dt = Db.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            List<ChargeDischargeTestResult> list = (from DataRow dr in dt.Rows select CreateObject(dr)).ToList();

            return list;
        }
        public static List<ChargeDischargeTestResult> GetTestDataForGraphs(int? experimentId = null, int? testDataId = null)
        {
            DataTable dt;
            //test type id = 1 

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"SELECT arr.test_data_id, 
(arr.item_object->>'testTime') :: numeric as test_time, 
(arr.item_object->>'voltage') :: numeric as voltage,
(arr.item_object->>'current') :: numeric as current,
(arr.item_object->>'chargeCapacity') :: numeric as charge_capacity,
(arr.item_object->>'dischargeCpacity') :: numeric as discharge_capacity,
(arr.item_object->>'chargeEnergy') :: numeric as charge_energy,
(arr.item_object->>'dischargeEnergy') :: numeric as discharge_energy,
(arr.item_object->>'cycleIndex') :: integer as cycle_index
FROM measurement_data,
jsonb_array_elements(data) with ordinality arr(item_object, test_data_id) 
WHERE fk_experiment=:eid
ORDER BY cycle_index, test_time
                          ;";
                //and (t.current != 0)
                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@tdid", testDataId, NpgsqlDbType.Bigint);

                dt = Db.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            List<ChargeDischargeTestResult> list = (from DataRow dr in dt.Rows select CreateObject(dr)).ToList();

            return list;
        }

        public static dynamic GetChargeDischargeMaxValues(int? experimentId = null)
        {
            DataTable dt;
            //test type id = 1 

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"SELECT  max(t.charge_capacity) as max_charge_capacity, max(t.discharge_capacity) as max_discharge_capacity, max(t.charge_energy) as max_charge_energy, max(t.discharge_energy) as max_discharge_energy
                              FROM charge_discharge_test_data t
                              WHERE
                                    (t.fk_experiment = :eid or :eid is null)
                          ;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            //List<ChargeDischargeTestResult> list = (from DataRow dr in dt.Rows select CreateObject(dr)).ToList();

            DataRow dr = dt.Rows[0];
            double? maxChargeCapacity = dr["max_charge_capacity"] != DBNull.Value ? double.Parse(dr["max_charge_capacity"].ToString()) : (double?)null;
            double? maxDischargeCapacity = dr["max_discharge_capacity"] != DBNull.Value ? double.Parse(dr["max_discharge_capacity"].ToString()) : (double?)null;
            double? maxChargeEnergy = dr["max_charge_energy"] != DBNull.Value ? double.Parse(dr["max_charge_energy"].ToString()) : (double?)null;
            double? maxDischargeEnergy = dr["max_discharge_energy"] != DBNull.Value ? double.Parse(dr["max_discharge_energy"].ToString()) : (double?)null;

            dynamic result = new ExpandoObject();
            result.maxChargeCapacity = maxChargeCapacity;
            result.maxDischargeCapacity = maxDischargeCapacity;
            result.maxChargeEnergy = maxChargeEnergy;
            result.maxDischargeEnergy = maxDischargeEnergy;

            return result;
            //return list;
        }

        public static dynamic GetCapacityMaxValuesByCycleIndex(int? experimentId, int? batchId)
        {
            DataTable dt;
            //test type id = 1 

            try
            {
                var cmd = Db.CreateCommand();
                if (experimentId != 0)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.CommandText =
                         @"SELECT max(arr.item_object->>'chargeCapacity') :: numeric as max_charge_capacity,
max(arr.item_object->>'dischargeCpacity') :: numeric as max_discharge_capacity,
max(arr.item_object->>'chargeEnergy') :: numeric as max_charge_energy,
max(arr.item_object->>'dischargeEnergy') :: numeric as max_discharge_energy,
(arr.item_object->>'cycleIndex') :: integer as cycle_index
FROM measurement_data,
jsonb_array_elements(data) with ordinality arr(item_object, test_data_id) 
WHERE fk_experiment = :eid or :eid is null
GROUP BY cycle_index
ORDER BY cycle_index
                          ;";
                    /*  cmd.CommandText =
                          @"SELECT  max(t.charge_capacity) as max_charge_capacity, max(t.discharge_capacity) as max_discharge_capacity, max(t.charge_energy) as max_charge_energy, max(t.discharge_energy) as max_discharge_energy,
                                    t.cycle_index
                                    FROM charge_discharge_test_data t
                                    WHERE
                                          (t.fk_experiment = :eid or :eid is null)
                                    GROUP BY t.cycle_index
                                    ORDER BY  t.cycle_index
                                ;";*/

                    Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);

                    dt = Db.ExecuteSelectCommand(cmd);
                }
                else
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.CommandText =
                         @"SELECT max(arr.item_object->>'chargeCapacity') :: numeric as max_charge_capacity,
max(arr.item_object->>'dischargeCpacity') :: numeric as max_discharge_capacity,
max(arr.item_object->>'chargeEnergy') :: numeric as max_charge_energy,
max(arr.item_object->>'dischargeEnergy') :: numeric as max_discharge_energy,
(arr.item_object->>'cycleIndex') :: integer as cycle_index
FROM measurement_data,
jsonb_array_elements(data) with ordinality arr(item_object, test_data_id) 
WHERE fk_batch = :bid or :bid is null
GROUP BY cycle_index
ORDER BY cycle_index
                          ;";

                    Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);

                    dt = Db.ExecuteSelectCommand(cmd);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            List<dynamic> list = (from DataRow dr in dt.Rows select CreateDynamicObject(dr)).ToList();

            return list;
        }

        public static dynamic CreateDynamicObject(DataRow dr)
        {
            double? maxChargeCapacity = dr["max_charge_capacity"] != DBNull.Value ? double.Parse(dr["max_charge_capacity"].ToString()) : (double?)null;
            double? maxDischargeCapacity = dr["max_discharge_capacity"] != DBNull.Value ? double.Parse(dr["max_discharge_capacity"].ToString()) : (double?)null;
            double? maxChargeEnergy = dr["max_charge_energy"] != DBNull.Value ? double.Parse(dr["max_charge_energy"].ToString()) : (double?)null;
            double? maxDischargeEnergy = dr["max_discharge_energy"] != DBNull.Value ? double.Parse(dr["max_discharge_energy"].ToString()) : (double?)null;
            int? cycleIndex = dr["cycle_index"] != DBNull.Value ? int.Parse(dr["cycle_index"].ToString()) : (int?)null;

            dynamic result = new ExpandoObject();
            result.maxChargeCapacity = maxChargeCapacity;
            result.maxDischargeCapacity = maxDischargeCapacity;
            result.maxChargeEnergy = maxChargeEnergy;
            result.maxDischargeEnergy = maxDischargeEnergy;
            result.cycleIndex = cycleIndex;

            return result;
        }
        public static ChargeDischargeTestResult CreateObject(DataRow dr)
        {
            int? fkTestVar = (int?)null;
            if (dr.Table.Columns.Contains("fk_test"))
            {
                fkTestVar = dr["fk_test"] != DBNull.Value ? int.Parse(dr["fk_test"].ToString()) : (int?)null;
            }
            int? fkTestTypeVar = (int?)null;
            if (dr.Table.Columns.Contains("fk_test_type"))
            {
                fkTestTypeVar = dr["fk_test_type"] != DBNull.Value ? int.Parse(dr["fk_test_type"].ToString()) : (int?)null;
            }
            int? fkExperimentVar = (int?)null;
            if (dr.Table.Columns.Contains("fk_experiment"))
            {
                fkExperimentVar = dr["fk_experiment"] != DBNull.Value ? int.Parse(dr["fk_experiment"].ToString()) : (int?)null;
            }

            int? dataPointVar = (int?)null;
            if (dr.Table.Columns.Contains("data_point"))
            {
                dataPointVar = dr["data_point"] != DBNull.Value ? int.Parse(dr["data_point"].ToString()) : (int?)null;
            }
            double? stepTimeVar = (double?)null;
            if (dr.Table.Columns.Contains("step_time"))
            {
                stepTimeVar = dr["step_time"] != DBNull.Value ? double.Parse(dr["step_time"].ToString()) : (double?)null;
            }
            double? testTimeVar = (double?)null;
            if (dr.Table.Columns.Contains("test_time"))
            {
                testTimeVar = dr["test_time"] != DBNull.Value ? double.Parse(dr["test_time"].ToString()) : (double?)null;
            }
            double? dateTimeVar = (double?)null;
            if (dr.Table.Columns.Contains("date_time"))
            {
                dateTimeVar = dr["date_time"] != DBNull.Value ? double.Parse(dr["date_time"].ToString()) : (double?)null;
            }
            int? stepIndexVar = (int?)null;
            if (dr.Table.Columns.Contains("step_index"))
            {
                stepIndexVar = dr["step_index"] != DBNull.Value ? int.Parse(dr["step_index"].ToString()) : (int?)null;
            }
            int? cycleIndexVar = (int?)null;
            if (dr.Table.Columns.Contains("cycle_index"))
            {
                cycleIndexVar = dr["cycle_index"] != DBNull.Value ? int.Parse(dr["cycle_index"].ToString()) : (int?)null;
            }
            int? isFcDataVar = (int?)null;
            if (dr.Table.Columns.Contains("is_fc_data"))
            {
                isFcDataVar = dr["is_fc_data"] != DBNull.Value ? int.Parse(dr["is_fc_data"].ToString()) : (int?)null;
            }
            double? currentVar = (double?)null;
            if (dr.Table.Columns.Contains("current"))
            {
                currentVar = dr["current"] != DBNull.Value ? double.Parse(dr["current"].ToString()) : (double?)null;
            }
            double? voltageVar = (double?)null;
            if (dr.Table.Columns.Contains("voltage"))
            {
                voltageVar = dr["voltage"] != DBNull.Value ? double.Parse(dr["voltage"].ToString()) : (double?)null;
            }
            double? chargeCapacityVar = (double?)null;
            if (dr.Table.Columns.Contains("charge_capacity"))
            {
                chargeCapacityVar = dr["charge_capacity"] != DBNull.Value ? double.Parse(dr["charge_capacity"].ToString()) : (double?)null;
            }
            double? dischargeCapacityVar = (double?)null;
            if (dr.Table.Columns.Contains("discharge_capacity"))
            {
                dischargeCapacityVar = dr["discharge_capacity"] != DBNull.Value ? double.Parse(dr["discharge_capacity"].ToString()) : (double?)null;
            }
            double? chargeEnergyVar = (double?)null;
            if (dr.Table.Columns.Contains("charge_energy"))
            {
                chargeEnergyVar = dr["charge_energy"] != DBNull.Value ? double.Parse(dr["charge_energy"].ToString()) : (double?)null;
            }
            double? dischargeEnergyVar = (double?)null;
            if (dr.Table.Columns.Contains("discharge_energy"))
            {
                dischargeEnergyVar = dr["discharge_energy"] != DBNull.Value ? double.Parse(dr["discharge_energy"].ToString()) : (double?)null;
            }
            double? dvDtVar = (double?)null;
            if (dr.Table.Columns.Contains("dv_dt"))
            {
                dvDtVar = dr["dv_dt"] != DBNull.Value ? double.Parse(dr["dv_dt"].ToString()) : (double?)null;
            }
            double? internalResistanceVar = (double?)null;
            if (dr.Table.Columns.Contains("internal_resistance"))
            {
                internalResistanceVar = dr["internal_resistance"] != DBNull.Value ? double.Parse(dr["internal_resistance"].ToString()) : (double?)null;
            }
            double? acImpedanceVar = (double?)null;
            if (dr.Table.Columns.Contains("ac_impedance"))
            {
                acImpedanceVar = dr["ac_impedance"] != DBNull.Value ? double.Parse(dr["ac_impedance"].ToString()) : (double?)null;
            }
            double? aciPhaseAngleVar = (double?)null;
            if (dr.Table.Columns.Contains("aci_phase_angle"))
            {
                aciPhaseAngleVar = dr["aci_phase_angle"] != DBNull.Value ? double.Parse(dr["aci_phase_angle"].ToString()) : (double?)null;
            }


            var chargeDischargeTestResult = new ChargeDischargeTestResult
            {
                testDataId = (long)dr["test_data_id"],
                fkTest = fkTestVar,
                fkTestType = fkTestTypeVar,
                fkExperiment = fkExperimentVar,
                dataPoint = dataPointVar,
                stepTime = stepTimeVar,
                testTime = testTimeVar,
                dateTime = dateTimeVar,
                stepIndex = stepIndexVar,
                cycleIndex = cycleIndexVar,
                isFcData = isFcDataVar,
                current = currentVar,
                voltage = voltageVar,
                chargeCapacity = chargeCapacityVar,
                dischargeCapacity = dischargeCapacityVar,
                chargeEnergy = chargeEnergyVar,
                dischargeEnergy = dischargeEnergyVar,
                dvDt = dvDtVar,
                internalResistance = internalResistanceVar,
                acImpedance = acImpedanceVar,
                aciPhaseAngle = aciPhaseAngleVar
            };
            return chargeDischargeTestResult;
        }
    }
}

