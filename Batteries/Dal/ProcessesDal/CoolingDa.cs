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
    public class CoolingDa
    {
        public static List<CoolingExt> GetAllCoolings(long? coolingId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM cooling c
                        left join experiment_process ep on c.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on c.fk_batch_process = bp.batch_process_id
                        left join equipment eq on c.fk_equipment = eq.equipment_id

                    WHERE (c.cooling_id = :cid or :cid is null) and
                        (c.fk_experiment_process = :epid or :epid is null) and
                        (c.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@cid", coolingId, NpgsqlDbType.Bigint);
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

            List<CoolingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<CoolingExt> GetRecentlyUsedCoolings(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(cooling_id) as cooling_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
temperature,
time,
vacuum,
comments,
label
                      FROM cooling
                          LEFT JOIN equipment e on cooling.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name,
temperature,
time,
vacuum,
comments,
label
                      ORDER BY max(cooling_id) DESC LIMIT 10;";

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

            List<CoolingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddCooling(Cooling cooling, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.cooling (
fk_experiment_process, fk_batch_process, fk_equipment, date_created,
temperature,
time,
vacuum,
comments,
label
)
                    VALUES (:epid, :bpid, :eid, now()::timestamp,
:temp,
:t,
:vc,
:com,
:lab
);";

                Db.CreateParameterFunc(cmd, "@epid", cooling.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", cooling.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", cooling.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@temp", cooling.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@t", cooling.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@vc", cooling.vacuum, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@com", cooling.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", cooling.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting process", ex);
            }

            return 0;
        }
        public static int UpdateCooling(Cooling cooling)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.cooling
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
temperature=:temp,
time=:t,
vacuum=:vc,
comments=:com,
label=:lab
                        WHERE cooling_id=:cid;";
                Db.CreateParameterFunc(cmd, "@epid", cooling.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", cooling.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", cooling.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@temp", cooling.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@t", cooling.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@vc", cooling.vacuum, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@com", cooling.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", cooling.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@cid", cooling.coolingId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating process info", ex);
            }
            return 0;
        }
        public static Cooling CreateObject(DataRow dr)
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

            var cooling = new Cooling
            {
                coolingId = (long)dr["cooling_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                temperature = dr["temperature"] != DBNull.Value ? double.Parse(dr["temperature"].ToString()) : (double?)null,
                time = dr["time"] != DBNull.Value ? double.Parse(dr["time"].ToString()) : (double?)null,
                vacuum = dr["vacuum"] != DBNull.Value ? double.Parse(dr["vacuum"].ToString()) : (double?)null,
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,

            };
            return cooling;
        }
        private static CoolingExt CreateObjectExt(DataRow dr)
        {
            var cooling = CreateObject(dr);

            var coolingExt = new CoolingExt(cooling)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return coolingExt;
        }
    }
}