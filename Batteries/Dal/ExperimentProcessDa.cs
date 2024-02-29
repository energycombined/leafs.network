using Batteries.Dal.Base;
using Batteries.Models;
using Batteries.Models.Responses;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Batteries.Dal
{
    public class ExperimentProcessDa
    {
        public static List<ExperimentProcessExt> GetAllExperimentProcesses(int? experimentProcessId = null, int? experimentId = null, int? batteryComponentTypeId = null, int? stepId = null, int? processTypeId = null)
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
                    FROM experiment_process ep
                        LEFT JOIN experiment e ON ep.fk_experiment = e.experiment_id
                        LEFT JOIN process p ON p.process_id=ep.fk_process
                        LEFT JOIN process_type pt ON p.fk_process_type = pt.process_type_id
                        LEFT JOIN equipment eq ON p.fk_equipment=eq.equipment_id
                        LEFT JOIN equipment_model m ON p.fk_equipment_model=m.equipment_model_id
                        LEFT JOIN battery_component_type bct ON ep.fk_battery_component_type = bct.battery_component_type_id
						

                    WHERE (ep.fk_experiment = :eid or :eid is null) and
                        (ep.fk_battery_component_type = :bctid or :bctid is null) and
                        (ep.step = :sid or :sid is null) and
                        (p.fk_process_type = :ptid or :ptid is null) and
                        (ep.experiment_process_id = :epid or :epid is null)
                        ORDER BY fk_battery_component_type, step, process_order_in_step;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bctid", batteryComponentTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@sid", stepId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@ptid", processTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@epid", experimentProcessId, NpgsqlDbType.Integer);

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

            List<ExperimentProcessExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }

        public static int AddExperimentProcess(ExperimentProcess experimentProcess)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"INSERT INTO public.experiment_process (fk_experiment, fk_battery_component_type, step, fk_process_type, process_order_in_step, is_complete) 
                    VALUES (:eid, :bctid, :step, :ptid, :pois, :iscomplete);";

                Db.CreateParameterFunc(cmd, "@eid", experimentProcess.fkExperiment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bctid", experimentProcess.fkBatteryComponentType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@step", experimentProcess.step, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@ptid", experimentProcess.fkProcessType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pois", experimentProcess.processOrderInStep, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@iscomplete", experimentProcess.isComplete, NpgsqlDbType.Boolean);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting experiment process", ex);
            }

            return 0;
        }
        public static int UpdateExperimentProcess(ExperimentProcess experimentProcess)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.experiment_process
                        SET fk_experiment=:eid, fk_battery_component_type=:bctid, step=:step, fk_process_type=:ptid, process_order_in_step=:pois, is_complete =:iscomplete
                        WHERE experiment_process_id=:epid;";
                Db.CreateParameterFunc(cmd, "@eid", experimentProcess.fkExperiment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bctid", experimentProcess.fkBatteryComponentType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@step", experimentProcess.step, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@ptid", experimentProcess.fkProcessType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pois", experimentProcess.processOrderInStep, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@epid", experimentProcess.experimentProcessId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@iscomplete", experimentProcess.isComplete, NpgsqlDbType.Boolean);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating experiment process info", ex);
            }
            return 0;
        }
        public static int DeleteExperimentProcess(int experimentProcessId)
        {
            try
            {
                var cmd = Db.CreateCommand();

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.experiment_process
                                WHERE experiment_process_id=:epid;";

                Db.CreateParameterFunc(cmd, "@epid", experimentProcessId, NpgsqlDbType.Integer);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        public static List<ExperimentProcessExt> GetRecentlyUsedExperimentProcess(int processId)
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
                    @"SELECT * FROM experiment_process ep
	                 LEFT JOIN process p ON p.process_id=ep.fk_process
                     LEFT JOIN equipment e ON p.fk_equipment=e.equipment_id
                     LEFT JOIN process_type pt ON pt.process_type_id=p.fk_process_type
					 LEFT JOIN equipment_model m ON m.equipment_model_id=p.fk_equipment_model
                     WHERE ep.fk_process_type=:pid
                     ORDER BY ep.experiment_process_id DESC LIMIT 10
                    ;";

                Db.CreateParameterFunc(cmd, "@pid", processId, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@fe", fkExperiment, NpgsqlDbType.Integer);

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
            List<ExperimentProcessExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();
            return list;
        }

        public static ExperimentProcess CreateObject(DataRow dr)
        {
            var experimentProcess = new ExperimentProcess
            {
                experimentProcessId = (int)dr["experiment_process_id"],
                fkExperiment = dr["fk_experiment"] != DBNull.Value ? int.Parse(dr["fk_experiment"].ToString()) : (int?)null,
                fkBatteryComponentType = dr["fk_battery_component_type"] != DBNull.Value ? int.Parse(dr["fk_battery_component_type"].ToString()) : (int?)null,
                step = dr["step"] != DBNull.Value ? int.Parse(dr["step"].ToString()) : (int?)null,
                //fkProcessType = dr["fk_process_type"] != DBNull.Value ? int.Parse(dr["fk_process_type"].ToString()) : (int?)null,
                processOrderInStep = dr["process_order_in_step"] != DBNull.Value ? int.Parse(dr["process_order_in_step"].ToString()) : (int?)null,
                isComplete = dr["is_complete"] != DBNull.Value ? Boolean.Parse(dr["is_complete"].ToString()) : false,
                label = dr["label"].ToString(),
                //fkEquipment = dr["fk_equipment"] != DBNull.Value ? int.Parse(dr["fk_equipment"].ToString()) : (int?)null,
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,
                fkProcess= dr["fk_process"] != DBNull.Value ? int.Parse(dr["fk_process"].ToString()) : (int?)null,
            };
            return experimentProcess;
        }
        private static ExperimentProcessExt CreateObjectExt(DataRow dr)
        {
            var experimentProcess = CreateObject(dr);

            string batteryComponentType = null;
            if (dr.Table.Columns.Contains("battery_component_type"))
            {
                batteryComponentType = dr["battery_component_type"].ToString();
            }
            string equipmentName = null;
            if (dr.Table.Columns.Contains("equipment_name"))
            {
                equipmentName = dr["equipment_name"].ToString();
            }
            string subcategory = null;
            if (dr.Table.Columns.Contains("subcategory"))
            {
                subcategory = dr["subcategory"].ToString();
            }
            int? equipmentModelId = null;
            if (dr.Table.Columns.Contains("equipment_model_id"))
            {
                equipmentModelId = dr["equipment_model_id"] != DBNull.Value ? int.Parse(dr["equipment_model_id"].ToString()) : (int?)null;
            }
            string equipmentModelName = null;
            if (dr.Table.Columns.Contains("equipment_model_name"))
            {
                equipmentModelName = dr["equipment_model_name"].ToString();
            }
            string modelBrand = null;
            if (dr.Table.Columns.Contains("model_brand"))
            {
                modelBrand = dr["model_brand"].ToString();
            }
            int? fkEquipment = (int?)null;
            if (dr.Table.Columns.Contains("fk_equipment"))
            {
                fkEquipment = dr["fk_equipment"] != DBNull.Value ? int.Parse(dr["fk_equipment"].ToString()) : (int?)null;
            }
            int? fkProcessTeype = (int?)null;
            if (dr.Table.Columns.Contains("fk_process_type"))
            {
                fkProcessTeype = dr["fk_process_type"] != DBNull.Value ? int.Parse(dr["fk_process_type"].ToString()) : (int?)null;
            }
            var experimentProcessExt = new ExperimentProcessExt(experimentProcess)
            {
                processType = dr["process_type"].ToString(),
                processDatabaseType = dr["process_database_type"].ToString(),
                batteryComponentType = batteryComponentType,
                equipmentName = equipmentName,
                subcategory=subcategory,
                equipmentModelId=equipmentModelId,
                equipmentModelName=equipmentModelName,
                modelBrand=modelBrand,
                fkProcessType=fkProcessTeype,
                fkEquipment=fkEquipment
                
            };
            return experimentProcessExt;
        }
    }
}