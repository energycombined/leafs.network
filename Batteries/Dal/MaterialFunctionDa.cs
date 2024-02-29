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
    public class MaterialFunctionDa
    {
        public static List<MaterialFunction> GetAllMaterialFunctions(int? materialFunctionId = null)
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
                    FROM material_function mf
                    WHERE (mf.material_function_id = :mfid or :mfid is null);";

                Db.CreateParameterFunc(cmd, "@mfid", materialFunctionId, NpgsqlDbType.Integer);

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

            List<MaterialFunction> list = (from DataRow dr in dt.Rows select CreateObject(dr)).ToList();

            return list;
        }
        public static string GetAllMaterialFunctionsJsonForDropdown(int? materialFunctionId = null)
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
                      FROM ( select material_function_id as id, COALESCE(mf.material_function_name, '') as text
                              from material_function mf
                              where (mf.material_function_id = :mfid or :mfid is null)
                          ) as t;";

                Db.CreateParameterFunc(cmd, "@mfid", materialFunctionId, NpgsqlDbType.Integer);

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
        public static List<MaterialFunction> GetMaterialFunctionsByName(string search = null, int? type = null)
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

                    FROM material_function mf
                        
                    WHERE (lower(mf.material_function_name) LIKE lower('%'|| :search ||'%') or :search is null)
                    LIMIT 10
                        ;";

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

            List<MaterialFunction> list = (from DataRow dr in dt.Rows select CreateObject(dr)).ToList();

            return list;
        }
        public static int AddMaterialFunction(MaterialFunction materialFunction)
        {
            int result = 0;
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"INSERT INTO public.material_function (material_function_name, date_created)
                    VALUES (:name, now()::timestamp) RETURNING material_function_id;";

                Db.CreateParameterFunc(cmd, "@name", materialFunction.materialFunctionName, NpgsqlDbType.Text);

                result = int.Parse(Db.ExecuteScalar(cmd));
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting material function", ex);
            }

            return result;
        }
        public static int UpdateMaterialFunction(MaterialFunction materialFunction)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.material_function
                        SET material_function_name=:name, last_change=now()::timestamp
                        WHERE material_function_id=:mfid;";

                Db.CreateParameterFunc(cmd, "@name", materialFunction.materialFunctionName, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@mfid", materialFunction.materialFunctionId, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Material Function info", ex);
            }
            return 0;
        }
        public static int DeleteMaterialFunction(int materialFunctionId)
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
                            @"SELECT m.battery_component_id 
                            FROM battery_component m
                            WHERE m.fk_function=:fid;";

                Db.CreateParameterFunc(cmd, "@fid", materialFunctionId, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count > 0)
                {
                    throw new Exception("This function is being used in a battery component");
                }

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                            @"SELECT m.batch_content_id 
                            FROM batch_content m
                            WHERE m.fk_function=:fid;";

                Db.CreateParameterFunc(cmd, "@fid", materialFunctionId, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count > 0)
                {
                    throw new Exception("This function is being used in a batch");
                }                

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.material_function
                                WHERE material_function_id=:mfid;";

                Db.CreateParameterFunc(cmd, "@mfid", materialFunctionId, NpgsqlDbType.Integer);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        public static MaterialFunction CreateObject(DataRow dr)
        {
            var materialFunction = new MaterialFunction
            {
                materialFunctionId = (int)dr["material_function_id"],
                materialFunctionName = dr["material_function_name"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,
                lastChange = dr["last_change"] != DBNull.Value ? DateTime.Parse(dr["last_change"].ToString()) : (DateTime?)null
            };
            return materialFunction;
        }
    }
}