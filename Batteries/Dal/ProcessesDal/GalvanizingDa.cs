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
    public class GalvanizingDa
    {
        public static List<GalvanizingExt> GetAllGalvanizings(long? galvanizingId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM galvanizing c
                        left join experiment_process ep on c.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on c.fk_batch_process = bp.batch_process_id
                        left join equipment eq on c.fk_equipment = eq.equipment_id

                    WHERE (c.galvanizing_id = :cid or :cid is null) and
                        (c.fk_experiment_process = :epid or :epid is null) and
                        (c.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@cid", galvanizingId, NpgsqlDbType.Bigint);
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

            List<GalvanizingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<GalvanizingExt> GetRecentlyUsedGalvanizings(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(galvanizing_id) as galvanizing_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
current_density,
voltage,
time,
comments,
label
                      FROM galvanizing
                          LEFT JOIN equipment e on galvanizing.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name,
current_density,
voltage,
time,
comments,
label
                      ORDER BY max(galvanizing_id) DESC LIMIT 10;";

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

            List<GalvanizingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddGalvanizing(Galvanizing galvanizing, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.galvanizing (
fk_experiment_process, fk_batch_process, fk_equipment, date_created,
current_density,
voltage,
time,
comments,
label
)
                    VALUES (:epid, :bpid, :eid, now()::timestamp,
:current_density,
:voltage,
:time,
:comments,
:label
);";

                Db.CreateParameterFunc(cmd, "@epid", galvanizing.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", galvanizing.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", galvanizing.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@current_density", galvanizing.currentDensity, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@voltage", galvanizing.voltage, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@time", galvanizing.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comments", galvanizing.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", galvanizing.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting process", ex);
            }

            return 0;
        }
        public static int UpdateGalvanizing(Galvanizing galvanizing)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.galvanizing
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
current_density=:current_density,
voltage=:voltage,
time=:time,
comments=:comments,
label=:label
                        WHERE galvanizing_id=:cid;";
                Db.CreateParameterFunc(cmd, "@epid", galvanizing.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", galvanizing.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", galvanizing.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@current_density", galvanizing.currentDensity, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@voltage", galvanizing.voltage, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@time", galvanizing.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comments", galvanizing.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", galvanizing.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@cid", galvanizing.galvanizingId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating process info", ex);
            }
            return 0;
        }
        public static Galvanizing CreateObject(DataRow dr)
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

            var galvanizing = new Galvanizing
            {
                galvanizingId = (long)dr["galvanizing_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                currentDensity = dr["current_density"] != DBNull.Value ? double.Parse(dr["current_density"].ToString()) : (double?)null,
                voltage = dr["voltage"] != DBNull.Value ? double.Parse(dr["voltage"].ToString()) : (double?)null,
                time = dr["time"] != DBNull.Value ? double.Parse(dr["time"].ToString()) : (double?)null,
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,

            };
            return galvanizing;
        }
        private static GalvanizingExt CreateObjectExt(DataRow dr)
        {
            var galvanizing = CreateObject(dr);

            var galvanizingExt = new GalvanizingExt(galvanizing)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return galvanizingExt;
        }
    }
}