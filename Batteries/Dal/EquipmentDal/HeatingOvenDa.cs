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
    public class HeatingOvenDa
    {
        public static List<HeatingOvenExt> GetAllHeatingOvens(long? settingsId = null, long? experimentProcessId = null, long? batchProcessId = null, int? equipmentModelId = null)
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
                    FROM heating_oven m
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

            List<HeatingOvenExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<HeatingOvenExt> GetRecentlyUsedHeatingOvens(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null, int? equipmentModelId = null)
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
                    @"SELECT max(settings_id) as settings_id, max(date_created) as date_created, fk_equipment_model, temperature, heating_time, atmosphere, comment, label
                        FROM heating_oven
                      GROUP BY fk_equipment_model, temperature, heating_time, atmosphere, comment, label
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

            List<HeatingOvenExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddHeatingOven(HeatingOven heatingOven, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.heating_oven (
fk_experiment_process,
fk_batch_process,
fk_equipment_model,
temperature,
heating_time,
atmosphere,
comment,
label,
date_created
)
                    VALUES (:epid, :bpid, :emid, :temp, :ht, :atm, :com, :lab, now()::timestamp);";


                Db.CreateParameterFunc(cmd, "@epid", heatingOven.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", heatingOven.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@emid", heatingOven.fkEquipmentModel, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@temp", heatingOven.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@ht", heatingOven.heatingTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@atm", heatingOven.atmosphere, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@com", heatingOven.comment, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", heatingOven.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting Heating Oven settings", ex);
            }

            return 0;
        }
        public static int UpdateHeatingOven(HeatingOven heatingOven)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.heating_oven
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment_model=:emid,

temperature=:temp,
heating_time=:ht,
atmosphere=:atm,
comment=:com,
label=:lab,
date_created=now()::timestamp

                        WHERE settings_id=:sid;";
                Db.CreateParameterFunc(cmd, "@epid", heatingOven.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", heatingOven.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@emid", heatingOven.fkEquipmentModel, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@temp", heatingOven.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@ht", heatingOven.heatingTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@atm", heatingOven.atmosphere, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@com", heatingOven.comment, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", heatingOven.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@sid", heatingOven.settingsId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Heating Oven settings", ex);
            }
            return 0;
        }
        public static int DeleteHeatingOven(long settingsId)
        {
            try
            {
                var cmd = Db.CreateCommand();

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.heating_oven
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
        public static HeatingOven CreateObject(DataRow dr)
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

            var heatingOven = new HeatingOven
            {
                settingsId = (long)dr["settings_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipmentModel = fkEquipmentModelVar,
                temperature = dr["temperature"] != DBNull.Value ? double.Parse(dr["temperature"].ToString()) : (double?)null,
                heatingTime = dr["heating_time"] != DBNull.Value ? double.Parse(dr["heating_time"].ToString()) : (double?)null,
                atmosphere = dr["atmosphere"].ToString(),
                comment = commentVar,
                label = labelVar,
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null                

            };
            return heatingOven;
        }
        private static HeatingOvenExt CreateObjectExt(DataRow dr)
        {
            var heatingOven = CreateObject(dr);

            var heatingOvenExt = new HeatingOvenExt(heatingOven)
            {
                equipmentModelName = dr.Table.Columns.Contains("equipment_model_name") ? dr["equipment_model_name"].ToString() : null
            };
            return heatingOvenExt;
        }
    }
}