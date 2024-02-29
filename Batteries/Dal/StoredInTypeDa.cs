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
    public class StoredInTypeDa
    {
        public static List<StoredInType> GetAllStoredInTypes(int? storedInTypeId = null)
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
                    FROM stored_in_type s                        
                    WHERE (s.stored_in_type_id = :sid or :sid is null);";

                Db.CreateParameterFunc(cmd, "@sid", storedInTypeId, NpgsqlDbType.Integer);

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

            List<StoredInType> list = (from DataRow dr in dt.Rows select CreateObject(dr)).ToList();

            return list;
        }
        public static List<StoredInType> GetStoredInTypesByName(string search = null)
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
                    FROM stored_in_type s  
                    WHERE (lower(s.stored_in_type) LIKE lower('%'|| :search ||'%') or :search is null)
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

            List<StoredInType> list = (from DataRow dr in dt.Rows select CreateObject(dr)).ToList();

            return list;
        }
        public static int AddStoredInType(StoredInType storedInType)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"INSERT INTO public.stored_in_type (stored_in_type)
                    VALUES (:st);";

                Db.CreateParameterFunc(cmd, "@st", storedInType.storedInType, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting storing type", ex);
            }

            return 0;
        }
        public static int UpdateStoredInType(StoredInType storedInType)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.stored_in_type
                        SET stored_in_type=:sn
                        WHERE stored_in_type_id=:sid;";

                Db.CreateParameterFunc(cmd, "@sn", storedInType.storedInType, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@sid", storedInType.storedInTypeId, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating storing type info", ex);
            }
            return 0;
        }
        public static int DeleteStoredInType(int storedInTypeId)
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
                            WHERE m.fk_stored_in_type=:sid;";

                Db.CreateParameterFunc(cmd, "@sid", storedInTypeId, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count > 0)
                {
                    throw new Exception("There is some material associated to this storage type!");
                }

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.stored_in_type
                                WHERE stored_in_type_id=:sid;";

                Db.CreateParameterFunc(cmd, "@sid", storedInTypeId, NpgsqlDbType.Integer);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        public static StoredInType CreateObject(DataRow dr)
        {
            var storedInType = new StoredInType
            {
                storedInTypeId = (int)dr["stored_in_type_id"],
                storedInType = dr["stored_in_type"].ToString()
            };
            return storedInType;
        }
    }
}