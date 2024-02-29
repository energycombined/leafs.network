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
    public class SlittingDa
    {
        public static List<SlittingExt> GetAllSlittings(long? slittingId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM slitting c
                        left join experiment_process ep on c.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on c.fk_batch_process = bp.batch_process_id
                        left join equipment eq on c.fk_equipment = eq.equipment_id

                    WHERE (c.slitting_id = :cid or :cid is null) and
                        (c.fk_experiment_process = :epid or :epid is null) and
                        (c.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@cid", slittingId, NpgsqlDbType.Bigint);
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

            List<SlittingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<SlittingExt> GetRecentlyUsedSlittings(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(slitting_id) as slitting_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
width,
length,
comments,
label
                      FROM slitting
                          LEFT JOIN equipment e on slitting.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name,
width,
length,
comments,
label
                      ORDER BY max(slitting_id) DESC LIMIT 10;";

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

            List<SlittingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddSlitting(Slitting slitting, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.slitting (
fk_experiment_process, fk_batch_process, fk_equipment, date_created,
width,
length,
comments,
label
)
                    VALUES (:epid, :bpid, :eid, now()::timestamp,
:width,
:length,
:comments,
:label
);";

                Db.CreateParameterFunc(cmd, "@epid", slitting.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", slitting.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", slitting.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@width", slitting.width, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@length", slitting.length, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comments", slitting.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", slitting.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting process", ex);
            }

            return 0;
        }
        public static int UpdateSlitting(Slitting slitting)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.slitting
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
width=:width,
length=:length,
comments=:comments,
label=:label
                        WHERE slitting_id=:cid;";
                Db.CreateParameterFunc(cmd, "@epid", slitting.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", slitting.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", slitting.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@width", slitting.width, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@length", slitting.length, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comments", slitting.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", slitting.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@cid", slitting.slittingId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating process info", ex);
            }
            return 0;
        }
        public static Slitting CreateObject(DataRow dr)
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

            var slitting = new Slitting
            {
                slittingId = (long)dr["slitting_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                width = dr["width"] != DBNull.Value ? double.Parse(dr["width"].ToString()) : (double?)null,
                length = dr["length"] != DBNull.Value ? double.Parse(dr["length"].ToString()) : (double?)null,
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,
            };
            return slitting;
        }
        private static SlittingExt CreateObjectExt(DataRow dr)
        {
            var slitting = CreateObject(dr);

            var slittingExt = new SlittingExt(slitting)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return slittingExt;
        }
    }
}