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
    public class ProcessTypeDa
    {
        public static List<ProcessTypeExt> GetAllProcessTypes(int? processTypeId = null)
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
                    FROM process_type pt
                    WHERE (pt.process_type_id = :ptid or :ptid is null);";

                Db.CreateParameterFunc(cmd, "@ptid", processTypeId, NpgsqlDbType.Integer);

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

            List<ProcessTypeExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<ProcessTypeExt> GetProcessTypesByName(string search, int? page = 0)
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
                    @"SELECT DISTINCT (pt.process_type_id), pt.process_type, pt.process_database_type, pt.subcategory
                    FROM process p
                    LEFT JOIN process_type pt ON p.fk_process_type=pt.process_type_id
                    WHERE ((lower(pt.process_type) LIKE lower('%'|| :search ||'%') or lower(pt.subcategory) LIKE lower('%'|| :search ||'%') or :search is null)) 
                    
                    ORDER BY pt.process_type_id DESC
                    LIMIT 10 OFFSET :offset
                        ;";

                //Db.CreateParameterFunc(cmd, "@ptid", processTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@offset", (page - 1) * 10, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);

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

            List<ProcessTypeExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }

        public static int UpdateProcessType(ProcessType processType)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.process_type
                        SET process_type=:ptn
                        WHERE process_type_id=:ptid;";

                Db.CreateParameterFunc(cmd, "@ptn", processType.processType, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@ptid", processType.processTypeId, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating process type info", ex);
            }
            return 0;
        }
        public static int GetProcess(int? equipmentId = null, int? processId = null, int? equipmentModelId = null)
        {
            int returnedProcessId;
            try
            {
                
                if (equipmentModelId == null)
                {
                    var cmd = Db.CreateCommand();
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.CommandText =
                      @"SELECT * from process
                    WHERE fk_process_type=:pid AND fk_equipment=:eqid AND fk_equipment_model is null
                    ;";

                    Db.CreateParameterFunc(cmd, "@pid", processId, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@eqid", equipmentId, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@mid", equipmentModelId, NpgsqlDbType.Integer);

                    returnedProcessId = int.Parse(Db.ExecuteScalar(cmd));
                }
                else
                {
                    var cmd = Db.CreateCommand();
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.CommandText =
                      @"SELECT * from process
                    WHERE fk_process_type=:pid 
                    --AND fk_equipment=:eqid 
                    AND fk_equipment_model=:mid
                    ;";

                    Db.CreateParameterFunc(cmd, "@pid", processId, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@eqid", equipmentId, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@mid", equipmentModelId, NpgsqlDbType.Integer);

                    returnedProcessId = int.Parse(Db.ExecuteScalar(cmd));
                }
                if(returnedProcessId == 0)
                {
                    throw new Exception("Error in process");
                }
               
            }
            catch (Exception ex)
            {
                throw new Exception("Error in process", ex);
            }
            return returnedProcessId;
        }

        public static ProcessType CreateObject(DataRow dr)
        {
            var processType = new ProcessType
            {
                processTypeId = (int)dr["process_type_id"],
                processType = dr["process_type"].ToString(),
                processDatabaseType = dr["process_database_type"].ToString(),
                subcategory = dr["subcategory"].ToString()
            };
            return processType;
        }
        private static ProcessTypeExt CreateObjectExt(DataRow dr)
        {
            var processType = CreateObject(dr);

            var processTypeExt = new ProcessTypeExt(processType)
            {                
            };
            return processTypeExt;
        }
    }
}