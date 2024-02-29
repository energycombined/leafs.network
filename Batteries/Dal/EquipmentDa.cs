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
    public class EquipmentDa
    {
        public static string GetAllEquipmentJsonForDropdown(int? processTypeId = null, int? equipmentId = null)
        {
            string json = "";
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

//                cmd.CommandText =
//                    @"SELECT json_agg(row_to_json(t))
//                      FROM ( select equipment_id as value, COALESCE(eq.equipment_name, '') as text
//                              from equipment eq
//                              where (eq.fk_process_type = :ptid or :ptid is null) and
//                                    (eq.equipment_id = :eid or :eid is null)
//                          ) as t;";
                cmd.CommandText =
                    @"SELECT json_agg(row_to_json(t))
                      FROM ( select equipment_id as value, COALESCE(eq.equipment_name, '') as text
                              from equipment eq
                              where
                                    eq.is_manual_settings != true and
                                    (eq.equipment_id = :eid or :eid is null)
                        UNION (
                              select equipment_id as value, COALESCE(eq.equipment_name, '') as text
                              from equipment eq
                              where (eq.fk_process_type = :ptid or :ptid is null) and
                                      eq.is_manual_settings = true
                             )
                          ) as t;";

                Db.CreateParameterFunc(cmd, "@ptid", processTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@eid", equipmentId, NpgsqlDbType.Integer);

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
        public static List<Equipment> GetEquipmentByProcessId(string search, int? fkProcess = null, int? page = 0)
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
                     @"SELECT DISTINCT(p.fk_equipment), e.equipment_name, e.equipment_id, e.equipment_label, p.fk_process_type
                        FROM process p
                        LEFT JOIN equipment_attribute_type at ON at.fk_process=p.process_id
                        LEFT JOIN equipment e ON e.equipment_id=p.fk_equipment
                    WHERE (lower(e.equipment_name) LIKE lower('%'|| :search ||'%') or :search is null) 
                    AND (p.fk_process_type=:process)

                    ORDER BY e.equipment_id DESC
                    LIMIT 10 OFFSET :offset
                        ;";

                Db.CreateParameterFunc(cmd, "@process", fkProcess, NpgsqlDbType.Integer);
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

            List<Equipment> list = (from DataRow dr in dt.Rows select CreateEquipmentObject(dr)).ToList();

            return list;
        }
        public static List<EquipmentExt> GetAllEquipment(int? processTypeId = null, int? equipmentId = null)
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
                              FROM equipment eq
                              LEFT JOIN process_type pt ON eq.fk_process_type = pt.process_type_id
                              WHERE (eq.fk_process_type = :ptid or :ptid is null) and
                                    (eq.equipment_id = :eid or :eid is null)
                              ORDER BY eq.fk_process_type
                              ;";

                Db.CreateParameterFunc(cmd, "@ptid", processTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@eid", equipmentId, NpgsqlDbType.Integer);

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

            List<EquipmentExt> list = (from DataRow dr in dt.Rows select CreateEquipmentObjectExt(dr)).ToList();

            return list;
        }
        public static int AddEquipment(Equipment equipment)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"INSERT INTO public.equipment (equipment_name,
equipment_label,
fk_process_type)
                    VALUES (:name, :label, :ptid);";

                Db.CreateParameterFunc(cmd, "@name", equipment.equipmentName, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", equipment.equipmentLabel, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@ptid", equipment.fkProcessType, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting equipment", ex);
            }

            return 0;
        }
        public static int UpdateEquipment(Equipment equipment)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.equipment
                        SET equipment_name=:name, equipment_label=:label, fk_process_type=:ptid
                        WHERE equipment_id=:eid;";

                Db.CreateParameterFunc(cmd, "@name", equipment.equipmentName, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", equipment.equipmentLabel, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@ptid", equipment.fkProcessType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@eid", equipment.equipmentId, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating equipment info", ex);
            }
            return 0;
        }
        public static int DeleteEquipment(int equipmentId)
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
                            @"SELECT m.milling_id 
                            FROM milling m
                            WHERE m.fk_equipment=:eid;";

                Db.CreateParameterFunc(cmd, "@eid", equipmentId, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count > 0)
                {
                    throw new Exception("There is a process referring to this particular equipment.");
                }

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.equipment
                                WHERE equipment_id=:eid;";

                Db.CreateParameterFunc(cmd, "@eid", equipmentId, NpgsqlDbType.Integer);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        public static Equipment CreateEquipmentObject(DataRow dr)
        {
            var Equipment = new Equipment
            {
                equipmentId = (int)dr["equipment_id"],
                equipmentName = dr["equipment_name"].ToString(),
                equipmentLabel = dr["equipment_label"].ToString(),
                fkProcessType = dr["fk_process_type"] != DBNull.Value ? int.Parse(dr["fk_process_type"].ToString()) : (int?)null,                
            };
            return Equipment;
        }
        private static EquipmentExt CreateEquipmentObjectExt(DataRow dr)
        {
            var Equipment = CreateEquipmentObject(dr);

           
            var EquipmentExt = new EquipmentExt(Equipment)
            {
                processType = dr["process_type"].ToString(),
            };
            return EquipmentExt;
        }
    }
}