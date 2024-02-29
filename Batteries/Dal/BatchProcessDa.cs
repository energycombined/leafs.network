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
    public class BatchProcessDa
    {
        //koga brises batchProcess have in mindtrebada brises i atributina proces -> na primer od Milling tabela.. 
        public static List<BatchProcessExt> GetAllBatchProcesses(int? batchId = null, int? stepId = null, int? processTypeId = null, long? batchProcessId = null)
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
                    FROM batch_process bp
                    LEFT JOIN batch b ON bp.fk_batch = b.batch_id
					LEFT JOIN process p ON p.process_id=bp.fk_process
                    LEFT JOIN process_type pt ON p.fk_process_type = pt.process_type_id                          
                    LEFT JOIN equipment eq ON p.fk_equipment=eq.equipment_id
                    LEFT JOIN equipment_model m ON p.fk_equipment_model=m.equipment_model_id
                    LEFT JOIN users u ON b.fk_user = u.user_id

                    WHERE (bp.fk_batch = :bid or :bid is null) and
                       (bp.step = :sid or :sid is null) and
                       (p.fk_process_type = :ptid or :ptid is null) and
                       (bp.batch_process_id = :bpid or :bpid is null)
                       ORDER BY bp.process_order_in_step;";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@sid", stepId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@ptid", processTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bpid", batchProcessId, NpgsqlDbType.Bigint);

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

            List<BatchProcessExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        //public static List<BatchContentExt> GetAllProcessesInBatchRecursive(List<BatchProcessExt> list, int batchId)
        //{
        //    List<BatchProcessExt> finalList = new List<BatchProcessExt>();
        //    List<BatchProcessExt> contentList = GetAllBatchProcesses(batchId);
        //    list.AddRange(contentList);

        //    foreach (BatchContentExt e in contentList)
        //    {
        //        if (e.fkStepBatch != null)
        //        {
        //            GetAllProcessesInBatchRecursive(list, (int)e.fkStepBatch);
        //        }
        //    }

        //    return list;
        //}

        public static int AddBatchProcess(BatchProcess batchProcess)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"INSERT INTO public.batch_process (fk_batch, step, fk_process_type, process_order_in_step)
                    VALUES (:bid, :step, :ptid, :pois);";

                Db.CreateParameterFunc(cmd, "@bid", batchProcess.fkBatch, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@step", batchProcess.step, NpgsqlDbType.Integer);
               // Db.CreateParameterFunc(cmd, "@ptid", batchProcess.fkProcessType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pois", batchProcess.processOrderInStep, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting batch process", ex);
            }

            return 0;
        }
        public static int UpdateBatchProcess(BatchProcess batchProcess)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.batch_process
                        SET fk_batch=:bid, step=:step, fk_process_type=:ptid, process_order_in_step=:pois
                        WHERE batch_process_id=:bpid;";
                Db.CreateParameterFunc(cmd, "@bid", batchProcess.fkBatch, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@step", batchProcess.step, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@ptid", batchProcess.fkProcessType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pois", batchProcess.processOrderInStep, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bpid", batchProcess.batchProcessId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating batch process info", ex);
            }
            return 0;
        }
        //dali da brise po batch process id..
        public static int DeleteBatchProcess(long batchProcessId)
        {
            //DataTable dt;
            try
            {
                var cmd = Db.CreateCommand();

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.batch_process
                                WHERE batch_process_id=:bpid;";

                Db.CreateParameterFunc(cmd, "@bpid", batchProcessId, NpgsqlDbType.Bigint);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        public static BatchProcess CreateObject(DataRow dr)
        {
            var batchProcess = new BatchProcess
            {
                batchProcessId = (long)dr["batch_process_id"],
                fkBatch = dr["fk_batch"] != DBNull.Value ? int.Parse(dr["fk_batch"].ToString()) : (int?)null,
                step = dr["step"] != DBNull.Value ? int.Parse(dr["step"].ToString()) : (int?)null,
                //fkProcessType = dr["fk_process_type"] != DBNull.Value ? int.Parse(dr["fk_process_type"].ToString()) : (int?)null,
                processOrderInStep = dr["process_order_in_step"] != DBNull.Value ? int.Parse(dr["process_order_in_step"].ToString()) : (int?)null,
                fkProcess = dr["fk_process"] != DBNull.Value ? int.Parse(dr["fk_process"].ToString()) : (int?)null,
                label = dr["label"].ToString()
            };
            return batchProcess;
        }
        private static BatchProcessExt CreateObjectExt(DataRow dr)
        {
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
            var batchProcess = CreateObject(dr);

            var batchProcessExt = new BatchProcessExt(batchProcess)
            {
                operatorUsername = dr["username"].ToString(),
                processType = dr["process_type"].ToString(),
                processDatabaseType = dr["process_database_type"].ToString(),
                fkProcessType=fkProcessTeype,
                subcategory=subcategory,
                fkEquipment=fkEquipment,
                equipmentName=equipmentName,
                equipmentModelName=equipmentModelName,
                equipmentModelId=equipmentModelId,
                modelBrand=modelBrand
            };
            return batchProcessExt;
        }
    }
}