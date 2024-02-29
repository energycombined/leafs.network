﻿using Batteries.Models.ProcessModels;
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
    public class SievingDa
    {
        public static List<SievingExt> GetAllSievings(long? sievingId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM sieving c
                        left join experiment_process ep on c.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on c.fk_batch_process = bp.batch_process_id
                        left join equipment eq on c.fk_equipment = eq.equipment_id

                    WHERE (c.sieving_id = :cid or :cid is null) and
                        (c.fk_experiment_process = :epid or :epid is null) and
                        (c.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@cid", sievingId, NpgsqlDbType.Bigint);
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

            List<SievingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<SievingExt> GetRecentlyUsedSievings(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(sieving_id) as sieving_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
sieve_width,
sieve_material,
time,
comments,
label
                      FROM sieving
                          LEFT JOIN equipment e on sieving.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name,
sieve_width,
sieve_material,
time,
comments,
label
                      ORDER BY max(sieving_id) DESC LIMIT 10;";

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

            List<SievingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddSieving(Sieving sieving, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.sieving (
fk_experiment_process, fk_batch_process, fk_equipment, date_created,
sieve_width,
sieve_material,
time,
comments,
label
)
                    VALUES (:epid, :bpid, :eid, now()::timestamp,
:sieve_width,
:sieve_material,
:time,
:comments,
:label
);";

                Db.CreateParameterFunc(cmd, "@epid", sieving.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", sieving.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", sieving.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@sieve_width", sieving.sieveWidth, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@sieve_material", sieving.sieveMaterial, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@time", sieving.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comments", sieving.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", sieving.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting process", ex);
            }

            return 0;
        }
        public static int UpdateSieving(Sieving sieving)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.sieving
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
sieve_width=:sieve_width,
sieve_material=:sieve_material,
time=:time,
comments=:comments,
label=:label
                        WHERE sieving_id=:cid;";
                Db.CreateParameterFunc(cmd, "@epid", sieving.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", sieving.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", sieving.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@sieve_width", sieving.sieveWidth, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@sieve_material", sieving.sieveMaterial, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@time", sieving.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comments", sieving.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", sieving.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@cid", sieving.sievingId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating process info", ex);
            }
            return 0;
        }
        public static Sieving CreateObject(DataRow dr)
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

            var sieving = new Sieving
            {
                sievingId = (long)dr["sieving_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                sieveWidth = dr["sieve_width"] != DBNull.Value ? double.Parse(dr["sieve_width"].ToString()) : (double?)null,
                sieveMaterial = dr["sieve_material"].ToString(),
                time = dr["time"] != DBNull.Value ? double.Parse(dr["time"].ToString()) : (double?)null,
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,

            };
            return sieving;
        }
        private static SievingExt CreateObjectExt(DataRow dr)
        {
            var sieving = CreateObject(dr);

            var sievingExt = new SievingExt(sieving)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return sievingExt;
        }
    }
}