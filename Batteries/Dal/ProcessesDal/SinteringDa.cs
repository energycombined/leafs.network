using Batteries.Models.ProcessModels;
using Batteries.Models.Responses.ProcessModels;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Batteries.Dal.Base;
using Npgsql;
using NpgsqlTypes;

namespace Batteries.Dal.ProcessesDal
{
    public class SinteringDa
    {
        public static List<SinteringExt> GetAllSinterings(long? sinteringId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM sintering c
                        left join experiment_process ep on c.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on c.fk_batch_process = bp.batch_process_id
                        left join equipment eq on c.fk_equipment = eq.equipment_id

                    WHERE (c.sintering_id = :cid or :cid is null) and
                        (c.fk_experiment_process = :epid or :epid is null) and
                        (c.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@cid", sinteringId, NpgsqlDbType.Bigint);
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

            List<SinteringExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<SinteringExt> GetRecentlyUsedSinterings(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(sintering_id) as sintering_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
temperature,
time,
ramp_up_time,
ramp_down_time,
plateau_time,
atmosphere,
comments,
label
                      FROM sintering
                          LEFT JOIN equipment e on sintering.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name,
temperature,
time,
ramp_up_time,
ramp_down_time,
plateau_time,
atmosphere,
comments,
label
                      ORDER BY max(sintering_id) DESC LIMIT 10;";

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

            List<SinteringExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddSintering(Sintering sintering, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.sintering (
fk_experiment_process, fk_batch_process, fk_equipment, date_created,
temperature,
time,
ramp_up_time,
ramp_down_time,
plateau_time,
atmosphere,
comments,
label
)
                    VALUES (:epid, :bpid, :eid, now()::timestamp,
:temperature,
:time,
:ramp_up_time,
:ramp_down_time,
:plateau_time,
:atmosphere,
:comments,
:label
);";

                Db.CreateParameterFunc(cmd, "@epid", sintering.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", sintering.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", sintering.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@temperature", sintering.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@time", sintering.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@ramp_up_time", sintering.rampUpTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@ramp_down_time", sintering.rampDownTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@plateau_time", sintering.plateauTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@atmosphere", sintering.atmosphere, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@comments", sintering.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", sintering.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting process", ex);
            }

            return 0;
        }
        public static int UpdateSintering(Sintering sintering)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.sintering
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
temperature=:temperature,
time=:time,
ramp_up_time=:ramp_up_time,
ramp_down_time=:ramp_down_time,
plateau_time=:plateau_time,
atmosphere=:atmosphere,
comments=:comments,
label=:label
                        WHERE sintering_id=:cid;";
                Db.CreateParameterFunc(cmd, "@epid", sintering.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", sintering.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", sintering.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@temperature", sintering.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@time", sintering.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@ramp_up_time", sintering.rampUpTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@ramp_down_time", sintering.rampDownTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@plateau_time", sintering.plateauTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@atmosphere", sintering.atmosphere, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@comments", sintering.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", sintering.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@cid", sintering.sinteringId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating process info", ex);
            }
            return 0;
        }
        public static Sintering CreateObject(DataRow dr)
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

            var sintering = new Sintering
            {
                sinteringId = (long)dr["sintering_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                temperature = dr["temperature"] != DBNull.Value ? double.Parse(dr["temperature"].ToString()) : (double?)null,
                time = dr["time"] != DBNull.Value ? double.Parse(dr["time"].ToString()) : (double?)null,
                rampUpTime = dr["ramp_up_time"] != DBNull.Value ? double.Parse(dr["ramp_up_time"].ToString()) : (double?)null,
                rampDownTime = dr["ramp_down_time"] != DBNull.Value ? double.Parse(dr["ramp_down_time"].ToString()) : (double?)null,
                plateauTime = dr["plateau_time"] != DBNull.Value ? double.Parse(dr["plateau_time"].ToString()) : (double?)null,
                atmosphere = dr["atmosphere"].ToString(),
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,

            };
            return sintering;
        }
        private static SinteringExt CreateObjectExt(DataRow dr)
        {
            var sintering = CreateObject(dr);

            var sinteringExt = new SinteringExt(sintering)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return sinteringExt;
        }
    }
}