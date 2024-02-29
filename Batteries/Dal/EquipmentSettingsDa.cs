using Batteries.Dal.Base;
using Batteries.Models;
using Batteries.Models.Responses;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
namespace Batteries.Dal
{
    public class EquipmentSettingsDa
    {
        public static List<EquipmentSettingsValue> GetAllEquipmentSettingsValue(long? fkExperiment = null, long? experimentProcessId = null, long? batchProcessId = null, int? sequenceContentId = null)
        {
            DataTable dt;

            try
            {
                var cmd = Db.CreateCommand();
                if (experimentProcessId != null && fkExperiment != null)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    // ne mozes join so processtype sega radi neznaenje dali e exp process ili batch process..
                    cmd.CommandText =
                        @" SELECT * FROM equipment_attribute_values eav
	            LEFT JOIN equipment_attribute_type eat ON eav.fk_attribute=eat.equipment_attribute_type_id
				LEFT JOIN equipment_attributes ea ON eav.fk_attribute=ea.equipment_attribute_id
                LEFT JOIN experiment_process ep ON ep.experiment_process_id=eav.fk_experiment_process
				LEFT JOIN process p ON p.process_id=ep.fk_process
                LEFT JOIN equipment e ON e.equipment_id=p.fk_equipment
                LEFT JOIN process_type pt ON pt.process_type_id=p.fk_process_type
                LEFT JOIN attribute_db_type db ON db.attribute_db_type_id=ea.fk_db_type
                LEFT JOIN equipment_model m ON p.fk_equipment_model=m.equipment_model_id
                LEFT JOIN attribute_measurement_unit amu ON amu.attribute_measurement_unit_id=ea.fk_attribute_measurement_unit   
                WHERE (ep.fk_experiment = :eid or :eid is null) and
				(eav.fk_experiment_process = :pid or :pid is null) and
				(eav.fk_batch_process = :bpid or :bpid is null)
                ORDER BY eav.equipment_attribute_value_id;";

                    Db.CreateParameterFunc(cmd, "@eid", fkExperiment, NpgsqlDbType.Bigint);
                    Db.CreateParameterFunc(cmd, "@pid", experimentProcessId, NpgsqlDbType.Bigint);
                    Db.CreateParameterFunc(cmd, "@bpid", batchProcessId, NpgsqlDbType.Bigint);

                    dt = Db.ExecuteSelectCommand(cmd);
                }
                else if (batchProcessId != null)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.CommandText =
                        @" SELECT * FROM equipment_attribute_values eav
                LEFT JOIN equipment_attribute_type eat ON eav.fk_attribute=eat.equipment_attribute_type_id
				LEFT JOIN equipment_attributes ea ON eav.fk_attribute=ea.equipment_attribute_id
                LEFT JOIN batch_process bp ON bp.batch_process_id=eav.fk_batch_process
				LEFT JOIN process p ON p.process_id=bp.fk_process
                LEFT JOIN equipment e ON e.equipment_id=p.fk_equipment
                LEFT JOIN process_type pt ON pt.process_type_id=p.fk_process_type
                LEFT JOIN attribute_db_type db ON db.attribute_db_type_id=ea.fk_db_type
                LEFT JOIN equipment_model m ON p.fk_equipment_model=m.equipment_model_id
                LEFT JOIN attribute_measurement_unit amu ON amu.attribute_measurement_unit_id=ea.fk_attribute_measurement_unit
                WHERE (eav.fk_batch_process = :bpid or :bpid is null)
                ORDER BY eav.equipment_attribute_value_id;";

                    Db.CreateParameterFunc(cmd, "@bpid", batchProcessId, NpgsqlDbType.Bigint);

                    dt = Db.ExecuteSelectCommand(cmd);
                }
                else
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.CommandText =
                        @"SELECT * FROM equipment_attribute_values eav
                        LEFT JOIN equipment_attribute_type eat ON eav.fk_attribute=eat.equipment_attribute_type_id
                        LEFT JOIN equipment_attributes ea ON eav.fk_attribute=ea.equipment_attribute_id
                        LEFT JOIN process_sequence_content psc ON psc.process_sequence_content_id=eav.fk_sequence_content
                        LEFT JOIN process p ON psc.fk_process=p.process_id
                        LEFT JOIN equipment e ON p.fk_equipment=e.equipment_id
                        LEFT JOIN process_type pt ON pt.process_type_id=p.fk_process_type
                        LEFT JOIN attribute_db_type db ON db.attribute_db_type_id=ea.fk_db_type
                        LEFT JOIN equipment_model m ON p.fk_equipment_model=m.equipment_model_id
                        LEFT JOIN attribute_measurement_unit amu ON amu.attribute_measurement_unit_id=ea.fk_attribute_measurement_unit
                        WHERE eav.fk_sequence_content=:scid
                        ORDER BY eav.equipment_attribute_value_id;";

                    Db.CreateParameterFunc(cmd, "@scid", sequenceContentId, NpgsqlDbType.Bigint);

                    dt = Db.ExecuteSelectCommand(cmd);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            List<EquipmentSettingsValue> equipmentSettingsValues = CreateAttributeList(dt);
            /* if (experimentProcessId != null)
             {
                 List<EquipmentSettings> distinctExperimentSettingsValues =
              list
              .GroupBy(pId => pId.fkExperimentProcess)
              .Select(first => first.First())
              .Where(s => s.fkExperimentProcess != null)
              .ToList(); foreach (EquipmentSettings a in distinctExperimentSettingsValues)
                 {
                     if (a.fkExperimentProcess != null)
                     {
                         var query = (from st in list
                                      where st.fkExperimentProcess == a.fkExperimentProcess
                                      select new EquipmentSettingsValue
                                      {
                                          attributeName = st.attributeName,
                                          value = st.value,
                                          type = st.type,
                                          equipmentAttributeTypeId = st.equipmentAttributeTypeId

                                      }).ToList();

                         a.equipmentSettingsValues.AddRange(query);
                     }
                 }
             }
             else if(batchProcessId !=null)
             {
                 List<EquipmentSettings> distinctExperimentSettingsValues =
             list
             .GroupBy(pId => pId.fkBatchProcess)
             .Select(first => first.First())
             .Where(s => s.fkBatchProcess != null)
             .ToList(); foreach (EquipmentSettings a in distinctExperimentSettingsValues)
                 {
                     if (a.fkBatchProcess != null)
                     {
                         var query = (from st in list
                                      where st.fkBatchProcess == a.fkBatchProcess
                                      select new EquipmentSettingsValue
                                      {
                                          attributeName = st.attributeName,
                                          value = st.value,
                                          type = st.type,
                                          equipmentAttributeTypeId = st.equipmentAttributeTypeId

                                      }).ToList();

                         a.equipmentSettingsValues.AddRange(query);
                     }
                 }
             }
             else
             {
                 List<EquipmentSettings> distinctExperimentSettingsValues =
            list
            .GroupBy(pId => pId.fkSequenceContent)
            .Select(first => first.First())
            .Where(s => s.fkSequenceContent != null)
            .ToList(); foreach (EquipmentSettings a in distinctExperimentSettingsValues)
                 {
                     if (a.fkSequenceContent != null)
                     {
                         var query = (from st in list
                                      where st.fkSequenceContent == a.fkSequenceContent
                                      select new EquipmentSettingsValue
                                      {
                                          attributeName = st.attributeName,
                                          value = st.value,
                                          type = st.type,
                                          equipmentAttributeTypeId = st.equipmentAttributeTypeId

                                      }).ToList();

                         a.equipmentSettingsValues.AddRange(query);
                     }
                 }
             }*/

            return equipmentSettingsValues;
        }

        public static EquipmentSettings GetEquipmentSettingsByProcess(int experimentOrBatchProcessId, bool comingFromBatch)
        {
            DataTable dt;

            try
            {
                var cmd = Db.CreateCommand();
                if (comingFromBatch == false)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.CommandText =
                        @"SELECT * FROM equipment_attribute_values eav			
					        LEFT JOIN equipment_attribute_type eat ON eav.fk_attribute=eat.equipment_attribute_type_id
					        LEFT JOIN equipment_attributes ea ON eav.fk_attribute=ea.equipment_attribute_id
					        LEFT JOIN experiment_process ep ON ep.experiment_process_id=eav.fk_experiment_process
			                LEFT JOIN process p ON p.process_id=ep.fk_process		 		
                            LEFT JOIN equipment e ON p.fk_equipment=e.equipment_id
                            LEFT JOIN process_type pt ON pt.process_type_id=p.fk_process_type
                            LEFT JOIN attribute_db_type db ON db.attribute_db_type_id=ea.fk_db_type
				            LEFT JOIN equipment_model m ON p.fk_equipment_model=m.equipment_model_id
                            LEFT JOIN attribute_measurement_unit amu ON amu.attribute_measurement_unit_id=ea.fk_attribute_measurement_unit
                        WHERE eav.fk_experiment_process=:pid
                        ORDER BY eav.equipment_attribute_value_id, eat.order ASC
                    ;";

                    Db.CreateParameterFunc(cmd, "@pid", experimentOrBatchProcessId, NpgsqlDbType.Integer);

                    dt = Db.ExecuteSelectCommand(cmd);
                }
                else
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.CommandText =
                        @"SELECT * FROM equipment_attribute_values eav			
					        LEFT JOIN equipment_attribute_type eat ON eav.fk_attribute=eat.equipment_attribute_type_id
					        LEFT JOIN equipment_attributes ea ON eav.fk_attribute=ea.equipment_attribute_id
					        LEFT JOIN batch_process bp ON bp.batch_process_id=eav.fk_batch_process
			                LEFT JOIN process p ON p.process_id=bp.fk_process		 		
                            LEFT JOIN equipment e ON p.fk_equipment=e.equipment_id
                            LEFT JOIN process_type pt ON pt.process_type_id=p.fk_process_type
                            LEFT JOIN attribute_db_type db ON db.attribute_db_type_id=ea.fk_db_type
				            LEFT JOIN equipment_model m ON p.fk_equipment_model=m.equipment_model_id
                            LEFT JOIN attribute_measurement_unit amu ON amu.attribute_measurement_unit_id=ea.fk_attribute_measurement_unit
                        WHERE eav.fk_batch_process=:bid
                        ORDER BY eav.equipment_attribute_value_id, eat.order ASC
                    ;";

                    Db.CreateParameterFunc(cmd, "@bid", experimentOrBatchProcessId, NpgsqlDbType.Integer);

                    dt = Db.ExecuteSelectCommand(cmd);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                throw new Exception("Data does not exsist");
            }

            DataRow dr2 = dt.Rows[0];
            int? fkExperimentProcess = (int?)null;
            if (dr2.Table.Columns.Contains("fk_experiment_process"))
            {
                fkExperimentProcess = dr2["fk_experiment_process"] != DBNull.Value ? int.Parse(dr2["fk_experiment_process"].ToString()) : (int?)null;
            }
            int fkProcessType = int.Parse(dr2["fk_process_type"].ToString());
            string label = null;
            if (dr2.Table.Columns.Contains("label"))
            {
                label = dr2["label"].ToString();
            }
            int? equipmentId = dr2["equipment_id"] != DBNull.Value ? int.Parse(dr2["equipment_id"].ToString()) : (int?)null;
            string equipmentName = dr2["equipment_name"].ToString();
            string processType = dr2["process_type"].ToString();
            string subcategory = dr2["subcategory"].ToString();
            int? equipmentModelId = dr2["equipment_model_id"] != DBNull.Value ? int.Parse(dr2["equipment_model_id"].ToString()) : (int?)null;
            string equipmentModelName = dr2["equipment_model_name"].ToString();
            string modelBrand = dr2["model_brand"].ToString();
            List<EquipmentSettingsValue> equipmentSettingsValues = CreateAttributeList(dt);


            EquipmentSettings result = new EquipmentSettings();
            result.fkExperimentProcess = fkExperimentProcess;
            result.fkProcessType = fkProcessType;
            result.label = label;
            result.equipmentId = equipmentId;
            result.equipmentName = equipmentName;
            result.processType = processType;
            result.equipmentSettingsValues = equipmentSettingsValues;
            result.equipmentModelId = equipmentModelId;
            result.equipmentModelName = equipmentModelName;
            result.modelBrand = modelBrand;
            result.subcategory = subcategory;
            return result;
        }
        public static List<EquipmentSettingsValue> GetEquipmentSettingsByEquipmentId(int? equipmentId = null, int? processId = null, int? equipmentModelId = null)
        {
            DataTable dt;
            int returnedProcessId = ProcessTypeDa.GetProcess(equipmentId, processId, equipmentModelId);
            try
            {
                var cmd = Db.CreateCommand();

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"SELECT *
                      FROM equipment_attribute_type eat
                        LEFT JOIN process p ON eat.fk_process=p.process_id 
                        LEFT JOIN equipment eq ON p.fk_equipment = eq.equipment_id
                        LEFT JOIN equipment_attributes ea ON eat.fk_attribute = ea.equipment_attribute_id
                        LEFT JOIN attribute_db_type db ON db.attribute_db_type_id = ea.fk_db_type
                        LEFT JOIN process_type pt ON pt.process_type_id=eq.fk_process_type
                        LEFT JOIN attribute_measurement_unit amu ON amu.attribute_measurement_unit_id=ea.fk_attribute_measurement_unit                    
                    WHERE eat.fk_process = :pid
                    ORDER BY eat.order ASC
                    ;";

                Db.CreateParameterFunc(cmd, "@pid", returnedProcessId, NpgsqlDbType.Integer);

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

            // List<EquipmentSettingsValue> list = (from DataRow dr in dt.Rows select CreateObjectEquipmentSettingsValues(dr)).ToList();
            List<EquipmentSettingsValue> equipmentSettingsValues = CreateAttributeList(dt);

            return equipmentSettingsValues;
        }
        public static int AddEquipmentAttributes(NpgsqlCommand cmd, List<EquipmentSettingsValue> equipmentAttributeList, int? experimentProcessId = null, int? batchProcessId = null, int? sequenceContentId = null)
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
                foreach (var item in equipmentAttributeList)
                {
                    if ((bool)item.isParent)
                    {
                        item.value = null;
                    }
                    cmd.Parameters.Clear();
                    cmd.CommandText =
                    @"INSERT INTO public.equipment_attribute_values (

fk_attribute,
value,
fk_experiment_process,
fk_batch_process,
fk_sequence_content,
date_created
)
                    VALUES (:att, :val, :ep, :bp, :sc, now()::timestamp
);";

                    Db.CreateParameterFunc(cmd, "@att", item.fkAttribute, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@val", item.value, NpgsqlDbType.Text);
                    Db.CreateParameterFunc(cmd, "@ep", experimentProcessId, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@bp", batchProcessId, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@sc", sequenceContentId, NpgsqlDbType.Integer);
                    Db.ExecuteNonQuery(cmd, false);

                    //insert children values
                    AddEquipmentAttributes(cmd, item.children, experimentProcessId, batchProcessId, sequenceContentId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting equipment attributes", ex);
            }

            return 0;
        }
        public static List<PreviousProcessResponse> GetRecentlyUsedProcess(int processId)
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
                    @"SELECT * FROM
                    (
                        (
                            SELECT ep.experiment_process_id as id, NULL as batch_process_id, ep.experiment_process_id, p.fk_process_type, ep.label, ep.fk_process, ep.date_created, p.fk_equipment, e.equipment_name, p.fk_equipment_model, m.equipment_model_name, m.model_brand, pt.process_type, pt.subcategory
                            FROM experiment_process ep
                             LEFT JOIN process p ON p.process_id=ep.fk_process
                            LEFT JOIN equipment e ON p.fk_equipment=e.equipment_id
                            LEFT JOIN process_type pt ON pt.process_type_id=p.fk_process_type
                             LEFT JOIN equipment_model m ON m.equipment_model_id=p.fk_equipment_model
                            WHERE p.fk_process_type=:pid
                            ORDER BY ep.experiment_process_id DESC LIMIT 10
                        )
                    UNION
                        (
                        SELECT ep.batch_process_id as id, ep.batch_process_id, NULL as experiment_process_id, p.fk_process_type, ep.label, ep.fk_process, ep.date_created, p.fk_equipment, e.equipment_name, p.fk_equipment_model, m.equipment_model_name, m.model_brand, pt.process_type, pt.subcategory
                        FROM batch_process ep
                         LEFT JOIN process p ON p.process_id=ep.fk_process
                        LEFT JOIN equipment e ON p.fk_equipment=e.equipment_id
                        LEFT JOIN process_type pt ON pt.process_type_id=p.fk_process_type
                         LEFT JOIN equipment_model m ON m.equipment_model_id=p.fk_equipment_model
                        WHERE p.fk_process_type=:pid
                        ORDER BY ep.batch_process_id DESC LIMIT 10
                        )
                    ) t
                    ORDER BY t.date_created DESC, t.id DESC
                    ;";

                Db.CreateParameterFunc(cmd, "@pid", processId, NpgsqlDbType.Integer);

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
            List<PreviousProcessResponse> list = (from DataRow dr in dt.Rows select CreatePreviousProcessResponseObject(dr)).ToList();
            return list;
        }
        public static EquipmentSettings CreateObject(DataRow dr)
        {
            int? fkSequenceContent = (int?)null;
            if (dr.Table.Columns.Contains("fk_sequence_content"))
            {
                fkSequenceContent = dr["fk_sequence_content"] != DBNull.Value ? int.Parse(dr["fk_sequence_content"].ToString()) : (int?)null;
            }
            int? fkExperimentProcess = (int?)null;
            if (dr.Table.Columns.Contains("fk_experiment_process"))
            {
                fkExperimentProcess = dr["fk_experiment_process"] != DBNull.Value ? int.Parse(dr["fk_experiment_process"].ToString()) : (int?)null;
            }

            int? fkBatchProcess = (int?)null;
            if (dr.Table.Columns.Contains("fk_batch_process"))
            {
                fkBatchProcess = dr["fk_batch_process"] != DBNull.Value ? int.Parse(dr["fk_batch_process"].ToString()) : (int?)null;
            }
            string label = null;
            if (dr.Table.Columns.Contains("label"))
            {
                label = dr["label"].ToString();
            }
            long? equipmentAttributeTypeId = (long?)null;
            if (dr.Table.Columns.Contains("equipment_attribute_type_id"))
            {
                equipmentAttributeTypeId = dr["equipment_attribute_type_id"] != DBNull.Value ? long.Parse(dr["equipment_attribute_type_id"].ToString()) : (long?)null;
            }
            long? equipmentAttributeValueId = (long?)null;
            if (dr.Table.Columns.Contains("equipment_attribute_value_id"))
            {
                equipmentAttributeValueId = dr["equipment_attribute_value_id"] != DBNull.Value ? long.Parse(dr["equipment_attribute_value_id"].ToString()) : (long?)null;
            }
            string value = null;
            if (dr.Table.Columns.Contains("value"))
            {
                value = dr["value"].ToString();
            }
            string attributeName = null;
            if (dr.Table.Columns.Contains("value"))
            {
                attributeName = dr["attribute_name"].ToString();
            }
            string type = null;
            if (dr.Table.Columns.Contains("type"))
            {
                type = dr["type"].ToString();
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
            var equipmentSettings = new EquipmentSettings
            {
                equipmentId = dr["equipment_id"] != DBNull.Value ? int.Parse(dr["equipment_id"].ToString()) : (int?)null,
                equipmentName = dr["equipment_name"].ToString(),
                //order = dr["order"] != DBNull.Value ? int.Parse(dr["order"].ToString()) : (int?)null,
                fkExperimentProcess = fkExperimentProcess,
                fkBatchProcess = fkBatchProcess,
                label = label,
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,
                fkProcessType = int.Parse(dr["fk_process_type"].ToString()),
                processType = dr["process_type"].ToString(),
                equipmentAttributeValueId = equipmentAttributeValueId,
                equipmentAttributeTypeId = equipmentAttributeTypeId,
                value = value,
                attributeName = attributeName,
                type = type,
                subcategory = subcategory,
                equipmentModelId = equipmentModelId,
                equipmentModelName = equipmentModelName,
                modelBrand = modelBrand,
                fkSequenceContent = fkSequenceContent
            };
            return equipmentSettings;
        }
        public static List<EquipmentSettingsValue> CreateAttributeList(DataTable dt)
        {
            List<EquipmentSettingsValue> response = new List<EquipmentSettingsValue>();
            foreach (DataRow dr in dt.Rows)
            {
                EquipmentSettingsValue value = CreateObjectEquipmentSettingsValues(dr);
                if ((bool)value.isParent)
                {
                    foreach (DataRow dr2 in dt.Rows)
                    {
                        EquipmentSettingsValue valueChild = new EquipmentSettingsValue();
                        valueChild = CreateObjectEquipmentSettingsValues(dr2);
                        if (valueChild.fkParentAttribute == value.fkAttribute)
                        {
                            value.children.Add(valueChild);
                        }
                    }
                    response.Add(value);
                }
                else
                {
                    if (value.fkParentAttribute == null)
                    {
                        response.Add(value);
                    }
                }
                /*EquipmentSettingsValue value2 = CreateObjectEquipmentSettingsValues(dr);
                response.Add(value2);*/
            }
            return response;
        }
        public static EquipmentSettingsValue CreateObjectEquipmentSettingsValues(DataRow dr)
        {
            long? equipmentAttributeTypeId = (long?)null;
            if (dr.Table.Columns.Contains("equipment_attribute_type_id"))
            {
                equipmentAttributeTypeId = dr["equipment_attribute_type_id"] != DBNull.Value ? long.Parse(dr["equipment_attribute_type_id"].ToString()) : (long?)null;
            }
            long? equipmentAttributeValueId = (long?)null;
            if (dr.Table.Columns.Contains("equipment_attribute_value_id"))
            {
                equipmentAttributeValueId = dr["equipment_attribute_value_id"] != DBNull.Value ? long.Parse(dr["equipment_attribute_value_id"].ToString()) : (long?)null;
            }
            string value = null;
            if (dr.Table.Columns.Contains("value"))
            {
                value = dr["value"].ToString();
            }
            string attMeasurementUnit = null;
            if (dr.Table.Columns.Contains("measurement_unit"))
            {
                attMeasurementUnit = dr["measurement_unit"].ToString();
            }
            bool? isParent = (bool?)null;
            if (dr.Table.Columns.Contains("is_parent"))
            {
                isParent = dr["is_parent"] != DBNull.Value ? bool.Parse(dr["is_parent"].ToString()) : (bool?)null;
            }
            int? fkParentAttribute = (int?)null;
            if (dr.Table.Columns.Contains("fk_parent_attribute"))
            {
                fkParentAttribute = dr["fk_parent_attribute"] != DBNull.Value ? int.Parse(dr["fk_parent_attribute"].ToString()) : (int?)null;
            }
            var equipmentSettings = new EquipmentSettingsValue
            {
                equipmentAttributeValueId = equipmentAttributeValueId,
                equipmentAttributeTypeId = equipmentAttributeTypeId,
                fkAttribute = dr["fk_attribute"] != DBNull.Value ? int.Parse(dr["fk_attribute"].ToString()) : (int?)null,
                //order = dr["order"] != DBNull.Value ? int.Parse(dr["order"].ToString()) : (int?)null,
                attributeName = dr["attribute_name"].ToString(),
                fkDbType = dr["fk_db_type"] != DBNull.Value ? int.Parse(dr["fk_db_type"].ToString()) : (int?)null,
                value = value,
                type = dr["type"].ToString(),
                attMeasurementUnit = attMeasurementUnit,
                isParent = isParent,
                fkParentAttribute = fkParentAttribute
            };
            return equipmentSettings;
        }
        public static PreviousProcessResponse CreatePreviousProcessResponseObject(DataRow dr)
        {

            var previousProcessResponse = new PreviousProcessResponse
            {
                batchProcessId = dr["batch_process_id"] != DBNull.Value ? int.Parse(dr["batch_process_id"].ToString()) : (int?)null,
                experimentProcessId = dr["experiment_process_id"] != DBNull.Value ? int.Parse(dr["experiment_process_id"].ToString()) : (int?)null,
                fkProcessType = dr["fk_process_type"] != DBNull.Value ? int.Parse(dr["fk_process_type"].ToString()) : (int?)null,
                label = dr["label"].ToString(),
                fkProcess = dr["fk_process"] != DBNull.Value ? int.Parse(dr["fk_process"].ToString()) : (int?)null,
                fkEquipment = dr["fk_equipment"] != DBNull.Value ? int.Parse(dr["fk_equipment"].ToString()) : (int?)null,
                equipmentName = dr["equipment_name"].ToString(),
                fkEquipmentModel = dr["fk_equipment_model"] != DBNull.Value ? int.Parse(dr["fk_equipment_model"].ToString()) : (int?)null,
                equipmentModelName = dr["equipment_model_name"].ToString(),
                modelBrand = dr["model_brand"].ToString(),
                processType = dr["process_type"].ToString(),
                subcategory = dr["subcategory"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null

            };
            return previousProcessResponse;
        }
    }
}