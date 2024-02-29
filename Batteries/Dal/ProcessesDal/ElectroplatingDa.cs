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
    public class ElectroplatingDa
    {
        public static List<ElectroplatingExt> GetAllElectroplatings(long? electroplatingId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM electroplating c
                        left join experiment_process ep on c.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on c.fk_batch_process = bp.batch_process_id
                        left join equipment eq on c.fk_equipment = eq.equipment_id

                    WHERE (c.electroplating_id = :cid or :cid is null) and
                        (c.fk_experiment_process = :epid or :epid is null) and
                        (c.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@cid", electroplatingId, NpgsqlDbType.Bigint);
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

            List<ElectroplatingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<ElectroplatingExt> GetRecentlyUsedElectroplatings(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(electroplating_id) as electroplating_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
current_density,
voltage,
time,
comments,
label
                      FROM electroplating
                          LEFT JOIN equipment e on electroplating.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name,
current_density,
voltage,
time,
comments,
label
                      ORDER BY max(electroplating_id) DESC LIMIT 10;";

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

            List<ElectroplatingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddElectroplating(Electroplating electroplating, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.electroplating (
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

                Db.CreateParameterFunc(cmd, "@epid", electroplating.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", electroplating.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", electroplating.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@current_density", electroplating.currentDensity, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@voltage", electroplating.voltage, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@time", electroplating.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comments", electroplating.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", electroplating.label, NpgsqlDbType.Text);               

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting process", ex);
            }

            return 0;
        }
        public static int UpdateElectroplating(Electroplating electroplating)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.electroplating
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
                        WHERE electroplating_id=:cid;";
                Db.CreateParameterFunc(cmd, "@epid", electroplating.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", electroplating.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", electroplating.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@current_density", electroplating.currentDensity, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@voltage", electroplating.voltage, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@time", electroplating.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comments", electroplating.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", electroplating.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@cid", electroplating.electroplatingId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating process info", ex);
            }
            return 0;
        }
        public static Electroplating CreateObject(DataRow dr)
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

            var electroplating = new Electroplating
            {
                electroplatingId = (long)dr["electroplating_id"],
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
            return electroplating;
        }
        private static ElectroplatingExt CreateObjectExt(DataRow dr)
        {
            var electroplating = CreateObject(dr);

            var electroplatingExt = new ElectroplatingExt(electroplating)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return electroplatingExt;
        }
    }
}