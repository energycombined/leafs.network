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
    public class MixingPlanetaryMixerDa
    {
        public static List<MixingPlanetaryMixerExt> GetAllMixingPlanetaryMixers(long? settingsId = null, long? experimentProcessId = null, long? batchProcessId = null, int? equipmentModelId = null)
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
                    FROM mixing_planetary_mixer m
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

            List<MixingPlanetaryMixerExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<MixingPlanetaryMixerExt> GetRecentlyUsedMixingPlanetaryMixers(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null, int? equipmentModelId = null)
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
                    @"SELECT max(settings_id) as settings_id, max(date_created) as date_created, fk_equipment_model, ball_powder_ratio, milling_speed, milling_time, resting_time, loop_count, cup_volume, cup_material, balls_size, balls_material, amount_of_balls, comment, label
                        FROM mixing_planetary_mixer
                      GROUP BY fk_equipment_model, ball_powder_ratio, milling_speed, milling_time, resting_time, loop_count, cup_volume, cup_material, balls_size, balls_material, amount_of_balls, comment, label
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

            List<MixingPlanetaryMixerExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddMixingPlanetaryMixer(MixingPlanetaryMixer mixingPlanetaryMixer, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.mixing_planetary_mixer (
fk_experiment_process,
fk_batch_process,
fk_equipment_model,
container_diameter_size,
amount_of_containers,
program_channel,
manual,
rotation_speed,
rotation_time,
rest_time,
comment,
label,
date_created
)
                    VALUES (:epid, :bpid, :emid, :cds, :aoc, :pc, :man, :rs, :rot, :ret, :comm, :lab, now()::timestamp);";


                Db.CreateParameterFunc(cmd, "@epid", mixingPlanetaryMixer.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", mixingPlanetaryMixer.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@emid", mixingPlanetaryMixer.fkEquipmentModel, NpgsqlDbType.Integer);

                Db.CreateParameterFunc(cmd, "@cds", mixingPlanetaryMixer.containerDiameterSize, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@aoc", mixingPlanetaryMixer.amountOfContainers, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pc", mixingPlanetaryMixer.programChannel, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@man", mixingPlanetaryMixer.manual, NpgsqlDbType.Boolean);
                Db.CreateParameterFunc(cmd, "@rs", mixingPlanetaryMixer.rotationSpeed, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@rot", mixingPlanetaryMixer.rotationTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@ret", mixingPlanetaryMixer.restTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comm", mixingPlanetaryMixer.comment, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", mixingPlanetaryMixer.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting Planetary Mixer settings", ex);
            }

            return 0;
        }
        public static int UpdateMixingPlanetaryMixer(MixingPlanetaryMixer mixingPlanetaryMixer)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.mixing_planetary_mixer
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment_model=:emid,

container_diameter_size=:cds,
amount_of_containers=:aoc,
program_channel=:pc,
manual=:man,
rotation_speed=:rs,
rotation_time=:rot,
rest_time=:ret,
comment=:comm,
label=:lab

                        WHERE settings_id=:sid;";
                Db.CreateParameterFunc(cmd, "@epid", mixingPlanetaryMixer.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", mixingPlanetaryMixer.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@emid", mixingPlanetaryMixer.fkEquipmentModel, NpgsqlDbType.Integer);
                
                Db.CreateParameterFunc(cmd, "@cds", mixingPlanetaryMixer.containerDiameterSize, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@aoc", mixingPlanetaryMixer.amountOfContainers, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pc", mixingPlanetaryMixer.programChannel, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@man", mixingPlanetaryMixer.manual, NpgsqlDbType.Boolean);
                Db.CreateParameterFunc(cmd, "@rs", mixingPlanetaryMixer.rotationSpeed, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@rot", mixingPlanetaryMixer.rotationTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@ret", mixingPlanetaryMixer.restTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comm", mixingPlanetaryMixer.comment, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", mixingPlanetaryMixer.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@sid", mixingPlanetaryMixer.settingsId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Planetary Mixer settings", ex);
            }
            return 0;
        }
        public static int DeleteMixingPlanetaryMixer(long settingsId)
        {
            try
            {
                var cmd = Db.CreateCommand();

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.mixing_planetary_mixer
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
        public static MixingPlanetaryMixer CreateObject(DataRow dr)
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

            var mixingPlanetaryMixer = new MixingPlanetaryMixer
            {
                settingsId = (long)dr["settings_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipmentModel = fkEquipmentModelVar,

                containerDiameterSize = dr["container_diameter_size"] != DBNull.Value ? double.Parse(dr["container_diameter_size"].ToString()) : (double?)null,
                amountOfContainers = dr["amount_of_containers"] != DBNull.Value ? int.Parse(dr["amount_of_containers"].ToString()) : (int?)null,
                programChannel = dr["program_channel"] != DBNull.Value ? int.Parse(dr["program_channel"].ToString()) : (int?)null,
                manual = dr["manual"] != DBNull.Value ? Boolean.Parse(dr["manual"].ToString()) : (Boolean?)null,
                rotationSpeed = dr["rotation_speed"] != DBNull.Value ? double.Parse(dr["rotation_speed"].ToString()) : (double?)null,
                rotationTime = dr["rotation_time"] != DBNull.Value ? double.Parse(dr["rotation_time"].ToString()) : (double?)null,
                restTime = dr["rest_time"] != DBNull.Value ? double.Parse(dr["rest_time"].ToString()) : (double?)null,
                comment = commentVar,
                label = labelVar,
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null
            };
            return mixingPlanetaryMixer;
        }
        private static MixingPlanetaryMixerExt CreateObjectExt(DataRow dr)
        {
            var mixingPlanetaryMixer = CreateObject(dr);

            var mixingPlanetaryMixerExt = new MixingPlanetaryMixerExt(mixingPlanetaryMixer)
            {
                equipmentModelName = dr.Table.Columns.Contains("equipment_model_name") ? dr["equipment_model_name"].ToString() : null
            };
            return mixingPlanetaryMixerExt;
        }
    }
}