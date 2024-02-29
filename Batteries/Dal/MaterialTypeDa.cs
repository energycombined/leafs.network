using Batteries.Dal.Base;
using Batteries.Models;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Batteries.Dal
{
    public class MaterialTypeDa
    {
        public static List<MaterialType> GetAllMaterialTypes(int? materialTypeId = null)
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
                    FROM material_type mt                        
                    WHERE (mt.material_type_id = :mtid or :mtid is null);";

                Db.CreateParameterFunc(cmd, "@mtid", materialTypeId, NpgsqlDbType.Integer);

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

            List<MaterialType> list = (from DataRow dr in dt.Rows select CreateObject(dr)).ToList();

            return list;
        }
        public static List<MaterialType> GetMaterialTypesByName(string search = null)
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
                    FROM material_type mt  
                    WHERE (lower(mt.material_type) LIKE lower('%'|| :search ||'%') or :search is null);";

                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);

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

            List<MaterialType> list = (from DataRow dr in dt.Rows select CreateObject(dr)).ToList();

            return list;
        }
        public static int AddMaterialType(MaterialType materialType)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"INSERT INTO public.material_type (material_type)
                    VALUES (:mtn);";

                Db.CreateParameterFunc(cmd, "@mtn", materialType.materialType, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting material type", ex);
            }

            return 0;
        }
        public static int UpdateMaterialType(MaterialType materialType)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.material_type
                        SET material_type=:mtn
                        WHERE material_type_id=:mtid;";

                Db.CreateParameterFunc(cmd, "@mtn", materialType.materialType, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@mtid", materialType.materialTypeId, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating material type info", ex);
            }
            return 0;
        }
        public static int DeleteMaterialType(int materialTypeId)
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
                            @"SELECT m.material_id 
                            FROM material m
                            WHERE m.fk_material_type=:mtid;";

                Db.CreateParameterFunc(cmd, "@mtid", materialTypeId, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count > 0)
                {
                    throw new Exception("There is some material associated to this material type!");
                }

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.material_type
                                WHERE material_type_id=:mtid;";

                Db.CreateParameterFunc(cmd, "@mtid", materialTypeId, NpgsqlDbType.Integer);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        public static MaterialType CreateObject(DataRow dr)
        {
            var materialType = new MaterialType
            {
                materialTypeId = (int)dr["material_type_id"],
                materialType = dr["material_type"].ToString()
            };
            return materialType;
        }
    }
}