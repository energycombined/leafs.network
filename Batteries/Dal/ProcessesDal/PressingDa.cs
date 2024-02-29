using Batteries.Dal.Base;
using Batteries.Models;
using Batteries.Models.ProcessModels;
using Batteries.Models.Responses;
using Batteries.Models.Responses.ProcessModels;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Batteries.Dal.ProcessesDal
{
    public class PressingDa
    {
        public static List<PressingExt> GetAllPressings(long? pressingId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM pressing p
                        left join experiment_process ep on p.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on p.fk_batch_process = bp.batch_process_id
                        left join equipment eq on p.fk_equipment = eq.equipment_id

                    WHERE (p.pressing_id = :pid or :pid is null) and
                        (p.fk_experiment_process = :epid or :epid is null) and
                        (p.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@pid", pressingId, NpgsqlDbType.Bigint);
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

            List<PressingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<PressingExt> GetRecentlyUsedPressings(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(pressing_id) as pressing_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
comments,
label
                      FROM pressing
                        LEFT JOIN equipment e on pressing.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name,
comments,
label
                      ORDER BY max(pressing_id) DESC LIMIT 10;";

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

            List<PressingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddPressing(Pressing pressing, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.pressing (
fk_experiment_process,
fk_batch_process,
fk_equipment,
date_created,
comments,
label
)
                    VALUES (:epid, :bpid, :eid, now()::timestamp,
:comments,
:label
);";

                Db.CreateParameterFunc(cmd, "@epid", pressing.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", pressing.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", pressing.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@comments", pressing.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", pressing.label, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@pb", pressing.substrate, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@s", pressing.substrate, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@m", pressing.material, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@v", pressing.vendor, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@pr", pressing.pressure, NpgsqlDbType.Double);
                //Db.CreateParameterFunc(cmd, "@ptime", pressing.pressingTime, NpgsqlDbType.Double);
                //Db.CreateParameterFunc(cmd, "@temp", pressing.temperature, NpgsqlDbType.Double);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting pressing", ex);
            }

            return 0;
        }
        public static int UpdatePressing(Pressing pressing)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.pressing
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid,
fk_equipment=:eid,
date_created=now()::timestamp,
comments=:comments,
label=:label

                        WHERE pressing_id=:pid;";
                Db.CreateParameterFunc(cmd, "@epid", pressing.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", pressing.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", pressing.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@comments", pressing.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", pressing.label, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@pb", pressing.substrate, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@s", pressing.substrate, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@m", pressing.material, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@v", pressing.vendor, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@pr", pressing.pressure, NpgsqlDbType.Double);
                //Db.CreateParameterFunc(cmd, "@ptime", pressing.pressingTime, NpgsqlDbType.Double);
                //Db.CreateParameterFunc(cmd, "@temp", pressing.temperature, NpgsqlDbType.Double);

                Db.CreateParameterFunc(cmd, "@pid", pressing.pressingId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating pressing info", ex);
            }
            return 0;
        }
        public static Pressing CreateObject(DataRow dr)
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

            var pressing = new Pressing
            {
                pressingId = (long)dr["pressing_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null

                //pressingBlocks = dr["pressing_blocks"].ToString(),
                //substrate = dr["substrate"].ToString(),
                //material = dr["material"].ToString(),
                //vendor = dr["vendor"].ToString(),                
                //pressure = dr["pressure"] != DBNull.Value ? double.Parse(dr["pressure"].ToString()) : (double?)null,
                //pressingTime = dr["pressing_time"] != DBNull.Value ? double.Parse(dr["pressing_time"].ToString()) : (double?)null,
                //temperature = dr["temperature"] != DBNull.Value ? double.Parse(dr["temperature"].ToString()) : (double?)null,
            };
            return pressing;
        }
        private static PressingExt CreateObjectExt(DataRow dr)
        {
            var pressing = CreateObject(dr);

            var pressingExt = new PressingExt(pressing)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return pressingExt;
        }
    }
}