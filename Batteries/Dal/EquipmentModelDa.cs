using Batteries.Dal.Base;
using Batteries.Forms.DataSources;
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
    public class EquipmentModelDa
    {
        public static string GetAllEquipmentModelsJsonForDropdown(int? equipmentId = null, int? equipmentModelId = null)
        {
            string json = "";
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                cmd.CommandText =
                    @"SELECT json_agg(row_to_json(t))
                      FROM ( select equipment_model_id as value, COALESCE(m.equipment_model_name, '') as text
                              from equipment_model m
                              where (m.fk_equipment = :eid or :eid is null) and
                                    (m.equipment_model_id = :emid or :emid is null)
                          ) as t;";

                Db.CreateParameterFunc(cmd, "@eid", equipmentId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@emid", equipmentModelId, NpgsqlDbType.Integer);

                json = Db.ExecuteScalar(cmd);

                if (json == null || json == "")
                {
                    json = "[]";
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return json;
        }
        public static List<EquipmentModelExt> GetAllEquipmentModels(int? equipmentId = null, int? equipmentModelId = null)
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
                              FROM equipment_model m
                              LEFT JOIN equipment eq ON m.fk_equipment = eq.equipment_id
                              WHERE (m.fk_equipment = :eid or :eid is null) and
                                    (m.equipment_model_id = :emid or :emid is null);";

                Db.CreateParameterFunc(cmd, "@eid", equipmentId, NpgsqlDbType.Integer);
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

            List<EquipmentModelExt> list = (from DataRow dr in dt.Rows select CreateEquipmentModelObjectExt(dr)).ToList();

            return list;
        }
        public static List<EquipmentModelExt> GetEquipmentModelsForDropdown(string search, int? processTypeId, int? equipmentId = null, int? page = 0)
        {
            DataTable dt;

            try
            {
                var cmd = Db.CreateCommand();
                if (equipmentId != null)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.CommandText =
                         @"SELECT DISTINCT(p.fk_equipment_model), p.fk_equipment, em.equipment_model_id, em.equipment_model_name, em.model_brand, p.fk_process_type, p.fk_equipment_model, e.equipment_name
                           FROM process p
                           INNER JOIN equipment_attribute_type at ON at.fk_process = p.process_id
                           LEFT JOIN equipment_model em ON em.equipment_model_id = p.fk_equipment_model
                           LEFT JOIN equipment e ON p.fk_equipment=e.equipment_id
                           WHERE (lower(em.equipment_model_name) LIKE lower('%'|| :search ||'%') or :search is null) 
                           AND (p.fk_process_type = :processtype) AND (p.fk_equipment = :eqtype)
                       
                           ORDER BY em.equipment_model_id DESC
                           LIMIT 10 OFFSET :offset
                         ;";

                    Db.CreateParameterFunc(cmd, "@processtype", processTypeId, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@eqtype", equipmentId, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
                    Db.CreateParameterFunc(cmd, "@offset", (page - 1) * 10, NpgsqlDbType.Integer);

                    dt = Db.ExecuteSelectCommand(cmd);
                }
                else
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.CommandText =
                         @"SELECT DISTINCT(p.fk_equipment_model), p.fk_equipment, em.equipment_model_id, em.equipment_model_name, em.model_brand, p.fk_process_type, p.fk_equipment_model, e.equipment_name
                            FROM process p	
                            INNER JOIN equipment_attribute_type at ON at.fk_process = p.process_id
                            LEFT JOIN equipment_model em ON em.equipment_model_id = p.fk_equipment_model
                            LEFT JOIN equipment e ON p.fk_equipment=e.equipment_id
                            WHERE (lower(em.equipment_model_name) LIKE lower('%'|| :search ||'%') or :search is null) 
                            AND p.fk_process_type=:processtype AND fk_equipment_model IS NOT NULL
                       
                            ORDER BY em.equipment_model_id DESC
                            LIMIT 10 OFFSET :offset
                     ;";

                    Db.CreateParameterFunc(cmd, "@processtype", processTypeId, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
                    Db.CreateParameterFunc(cmd, "@offset", (page - 1) * 10, NpgsqlDbType.Integer);

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

            List<EquipmentModelExt> list = (from DataRow dr in dt.Rows select CreateEquipmentModelObjectExt(dr)).ToList();

            return list;
        }
        public static int AddEquipmentModel(EquipmentModel equipmentModel)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"INSERT INTO public.equipment_model (fk_equipment, equipment_model_name
)
                    VALUES (:eid, :name);";

                Db.CreateParameterFunc(cmd, "@eid", equipmentModel.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@name", equipmentModel.equipmentModelName, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting Equipment Model", ex);
            }

            return 0;
        }
        public static int UpdateEquipmentModel(Batteries.Models.EquipmentModel equipmentModel)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.equipment_model
                        SET fk_equipment=:eid, equipment_model_name=:name
                        WHERE equipment_model_id=:emid;";

                Db.CreateParameterFunc(cmd, "@eid", equipmentModel.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@name", equipmentModel.equipmentModelName, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@emid", equipmentModel.equipmentModelId, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Equipment Model info", ex);
            }
            return 0;
        }
        public static int DeleteEquipmentModel(int equipmentModelId)
        {
            DataTable dt;
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                //CHECK IF ANY EQUIPMENT IS CONNECTED TO THIS MODEL
                cmd.CommandText =
                            @"SELECT m.settings_id
                            FROM tube_furnace m
                            WHERE m.fk_equipment_model=:emid;";

                Db.CreateParameterFunc(cmd, "@emid", equipmentModelId, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count > 0)
                {
                    throw new Exception("There is a process referring to this particular Equipment Model.");
                }

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                            @"SELECT m.settings_id
                            FROM heating_plate m
                            WHERE m.fk_equipment_model=:emid;";

                Db.CreateParameterFunc(cmd, "@emid", equipmentModelId, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count > 0)
                {
                    throw new Exception("There is a process referring to this particular Equipment Model.");
                }



                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.equipment_model
                                WHERE equipment_model_id=:eid;";

                Db.CreateParameterFunc(cmd, "@eid", equipmentModelId, NpgsqlDbType.Integer);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        public static EquipmentModel CreateEquipmentModelObject(DataRow dr)
        {
            var equipmentModel = new EquipmentModel
            {
                equipmentModelId = (int)dr["equipment_model_id"],
                fkEquipment = dr["fk_equipment"] != DBNull.Value ? int.Parse(dr["fk_equipment"].ToString()) : (int?)null,
                equipmentModelName = dr["equipment_model_name"].ToString(),
                modelBrand = dr["model_brand"].ToString()
            };
            return equipmentModel;
        }
        private static EquipmentModelExt CreateEquipmentModelObjectExt(DataRow dr)
        {
            var equipmentModel = CreateEquipmentModelObject(dr);

            string equipmentName = (string)null;
            if (dr.Table.Columns.Contains("equipment_name"))
            {
                equipmentName = dr["equipment_name"].ToString();
            }
            var equipmentModelExt = new EquipmentModelExt(equipmentModel)
            {
                equipmentName = equipmentName
            };

            return equipmentModelExt;
        }
    }
}