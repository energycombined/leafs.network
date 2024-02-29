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
    public class MixingHotPlateStirrerDa
    {
        public static List<MixingHotPlateStirrerExt> GetAllMixingHotPlateStirrers(long? settingsId = null, long? experimentProcessId = null, long? batchProcessId = null, int? equipmentModelId = null)
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
                    FROM mixing_hot_plate_stirrer m
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

            List<MixingHotPlateStirrerExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<MixingHotPlateStirrerExt> GetRecentlyUsedMixingHotPlateStirrers(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null, int? equipmentModelId = null)
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
                    @"SELECT max(settings_id) as settings_id, max(date_created) as date_created, fk_equipment_model, rotation_speed, rotation_time, rest_time, temperature, comment, label
                        FROM mixing_hot_plate_stirrer
                      GROUP BY fk_equipment_model, rotation_speed, rotation_time, rest_time, temperature, comment, label
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

            List<MixingHotPlateStirrerExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddMixingHotPlateStirrer(MixingHotPlateStirrer mixingHotPlateStirrer, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.mixing_hot_plate_stirrer (
fk_experiment_process,
fk_batch_process,
fk_equipment_model,
rotation_speed,
rotation_time,
rest_time,
temperature,
comment,
label,
date_created
)
                    VALUES (:epid, :bpid, :emid, :rs, :rot, :ret, :temp, :com, :lab, now()::timestamp);";


                Db.CreateParameterFunc(cmd, "@epid", mixingHotPlateStirrer.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", mixingHotPlateStirrer.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@emid", mixingHotPlateStirrer.fkEquipmentModel, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rs", mixingHotPlateStirrer.rotationSpeed, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@rot", mixingHotPlateStirrer.rotationTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@ret", mixingHotPlateStirrer.restTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@temp", mixingHotPlateStirrer.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@com", mixingHotPlateStirrer.comment, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", mixingHotPlateStirrer.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting Hot Plate Stirrer settings", ex);
            }

            return 0;
        }
        public static int UpdateMixingHotPlateStirrer(MixingHotPlateStirrer mixingHotPlateStirrer)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.mixing_hot_plate_stirrer
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment_model=:emid,

rotation_speed =:rs,
rotation_time =:rot,
rest_time =:ret,
temperature =:temp,
comment=:com,
label=:lab
                        WHERE settings_id=:sid;";
                Db.CreateParameterFunc(cmd, "@epid", mixingHotPlateStirrer.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", mixingHotPlateStirrer.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@emid", mixingHotPlateStirrer.fkEquipmentModel, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rs", mixingHotPlateStirrer.rotationSpeed, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@rot", mixingHotPlateStirrer.rotationTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@ret", mixingHotPlateStirrer.restTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@temp", mixingHotPlateStirrer.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@com", mixingHotPlateStirrer.comment, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", mixingHotPlateStirrer.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@sid", mixingHotPlateStirrer.settingsId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Hot Plate Stirrer settings", ex);
            }
            return 0;
        }
        public static int DeleteMixingHotPlateStirrer(long settingsId)
        {
            try
            {
                var cmd = Db.CreateCommand();

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.mixing_hot_plate_stirrer
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
        public static MixingHotPlateStirrer CreateObject(DataRow dr)
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

            var mixingHotPlateStirrer = new MixingHotPlateStirrer
            {
                settingsId = (long)dr["settings_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipmentModel = fkEquipmentModelVar,
                rotationSpeed = dr["rotation_speed"] != DBNull.Value ? double.Parse(dr["rotation_speed"].ToString()) : (double?)null,
                rotationTime = dr["rotation_time"] != DBNull.Value ? double.Parse(dr["rotation_time"].ToString()) : (double?)null,
                restTime = dr["rest_time"] != DBNull.Value ? double.Parse(dr["rest_time"].ToString()) : (double?)null,
                temperature = dr["temperature"] != DBNull.Value ? double.Parse(dr["temperature"].ToString()) : (double?)null,
                comment = commentVar,
                label = labelVar,
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null                
            };
            return mixingHotPlateStirrer;
        }
        private static MixingHotPlateStirrerExt CreateObjectExt(DataRow dr)
        {
            var mixingHotPlateStirrer = CreateObject(dr);

            var mixingHotPlateStirrerExt = new MixingHotPlateStirrerExt(mixingHotPlateStirrer)
            {
                equipmentModelName = dr.Table.Columns.Contains("equipment_model_name") ? dr["equipment_model_name"].ToString() : null
            };
            return mixingHotPlateStirrerExt;
        }
    }
}