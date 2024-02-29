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
    public class CentrifugingDa
    {
        public static List<CentrifugingExt> GetAllCentrifugings(long? centrifugingId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM centrifuging c
                        left join experiment_process ep on c.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on c.fk_batch_process = bp.batch_process_id
                        left join equipment eq on c.fk_equipment = eq.equipment_id

                    WHERE (c.centrifuging_id = :cid or :cid is null) and
                        (c.fk_experiment_process = :epid or :epid is null) and
                        (c.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@cid", centrifugingId, NpgsqlDbType.Bigint);
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

            List<CentrifugingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<CentrifugingExt> GetRecentlyUsedCentrifugings(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(centrifuging_id) as centrifuging_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
speed,
cup_size,
time,
comments,
label
                      FROM centrifuging
                          LEFT JOIN equipment e on centrifuging.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name,
speed,
cup_size,
time,
comments,
label
                      ORDER BY max(centrifuging_id) DESC LIMIT 10;";

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

            List<CentrifugingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddCentrifuging(Centrifuging centrifuging, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.centrifuging (
fk_experiment_process, fk_batch_process, fk_equipment, date_created,
speed,
cup_size,
time,
comments,
label
)
                    VALUES (:epid, :bpid, :eid, now()::timestamp,
:s,
:cs,
:t,
:com,
:lab
);";

                Db.CreateParameterFunc(cmd, "@epid", centrifuging.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", centrifuging.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", centrifuging.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@s", centrifuging.speed, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@cs", centrifuging.cupSize, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@t", centrifuging.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@com", centrifuging.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", centrifuging.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting process", ex);
            }

            return 0;
        }
        public static int UpdateCentrifuging(Centrifuging centrifuging)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.centrifuging
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
speed=:s,
cup_size=:cs,
time=:t,
comments=:com,
label=:lab
                        WHERE centrifuging_id=:cid;";
                Db.CreateParameterFunc(cmd, "@epid", centrifuging.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", centrifuging.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", centrifuging.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@s", centrifuging.speed, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@cs", centrifuging.cupSize, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@t", centrifuging.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@com", centrifuging.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", centrifuging.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@cid", centrifuging.centrifugingId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating process info", ex);
            }
            return 0;
        }
        public static Centrifuging CreateObject(DataRow dr)
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

            var centrifuging = new Centrifuging
            {
                centrifugingId = (long)dr["centrifuging_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                speed = dr["speed"] != DBNull.Value ? double.Parse(dr["speed"].ToString()) : (double?)null,
                cupSize = dr["cup_size"] != DBNull.Value ? double.Parse(dr["cup_size"].ToString()) : (double?)null,
                time = dr["time"] != DBNull.Value ? double.Parse(dr["time"].ToString()) : (double?)null,
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,

            };
            return centrifuging;
        }
        private static CentrifugingExt CreateObjectExt(DataRow dr)
        {
            var centrifuging = CreateObject(dr);

            var centrifugingExt = new CentrifugingExt(centrifuging)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return centrifugingExt;
        }
    }
}