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
    public class PastingDa
    {
        public static List<PastingExt> GetAllPastings(long? pastingId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM pasting c
                        left join experiment_process ep on c.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on c.fk_batch_process = bp.batch_process_id
                        left join equipment eq on c.fk_equipment = eq.equipment_id

                    WHERE (c.pasting_id = :cid or :cid is null) and
                        (c.fk_experiment_process = :epid or :epid is null) and
                        (c.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@cid", pastingId, NpgsqlDbType.Bigint);
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

            List<PastingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<PastingExt> GetRecentlyUsedPastings(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(pasting_id) as pasting_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
thickness,
roll_speed,
temperature,
substrate,
time,
comments,
label
                      FROM pasting
                          LEFT JOIN equipment e on pasting.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name,
thickness,
roll_speed,
temperature,
substrate,
time,
comments,
label
                      ORDER BY max(pasting_id) DESC LIMIT 10;";

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

            List<PastingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddPasting(Pasting pasting, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.pasting (
fk_experiment_process, fk_batch_process, fk_equipment, date_created,
thickness,
roll_speed,
temperature,
substrate,
time,
comments,
label
)
                    VALUES (:epid, :bpid, :eid, now()::timestamp,
:thickness,
:roll_speed,
:temperature,
:substrate,
:time,
:comments,
:label
);";

                Db.CreateParameterFunc(cmd, "@epid", pasting.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", pasting.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", pasting.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@thickness", pasting.thickness, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@roll_speed", pasting.rollSpeed, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@temperature", pasting.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@substrate", pasting.substrate, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@time", pasting.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comments", pasting.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", pasting.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting process", ex);
            }

            return 0;
        }
        public static int UpdatePasting(Pasting pasting)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.pasting
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
thickness=:thickness,
roll_speed=:roll_speed,
temperature=:temperature,
substrate=:substrate,
time=:time,
comments=:comments,
label=:label,
                        WHERE pasting_id=:cid;";
                Db.CreateParameterFunc(cmd, "@epid", pasting.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", pasting.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", pasting.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@thickness", pasting.thickness, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@roll_speed", pasting.rollSpeed, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@temperature", pasting.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@substrate", pasting.substrate, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@time", pasting.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comments", pasting.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", pasting.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@cid", pasting.pastingId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating process info", ex);
            }
            return 0;
        }
        public static Pasting CreateObject(DataRow dr)
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

            var pasting = new Pasting
            {
                pastingId = (long)dr["pasting_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                thickness = dr["thickness"] != DBNull.Value ? double.Parse(dr["thickness"].ToString()) : (double?)null,
                rollSpeed = dr["roll_speed"] != DBNull.Value ? double.Parse(dr["roll_speed"].ToString()) : (double?)null,
                temperature = dr["temperature"] != DBNull.Value ? double.Parse(dr["temperature"].ToString()) : (double?)null,
                substrate = dr["substrate"].ToString(),
                time = dr["time"] != DBNull.Value ? double.Parse(dr["time"].ToString()) : (double?)null,
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,

            };
            return pasting;
        }
        private static PastingExt CreateObjectExt(DataRow dr)
        {
            var pasting = CreateObject(dr);

            var pastingExt = new PastingExt(pasting)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return pastingExt;
        }
    }
}