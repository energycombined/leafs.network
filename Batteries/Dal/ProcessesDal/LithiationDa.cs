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
    public class LithiationDa
    {
        public static List<LithiationExt> GetAllLithiations(long? lithiationId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM lithiation c
                        left join experiment_process ep on c.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on c.fk_batch_process = bp.batch_process_id
                        left join equipment eq on c.fk_equipment = eq.equipment_id

                    WHERE (c.lithiation_id = :cid or :cid is null) and
                        (c.fk_experiment_process = :epid or :epid is null) and
                        (c.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@cid", lithiationId, NpgsqlDbType.Bigint);
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

            List<LithiationExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<LithiationExt> GetRecentlyUsedLithiations(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(lithiation_id) as lithiation_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
temperature,
time,
comments,
label
                      FROM lithiation
                          LEFT JOIN equipment e on lithiation.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name,
temperature,
time,
comments,
label
                      ORDER BY max(lithiation_id) DESC LIMIT 10;";

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

            List<LithiationExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddLithiation(Lithiation lithiation, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.lithiation (
fk_experiment_process, fk_batch_process, fk_equipment, date_created,
temperature,
time,
comments,
label
)
                    VALUES (:epid, :bpid, :eid, now()::timestamp,
:temperature,
:time,
:comments,
:label
);";

                Db.CreateParameterFunc(cmd, "@epid", lithiation.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", lithiation.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", lithiation.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@temperature", lithiation.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@time", lithiation.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comments", lithiation.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", lithiation.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting process", ex);
            }

            return 0;
        }
        public static int UpdateLithiation(Lithiation lithiation)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.lithiation
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
temperature=:temperature,
time=:time,
comments=:comments,
label=:label
                        WHERE lithiation_id=:cid;";
                Db.CreateParameterFunc(cmd, "@epid", lithiation.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", lithiation.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", lithiation.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@temperature", lithiation.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@time", lithiation.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comments", lithiation.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", lithiation.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@cid", lithiation.lithiationId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating process info", ex);
            }
            return 0;
        }
        public static Lithiation CreateObject(DataRow dr)
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

            var lithiation = new Lithiation
            {
                lithiationId = (long)dr["lithiation_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                temperature = dr["temperature"] != DBNull.Value ? double.Parse(dr["temperature"].ToString()) : (double?)null,
                time = dr["time"] != DBNull.Value ? double.Parse(dr["time"].ToString()) : (double?)null,
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,

            };
            return lithiation;
        }
        private static LithiationExt CreateObjectExt(DataRow dr)
        {
            var lithiation = CreateObject(dr);

            var lithiationExt = new LithiationExt(lithiation)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return lithiationExt;
        }
    }
}