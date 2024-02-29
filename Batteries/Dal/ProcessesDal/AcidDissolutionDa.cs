using Batteries.Dal.Base;
using Batteries.Models.ProcessModels;
using Batteries.Models.Responses.ProcessModels;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Batteries.Dal.ProcessesDal
{
    public class AcidDissolutionDa
    {
        public static List<AcidDissolutionExt> GetAllAcidDissolutions(long? acidDissolutionId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM acid_dissolution c
                        left join experiment_process ep on c.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on c.fk_batch_process = bp.batch_process_id
                        left join equipment eq on c.fk_equipment = eq.equipment_id

                    WHERE (c.acid_dissolution_id = :cid or :cid is null) and
                        (c.fk_experiment_process = :epid or :epid is null) and
                        (c.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@cid", acidDissolutionId, NpgsqlDbType.Bigint);
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

            List<AcidDissolutionExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<AcidDissolutionExt> GetRecentlyUsedAcidDissolutions(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(acid_dissolution_id) as acid_dissolution_id, max(date_created) as date_created, fk_equipment, e.equipment_name, acid_used, time, comments, label
                      FROM acid_dissolution
                          LEFT JOIN equipment e on acid_dissolution.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name, acid_used, time, comments, label
                      ORDER BY max(acid_dissolution_id) DESC LIMIT 10;";

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

            List<AcidDissolutionExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddAcidDissolution(AcidDissolution acidDissolution, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.acid_dissolution (
fk_experiment_process, fk_batch_process, fk_equipment, date_created, acid_used, time, comments, label
)
                    VALUES (:epid, :bpid, :eid, now()::timestamp, :au, :t, :com, :lab);";

                Db.CreateParameterFunc(cmd, "@epid", acidDissolution.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", acidDissolution.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", acidDissolution.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@au", acidDissolution.acidUsed, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@t", acidDissolution.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@com", acidDissolution.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", acidDissolution.label, NpgsqlDbType.Text);              

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting acid dissolution", ex);
            }

            return 0;
        }
        public static int UpdateAcidDissolution(AcidDissolution acidDissolution)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.acid_dissolution
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
acid_used=:au,
time=:t,
comments=:com,
label=:lab

                        WHERE acid_dissolution_id=:cid;";
                Db.CreateParameterFunc(cmd, "@epid", acidDissolution.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", acidDissolution.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", acidDissolution.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@au", acidDissolution.acidUsed, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@t", acidDissolution.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@com", acidDissolution.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", acidDissolution.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@cid", acidDissolution.acidDissolutionId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating acid dissolution info", ex);
            }
            return 0;
        }
        public static AcidDissolution CreateObject(DataRow dr)
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

            var acidDissolution = new AcidDissolution
            {
                acidDissolutionId = (long)dr["acid_dissolution_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                acidUsed = dr["acid_used"].ToString(),
                time = dr["time"] != DBNull.Value ? double.Parse(dr["time"].ToString()) : (double?)null,
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,

            };
            return acidDissolution;
        }
        private static AcidDissolutionExt CreateObjectExt(DataRow dr)
        {
            var acidDissolution = CreateObject(dr);

            var acidDissolutionExt = new AcidDissolutionExt(acidDissolution)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return acidDissolutionExt;
        }
    }
}