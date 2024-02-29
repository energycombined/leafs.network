using Batteries.Dal.Base;
using Batteries.Models;
using Batteries.Models.Requests;
using Batteries.Models.Responses;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Batteries.Dal
{
    public class ProcessSequenceDa
    {
        /// <summary>
        /// Submit sequence of processes
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static int SubmitProcessSequence(AddSequenceRequest req)
        {
            //METHOD USED FOR CREATING FROM EXPERIMENT UI ONLY
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandType = CommandType.Text;

            NpgsqlTransaction t = cmd.Connection.BeginTransaction();

            DataTable dt;
            int returnedSequenceID = 0;

            try
            {

                //CHECK BATCH REQUEST

                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"SELECT * FROM public.process_sequence WHERE label=:label ;";

                Db.CreateParameterFunc(cmd, "@label", req.sequenceInfo.label, NpgsqlDbType.Text);

                dt = Db.ExecuteSelectCommand(cmd, false);
                if (dt.Rows.Count > 0)
                {
                    throw new Exception("Sequence already exist");
                }

                //INSERT BATCH GENERAL INFO
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"INSERT INTO public.process_sequence (label, fk_user, fk_research_group, date_created)
                    VALUES (:label, :uid, :rid, now()::timestamp)
                    RETURNING process_sequence_id;";

                Db.CreateParameterFunc(cmd, "@label", req.sequenceInfo.label, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@uid", req.sequenceInfo.fkUser, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rid", req.sequenceInfo.fkResearchGroup, NpgsqlDbType.Integer);

                returnedSequenceID = int.Parse(Db.ExecuteScalar(cmd, false));

                if (returnedSequenceID <= 0)
                {
                    t.Rollback();
                    //return 5;
                    throw new Exception("Error inserting sequence");
                }
                dynamic sequenceProcessList = req.sequenceProcesses;
                if (sequenceProcessList.Count <= 0)
                {
                    throw new Exception("At least one process needs to be chosen");
                }
                foreach (var process in req.sequenceProcesses)
                {
                    int returnedProcessId = ProcessTypeDa.GetProcess(process.equipmentId, process.processTypeId, process.equipmentModelId);
                    cmd.Parameters.Clear();
                    cmd.CommandText =
                         @"INSERT INTO public.process_sequence_content (fk_process_sequence, fk_process, order_in_step, process_label, date_created)
                    VALUES (:ps, :process, :order, :pl, now()::timestamp)
                    RETURNING process_sequence_content_id;";

                    Db.CreateParameterFunc(cmd, "@ps", returnedSequenceID, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@process", returnedProcessId, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@order", process.processOrderInStep, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@pl", process.processLabel, NpgsqlDbType.Text);

                    int returnedSequenceContentID = int.Parse(Db.ExecuteScalar(cmd, false));
                    if (returnedSequenceContentID <= 0)
                    {
                        t.Rollback();
                        throw new Exception("Error inserting sequence content");
                    }
                    string sequenceProcessAttributesString = Convert.ToString(process.processAttributes);
                    List<EquipmentSettingsValue> equipmentAttributeList = JsonConvert.DeserializeObject<List<EquipmentSettingsValue>>(sequenceProcessAttributesString) as List<EquipmentSettingsValue>;

                    EquipmentSettingsDa.AddEquipmentAttributes(cmd, equipmentAttributeList, null, null, returnedSequenceContentID);

                }

                t.Commit();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                t.Rollback();
                cmd.Connection.Close();
                throw new Exception(ex.Message);
            }
            return 0;
            // return returnedSequence;
        }
        public static List<SequenceExt> GetProcessSequence(string search, int? page = 0)
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
                    @"SELECT * FROM process_sequence ps
                    LEFT JOIN users u ON ps.fk_user=u.user_id
                    LEFT JOIN research_group rg ON ps.fk_research_group=rg.research_group_id
                    
                    WHERE (lower(ps.label) LIKE lower('%'|| :search ||'%') or :search is null)
                    
                    ORDER BY ps.process_sequence_id DESC
                    LIMIT 10 OFFSET :offset
                        ;";
                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@offset", (page - 1) * 10, NpgsqlDbType.Integer);

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

            List<SequenceExt> list = (from DataRow dr in dt.Rows select CreateprocessSequenceObjectExt(dr)).ToList();

            return list;
        }
        public static List<ProcessSequenceContentExt> GetSequenceContent(int? processSequenceId = null)
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
                    @"SELECT * FROM process_sequence_content psc
                    LEFT JOIN process p ON psc.fk_process=p.process_id
                    LEFT JOIN equipment e ON p.fk_equipment=e.equipment_id
                    LEFT JOIN process_type pt ON pt.process_type_id=p.fk_process_type
                    LEFT JOIN equipment_model m ON p.fk_equipment_model=m.equipment_model_id
                    WHERE psc.fk_process_sequence=:psid
					ORDER BY psc.order_in_step;";

                Db.CreateParameterFunc(cmd, "@psid", processSequenceId, NpgsqlDbType.Integer);

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

            List<ProcessSequenceContentExt> list = (from DataRow dr in dt.Rows select CreateSequenceContentObjectExt(dr)).ToList();

            return list;
        }
        /*  public static List<EquipmentSettings> GetValuesBySequenceId(int processSequenceId)
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
                      @"SELECT * FROM equipment_attribute_values eav
                      LEFT JOIN equipment_attribute_type eat ON eav.fk_attribute=eat.equipment_attribute_type_id
                      LEFT JOIN equipment_attributes ea ON eav.fk_attribute=ea.equipment_attribute_id
                      LEFT JOIN process_sequence_content psc ON psc.process_sequence_content_id=eav.fk_sequence_content
                      LEFT JOIN process_sequence ps ON ps.process_sequence_id=psc.fk_process_sequence
                      LEFT JOIN process p ON psc.fk_process=p.process_id
                      LEFT JOIN equipment e ON p.fk_equipment=e.equipment_id
                      LEFT JOIN process_type pt ON pt.process_type_id=p.fk_process_type
                      LEFT JOIN attributes_db_types db ON db.attribute_db_type_id=ea.fk_db_type
                      LEFT JOIN equipment_model m ON p.fk_equipment_model=m.equipment_model_id
                      WHERE psc.fk_process_sequence=:psid 
                      ;";

                  Db.CreateParameterFunc(cmd, "@psid", processSequenceId, NpgsqlDbType.Integer);

                  dt = Db.ExecuteSelectCommand(cmd);
              }
              catch (Exception ex)
              {
                  throw new Exception(ex.Message);
              }

              if (dt == null || dt.Rows.Count == 0)
              {
                  throw new Exception("Data does not exsist");
              }
              List<EquipmentSettings> list = (from DataRow dr in dt.Rows select CreateObject(dr)).ToList();
              List<EquipmentSettings> distinctExperimentSettingsValues =
              list
             .GroupBy(pId => pId.fkSequenceContent)
               .Select(first => first.First())
               .Where(s => s.fkSequenceContent != null)
               .ToList();
              foreach (EquipmentSettings a in distinctExperimentSettingsValues)
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
              return distinctExperimentSettingsValues;
          }*/
        private static ProcessSequence CreateProcessSequenceObject(DataRow dr)
        {
            var sequense = new ProcessSequence
            {
                processSequenceId = int.Parse(dr["process_sequence_id"].ToString()),
                label = dr["label"].ToString(),
                fkUser = dr["fk_user"] != DBNull.Value ? int.Parse(dr["fk_user"].ToString()) : (int?)null,
                fkResearchGroup = dr["fk_research_group"] != DBNull.Value ? int.Parse(dr["fk_research_group"].ToString()) : (int?)null,
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,
            };
            return sequense;
        }
        private static SequenceExt CreateprocessSequenceObjectExt(DataRow dr)
        {
            var sequense = CreateProcessSequenceObject(dr);


            var sequenseExt = new SequenceExt(sequense)
            {
                username = dr["username"].ToString(),
                researchGroupName = dr["research_group_name"].ToString()
            };
            return sequenseExt;
        }
        private static ProcessSequenceContent CreateProcessSequenceContentObject(DataRow dr)
        {
            var sequense = new ProcessSequenceContent
            {
                processSequenceContentId = int.Parse(dr["process_sequence_content_id"].ToString()),
                fkProcess = dr["fk_process"] != DBNull.Value ? int.Parse(dr["fk_process"].ToString()) : (int?)null,
                fkProcessSequence = dr["fk_process_sequence"] != DBNull.Value ? int.Parse(dr["fk_process_sequence"].ToString()) : (int?)null,
                order = dr["order_in_step"] != DBNull.Value ? int.Parse(dr["order_in_step"].ToString()) : (int?)null,
                processLabel = dr["process_label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,
            };
            return sequense;
        }
        private static ProcessSequenceContentExt CreateSequenceContentObjectExt(DataRow dr)
        {
            var sequense = CreateProcessSequenceContentObject(dr);


            var sequenseExt = new ProcessSequenceContentExt(sequense)
            {
                fkProcessType = dr["fk_process_type"] != DBNull.Value ? int.Parse(dr["fk_process_type"].ToString()) : (int?)null,
                processType = dr["process_type"].ToString(),
                subcategory = dr["subcategory"].ToString(),
                fkEquipment = dr["fk_equipment"] != DBNull.Value ? int.Parse(dr["fk_equipment"].ToString()) : (int?)null,
                equipmentName = dr["equipment_name"].ToString(),
                fkEquipmentModel = dr["fk_equipment_model"] != DBNull.Value ? int.Parse(dr["fk_equipment_model"].ToString()) : (int?)null,
                equipmentModelName = dr["equipment_model_name"].ToString(),
                modelBrand = dr["model_brand"].ToString(),
            };
            return sequenseExt;
        }
    }
}