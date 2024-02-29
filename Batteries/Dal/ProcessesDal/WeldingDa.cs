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
    public class WeldingDa
    {
        public static List<WeldingExt> GetAllWeldings(long? weldingId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM welding c
                        left join experiment_process ep on c.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on c.fk_batch_process = bp.batch_process_id
                        left join equipment eq on c.fk_equipment = eq.equipment_id

                    WHERE (c.welding_id = :cid or :cid is null) and
                        (c.fk_experiment_process = :epid or :epid is null) and
                        (c.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@cid", weldingId, NpgsqlDbType.Bigint);
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

            List<WeldingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<WeldingExt> GetRecentlyUsedWeldings(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(welding_id) as welding_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
welding_points_number,
time,
comments,
label
                      FROM welding
                          LEFT JOIN equipment e on welding.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name,
welding_points_number,
time,
comments,
label
                      ORDER BY max(welding_id) DESC LIMIT 10;";

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

            List<WeldingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddWelding(Welding welding, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.welding (
fk_experiment_process, fk_batch_process, fk_equipment, date_created,
welding_points_number,
time,
comments,
label
)
                    VALUES (:epid, :bpid, :eid, now()::timestamp,
:welding_points_number,
:time,
:comments,
:label
);";

                Db.CreateParameterFunc(cmd, "@epid", welding.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", welding.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", welding.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@welding_points_number", welding.weldingPointsNumber, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@time", welding.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comments", welding.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", welding.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting process", ex);
            }

            return 0;
        }
        public static int UpdateWelding(Welding welding)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.welding
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
welding_points_number=:welding_points_number,
time=:time,
comments=:comments,
label=:label
                        WHERE welding_id=:cid;";
                Db.CreateParameterFunc(cmd, "@epid", welding.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", welding.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", welding.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@welding_points_number", welding.weldingPointsNumber, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@time", welding.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comments", welding.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", welding.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@cid", welding.weldingId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating process info", ex);
            }
            return 0;
        }
        public static Welding CreateObject(DataRow dr)
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

            var welding = new Welding
            {
                weldingId = (long)dr["welding_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                weldingPointsNumber = dr["welding_points_number"] != DBNull.Value ? int.Parse(dr["welding_points_number"].ToString()) : (int?)null,
                time = dr["time"] != DBNull.Value ? double.Parse(dr["time"].ToString()) : (double?)null,
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,

            };
            return welding;
        }
        private static WeldingExt CreateObjectExt(DataRow dr)
        {
            var welding = CreateObject(dr);

            var weldingExt = new WeldingExt(welding)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return weldingExt;
        }
    }
}