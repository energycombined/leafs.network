using Batteries.Dal.Base;
using Batteries.Models;
using Batteries.Models.EquipmentModels;
using Batteries.Models.Responses;
using Batteries.Models.Responses.EquipmentModels;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Batteries.Dal.EquipmentDal
{
    public class CalenderingManualPressDa
    {
        public static List<CalenderingManualPressExt> GetAllCalenderingManualPresses(long? settingsId = null, long? experimentProcessId = null, long? batchProcessId = null, int? equipmentModelId = null)
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
                    FROM calendering_manual_press m
                        left join experiment_process ep on m.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on m.fk_batch_process = bp.batch_process_id
                        left join equipment_model eq on m.fk_equipment_model = eq.equipment_model_id

                    WHERE (m.settings_id = :sid or :sid is null) and
                        (m.fk_experiment_process = :epid or :epid is null) and
                        (m.fk_batch_process = :bpid or :bpid is null) and
                        (m.fk_equipment_model = :emid or :emid is null)
                    ;";

                Db.CreateParameterFunc(cmd, "@sid", settingsId, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@epid", experimentProcessId, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", batchProcessId, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@emid", equipmentModelId, NpgsqlDbType.Integer);

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

            List<CalenderingManualPressExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<CalenderingManualPressExt> GetRecentlyUsedCalenderingManualPresses(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null, int? equipmentModelId = null)
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
                    @"SELECT max(settings_id) as settings_id, max(date_created) as date_created, fk_equipment_model, pressing_foil, pressing_foil_material, thickness, temperature, pressure, speed, comment, label
                        FROM calendering_manual_press
                      GROUP BY fk_equipment_model, pressing_foil, pressing_foil_material, thickness, temperature, pressure, speed, comment, label
                      ORDER BY max(settings_id) DESC LIMIT 10;";

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

            List<CalenderingManualPressExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddCalenderingManualPress(CalenderingManualPress calenderingManualPress, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.calendering_manual_press (
fk_experiment_process,
fk_batch_process,
fk_equipment_model,
pressing_foil,
pressing_foil_material,
thickness,
temperature,
pressure,
speed,
comment,
label,
date_created
)
                    VALUES (:epid, :bpid, :emid, :pf, :pfm, :th, :temp, :pr, :sp, :comm, :lab, now()::timestamp);";


                Db.CreateParameterFunc(cmd, "@epid", calenderingManualPress.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", calenderingManualPress.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@emid", calenderingManualPress.fkEquipmentModel, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pf", calenderingManualPress.pressingFoil, NpgsqlDbType.Boolean);
                Db.CreateParameterFunc(cmd, "@pfm", calenderingManualPress.pressingFoilMaterial, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@th", calenderingManualPress.thickness, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@temp", calenderingManualPress.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@pr", calenderingManualPress.pressure, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@sp", calenderingManualPress.speed, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comm", calenderingManualPress.comment, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", calenderingManualPress.label, NpgsqlDbType.Text);
                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting Manual Press settings", ex);
            }

            return 0;
        }
        public static int UpdateCalenderingManualPress(CalenderingManualPress calenderingManualPress)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.calendering_manual_press
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment_model=:emid,
pressing_foil=:pf,
pressing_foil_material=:pfm,
thickness=:th,
temperature=:temp,
pressure=:pr,
speed=:sp,
comment=:comm,
label=:lab
        WHERE settings_id=:sid;";
                Db.CreateParameterFunc(cmd, "@epid", calenderingManualPress.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", calenderingManualPress.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@emid", calenderingManualPress.fkEquipmentModel, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pf", calenderingManualPress.pressingFoil, NpgsqlDbType.Boolean);
                Db.CreateParameterFunc(cmd, "@pfm", calenderingManualPress.pressingFoilMaterial, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@th", calenderingManualPress.thickness, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@temp", calenderingManualPress.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@pr", calenderingManualPress.pressure, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@sp", calenderingManualPress.speed, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comm", calenderingManualPress.comment, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", calenderingManualPress.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@sid", calenderingManualPress.settingsId, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Manual Press settings", ex);
            }
            return 0;
        }
        public static int DeleteCalenderingManualPress(long settingsId)
        {
            try
            {
                var cmd = Db.CreateCommand();

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.calendering_manual_press
                                WHERE settings_id=:sid;";

                Db.CreateParameterFunc(cmd, "@sid", settingsId, NpgsqlDbType.Bigint);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        public static CalenderingManualPress CreateObject(DataRow dr)
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
            int? fkEquipmentModelVar = (int?)null;
            if (dr.Table.Columns.Contains("fk_equipment_model"))
            {
                fkEquipmentModelVar = dr["fk_equipment_model"] != DBNull.Value ? int.Parse(dr["fk_equipment_model"].ToString()) : (int?)null;
            }
            string commentVar = null;
            if (dr.Table.Columns.Contains("comment"))
            {
                commentVar = dr["comment"].ToString();
            }
            string labelVar = null;
            if (dr.Table.Columns.Contains("label"))
            {
                labelVar = dr["label"].ToString();
            }

            var calenderingManualPress = new CalenderingManualPress
            {
                settingsId = (long)dr["settings_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipmentModel = fkEquipmentModelVar,
                pressingFoil = dr["pressing_foil"] != DBNull.Value ? Boolean.Parse(dr["pressing_foil"].ToString()) : (Boolean?)null,
                pressingFoilMaterial = dr["pressing_foil_material"].ToString(),
                thickness = dr["thickness"] != DBNull.Value ? double.Parse(dr["thickness"].ToString()) : (double?)null,
                temperature = dr["temperature"] != DBNull.Value ? double.Parse(dr["temperature"].ToString()) : (double?)null,
                pressure = dr["pressure"] != DBNull.Value ? double.Parse(dr["pressure"].ToString()) : (double?)null,
                speed = dr["speed"] != DBNull.Value ? double.Parse(dr["speed"].ToString()) : (double?)null,
                comment = commentVar,
                label = labelVar,
                dateCreated= dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null
            };
            return calenderingManualPress;
        }
        private static CalenderingManualPressExt CreateObjectExt(DataRow dr)
        {
            var calenderingManualPress = CreateObject(dr);

            var calenderingManualPressExt = new CalenderingManualPressExt(calenderingManualPress)
            {
                equipmentModelName = dr.Table.Columns.Contains("equipment_model_name") ? dr["equipment_model_name"].ToString() : null
            };
            return calenderingManualPressExt;
        }
    }
}