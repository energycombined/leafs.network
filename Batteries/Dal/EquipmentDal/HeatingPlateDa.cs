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
    public class HeatingPlateDa
    {
        public static List<HeatingPlateExt> GetAllHeatingPlates(long? settingsId = null, long? experimentProcessId = null, long? batchProcessId = null, int? equipmentModelId = null)
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
                    FROM heating_plate m
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

            List<HeatingPlateExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<HeatingPlateExt> GetRecentlyUsedHeatingPlates(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null, int? equipmentModelId = null)
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
                    @"SELECT max(settings_id) as settings_id, max(date_created) as date_created, fk_equipment_model, temperature, heating_time, stirring, stirring_speed, stir_bar_size, atmosphere, flow, flow_rate, comment, label
                        FROM heating_plate
                      GROUP BY fk_equipment_model, temperature, heating_time, stirring, stirring_speed, stir_bar_size, atmosphere, flow, flow_rate, comment, label
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

            List<HeatingPlateExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddHeatingPlate(HeatingPlate heatingPlate, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.heating_plate (
fk_experiment_process,
fk_batch_process,
fk_equipment_model,
temperature,
heating_time,
stirring,
stirring_speed,
stir_bar_size,
atmosphere,
flow,
flow_rate,
comment,
label,
date_created
)
                    VALUES (:epid, :bpid, :emid, :temp, :ht, :stir, :ss, :sbs, :atm, :fl, :fr, :com, :lab, now()::timestamp);";


                Db.CreateParameterFunc(cmd, "@epid", heatingPlate.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", heatingPlate.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@emid", heatingPlate.fkEquipmentModel, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@temp", heatingPlate.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@ht", heatingPlate.heatingTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@stir", heatingPlate.stirring, NpgsqlDbType.Boolean);
                Db.CreateParameterFunc(cmd, "@ss", heatingPlate.stirringSpeed, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@sbs", heatingPlate.stirBarSize, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@atm", heatingPlate.atmosphere, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@fl", heatingPlate.flow, NpgsqlDbType.Boolean);
                Db.CreateParameterFunc(cmd, "@fr", heatingPlate.flowRate, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@com", heatingPlate.comment, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", heatingPlate.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting Heating Plate settings", ex);
            }

            return 0;
        }
        public static int UpdateHeatingPlate(HeatingPlate heatingPlate)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.heating_plate
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment_model=emid,
temperature=:temp, 
heating_time=:ht, 
stirring=:stir, 
stirring_speed=:ss, 
stir_bar_size=:sbs, 
atmosphere=:atm, 
flow=:fl, 
flow_rate=:fr, 
comment=:com,
label=:lab,
date_created=now()::timestamp


                        WHERE settings_id=:sid;";
                Db.CreateParameterFunc(cmd, "@epid", heatingPlate.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", heatingPlate.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@emid", heatingPlate.fkEquipmentModel, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@temp", heatingPlate.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@ht", heatingPlate.heatingTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@stir", heatingPlate.stirring, NpgsqlDbType.Boolean);
                Db.CreateParameterFunc(cmd, "@ss", heatingPlate.stirringSpeed, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@sbs", heatingPlate.stirBarSize, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@atm", heatingPlate.atmosphere, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@fl", heatingPlate.flow, NpgsqlDbType.Boolean);
                Db.CreateParameterFunc(cmd, "@fr", heatingPlate.flowRate, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@com", heatingPlate.comment, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", heatingPlate.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@sid", heatingPlate.settingsId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Heating Plate settings", ex);
            }
            return 0;
        }
        public static int DeleteHeatingPlate(long settingsId)
        {
            try
            {
                var cmd = Db.CreateCommand();

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.heating_plate
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
        public static HeatingPlate CreateObject(DataRow dr)
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

            var heatingPlate = new HeatingPlate
            {
                settingsId = (long)dr["settings_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipmentModel = fkEquipmentModelVar,
                temperature = dr["temperature"] != DBNull.Value ? double.Parse(dr["temperature"].ToString()) : (double?)null,
                heatingTime = dr["heating_time"] != DBNull.Value ? double.Parse(dr["heating_time"].ToString()) : (double?)null,
                stirring = dr["stirring"] != DBNull.Value ? Boolean.Parse(dr["stirring"].ToString()) : (Boolean?)null,
                stirringSpeed = dr["stirring_speed"] != DBNull.Value ? int.Parse(dr["stirring_speed"].ToString()) : (int?)null,
                stirBarSize = dr["stir_bar_size"] != DBNull.Value ? int.Parse(dr["stir_bar_size"].ToString()) : (int?)null,
                atmosphere = dr["atmosphere"].ToString(),
                flow = dr["flow"] != DBNull.Value ? Boolean.Parse(dr["flow"].ToString()) : (Boolean?)null,
                flowRate = dr["flow_rate"] != DBNull.Value ? double.Parse(dr["flow_rate"].ToString()) : (double?)null,
                comment = commentVar,
                label = labelVar,
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null
                
            };
            return heatingPlate;
        }
        private static HeatingPlateExt CreateObjectExt(DataRow dr)
        {
            var heatingPlate = CreateObject(dr);
            
            var heatingPlateExt = new HeatingPlateExt(heatingPlate)
            {
                equipmentModelName = dr.Table.Columns.Contains("equipment_model_name") ? dr["equipment_model_name"].ToString() : null
            };
            return heatingPlateExt;
        }
    }
}