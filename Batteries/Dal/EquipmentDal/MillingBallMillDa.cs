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
    public class MillingBallMillDa
    {
        public static List<MillingBallMillExt> GetAllMillingBallMills(long? settingsId = null, long? experimentProcessId = null, long? batchProcessId = null, int? equipmentModelId = null)
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
                    FROM milling_ball_mill m
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

            List<MillingBallMillExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<MillingBallMillExt> GetRecentlyUsedMillingBallMills(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null, int? equipmentModelId = null)
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
                        FROM milling_ball_mill
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

            List<MillingBallMillExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddMillingBallMill(MillingBallMill millingBallMill, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.milling_ball_mill (
fk_experiment_process,
fk_batch_process,
fk_equipment_model,
ball_powder_ratio,
milling_speed,
milling_time,
resting_time,
loop_count,
cup_volume,
cup_material,
balls_size,
balls_material,
amount_of_balls,
comment,
label,
date_created
)
                    VALUES (:epid, :bpid, :emid, :bpr, :ms, :mt, :rt, :lc, :cv, :cm, :bs, :bm, :aob, :com, :lab, now()::timestamp);";


                Db.CreateParameterFunc(cmd, "@epid", millingBallMill.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", millingBallMill.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@emid", millingBallMill.fkEquipmentModel, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bpr", millingBallMill.ballPowderRatio, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@ms", millingBallMill.millingSpeed, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@mt", millingBallMill.millingTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@rt", millingBallMill.restingTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@lc", millingBallMill.loopCount, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@cv", millingBallMill.cupVolume, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@cm", millingBallMill.cupMaterial, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@bs", millingBallMill.ballsSize, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@bm", millingBallMill.ballsMaterial, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@aob", millingBallMill.amountOfBalls, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@com", millingBallMill.comment, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", millingBallMill.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting Ball Mill settings", ex);
            }

            return 0;
        }
        public static int UpdateMillingBallMill(MillingBallMill millingBallMill)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.milling_ball_mill
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment_model=:emid,

ball_powder_ratio=:bpr,
milling_speed=:ms,
milling_time=:mt,
resting_time=:rt,
loop_count=:lc,
cup_volume=:cv,
cup_material=:cm,
balls_size=:bs,
balls_material=:bm,
amount_of_balls=:aob,
comment=:com,
label=:lab
                        WHERE settings_id=:sid;";
                Db.CreateParameterFunc(cmd, "@epid", millingBallMill.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", millingBallMill.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@emid", millingBallMill.fkEquipmentModel, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bpr", millingBallMill.ballPowderRatio, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@ms", millingBallMill.millingSpeed, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@mt", millingBallMill.millingTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@rt", millingBallMill.restingTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@lc", millingBallMill.loopCount, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@cv", millingBallMill.cupVolume, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@cm", millingBallMill.cupMaterial, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@bs", millingBallMill.ballsSize, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@bm", millingBallMill.ballsMaterial, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@aob", millingBallMill.amountOfBalls, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@com", millingBallMill.comment, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", millingBallMill.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@sid", millingBallMill.settingsId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Ball Mill settings", ex);
            }
            return 0;
        }
        public static int DeleteMillingBallMill(long settingsId)
        {
            try
            {
                var cmd = Db.CreateCommand();

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.milling_ball_mill
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
        public static MillingBallMill CreateObject(DataRow dr)
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

            var millingBallMill = new MillingBallMill
            {
                settingsId = (long)dr["settings_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipmentModel = fkEquipmentModelVar,
                ballPowderRatio = dr["ball_powder_ratio"] != DBNull.Value ? double.Parse(dr["ball_powder_ratio"].ToString()) : (double?)null,
                millingSpeed = dr["milling_speed"] != DBNull.Value ? double.Parse(dr["milling_speed"].ToString()) : (double?)null,
                millingTime = dr["milling_time"] != DBNull.Value ? double.Parse(dr["milling_time"].ToString()) : (double?)null,
                restingTime = dr["resting_time"] != DBNull.Value ? double.Parse(dr["resting_time"].ToString()) : (double?)null,
                loopCount = dr["loop_count"] != DBNull.Value ? int.Parse(dr["loop_count"].ToString()) : (int?)null,
                cupVolume = dr["cup_volume"] != DBNull.Value ? double.Parse(dr["cup_volume"].ToString()) : (double?)null,
                cupMaterial = dr["cup_material"].ToString(),
                ballsSize = dr["balls_size"] != DBNull.Value ? double.Parse(dr["balls_size"].ToString()) : (double?)null,
                ballsMaterial = dr["balls_material"].ToString(),
                amountOfBalls = dr["amount_of_balls"] != DBNull.Value ? int.Parse(dr["amount_of_balls"].ToString()) : (int?)null,
                comment = commentVar,
                label = labelVar,
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null
            };
            return millingBallMill;
        }
        private static MillingBallMillExt CreateObjectExt(DataRow dr)
        {
            var millingBallMill = CreateObject(dr);

            var millingBallMillExt = new MillingBallMillExt(millingBallMill)
            {
                equipmentModelName = dr.Table.Columns.Contains("equipment_model_name") ? dr["equipment_model_name"].ToString() : null
            };
            return millingBallMillExt;
        }
    }
}