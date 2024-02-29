using Batteries.Dal.Base;
using Batteries.Dal.EquipmentDal;
using Batteries.Dal.ProcessesDal;
using Batteries.Helpers;
using Batteries.Models;
using Batteries.Models.EquipmentModels;
using Batteries.Models.ProcessModels;
using Batteries.Models.Requests;
using Batteries.Models.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Batteries.Dal
{
    public class BatchDa
    {
        /// <summary>
        /// Get batches that researchGroupId is meant to see
        /// </summary>
        /// <param name="researchGroupId"></param>
        /// <param name="batchId"></param>
        /// <param name="materialTypeId"></param>
        /// <returns></returns>
        public static List<BatchExt> GetAllCompleteBatchesWithQuantity(int researchGroupId, int? batchId = null, int? materialTypeId = null)
        {
            DataTable dt;

            //if (researchGroupId == null)
            //{
            //    researchGroupId = 1;
            //}
            //every researchGroupId id has different stock value

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                cmd.CommandText =
                    @"SELECT DISTINCT (b.batch_id), b.*, u.fk_research_group, u.username as operator_username, edu.username as editing_operator_username,
                    rg.*, mt.*, mu.*, rg.*,
					(
                        SELECT SUM(stock_transaction.amount * stock_transaction.transaction_direction)
                        FROM stock_transaction
                        WHERE stock_transaction.fk_batch = b.batch_id AND
                          stock_transaction.fk_research_group = :rgid
                      ) as available_quantity
                    FROM project_batch pb
                    LEFT JOIN batch b ON b.batch_id = pb.fk_batch
                    LEFT JOIN users u on b.fk_user = u.user_id
                    LEFT JOIN research_group rg ON u.fk_research_group = rg.research_group_id
                    LEFT JOIN users edu on b.fk_edited_by = edu.user_id
                    LEFT JOIN material_type mt on b.fk_material_type = mt.material_type_id
                    LEFT JOIN measurement_unit mu on b.fk_measurement_unit = mu.measurement_unit_id
						
                    WHERE pb.fk_project IN (
	                    SELECT fk_project 
	                    FROM project_research_group prg 
	                    WHERE prg.fk_research_group = :rgid
                    )
                    AND (b.batch_id = :bid or :bid is null)
                    AND (b.is_complete = true)
                    ORDER BY b.date_created DESC
                    ;";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@mtid", materialTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);

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

            List<BatchExt> list = (from DataRow dr in dt.Rows select CreateBatchObjectExt(dr)).ToList();

            return list;
        }

        public static List<BatchExt> GetCompleteBatchesWithQuantityNoRG(int? researchGroupId = null, int? batchId = null, int? materialTypeId = null)
        {
            //ANY RESEARCH GROUP
            DataTable dt;

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                cmd.CommandText =
                    @"SELECT *,
                      u.username as operator_username, edu.username as editing_operator_username,
                      (
                        SELECT SUM(stock_transaction.amount * stock_transaction.transaction_direction)
                        FROM stock_transaction
                        WHERE stock_transaction.fk_batch = b.batch_id AND
                          stock_transaction.fk_research_group = :rgid
                      ) as available_quantity

                    FROM batch b
                        left join users u on b.fk_user = u.user_id
left join users edu on b.fk_edited_by = edu.user_id
                        left join material_type mt on b.fk_material_type = mt.material_type_id
                        left join measurement_unit mu on b.fk_measurement_unit = mu.measurement_unit_id

                    WHERE (b.batch_id = :bid or :bid is null) and
(b.is_complete = true)
            ORDER BY b.date_created DESC
        ;";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@mtid", materialTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);

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

            List<BatchExt> list = (from DataRow dr in dt.Rows select CreateBatchObjectExt(dr)).ToList();

            return list;
        }
        public static List<BatchExt> GetBatchesFromOtherRG(int? myResearchGroupId = null, int? otherResearchGroupId = null, int? batchId = null, int? materialTypeId = null)
        {
            //Complete Batches
            //NO Quantity

            DataTable dt;

            //if (researchGroupId == null)
            //{
            //    researchGroupId = 1;
            //}
            //mora sekogas da se prati regid (pri povik) za da se dade tocna presmetka

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                cmd.CommandText =
                    @"SELECT *,
                      u.username as operator_username, edu.username as editing_operator_username                      

                    FROM batch b
                        left join users u on b.fk_user = u.user_id
left join users edu on b.fk_edited_by = edu.user_id
                        left join material_type mt on b.fk_material_type = mt.material_type_id
                        left join measurement_unit mu on b.fk_measurement_unit = mu.measurement_unit_id

                    WHERE (b.batch_id = :bid or :bid is null) and
(b.is_complete = true) and
                        (u.fk_research_group != :myrgid) and
                        (u.fk_research_group = :orgid or :orgid is null)
            ORDER BY b.date_created DESC
        ;";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@mtid", materialTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@myrgid", myResearchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@orgid", otherResearchGroupId, NpgsqlDbType.Integer);

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

            List<BatchExt> list = (from DataRow dr in dt.Rows select CreateBatchObjectExt(dr)).ToList();

            return list;
        }

        public static List<BatchExt> GetBatchesFromOtherRGWithQuantity(int? myResearchGroupId = null, int? otherResearchGroupId = null, int? batchId = null, int? materialTypeId = null)
        {
            //Complete Batches
            //With Quantity

            DataTable dt;

            //if (researchGroupId == null)
            //{
            //    researchGroupId = 1;
            //}
            //mora sekogas da se prati regid (pri povik) za da se dade tocna presmetka

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                cmd.CommandText =
                    @"SELECT *,
                      u.username as operator_username, edu.username as editing_operator_username,
                      (
                        SELECT SUM(stock_transaction.amount * stock_transaction.transaction_direction)
                        FROM stock_transaction
                        WHERE stock_transaction.fk_batch = b.batch_id AND
                          stock_transaction.fk_research_group = :myrgid
                      ) as available_quantity

                    FROM batch b
                        left join users u on b.fk_user = u.user_id
left join users edu on b.fk_edited_by = edu.user_id
                        left join material_type mt on b.fk_material_type = mt.material_type_id
                        left join measurement_unit mu on b.fk_measurement_unit = mu.measurement_unit_id

                    WHERE (b.batch_id = :bid or :bid is null) and
(b.is_complete = true) and
                        (u.fk_research_group != :myrgid) and
                        (u.fk_research_group = :orgid or :orgid is null)
            ORDER BY b.date_created DESC
        ;";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@mtid", materialTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@myrgid", myResearchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@orgid", otherResearchGroupId, NpgsqlDbType.Integer);

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

            List<BatchExt> list = (from DataRow dr in dt.Rows select CreateBatchObjectExt(dr)).ToList();

            return list;
        }

        public static List<BatchExt> GetAllCompleteBatches(int? researchGroupCreatorId = null, int? batchId = null, int? materialTypeId = null)
        {
            //WITHOUT QUANTITY
            DataTable dt;

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"SELECT *, u.username as operator_username
                    
                        FROM batch b
                        left join users u on b.fk_user = u.user_id
                        left join research_group rg on u.fk_research_group = rg.research_group_id
                        left join material_type mt on b.fk_material_type = mt.material_type_id
                        left join measurement_unit mu on b.fk_measurement_unit = mu.measurement_unit_id

                    WHERE (b.batch_id = :bid or :bid is null) and
(b.is_complete = true) and
                        (u.fk_research_group = :rgid or :rgid is null)
            ORDER BY b.date_created DESC;";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@mtid", materialTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupCreatorId, NpgsqlDbType.Integer);

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

            List<BatchExt> list = (from DataRow dr in dt.Rows select CreateBatchObjectExt(dr)).ToList();

            return list;
        }

        public static List<BatchExt> GetAllBatchesGeneralData(int? batchId = null, int? researchGroupId = null)
        {
            //DOES NOT MATTER IF COMPLETE
            DataTable dt;

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"SELECT *, u.username as operator_username
                      
                    FROM batch
                        left join users u on batch.fk_user = u.user_id
                        left join research_group rg on u.fk_research_group = rg.research_group_id
                        left join material_type mt on batch.fk_material_type = mt.material_type_id
                        left join measurement_unit mu on batch.fk_measurement_unit = mu.measurement_unit_id

                    WHERE (batch.batch_id = :bid) and
                            (u.fk_research_group = :rgid or :rgid is null)
                    ORDER BY batch.date_created DESC;";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);

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

            List<BatchExt> list = (from DataRow dr in dt.Rows select CreateBatchObjectExt(dr)).ToList();

            return list;
        }

        public static BatchExt GetBatchById(int batchId, int? researchGroupId = null)
        {
            //COMPLETE
            DataTable dt;

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"SELECT *, u.username as operator_username
                      
                    FROM batch
                        LEFT JOIN users u on batch.fk_user = u.user_id
                        LEFT JOIN research_group rg on u.fk_research_group = rg.research_group_id
                        LEFT JOIN material_type mt on batch.fk_material_type = mt.material_type_id
                        LEFT JOIN measurement_unit mu on batch.fk_measurement_unit = mu.measurement_unit_id

                    WHERE   (batch.batch_id = :bid) AND
                            batch.is_complete = true AND
                            (u.fk_research_group = :rgid or :rgid is null)
                    ORDER BY batch.date_created DESC;";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);

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

            BatchExt item = CreateBatchObjectExt(dt.Rows[0]);

            return item;
        }

        public static List<BatchExt> GetUnfinishedBatches(int? researchGroupId = null, int? batchId = null, int? materialTypeId = null)
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
                    @"SELECT *, u.username as operator_username, edu.username as editing_operator_username                    
                        FROM batch b
                        left join users u on b.fk_user = u.user_id
left join users edu on b.fk_edited_by = edu.user_id
                        left join research_group rg on u.fk_research_group = rg.research_group_id
                        left join material_type mt on b.fk_material_type = mt.material_type_id
                        left join measurement_unit mu on b.fk_measurement_unit = mu.measurement_unit_id

                    WHERE (b.batch_id = :bid or :bid is null) and
(b.is_complete = false) and
                        (u.fk_research_group = :rgid or :rgid is null)
            ORDER BY b.date_created DESC;";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@mtid", materialTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);

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

            List<BatchExt> list = (from DataRow dr in dt.Rows select CreateBatchObjectExt(dr)).ToList();

            return list;
        }


        public static BatchExt GetBatchWithContent(int batchId, int researchGroupId)
        {
            DataTable dtBatch;
            DataTable dtBatchContents;
            DataTable dtBatchProcesses;
            //int returnedBatchId = 0;

            var cmd = Db.CreateCommand();
            try
            {
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                cmd.CommandText =
                @"SELECT *, u.username as operator_username,
                      (
                        SELECT SUM(stock_transaction.amount * stock_transaction.transaction_direction)
                        FROM stock_transaction
                        WHERE stock_transaction.fk_batch = batch.batch_id AND
                          stock_transaction.fk_research_group = :rgid
                      ) as available_quantity
                    FROM batch
                        left join users u on batch.fk_user = u.user_id
                        left join research_group rg on u.fk_research_group = rg.research_group_id
                        left join material_type mt on batch.fk_material_type = mt.material_type_id
                        left join measurement_unit mu on batch.fk_measurement_unit = mu.measurement_unit_id

                    WHERE (batch.batch_id = :bid);";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);

                dtBatch = Db.ExecuteSelectCommand(cmd);

                if (dtBatch == null || dtBatch.Rows.Count == 0)
                {
                    return null;
                }

                //GET BATCH CONTENT
                if (dtBatch.Rows.Count > 0)
                {
                    //DataRow batchRow = dtBatch.Rows[0];
                    //returnedBatchId = (int)batchRow["batch_id"];
                    //(int)dr["batch_id"]

                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.Parameters.Clear();

                    cmd.CommandText =
                        @"SELECT *, mitm.measurement_unit_name as material_measurement_unit_name,
                        mitm.measurement_unit_symbol as material_measurement_unit_symbol,
                        mitb.measurement_unit_name as batch_measurement_unit_name,
                        mitb.measurement_unit_symbol as batch_measurement_unit_symbol,
                        m.chemical_formula as material_chemical_formula,
                        bat.chemical_formula as batch_chemical_formula

                    FROM batch_content bc
                        left join material m on bc.fk_step_material = m.material_id
                        left join batch bat on bc.fk_step_batch = bat.batch_id
                        left join material_function mf on bc.fk_function = mf.material_function_id
                        
                        left join measurement_unit mitm on m.fk_measurement_unit = mitm.measurement_unit_id
                        left join measurement_unit mitb on bat.fk_measurement_unit = mitb.measurement_unit_id

                    WHERE (bc.fk_batch = :bid or :bid is null)
                    ORDER BY bc.step, bc.order_in_step
                    ;";

                    Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);

                    dtBatchContents = Db.ExecuteSelectCommand(cmd);

                    //ako ima poveke od 0 redovi contents, togas barame i processes
                    if (dtBatchContents.Rows.Count > 0)
                    {
                        if (cmd.Connection.State != ConnectionState.Open)
                        {
                            cmd.Connection.Open();
                        }
                        cmd.Parameters.Clear();

                        cmd.CommandText =
                    @"SELECT *
                    FROM batch_process bp
                          left join process_type pt on bp.fk_process_type = pt.process_type_id
                    WHERE (bp.fk_batch = :bid)
                    ORDER BY bp.step, bp.process_order_in_step;";

                        Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);

                        dtBatchProcesses = Db.ExecuteSelectCommand(cmd);
                    }
                    else
                    {
                        dtBatchProcesses = null;
                    }
                }
                else
                {
                    dtBatchContents = null;
                    dtBatchProcesses = null;
                }

                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            //List<BatchExt> list = (from DataRow dr in dt.Rows select CreateBatchObjectExt(dr)).ToList();
            DataRow batchRow = dtBatch.Rows[0];
            //returnedBatchId = (int)batchRow["batch_id"];
            BatchExt batchFull = CreateBatchObjectExt(batchRow);

            List<BatchContentExt> contentsList = (from DataRow dr in dtBatchContents.Rows select CreateBatchContentObjectExt(dr)).ToList();
            batchFull.batchContentList = contentsList;
            List<BatchProcessExt> processesList = (from DataRow dr in dtBatchProcesses.Rows select CreateBatchProcessObjectExt(dr)).ToList();
            batchFull.batchProcessList = processesList;

            return batchFull;
        }

        public static List<BatchExt> GetBatchesByName(string search, int researchGroupId, int? page = 1)
        {
            DataTable dt;

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                //                                batch_output
                //fk_measurement_unit
                //chemical_formula
                cmd.CommandText =
                    @"SELECT *, u.username as operator_username
                    FROM batch
                        left join users u on batch.fk_user = u.user_id
                        left join research_group rg on u.fk_research_group = rg.research_group_id
                        left join material_type mt on batch.fk_material_type = mt.material_type_id
                        left join measurement_unit mu on batch.fk_measurement_unit = mu.measurement_unit_id

                    WHERE 
((lower(batch.batch_personal_label) LIKE lower('%'|| :search ||'%') or lower(batch.batch_system_label) LIKE lower('%'|| :search ||'%')) or :search is null) and
(batch.is_complete = true) and
(u.fk_research_group = :rgid or :rgid is null)
ORDER BY batch.date_created DESC
    LIMIT 10 OFFSET :offset
                        ;";

                //Db.CreateParameterFunc(cmd, "@mid", materialId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
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

            List<BatchExt> list = (from DataRow dr in dt.Rows select CreateBatchObjectExt(dr)).ToList();

            return list;
        }
        public static List<BatchExt> GetBatchesOutsideProject(string search, int researchGroupId, int projectId, int? page = 1)
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
                    @"
                    SELECT batch.*, u.username as operator_username, rg.*, mt.*, mu.*
                    FROM batch
                        LEFT JOIN users u on batch.fk_user = u.user_id
                        LEFT JOIN research_group rg on u.fk_research_group = rg.research_group_id
                        LEFT JOIN material_type mt on batch.fk_material_type = mt.material_type_id
                        LEFT JOIN measurement_unit mu on batch.fk_measurement_unit = mu.measurement_unit_id
                    WHERE 
                    ((lower(batch.batch_personal_label) LIKE lower('%'|| :search ||'%') or lower(batch.batch_system_label) LIKE lower('%'|| :search ||'%')) or :search is null) AND
                    (batch.is_complete = true) AND
                    (u.fk_research_group = :rgid) AND
                    batch.batch_id NOT IN (
	                    SELECT DISTINCT(fk_batch)
	                    FROM project_batch pb
	                    WHERE pb.fk_project = :pid
                    )
                    ;";

                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pid", projectId, NpgsqlDbType.Integer);
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

            List<BatchExt> list = (from DataRow dr in dt.Rows select CreateBatchObjectExt(dr)).ToList();

            return list;
        }

        public static List<Batch> GetBatchForMeasurementsDropdown(string search = null, int? page = 1, int? researchGroupId = null)
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
                      FROM batch b
                      LEFT JOIN users u ON b.fk_user = u.user_id
                      WHERE (b.is_complete = true) AND
                        (u.fk_research_group = :rgid or :rgid is null) AND
                        ((lower(b.batch_personal_label) LIKE lower('%'|| :search ||'%') or lower(b.batch_system_label) LIKE lower('%'|| :search ||'%')) or :search is null)
                        
                        ORDER BY batch_id DESC LIMIT 10 OFFSET :offset;
                        ";

                // Db.CreateParameterFunc(cmd, "@tgid", projectId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
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

            List<Batch> list = (from DataRow dr in dt.Rows select CreateBatchObject(dr)).ToList();

            return list;
        }
        public static List<BatchExt> GetAllCompleteBatchGeneralData(int? batchId = null, int? researchGroupId = null)
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
                    @"SELECT *, u.username as operator_username
                              FROM batch b
                              LEFT JOIN users u ON b.fk_user = u.user_id
                              LEFT JOIN measurement_unit m ON m.measurement_unit_id=b.fk_measurement_unit
							  LEFT JOIN material_type mt ON mt.material_type_id=b.fk_material_type
                              WHERE 
                                    (b.is_complete = true) and
                                    (b.batch_id = :eid or :eid is null);";

                //Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@eid", batchId, NpgsqlDbType.Integer);

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

            List<BatchExt> list = (from DataRow dr in dt.Rows select CreateBatchObjectExt(dr)).ToList();

            return list;
        }

        /// <summary>
        /// Get a list of batches that are shared with researchGroupId
        /// </summary>
        /// <param name="currentRGId"></param>
        /// <param name="researchGroupIdCreator"></param>
        /// <param name="userId"></param>
        /// <param name="projectId"></param>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static List<DropdownItem> GetBatchesForCharts(int currentRGId, int? researchGroupIdCreator = null, int? userId = null, int? projectId = null, int? testTypeId = null, string search = null, int? page = 1)
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
                    @"SELECT DISTINCT(pb.fk_batch) as batch_id, b.batch_system_label, b.batch_personal_label
                      FROM project_batch pb
                      LEFT JOIN batch b ON pb.fk_batch=b.batch_id
                      LEFT JOIN users u ON b.fk_user = u.user_id
                      LEFT JOIN project_research_group prg ON prg.fk_project = pb.fk_project 
                      LEFT JOIN test t on b.batch_id = t.fk_batch
                      WHERE  (pb.fk_project = :pid or :pid is null) AND
                        (b.is_complete = true) AND
                        (b.has_test_results_doc = true) AND
                        (prg.fk_research_group= :rgidcurrent) AND
                        (b.fk_research_group = :rgidCreator or :rgidCreator is null) AND
                        (b.fk_user = :uidCreator or :uidCreator is null) AND
                        (t.fk_test_type = :ttype or :ttype is null) AND
                        ((lower(b.batch_personal_label) LIKE lower('%'|| :search ||'%') or lower(b.batch_system_label) LIKE lower('%'|| :search ||'%')) or :search is null)
                    
                        ORDER BY batch_id DESC LIMIT 10 OFFSET :offset;
                        ";

                Db.CreateParameterFunc(cmd, "@rgidcurrent", currentRGId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pid", projectId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgidCreator", researchGroupIdCreator, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@uidCreator", userId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@ttype", testTypeId, NpgsqlDbType.Integer);

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

            List<DropdownItem> list = (from DataRow dr in dt.Rows select CreateDropdownObject(dr)).ToList();

            return list;
        }
        public static bool HasViewPermission(int batchId, int researchGroupId)
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
                    @"SELECT DISTINCT fk_batch 
                        FROM project_batch prg
                        WHERE prg.fk_batch = :bid
                        AND prg.fk_project IN (
	                        SELECT fk_project 
	                        FROM project_research_group
	                        WHERE fk_research_group = :rgid 
                        );";

                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            if (dt == null || dt.Rows.Count == 0)
            {
                return false;
            }
            return true;
        }

        //NOT USED, NOT UPDATED
        public static int AddBatchWithContent(AddBatchRequest req, int researchGroupId)
        {
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandType = CommandType.Text;

            NpgsqlTransaction t = cmd.Connection.BeginTransaction();
            int res = 0;
            int returnedBatchID = 0;
            int returnedBatchProcessID = 0;
            int operatorId = 0;
            double batchOutputWeight = 0;
            //bool weightsInputOK = true;
            bool materialQuantityOk = true;
            bool batchQuantityOk = true;

            //bool atLeastOneMaterialOrBatch = false;
            //bool atLeastOneProcess = true;

            string notEnoughMessage = "Not enough ";
            //req.batchInfo.suggestedLabel == null
            try
            {
                if (req.batchInfo.batchOutput == null ||
                    req.batchInfo.fkMeasurementUnit == null || req.batchInfo.chemicalFormula == null || req.batchInfo.fkMaterialType == null ||
                    req.batchInfo.description == null
                    )
                {
                    throw new Exception("Please enter all general batch data");
                }
                operatorId = (int)req.batchInfo.fkUser;
                batchOutputWeight = (double)req.batchInfo.batchOutput;

                List<BatchContentExt> batchContentList = req.batchContent;
                if (batchContentList.Count <= 0)
                {
                    throw new Exception("At least one material or batch needs to be chosen");
                }

                //List<dynamic> batchProcessList = req.batchProcesses;
                List<BatchProcessResponse> batchProcessList = req.batchProcesses;
                if (batchProcessList.Count <= 0)
                {
                    throw new Exception("At least one process needs to be chosen");
                }

                List<string> materials = new List<string>();
                List<string> batches = new List<string>();

                foreach (BatchContentExt batchContent in batchContentList)
                {
                    double wantedQuantity = (double)batchContent.weight;
                    if (batchContent.fkStepMaterial != null)
                    {
                        int materialId = (int)batchContent.fkStepMaterial;
                        bool result = StockTransactionDa.CheckMaterialStockQuantityEnough(materialId, wantedQuantity, researchGroupId, cmd);

                        if (result == false)
                        {
                            materialQuantityOk = false;
                            //notEnoughMessage += batchContent.materialName + "in stock";
                            materials.Add(batchContent.materialName);
                            //throw new Exception("Not enough");
                            //break;
                        }
                    }
                    else
                    {
                        //ako e batch
                        int batchId = (int)batchContent.fkStepBatch;
                        //throw new Exception("batch empty weigh " + batchContent.weight);
                        bool result = StockTransactionDa.CheckBatchStockQuantityEnough(batchId, wantedQuantity, researchGroupId, cmd);
                        //bool result = true;
                        if (result == false)
                        {
                            batchQuantityOk = false;
                            //notEnoughMessage += batchContent.materialName + "in stock";
                            batches.Add(batchContent.batchSystemLabel);
                            //throw new Exception("Not enough");
                            //break;
                        }
                    }
                }

                if (materialQuantityOk == false)
                {
                    string materialsString = "";
                    foreach (string m in materials)
                    {
                        if (materials.IndexOf(m) == materials.Count - 1)
                        {
                            materialsString += m;
                        }
                        else
                        {
                            materialsString += m + ", ";
                        }
                    }
                    //return 0;
                    notEnoughMessage += "Materials: " + materialsString;
                }
                if (batchQuantityOk == false)
                {
                    string batchesString = "";
                    foreach (string b in batches)
                    {
                        if (batches.IndexOf(b) == batches.Count - 1)
                        {
                            batchesString += b;
                        }
                        else
                        {
                            batchesString += b + ", ";
                        }
                    }
                    notEnoughMessage += " Batches: " + batchesString;
                }
                if (!materialQuantityOk || !batchQuantityOk)
                {
                    notEnoughMessage += " in stock!";
                    throw new Exception(notEnoughMessage);
                }


                //if (batchContent.weight != null)
                //{
                //    //throw new Exception("batch weigh ");
                //    //throw new Exception("batch weigh " + batchContent.weight);
                //    //double wantedQuantity = (double)batchContent.weight;
                //    //if (batchContent.fkStepMaterial != null)
                //    //{
                //    //int materialId = (int)batchContent.fkStepMaterial;
                //    //bool result = StockTransactionDa.CheckMaterialStockQuantityEnough(materialId, wantedQuantity, researchGroupId);
                //    //    if (result == false)
                //    //    {
                //    //        materialQuantityOk = false;
                //    //        //notEnoughMessage += batchContent.materialName + "in stock";
                //    //        materials.Add(batchContent.materialName);
                //    //        //throw new Exception("Not enough");
                //    //        //break;
                //    //    }
                //    //}
                //    //else
                //    //{
                //    //    //ako e batch
                //    //    int batchId = (int)batchContent.fkStepBatch;
                //    //    throw new Exception("batch empty weigh " + batchContent.weight);
                //    //}
                //}

                //else
                //{
                //    throw new Exception("Invalid number for " + batchContent.batchLabel + " weight");
                //}

            }
            catch (ValidationException ve)
            {
                //do whatever
                throw new Exception(ve.Message);
            }


            try
            {
                //INSERT BATCH
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"INSERT INTO public.batch (batch_personal_label, fk_user, batch_output, fk_measurement_unit, chemical_formula, fk_material_type, description, date_created)
                    VALUES (:plabel, :uid, :bout, :muid, :cf, :mtid, :desc, now()::timestamp)
                    RETURNING batch_id;";

                Db.CreateParameterFunc(cmd, "@plabel", req.batchInfo.batchPersonalLabel, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@sl", req.batchInfo.suggestedLabel, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@uid", req.batchInfo.fkUser, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bout", req.batchInfo.batchOutput, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@muid", req.batchInfo.fkMeasurementUnit, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@cf", req.batchInfo.chemicalFormula, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@mtid", req.batchInfo.fkMaterialType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@desc", req.batchInfo.description, NpgsqlDbType.Text);

                //res = Db.ExecuteNonQuery(cmd, false);
                returnedBatchID = int.Parse(Db.ExecuteScalar(cmd, false));

                if (returnedBatchID <= 0)
                {
                    t.Rollback();
                    //return 5;
                    throw new Exception("Error inserting batch");
                }

                string systemLabel = "BTC_" + returnedBatchID;
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.batch
                        SET batch_system_label=:slabel
                        WHERE batch_id=:bid;";

                Db.CreateParameterFunc(cmd, "@bid", returnedBatchID, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@slabel", systemLabel, NpgsqlDbType.Text);
                res = Db.ExecuteNonQuery(cmd, false);
                if (res <= 0)
                {
                    t.Rollback();
                    throw new Exception("Error updating batch system label info");
                }

                //INSERT NA BATCH CONTENTS
                foreach (BatchContent batchContent in req.batchContent)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText =
                    @"INSERT INTO public.batch_content (fk_batch, step, fk_step_material, fk_step_batch, weight, fk_function, order_in_step)
                    VALUES (:bid, :step, :sm, :sb, :w, :f, :ois);";

                    Db.CreateParameterFunc(cmd, "@bid", returnedBatchID, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@step", batchContent.step, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@sm", batchContent.fkStepMaterial, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@sb", batchContent.fkStepBatch, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@w", batchContent.weight, NpgsqlDbType.Double);
                    Db.CreateParameterFunc(cmd, "@f", batchContent.fkFunction, NpgsqlDbType.Integer);
                    //Db.CreateParameterFunc(cmd, "@sit", batchContent.fkStoredInType, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@ois", batchContent.orderInStep, NpgsqlDbType.Integer);

                    res = Db.ExecuteNonQuery(cmd, false);
                    if (res <= 0)
                    {
                        t.Rollback();
                        throw new Exception("Error inserting batch content");
                        //return 5;
                    }


                    //Odzemi od stock za sekoj materijal/batch
                    cmd.Parameters.Clear();
                    if (batchContent.fkStepMaterial != null)
                    {
                        cmd.CommandText =
                        @"INSERT INTO public.stock_transaction (fk_material, amount, fk_operator, fk_research_group, fk_experiment, transaction_direction, stock_transaction_element_type)
                    VALUES (:mid, :amount, :oid, :rgid, :eid, -1, 1);";

                        Db.CreateParameterFunc(cmd, "@mid", batchContent.fkStepMaterial, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@amount", batchContent.weight, NpgsqlDbType.Double);
                        Db.CreateParameterFunc(cmd, "@oid", operatorId, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@eid", null, NpgsqlDbType.Double);
                        //Db.CreateParameterFunc(cmd, "@stet", batchContent.fkFunction, NpgsqlDbType.Integer);

                        res = Db.ExecuteNonQuery(cmd, false);
                        if (res <= 0)
                        {
                            t.Rollback();
                            throw new Exception("Error inserting material stock info");
                        }
                    }
                    else if (batchContent.fkStepBatch != null)
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText =
                        @"INSERT INTO public.stock_transaction (fk_batch, amount, fk_operator, fk_research_group, fk_experiment, transaction_direction, stock_transaction_element_type)
                    VALUES (:bid, :amount, :oid, :rgid, :eid, -1, 2);";

                        Db.CreateParameterFunc(cmd, "@bid", batchContent.fkStepBatch, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@amount", batchContent.weight, NpgsqlDbType.Double);
                        Db.CreateParameterFunc(cmd, "@oid", operatorId, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@eid", null, NpgsqlDbType.Double);
                        //Db.CreateParameterFunc(cmd, "@bid", returnedBatchID, NpgsqlDbType.Integer);

                        res = Db.ExecuteNonQuery(cmd, false);
                        if (res <= 0)
                        {
                            t.Rollback();
                            throw new Exception("Error inserting batch stock info");
                        }
                    }
                }

                //INSERT NA BATCH PROCESSES
                //foreach (BatchProcess batchProcess in req.batchProcesses)
                //List<dynamic> dynDataList = JsonConvert.DeserializeObject(req.batchProcesses);
                object submittedItemBatchProcess;
                object submittedItemProcessAttributes;
                foreach (dynamic batchProcessData in req.batchProcesses)
                {
                    //batchProcessList
                    //Milling processAttributes = (Milling)batchProcessData.processAttributes;
                    string processType = batchProcessData.processAttributes.processType;
                    string batchProcessDataString = Convert.ToString(batchProcessData);
                    string batchProcessAttributesString = Convert.ToString(batchProcessData.processAttributes);

                    submittedItemBatchProcess = JsonConvert.DeserializeObject<BatchProcess>(batchProcessDataString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                    BatchProcess batchProcess = (BatchProcess)submittedItemBatchProcess;
                    batchProcess.fkBatch = returnedBatchID;
                    var validationResult = ValidationHelper.IsModelValidWithErrors(batchProcess);
                    if (validationResult.Count != 0)
                    {
                        throw new Exception(validationResult[0].ErrorMessage);
                    }
                    //return 0;
                    //BatchProcess batchProcess = (BatchProcess)batchProcessData;

                    cmd.Parameters.Clear();
                    cmd.CommandText =
                    @"INSERT INTO public.batch_process (fk_batch, step, fk_process, process_order_in_step)
                    VALUES (:bid, :step, :ptid, :pois)
                    RETURNING batch_process_id;";

                    Db.CreateParameterFunc(cmd, "@bid", returnedBatchID, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@step", batchProcess.step, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@ptid", batchProcess.fkProcess, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@pois", batchProcess.processOrderInStep, NpgsqlDbType.Integer);

                    //res = Db.ExecuteNonQuery(cmd, false);
                    returnedBatchProcessID = int.Parse(Db.ExecuteScalar(cmd, false));
                    if (returnedBatchProcessID <= 0)
                    {
                        t.Rollback();
                        throw new Exception("Error inserting batch process");
                    }

                    var processValidationResult = new List<ValidationResult>();
                    switch (processType)
                    {
                        case "Milling":
                            submittedItemProcessAttributes = JsonConvert.DeserializeObject<Milling>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                            Milling milling = (Milling)submittedItemProcessAttributes;
                            milling.fkBatchProcess = returnedBatchProcessID;
                            //milling.fkEquipment = null;
                            //setiraj fkEquipment, fkBatchProcess
                            processValidationResult = ValidationHelper.IsModelValidWithErrors(milling);
                            if (processValidationResult.Count != 0)
                            {
                                throw new Exception(processValidationResult[0].ErrorMessage);
                            }

                            MillingDa.AddMilling(milling, cmd);
                            break;

                        case "Heating":
                            submittedItemProcessAttributes = JsonConvert.DeserializeObject<Heating>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                            Heating heating = (Heating)submittedItemProcessAttributes;
                            heating.fkBatchProcess = returnedBatchProcessID;
                            processValidationResult = ValidationHelper.IsModelValidWithErrors(heating);
                            if (processValidationResult.Count != 0)
                            {
                                throw new Exception(processValidationResult[0].ErrorMessage);
                            }

                            HeatingDa.AddHeating(heating, cmd);
                            break;

                        default:
                            throw new Exception("Process type not defined");
                    }



                    //dynamic processRequest = batchProcess;
                    //MillingDa.AddMilling(processRequest);


                    //                    cmd.Parameters.Clear();
                    //                    cmd.CommandText =
                    //                    @"INSERT INTO public.batch_process (fk_batch, step, fk_process_type, process_order_in_step)
                    //                    VALUES (:bid, :step, :ptid, :pois);";

                    //                    Db.CreateParameterFunc(cmd, "@bid", returnedBatchID, NpgsqlDbType.Integer);
                    //                    Db.CreateParameterFunc(cmd, "@step", batchProcess.step, NpgsqlDbType.Integer);
                    //                    Db.CreateParameterFunc(cmd, "@ptid", batchProcess.fkProcessType, NpgsqlDbType.Integer);
                    //                    Db.CreateParameterFunc(cmd, "@pois", batchProcess.processOrderInStep, NpgsqlDbType.Integer);

                    //                    res = Db.ExecuteNonQuery(cmd, false);
                    //                    if (res <= 0)
                    //                    {
                    //                        t.Rollback();
                    //                        throw new Exception("Error inserting batch process");
                    //                    }
                }

                //Na kraj dodaj izlezna tezina na batch
                cmd.Parameters.Clear();
                cmd.CommandText =
                        @"INSERT INTO public.stock_transaction (fk_batch, amount, fk_operator, fk_research_group, fk_experiment, transaction_direction, stock_transaction_element_type)
                    VALUES (:bid, :amount, :oid, :rgid, :eid, 1, 2);";

                Db.CreateParameterFunc(cmd, "@bid", returnedBatchID, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@amount", batchOutputWeight, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@oid", operatorId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@eid", null, NpgsqlDbType.Double); //ova ako e napraven pri pravenje eksperiment..dali bas treba..
                //Db.CreateParameterFunc(cmd, "@bid", returnedBatchID, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd, false);
                if (res <= 0)
                {
                    t.Rollback();
                    throw new Exception("Error inserting batch stock info");
                }


                t.Commit();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                t.Rollback();
                throw new Exception(ex.Message);
            }
            //return 0;
            return returnedBatchID;
        }

        public static int RemoveBatchContent(int batchId, int researchGroupId, NpgsqlCommand cmd)
        {
            int res = 0;

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
            try
            {
                //REMOVE BATCH LEVEL MEASUREMENTS
                cmd.Parameters.Clear();
                cmd.CommandText =
                        @"DELETE FROM public.measurements
                                WHERE fk_batch=:bid;";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd, false);
                //if (res <= 0)
                //{
                //    throw new Exception("Error removing batch measurements info");
                //}

                //REMOVE BATCH CONTENTS
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"DELETE FROM public.batch_content
                                WHERE fk_batch=:bid;";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                Db.ExecuteNonQuery(cmd, false);


                /* //REMOVE ALL EQUIPMENT SETTINGS
                 cmd.CommandText =
                     @"DELETE FROM public.calendering_heat_roller_press
                                 WHERE fk_batch_process IN (
                                     SELECT batch_process_id
                                     FROM public.batch_process
                                     WHERE fk_batch=:bid                                      
                                     );";
                 Db.ExecuteNonQuery(cmd, false);
                 cmd.CommandText =
                     @"DELETE FROM public.calendering_manual
                                 WHERE fk_batch_process IN (
                                     SELECT batch_process_id
                                     FROM public.batch_process
                                     WHERE fk_batch=:bid                                      
                                     );";
                 Db.ExecuteNonQuery(cmd, false);
                 cmd.CommandText =
                     @"DELETE FROM public.calendering_manual_press
                                 WHERE fk_batch_process IN (
                                     SELECT batch_process_id
                                     FROM public.batch_process
                                     WHERE fk_batch=:bid                                      
                                     );";
                 Db.ExecuteNonQuery(cmd, false);
                 cmd.CommandText =
                     @"DELETE FROM public.heating_manual
                                 WHERE fk_batch_process IN (
                                     SELECT batch_process_id
                                     FROM public.batch_process
                                     WHERE fk_batch=:bid                                      
                                     );";
                 Db.ExecuteNonQuery(cmd, false);
                 cmd.CommandText =
                     @"DELETE FROM public.heating_oven
                                 WHERE fk_batch_process IN (
                                     SELECT batch_process_id
                                     FROM public.batch_process
                                     WHERE fk_batch=:bid                                      
                                     );";
                 Db.ExecuteNonQuery(cmd, false);
                 cmd.CommandText =
                     @"DELETE FROM public.heating_plate
                                 WHERE fk_batch_process IN (
                                     SELECT batch_process_id
                                     FROM public.batch_process
                                     WHERE fk_batch=:bid                                      
                                     );";
                 Db.ExecuteNonQuery(cmd, false);
                 cmd.CommandText =
                     @"DELETE FROM public.heating_tube_furnace
                                 WHERE fk_batch_process IN (
                                     SELECT batch_process_id
                                     FROM public.batch_process
                                     WHERE fk_batch=:bid                                      
                                     );";
                 Db.ExecuteNonQuery(cmd, false);
                 cmd.CommandText =
                     @"DELETE FROM public.milling_ball_mill
                                 WHERE fk_batch_process IN (
                                     SELECT batch_process_id
                                     FROM public.batch_process
                                     WHERE fk_batch=:bid                                      
                                     );";
                 Db.ExecuteNonQuery(cmd, false);
                 cmd.CommandText =
                     @"DELETE FROM public.milling_mortar_and_pestle
                                 WHERE fk_batch_process IN (
                                     SELECT batch_process_id
                                     FROM public.batch_process
                                     WHERE fk_batch=:bid                                      
                                     );";
                 Db.ExecuteNonQuery(cmd, false);
                 cmd.CommandText =
                     @"DELETE FROM public.mixing_ball_mill
                                 WHERE fk_batch_process IN (
                                     SELECT batch_process_id
                                     FROM public.batch_process
                                     WHERE fk_batch=:bid                                      
                                     );";
                 Db.ExecuteNonQuery(cmd, false);
                 cmd.CommandText =
                     @"DELETE FROM public.mixing_planetary_mixer
                                 WHERE fk_batch_process IN (
                                     SELECT batch_process_id
                                     FROM public.batch_process
                                     WHERE fk_batch=:bid                                      
                                     );";
                 Db.ExecuteNonQuery(cmd, false);
                 cmd.CommandText =
                     @"DELETE FROM public.mixing_hot_plate_stirrer
                                 WHERE fk_batch_process IN (
                                     SELECT batch_process_id
                                     FROM public.batch_process
                                     WHERE fk_batch=:bid                                      
                                     );";
                 Db.ExecuteNonQuery(cmd, false);
                 cmd.CommandText =
                     @"DELETE FROM public.pressing_manual
                                 WHERE fk_batch_process IN (
                                     SELECT batch_process_id
                                     FROM public.batch_process
                                     WHERE fk_batch=:bid                                      
                                     );";
                 Db.ExecuteNonQuery(cmd, false);
                 cmd.CommandText =
                     @"DELETE FROM public.pressing_manual_hydraulic_press
                                 WHERE fk_batch_process IN (
                                     SELECT batch_process_id
                                     FROM public.batch_process
                                     WHERE fk_batch=:bid                                      
                                     );";
                 Db.ExecuteNonQuery(cmd, false);*/


                /*  //REMOVE ALL BATCH PROCESS ATTRIBUTES
                  cmd.CommandText =
                      @"DELETE FROM public.milling
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid                                      
                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.mixing
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid                                      
                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.heating
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid                                      
                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.pressing
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid                                      
                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.calendering
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid                                      
                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.phase_inversion
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid                                      
                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.acid_dissolution
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.annealing
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.assembling
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.atomic_layer_deposition
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.centrifuging
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.coating
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.cooling
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.co_precipitation
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.decanting
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.decomposing
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.depositing
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.dropcasting
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.drying
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.electrodepositing
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.electrode_winding
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.electrolyte_diffusion_degassing
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.electroplating
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.etching
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.filtrating
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.formation
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.galvanizing
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.impregnating
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.lithiation
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.ozone
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.pasting
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.purifying
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.recrystalizing
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.screenprinting
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.sealing
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.sieving
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.sintering
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.slitting
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.sonicating
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.spray_pyrrolysis
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.stacking
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.washing
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.welding
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);

                  cmd.CommandText =
                      @"DELETE FROM public.wet_impregnating
                                  WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid

                                      );";
                  Db.ExecuteNonQuery(cmd, false);*/
                //REMOVE ALL EQUIPMENT ATTRIBUTES
                cmd.CommandText =
                                  @"DELETE FROM public.equipment_attribute_values
                                    WHERE fk_batch_process IN (
                                      SELECT batch_process_id
                                      FROM public.batch_process
                                      WHERE fk_batch=:bid
                                      );";

                Db.ExecuteNonQuery(cmd, false);

                //REMOVE ALL OF THE PROCESSES

                cmd.CommandText =
                    @"DELETE FROM public.batch_process
                                WHERE fk_batch=:bid;";

                Db.ExecuteNonQuery(cmd, false);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return 0;
        }
        public static int RemoveBatchContentFromStock(int batchId, int researchGroupId, NpgsqlCommand cmd)
        {
            int res = 0;

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
            try
            {
                //REMOVE BATCH STOCK ENTRIES
                cmd.Parameters.Clear();
                cmd.CommandText =
                        @"DELETE FROM public.stock_transaction WHERE fk_batch_coming=:bcomid;";

                Db.CreateParameterFunc(cmd, "@bcomid", batchId, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd, false);
                //if (res <= 0)
                //{
                //    throw new Exception("Error removing batch stock info");
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return 0;
        }
        public static int RemoveBatchOutputFromStock(int batchId, int researchGroupId, NpgsqlCommand cmd)
        {
            int res = 0;

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
            try
            {
                //REMOVE BATCH OUTPUT WEIGHT ENTRY TOGETHER WITH RECREATIONS
                cmd.Parameters.Clear();
                cmd.CommandText =
                        @"DELETE FROM public.stock_transaction 
                            WHERE fk_batch=:bcomid AND
                            fk_experiment_coming IS NULL AND
                            fk_batch_coming IS NULL
                        ;";

                Db.CreateParameterFunc(cmd, "@bcomid", batchId, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return 0;
        }
        public static int RemoveBatchDocuments(int batchId, NpgsqlCommand cmd)
        {
            bool isEnclosedInTransaction = true;

            if (cmd != null)
            {
                cmd.Parameters.Clear();
            }
            else
            {
                cmd = Db.CreateCommand();

                isEnclosedInTransaction = false;

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
            }
            try
            {
                var fileAttachmentsList = FileAttachmentDa.GetFileAttachments(null, "Batch", batchId);
                if (fileAttachmentsList != null)
                {
                    foreach (FileAttachmentExt fileAttachment in fileAttachmentsList)
                    {
                        Helpers.Files.DeleteFile(fileAttachment.serverFilename);
                    }
                }

                FileAttachmentDa.DeleteAllFileAttachmentByElement("Batch", batchId, cmd);

                if (!isEnclosedInTransaction)
                {
                    cmd.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return 0;
        }

        //        public static int AddBatch(Batch batch)
        //        {
        //            try
        //            {
        //                var cmd = Db.CreateCommand();
        //                if (cmd.Connection.State != ConnectionState.Open)
        //                {
        //                    cmd.Connection.Open();
        //                }
        //                cmd.CommandText =
        //                    @"INSERT INTO public.batch (batch_system_label, batch_personal_label, fk_user, batch_output, fk_measurement_unit, chemical_formula, description, date_created)
        //                    VALUES (:slabel, :plabel, :uid, :bout, :muid, :cf, :desc, now()::timestamp);";


        //                Db.CreateParameterFunc(cmd, "@slabel", batch.batchSystemLabel, NpgsqlDbType.Text);
        //                Db.CreateParameterFunc(cmd, "@plabel", batch.batchPersonalLabel, NpgsqlDbType.Text);
        //                Db.CreateParameterFunc(cmd, "@uid", batch.fkUser, NpgsqlDbType.Integer);
        //                Db.CreateParameterFunc(cmd, "@bout", batch.batchOutput, NpgsqlDbType.Double);
        //                Db.CreateParameterFunc(cmd, "@muid", batch.fkMeasurementUnit, NpgsqlDbType.Integer);
        //                Db.CreateParameterFunc(cmd, "@cf", batch.chemicalFormula, NpgsqlDbType.Text);
        //                Db.CreateParameterFunc(cmd, "@desc", batch.description, NpgsqlDbType.Text);
        //                //Db.CreateParameterFunc(cmd, "@datec", batch.dateCreated, NpgsqlDbType.Date);
        //                //Db.CreateParameterFunc(cmd, "@lastc", batch.lastChange, NpgsqlDbType.Date);

        //                Db.ExecuteNonQuery(cmd);
        //            }
        //            catch (Exception ex)
        //            {
        //                throw new Exception("Error inserting batch", ex);
        //            }

        //            return 0;
        //        }

        public static int AddBatchGeneralInfo(Batch batch, string acronym, int lastBatchNumber)
        {
            int returnedBatchId = 0;

            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            NpgsqlTransaction t = cmd.Connection.BeginTransaction();

            try
            {
                string systemLabel = acronym + "_" + "BTC_" + lastBatchNumber;
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"INSERT INTO public.batch (batch_personal_label, fk_user, fk_project, batch_output, fk_measurement_unit, chemical_formula, fk_material_type, description, fk_template, fk_research_group, date_created, batch_system_label)
                    VALUES (:plabel, :uid, :pid, :bout, :muid, :cf, :mtid, :desc, :tmpid, :rgid, now()::timestamp, :slabel)
                    RETURNING batch_id;";


                Db.CreateParameterFunc(cmd, "@plabel", batch.batchPersonalLabel, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@sl", batch.suggestedLabel, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@uid", batch.fkUser, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pid", batch.fkProject, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bout", batch.batchOutput, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@muid", batch.fkMeasurementUnit, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@cf", batch.chemicalFormula, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@mtid", batch.fkMaterialType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@desc", batch.description, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@tmpid", batch.fkTemplate, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", batch.fkResearchGroup, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@slabel", systemLabel, NpgsqlDbType.Text);


                returnedBatchId = int.Parse(Db.ExecuteScalar(cmd, false));
                if (returnedBatchId <= 0)
                {
                    t.Rollback();
                    throw new Exception("Error inserting batch general info");
                }
                //string systemLabel = "BTC_" + returnedBatchId;
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.research_group
                        SET last_batch_number=:lbn
                        WHERE research_group_id=:rgid;";

                Db.CreateParameterFunc(cmd, "@lbn", lastBatchNumber + 1, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", batch.fkResearchGroup, NpgsqlDbType.Integer);
                var res = Db.ExecuteNonQuery(cmd, false);
                if (res <= 0)
                {
                    t.Rollback();
                    throw new Exception("Error updating batch system label info");
                }

                t.Commit();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                t.Rollback();
                throw new Exception(ex.Message);
            }

            return returnedBatchId;
        }

        public static int CheckBatchGeneralInfoForErrors(AddBatchRequest req, int researchGroupId)
        {
            //if (cmd != null)
            //{
            //    cmd.Parameters.Clear();
            //}
            //else
            //{
            //    cmd = Db.CreateCommand();

            //    if (cmd.Connection.State != ConnectionState.Open)
            //    {
            //        cmd.Connection.Open();
            //    }
            //}
            try
            {
                if (req.batchInfo.batchOutput == null || req.batchInfo.totalBatchOutput == null ||
                    req.batchInfo.fkMeasurementUnit == null || req.batchInfo.chemicalFormula == null || req.batchInfo.fkMaterialType == null ||
                    req.batchInfo.description == null
                    )
                {
                    throw new Exception("Please enter all general batch data");
                }
                if (req.batchInfo.batchOutput <= 0)
                {
                    throw new Exception("Batch output must be greater then zero!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return 0;
        }
        public static int CheckBatchContentForErrors(AddBatchRequest req, int researchGroupId, NpgsqlCommand cmd)
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

            string invalidPercentageMessage = "Invalid \'percentage of active\' value for ";

            try
            {
                //operatorId = (int)req.batchInfo.fkUser;
                //batchOutputWeight = (double)req.batchInfo.batchOutput;

                List<BatchContentExt> batchContentList = req.batchContent;
                if (batchContentList.Count <= 0)
                {
                    throw new Exception("At least one material or batch needs to be chosen");
                }

                //List<dynamic> batchProcessList = req.batchProcesses;
                List<BatchProcessResponse> batchProcessList = req.batchProcesses;
                if (batchProcessList.Count <= 0)
                {
                    throw new Exception("At least one process needs to be chosen");
                }

                if (batchContentList.Count > 0)
                {

                    foreach (BatchContentExt batchContent in batchContentList)
                    {
                        if (batchContent.weight <= 0)
                        {
                            throw new Exception("Material/Batch quantity must be greater then zero!");
                        }
                        if (batchContent.percentageOfActive != null)
                        {
                            if (batchContent.percentageOfActive < 0 || batchContent.percentageOfActive > 100)
                            {
                                if (batchContent.fkStepMaterial != null)
                                {
                                    invalidPercentageMessage += batchContent.materialName;
                                }
                                else
                                {
                                    invalidPercentageMessage += batchContent.batchSystemLabel;
                                }
                                Exception ex = new Exception(invalidPercentageMessage);
                                throw ex;
                            }
                        }
                    }
                }
            }
            //catch (ValidationException ve)
            //{
            //    //do whatever
            //    throw new Exception(ve.Message);
            //}
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return 0;
        }
        //NOT IN USE ANYMORE - checks are includen in AddStock
        public static int CheckBatchStockForErrors(AddBatchRequest req, int researchGroupId, NpgsqlCommand cmd)
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
            bool materialQuantityOk = true;
            bool batchQuantityOk = true;

            string invalidWeightMessage = "Invalid quantity value for ";
            string notEnoughMessage = "Not enough ";

            try
            {
                //STOCK VALIDATION

                List<BatchContentExt> batchContentList = req.batchContent;

                List<string> materials = new List<string>();
                List<string> batches = new List<string>();

                foreach (BatchContentExt batchContent in batchContentList)
                {
                    if (batchContent.weight == null)
                    {
                        if (batchContent.fkStepMaterial != null)
                        {
                            invalidWeightMessage += batchContent.materialName;
                        }

                        else
                        {
                            invalidWeightMessage += batchContent.batchSystemLabel;
                        }
                        Exception ex = new Exception(invalidWeightMessage);
                        throw ex;
                    }

                    double wantedQuantity = (double)batchContent.weight;
                    if (batchContent.fkStepMaterial != null)
                    {
                        int materialId = (int)batchContent.fkStepMaterial;
                        bool result = StockTransactionDa.CheckMaterialStockQuantityEnough(materialId, wantedQuantity, researchGroupId, cmd);

                        if (result == false)
                        {
                            materialQuantityOk = false;
                            materials.Add(batchContent.materialName);
                        }
                    }
                    else
                    {
                        //ako e batch
                        int batchId = (int)batchContent.fkStepBatch;
                        bool result = StockTransactionDa.CheckBatchStockQuantityEnough(batchId, wantedQuantity, researchGroupId, cmd);
                        if (result == false)
                        {
                            batchQuantityOk = false;
                            batches.Add(batchContent.batchSystemLabel);
                        }
                    }
                }

                if (materialQuantityOk == false)
                {
                    string materialsString = "";
                    foreach (string m in materials)
                    {
                        if (materials.IndexOf(m) == materials.Count - 1)
                        {
                            materialsString += m;
                        }
                        else
                        {
                            materialsString += m + ", ";
                        }
                    }
                    //return 0;
                    notEnoughMessage += "Materials: " + materialsString;
                }
                if (batchQuantityOk == false)
                {
                    string batchesString = "";
                    foreach (string b in batches)
                    {
                        if (batches.IndexOf(b) == batches.Count - 1)
                        {
                            batchesString += b;
                        }
                        else
                        {
                            batchesString += b + ", ";
                        }
                    }
                    notEnoughMessage += " Batches: " + batchesString;
                }
                if (!materialQuantityOk || !batchQuantityOk)
                {
                    notEnoughMessage += " in stock!";
                    throw new Exception(notEnoughMessage);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return 0;
        }


        public static int AddBatchOutputStock(AddBatchRequest req, int batchId, int researchGroupId, NpgsqlCommand cmd)
        {
            int res = 0;
            int returnedBatchProcessID = 0;
            int operatorId = 0;
            double batchOutputWeight = 0;

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
            try
            {
                operatorId = (int)req.batchInfo.fkUser;
                batchOutputWeight = (double)req.batchInfo.batchOutput;


                //ADD BATCH OUTPUT WEIGHT TO STOCK
                cmd.Parameters.Clear();
                cmd.CommandText =
                        @"INSERT INTO public.stock_transaction (
                            fk_batch,
                            stock_transaction_element_type,
                            amount,
                            fk_operator,
                            fk_research_group,
                            fk_experiment_coming,
                            fk_batch_coming,
                            transaction_direction,
                            date_created

                            )
                                                VALUES (
                            :bid,
                            2,
                            :a,
                            :oid,
                            :rgid,
                            :ecomid,
                            :bcomid,
                            1,
                            now()::timestamp);";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@a", batchOutputWeight, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@oid", operatorId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@ecomid", null, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bcomid", null, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@stet", batchContent.fkFunction, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd, false);
                if (res <= 0)
                {
                    throw new Exception("Error inserting batch stock info");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return 0;

        }
        public static int AddBatchStock(AddBatchRequest req, int batchId, int researchGroupId, NpgsqlCommand cmd)
        {
            int res = 0;
            int operatorId = 0;

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
            try
            {
                operatorId = (int)req.batchInfo.fkUser;

                bool materialQuantityOk = true;
                bool batchQuantityOk = true;

                string invalidWeightMessage = "Invalid quantity value for ";
                string notEnoughMessage = "Not enough ";

                List<BatchContentExt> batchContentList = req.batchContent;

                List<string> materials = new List<string>();
                List<string> batches = new List<string>();

                //INSERT BATCH CONTENT IN STOCK
                foreach (BatchContentExt batchContent in batchContentList)
                {
                    //CONTENT VALIDATION
                    if (batchContent.weight == null)
                    {
                        if (batchContent.fkStepMaterial != null)
                        {
                            invalidWeightMessage += batchContent.materialName;
                        }

                        else
                        {
                            invalidWeightMessage += batchContent.batchSystemLabel;
                        }
                        Exception ex = new Exception(invalidWeightMessage);
                        throw ex;
                    }
                    //STOCK VALIDATION AND INSERT
                    double wantedQuantity = (double)batchContent.weight;
                    if (batchContent.fkStepMaterial != null)
                    {
                        int materialId = (int)batchContent.fkStepMaterial;
                        bool result = StockTransactionDa.CheckMaterialStockQuantityEnough(materialId, wantedQuantity, researchGroupId, cmd);

                        if (result == false)
                        {
                            materialQuantityOk = false;
                            materials.Add(batchContent.materialName);
                        }
                        else
                        {
                            //INSERT IN STOCK - MATERIAL
                            cmd.Parameters.Clear();
                            cmd.CommandText =
                            @"INSERT INTO public.stock_transaction (
                            fk_material,
                            stock_transaction_element_type,
                            amount,
                            fk_operator,
                            fk_research_group,
                            fk_experiment_coming,
                            fk_batch_coming,
                            transaction_direction,
                            date_created

                            )
                                                VALUES (
                            :mid,
                            1,
                            :a,
                            :oid,
                            :rgid,
                            :ecomid,
                            :bcomid,
                            -1,
                            now()::timestamp);";

                            Db.CreateParameterFunc(cmd, "@mid", batchContent.fkStepMaterial, NpgsqlDbType.Integer);
                            Db.CreateParameterFunc(cmd, "@a", batchContent.weight, NpgsqlDbType.Double);
                            Db.CreateParameterFunc(cmd, "@oid", operatorId, NpgsqlDbType.Integer);
                            Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                            Db.CreateParameterFunc(cmd, "@ecomid", null, NpgsqlDbType.Integer);
                            Db.CreateParameterFunc(cmd, "@bcomid", batchId, NpgsqlDbType.Integer);

                            res = Db.ExecuteNonQuery(cmd, false);
                            //if (res <= 0)
                            //{
                            //    //t.Rollback();
                            //    throw new Exception("Error inserting material stock info");
                            //}
                        }
                    }
                    else
                    {
                        //IT'S A BATCH
                        int stepBatchId = (int)batchContent.fkStepBatch;
                        bool result = StockTransactionDa.CheckBatchStockQuantityEnough(stepBatchId, wantedQuantity, researchGroupId, cmd);
                        if (result == false)
                        {
                            batchQuantityOk = false;
                            batches.Add(batchContent.batchSystemLabel);
                        }
                        else
                        {
                            //INSERT IN STOCK - BATCH
                            cmd.Parameters.Clear();
                            cmd.Parameters.Clear();
                            cmd.CommandText =
                            @"INSERT INTO public.stock_transaction (
                            fk_batch,
                            stock_transaction_element_type,
                            amount,
                            fk_operator,
                            fk_research_group,
                            fk_experiment_coming,
                            fk_batch_coming,
                            transaction_direction,
                            date_created

                            )
                                                VALUES (
                            :bid,
                            2,
                            :a,
                            :oid,
                            :rgid,
                            :ecomid,
                            :bcomid,
                            -1,
                            now()::timestamp);";

                            Db.CreateParameterFunc(cmd, "@bid", batchContent.fkStepBatch, NpgsqlDbType.Integer);
                            Db.CreateParameterFunc(cmd, "@a", batchContent.weight, NpgsqlDbType.Double);
                            Db.CreateParameterFunc(cmd, "@oid", operatorId, NpgsqlDbType.Integer);
                            Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                            Db.CreateParameterFunc(cmd, "@ecomid", null, NpgsqlDbType.Integer);
                            Db.CreateParameterFunc(cmd, "@bcomid", batchId, NpgsqlDbType.Integer);

                            res = Db.ExecuteNonQuery(cmd, false);
                            //if (res <= 0)
                            //{
                            //    //t.Rollback();
                            //    throw new Exception("Error inserting batch stock info");
                            //}
                        }
                    }
                }

                //IF ANY ERROR, RETURN
                if (materialQuantityOk == false)
                {
                    string materialsString = "";
                    foreach (string m in materials)
                    {
                        if (materials.IndexOf(m) == materials.Count - 1)
                        {
                            materialsString += m;
                        }
                        else
                        {
                            materialsString += m + ", ";
                        }
                    }
                    //return 0;
                    notEnoughMessage += "Materials: " + materialsString;
                }
                if (batchQuantityOk == false)
                {
                    string batchesString = "";
                    foreach (string b in batches)
                    {
                        if (batches.IndexOf(b) == batches.Count - 1)
                        {
                            batchesString += b;
                        }
                        else
                        {
                            batchesString += b + ", ";
                        }
                    }
                    notEnoughMessage += " Batches: " + batchesString;
                }
                if (!materialQuantityOk || !batchQuantityOk)
                {
                    notEnoughMessage += " in stock!";
                    throw new Exception(notEnoughMessage);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return 0;

        }
        public static int AddBatchContent(AddBatchRequest req, int batchId, int researchGroupId, NpgsqlCommand cmd)
        {
            int res = 0;
            int returnedBatchProcessID = 0;
            //int operatorId = 0;
            //double batchOutputWeight = 0;

            bool isEnclosedInTransaction = true;

            if (cmd != null)
            {
                cmd.Parameters.Clear();
            }
            else
            {
                cmd = Db.CreateCommand();

                isEnclosedInTransaction = false;

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
            }
            try
            {
                //operatorId = (int)req.batchInfo.fkUser;
                //batchOutputWeight = (double)req.batchInfo.batchOutput;

                //INSERT BATCH CONTENTS
                foreach (BatchContentExt batchContent in req.batchContent)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText =
                    @"INSERT INTO public.batch_content (fk_batch, step, fk_step_material, fk_step_batch, weight, fk_function, percentage_of_active, order_in_step)
                    VALUES (:bid, :step, :sm, :sb, :w, :f, :poa, :ois)
                    RETURNING batch_content_id;";

                    Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@step", batchContent.step, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@sm", batchContent.fkStepMaterial, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@sb", batchContent.fkStepBatch, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@w", batchContent.weight, NpgsqlDbType.Double);
                    Db.CreateParameterFunc(cmd, "@f", batchContent.fkFunction, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@poa", batchContent.percentageOfActive, NpgsqlDbType.Double);
                    //Db.CreateParameterFunc(cmd, "@sit", batchContent.fkStoredInType, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@ois", batchContent.orderInStep, NpgsqlDbType.Integer);

                    //res = Db.ExecuteNonQuery(cmd, false);
                    int returnedBatchContentID = int.Parse(Db.ExecuteScalar(cmd, false));

                    if (returnedBatchContentID <= 0)
                    {
                        throw new Exception("Error inserting batch content");
                        //return 5;
                    }

                    //Insert content level measurements
                    if (batchContent.measurements != null)
                    {
                        MeasurementsExt measurements = (MeasurementsExt)batchContent.measurements;
                        measurements.fkBatch = batchId;
                        measurements.fkBatchContent = returnedBatchContentID;

                        measurements.fkExperiment = null;
                        measurements.fkBatteryComponentType = null;
                        measurements.fkBatteryComponentContent = null;
                        measurements.fkMeasurementLevelType = 4;
                        //measurement level type id: content=1; step=2; component=3; batch_content=4; batch=5;


                        MeasurementsDa.AddMeasurements(measurements, cmd);
                    }

                }

                //INSERT NA BATCH PROCESSES                
                //object submittedItemBatchProcess;
                foreach (BatchProcessResponse batchProcessData in req.batchProcesses)
                {
                    //string processType = batchProcessData.processAttributes.processType;
                    //int processTypeId = (int)batchProcessData.processAttributes.processTypeId;
                    int processTypeId = (int)batchProcessData.batchProcess.fkProcessType;
                    //string batchProcessDataString = Convert.ToString(batchProcessData);
                    string batchProcessAttributesString = Convert.ToString(batchProcessData.processAttributes);
                    List<EquipmentSettingsValue> equipmentAttributeList = JsonConvert.DeserializeObject<List<EquipmentSettingsValue>>(batchProcessAttributesString) as List<EquipmentSettingsValue>;
                    int? equipmentModelId = null;
                    equipmentModelId = batchProcessData.batchProcess.equipmentModelId;
                    int? equipmentId;
                    equipmentId = batchProcessData.batchProcess.fkEquipment;
                    string equipmentSettingsString = Convert.ToString(batchProcessData.equipmentSettings);

                    //submittedItemBatchProcess = JsonConvert.DeserializeObject<BatchProcess>(batchProcessDataString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                    //BatchProcess batchProcess = (BatchProcess)submittedItemBatchProcess;

                    BatchProcess batchProcess = (BatchProcess)batchProcessData.batchProcess;
                    batchProcess.fkBatch = batchId;
                    var validationResult = ValidationHelper.IsModelValidWithErrors(batchProcess);
                    if (validationResult.Count != 0)
                    {
                        throw new Exception(validationResult[0].ErrorMessage);
                    }
                    //return 0;
                    int returnedProcessId = ProcessTypeDa.GetProcess(equipmentId, processTypeId, equipmentModelId);
                    cmd.Parameters.Clear();
                    cmd.CommandText =
                    @"INSERT INTO public.batch_process (fk_batch, step, process_order_in_step, fk_process, label)
                    VALUES (:bid, :step, :pois, :process, :label)
                    RETURNING batch_process_id;";

                    Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@step", batchProcess.step, NpgsqlDbType.Integer);
                    // Db.CreateParameterFunc(cmd, "@ptid", batchProcess.fkProcessType, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@pois", batchProcess.processOrderInStep, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@process", returnedProcessId, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@label", batchProcess.label, NpgsqlDbType.Text);

                    //res = Db.ExecuteNonQuery(cmd, false);
                    returnedBatchProcessID = int.Parse(Db.ExecuteScalar(cmd, false));
                    if (returnedBatchProcessID <= 0)
                    {
                        throw new Exception("Error inserting batch process");
                    }

                    var processValidationResult = new List<ValidationResult>();
                    EquipmentSettingsDa.AddEquipmentAttributes(cmd, equipmentAttributeList, null, returnedBatchProcessID);
                    /* switch (processTypeId)
                     {
                         case 1:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Milling>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Milling milling = (Milling)submittedItemProcessAttributes;
                             milling.fkExperimentProcess = null;
                             milling.fkBatchProcess = returnedBatchProcessID;
                             //milling.fkEquipment = null;
                             //setiraj fkEquipment, fkBatchProcess
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(milling);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             MillingDa.AddMilling(milling, cmd);
                             break;

                         case 2:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Mixing>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Mixing mixing = (Mixing)submittedItemProcessAttributes;
                             mixing.fkExperimentProcess = null;
                             mixing.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(mixing);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             MixingDa.AddMixing(mixing, cmd);
                             break;

                         case 3:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Heating>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Heating heating = (Heating)submittedItemProcessAttributes;
                             heating.fkExperimentProcess = null;
                             heating.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(heating);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             HeatingDa.AddHeating(heating, cmd);
                             break;

                         case 4:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Pressing>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Pressing pressing = (Pressing)submittedItemProcessAttributes;
                             pressing.fkExperimentProcess = null;
                             pressing.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(pressing);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             PressingDa.AddPressing(pressing, cmd);
                             break;

                         case 5:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Calendering>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Calendering calendering = (Calendering)submittedItemProcessAttributes;
                             calendering.fkExperimentProcess = null;
                             calendering.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(calendering);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             CalenderingDa.AddCalendering(calendering, cmd);
                             break;

                         case 6:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<PhaseInversion>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             PhaseInversion phaseInversion = (PhaseInversion)submittedItemProcessAttributes;
                             phaseInversion.fkExperimentProcess = null;
                             phaseInversion.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(phaseInversion);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             PhaseInversionDa.AddPhaseInversion(phaseInversion, cmd);
                             break;

                         case 7:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<AcidDissolution>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             AcidDissolution acidDissolution = (AcidDissolution)submittedItemProcessAttributes;
                             acidDissolution.fkExperimentProcess = null;
                             acidDissolution.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(acidDissolution);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             AcidDissolutionDa.AddAcidDissolution(acidDissolution, cmd);
                             break;
                         case 8:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Annealing>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Annealing annealing = (Annealing)submittedItemProcessAttributes;
                             annealing.fkExperimentProcess = null;
                             annealing.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(annealing);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             AnnealingDa.AddAnnealing(annealing, cmd);
                             break;
                         case 9:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Assembling>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Assembling assembling = (Assembling)submittedItemProcessAttributes;
                             assembling.fkExperimentProcess = null;
                             assembling.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(assembling);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             AssemblingDa.AddAssembling(assembling, cmd);
                             break;
                         case 10:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<AtomicLayerDeposition>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             AtomicLayerDeposition atomicLayerDeposition = (AtomicLayerDeposition)submittedItemProcessAttributes;
                             atomicLayerDeposition.fkExperimentProcess = null;
                             atomicLayerDeposition.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(atomicLayerDeposition);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             AtomicLayerDepositionDa.AddAtomicLayerDeposition(atomicLayerDeposition, cmd);
                             break;
                         case 11:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Centrifuging>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Centrifuging centrifuging = (Centrifuging)submittedItemProcessAttributes;
                             centrifuging.fkExperimentProcess = null;
                             centrifuging.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(centrifuging);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             CentrifugingDa.AddCentrifuging(centrifuging, cmd);
                             break;
                         case 12:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Coating>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Coating coating = (Coating)submittedItemProcessAttributes;
                             coating.fkExperimentProcess = null;
                             coating.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(coating);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             CoatingDa.AddCoating(coating, cmd);
                             break;
                         case 13:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Cooling>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Cooling cooling = (Cooling)submittedItemProcessAttributes;
                             cooling.fkExperimentProcess = null;
                             cooling.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(cooling);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             CoolingDa.AddCooling(cooling, cmd);
                             break;
                         case 14:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<CoPrecipitation>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             CoPrecipitation coPrecipitation = (CoPrecipitation)submittedItemProcessAttributes;
                             coPrecipitation.fkExperimentProcess = null;
                             coPrecipitation.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(coPrecipitation);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             CoPrecipitationDa.AddCoPrecipitation(coPrecipitation, cmd);
                             break;
                         case 15:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Decanting>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Decanting decanting = (Decanting)submittedItemProcessAttributes;
                             decanting.fkExperimentProcess = null;
                             decanting.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(decanting);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             DecantingDa.AddDecanting(decanting, cmd);
                             break;
                         case 16:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Decomposing>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Decomposing decomposing = (Decomposing)submittedItemProcessAttributes;
                             decomposing.fkExperimentProcess = null;
                             decomposing.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(decomposing);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             DecomposingDa.AddDecomposing(decomposing, cmd);
                             break;
                         case 17:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Depositing>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Depositing depositing = (Depositing)submittedItemProcessAttributes;
                             depositing.fkExperimentProcess = null;
                             depositing.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(depositing);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             DepositingDa.AddDepositing(depositing, cmd);
                             break;
                         case 18:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Dropcasting>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Dropcasting dropcasting = (Dropcasting)submittedItemProcessAttributes;
                             dropcasting.fkExperimentProcess = null;
                             dropcasting.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(dropcasting);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             DropcastingDa.AddDropcasting(dropcasting, cmd);
                             break;
                         case 19:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Drying>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Drying drying = (Drying)submittedItemProcessAttributes;
                             drying.fkExperimentProcess = null;
                             drying.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(drying);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             DryingDa.AddDrying(drying, cmd);
                             break;
                         case 20:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Electrodepositing>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Electrodepositing electrodepositing = (Electrodepositing)submittedItemProcessAttributes;
                             electrodepositing.fkExperimentProcess = null;
                             electrodepositing.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(electrodepositing);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             ElectrodepositingDa.AddElectrodepositing(electrodepositing, cmd);
                             break;
                         case 21:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<ElectrodeWinding>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             ElectrodeWinding electrodeWinding = (ElectrodeWinding)submittedItemProcessAttributes;
                             electrodeWinding.fkExperimentProcess = null;
                             electrodeWinding.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(electrodeWinding);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             ElectrodeWindingDa.AddElectrodeWinding(electrodeWinding, cmd);
                             break;
                         case 22:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<ElectrolyteDiffusionDegassing>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             ElectrolyteDiffusionDegassing electrolyteDiffusionDegassing = (ElectrolyteDiffusionDegassing)submittedItemProcessAttributes;
                             electrolyteDiffusionDegassing.fkExperimentProcess = null;
                             electrolyteDiffusionDegassing.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(electrolyteDiffusionDegassing);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             ElectrolyteDiffusionDegassingDa.AddElectrolyteDiffusionDegassing(electrolyteDiffusionDegassing, cmd);
                             break;
                         case 23:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Electroplating>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Electroplating electroplating = (Electroplating)submittedItemProcessAttributes;
                             electroplating.fkExperimentProcess = null;
                             electroplating.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(electroplating);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             ElectroplatingDa.AddElectroplating(electroplating, cmd);
                             break;
                         case 24:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Etching>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Etching etching = (Etching)submittedItemProcessAttributes;
                             etching.fkExperimentProcess = null;
                             etching.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(etching);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             EtchingDa.AddEtching(etching, cmd);
                             break;
                         case 25:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Filtrating>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Filtrating filtrating = (Filtrating)submittedItemProcessAttributes;
                             filtrating.fkExperimentProcess = null;
                             filtrating.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(filtrating);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             FiltratingDa.AddFiltrating(filtrating, cmd);
                             break;
                         case 26:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Formation>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Formation formation = (Formation)submittedItemProcessAttributes;
                             formation.fkExperimentProcess = null;
                             formation.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(formation);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             FormationDa.AddFormation(formation, cmd);
                             break;
                         case 27:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Galvanizing>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Galvanizing galvanizing = (Galvanizing)submittedItemProcessAttributes;
                             galvanizing.fkExperimentProcess = null;
                             galvanizing.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(galvanizing);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             GalvanizingDa.AddGalvanizing(galvanizing, cmd);
                             break;
                         case 28:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Impregnating>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Impregnating impregnating = (Impregnating)submittedItemProcessAttributes;
                             impregnating.fkExperimentProcess = null;
                             impregnating.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(impregnating);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             ImpregnatingDa.AddImpregnating(impregnating, cmd);
                             break;
                         case 29:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Lithiation>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Lithiation lithiation = (Lithiation)submittedItemProcessAttributes;
                             lithiation.fkExperimentProcess = null;
                             lithiation.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(lithiation);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             LithiationDa.AddLithiation(lithiation, cmd);
                             break;
                         case 30:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Ozone>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Ozone ozone = (Ozone)submittedItemProcessAttributes;
                             ozone.fkExperimentProcess = null;
                             ozone.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(ozone);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             OzoneDa.AddOzone(ozone, cmd);
                             break;
                         case 31:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Pasting>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Pasting pasting = (Pasting)submittedItemProcessAttributes;
                             pasting.fkExperimentProcess = null;
                             pasting.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(pasting);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             PastingDa.AddPasting(pasting, cmd);
                             break;
                         case 32:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Purifying>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Purifying purifying = (Purifying)submittedItemProcessAttributes;
                             purifying.fkExperimentProcess = null;
                             purifying.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(purifying);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             PurifyingDa.AddPurifying(purifying, cmd);
                             break;
                         case 33:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Recrystalizing>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Recrystalizing recrystalizing = (Recrystalizing)submittedItemProcessAttributes;
                             recrystalizing.fkExperimentProcess = null;
                             recrystalizing.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(recrystalizing);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             RecrystalizingDa.AddRecrystalizing(recrystalizing, cmd);
                             break;
                         case 34:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Screenprinting>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Screenprinting screenprinting = (Screenprinting)submittedItemProcessAttributes;
                             screenprinting.fkExperimentProcess = null;
                             screenprinting.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(screenprinting);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             ScreenprintingDa.AddScreenprinting(screenprinting, cmd);
                             break;
                         case 35:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Sealing>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Sealing sealing = (Sealing)submittedItemProcessAttributes;
                             sealing.fkExperimentProcess = null;
                             sealing.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(sealing);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             SealingDa.AddSealing(sealing, cmd);
                             break;
                         case 36:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Sieving>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Sieving sieving = (Sieving)submittedItemProcessAttributes;
                             sieving.fkExperimentProcess = null;
                             sieving.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(sieving);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             SievingDa.AddSieving(sieving, cmd);
                             break;
                         case 37:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Sintering>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Sintering sintering = (Sintering)submittedItemProcessAttributes;
                             sintering.fkExperimentProcess = null;
                             sintering.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(sintering);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             SinteringDa.AddSintering(sintering, cmd);
                             break;
                         case 38:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Slitting>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Slitting slitting = (Slitting)submittedItemProcessAttributes;
                             slitting.fkExperimentProcess = null;
                             slitting.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(slitting);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             SlittingDa.AddSlitting(slitting, cmd);
                             break;
                         case 39:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Sonicating>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Sonicating sonicating = (Sonicating)submittedItemProcessAttributes;
                             sonicating.fkExperimentProcess = null;
                             sonicating.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(sonicating);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             SonicatingDa.AddSonicating(sonicating, cmd);
                             break;
                         case 40:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<SprayPyrrolysis>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             SprayPyrrolysis sprayPyrrolysis = (SprayPyrrolysis)submittedItemProcessAttributes;
                             sprayPyrrolysis.fkExperimentProcess = null;
                             sprayPyrrolysis.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(sprayPyrrolysis);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             SprayPyrrolysisDa.AddSprayPyrrolysis(sprayPyrrolysis, cmd);
                             break;
                         case 41:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Stacking>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Stacking stacking = (Stacking)submittedItemProcessAttributes;
                             stacking.fkExperimentProcess = null;
                             stacking.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(stacking);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             StackingDa.AddStacking(stacking, cmd);
                             break;
                         case 42:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Washing>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Washing washing = (Washing)submittedItemProcessAttributes;
                             washing.fkExperimentProcess = null;
                             washing.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(washing);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             WashingDa.AddWashing(washing, cmd);
                             break;
                         case 43:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<Welding>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             Welding welding = (Welding)submittedItemProcessAttributes;
                             welding.fkExperimentProcess = null;
                             welding.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(welding);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             WeldingDa.AddWelding(welding, cmd);
                             break;
                         case 44:
                             submittedItemProcessAttributes = JsonConvert.DeserializeObject<WetImpregnating>(batchProcessAttributesString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                             WetImpregnating wetImpregnating = (WetImpregnating)submittedItemProcessAttributes;
                             wetImpregnating.fkExperimentProcess = null;
                             wetImpregnating.fkBatchProcess = returnedBatchProcessID;
                             processValidationResult = ValidationHelper.IsModelValidWithErrors(wetImpregnating);
                             if (processValidationResult.Count != 0)
                             {
                                 throw new Exception(processValidationResult[0].ErrorMessage);
                             }

                             WetImpregnatingDa.AddWetImpregnating(wetImpregnating, cmd);
                             break;

                         default:
                             throw new Exception("Process type not defined");
                     }*/

                    /*if (equipmentSettingsString != "" && equipmentSettingsString != null && equipmentId != null)
                    {
                        switch ((int)equipmentId)
                        {
                            case 1:
                                submittedItemEquipmentSettings = JsonConvert.DeserializeObject<MillingBallMill>(equipmentSettingsString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                                MillingBallMill millingBallMill = (MillingBallMill)submittedItemEquipmentSettings;
                                millingBallMill.fkExperimentProcess = null;
                                millingBallMill.fkBatchProcess = returnedBatchProcessID;

                                MillingBallMillDa.AddMillingBallMill(millingBallMill, cmd);
                                break;
                            case 2:
                                submittedItemEquipmentSettings = JsonConvert.DeserializeObject<MixingPlanetaryMixer>(equipmentSettingsString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                                MixingPlanetaryMixer mixingPlanetaryMixer = (MixingPlanetaryMixer)submittedItemEquipmentSettings;
                                mixingPlanetaryMixer.fkExperimentProcess = null;
                                mixingPlanetaryMixer.fkBatchProcess = returnedBatchProcessID;

                                MixingPlanetaryMixerDa.AddMixingPlanetaryMixer(mixingPlanetaryMixer, cmd);
                                break;
                            case 3:
                                submittedItemEquipmentSettings = JsonConvert.DeserializeObject<HeatingPlate>(equipmentSettingsString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                                HeatingPlate heatingPlate = (HeatingPlate)submittedItemEquipmentSettings;
                                heatingPlate.fkExperimentProcess = null;
                                heatingPlate.fkBatchProcess = returnedBatchProcessID;

                                HeatingPlateDa.AddHeatingPlate(heatingPlate, cmd);
                                break;
                            case 4:
                                submittedItemEquipmentSettings = JsonConvert.DeserializeObject<HeatingTubeFurnace>(equipmentSettingsString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                                HeatingTubeFurnace tubeFurnace = (HeatingTubeFurnace)submittedItemEquipmentSettings;
                                tubeFurnace.fkExperimentProcess = null;
                                tubeFurnace.fkBatchProcess = returnedBatchProcessID;

                                HeatingTubeFurnaceDa.AddTubeFurnace(tubeFurnace, cmd);
                                break;
                            case 5:
                                submittedItemEquipmentSettings = JsonConvert.DeserializeObject<PressingManualHydraulicPress>(equipmentSettingsString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                                PressingManualHydraulicPress pressingManualHydraulicPress = (PressingManualHydraulicPress)submittedItemEquipmentSettings;
                                pressingManualHydraulicPress.fkExperimentProcess = null;
                                pressingManualHydraulicPress.fkBatchProcess = returnedBatchProcessID;

                                PressingManualHydraulicPressDa.AddPressingManualHydraulicPress(pressingManualHydraulicPress, cmd);
                                break;
                            case 6:
                                submittedItemEquipmentSettings = JsonConvert.DeserializeObject<CalenderingHeatRollerPress>(equipmentSettingsString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                                CalenderingHeatRollerPress calenderingHeatRollerPress = (CalenderingHeatRollerPress)submittedItemEquipmentSettings;
                                calenderingHeatRollerPress.fkExperimentProcess = null;
                                calenderingHeatRollerPress.fkBatchProcess = returnedBatchProcessID;

                                CalenderingHeatRollerPressDa.AddCalenderingHeatRollerPress(calenderingHeatRollerPress, cmd);
                                break;
                            case 7:
                                submittedItemEquipmentSettings = JsonConvert.DeserializeObject<CalenderingManualPress>(equipmentSettingsString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                                CalenderingManualPress calenderingManualPress = (CalenderingManualPress)submittedItemEquipmentSettings;
                                calenderingManualPress.fkExperimentProcess = null;
                                calenderingManualPress.fkBatchProcess = returnedBatchProcessID;

                                CalenderingManualPressDa.AddCalenderingManualPress(calenderingManualPress, cmd);
                                break;
                            case 8:
                                submittedItemEquipmentSettings = JsonConvert.DeserializeObject<HeatingOven>(equipmentSettingsString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                                HeatingOven heatingOven = (HeatingOven)submittedItemEquipmentSettings;
                                heatingOven.fkExperimentProcess = null;
                                heatingOven.fkBatchProcess = returnedBatchProcessID;

                                HeatingOvenDa.AddHeatingOven(heatingOven, cmd);
                                break;
                            case 9:
                                submittedItemEquipmentSettings = JsonConvert.DeserializeObject<HeatingManual>(equipmentSettingsString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                                HeatingManual heatingManual = (HeatingManual)submittedItemEquipmentSettings;
                                heatingManual.fkExperimentProcess = null;
                                heatingManual.fkBatchProcess = returnedBatchProcessID;

                                HeatingManualDa.AddHeatingManual(heatingManual, cmd);
                                break;
                            case 10:
                                submittedItemEquipmentSettings = JsonConvert.DeserializeObject<PressingManual>(equipmentSettingsString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                                PressingManual pressingManual = (PressingManual)submittedItemEquipmentSettings;
                                pressingManual.fkExperimentProcess = null;
                                pressingManual.fkBatchProcess = returnedBatchProcessID;

                                PressingManualDa.AddPressingManual(pressingManual, cmd);
                                break;
                            case 11:
                                submittedItemEquipmentSettings = JsonConvert.DeserializeObject<CalenderingManual>(equipmentSettingsString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                                CalenderingManual calenderingManual = (CalenderingManual)submittedItemEquipmentSettings;
                                calenderingManual.fkExperimentProcess = null;
                                calenderingManual.fkBatchProcess = returnedBatchProcessID;

                                CalenderingManualDa.AddCalenderingManual(calenderingManual, cmd);
                                break;
                            case 12:
                                submittedItemEquipmentSettings = JsonConvert.DeserializeObject<MillingMortarAndPestle>(equipmentSettingsString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                                MillingMortarAndPestle millingMortarAndPestle = (MillingMortarAndPestle)submittedItemEquipmentSettings;
                                millingMortarAndPestle.fkExperimentProcess = null;
                                millingMortarAndPestle.fkBatchProcess = returnedBatchProcessID;

                                MillingMortarAndPestleDa.AddMillingMortarAndPestle(millingMortarAndPestle, cmd);
                                break;
                            //case 13:
                            //    submittedItemEquipmentSettings = JsonConvert.DeserializeObject<MixingBallMill>(equipmentSettingsString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                            //    MixingBallMill mixingBallMill = (MixingBallMill)submittedItemEquipmentSettings;
                            //    mixingBallMill.fkExperimentProcess = null;
                            //    mixingBallMill.fkBatchProcess = returnedBatchProcessID;

                            //    MixingBallMillDa.AddMixingBallMill(mixingBallMill, cmd);
                            //    break;
                            case 14:
                                submittedItemEquipmentSettings = JsonConvert.DeserializeObject<MixingHotPlateStirrer>(equipmentSettingsString, new IsoDateTimeConverter { DateTimeFormat = ConfigurationManager.AppSettings["dateFormat"] });
                                MixingHotPlateStirrer mixingHotPlateStirrer = (MixingHotPlateStirrer)submittedItemEquipmentSettings;
                                mixingHotPlateStirrer.fkExperimentProcess = null;
                                mixingHotPlateStirrer.fkBatchProcess = returnedBatchProcessID;

                                MixingHotPlateStirrerDa.AddMixingHotPlateStirrer(mixingHotPlateStirrer, cmd);
                                break;

                            //default:
                            //    throw new Exception("Batch: " + "Equipment type not defined");
                        }
                    }*/
                }

                //Add batch level measurements
                if (req.measurements != null)
                {
                    MeasurementsExt measurements = (MeasurementsExt)req.measurements;
                    measurements.fkBatch = batchId;
                    measurements.fkBatchContent = null;

                    measurements.fkExperiment = null;
                    measurements.fkBatteryComponentType = null;
                    measurements.stepId = null;
                    measurements.fkBatteryComponentContent = null;
                    measurements.fkMeasurementLevelType = 5;
                    MeasurementsDa.AddMeasurements(measurements, cmd);
                    //measurement level type id: content=1; step=2; component=3; batch_content=4; batch=5;
                }

                if (!isEnclosedInTransaction)
                {
                    cmd.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return 0;
        }

        public static List<BatchExt> GetBatchesByIdForCharts(int researchGroupId, int[] batchIdArray)
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
                    @"SELECT DISTINCT (b.batch_id), b.*, u.fk_research_group, u.username as operator_username, edu.username as editing_operator_username,
                    rg.*, mt.*, mu.*

                    FROM project_batch pb
                    LEFT JOIN batch b ON b.batch_id = pb.fk_batch
                    LEFT JOIN users u on b.fk_user = u.user_id
                    LEFT JOIN research_group rg ON u.fk_research_group = rg.research_group_id
                    LEFT JOIN users edu on b.fk_edited_by = edu.user_id
                    LEFT JOIN material_type mt on b.fk_material_type = mt.material_type_id
                    LEFT JOIN measurement_unit mu on b.fk_measurement_unit = mu.measurement_unit_id
						
                    WHERE pb.fk_project IN (
	                    SELECT fk_project 
	                    FROM project_research_group prg 
	                    WHERE prg.fk_research_group = :rgid
                    )                    
                    AND (b.is_complete = true)
                    AND b.has_test_results_doc = true
                    AND (b.batch_id = ANY(:bidList) or :bidList is null)
                    ORDER BY b.date_created DESC
                    ;";

                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bidList", batchIdArray, NpgsqlDbType.Array | NpgsqlDbType.Integer);

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

            List<BatchExt> list = (from DataRow dr in dt.Rows select CreateBatchObjectExt(dr)).ToList();

            return list;
        }

        public static BatchExt AddBatchWithContentAndReturn(AddBatchRequest req, int researchGroupId)
        {
            //METHOD USED FOR CREATING FROM EXPERIMENT UI ONLY
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandType = CommandType.Text;

            NpgsqlTransaction t = cmd.Connection.BeginTransaction();

            BatchExt returnedBatch;
            int res = 0;
            int returnedBatchID = 0;

            //int returnedBatchProcessID = 0;
            //int operatorId = 0;
            //double batchOutputWeight = 0;

            try
            {
                //CHECK BATCH REQUEST
                CheckBatchGeneralInfoForErrors(req, researchGroupId);
                CheckBatchContentForErrors(req, researchGroupId, cmd);
                //CheckBatchStockForErrors(req, researchGroupId, cmd);
                // Get last batch number and acronym
                var result = ResearchGroupDa.GetAllResearchGroups(researchGroupId)[0];
                string acronym = result.acronym;
                int lastBatchNumber = (int)result.lastBatchNumber;
                string systemLabel = acronym + "_" + "BTC_" + lastBatchNumber;
                //INSERT BATCH GENERAL INFO
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"INSERT INTO public.batch (batch_personal_label, fk_user, batch_output, fk_measurement_unit, chemical_formula, fk_material_type, description, fk_research_group, date_created, batch_system_label, total_batch_output, waste_amount, released_as, waste_chemical_composition, waste_comment)
                    VALUES (:plabel, :uid, :bout, :muid, :cf, :mtid, :desc, :rgid, now()::timestamp, :slabel, :tbo, :wa, :ra, :wcc, :wc)
                    RETURNING batch_id;";

                Db.CreateParameterFunc(cmd, "@plabel", req.batchInfo.batchPersonalLabel, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@sl", req.batchInfo.suggestedLabel, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@uid", req.batchInfo.fkUser, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bout", req.batchInfo.batchOutput, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@muid", req.batchInfo.fkMeasurementUnit, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@cf", req.batchInfo.chemicalFormula, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@mtid", req.batchInfo.fkMaterialType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@desc", req.batchInfo.description, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@rgid", req.batchInfo.fkResearchGroup, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@slabel", systemLabel, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@tbo", req.batchInfo.totalBatchOutput, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@wa", req.batchInfo.wasteAmount, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@ra", req.batchInfo.releasedAs, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@wcc", req.batchInfo.wasteChemicalComposition, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@wc", req.batchInfo.wasteComment, NpgsqlDbType.Text);

                //res = Db.ExecuteNonQuery(cmd, false);
                returnedBatchID = int.Parse(Db.ExecuteScalar(cmd, false));

                if (returnedBatchID <= 0)
                {
                    t.Rollback();
                    //return 5;
                    throw new Exception("Error inserting batch");
                }

                //string systemLabel = "BTC_" + returnedBatchID;
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.research_group
                        SET last_batch_number=:lbn
                        WHERE research_group_id=:rgid;";

                Db.CreateParameterFunc(cmd, "@lbn", lastBatchNumber + 1, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", req.batchInfo.fkResearchGroup, NpgsqlDbType.Integer);
                res = Db.ExecuteNonQuery(cmd, false);
                if (res <= 0)
                {
                    t.Rollback();
                    throw new Exception("Error updating batch system label info");
                }


                //INSERT BATCH CONTENT
                AddBatchContent(req, returnedBatchID, researchGroupId, cmd);

                //INSERT BATCH STOCK
                AddBatchStock(req, returnedBatchID, researchGroupId, cmd);

                //INSERT BATCH OUTPUT IN STOCK
                AddBatchOutputStock(req, returnedBatchID, researchGroupId, cmd);

                //SET BATCH COMPLETE TRUE
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.batch 
                        SET is_complete=true, last_change=now()::timestamp
                        WHERE batch_id=:bid;";

                Db.CreateParameterFunc(cmd, "@bid", returnedBatchID, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd, false);


                //RETURN THE BATCH WE JUST INSERTED                
                DataTable dt;
                cmd.Parameters.Clear();

                cmd.CommandText =
                    @"SELECT *, u.username as operator_username
                    FROM batch
                        left join users u on batch.fk_user = u.user_id
                        left join research_group rg on u.fk_research_group = rg.research_group_id
                        left join material_type mt on batch.fk_material_type = mt.material_type_id
                        left join measurement_unit mu on batch.fk_measurement_unit = mu.measurement_unit_id

                    WHERE (batch.batch_id = :bid or :bid is null) and
                        (u.fk_research_group = :rgid or :rgid is null);";

                Db.CreateParameterFunc(cmd, "@bid", returnedBatchID, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd, false);

                if (dt == null || dt.Rows.Count == 0)
                {
                    return null;
                }
                List<BatchExt> list = (from DataRow dr in dt.Rows select CreateBatchObjectExt(dr)).ToList();

                returnedBatch = list[0];

                t.Commit();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                t.Rollback();
                cmd.Connection.Close();
                throw new Exception(ex.Message);
            }
            //return 0;
            return returnedBatch;
        }


        public static List<int> GetBatchesInBatch(int batchId)
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
                       @"SELECT DISTINCT bc.fk_batch, bc.fk_step_batch, e.fk_project
                         FROM public.batch_content bc
                         
                         LEFT JOIN batch e ON bc.fk_batch = e.batch_id
                         LEFT JOIN project pr ON e.fk_project = pr.project_id  

                         WHERE fk_step_batch IS NOT NULL and
                               bc.fk_batch = :bid 

                         ORDER BY fk_batch DESC;";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            List<int> batchBatchList = new List<int>();

            if (dt == null || dt.Rows.Count == 0)
            {
                return batchBatchList;
            }

            foreach (DataRow dr in dt.Rows)
            {
                int id = int.Parse(dr["fk_step_batch"].ToString());
                batchBatchList.Add(id);
            }
            return batchBatchList;
        }

        public static int UpdateBatchWithContentAndReturn(AddBatchRequest req, int batchId, int researchGroupId, int userId, bool editing)
        {
            //FINISH BATCH CREATION
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandType = CommandType.Text;

            //BatchExt returnedBatch;
            int res = 0;
            //int returnedBatchID = 0;

            NpgsqlTransaction t = cmd.Connection.BeginTransaction();

            try
            {
                //CHECK BATCH REQUEST AND STOCK
                CheckBatchGeneralInfoForErrors(req, researchGroupId);
                CheckBatchContentForErrors(req, researchGroupId, cmd);

                //UPDATE BATCH GENERAL INFO
                if (editing == true)
                {
                    //SET UPDATED BY AND DATETIME WHEN UPDATED
                    cmd.Parameters.Clear();
                    //fk_user=:uid,
                    cmd.CommandText =
                        @"UPDATE public.batch 
                        SET batch_personal_label=:plabel,  
                            batch_output=:bout, fk_measurement_unit=:muid, chemical_formula=:cf, fk_material_type=:mtid,
                            description=:desc, fk_edited_by=:uid, last_change=now()::timestamp 
                        WHERE batch_id=:bid;";

                    Db.CreateParameterFunc(cmd, "@plabel", req.batchInfo.batchPersonalLabel, NpgsqlDbType.Text);
                    //Db.CreateParameterFunc(cmd, "@uid", req.batchInfo.fkUser, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@bout", req.batchInfo.batchOutput, NpgsqlDbType.Double);
                    Db.CreateParameterFunc(cmd, "@muid", req.batchInfo.fkMeasurementUnit, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@cf", req.batchInfo.chemicalFormula, NpgsqlDbType.Text);
                    Db.CreateParameterFunc(cmd, "@mtid", req.batchInfo.fkMaterialType, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@desc", req.batchInfo.description, NpgsqlDbType.Text);
                    Db.CreateParameterFunc(cmd, "@uid", userId, NpgsqlDbType.Integer);

                    Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                }
                else
                {
                    //SET CREATED BY AND DATETIME WHEN CREATED
                    cmd.Parameters.Clear();
                    //fk_user=:uid,
                    cmd.CommandText =
                        @"UPDATE public.batch 
                        SET batch_personal_label=:plabel,  
                            batch_output=:bout, fk_measurement_unit=:muid, chemical_formula=:cf, fk_material_type=:mtid,
                            description=:desc, fk_user=:uid, date_created=now()::timestamp
                        WHERE batch_id=:bid;";

                    Db.CreateParameterFunc(cmd, "@plabel", req.batchInfo.batchPersonalLabel, NpgsqlDbType.Text);
                    //Db.CreateParameterFunc(cmd, "@uid", req.batchInfo.fkUser, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@bout", req.batchInfo.batchOutput, NpgsqlDbType.Double);
                    Db.CreateParameterFunc(cmd, "@muid", req.batchInfo.fkMeasurementUnit, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@cf", req.batchInfo.chemicalFormula, NpgsqlDbType.Text);
                    Db.CreateParameterFunc(cmd, "@mtid", req.batchInfo.fkMaterialType, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@desc", req.batchInfo.description, NpgsqlDbType.Text);
                    Db.CreateParameterFunc(cmd, "@uid", userId, NpgsqlDbType.Integer);

                    Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                }


                res = Db.ExecuteNonQuery(cmd, false);

                if (res <= 0)
                {
                    throw new Exception("Error updating batch general info");
                }

                //REMOVE ALL BATCH CONTENT STOCK AND PROCESSES
                RemoveBatchContent(batchId, researchGroupId, cmd);
                RemoveBatchContentFromStock(batchId, researchGroupId, cmd);
                RemoveBatchOutputFromStock(batchId, researchGroupId, cmd);


                //INSERT BATCH CONTENT
                AddBatchContent(req, batchId, researchGroupId, cmd);


                //CheckBatchStockForErrors(req, researchGroupId, cmd);
                //CHECK AND INSERT BATCH STOCK
                AddBatchStock(req, batchId, researchGroupId, cmd);

                //INSERT BATCH OUTPUT IN STOCK
                AddBatchOutputStock(req, batchId, researchGroupId, cmd);

                //SET BATCH COMPLETE TRUE
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.batch 
                        SET is_complete=true
                        WHERE batch_id=:bid;";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd, false);


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
        }

        public static int DiscardBatch(int batchId, int researchGroupId)
         {
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandType = CommandType.Text;

            NpgsqlTransaction t = cmd.Connection.BeginTransaction();

            int res = 0;
            DataTable dt;
            try
            {
                //CHECK BATCH USE
                CheckBatchUse(batchId);


                //REMOVE ALL BATCH CONTENT STOCK AND PROCESSES
                RemoveBatchContent(batchId, researchGroupId, cmd);
                RemoveBatchContentFromStock(batchId, researchGroupId, cmd);
                RemoveBatchOutputFromStock(batchId, researchGroupId, cmd);

                //DELETE FILE ATTACHMENTS
                RemoveBatchDocuments(batchId, cmd);

                //DELETE FROM PROJECT_BATCH
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"DELETE FROM public.project_batch
                                WHERE fk_batch=:bid;";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd, false);

                //CHECK BATCH USE AS A TEMPLATE
                cmd.Parameters.Clear();
                cmd.CommandText =
                            @"SELECT batch_system_label 
                            FROM batch b
                            WHERE b.fk_template=:bid;";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd, false);
                var count = dt.Rows.Count;
                string batch = "";   
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        batch += dr["batch_system_label"].ToString() + " ";
                    }
                    throw new Exception("This batch is used as a template for " + batch);
                }

                //DELETE BATCH GENERAL INFO
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"DELETE FROM public.batch
                                WHERE batch_id=:bid;";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd, false);


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
        }

        //Dodavanje na veke postoecki Batch
        public static int RecreateBatch(int batchId, int researchGroupId)
        {
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandType = CommandType.Text;

            NpgsqlTransaction t = cmd.Connection.BeginTransaction();
            int res = 0;
            double batchOutputWeight;
            int operatorId;
            bool materialQuantityOk = true;
            bool batchQuantityOk = true;

            string notEnoughMessage = "Not enough ";

            BatchExt batch;

            try
            {
                //get the batch
                batch = GetBatchWithContent(batchId, researchGroupId);
                batchOutputWeight = (double)batch.batchOutput;
                operatorId = (int)batch.fkUser;

                List<BatchContentExt> batchContentList = batch.batchContentList;
                if (batchContentList.Count <= 0)
                {
                    throw new Exception("Invalid batch. It should contain at least one material or batch as content.");
                }

                List<string> materials = new List<string>();
                List<string> batches = new List<string>();

                foreach (BatchContentExt batchContent in batchContentList)
                {
                    double wantedQuantity = (double)batchContent.weight;
                    if (batchContent.fkStepMaterial != null)
                    {
                        int stepMaterialId = (int)batchContent.fkStepMaterial;
                        bool result = StockTransactionDa.CheckMaterialStockQuantityEnough(stepMaterialId, wantedQuantity, researchGroupId, cmd);

                        if (result == false)
                        {
                            materialQuantityOk = false;
                            materials.Add(batchContent.materialName);
                        }
                    }
                    else
                    {
                        //if batch
                        int stepBatchId = (int)batchContent.fkStepBatch;
                        bool result = StockTransactionDa.CheckBatchStockQuantityEnough(stepBatchId, wantedQuantity, researchGroupId, cmd);
                        if (result == false)
                        {
                            batchQuantityOk = false;
                            batches.Add(batchContent.batchSystemLabel);
                        }
                    }
                }
                if (materialQuantityOk == false)
                {
                    string materialsString = "";
                    foreach (string m in materials)
                    {
                        if (materials.IndexOf(m) == materials.Count - 1)
                        {
                            materialsString += m;
                        }
                        else
                        {
                            materialsString += m + ", ";
                        }
                    }
                    notEnoughMessage += "Materials: " + materialsString;
                }
                if (batchQuantityOk == false)
                {
                    string batchesString = "";
                    foreach (string b in batches)
                    {
                        if (batches.IndexOf(b) == batches.Count - 1)
                        {
                            batchesString += b;
                        }
                        else
                        {
                            batchesString += b + ", ";
                        }
                    }
                    notEnoughMessage += " Batches: " + batchesString;
                }
                if (!materialQuantityOk || !batchQuantityOk)
                {
                    notEnoughMessage += " in stock!";
                    throw new Exception(notEnoughMessage);
                }
            }
            catch (ValidationException ve)
            {
                throw new Exception(ve.Message);
            }

            try
            {
                //INSERT BATCH CONTENTS STOCK
                foreach (BatchContent batchContent in batch.batchContentList)
                {
                    cmd.Parameters.Clear();
                    if (batchContent.fkStepMaterial != null)
                    {
                        cmd.CommandText =
                        @"INSERT INTO public.stock_transaction (
fk_material,
stock_transaction_element_type,
amount,
fk_operator,
fk_research_group,
fk_experiment_coming,
fk_batch_coming,
transaction_direction,
date_created

)
                    VALUES (
:mid,
1,
:a,
:oid,
:rgid,
:ecomid,
:bcomid,
-1,
now()::timestamp);";

                        Db.CreateParameterFunc(cmd, "@mid", batchContent.fkStepMaterial, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@a", batchContent.weight, NpgsqlDbType.Double);
                        Db.CreateParameterFunc(cmd, "@oid", operatorId, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@ecomid", null, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@bcomid", batchId, NpgsqlDbType.Integer);
                        //Db.CreateParameterFunc(cmd, "@stet", batchContent.fkFunction, NpgsqlDbType.Integer);

                        res = Db.ExecuteNonQuery(cmd, false);
                        if (res <= 0)
                        {
                            //t.Rollback();
                            throw new Exception("Error inserting material stock info");
                        }
                    }
                    else if (batchContent.fkStepBatch != null)
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText =
                        @"INSERT INTO public.stock_transaction (
fk_batch,
stock_transaction_element_type,
amount,
fk_operator,
fk_research_group,
fk_experiment_coming,
fk_batch_coming,
transaction_direction,
date_created

)
                    VALUES (
:bid,
2,
:a,
:oid,
:rgid,
:ecomid,
:bcomid,
-1,
now()::timestamp);";

                        Db.CreateParameterFunc(cmd, "@bid", batchContent.fkStepBatch, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@a", batchContent.weight, NpgsqlDbType.Double);
                        Db.CreateParameterFunc(cmd, "@oid", operatorId, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@ecomid", null, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@bcomid", batchId, NpgsqlDbType.Integer);
                        //Db.CreateParameterFunc(cmd, "@stet", batchContent.fkFunction, NpgsqlDbType.Integer);

                        res = Db.ExecuteNonQuery(cmd, false);
                        if (res <= 0)
                        {
                            //t.Rollback();
                            throw new Exception("Error inserting batch stock info");
                        }
                    }
                }

                //Add batch output weight to stock
                cmd.Parameters.Clear();
                cmd.CommandText =
                        @"INSERT INTO public.stock_transaction (
fk_batch,
stock_transaction_element_type,
amount,
fk_operator,
fk_research_group,
fk_experiment_coming,
fk_batch_coming,
transaction_direction,
date_created

)
                    VALUES (
:bid,
2,
:a,
:oid,
:rgid,
:ecomid,
:bcomid,
1,
now()::timestamp);";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@a", batchOutputWeight, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@oid", operatorId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@ecomid", null, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bcomid", null, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@stet", batchContent.fkFunction, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd, false);
                if (res <= 0)
                {
                    throw new Exception("Error inserting batch stock info");
                }

                t.Commit();
                cmd.Connection.Close();
            }

            catch (Exception ex)
            {
                t.Rollback();
                throw new Exception(ex.Message);
            }

            return 0;
            //return returnedBatchID;
        }

        public static int UpdateBatch(Batch batch)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.batch
                        SET batch_personal_label=:plabel, fk_user=:uid, 
                            batch_output=:bout, fk_measurement_unit=:muid, chemical_formula=:cf, fk_material_type=:mtid,
                            description=:desc, last_change=now()::timestamp
                        WHERE batch_id=:bid;";
                //do not change date created obviously

                Db.CreateParameterFunc(cmd, "@plabel", batch.batchPersonalLabel, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@sl", batch.suggestedLabel, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@uid", batch.fkUser, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bout", batch.batchOutput, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@muid", batch.fkMeasurementUnit, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@cf", batch.chemicalFormula, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@mtid", batch.fkMaterialType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@desc", batch.description, NpgsqlDbType.Date);
                Db.CreateParameterFunc(cmd, "@bid", batch.batchId, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating batch info", ex);
            }
            return 0;
        }
        public static int UpdateBatchGeneralDataAndSetComplete(int batchId, AddBatchRequest req, int researchGroupId, int userId)
        {
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandType = CommandType.Text;

            int res = 0;

            try
            {
                //CHECK BATCH REQUEST
                CheckBatchGeneralInfoForErrors(req, researchGroupId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            try
            {
                //fk_user=:uid,
                //UPDATE BATCH GENERAL INFO
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.batch 
                        SET batch_personal_label=:plabel,  
                            batch_output=:bout, fk_measurement_unit=:muid, chemical_formula=:cf, fk_material_type=:mtid,
                            description=:desc, is_complete=true, last_change=now()::timestamp, fk_edited_by=:uid
                        WHERE batch_id=:bid;";

                Db.CreateParameterFunc(cmd, "@plabel", req.batchInfo.batchPersonalLabel, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@uid", req.batchInfo.fkUser, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bout", req.batchInfo.batchOutput, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@muid", req.batchInfo.fkMeasurementUnit, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@cf", req.batchInfo.chemicalFormula, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@mtid", req.batchInfo.fkMaterialType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@desc", req.batchInfo.description, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@uid", userId, NpgsqlDbType.Integer);

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd);

            }
            catch (Exception ex)
            {
                throw new Exception("Error updating batch info", ex);
            }

            return 0;
        }

        public static int UpdateBatchGeneralData(int batchId, AddBatchRequest req, int researchGroupId)
        {
            //var isComplete = false;
            //if (req.batchInfo.isComplete == true)
            //{
            //    isComplete = true;
            //}

            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandType = CommandType.Text;

            int res = 0;

            try
            {
                //CHECK BATCH REQUEST
                CheckBatchGeneralInfoForErrors(req, researchGroupId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            try
            {
                //fk_user=:uid,
                //UPDATE BATCH GENERAL INFO
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.batch 
                        SET batch_personal_label=:plabel,  
                            batch_output=:bout, fk_measurement_unit=:muid, chemical_formula=:cf, fk_material_type=:mtid,
                            description=:desc, total_batch_output=:tbo, waste_amount=:wa, released_as=:ras, waste_chemical_composition=:wcc, waste_comment=:wc, last_change=now()::timestamp
                        WHERE batch_id=:bid;";

                Db.CreateParameterFunc(cmd, "@plabel", req.batchInfo.batchPersonalLabel, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@uid", req.batchInfo.fkUser, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bout", req.batchInfo.batchOutput, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@muid", req.batchInfo.fkMeasurementUnit, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@cf", req.batchInfo.chemicalFormula, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@mtid", req.batchInfo.fkMaterialType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@desc", req.batchInfo.description, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@tbo", req.batchInfo.totalBatchOutput, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@wa", req.batchInfo.wasteAmount, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@ras", req.batchInfo.releasedAs, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@wcc", req.batchInfo.wasteChemicalComposition, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@wc", req.batchInfo.wasteComment, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@compl", isComplete, NpgsqlDbType.Boolean);

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd);

            }
            catch (Exception ex)
            {
                throw new Exception("Error updating batch info", ex);
            }

            return 0;
        }

        public static int UpdateBatchContent(int batchId, AddBatchRequest req, int researchGroupId)
        {
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandType = CommandType.Text;


            NpgsqlTransaction t = cmd.Connection.BeginTransaction();

            try
            {
                //CHECK BATCH CONTENT BUT NOT AGAINST STOCK
                CheckBatchContentForErrors(req, researchGroupId, cmd);

                //REMOVE BATCH CONTENT
                RemoveBatchContent(batchId, researchGroupId, cmd);

                //INSERT BATCH CONTENT
                AddBatchContent(req, batchId, researchGroupId, cmd);

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
        }

        public static int EditBatch(int batchId, int userId)
        {
            CheckBatchUse(batchId);
            int res = 0;
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            NpgsqlTransaction t = cmd.Connection.BeginTransaction();

            try
            {
                //Mark batch as not complete

                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.batch
                        SET is_complete=false, fk_edited_by=:euid, last_change=now()::timestamp
                         WHERE batch_id=:bid;";

                Db.CreateParameterFunc(cmd, "@euid", userId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd, false);
                if (res <= 0)
                {
                    throw new Exception("Error setting batch to edit mode");
                }

                //remove all batches from the same project
                cmd.Parameters.Clear();
                cmd.CommandText =
                   @"DELETE FROM public.project_batch
                                WHERE fk_coming_batch =:bid;";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                res = Db.ExecuteNonQuery(cmd, false);

                t.Commit();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                t.Rollback();
                throw new Exception(ex.Message);
            }
            return 0;
        }

        public static int CheckBatchUse(int batchId)
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
                            @"SELECT bc.battery_component_id 
                            FROM battery_component bc
                            WHERE bc.fk_step_batch=:sbid;";

                Db.CreateParameterFunc(cmd, "@sbid", batchId, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count > 0)
                {
                    throw new Exception("This batch is in use by some experiment");
                }
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                            @"SELECT b.batch_content_id 
                            FROM batch_content b
                            WHERE b.fk_step_batch=:sbid;";

                Db.CreateParameterFunc(cmd, "@sbid", batchId, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count > 0)
                {
                    throw new Exception("This batch is in use by some other batch");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }

        public static int DeleteBatch(int batchId)
        {
            //DataTable dt;
            try
            {
                var cmd = Db.CreateCommand();
                //                if (cmd.Connection.State != ConnectionState.Open)
                //                {
                //                    cmd.Connection.Open();
                //                }
                //                cmd.CommandText =
                //                            @"SELECT bc.battery_component_id 
                //                            FROM battery_component bc
                //                            WHERE bc.fk_batch=:bid;";

                //                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);

                //                dt = Db.ExecuteSelectCommand(cmd);

                //                if (dt.Rows.Count > 0)
                //                {
                //                    throw new Exception("This batch is in use by some experiment");
                //                }
                //                if (cmd.Connection.State != ConnectionState.Open)
                //                {
                //                    cmd.Connection.Open();
                //                }
                //                cmd.CommandText =
                //                            @"SELECT b.batch_content_id 
                //                            FROM batch_content b
                //                            WHERE b.fk_step_batch=:sbid;";

                //                Db.CreateParameterFunc(cmd, "@sbid", batchId, NpgsqlDbType.Integer);

                //                dt = Db.ExecuteSelectCommand(cmd);

                //                if (dt.Rows.Count > 0)
                //                {
                //                    throw new Exception("This batch is in use by some other batch");
                //                }

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.batch
                                WHERE batch_id=:bid;";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        private static Batch CreateBatchObject(DataRow dr)
        {
            Boolean? hasTestResultsDocVar = (Boolean?)null;
            if (dr.Table.Columns.Contains("has_test_results_doc"))
            {
                hasTestResultsDocVar = dr["has_test_results_doc"] != DBNull.Value ? Boolean.Parse(dr["has_test_results_doc"].ToString()) : false;
            }
            double? totalBatchOutput = (double?)null;
            if (dr.Table.Columns.Contains("total_batch_output"))
            {
                totalBatchOutput = dr["total_batch_output"] != DBNull.Value ? double.Parse(dr["total_batch_output"].ToString()) : (double?)null;
            }
            double? wasteAmount = (double?)null;
            if (dr.Table.Columns.Contains("waste_amount"))
            {
                wasteAmount = dr["waste_amount"] != DBNull.Value ? double.Parse(dr["waste_amount"].ToString()) : (double?)null;
            }
            string releasedAs = null;
            if (dr.Table.Columns.Contains("released_as"))
            {
                releasedAs = dr["released_as"].ToString();
            }
            string wasteChemicalComposition = null;
            if (dr.Table.Columns.Contains("waste_chemical_composition"))
            {
                wasteChemicalComposition = dr["waste_chemical_composition"].ToString();
            }
            string wasteComment = null;
            if (dr.Table.Columns.Contains("waste_comment"))
            {
                wasteComment = dr["waste_comment"].ToString();
            }

            var batch = new Batch
            {
                batchId = (int)dr["batch_id"],
                batchPersonalLabel = dr["batch_personal_label"].ToString(),
                batchSystemLabel = dr["batch_system_label"].ToString(),
                fkUser = dr["fk_user"] != DBNull.Value ? int.Parse(dr["fk_user"].ToString()) : (int?)null,
                description = dr["description"].ToString(),
                batchOutput = dr["batch_output"] != DBNull.Value ? double.Parse(dr["batch_output"].ToString()) : (double?)null,
                fkMeasurementUnit = dr["fk_measurement_unit"] != DBNull.Value ? int.Parse(dr["fk_measurement_unit"].ToString()) : (int?)null,
                chemicalFormula = dr["chemical_formula"].ToString(),
                fkMaterialType = dr["fk_material_type"] != DBNull.Value ? int.Parse(dr["fk_material_type"].ToString()) : (int?)null,
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,
                lastChange = dr["last_change"] != DBNull.Value ? DateTime.Parse(dr["last_change"].ToString()) : (DateTime?)null,
                fkTemplate = dr["fk_template"] != DBNull.Value ? int.Parse(dr["fk_template"].ToString()) : (int?)null,
                isComplete = dr["is_complete"] != DBNull.Value ? Boolean.Parse(dr["is_complete"].ToString()) : false,
                fkEditedBy = dr["fk_edited_by"] != DBNull.Value ? int.Parse(dr["fk_edited_by"].ToString()) : (int?)null,
                fkProject = dr["fk_project"] != DBNull.Value ? int.Parse(dr["fk_project"].ToString()) : (int?)null,
                fkResearchGroup = dr["fk_research_group"] != DBNull.Value ? int.Parse(dr["fk_research_group"].ToString()) : (int?)null,
                hasTestResultsDoc = hasTestResultsDocVar,
                totalBatchOutput = totalBatchOutput,
                wasteAmount = wasteAmount,
                releasedAs = releasedAs,
                wasteChemicalComposition = wasteChemicalComposition,
                wasteComment = wasteComment

            };
            return batch;
        }
        private static DropdownItem CreateDropdownObject(DataRow dr)
        {
            var item = new DropdownItem
            {
                id = (int)dr["batch_id"],
                text = dr["batch_system_label"].ToString() + " | " + dr["batch_personal_label"].ToString(),
            };
            return item;
        }
        private static BatchExt CreateBatchObjectExt(DataRow dr)
        {
            var batch = CreateBatchObject(dr);

            double? availableQuantityVar = (double?)null;
            if (dr.Table.Columns.Contains("available_quantity"))
            {
                availableQuantityVar = dr["available_quantity"] != DBNull.Value ? double.Parse(dr["available_quantity"].ToString()) : (double?)null;
            }
            string operatorUsernameVar = dr.Table.Columns.Contains("operator_username") ? dr["operator_username"].ToString() : null;
            string editingOperatorUsernameVar = dr.Table.Columns.Contains("editing_operator_username") ? dr["editing_operator_username"].ToString() : null;
            string researchGroupVar = dr.Table.Columns.Contains("research_group_name") ? dr["research_group_name"].ToString() : null;
            string researchGroupAcronymVar = dr.Table.Columns.Contains("acronym") ? dr["acronym"].ToString() : null;


            var batchExt = new BatchExt(batch)
            {
                operatorUsername = operatorUsernameVar,
                //operatorResearchGroupName = dr["research_group_name"].ToString(),
                measurementUnitName = dr["measurement_unit_name"].ToString(),
                measurementUnitSymbol = dr["measurement_unit_symbol"].ToString(),
                availableQuantity = availableQuantityVar,
                materialType = dr["material_type"].ToString(),
                editingOperatorUsername = editingOperatorUsernameVar,
                researchGroupName = researchGroupVar,
                researchGroupAcronym = researchGroupAcronymVar,
            };
            return batchExt;
        }

        public static List<Batch> GetDistinctBatchesInProject(int? projectId = null)
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
                    @"SELECT DISTINCT (b.*)
                      FROM project_batch pb
                      LEFT JOIN batch b ON b.batch_id = pb.fk_batch
                      WHERE (pb.fk_project = :pid);";

                Db.CreateParameterFunc(cmd, "@pid", projectId, NpgsqlDbType.Integer);

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

            List<Batch> list = (from DataRow dr in dt.Rows select CreateBatchObject(dr)).ToList();

            return list;
        }

        private static BatchContent CreateBatchContentObject(DataRow dr)
        {
            var batchContent = new BatchContent
            {
                batchContentId = (int)dr["batch_content_id"],
                fkBatch = dr["fk_batch"] != DBNull.Value ? int.Parse(dr["fk_batch"].ToString()) : (int?)null,
                step = dr["step"] != DBNull.Value ? int.Parse(dr["step"].ToString()) : (int?)null,
                fkStepMaterial = dr["fk_step_material"] != DBNull.Value ? int.Parse(dr["fk_step_material"].ToString()) : (int?)null,
                fkStepBatch = dr["fk_step_batch"] != DBNull.Value ? int.Parse(dr["fk_step_batch"].ToString()) : (int?)null,
                weight = dr["weight"] != DBNull.Value ? double.Parse(dr["weight"].ToString()) : (double?)null,
                fkFunction = dr["fk_function"] != DBNull.Value ? int.Parse(dr["fk_function"].ToString()) : (int?)null,
                orderInStep = dr["order_in_step"] != DBNull.Value ? int.Parse(dr["order_in_step"].ToString()) : (int?)null,
                isComplete = dr["is_complete"] != DBNull.Value ? Boolean.Parse(dr["is_complete"].ToString()) : false
            };
            return batchContent;
        }
        private static BatchContentExt CreateBatchContentObjectExt(DataRow dr)
        {
            var batchContent = CreateBatchContentObject(dr);
            var measurementUnitName = "";
            var measurementUnitSymbol = "";
            var chemicalFormula = "";

            if ((dr.Table.Columns.Contains("material_measurement_unit_name")) && dr["material_measurement_unit_name"] != DBNull.Value)
            {
                measurementUnitName = dr["material_measurement_unit_name"].ToString();
            }
            else if ((dr.Table.Columns.Contains("batch_measurement_unit_name")) && dr["batch_measurement_unit_name"] != DBNull.Value)
            {
                measurementUnitName = dr["batch_measurement_unit_name"].ToString();
            }
            if ((dr.Table.Columns.Contains("material_measurement_unit_symbol")) && dr["material_measurement_unit_symbol"] != DBNull.Value)
            {
                measurementUnitSymbol = dr["material_measurement_unit_symbol"].ToString();
            }
            else if ((dr.Table.Columns.Contains("batch_measurement_unit_symbol")) && dr["batch_measurement_unit_symbol"] != DBNull.Value)
            {
                measurementUnitSymbol = dr["batch_measurement_unit_symbol"].ToString();
            }

            if ((dr.Table.Columns.Contains("material_chemical_formula")) && dr["material_chemical_formula"] != DBNull.Value)
            {
                chemicalFormula = dr["material_chemical_formula"].ToString();
            }
            else if ((dr.Table.Columns.Contains("batch_chemical_formula")) && dr["batch_chemical_formula"] != DBNull.Value)
            {
                chemicalFormula = dr["batch_chemical_formula"].ToString();
            }

            var batchContentExt = new BatchContentExt(batchContent)
            {
                materialName = dr["material_name"].ToString(),
                batchSystemLabel = dr["batch_system_label"].ToString(),
                batchPersonalLabel = dr["batch_personal_label"].ToString(),
                measurementUnitName = measurementUnitName,
                measurementUnitSymbol = measurementUnitSymbol,
                //measurementUnitName = dr["measurement_unit_name"].ToString(),
                //measurementUnitSymbol = dr["measurement_unit_symbol"].ToString(),
                chemicalFormula = chemicalFormula
                //chemicalFormula = dr["chemical_formula"].ToString()
            };
            return batchContentExt;
        }


        public static BatchProcess CreateBatchProcessObject(DataRow dr)
        {
            var batchProcess = new BatchProcess
            {
                batchProcessId = (int)dr["batch_process_id"],
                fkBatch = dr["fk_batch"] != DBNull.Value ? int.Parse(dr["fk_batch"].ToString()) : (int?)null,
                step = dr["step"] != DBNull.Value ? int.Parse(dr["step"].ToString()) : (int?)null,
                fkProcess = dr["fk_process"] != DBNull.Value ? int.Parse(dr["fk_process"].ToString()) : (int?)null,
                processOrderInStep = dr["process_order_in_step"] != DBNull.Value ? int.Parse(dr["process_order_in_step"].ToString()) : (int?)null
            };
            return batchProcess;
        }
        private static BatchProcessExt CreateBatchProcessObjectExt(DataRow dr)
        {
            var batchProcess = CreateBatchProcessObject(dr);

            var batchProcessExt = new BatchProcessExt(batchProcess)
            {
                //operatorUsername = dr["username"].ToString(),
                processType = dr["process_type"].ToString(),
            };
            return batchProcessExt;
        }
    }
}