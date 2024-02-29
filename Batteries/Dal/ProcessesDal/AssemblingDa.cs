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
    public class AssemblingDa
    {
        public static List<AssemblingExt> GetAllAssemblings(long? assemblingId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM assembling c
                        left join experiment_process ep on c.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on c.fk_batch_process = bp.batch_process_id
                        left join equipment eq on c.fk_equipment = eq.equipment_id

                    WHERE (c.assembling_id = :cid or :cid is null) and
                        (c.fk_experiment_process = :epid or :epid is null) and
                        (c.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@cid", assemblingId, NpgsqlDbType.Bigint);
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

            List<AssemblingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<AssemblingExt> GetRecentlyUsedAssemblings(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(assembling_id) as assembling_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
time,
comments,
label
                      FROM assembling
                          LEFT JOIN equipment e on assembling.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name, 
time,
comments,
label
                      ORDER BY max(assembling_id) DESC LIMIT 10;";

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

            List<AssemblingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddAssembling(Assembling assembling, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.assembling (
fk_experiment_process, fk_batch_process, fk_equipment, date_created,
time,
comments,
label
)
                    VALUES (:epid, :bpid, :eid, now()::timestamp, 
:t,
:com,
:lab);";

                Db.CreateParameterFunc(cmd, "@epid", assembling.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", assembling.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", assembling.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@t", assembling.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@com", assembling.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", assembling.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting process", ex);
            }

            return 0;
        }
        public static int UpdateAssembling(Assembling assembling)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.assembling
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
time=:t,
comments=:com,
label=:lab
                        WHERE assembling_id=:cid;";
                Db.CreateParameterFunc(cmd, "@epid", assembling.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", assembling.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", assembling.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@t", assembling.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@com", assembling.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", assembling.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@cid", assembling.assemblingId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating process info", ex);
            }
            return 0;
        }
        public static Assembling CreateObject(DataRow dr)
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

            var assembling = new Assembling
            {
                assemblingId = (long)dr["assembling_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                time = dr["time"] != DBNull.Value ? double.Parse(dr["time"].ToString()) : (double?)null,
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,

            };
            return assembling;
        }
        private static AssemblingExt CreateObjectExt(DataRow dr)
        {
            var assembling = CreateObject(dr);

            var assemblingExt = new AssemblingExt(assembling)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return assemblingExt;
        }


    }
}