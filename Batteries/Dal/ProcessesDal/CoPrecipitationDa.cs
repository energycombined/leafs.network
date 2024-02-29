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
    public class CoPrecipitationDa
    {
        public static List<CoPrecipitationExt> GetAllCoPrecipitations(long? coPrecipitationId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM co_precipitation c
                        left join experiment_process ep on c.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on c.fk_batch_process = bp.batch_process_id
                        left join equipment eq on c.fk_equipment = eq.equipment_id

                    WHERE (c.co_precipitation_id = :cid or :cid is null) and
                        (c.fk_experiment_process = :epid or :epid is null) and
                        (c.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@cid", coPrecipitationId, NpgsqlDbType.Bigint);
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

            List<CoPrecipitationExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<CoPrecipitationExt> GetRecentlyUsedCoPrecipitations(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(co_precipitation_id) as co_precipitation_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
atmosphere,
pressure,
temperature,
time,
comments,
label
                      FROM co_precipitation
                          LEFT JOIN equipment e on co_precipitation.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name,
atmosphere,
pressure,
temperature,
time,
comments,
label
                      ORDER BY max(co_precipitation_id) DESC LIMIT 10;";

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

            List<CoPrecipitationExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddCoPrecipitation(CoPrecipitation coPrecipitation, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.co_precipitation (
fk_experiment_process, fk_batch_process, fk_equipment, date_created,
atmosphere,
pressure,
temperature,
time,
comments,
label
)
                    VALUES (:epid, :bpid, :eid, now()::timestamp,
:atm,
:pr,
:temp,
:t,
:com,
:lab
);";

                Db.CreateParameterFunc(cmd, "@epid", coPrecipitation.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", coPrecipitation.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", coPrecipitation.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@atm", coPrecipitation.atmosphere, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@pr", coPrecipitation.pressure, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@temp", coPrecipitation.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@t", coPrecipitation.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@com", coPrecipitation.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", coPrecipitation.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting process", ex);
            }

            return 0;
        }
        public static int UpdateCoPrecipitation(CoPrecipitation coPrecipitation)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.co_precipitation
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
atmosphere=:atm,
pressure=:pr,
temperature=:temp,
time=:t,
comments=:com,
label=:lab
                        WHERE co_precipitation_id=:cid;";
                Db.CreateParameterFunc(cmd, "@epid", coPrecipitation.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", coPrecipitation.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", coPrecipitation.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@atm", coPrecipitation.atmosphere, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@pr", coPrecipitation.pressure, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@temp", coPrecipitation.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@t", coPrecipitation.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@com", coPrecipitation.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", coPrecipitation.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@cid", coPrecipitation.coPrecipitationId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating process info", ex);
            }
            return 0;
        }
        public static CoPrecipitation CreateObject(DataRow dr)
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

            var coPrecipitation = new CoPrecipitation
            {
                coPrecipitationId = (long)dr["co_precipitation_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                atmosphere = dr["atmosphere"].ToString(),
                pressure = dr["pressure"] != DBNull.Value ? double.Parse(dr["pressure"].ToString()) : (double?)null,
                temperature = dr["temperature"] != DBNull.Value ? double.Parse(dr["temperature"].ToString()) : (double?)null,
                time = dr["time"] != DBNull.Value ? double.Parse(dr["time"].ToString()) : (double?)null,
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,
            };
            return coPrecipitation;
        }
        private static CoPrecipitationExt CreateObjectExt(DataRow dr)
        {
            var coPrecipitation = CreateObject(dr);

            var coPrecipitationExt = new CoPrecipitationExt(coPrecipitation)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return coPrecipitationExt;
        }
    }
}