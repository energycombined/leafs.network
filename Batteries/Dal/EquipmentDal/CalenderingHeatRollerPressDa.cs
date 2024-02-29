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
    public class CalenderingHeatRollerPressDa
    {
        public static List<CalenderingHeatRollerPressExt> GetAllCalenderingHeatRollerPresses(long? settingsId = null, long? experimentProcessId = null, long? batchProcessId = null, int? equipmentModelId = null)
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
                    FROM calendering_heat_roller_press m
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

            List<CalenderingHeatRollerPressExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<CalenderingHeatRollerPressExt> GetRecentlyUsedCalenderingHeatRollerPresses(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null, int? equipmentModelId = null)
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
                        FROM calendering_heat_roller_press
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

            List<CalenderingHeatRollerPressExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddCalenderingHeatRollerPress(CalenderingHeatRollerPress calenderingHeatRollerPress, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.calendering_heat_roller_press (
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


                Db.CreateParameterFunc(cmd, "@epid", calenderingHeatRollerPress.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", calenderingHeatRollerPress.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@emid", calenderingHeatRollerPress.fkEquipmentModel, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pf", calenderingHeatRollerPress.pressingFoil, NpgsqlDbType.Boolean);
                Db.CreateParameterFunc(cmd, "@pfm", calenderingHeatRollerPress.pressingFoilMaterial, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@th", calenderingHeatRollerPress.thickness, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@temp", calenderingHeatRollerPress.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@pr", calenderingHeatRollerPress.pressure, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@sp", calenderingHeatRollerPress.speed, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comm", calenderingHeatRollerPress.comment, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", calenderingHeatRollerPress.label, NpgsqlDbType.Text);
                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting Heat Roller Press settings", ex);
            }

            return 0;
        }
        public static int UpdateCalenderingHeatRollerPress(CalenderingHeatRollerPress calenderingHeatRollerPress)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.calendering_heat_roller_press
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
                Db.CreateParameterFunc(cmd, "@epid", calenderingHeatRollerPress.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", calenderingHeatRollerPress.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@emid", calenderingHeatRollerPress.fkEquipmentModel, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pf", calenderingHeatRollerPress.pressingFoil, NpgsqlDbType.Boolean);
                Db.CreateParameterFunc(cmd, "@pfm", calenderingHeatRollerPress.pressingFoilMaterial, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@th", calenderingHeatRollerPress.thickness, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@temp", calenderingHeatRollerPress.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@pr", calenderingHeatRollerPress.pressure, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@sp", calenderingHeatRollerPress.speed, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comm", calenderingHeatRollerPress.comment, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", calenderingHeatRollerPress.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@sid", calenderingHeatRollerPress.settingsId, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Heat Roller Press settings", ex);
            }
            return 0;
        }
        public static int DeleteCalenderingHeatRollerPress(long settingsId)
        {
            try
            {
                var cmd = Db.CreateCommand();

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.calendering_heat_roller_press
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
        public static CalenderingHeatRollerPress CreateObject(DataRow dr)
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

            var calenderingHeatRollerPress = new CalenderingHeatRollerPress
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
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null
            };
            return calenderingHeatRollerPress;
        }
        private static CalenderingHeatRollerPressExt CreateObjectExt(DataRow dr)
        {
            var calenderingHeatRollerPress = CreateObject(dr);

            var calenderingHeatRollerPressExt = new CalenderingHeatRollerPressExt(calenderingHeatRollerPress)
            {
                equipmentModelName = dr.Table.Columns.Contains("equipment_model_name") ? dr["equipment_model_name"].ToString() : null
            };
            return calenderingHeatRollerPressExt;
        }
    }
}