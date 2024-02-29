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
    public class CoatingDa
    {
        public static List<CoatingExt> GetAllCoatings(long? coatingId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM coating c
                        left join experiment_process ep on c.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on c.fk_batch_process = bp.batch_process_id
                        left join equipment eq on c.fk_equipment = eq.equipment_id

                    WHERE (c.coating_id = :cid or :cid is null) and
                        (c.fk_experiment_process = :epid or :epid is null) and
                        (c.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@cid", coatingId, NpgsqlDbType.Bigint);
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

            List<CoatingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<CoatingExt> GetRecentlyUsedCoatings(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(coating_id) as coating_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
thickness,
width,
length,
drop_volume,
acceleration,
time,
comments,
label
                      FROM coating
                          LEFT JOIN equipment e on coating.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name,
thickness,
width,
length,
drop_volume,
acceleration,
time,
comments,
label
                      ORDER BY max(coating_id) DESC LIMIT 10;";

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

            List<CoatingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddCoating(Coating coating, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.coating (
fk_experiment_process, fk_batch_process, fk_equipment, date_created,
thickness,
width,
length,
drop_volume,
acceleration,
time,
comments,
label
)
                    VALUES (:epid, :bpid, :eid, now()::timestamp,
:th,
:wi,
:le,
:dv,
:acc,
:t,
:com,
:lab
);";

                Db.CreateParameterFunc(cmd, "@epid", coating.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", coating.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", coating.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@th", coating.thickness, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@wi", coating.width, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@le", coating.length, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@dv", coating.dropVolume, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@acc", coating.acceleration, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@t", coating.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@com", coating.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", coating.label, NpgsqlDbType.Text);                

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting process", ex);
            }

            return 0;
        }
        public static int UpdateCoating(Coating coating)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.coating
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
thickness=:th,
width=:wi,
length=:le,
drop_volume=:dv,
acceleration=:acc,
time=:t,
comments=:com,
label=:lab
                        WHERE coating_id=:cid;";
                Db.CreateParameterFunc(cmd, "@epid", coating.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", coating.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", coating.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@th", coating.thickness, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@wi", coating.width, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@le", coating.length, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@dv", coating.dropVolume, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@acc", coating.acceleration, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@t", coating.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@com", coating.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", coating.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@cid", coating.coatingId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating process info", ex);
            }
            return 0;
        }
        public static Coating CreateObject(DataRow dr)
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

            var coating = new Coating
            {
                coatingId = (long)dr["coating_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,                
                thickness = dr["thickness"] != DBNull.Value ? double.Parse(dr["thickness"].ToString()) : (double?)null,
                width = dr["width"] != DBNull.Value ? double.Parse(dr["width"].ToString()) : (double?)null,
                length = dr["length"] != DBNull.Value ? double.Parse(dr["length"].ToString()) : (double?)null,
                dropVolume = dr["drop_volume"] != DBNull.Value ? double.Parse(dr["drop_volume"].ToString()) : (double?)null,
                acceleration = dr["acceleration"] != DBNull.Value ? double.Parse(dr["acceleration"].ToString()) : (double?)null,
                time = dr["time"] != DBNull.Value ? double.Parse(dr["time"].ToString()) : (double?)null,
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,


            };
            return coating;
        }
        private static CoatingExt CreateObjectExt(DataRow dr)
        {
            var coating = CreateObject(dr);

            var coatingExt = new CoatingExt(coating)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return coatingExt;
        }
    }
}