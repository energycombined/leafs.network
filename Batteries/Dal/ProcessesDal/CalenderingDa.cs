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
    public class CalenderingDa
    {
        public static List<CalenderingExt> GetAllCalenderings(long? calenderingId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM calendering c
                        left join experiment_process ep on c.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on c.fk_batch_process = bp.batch_process_id
                        left join equipment eq on c.fk_equipment = eq.equipment_id

                    WHERE (c.calendering_id = :cid or :cid is null) and
                        (c.fk_experiment_process = :epid or :epid is null) and
                        (c.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@cid", calenderingId, NpgsqlDbType.Bigint);
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

            List<CalenderingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<CalenderingExt> GetRecentlyUsedCalenderings(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(calendering_id) as calendering_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
comments,
label
                      FROM calendering
                          LEFT JOIN equipment e on calendering.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name,
comments,
label
                      ORDER BY max(calendering_id) DESC LIMIT 10;";

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

            List<CalenderingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddCalendering(Calendering calendering, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.calendering (
fk_experiment_process, fk_batch_process, fk_equipment, date_created,
comments,
label
)
                    VALUES (:epid, :bpid, :eid, now()::timestamp,
:comments,
:label
);";

                Db.CreateParameterFunc(cmd, "@epid", calendering.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", calendering.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", calendering.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@comments", calendering.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", calendering.label, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@s", calendering.substrate, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@m", calendering.material, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@mtype", calendering.materialType, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@t", calendering.thickness, NpgsqlDbType.Double);
                //Db.CreateParameterFunc(cmd, "@temp", calendering.temperature, NpgsqlDbType.Double);
                //Db.CreateParameterFunc(cmd, "@pressure", calendering.pressure, NpgsqlDbType.Double);
                //Db.CreateParameterFunc(cmd, "@speed", calendering.speed, NpgsqlDbType.Double);                

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting calendering", ex);
            }

            return 0;
        }
        public static int UpdateCalendering(Calendering calendering)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.calendering
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
comments=:comments,
label=:label
                        WHERE calendering_id=:cid;";
                Db.CreateParameterFunc(cmd, "@epid", calendering.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", calendering.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", calendering.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@comments", calendering.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", calendering.label, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@s", calendering.substrate, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@m", calendering.material, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@mtype", calendering.materialType, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@t", calendering.thickness, NpgsqlDbType.Double);
                //Db.CreateParameterFunc(cmd, "@temp", calendering.temperature, NpgsqlDbType.Double);
                //Db.CreateParameterFunc(cmd, "@pressure", calendering.pressure, NpgsqlDbType.Double);
                //Db.CreateParameterFunc(cmd, "@speed", calendering.speed, NpgsqlDbType.Double);

                Db.CreateParameterFunc(cmd, "@cid", calendering.calenderingId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating calendering info", ex);
            }
            return 0;
        }
        public static Calendering CreateObject(DataRow dr)
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

            var calendering = new Calendering
            {
                calenderingId = (long)dr["calendering_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null

                //substrate = dr["substrate"].ToString(),
                //material = dr["material"].ToString(),
                //materialType = dr["material_type"].ToString(),
                //thickness = dr["thickness"] != DBNull.Value ? double.Parse(dr["thickness"].ToString()) : (double?)null,
                //temperature = dr["temperature"] != DBNull.Value ? double.Parse(dr["temperature"].ToString()) : (double?)null,
                //pressure = dr["pressure"] != DBNull.Value ? double.Parse(dr["pressure"].ToString()) : (double?)null,
                //speed = dr["speed"] != DBNull.Value ? double.Parse(dr["speed"].ToString()) : (double?)null,

            };
            return calendering;
        }
        private static CalenderingExt CreateObjectExt(DataRow dr)
        {
            var calendering = CreateObject(dr);

            var calenderingExt = new CalenderingExt(calendering)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return calenderingExt;
        }
    }
}