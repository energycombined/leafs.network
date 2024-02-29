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
    public class PressingManualDa
    {
        public static List<PressingManualExt> GetAllPressingManuals(long? settingsId = null, long? experimentProcessId = null, long? batchProcessId = null, int? equipmentModelId = null)
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
                    FROM pressing_manual m
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

            List<PressingManualExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<PressingManualExt> GetRecentlyUsedPressingManuals(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null, int? equipmentModelId = null)
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
                    @"SELECT max(settings_id) as settings_id, max(date_created) as date_created, fk_equipment_model, pressing_blocks, substrate_material, pressure, pressing_time, temperature, comment, label
                        FROM pressing_manual
                      GROUP BY fk_equipment_model, pressing_blocks, substrate_material, pressure, pressing_time, temperature, comment, label
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

            List<PressingManualExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddPressingManual(PressingManual pressingManual, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.pressing_manual (
fk_experiment_process,
fk_batch_process,
fk_equipment_model,
pressing_blocks,
substrate_material,
pressure,
pressing_time,
temperature,
comment,
label,
date_created
)
                    VALUES (:epid, :bpid, :emid, :pb, :sm, :pr, :pt, :temp, :comm, :lab, now()::timestamp);";


                Db.CreateParameterFunc(cmd, "@epid", pressingManual.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", pressingManual.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@emid", pressingManual.fkEquipmentModel, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pb", pressingManual.pressingBlocks, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@sm", pressingManual.substrateMaterial, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@pr", pressingManual.pressure, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@pt", pressingManual.pressingTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@temp", pressingManual.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comm", pressingManual.comment, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", pressingManual.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting Pressing Manual settings", ex);
            }

            return 0;
        }
        public static int UpdatePressingManual(PressingManual pressingManual)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.pressing_manual
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment_model=:emid,
pressing_blocks=:pb,
substrate_material=:sm,
pressure=:pr,
pressing_time=:pt,
temperature=:temp,
comment=:comm,
label=:lab
                        WHERE settings_id=:sid;";
                Db.CreateParameterFunc(cmd, "@epid", pressingManual.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", pressingManual.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@emid", pressingManual.fkEquipmentModel, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pb", pressingManual.pressingBlocks, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@sm", pressingManual.substrateMaterial, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@pr", pressingManual.pressure, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@pt", pressingManual.pressingTime, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@temp", pressingManual.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comm", pressingManual.comment, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", pressingManual.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@sid", pressingManual.settingsId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Pressing Manual settings", ex);
            }
            return 0;
        }
        public static int DeletePressingManual(long settingsId)
        {
            try
            {
                var cmd = Db.CreateCommand();

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.pressing_manual
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
        public static PressingManual CreateObject(DataRow dr)
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

            var pressingManual = new PressingManual
            {
                settingsId = (long)dr["settings_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipmentModel = fkEquipmentModelVar,
                pressingBlocks = dr["pressing_blocks"].ToString(),
                substrateMaterial = dr["substrate_material"].ToString(),
                pressure = dr["pressure"] != DBNull.Value ? double.Parse(dr["pressure"].ToString()) : (double?)null,
                pressingTime = dr["pressing_time"] != DBNull.Value ? double.Parse(dr["pressing_time"].ToString()) : (double?)null,
                temperature = dr["temperature"] != DBNull.Value ? double.Parse(dr["temperature"].ToString()) : (double?)null,
                comment = commentVar,
                label = labelVar,
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null
            };
            return pressingManual;
        }
        private static PressingManualExt CreateObjectExt(DataRow dr)
        {
            var pressingManual = CreateObject(dr);

            var pressingManualExt = new PressingManualExt(pressingManual)
            {
                equipmentModelName = dr.Table.Columns.Contains("equipment_model_name") ? dr["equipment_model_name"].ToString() : null
            };
            return pressingManualExt;
        }
    }
}