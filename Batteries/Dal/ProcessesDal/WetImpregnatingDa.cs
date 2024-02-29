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
    public class WetImpregnatingDa
    {
        public static List<WetImpregnatingExt> GetAllWetImpregnatings(long? wetImpregnatingId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM wet_impregnating c
                        left join experiment_process ep on c.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on c.fk_batch_process = bp.batch_process_id
                        left join equipment eq on c.fk_equipment = eq.equipment_id

                    WHERE (c.wet_impregnating_id = :cid or :cid is null) and
                        (c.fk_experiment_process = :epid or :epid is null) and
                        (c.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@cid", wetImpregnatingId, NpgsqlDbType.Bigint);
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

            List<WetImpregnatingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<WetImpregnatingExt> GetRecentlyUsedWetImpregnatings(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(wet_impregnating_id) as wet_impregnating_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
solution_type,
concentration,
volume,
time,
comments,
label
                      FROM wet_impregnating
                          LEFT JOIN equipment e on wet_impregnating.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name,
solution_type,
concentration,
volume,
time,
comments,
label
                      ORDER BY max(wet_impregnating_id) DESC LIMIT 10;";

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

            List<WetImpregnatingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddWetImpregnating(WetImpregnating wetImpregnating, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.wet_impregnating (
fk_experiment_process, fk_batch_process, fk_equipment, date_created,
solution_type,
concentration,
volume,
time,
comments,
label
)
                    VALUES (:epid, :bpid, :eid, now()::timestamp,
:solution_type,
:concentration,
:volume,
:time,
:comments,
:label
);";

                Db.CreateParameterFunc(cmd, "@epid", wetImpregnating.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", wetImpregnating.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", wetImpregnating.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@solution_type", wetImpregnating.solutionType, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@concentration", wetImpregnating.concentration, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@volume", wetImpregnating.volume, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@time", wetImpregnating.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comments", wetImpregnating.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", wetImpregnating.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting process", ex);
            }

            return 0;
        }
        public static int UpdateWetImpregnating(WetImpregnating wetImpregnating)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.wet_impregnating
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
solution_type=:solution_type,
concentration=:concentration,
volume=:volume,
time=:time,
comments=:comments,
label=:label
                        WHERE wet_impregnating_id=:cid;";
                Db.CreateParameterFunc(cmd, "@epid", wetImpregnating.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", wetImpregnating.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", wetImpregnating.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@solution_type", wetImpregnating.solutionType, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@concentration", wetImpregnating.concentration, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@volume", wetImpregnating.volume, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@time", wetImpregnating.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comments", wetImpregnating.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", wetImpregnating.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@cid", wetImpregnating.wetImpregnatingId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating process info", ex);
            }
            return 0;
        }
        public static WetImpregnating CreateObject(DataRow dr)
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

            var wetImpregnating = new WetImpregnating
            {
                wetImpregnatingId = (long)dr["wet_impregnating_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                solutionType = dr["solution_type"].ToString(),
                concentration = dr["concentration"] != DBNull.Value ? double.Parse(dr["concentration"].ToString()) : (double?)null,
                volume = dr["volume"] != DBNull.Value ? double.Parse(dr["volume"].ToString()) : (double?)null,
                time = dr["time"] != DBNull.Value ? double.Parse(dr["time"].ToString()) : (double?)null,
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,

            };
            return wetImpregnating;
        }
        private static WetImpregnatingExt CreateObjectExt(DataRow dr)
        {
            var wetImpregnating = CreateObject(dr);

            var wetImpregnatingExt = new WetImpregnatingExt(wetImpregnating)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return wetImpregnatingExt;
        }
    }
}