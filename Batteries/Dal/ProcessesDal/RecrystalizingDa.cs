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
    public class RecrystalizingDa
    {
        public static List<RecrystalizingExt> GetAllRecrystalizings(long? recrystalizingId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM recrystalizing c
                        left join experiment_process ep on c.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on c.fk_batch_process = bp.batch_process_id
                        left join equipment eq on c.fk_equipment = eq.equipment_id

                    WHERE (c.recrystalizing_id = :cid or :cid is null) and
                        (c.fk_experiment_process = :epid or :epid is null) and
                        (c.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@cid", recrystalizingId, NpgsqlDbType.Bigint);
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

            List<RecrystalizingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<RecrystalizingExt> GetRecentlyUsedRecrystalizings(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(recrystalizing_id) as recrystalizing_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
solvent,
temperature,
time,
comments,
label
                      FROM recrystalizing
                          LEFT JOIN equipment e on recrystalizing.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name,
solvent,
temperature,
time,
comments,
label
                      ORDER BY max(recrystalizing_id) DESC LIMIT 10;";

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

            List<RecrystalizingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddRecrystalizing(Recrystalizing recrystalizing, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.recrystalizing (
fk_experiment_process, fk_batch_process, fk_equipment, date_created,
solvent,
temperature,
time,
comments,
label
)
                    VALUES (:epid, :bpid, :eid, now()::timestamp,
:solvent,
:temperature,
:time,
:comments,
:label
);";

                Db.CreateParameterFunc(cmd, "@epid", recrystalizing.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", recrystalizing.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", recrystalizing.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@solvent", recrystalizing.solvent, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@temperature", recrystalizing.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@time", recrystalizing.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comments", recrystalizing.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", recrystalizing.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting process", ex);
            }

            return 0;
        }
        public static int UpdateRecrystalizing(Recrystalizing recrystalizing)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.recrystalizing
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
solvent=:solvent,
temperature=:temperature,
time=:time,
comments=:comments,
label=:label
                        WHERE recrystalizing_id=:cid;";
                Db.CreateParameterFunc(cmd, "@epid", recrystalizing.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", recrystalizing.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", recrystalizing.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@solvent", recrystalizing.solvent, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@temperature", recrystalizing.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@time", recrystalizing.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comments", recrystalizing.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", recrystalizing.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@cid", recrystalizing.recrystalizingId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating process info", ex);
            }
            return 0;
        }
        public static Recrystalizing CreateObject(DataRow dr)
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

            var recrystalizing = new Recrystalizing
            {
                recrystalizingId = (long)dr["recrystalizing_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                solvent = dr["solvent"].ToString(),
                temperature = dr["temperature"] != DBNull.Value ? double.Parse(dr["temperature"].ToString()) : (double?)null,
                time = dr["time"] != DBNull.Value ? double.Parse(dr["time"].ToString()) : (double?)null,
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,

            };
            return recrystalizing;
        }
        private static RecrystalizingExt CreateObjectExt(DataRow dr)
        {
            var recrystalizing = CreateObject(dr);

            var recrystalizingExt = new RecrystalizingExt(recrystalizing)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return recrystalizingExt;
        }
    }
}