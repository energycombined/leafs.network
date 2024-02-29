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
    public class ScreenprintingDa
    {
        public static List<ScreenprintingExt> GetAllScreenprintings(long? screenprintingId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM screenprinting c
                        left join experiment_process ep on c.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on c.fk_batch_process = bp.batch_process_id
                        left join equipment eq on c.fk_equipment = eq.equipment_id

                    WHERE (c.screenprinting_id = :cid or :cid is null) and
                        (c.fk_experiment_process = :epid or :epid is null) and
                        (c.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@cid", screenprintingId, NpgsqlDbType.Bigint);
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

            List<ScreenprintingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<ScreenprintingExt> GetRecentlyUsedScreenprintings(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(screenprinting_id) as screenprinting_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
screen_mesh_size,
thickness,
time,
comments,
label
                      FROM screenprinting
                          LEFT JOIN equipment e on screenprinting.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name,
screen_mesh_size,
thickness,
time,
comments,
label
                      ORDER BY max(screenprinting_id) DESC LIMIT 10;";

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

            List<ScreenprintingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddScreenprinting(Screenprinting screenprinting, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.screenprinting (
fk_experiment_process, fk_batch_process, fk_equipment, date_created,
screen_mesh_size,
thickness,
time,
comments,
label
)
                    VALUES (:epid, :bpid, :eid, now()::timestamp,
:screen_mesh_size,
:thickness,
:time,
:comments,
:label
);";

                Db.CreateParameterFunc(cmd, "@epid", screenprinting.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", screenprinting.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", screenprinting.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@screen_mesh_size", screenprinting.screenMeshSize, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@thickness", screenprinting.thickness, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@time", screenprinting.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comments", screenprinting.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", screenprinting.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting process", ex);
            }

            return 0;
        }
        public static int UpdateScreenprinting(Screenprinting screenprinting)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.screenprinting
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
screen_mesh_size=:screen_mesh_size,
thickness=:thickness,
time=:time,
comments=:comments,
label=:label
                        WHERE screenprinting_id=:cid;";
                Db.CreateParameterFunc(cmd, "@epid", screenprinting.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", screenprinting.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", screenprinting.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@screen_mesh_size", screenprinting.screenMeshSize, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@thickness", screenprinting.thickness, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@time", screenprinting.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comments", screenprinting.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", screenprinting.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@cid", screenprinting.screenprintingId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating process info", ex);
            }
            return 0;
        }
        public static Screenprinting CreateObject(DataRow dr)
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

            var screenprinting = new Screenprinting
            {
                screenprintingId = (long)dr["screenprinting_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                screenMeshSize = dr["screen_mesh_size"] != DBNull.Value ? double.Parse(dr["screen_mesh_size"].ToString()) : (double?)null,
                thickness = dr["thickness"] != DBNull.Value ? double.Parse(dr["thickness"].ToString()) : (double?)null,
                time = dr["time"] != DBNull.Value ? double.Parse(dr["time"].ToString()) : (double?)null,
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,

            };
            return screenprinting;
        }
        private static ScreenprintingExt CreateObjectExt(DataRow dr)
        {
            var screenprinting = CreateObject(dr);

            var screenprintingExt = new ScreenprintingExt(screenprinting)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return screenprintingExt;
        }
    }
}