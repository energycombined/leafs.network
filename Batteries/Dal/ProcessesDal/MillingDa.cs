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
    public class MillingDa
    {
        public static List<MillingExt> GetAllMillings(long? millingId = null, long? experimentProcessId = null, long? batchProcessId = null)
        {
            DataTable dt;

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                // ne mozes join so processtype sega radi neznaenje dali e exp process ili batch process..
                cmd.CommandText =
                    @"SELECT *
                    FROM milling m
                        left join experiment_process ep on m.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on m.fk_batch_process = bp.batch_process_id
                        left join equipment eq on m.fk_equipment = eq.equipment_id

                    WHERE (m.milling_id = :mid or :mid is null) and
                        (m.fk_experiment_process = :epid or :epid is null) and
                        (m.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@mid", millingId, NpgsqlDbType.Bigint);
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

            List<MillingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }

        public static List<MillingExt> GetRecentlyUsedMillings(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
        {
            DataTable dt;

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
//                cmd.CommandText =
//                    @"SELECT max(milling_id) as milling_id, max(date_created) as date_created, ball_mill_cups_size, ball_mill_cups_material, ball_mill_cups_vendor, balls_size, balls_material, balls_amount, milling_speed_rpm, resting_time, loop_count, ball_powder_ratio
//                      FROM milling
//                      GROUP BY ball_mill_cups_size, ball_mill_cups_material, ball_mill_cups_vendor, balls_size, balls_material, balls_amount, milling_speed_rpm, resting_time, loop_count, ball_powder_ratio
//                      ORDER BY max(milling_id) DESC LIMIT 10;";
                cmd.CommandText =
                    @"SELECT max(milling_id) as milling_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
comments,
label
                        FROM milling
                        LEFT JOIN equipment e on milling.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name,
comments,
label
                      ORDER BY max(milling_id) DESC LIMIT 10;";

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

            List<MillingExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }


        public static int AddMilling(Milling milling, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.milling (

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

                Db.CreateParameterFunc(cmd, "@epid", milling.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", milling.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", milling.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@comments", milling.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", milling.label, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@bmcs", milling.ballMillCupsSize, NpgsqlDbType.Double);
                //Db.CreateParameterFunc(cmd, "@bmcm", milling.ballMillCupsMaterial, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@bmcv", milling.ballMillCupsVendor, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@bs", milling.ballsSize, NpgsqlDbType.Double);
                //Db.CreateParameterFunc(cmd, "@bm", milling.ballsMaterial, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@ba", milling.ballsAmount, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@rpm", milling.millingSpeedRpm, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@rt", milling.restingTime, NpgsqlDbType.Double);
                //Db.CreateParameterFunc(cmd, "@lc", milling.loopCount, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@bpr", milling.ballPowderRatio, NpgsqlDbType.Double);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting milling", ex);
            }

            return 0;
        }
        public static int UpdateMilling(Milling milling)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.milling
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
comments=:comments,
label=:label
                        WHERE milling_id=:mid;";
                Db.CreateParameterFunc(cmd, "@epid", milling.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", milling.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", milling.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@comments", milling.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", milling.label, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@grb", milling.ballMillCupsSize, NpgsqlDbType.Double);
                //Db.CreateParameterFunc(cmd, "@bmcs", milling.ballMillCupsMaterial, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@bmcm", milling.ballMillCupsVendor, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@bmcv", milling.ballsSize, NpgsqlDbType.Double);
                //Db.CreateParameterFunc(cmd, "@bs", milling.ballsMaterial, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@bm", milling.ballsAmount, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@ba", milling.millingSpeedRpm, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@rpm", milling.restingTime, NpgsqlDbType.Double);
                //Db.CreateParameterFunc(cmd, "@rt", milling.loopCount, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@lc", milling.ballPowderRatio, NpgsqlDbType.Double);

                Db.CreateParameterFunc(cmd, "@mid", milling.millingId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating milling info", ex);
            }
            return 0;
        }

        //        public static int CheckMillingUse(int millingId)
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
        //                            WHERE bc.fk_milling=:bid;";

        //                Db.CreateParameterFunc(cmd, "@bid", millingId, NpgsqlDbType.Integer);

        //                dt = Db.ExecuteSelectCommand(cmd);

        //                if (dt.Rows.Count > 0)
        //                {
        //                    throw new Exception("This milling is in use by some experiment");
        //                }
        //                if (cmd.Connection.State != ConnectionState.Open)
        //                {
        //                    cmd.Connection.Open();
        //                }
        //                cmd.CommandText =
        //                            @"SELECT b.milling_content_id 
        //                            FROM milling_content b
        //                            WHERE b.fk_step_milling=:sbid;";

        //                Db.CreateParameterFunc(cmd, "@sbid", millingId, NpgsqlDbType.Integer);

        //                dt = Db.ExecuteSelectCommand(cmd);

        //                if (dt.Rows.Count > 0)
        //                {
        //                    throw new Exception("This milling is in use by some other milling");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw new Exception(ex.Message);
        //            }
        //            return 0;
        //        }


        //moze ke treba pri brisenje na batch_process/.. da se izbrisat od tamu site povrzani setinzi od procesot so soodvetno ime
        public static int DeleteMilling(long millingId)
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
                //                            WHERE bc.fk_milling=:bid;";

                //                Db.CreateParameterFunc(cmd, "@bid", millingId, NpgsqlDbType.Integer);

                //                dt = Db.ExecuteSelectCommand(cmd);

                //                if (dt.Rows.Count > 0)
                //                {
                //                    throw new Exception("This milling is in use by some experiment");
                //                }
                //                if (cmd.Connection.State != ConnectionState.Open)
                //                {
                //                    cmd.Connection.Open();
                //                }
                //                cmd.CommandText =
                //                            @"SELECT b.milling_content_id 
                //                            FROM milling_content b
                //                            WHERE b.fk_step_milling=:sbid;";

                //                Db.CreateParameterFunc(cmd, "@sbid", millingId, NpgsqlDbType.Integer);

                //                dt = Db.ExecuteSelectCommand(cmd);

                //                if (dt.Rows.Count > 0)
                //                {
                //                    throw new Exception("This milling is in use by some other milling");
                //                }

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.milling
                                WHERE milling_id=:mid;";

                Db.CreateParameterFunc(cmd, "@mid", millingId, NpgsqlDbType.Bigint);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        public static Milling CreateObject(DataRow dr)
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

            var milling = new Milling
            {
                millingId = (long)dr["milling_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null

                //ballMillCupsSize = dr["ball_mill_cups_size"] != DBNull.Value ? double.Parse(dr["ball_mill_cups_size"].ToString()) : (double?)null,
                //ballMillCupsMaterial = dr["ball_mill_cups_material"].ToString(),
                //ballMillCupsVendor = dr["ball_mill_cups_vendor"].ToString(),
                //ballsSize = dr["balls_size"] != DBNull.Value ? double.Parse(dr["balls_size"].ToString()) : (double?)null,
                //ballsMaterial = dr["balls_material"].ToString(),
                //ballsAmount = dr["balls_amount"] != DBNull.Value ? int.Parse(dr["balls_amount"].ToString()) : (int?)null,
                //millingSpeedRpm = dr["milling_speed_rpm"] != DBNull.Value ? int.Parse(dr["milling_speed_rpm"].ToString()) : (int?)null,
                //restingTime = dr["resting_time"] != DBNull.Value ? double.Parse(dr["resting_time"].ToString()) : (double?)null,
                //loopCount = dr["loop_count"] != DBNull.Value ? int.Parse(dr["loop_count"].ToString()) : (int?)null,
                //ballPowderRatio = dr["ball_powder_ratio"] != DBNull.Value ? double.Parse(dr["ball_powder_ratio"].ToString()) : (double?)null,
                //dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null
            };
            return milling;
        }
        private static MillingExt CreateObjectExt(DataRow dr)
        {
            var milling = CreateObject(dr);

            var millingExt = new MillingExt(milling)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return millingExt;
        }
    }
}