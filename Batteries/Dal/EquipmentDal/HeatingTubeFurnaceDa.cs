using Batteries.Dal.Base;
using Batteries.Models;
using Batteries.Models.EquipmentModels;
using Batteries.Models.Responses;
using Batteries.Models.Responses.EquipmentModels;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Batteries.Dal.EquipmentDal
{
    public class HeatingTubeFurnaceDa
    {
        public static List<HeatingTubeFurnaceExt> GetAllTubeFurnaces(long? settingsId = null, long? experimentProcessId = null, long? batchProcessId = null, int? equipmentModelId = null)
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
                    FROM heating_tube_furnace m
                        left join experiment_process ep on m.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on m.fk_batch_process = bp.batch_process_id
                        left join equipment_model eq on m.fk_equipment_model = eq.equipment_model_id

                    WHERE (m.settings_id = :sid or :sid is null) and
                        (m.fk_experiment_process = :epid or :epid is null) and
                        (m.fk_batch_process = :bpid or :bpid is null) and
                        (m.fk_equipment_model = :emid or :emid is null)
                    ;";

                Db.CreateParameterFunc(cmd, "@sid", settingsId, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@epid", experimentProcessId, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", batchProcessId, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@emid", equipmentModelId, NpgsqlDbType.Integer);

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

            List<HeatingTubeFurnaceExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<HeatingTubeFurnaceExt> GetRecentlyUsedTubeFurnaces(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null, int? equipmentModelId = null)
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
                    @"SELECT max(settings_id) as settings_id, max(date_created) as date_created, fk_equipment_model, tube_material, tube_diameter, tube_amount_of_openings, atmosphere, flow, ramp_up_time, temperature, duration, ramp_down_time, loop_count, comment, label
                        FROM heating_tube_furnace
                      GROUP BY fk_equipment_model, tube_material, tube_diameter, tube_amount_of_openings, atmosphere, flow, ramp_up_time, temperature, duration, ramp_down_time, loop_count, comment, label
                      ORDER BY max(settings_id) DESC LIMIT 10;";

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

            List<HeatingTubeFurnaceExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddTubeFurnace(HeatingTubeFurnace tubeFurnace, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.heating_tube_furnace (
fk_experiment_process,
fk_batch_process,
fk_equipment_model,
tube_material,
tube_diameter,
tube_amount_of_openings,
atmosphere,
flow,
ramp_up_time,
temperature,
duration,
ramp_down_time,
loop_count,
comment,
label,
date_created)
                    VALUES (:epid, :bpid, :emid, :tm, :td, :tao, :atm, :flow, :rut, :temp, :dur, :rdt, :lc, :com, :lab, now()::timestamp);";

                //:envtype::environment,

                Db.CreateParameterFunc(cmd, "@epid", tubeFurnace.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", tubeFurnace.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@emid", tubeFurnace.fkEquipmentModel, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@tm", tubeFurnace.tubeMaterial, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@td", tubeFurnace.tubeDiameter, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@tao", tubeFurnace.tubeAmountOfOpenings, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@atm", tubeFurnace.atmosphere, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@flow", tubeFurnace.flow, NpgsqlDbType.Boolean);
                Db.CreateParameterFunc(cmd, "@rut", tubeFurnace.rampUpTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@temp", tubeFurnace.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@dur", tubeFurnace.duration, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@rdt", tubeFurnace.rampDownTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@lc", tubeFurnace.loopCount, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@com", tubeFurnace.comment, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", tubeFurnace.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting Tube Furnace settings", ex);
            }

            return 0;
        }
        public static int UpdateTubeFurnace(HeatingTubeFurnace tubeFurnace)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.heating_tube_furnace
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment_model=emid,
tube_material =:tm,
tube_diameter =:td,
tube_amount_of_openings =:tao,
atmosphere =:atm,
flow =:flow,
ramp_up_time =:rut,
temperature =:temp,
duration =:dur,
ramp_down_time =:rdt,
loop_count =:lc,
comment =:com,
label =:lab,
date_created=now()::timestamp

                        WHERE settings_id=:sid;";
                Db.CreateParameterFunc(cmd, "@epid", tubeFurnace.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", tubeFurnace.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@emid", tubeFurnace.fkEquipmentModel, NpgsqlDbType.Integer);

                Db.CreateParameterFunc(cmd, "@tm", tubeFurnace.tubeMaterial, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@td", tubeFurnace.tubeDiameter, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@tao", tubeFurnace.tubeAmountOfOpenings, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@atm", tubeFurnace.atmosphere, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@flow", tubeFurnace.flow, NpgsqlDbType.Boolean);
                Db.CreateParameterFunc(cmd, "@rut", tubeFurnace.rampUpTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@temp", tubeFurnace.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@dur", tubeFurnace.duration, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@rdt", tubeFurnace.rampDownTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@lc", tubeFurnace.loopCount, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@com", tubeFurnace.comment, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", tubeFurnace.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@sid", tubeFurnace.settingsId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Tube Furnace settings", ex);
            }
            return 0;
        }
        public static int DeleteTubeFurnace(long settingsId)
        {
            try
            {
                var cmd = Db.CreateCommand();

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.heating_tube_furnace
                                WHERE settings_id=:sid;";

                Db.CreateParameterFunc(cmd, "@sid", settingsId, NpgsqlDbType.Bigint);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        public static HeatingTubeFurnace CreateObject(DataRow dr)
        {
            long? fkExperimentProcessVar = (long?)null;
            if (dr.Table.Columns.Contains("fk_experiment_process"))
            {
                fkExperimentProcessVar = dr["fk_experiment_process"] != DBNull.Value ? long.Parse(dr["fk_experiment_process"].ToString()) : (long?)null;
            }
            long? fkBatchProcessVar = (long?)null;
            if (dr.Table.Columns.Contains("fk_batch_process"))
            {
                fkBatchProcessVar = dr["fk_batch_process"] != DBNull.Value ? long.Parse(dr["fk_batch_process"].ToString()) : (long?)null;
            }
            int? fkEquipmentModelVar = (int?)null;
            if (dr.Table.Columns.Contains("fk_equipment_model"))
            {
                fkEquipmentModelVar = dr["fk_equipment_model"] != DBNull.Value ? int.Parse(dr["fk_equipment_model"].ToString()) : (int?)null;
            }
            string commentVar = null;
            if (dr.Table.Columns.Contains("comment"))
            {
                commentVar = dr["comment"].ToString();
            }
            string labelVar = null;
            if (dr.Table.Columns.Contains("label"))
            {
                labelVar = dr["label"].ToString();
            }

            var tubeFurnace = new HeatingTubeFurnace
            {
                settingsId = (long)dr["settings_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipmentModel = fkEquipmentModelVar,
                tubeMaterial = dr["tube_material"].ToString(),
                tubeDiameter = dr["tube_diameter"] != DBNull.Value ? double.Parse(dr["tube_diameter"].ToString()) : (double?)null,
                tubeAmountOfOpenings = dr["tube_amount_of_openings"] != DBNull.Value ? int.Parse(dr["tube_amount_of_openings"].ToString()) : (int?)null,
                atmosphere = dr["atmosphere"].ToString(),
                flow = dr["flow"] != DBNull.Value ? Boolean.Parse(dr["flow"].ToString()) : (Boolean?)null,
                rampUpTime = dr["ramp_up_time"] != DBNull.Value ? double.Parse(dr["ramp_up_time"].ToString()) : (double?)null,
                temperature = dr["temperature"] != DBNull.Value ? double.Parse(dr["temperature"].ToString()) : (double?)null,
                duration = dr["duration"] != DBNull.Value ? double.Parse(dr["duration"].ToString()) : (double?)null,
                rampDownTime = dr["ramp_down_time"] != DBNull.Value ? double.Parse(dr["ramp_down_time"].ToString()) : (double?)null,
                loopCount = dr["loop_count"] != DBNull.Value ? int.Parse(dr["loop_count"].ToString()) : (int?)null,
                comment = commentVar,
                label = labelVar,
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null
            };
            return tubeFurnace;
        }
        private static HeatingTubeFurnaceExt CreateObjectExt(DataRow dr)
        {
            var tubeFurnace = CreateObject(dr);

            string equipmentModelNameVar = null;
            if (dr.Table.Columns.Contains("equipment_model_name"))
            {
                equipmentModelNameVar = dr["equipment_model_name"].ToString();
            }

            var tubeFurnaceExt = new HeatingTubeFurnaceExt(tubeFurnace)
            {
                equipmentModelName = dr.Table.Columns.Contains("equipment_model_name") ? dr["equipment_model_name"].ToString() : null
            };
            return tubeFurnaceExt;
        }
    }
}