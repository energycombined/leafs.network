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
    public class DropcastingDa
    {
        public static List<DropcastingExt> GetAllDropcastings(long? dropcastingId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM dropcasting c
                        left join experiment_process ep on c.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on c.fk_batch_process = bp.batch_process_id
                        left join equipment eq on c.fk_equipment = eq.equipment_id

                    WHERE (c.dropcasting_id = :cid or :cid is null) and
                        (c.fk_experiment_process = :epid or :epid is null) and
                        (c.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@cid", dropcastingId, NpgsqlDbType.Bigint);
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

            List<DropcastingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<DropcastingExt> GetRecentlyUsedDropcastings(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(dropcasting_id) as dropcasting_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
volume,
concentration,
time,
comments,
label
                      FROM dropcasting
                          LEFT JOIN equipment e on dropcasting.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name,
volume,
concentration,
time,
comments,
label
                      ORDER BY max(dropcasting_id) DESC LIMIT 10;";

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

            List<DropcastingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddDropcasting(Dropcasting dropcasting, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.dropcasting (
fk_experiment_process, fk_batch_process, fk_equipment, date_created,
volume,
concentration,
time,
comments,
label

)
                    VALUES (:epid, :bpid, :eid, now()::timestamp,
:vol,
:con,
:t,
:com,
:lab
);";

                Db.CreateParameterFunc(cmd, "@epid", dropcasting.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", dropcasting.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", dropcasting.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@vol", dropcasting.volume, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@con", dropcasting.concentration, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@t", dropcasting.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@com", dropcasting.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", dropcasting.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting process", ex);
            }

            return 0;
        }
        public static int UpdateDropcasting(Dropcasting dropcasting)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.dropcasting
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
volume:vol,
concentration:con,
time:t,
comments:com,
label:lab
                        WHERE dropcasting_id=:cid;";
                Db.CreateParameterFunc(cmd, "@epid", dropcasting.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", dropcasting.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", dropcasting.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@vol", dropcasting.volume, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@con", dropcasting.concentration, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@t", dropcasting.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@com", dropcasting.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", dropcasting.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@cid", dropcasting.dropcastingId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating process info", ex);
            }
            return 0;
        }
        public static Dropcasting CreateObject(DataRow dr)
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

            var dropcasting = new Dropcasting
            {
                dropcastingId = (long)dr["dropcasting_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                volume = dr["volume"] != DBNull.Value ? double.Parse(dr["volume"].ToString()) : (double?)null,
                concentration = dr["concentration"] != DBNull.Value ? double.Parse(dr["concentration"].ToString()) : (double?)null,
                time = dr["time"] != DBNull.Value ? double.Parse(dr["time"].ToString()) : (double?)null,
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,
            };
            return dropcasting;
        }
        private static DropcastingExt CreateObjectExt(DataRow dr)
        {
            var dropcasting = CreateObject(dr);

            var dropcastingExt = new DropcastingExt(dropcasting)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return dropcastingExt;
        }
    }
}