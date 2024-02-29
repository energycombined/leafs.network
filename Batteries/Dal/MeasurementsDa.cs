using Batteries.Dal.Base;
using Batteries.Models;
using Batteries.Models.Responses;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Batteries.Dal
{
    public class MeasurementsDa
    {
        public static List<MeasurementsExt> GetAllMeasurements(int? measurementLevelType = null, int? experimentId = null, int? batchId = null, int? batteryComponentTypeId = null, int? stepId = null, int? batteryComponentContentId = null, int? measurementsId = null)
        {
            DataTable dt;

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"SELECT *
                    FROM measurements m

                    WHERE (m.measurements_id = :mid or :mid is null) and
                        (m.fk_experiment = :eid or :eid is null) and
                        (m.fk_batch = :bid or :bid is null) and
                        (m.fk_battery_component_type = :bct or :bct is null) and
                        (m.step_id = :step or :step is null) and
                        (m.fk_battery_component_content = :bcc or :bcc is null)
                        ;";

                Db.CreateParameterFunc(cmd, "@mid", measurementsId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bct", batteryComponentTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@step", stepId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bcc", batteryComponentContentId, NpgsqlDbType.Integer);

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

            List<MeasurementsExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static MeasurementsExt GetMeasurementsComponentLevel(int? experimentId = null, int? batchId = null, int? batteryComponentTypeId = null)
        {
            DataTable dt;

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"SELECT *
                    FROM measurements m

                    WHERE 
                        (m.fk_measurement_level_type = 3) and
                        (m.fk_experiment = :eid or :eid is null) and
                        (m.fk_batch = :bid or :bid is null) and
                        (m.fk_battery_component_type = :bct or :bct is null) and
                        (m.step_id is null) and
                        (m.fk_battery_component_content is null)
                        ;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bct", batteryComponentTypeId, NpgsqlDbType.Integer);

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
            MeasurementsExt result;
            result = CreateObjectExt(dt.Rows[0]);

            return result;
        }
        public static MeasurementsExt GetMeasurementsStepLevel(int? experimentId = null, int? batchId = null, int? batteryComponentTypeId = null, int? stepId = null)
        {
            DataTable dt;

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"SELECT *
                    FROM measurements m

                    WHERE 
                        (m.fk_measurement_level_type = 2) and
                        (m.fk_experiment = :eid or :eid is null) and
                        (m.fk_batch = :bid or :bid is null) and
                        (m.fk_battery_component_type = :bct or :bct is null) and
                        (m.step_id = :step or :step is null) and
                        (m.fk_battery_component_content is null)
                        ;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bct", batteryComponentTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@step", stepId, NpgsqlDbType.Integer);

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
            MeasurementsExt result;
            result = CreateObjectExt(dt.Rows[0]);

            return result;
        }

        public static MeasurementsExt GetMeasurementsBatchLevel(int? batchId = null)
        {
            DataTable dt;

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"SELECT *
                    FROM measurements m

                    WHERE 
                        (m.fk_measurement_level_type = 5) and
                        (m.fk_batch = :bid or :bid is null) and
                        (m.fk_battery_component_content is null) and
                        (m.fk_batch_content is null)
                        ;";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);

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
            MeasurementsExt result;
            result = CreateObjectExt(dt.Rows[0]);

            return result;
        }
        public static int AddMeasurements(Measurements measurements, NpgsqlCommand cmd)
        {
            try
            {
                if (cmd != null)
                {
                    cmd.Parameters.Clear();
                }
                else
                {
                    cmd = Db.CreateCommand();

                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                }

                cmd.CommandText =
                    @"INSERT INTO public.measurements (
                    fk_measurement_level_type,
                    fk_experiment,
                    fk_batch,
                    fk_battery_component_type,
                    step_id,
                    fk_battery_component_content,
                    fk_batch_content,
                    measured_time,
                    measured_width,
                    measured_length,
                    measured_conductivity,
                    measured_thickness,
                    measured_weight
)
                    VALUES (:mlt, :eid, :bid, :bct, :step, :bcc, :btcc, :mtime, :mwidth, :mlength, :mcond, :mthickness, :mweight);";

                Db.CreateParameterFunc(cmd, "@mlt", measurements.fkMeasurementLevelType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@eid", measurements.fkExperiment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bid", measurements.fkBatch, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bct", measurements.fkBatteryComponentType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@step", measurements.stepId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bcc", measurements.fkBatteryComponentContent, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@btcc", measurements.fkBatchContent, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@mtime", measurements.measuredTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@mwidth", measurements.measuredWidth, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@mlength", measurements.measuredLength, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@mcond", measurements.measuredConductivity, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@mthickness", measurements.measuredThickness, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@mweight", measurements.measuredWeight, NpgsqlDbType.Double);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting measurements", ex);
            }

            return 0;
        }
        public static int UpdateMeasurements(Measurements measurements)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.measurements
                        SET 
                    fk_measurement_level_type=:mlt,
                    fk_experiment=:eid,
                    fk_batch=:bid,
                    fk_battery_component_type=:bct,
                    step_id=:step,
                    fk_battery_component_content=:bcc,
                    measured_time=:mtime,
                    measured_width=:mwidth,
                    measured_length=:mlength,
                    measured_conductivity=:mcond,
                    measured_thickness=:mthickness,
                    measured_weight=:mweight

                    WHERE measurements_id=:mid;";
                Db.CreateParameterFunc(cmd, "@mlt", measurements.fkMeasurementLevelType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@eid", measurements.fkExperiment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bid", measurements.fkBatch, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bct", measurements.fkBatteryComponentType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@step", measurements.stepId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bcc", measurements.fkBatteryComponentContent, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@mtime", measurements.measuredTime, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@mwidth", measurements.measuredWidth, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@mlength", measurements.measuredLength, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@mcond", measurements.measuredConductivity, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@mthickness", measurements.measuredThickness, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@mweight", measurements.measuredWeight, NpgsqlDbType.Double);

                Db.CreateParameterFunc(cmd, "@mid", measurements.measurementsId, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating measurements info", ex);
            }
            return 0;
        }

        public static int DeleteMeasurements(int measurementsId)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.measurements
                                WHERE measurements_id=:mid;";

                Db.CreateParameterFunc(cmd, "@mid", measurementsId, NpgsqlDbType.Integer);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        public static Measurements CreateObject(DataRow dr)
        {
            var measurements = new Measurements
            {
                measurementsId = (long)dr["measurements_id"],
                fkMeasurementLevelType = dr["fk_measurement_level_type"] != DBNull.Value ? int.Parse(dr["fk_measurement_level_type"].ToString()) : (int?)null,
                fkExperiment = dr["fk_experiment"] != DBNull.Value ? int.Parse(dr["fk_experiment"].ToString()) : (int?)null,
                fkBatch = dr["fk_batch"] != DBNull.Value ? int.Parse(dr["fk_batch"].ToString()) : (int?)null,
                fkBatteryComponentType = dr["fk_battery_component_type"] != DBNull.Value ? int.Parse(dr["fk_battery_component_type"].ToString()) : (int?)null,
                stepId = dr["step_id"] != DBNull.Value ? int.Parse(dr["step_id"].ToString()) : (int?)null,
                fkBatteryComponentContent = dr["fk_battery_component_content"] != DBNull.Value ? int.Parse(dr["fk_battery_component_content"].ToString()) : (int?)null,
                fkBatchContent = dr["fk_batch_content"] != DBNull.Value ? int.Parse(dr["fk_batch_content"].ToString()) : (int?)null,
                measuredTime = dr["measured_time"] != DBNull.Value ? double.Parse(dr["measured_time"].ToString()) : (double?)null,
                measuredWidth = dr["measured_width"] != DBNull.Value ? double.Parse(dr["measured_width"].ToString()) : (double?)null,
                measuredLength = dr["measured_length"] != DBNull.Value ? double.Parse(dr["measured_length"].ToString()) : (double?)null,
                measuredConductivity = dr["measured_conductivity"] != DBNull.Value ? double.Parse(dr["measured_conductivity"].ToString()) : (double?)null,
                measuredThickness = dr["measured_thickness"] != DBNull.Value ? double.Parse(dr["measured_thickness"].ToString()) : (double?)null,
                measuredWeight = dr["measured_weight"] != DBNull.Value ? double.Parse(dr["measured_weight"].ToString()) : (double?)null
            };
            return measurements;
        }
        private static MeasurementsExt CreateObjectExt(DataRow dr)
        {
            var measurements = CreateObject(dr);

            var measurementsExt = new MeasurementsExt(measurements)
            {
            };
            return measurementsExt;
        }
    }
}