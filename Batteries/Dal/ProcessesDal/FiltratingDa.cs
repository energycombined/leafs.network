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
    public class FiltratingDa
    {
        public static List<FiltratingExt> GetAllFiltratings(long? filtratingId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM filtrating c
                        left join experiment_process ep on c.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on c.fk_batch_process = bp.batch_process_id
                        left join equipment eq on c.fk_equipment = eq.equipment_id

                    WHERE (c.filtrating_id = :cid or :cid is null) and
                        (c.fk_experiment_process = :epid or :epid is null) and
                        (c.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@cid", filtratingId, NpgsqlDbType.Bigint);
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

            List<FiltratingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<FiltratingExt> GetRecentlyUsedFiltratings(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(filtrating_id) as filtrating_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
filter_water,
comments,
label
                      FROM filtrating
                          LEFT JOIN equipment e on filtrating.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name,
filter_water,
comments,
label
                      ORDER BY max(filtrating_id) DESC LIMIT 10;";

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

            List<FiltratingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddFiltrating(Filtrating filtrating, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.filtrating (
fk_experiment_process, fk_batch_process, fk_equipment, date_created,
filter_water,
comments,
label
)
                    VALUES (:epid, :bpid, :eid, now()::timestamp,
:filter_water,
:comments,
:label);";

                Db.CreateParameterFunc(cmd, "@epid", filtrating.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", filtrating.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", filtrating.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@filter_water", filtrating.filterWater, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@comments", filtrating.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", filtrating.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting process", ex);
            }

            return 0;
        }
        public static int UpdateFiltrating(Filtrating filtrating)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.filtrating
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
filter_water=:filter_water,
comments=:comments,
label=:label
                        WHERE filtrating_id=:cid;";
                Db.CreateParameterFunc(cmd, "@epid", filtrating.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", filtrating.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", filtrating.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@filter_water", filtrating.filterWater, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@comments", filtrating.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", filtrating.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@cid", filtrating.filtratingId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating process info", ex);
            }
            return 0;
        }
        public static Filtrating CreateObject(DataRow dr)
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

            var filtrating = new Filtrating
            {
                filtratingId = (long)dr["filtrating_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                filterWater = dr["filter_water"].ToString(),
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,

            };
            return filtrating;
        }
        private static FiltratingExt CreateObjectExt(DataRow dr)
        {
            var filtrating = CreateObject(dr);

            var filtratingExt = new FiltratingExt(filtrating)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return filtratingExt;
        }
    }
}