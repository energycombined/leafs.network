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
    public class BatchContentDa
    {
        //        public static List<BatchContentExt> GetAllBatchContents(int? batchId=null, int? stepId = null, int? batchContentId = null)
        //        {
        //            DataTable dt;

        //            try
        //            {
        //                var cmd = Db.CreateCommand();
        //                if (cmd.Connection.State != ConnectionState.Open)
        //                {
        //                    cmd.Connection.Open();
        //                }
        //                cmd.CommandText =
        //                    @"SELECT *
        //                    FROM batch_content bc
        //                        left join batch b on bc.fk_batch = b.batch_id
        //                        left join material m on bc.fk_step_material = m.material_id
        //                        left join batch bat on bc.fk_step_batch = bat.batch_id
        //                        left join material_function f on bc.fk_function = f.material_function_id                        
        //
        //                    WHERE (bc.fk_batch = :bid or :bid is null) and
        //                        (bc.step = :sid or :sid is null) and
        //                        (bc.batch_content_id = :bcid or :bcid is null);";

        //                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
        //                Db.CreateParameterFunc(cmd, "@sid", stepId, NpgsqlDbType.Integer);
        //                Db.CreateParameterFunc(cmd, "@bcid", batchContentId, NpgsqlDbType.Integer);

        //                dt = Db.ExecuteSelectCommand(cmd);
        //            }
        //            catch (Exception ex)
        //            {
        //                throw new Exception(ex.Message);
        //            }

        //            if (dt == null || dt.Rows.Count == 0)
        //            {
        //                return null;
        //            }

        //            List<BatchContentExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

        //            return list;
        //        }


        public static List<BatchContentExt> GetAllBatchContents(int? batchId = null, int? stepId = null, long? batchContentId = null)
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
                    @"SELECT *, mitm.measurement_unit_name as material_measurement_unit_name,
                        mitm.measurement_unit_symbol as material_measurement_unit_symbol,
                        mitb.measurement_unit_name as batch_measurement_unit_name,
                        mitb.measurement_unit_symbol as batch_measurement_unit_symbol,
                        m.chemical_formula as material_chemical_formula,
                        bat.chemical_formula as batch_chemical_formula,

                        bat.description as batch_description,
                        m.description as material_description,
                        m.price as material_price,
                        m.bulk_price as material_bulk_price

                    FROM batch_content bc
                        LEFT JOIN material m on bc.fk_step_material = m.material_id
                        LEFT JOIN batch bat on bc.fk_step_batch = bat.batch_id

                        LEFT JOIN material_function mf on bc.fk_function = mf.material_function_id

                        LEFT JOIN measurement_unit mitm on m.fk_measurement_unit = mitm.measurement_unit_id
                        LEFT JOIN measurement_unit mitb on bat.fk_measurement_unit = mitb.measurement_unit_id
                        LEFT JOIN measurements meas on bc.batch_content_id = meas.fk_batch_content

                    WHERE (bc.fk_batch = :bid or :bid is null) and
                        (bc.step = :sid or :sid is null) and
                        (bc.batch_content_id = :bcid or :bcid is null)

                    ORDER BY bc.order_in_step;";

                //and                                     bc.is_complete = true

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@sid", stepId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bcid", batchContentId, NpgsqlDbType.Bigint);

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

            List<BatchContentExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        //public static double GetTotalBatchWeightRecursive(double totalWeight, int batchId, double percentageUsed)
        //{
        //    //first call with percentageUsed 1.0
        //    List<BatchContentExt> contentList = GetAllBatchContents(batchId);

        //    foreach (BatchContentExt e in contentList)
        //    {
        //        double usedWeight = double.Parse(e.weight.ToString());
        //        //e.weight = e.weight * percentageUsed;

        //        if(e.fkStepMaterial != null)
        //            totalWeight += usedWeight * percentageUsed;

        //        else
        //        {
        //            double batchOutput = GetTotalBatchWeightRecursive(totalWeight, (int)e.fkStepBatch, usedWeight);
        //            double pU = (usedWeight / batchOutput);

        //            GetTotalBatchWeightRecursive(totalWeight, (int)e.fkStepBatch, pU);
        //        }
        //    }

        //    return totalWeight;
        //}

        public static List<BatchContentExt> GetAllContentInBatchRecursive(List<BatchContentExt> list, int batchId, double percentageUsed, int[] ignoredMaterialFunctionsArray)
        {
            List<BatchContentExt> finalList = new List<BatchContentExt>();
            List<BatchContentExt> contentList = GetAllBatchContents(batchId);
            //list.AddRange(contentList);

            foreach (BatchContentExt e in contentList)
            {
                double usedWeight = double.Parse(e.weight.ToString());
                e.weight = (e.weight * percentageUsed);

                list.Add(e);


                if (e.fkStepBatch != null)
                {
                    usedWeight = usedWeight * percentageUsed;
                    //double batchOutput = double.Parse(e.batchOutput.ToString());
                    double batchTotalWeight = GetBatchTotalWeight((int)e.fkStepBatch, ignoredMaterialFunctionsArray);
                    double pU = batchTotalWeight > 0 ? (usedWeight / batchTotalWeight) : 0;

                    GetAllContentInBatchRecursive(list, (int)e.fkStepBatch, pU, ignoredMaterialFunctionsArray);
                }
            }

            return list;
        }

        public static List<BatchContentExt> GetAllContentInBatchRecursive(List<BatchContentExt> list, int batchId)
        {
            List<BatchContentExt> contentList = GetAllBatchContents(batchId);
            foreach (BatchContentExt e in contentList)
            {
                list.Add(e);
                if (e.fkStepBatch != null)
                {
                    GetAllContentInBatchRecursive(list, (int)e.fkStepBatch);
                }
            }
            return list;
        }

        //public static double GetBatchTotalWeight(int batchId, int[] ignoredMaterialFunctionsArray)
        //{
        //    //int[] ignoredMaterialFunctionsArray = Helpers.GeneralHelper.GetMaterialFunctionsToIgnore();

        //    double total = 0;
        //    List<BatchContentExt> contentList = GetAllBatchContents(batchId);

        //    foreach (BatchContentExt e in contentList)
        //    {
        //        if (e.fkStepBatch != null)
        //        {
        //            total += GetBatchTotalWeight((int)e.fkStepBatch, ignoredMaterialFunctionsArray);
        //        }
        //        else
        //        {
        //            if (e.fkFunction != null && !ignoredMaterialFunctionsArray.Contains((int)e.fkFunction))
        //            {
        //                total += double.Parse(e.weight.ToString());
        //            }
        //        }
        //    }
        //    return total;
        //}

        public static double SumBatchTotalWeight(List<BatchContentExt> contentList, int[] ignoredMaterialFunctionsArray)
        {
            double total = 0;
            foreach (BatchContentExt e in contentList)
            {
                if (e.fkStepBatch == null)
                {
                    if (e.fkFunction != null && !ignoredMaterialFunctionsArray.Contains((int)e.fkFunction))
                    {
                        total += double.Parse(e.weight.ToString());
                    }
                }
            }
            return total;

        }
        //public static double GetBatchTotalWeight(int batchId, int[] ignoredMaterialFunctionsArray)
        //{
        //    double total = 0;
        //    List<BatchContentExt> contentList = GetAllBatchContents(batchId);
        //    total += SumBatchTotalWeight(contentList, ignoredMaterialFunctionsArray);            

        //    foreach (BatchContentExt e in contentList)
        //    {
        //        if (e.fkStepBatch != null)
        //        {
        //            double usedWeight = double.Parse(e.weight.ToString());
        //            double batchTotalWeight = GetBatchTotalWeight((int)e.fkStepBatch, ignoredMaterialFunctionsArray);
        //            double percentageUsed = (usedWeight / batchTotalWeight);

        //            List<BatchContentExt> list = new List<BatchContentExt>();
        //            GetAllContentInBatchRecursive(list, (int)e.fkStepBatch, percentageUsed, ignoredMaterialFunctionsArray);

        //            total += SumBatchTotalWeight(list, ignoredMaterialFunctionsArray);
        //        }
        //        //else
        //        //{
        //        //    if (e.fkFunction != null && !ignoredMaterialFunctionsArray.Contains((int)e.fkFunction))
        //        //    {
        //        //        total += double.Parse(e.weight.ToString());
        //        //    }
        //        //}
        //    }
        //    return total;
        //}

        //public static double GetBatchTotalWeight(int batchId, int[] ignoredMaterialFunctionsArray)
        //{
        //    double total = 0;
        //    List<BatchContentExt> contentList = GetAllBatchContents(batchId);

        //    foreach (BatchContentExt e in contentList)
        //    {
        //        if (e.fkStepBatch != null)
        //        {

        //            total += double.Parse(e.weight.ToString());
        //            //total += GetBatchTotalWeight((int)e.fkStepBatch, ignoredMaterialFunctionsArray);
        //        }
        //        else
        //        {
        //            if (e.fkFunction != null && !ignoredMaterialFunctionsArray.Contains((int)e.fkFunction))
        //            {
        //                total += double.Parse(e.weight.ToString());
        //            }
        //        }
        //    }
        //    return total;
        //}

        //NOVA
        public static double GetBatchTotalWeight(int batchId, int[] ignoredMaterialFunctionsArray)
        {
            double total = 0;
            List<dynamic> allContent = new List<dynamic>();
            List<BatchContentExt> contentList = GetAllBatchContents(batchId);

            //izvadi contents

            foreach (BatchContentExt e in contentList)
            {
                List<BatchContentExt> list = new List<BatchContentExt>();
                allContent.Add(e);

                if (e.fkStepBatch != null)
                {
                    double usedWeight = double.Parse(e.weight.ToString());
                    double batchTotalWeight = GetBatchTotalWeight((int)e.fkStepBatch, ignoredMaterialFunctionsArray);
                    double percentageUsed = batchTotalWeight > 0 ? (usedWeight / batchTotalWeight) : 0;

                    List<BatchContentExt> allBatchContent = BatchContentDa.GetAllContentInBatchRecursive(list, (int)e.fkStepBatch, percentageUsed, ignoredMaterialFunctionsArray);
                    foreach (BatchContentExt batc in allBatchContent)
                    {
                        allContent.Add(batc);
                    }
                }
            }


            //istrcaj soberi


            foreach (BatchContentExt e in allContent)
            {
                if (e.fkStepBatch == null)
                {
                    if (e.fkFunction != null && !ignoredMaterialFunctionsArray.Contains((int)e.fkFunction))
                    {
                        total += double.Parse(e.weight.ToString());
                    }
                }
            }
            return total;
        }


        public static int AddBatchContent(BatchContent batchContent)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"INSERT INTO public.batch_content (fk_batch, step, fk_step_material, fk_step_batch, weight, fk_function, order_in_step)
                    VALUES (:bid, :step, :sm, :sb, :w, :f, :ois);";

                Db.CreateParameterFunc(cmd, "@bid", batchContent.fkBatch, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@step", batchContent.step, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@sm", batchContent.fkStepMaterial, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@sb", batchContent.fkStepBatch, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@w", batchContent.weight, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@f", batchContent.fkFunction, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@ois", batchContent.orderInStep, NpgsqlDbType.Integer);
                //is complete default false

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting batch content", ex);
            }

            return 0;
        }
        public static int UpdateBatchContent(BatchContent batchContent)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.batch_content
                        SET fk_batch=:bid, step=:step, fk_step_material=:sm, fk_step_batch=:sb, weight=:w, fk_function=:f, order_in_step=:ois
                        WHERE batch_content_id=:bcid;";
                //last_change=now()::timestamp
                Db.CreateParameterFunc(cmd, "@bid", batchContent.fkBatch, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@step", batchContent.step, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@sm", batchContent.fkStepMaterial, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@sb", batchContent.fkStepBatch, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@w", batchContent.weight, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@f", batchContent.fkFunction, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@ois", batchContent.orderInStep, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bcid", batchContent.batchContentId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating batch content info", ex);
            }
            return 0;
        }
        public static int DeleteBatchContent(int batchId)
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
                    @"DELETE FROM public.batch_content
                                WHERE fk_batch=:bid;";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        private static BatchContent CreateObject(DataRow dr)
        {
            var batchContent = new BatchContent
            {
                batchContentId = (long)dr["batch_content_id"],
                fkBatch = dr["fk_batch"] != DBNull.Value ? int.Parse(dr["fk_batch"].ToString()) : (int?)null,
                step = dr["step"] != DBNull.Value ? int.Parse(dr["step"].ToString()) : (int?)null,
                fkStepMaterial = dr["fk_step_material"] != DBNull.Value ? long.Parse(dr["fk_step_material"].ToString()) : (long?)null,
                fkStepBatch = dr["fk_step_batch"] != DBNull.Value ? int.Parse(dr["fk_step_batch"].ToString()) : (int?)null,
                weight = dr["weight"] != DBNull.Value ? double.Parse(dr["weight"].ToString()) : (double?)null,
                fkFunction = dr["fk_function"] != DBNull.Value ? int.Parse(dr["fk_function"].ToString()) : (int?)null,
                orderInStep = dr["order_in_step"] != DBNull.Value ? int.Parse(dr["order_in_step"].ToString()) : (int?)null,
                isComplete = dr["is_complete"] != DBNull.Value ? Boolean.Parse(dr["is_complete"].ToString()) : false,
                percentageOfActive = dr["percentage_of_active"] != DBNull.Value ? double.Parse(dr["percentage_of_active"].ToString()) : (double?)null
            };
            return batchContent;
        }
        private static BatchContentExt CreateObjectExt(DataRow dr)
        {
            var batchContent = CreateObject(dr);

            var measurementUnitName = "";
            var measurementUnitSymbol = "";
            var chemicalFormula = "";

            var materialLabel = "";
            var description = "";
            var storedInType = "";
            var price = "";
            var bulkPrice = "";

            string materialFunctionVar = dr.Table.Columns.Contains("material_function_name") ? dr["material_function_name"].ToString() : null;

            double? batchOutputVar = (double?)null;
            if (dr.Table.Columns.Contains("batch_output"))
            {
                batchOutputVar = dr["batch_output"] != DBNull.Value ? double.Parse(dr["batch_output"].ToString()) : (double?)null;
            }

            MeasurementsExt measurements = new MeasurementsExt();
            if ((dr.Table.Columns.Contains("fk_battery_component_content")) && dr["fk_battery_component_content"] != DBNull.Value)
            {
                measurements.fkBatteryComponentContent = dr["fk_battery_component_content"] != DBNull.Value ? long.Parse(dr["fk_battery_component_content"].ToString()) : (long?)null;
            }
            if ((dr.Table.Columns.Contains("fk_measurement_level_type")) && dr["fk_measurement_level_type"] != DBNull.Value)
            {
                measurements.fkMeasurementLevelType = dr["fk_measurement_level_type"] != DBNull.Value ? int.Parse(dr["fk_measurement_level_type"].ToString()) : (int?)null;
            }

            if ((dr.Table.Columns.Contains("measured_time")) && dr["measured_time"] != DBNull.Value)
            {
                measurements.measuredTime = dr["measured_time"] != DBNull.Value ? double.Parse(dr["measured_time"].ToString()) : (double?)null;
            }
            if ((dr.Table.Columns.Contains("measured_width")) && dr["measured_width"] != DBNull.Value)
            {
                measurements.measuredWidth = dr["measured_width"] != DBNull.Value ? double.Parse(dr["measured_width"].ToString()) : (double?)null;
            }
            if ((dr.Table.Columns.Contains("measured_length")) && dr["measured_length"] != DBNull.Value)
            {
                measurements.measuredLength = dr["measured_length"] != DBNull.Value ? double.Parse(dr["measured_length"].ToString()) : (double?)null;
            }
            if ((dr.Table.Columns.Contains("measured_conductivity")) && dr["measured_conductivity"] != DBNull.Value)
            {
                measurements.measuredConductivity = dr["measured_conductivity"] != DBNull.Value ? double.Parse(dr["measured_conductivity"].ToString()) : (double?)null;
            }
            if ((dr.Table.Columns.Contains("measured_thickness")) && dr["measured_thickness"] != DBNull.Value)
            {
                measurements.measuredThickness = dr["measured_thickness"] != DBNull.Value ? double.Parse(dr["measured_thickness"].ToString()) : (double?)null;
            }
            if ((dr.Table.Columns.Contains("measured_weight")) && dr["measured_weight"] != DBNull.Value)
            {
                measurements.measuredWeight = dr["measured_weight"] != DBNull.Value ? double.Parse(dr["measured_weight"].ToString()) : (double?)null;
            }

            if (measurements.measuredTime == null &&
                measurements.measuredWidth == null &&
                measurements.measuredLength == null &&
                measurements.measuredConductivity == null &&
                measurements.measuredThickness == null &&
                measurements.measuredWeight == null)
                measurements = null;


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






            if ((dr.Table.Columns.Contains("material_label")) && dr["material_label"] != DBNull.Value)
            {
                materialLabel = dr["material_label"].ToString();
            }

            if ((dr.Table.Columns.Contains("batch_description")) && dr["batch_description"] != DBNull.Value)
            {
                description = dr["batch_description"].ToString();
            }
            else if ((dr.Table.Columns.Contains("material_description")) && dr["material_description"] != DBNull.Value)
            {
                description = dr["material_description"].ToString();
            }

            if ((dr.Table.Columns.Contains("material_stored_in_type")) && dr["material_stored_in_type"] != DBNull.Value)
            {
                storedInType = dr["material_stored_in_type"].ToString();
            }

            if ((dr.Table.Columns.Contains("material_price")) && dr["material_price"] != DBNull.Value)
            {
                price = dr["material_price"].ToString();
            }

            if ((dr.Table.Columns.Contains("material_bulk_price")) && dr["material_bulk_price"] != DBNull.Value)
            {
                bulkPrice = dr["material_bulk_price"].ToString();
            }

            var batchContentExt = new BatchContentExt(batchContent)
            {
                materialName = dr["material_name"].ToString(),
                batchSystemLabel = dr["batch_system_label"].ToString(),
                batchPersonalLabel = dr["batch_personal_label"].ToString(),
                measurementUnitName = measurementUnitName,
                measurementUnitSymbol = measurementUnitSymbol,
                chemicalFormula = chemicalFormula,
                measurements = measurements,

                materialLabel = materialLabel,
                description = description,
                //storedInType = storedInType,
                price = price,
                bulkPrice = bulkPrice,

                materialFunction = materialFunctionVar,
                batchOutput = batchOutputVar
            };
            return batchContentExt;
        }
    }
}