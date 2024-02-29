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
    public class HeatingDa
    {
        public static List<HeatingExt> GetAllHeatings(long? heatingId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM heating m
                        left join experiment_process ep on m.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on m.fk_batch_process = bp.batch_process_id
                        left join equipment eq on m.fk_equipment = eq.equipment_id

                    WHERE (m.heating_id = :hid or :hid is null) and
                        (m.fk_experiment_process = :epid or :epid is null) and
                        (m.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@hid", heatingId, NpgsqlDbType.Bigint);
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

            List<HeatingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<HeatingExt> GetRecentlyUsedHeatings(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(heating_id) as heating_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
comments,
label
                      FROM heating
                        LEFT JOIN equipment e on heating.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name,
comments,
label
                      ORDER BY max(heating_id) DESC LIMIT 10;";

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

            List<HeatingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddHeating(Heating heating, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.heating (
fk_experiment_process,
fk_batch_process,
fk_equipment,
date_created,
comments,
label
)
                    VALUES (:epid, :bpid, :eid, now()::timestamp,
:comments,
:label
);";

                Db.CreateParameterFunc(cmd, "@epid", heating.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", heating.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", heating.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@comments", heating.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", heating.label, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@tm", heating.tubeMaterial, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@ts", heating.tubeSize, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@noo", heating.numberOfOpenings, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@tv", heating.tubeVendor, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@et", heating.environmentType, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@fr", heating.flowRate, NpgsqlDbType.Double);
                //Db.CreateParameterFunc(cmd, "@pressure", heating.pressure, NpgsqlDbType.Double);
                //Db.CreateParameterFunc(cmd, "@temp", heating.temperature, NpgsqlDbType.Double);
                //Db.CreateParameterFunc(cmd, "@dur", heating.duration, NpgsqlDbType.Double);
                //Db.CreateParameterFunc(cmd, "@dt", heating.dwellTime, NpgsqlDbType.Double);
                //Db.CreateParameterFunc(cmd, "@lc", heating.loopCount, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@ht", heating.heatingTime, NpgsqlDbType.Double);
                //Db.CreateParameterFunc(cmd, "@mss", heating.magneticStirringSpeed, NpgsqlDbType.Double);
                //Db.CreateParameterFunc(cmd, "@sbs", heating.stirBarSize, NpgsqlDbType.Double);
                                                                
                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting heating", ex);
            }

            return 0;
        }
        public static int UpdateHeating(Heating heating)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.heating
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
comments=:comments,
label=:label
                        WHERE heating_id=:hid;";
                Db.CreateParameterFunc(cmd, "@epid", heating.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", heating.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", heating.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@comments", heating.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", heating.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@hid", heating.heatingId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating heating info", ex);
            }
            return 0;
        }

        //        public static int CheckHeatingUse(int heatingId)
        //        {
        //            DataTable dt;
        //            try
        //            {
        //                var cmd = Db.CreateCommand();
        //                if (cmd.Connection.State != ConnectionState.Open)
        //                {
        //                    cmd.Connection.Open();
        //                }
        //                cmd.CommandText =
        //                            @"SELECT bc.battery_component_id 
        //                            FROM battery_component bc
        //                            WHERE bc.fk_heating=:bid;";

        //                Db.CreateParameterFunc(cmd, "@bid", heatingId, NpgsqlDbType.Integer);

        //                dt = Db.ExecuteSelectCommand(cmd);

        //                if (dt.Rows.Count > 0)
        //                {
        //                    throw new Exception("This heating is in use by some experiment");
        //                }
        //                if (cmd.Connection.State != ConnectionState.Open)
        //                {
        //                    cmd.Connection.Open();
        //                }
        //                cmd.CommandText =
        //                            @"SELECT b.heating_content_id 
        //                            FROM heating_content b
        //                            WHERE b.fk_step_heating=:sbid;";

        //                Db.CreateParameterFunc(cmd, "@sbid", heatingId, NpgsqlDbType.Integer);

        //                dt = Db.ExecuteSelectCommand(cmd);

        //                if (dt.Rows.Count > 0)
        //                {
        //                    throw new Exception("This heating is in use by some other heating");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw new Exception(ex.Message);
        //            }
        //            return 0;
        //        }


        //moze ke treba pri brisenje na batch_process/.. da se izbrisat od tamu site povrzani setinzi od procesot so soodvetno ime
        public static int DeleteHeating(long heatingId)
        {
            //DataTable dt;
            try
            {
                var cmd = Db.CreateCommand();
                //                if (cmd.Connection.State != ConnectionState.Open)
                //                {
                //                    cmd.Connection.Open();
                //                }
                //                cmd.CommandText =
                //                            @"SELECT bc.battery_component_id 
                //                            FROM battery_component bc
                //                            WHERE bc.fk_heating=:bid;";

                //                Db.CreateParameterFunc(cmd, "@bid", heatingId, NpgsqlDbType.Integer);

                //                dt = Db.ExecuteSelectCommand(cmd);

                //                if (dt.Rows.Count > 0)
                //                {
                //                    throw new Exception("This heating is in use by some experiment");
                //                }
                //                if (cmd.Connection.State != ConnectionState.Open)
                //                {
                //                    cmd.Connection.Open();
                //                }
                //                cmd.CommandText =
                //                            @"SELECT b.heating_content_id 
                //                            FROM heating_content b
                //                            WHERE b.fk_step_heating=:sbid;";

                //                Db.CreateParameterFunc(cmd, "@sbid", heatingId, NpgsqlDbType.Integer);

                //                dt = Db.ExecuteSelectCommand(cmd);

                //                if (dt.Rows.Count > 0)
                //                {
                //                    throw new Exception("This heating is in use by some other heating");
                //                }

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.heating
                                WHERE heating_id=:mid;";

                Db.CreateParameterFunc(cmd, "@mid", heatingId, NpgsqlDbType.Bigint);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        public static Heating CreateObject(DataRow dr)
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

            var heating = new Heating
            {
                heatingId = (long)dr["heating_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null
            };
            return heating;
        }
        private static HeatingExt CreateObjectExt(DataRow dr)
        {
            var heating = CreateObject(dr);

            var heatingExt = new HeatingExt(heating)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return heatingExt;
        }
    }
}