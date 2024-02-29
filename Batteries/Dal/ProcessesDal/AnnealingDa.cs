using Batteries.Models.ProcessModels;
using Batteries.Models.Responses.ProcessModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Batteries.Dal.Base;
using Npgsql;
using NpgsqlTypes;

namespace Batteries.Dal.ProcessesDal
{
    public class AnnealingDa
    {
        public static List<AnnealingExt> GetAllAnnealings(long? annealingId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM annealing c
                        left join experiment_process ep on c.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on c.fk_batch_process = bp.batch_process_id
                        left join equipment eq on c.fk_equipment = eq.equipment_id

                    WHERE (c.annealing_id = :cid or :cid is null) and
                        (c.fk_experiment_process = :epid or :epid is null) and
                        (c.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@cid", annealingId, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@epid", experimentProcessId, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", batchProcessId, NpgsqlDbType.Bigint);

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

            List<AnnealingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<AnnealingExt> GetRecentlyUsedAnnealings(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(annealing_id) as annealing_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
ramp_up_time,
ramp_down_time,
plateau_time,
atmosphere,
temperature,
time,
comments,
label
                      FROM annealing
                          LEFT JOIN equipment e on annealing.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name, ramp_up_time,
ramp_down_time,
plateau_time,
atmosphere,
temperature,
time,
comments,
label
                      ORDER BY max(annealing_id) DESC LIMIT 10;";

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

            List<AnnealingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddAnnealing(Annealing annealing, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.annealing (
fk_experiment_process, fk_batch_process, fk_equipment, date_created,
ramp_up_time,
ramp_down_time,
plateau_time,
atmosphere,
temperature,
time,
comments,
label
)
                    VALUES (:epid, :bpid, :eid, now()::timestamp, 
:rup,
:rdt,
:pt,
:atm,
:temp,
:t,
:com,
:lab);";

                Db.CreateParameterFunc(cmd, "@epid", annealing.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", annealing.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", annealing.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rup", annealing.rampUpTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@rdt", annealing.rampDownTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@pt", annealing.plateauTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@atm", annealing.atmosphere, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@temp", annealing.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@t", annealing.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@com", annealing.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", annealing.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting annealing", ex);
            }

            return 0;
        }
        public static int UpdateAnnealing(Annealing annealing)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.annealing
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
ramp_up_time=:rup,
ramp_down_time=:rdt,
plateau_time=:pt,
atmosphere=:atm,
temperature=:temp,
time=:t,
comments=:com,
label=:lab
                        WHERE annealing_id=:cid;";
                Db.CreateParameterFunc(cmd, "@epid", annealing.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", annealing.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", annealing.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rup", annealing.rampUpTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@rdt", annealing.rampDownTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@pt", annealing.plateauTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@atm", annealing.atmosphere, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@temp", annealing.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@t", annealing.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@com", annealing.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", annealing.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@cid", annealing.annealingId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating annealing info", ex);
            }
            return 0;
        }
        public static Annealing CreateObject(DataRow dr)
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
            int? fkEquipmentVar = (int?)null;
            if (dr.Table.Columns.Contains("fk_equipment"))
            {
                fkEquipmentVar = dr["fk_equipment"] != DBNull.Value ? int.Parse(dr["fk_equipment"].ToString()) : (int?)null;
            }

            var annealing = new Annealing
            {
                annealingId = (long)dr["annealing_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,                
                rampUpTime = dr["ramp_up_time"] != DBNull.Value ? double.Parse(dr["ramp_up_time"].ToString()) : (double?)null,
                rampDownTime = dr["ramp_down_time"] != DBNull.Value ? double.Parse(dr["ramp_down_time"].ToString()) : (double?)null,
                plateauTime = dr["plateau_time"] != DBNull.Value ? double.Parse(dr["plateau_time"].ToString()) : (double?)null,
                atmosphere = dr["atmosphere"].ToString(),
                temperature = dr["temperature"] != DBNull.Value ? double.Parse(dr["temperature"].ToString()) : (double?)null,
                time = dr["time"] != DBNull.Value ? double.Parse(dr["time"].ToString()) : (double?)null,
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,
            };
            return annealing;
        }
        private static AnnealingExt CreateObjectExt(DataRow dr)
        {
            var annealing = CreateObject(dr);

            var annealingExt = new AnnealingExt(annealing)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return annealingExt;
        }

    }
}