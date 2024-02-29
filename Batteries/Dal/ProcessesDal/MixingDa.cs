using Batteries.Dal.Base;
using Batteries.Models;
using Batteries.Models.ProcessModels;
using Batteries.Models.Responses;
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
    public class MixingDa
    {
        public static List<MixingExt> GetAllMixings(long? mixingId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM mixing m
                        left join experiment_process ep on m.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on m.fk_batch_process = bp.batch_process_id
                        left join equipment eq on m.fk_equipment = eq.equipment_id

                    WHERE (m.mixing_id = :mid or :mid is null) and
                        (m.fk_experiment_process = :epid or :epid is null) and
                        (m.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@mid", mixingId, NpgsqlDbType.Bigint);
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

            List<MixingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<MixingExt> GetRecentlyUsedMixings(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(mixing_id) as mixing_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
comments,
label
                      FROM mixing
                      LEFT JOIN equipment e on mixing.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name
                      ORDER BY max(mixing_id) DESC LIMIT 10;";

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

            List<MixingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddMixing(Mixing mixing, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.mixing (fk_experiment_process, fk_batch_process, fk_equipment, date_created,
comments,
label
)
                    VALUES (:epid, :bpid, :eid, now()::timestamp,
:comments,
:label
);";

                Db.CreateParameterFunc(cmd, "@epid", mixing.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", mixing.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", mixing.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@comments", mixing.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", mixing.label, NpgsqlDbType.Text);

                //Db.CreateParameterFunc(cmd, "@rs", mixing.rotationSpeed, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@rtime", mixing.rotationTime, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@ctype", mixing.cupType, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@mat", mixing.cupMaterial, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@csize", mixing.cupSize, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@contype", mixing.containerType, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@conmat", mixing.containerMaterial, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@consize", mixing.containerSize, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@pch", mixing.programChannel, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting mixing", ex);
            }

            return 0;
        }
        public static int UpdateMixing(Mixing mixing)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.mixing
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
comments=:comments,
label=:label

                        WHERE mixing_id=:mid;";
                Db.CreateParameterFunc(cmd, "@epid", mixing.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", mixing.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", mixing.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@comments", mixing.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", mixing.label, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@rs", mixing.rotationSpeed, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@rtime", mixing.rotationTime, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@ctype", mixing.cupType, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@mat", mixing.cupMaterial, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@csize", mixing.cupSize, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@contype", mixing.containerType, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@conmat", mixing.containerMaterial, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@consize", mixing.containerSize, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@pch", mixing.programChannel, NpgsqlDbType.Integer);

                Db.CreateParameterFunc(cmd, "@mid", mixing.mixingId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating mixing info", ex);
            }
            return 0;
        }

        public static Mixing CreateObject(DataRow dr)
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

            var mixing = new Mixing
            {
                mixingId = (long)dr["mixing_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null
                //rotationSpeed = dr["rotation_speed"] != DBNull.Value ? int.Parse(dr["rotation_speed"].ToString()) : (int?)null,
                //rotationTime = dr["rotation_time"] != DBNull.Value ? int.Parse(dr["rotation_time"].ToString()) : (int?)null,
                //cupType = dr["cup_type"].ToString(),
                //cupMaterial = dr["cup_material"].ToString(),
                //cupSize = dr["cup_size"] != DBNull.Value ? int.Parse(dr["cup_size"].ToString()) : (int?)null,
                //containerType = dr["container_type"].ToString(),
                //containerMaterial = dr["container_material"].ToString(),
                //containerSize = dr["container_size"] != DBNull.Value ? int.Parse(dr["container_size"].ToString()) : (int?)null,
                //programChannel = dr["program_channel"] != DBNull.Value ? int.Parse(dr["program_channel"].ToString()) : (int?)null,
                //dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null
            };
            return mixing;
        }
        private static MixingExt CreateObjectExt(DataRow dr)
        {
            var mixing = CreateObject(dr);

            var mixingExt = new MixingExt(mixing)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return mixingExt;
        }
    }
}