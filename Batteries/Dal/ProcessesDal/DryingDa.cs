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
    public class DryingDa
    {
        public static List<DryingExt> GetAllDryings(long? dryingId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM drying c
                        left join experiment_process ep on c.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on c.fk_batch_process = bp.batch_process_id
                        left join equipment eq on c.fk_equipment = eq.equipment_id

                    WHERE (c.drying_id = :cid or :cid is null) and
                        (c.fk_experiment_process = :epid or :epid is null) and
                        (c.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@cid", dryingId, NpgsqlDbType.Bigint);
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

            List<DryingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<DryingExt> GetRecentlyUsedDryings(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(drying_id) as drying_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
atmosphere,
gas_flow,
temperature,
time,
comments,
label
                      FROM drying
                          LEFT JOIN equipment e on drying.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name,
atmosphere,
gas_flow,
temperature,
time,
comments,
label
                      ORDER BY max(drying_id) DESC LIMIT 10;";

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

            List<DryingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddDrying(Drying drying, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.drying (
fk_experiment_process, fk_batch_process, fk_equipment, date_created,
atmosphere,
gas_flow,
temperature,
time,
comments,
label
)
                    VALUES (:epid, :bpid, :eid, now()::timestamp,
:atm,
:gf,
:temp,
:t,
:com,
:lab
);";

                Db.CreateParameterFunc(cmd, "@epid", drying.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", drying.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", drying.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@atm", drying.atmosphere, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@gf", drying.gasFlow, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@temp", drying.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@t", drying.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@com", drying.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", drying.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting process", ex);
            }

            return 0;
        }
        public static int UpdateDrying(Drying drying)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.drying
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
atmosphere=:atm,
gas_flow=:gf,
temperature=:temp,
time=:t,
comments=:com,
label=:lab
                        WHERE drying_id=:cid;";
                Db.CreateParameterFunc(cmd, "@epid", drying.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", drying.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", drying.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@atm", drying.atmosphere, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@gf", drying.gasFlow, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@temp", drying.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@t", drying.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@com", drying.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", drying.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@cid", drying.dryingId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating process info", ex);
            }
            return 0;
        }
        public static Drying CreateObject(DataRow dr)
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

            var drying = new Drying
            {
                dryingId = (long)dr["drying_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                atmosphere = dr["atmosphere"].ToString(),
                gasFlow = dr["gas_flow"] != DBNull.Value ? double.Parse(dr["gas_flow"].ToString()) : (double?)null,
                temperature = dr["temperature"] != DBNull.Value ? double.Parse(dr["temperature"].ToString()) : (double?)null,
                time = dr["time"] != DBNull.Value ? double.Parse(dr["time"].ToString()) : (double?)null,
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,
            };
            return drying;
        }
        private static DryingExt CreateObjectExt(DataRow dr)
        {
            var drying = CreateObject(dr);

            var dryingExt = new DryingExt(drying)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return dryingExt;
        }
    }
}