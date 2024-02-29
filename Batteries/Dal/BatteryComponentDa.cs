using Batteries.Dal.Base;
using Batteries.Helpers;
using Batteries.Models;
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
    public class BatteryComponentDa
    {
        public static List<BatteryComponentExt> GetAllBatteryComponents(long? batteryComponentId = null, int? experimentId = null, int? batteryComponentTypeId = null, int? stepId = null)
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
                        sit.stored_in_type as material_stored_in_type,
                        m.price as material_price,
                        m.bulk_price as material_bulk_price


                              FROM battery_component bc
LEFT JOIN battery_component_type bct on bc.fk_battery_component_type = bct.battery_component_type_id
                                LEFT JOIN material m on bc.fk_step_material = m.material_id
                                LEFT JOIN batch bat on bc.fk_step_batch = bat.batch_id
                                LEFT JOIN material_type mt on bc.fk_material_type = mt.material_type_id
                                LEFT JOIN material_function mf on bc.fk_function = mf.material_function_id
                                LEFT JOIN stored_in_type sit on bc.fk_stored_in_type = sit.stored_in_type_id

                                LEFT JOIN measurement_unit mitm on m.fk_measurement_unit = mitm.measurement_unit_id
                                LEFT JOIN measurement_unit mitb on bat.fk_measurement_unit = mitb.measurement_unit_id
                                LEFT JOIN measurements meas on bc.battery_component_id = meas.fk_battery_component_content

                              WHERE (bc.fk_experiment = :eid or :eid is null) and
                                    (bc.fk_battery_component_type = :bctid or :bctid is null) and
                                    (bc.step = :sid or :sid is null) and
                                    (bc.battery_component_id = :bcid or :bcid is null)
                                    ORDER BY bc.fk_battery_component_type, step, order_in_step;";

                //and                                     bc.is_complete = true

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bctid", batteryComponentTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@sid", stepId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bcid", batteryComponentId, NpgsqlDbType.Bigint);

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

            List<BatteryComponentExt> list = (from DataRow dr in dt.Rows select CreateBatteryComponentObjectExt(dr)).ToList();

            return list;
        }

        //        public static List<BatteryComponentExt> GetAllMaterialsInExperiment(int? experimentId = null, int? batteryComponentTypeId = null, int? stepId = null, int? batteryComponentId = null)
        //        {
        //            //ne raboti zatoa sto fali batch za kreiranje na objektot, nema proverki
        //            DataTable dt;
        //            try
        //            {
        //                var cmd = Db.CreateCommand();
        //                if (cmd.Connection.State != ConnectionState.Open)
        //                {
        //                    cmd.Connection.Open();
        //                }
        //                cmd.CommandText =
        //                    @"SELECT *,
        //                        mitm.measurement_unit_name as material_measurement_unit_name,
        //                        mitm.measurement_unit_symbol as material_measurement_unit_symbol,
        //                        m.chemical_formula as material_chemical_formula,
        //                        
        //                        m.description as material_description,
        //                        sit.stored_in_type as material_stored_in_type,
        //                        m.price as material_price,
        //                        m.bulk_price as material_bulk_price
        //
        //
        //                              FROM battery_component bc
        //                                LEFT JOIN material m on bc.fk_step_material = m.material_id                                
        //                                LEFT JOIN material_type mt on bc.fk_material_type = mt.material_type_id
        //                                LEFT JOIN material_function mf on bc.fk_function = mf.material_function_id
        //                                LEFT JOIN stored_in_type sit on bc.fk_stored_in_type = sit.stored_in_type_id
        //
        //                                LEFT JOIN measurement_unit mitm on m.fk_measurement_unit = mitm.measurement_unit_id
        //                                LEFT JOIN measurements meas on bc.battery_component_id = meas.fk_battery_component_content
        //                                
        //                                LEFT JOIN experiment e on bc.fk_experiment = e.experiment_id
        //
        //                              WHERE (bc.fk_experiment = :eid or :eid is null) and
        //                                    (e.is_complete = true) and
        //                                    (bc.fk_battery_component_type = :bctid or :bctid is null) and
        //                                    (bc.step = :sid or :sid is null) and
        //                                    (bc.battery_component_id = :bcid or :bcid is null)
        //                                    ORDER BY bc.fk_battery_component_type, step, order_in_step;";

        //                //and                                     bc.is_complete = true


        //                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
        //                Db.CreateParameterFunc(cmd, "@bctid", batteryComponentTypeId, NpgsqlDbType.Integer);
        //                Db.CreateParameterFunc(cmd, "@sid", stepId, NpgsqlDbType.Integer);
        //                Db.CreateParameterFunc(cmd, "@bcid", batteryComponentId, NpgsqlDbType.Integer);

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

        //            List<BatteryComponentExt> list = (from DataRow dr in dt.Rows select CreateBatteryComponentObjectExt(dr)).ToList();

        //            return list;
        //        }        

        public static List<BatteryComponentExt> GetAllContentInExperiment(int? experimentId = null, int? batteryComponentTypeId = null, int? stepId = null, long? batteryComponentId = null)
        {
            //experiment is complete = TRUE

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
                        sit.stored_in_type as material_stored_in_type,
                        m.price as material_price,
                        m.bulk_price as material_bulk_price


                              FROM battery_component bc
LEFT JOIN battery_component_type bct on bc.fk_battery_component_type = bct.battery_component_type_id
                                LEFT JOIN material m on bc.fk_step_material = m.material_id
                                LEFT JOIN batch bat on bc.fk_step_batch = bat.batch_id
                                LEFT JOIN material_type mt on bc.fk_material_type = mt.material_type_id
                                LEFT JOIN material_function mf on bc.fk_function = mf.material_function_id
                                LEFT JOIN stored_in_type sit on bc.fk_stored_in_type = sit.stored_in_type_id

                                LEFT JOIN measurement_unit mitm on m.fk_measurement_unit = mitm.measurement_unit_id
                                LEFT JOIN measurement_unit mitb on bat.fk_measurement_unit = mitb.measurement_unit_id
                                LEFT JOIN measurements meas on bc.battery_component_id = meas.fk_battery_component_content

                                LEFT JOIN experiment e on bc.fk_experiment = e.experiment_id

                              WHERE 
                                    (bc.fk_experiment = :eid or :eid is null) and
                                    (e.is_complete = true) and
                                    (bc.fk_battery_component_type = :bctid or :bctid is null) and
                                    (bc.step = :sid or :sid is null) and
                                    (bc.battery_component_id = :bcid or :bcid is null)
                                    ORDER BY bc.fk_experiment, bc.fk_battery_component_type, step, order_in_step;";

                //and                                     bc.is_complete = true


                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bctid", batteryComponentTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@sid", stepId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bcid", batteryComponentId, NpgsqlDbType.Bigint);

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

            List<BatteryComponentExt> list = (from DataRow dr in dt.Rows select CreateBatteryComponentObjectExt(dr)).ToList();

            return list;
        }


        //public static List<BatteryComponentExt> GetBatchesInExperiment(int experimentId)
        //{
        //    DataTable dt;
        //    try
        //    {
        //        var cmd = Db.CreateCommand();
        //        if (cmd.Connection.State != ConnectionState.Open)
        //        {
        //            cmd.Connection.Open();
        //        }
        //        cmd.CommandText =
        //               @"SELECT DISTINCT bc.fk_experiment, bc.fk_step_batch, pr.project_id
        //                 FROM public.battery_component bc

        //                 LEFT JOIN experiment e ON bc.fk_experiment = e.experiment_id
        //                 LEFT JOIN project pr ON e.fk_project = pr.project_id                         

        //                 WHERE bc.fk_experiment = :eid
        //        ORDER BY fk_experiment DESC;";

        //        Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);

        //        dt = Db.ExecuteSelectCommand(cmd);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    List<int> experimentBatchList = new List<int>();

        //    if (dt == null || dt.Rows.Count == 0)
        //    {
        //        return null;
        //    }

        //    List<BatteryComponentExt> list = (from DataRow dr in dt.Rows select CreateBatteryComponentObjectExt(dr)).ToList();

        //    return list;
        //}

        /// <summary>
        /// Get all content in all completed experiments
        /// </summary>
        /// <param name="researchGroupIdCreator"></param>
        /// <param name="experimentId"></param>
        /// <param name="batteryComponentTypeId"></param>
        /// <param name="stepId"></param>
        /// <param name="batteryComponentId"></param>
        /// <returns></returns>
        //not used, query OK
        public static List<BatteryComponentExt> GetAllContentInExperimentByResearchGroupCreator(int? researchGroupIdCreator = null, int? experimentId = null, int? batteryComponentTypeId = null, int? stepId = null, long? batteryComponentId = null)
        {
            //experiment is complete = TRUE

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
                        sit.stored_in_type as material_stored_in_type,
                        m.price as material_price,
                        m.bulk_price as material_bulk_price

                     FROM battery_component bc
                       LEFT JOIN battery_component_type bct on bc.fk_battery_component_type = bct.battery_component_type_id
                       LEFT JOIN material m on bc.fk_step_material = m.material_id
                       LEFT JOIN batch bat on bc.fk_step_batch = bat.batch_id
                       LEFT JOIN material_type mt on bc.fk_material_type = mt.material_type_id
                       LEFT JOIN material_function mf on bc.fk_function = mf.material_function_id
                       LEFT JOIN stored_in_type sit on bc.fk_stored_in_type = sit.stored_in_type_id

                       LEFT JOIN measurement_unit mitm on m.fk_measurement_unit = mitm.measurement_unit_id
                       LEFT JOIN measurement_unit mitb on bat.fk_measurement_unit = mitb.measurement_unit_id
                       LEFT JOIN measurements meas on bc.battery_component_id = meas.fk_battery_component_content

                       LEFT JOIN experiment e on bc.fk_experiment = e.experiment_id
                       --LEFT JOIN project_research_group p on e.fk_project = p.fk_project

                     WHERE (e.fk_research_group = :rgid or :rgid is null) and
                           (bc.fk_experiment = :eid or :eid is null) and
                           (e.is_complete = true) and
                           (bc.fk_battery_component_type = :bctid or :bctid is null) and
                           (bc.step = :sid or :sid is null) and
                           (bc.battery_component_id = :bcid or :bcid is null)
                           ORDER BY bc.fk_experiment, bc.fk_battery_component_type, step, order_in_step;";

                //and                                     bc.is_complete = true


                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bctid", batteryComponentTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@sid", stepId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bcid", batteryComponentId, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupIdCreator, NpgsqlDbType.Integer);

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

            List<BatteryComponentExt> list = (from DataRow dr in dt.Rows select CreateBatteryComponentObjectExt(dr)).ToList();

            return list;
        }

        /// <summary>
        /// Get all content in all completed experiments filtered by what the researchGroupId can see
        /// </summary>
        /// <param name="researchGroupIdCreator"></param>
        /// <param name="experimentId"></param>
        /// <param name="batteryComponentTypeId"></param>
        /// <param name="stepId"></param>
        /// <param name="batteryComponentId"></param>
        /// <returns></returns>
        public static List<BatteryComponentExt> GetAllContentInExperimentByResearchGroup(int researchGroupId, int? researchGroupIdCreator = null, int? experimentId = null, int? batteryComponentTypeId = null, int? stepId = null, long? batteryComponentId = null)
        {
            //experiment is complete = TRUE

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
                        sit.stored_in_type as material_stored_in_type,
                        m.price as material_price,
                        m.bulk_price as material_bulk_price

                     FROM battery_component bc
                       LEFT JOIN battery_component_type bct on bc.fk_battery_component_type = bct.battery_component_type_id
                       LEFT JOIN material m on bc.fk_step_material = m.material_id
                       LEFT JOIN batch bat on bc.fk_step_batch = bat.batch_id
                       LEFT JOIN material_type mt on bc.fk_material_type = mt.material_type_id
                       LEFT JOIN material_function mf on bc.fk_function = mf.material_function_id
                       LEFT JOIN stored_in_type sit on bc.fk_stored_in_type = sit.stored_in_type_id

                       LEFT JOIN measurement_unit mitm on m.fk_measurement_unit = mitm.measurement_unit_id
                       LEFT JOIN measurement_unit mitb on bat.fk_measurement_unit = mitb.measurement_unit_id
                       LEFT JOIN measurements meas on bc.battery_component_id = meas.fk_battery_component_content

                       LEFT JOIN experiment e on bc.fk_experiment = e.experiment_id
                       LEFT JOIN project_research_group p on e.fk_project = p.fk_project

                     WHERE (p.fk_research_group = :rgid) and
                           (e.fk_research_group = :rgidCreator or :rgidCreator is null) and
                           (bc.fk_experiment = :eid or :eid is null) and
                           (e.is_complete = true) and
                           (bc.fk_battery_component_type = :bctid or :bctid is null) and
                           (bc.step = :sid or :sid is null) and
                           (bc.battery_component_id = :bcid or :bcid is null)
                           ORDER BY bc.fk_experiment, bc.fk_battery_component_type, step, order_in_step;";

                //and                                     bc.is_complete = true


                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgidCreator", researchGroupIdCreator, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bctid", batteryComponentTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@sid", stepId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bcid", batteryComponentId, NpgsqlDbType.Bigint);

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

            List<BatteryComponentExt> list = (from DataRow dr in dt.Rows select CreateBatteryComponentObjectExt(dr)).ToList();

            return list;
        }


        //webmethod actually NOT USED 
        public static List<BatteryComponentExt> GetAllMaterialsInExperimentForCsv(int? experimentId = null, long? batteryComponentTypeId = null, int? stepId = null)
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
                    @"SELECT bc.*, m.*, bat.*, bc.fk_experiment, battery_component_type, (bc.weight * m.price) as bill,
                        bc.weight,
                        m.material_name,
                        
                        m.chemical_formula as material_chemical_formula,
                        mitm.measurement_unit_name as material_measurement_unit_name,
                        mitm.measurement_unit_symbol as material_measurement_unit_symbol,
                        
                        
                        
                        bat.batch_system_label,
                        bat.chemical_formula as batch_chemical_formula,
                        mitb.measurement_unit_name as batch_measurement_unit_name,
                        mitb.measurement_unit_symbol as batch_measurement_unit_symbol,
                        
                        bat.description as batch_description,
                        m.description as material_description,
                        sit.stored_in_type as material_stored_in_type,
                        m.price as material_price,
                        m.bulk_price as material_bulk_price,
                        meas.*


                              FROM battery_component bc
                                LEFT JOIN battery_component_type bct on bc.fk_battery_component_type = bct.battery_component_type_id

                                LEFT JOIN material m on bc.fk_step_material = m.material_id
                                LEFT JOIN batch bat on bc.fk_step_batch = bat.batch_id
                                LEFT JOIN material_type mt on bc.fk_material_type = mt.material_type_id
                                LEFT JOIN material_function mf on bc.fk_function = mf.material_function_id
                                LEFT JOIN stored_in_type sit on bc.fk_stored_in_type = sit.stored_in_type_id

                                LEFT JOIN measurement_unit mitm on m.fk_measurement_unit = mitm.measurement_unit_id
                                LEFT JOIN measurement_unit mitb on bat.fk_measurement_unit = mitb.measurement_unit_id
                                LEFT JOIN measurements meas on bc.battery_component_id = meas.fk_battery_component_content


                              WHERE (bc.fk_experiment = :eid or :eid is null) and
                                    (bc.fk_battery_component_type = :bctid or :bctid is null) and
                                    (bc.step = :sid or :sid is null) and
                                bc.is_complete = true
                                    ORDER BY bc.fk_experiment, bc.fk_battery_component_type, step, order_in_step;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bctid", batteryComponentTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@sid", stepId, NpgsqlDbType.Integer);

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

            List<BatteryComponentExt> list = (from DataRow dr in dt.Rows select CreateBatteryComponentObjectExt(dr)).ToList();

            return list;
        }
        public static int AddBatteryComponentPart(BatteryComponent batteryComponent, NpgsqlCommand cmd)
        {
            try
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

                cmd.CommandText =
                    @"INSERT INTO public.battery_component (
                        fk_experiment,
                        fk_battery_component_type,
                        step,
                        fk_step_material,
                        fk_step_batch,
                        weight,
                        fk_function,
                        fk_material_type,
                        fk_stored_in_type,
                        order_in_step,
                        fk_commercial_type,
                        is_complete
                        )
                    VALUES (:eid, :bctid, :step, :smid, :sbid, :w, :fid, :mtid, :sit, :ois, :comt, :iscomplete);";

                Db.CreateParameterFunc(cmd, "@eid", batteryComponent.fkExperiment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bctid", batteryComponent.fkBatteryComponentType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@step", batteryComponent.step, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@smid", batteryComponent.fkStepMaterial, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@sbid", batteryComponent.fkStepBatch, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@w", batteryComponent.weight, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@fid", batteryComponent.fkFunction, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@mtid", batteryComponent.fkMaterialType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@sit", batteryComponent.fkStoredInType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@ois", batteryComponent.orderInStep, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@comt", batteryComponent.fkCommercialType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@iscomplete", batteryComponent.isComplete, NpgsqlDbType.Boolean);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting Battery Component", ex);
            }

            return 0;
        }





        //za edna komponenta samo, ne treba
        //public static int AddBatteryComponentStockData(AddBatteryComponentRequest req, int researchGroupId)
        //        {
        //            var cmd = Db.CreateCommand();
        //            if (cmd.Connection.State != ConnectionState.Open)
        //            {
        //                cmd.Connection.Open();
        //            }
        //            cmd.CommandType = CommandType.Text;

        //            NpgsqlTransaction t = cmd.Connection.BeginTransaction();

        //            int res = 0;
        //            int experimentId = 0;
        //            int operatorId = 0;
        //            bool materialQuantityOk = true;
        //            bool batchQuantityOk = true;

        //            int componentTypeId = 0;
        //            switch (req.componentType)
        //            {
        //                case "Anode":
        //                    componentTypeId = 1;
        //                    break;
        //                case "Cathode":
        //                    componentTypeId = 2;
        //                    break;
        //                case "Separator":
        //                    componentTypeId = 3;
        //                    break;
        //                case "Electrolyte":
        //                    componentTypeId = 4;
        //                    break;
        //                case "ReferenceElectrode":
        //                    componentTypeId = 5;
        //                    break;
        //                case "Casing":
        //                    componentTypeId = 6;
        //                    break;
        //            }
        //            List<AddBatteryComponentStepRequest> componentStepsContentList;

        //            string notEnoughMessage = "Not enough ";
        //            try
        //            {
        //                experimentId = (int)req.experimentId;
        //                operatorId = (int)req.userId;
        //                componentStepsContentList = req.componentStepsContentList;

        //                //STOCK VALIDATION
        //                foreach (AddBatteryComponentStepRequest step in componentStepsContentList)
        //                {
        //                    List<string> materials = new List<string>();
        //                    List<string> batches = new List<string>();

        //                    if (step.isSavedAsBatch == false)
        //                    {
        //                        foreach (BatteryComponentExt stepContent in step.stepContent)
        //                        {
        //                            double wantedQuantity = (double)stepContent.weight;
        //                            if (stepContent.fkStepMaterial != null)
        //                            {
        //                                int materialId = (int)stepContent.fkStepMaterial;
        //                                bool result = StockTransactionDa.CheckMaterialStockQuantityEnough(materialId, wantedQuantity, researchGroupId);

        //                                if (result == false)
        //                                {
        //                                    materialQuantityOk = false;
        //                                    materials.Add(stepContent.materialName);
        //                                }
        //                            }
        //                            else
        //                            {
        //                                //ako e batch
        //                                int batchId = (int)stepContent.fkStepBatch;
        //                                bool result = StockTransactionDa.CheckBatchStockQuantityEnough(batchId, wantedQuantity, researchGroupId);
        //                                if (result == false)
        //                                {
        //                                    batchQuantityOk = false;
        //                                    batches.Add(stepContent.batchSystemLabel);
        //                                }
        //                            }
        //                        }
        //                        if (materialQuantityOk == false)
        //                        {
        //                            string materialsString = "";
        //                            foreach (string m in materials)
        //                            {
        //                                if (materials.IndexOf(m) == materials.Count - 1)
        //                                {
        //                                    materialsString += m;
        //                                }
        //                                else
        //                                {
        //                                    materialsString += m + ", ";
        //                                }
        //                            }
        //                            notEnoughMessage += "Materials: " + materialsString;
        //                        }
        //                        if (batchQuantityOk == false)
        //                        {
        //                            string batchesString = "";
        //                            foreach (string b in batches)
        //                            {
        //                                if (batches.IndexOf(b) == batches.Count - 1)
        //                                {
        //                                    batchesString += b;
        //                                }
        //                                else
        //                                {
        //                                    batchesString += b + ", ";
        //                                }
        //                            }
        //                            notEnoughMessage += " Batches: " + batchesString;
        //                        }
        //                        if (!materialQuantityOk || !batchQuantityOk)
        //                        {
        //                            notEnoughMessage += " in stock!";
        //                            throw new Exception(notEnoughMessage);
        //                        }
        //                    }

        //                }
        //            }
        //            catch (ValidationException ve)
        //            {
        //                //do whatever
        //                throw new Exception(ve.Message);
        //            }


        //            try
        //            {
        //                //GO THROUGH COMPONENT STEP CONTENTS AND WORK WITH STOCK
        //                foreach (AddBatteryComponentStepRequest step in componentStepsContentList)
        //                {
        //                    foreach (BatteryComponent batteryComponent in step.stepContent)
        //                    {
        //                        //Odzemi od stock za sekoj materijal/batch
        //                        //samo ako ne e veke zacuvan ko batch
        //                        if (step.isSavedAsBatch == false)
        //                        {
        //                            cmd.Parameters.Clear();
        //                            if (batteryComponent.fkStepMaterial != null)
        //                            {
        //                                cmd.CommandText =
        //                                @"INSERT INTO public.stock_transaction (
        //fk_material,
        //stock_transaction_element_type,
        //amount,
        //fk_operator,
        //fk_research_group,
        //fk_experiment_coming,
        //transaction_direction,
        //fk_battery_component_type,
        //date_created
        //
        //)
        //                    VALUES (
        //:mid,
        //1,
        //:a,
        //:oid,
        //:rgid,
        //:ecomid,
        //-1,
        //:bctid,
        //now()::timestamp);";

        //                                Db.CreateParameterFunc(cmd, "@mid", batteryComponent.fkStepMaterial, NpgsqlDbType.Integer);
        //                                Db.CreateParameterFunc(cmd, "@a", batteryComponent.weight, NpgsqlDbType.Double);
        //                                Db.CreateParameterFunc(cmd, "@oid", operatorId, NpgsqlDbType.Integer);
        //                                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
        //                                Db.CreateParameterFunc(cmd, "@ecomid", experimentId, NpgsqlDbType.Integer);
        //                                Db.CreateParameterFunc(cmd, "@bctid", componentTypeId, NpgsqlDbType.Integer);
        //                                //Db.CreateParameterFunc(cmd, "@stet", batchContent.fkFunction, NpgsqlDbType.Integer);

        //                                res = Db.ExecuteNonQuery(cmd, false);
        //                                if (res <= 0)
        //                                {
        //                                    //t.Rollback();
        //                                    throw new Exception("Error inserting material stock info");
        //                                }
        //                            }
        //                            else if (batteryComponent.fkStepBatch != null)
        //                            {
        //                                cmd.Parameters.Clear();
        //                                cmd.CommandText =
        //                                @"IINSERT INTO public.stock_transaction (
        //fk_batch,
        //stock_transaction_element_type,
        //amount,
        //fk_operator,
        //fk_research_group,
        //fk_experiment_coming,
        //transaction_direction,
        //fk_battery_component_type,
        //date_created
        //
        //)
        //                    VALUES (
        //:bid,
        //2,
        //:a,
        //:oid,
        //:rgid,
        //:ecomid,
        //-1,
        //:bctid,
        //now()::timestamp);";

        //                                Db.CreateParameterFunc(cmd, "@bid", batteryComponent.fkStepBatch, NpgsqlDbType.Integer);
        //                                Db.CreateParameterFunc(cmd, "@a", batteryComponent.weight, NpgsqlDbType.Double);
        //                                Db.CreateParameterFunc(cmd, "@oid", operatorId, NpgsqlDbType.Integer);
        //                                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
        //                                Db.CreateParameterFunc(cmd, "@ecomid", experimentId, NpgsqlDbType.Integer);
        //                                Db.CreateParameterFunc(cmd, "@bctid", componentTypeId, NpgsqlDbType.Integer);

        //                                res = Db.ExecuteNonQuery(cmd, false);
        //                                if (res <= 0)
        //                                {
        //                                    //t.Rollback();
        //                                    throw new Exception("Error inserting batch stock info");
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //                //Mark Battery Component as Complete
        //                //var markRes = MarkBatteryComponentComplete(experimentId, componentTypeId, cmd);
        //                //if(markRes != 0)
        //                //{
        //                //    t.Rollback();
        //                //    throw new Exception("Error saving battery component");
        //                //}
        //                cmd.Parameters.Clear();
        //                cmd.CommandText =
        //                    @"UPDATE public.battery_component
        //                        SET is_complete=true
        //                        WHERE fk_experiment=:eid AND
        //                        fk_battery_component_type=:bctid;";

        //                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
        //                Db.CreateParameterFunc(cmd, "@bctid", componentTypeId, NpgsqlDbType.Integer);

        //                res = Db.ExecuteNonQuery(cmd, false);
        //                if (res <= 0)
        //                {
        //                    t.Rollback();
        //                    throw new Exception("Error updating battery component info");
        //                }

        //                cmd.Parameters.Clear();
        //                cmd.CommandText =
        //                    @"UPDATE public.experiment_process
        //                        SET is_complete=true
        //                        WHERE fk_experiment=:eid AND
        //                        fk_battery_component_type=:bctid;";

        //                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
        //                Db.CreateParameterFunc(cmd, "@bctid", componentTypeId, NpgsqlDbType.Integer);

        //                res = Db.ExecuteNonQuery(cmd, false);
        //                if (res <= 0)
        //                {
        //                    t.Rollback();
        //                    throw new Exception("Error updating experiment process info");
        //                }

        //                t.Commit();
        //                cmd.Connection.Close();
        //            }
        //            catch (Exception ex)
        //            {
        //                t.Rollback();
        //                throw new Exception(ex.Message);
        //            }
        //            return 0;
        //        }
        public static int MarkBatteryComponentComplete(int experimentId, int componentTypeId, NpgsqlCommand cmd)
        {
            //Mark component and it's content as complete
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
            NpgsqlTransaction t = cmd.Connection.BeginTransaction();

            try
            {
                cmd.CommandText =
                    @"UPDATE public.battery_component
                        SET is_complete=true
                        WHERE fk_experiment=:eid AND
                        fk_battery_component_type=:bctid;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bctid", componentTypeId, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd, false);
                if (res <= 0)
                {
                    t.Rollback();
                    throw new Exception("Error updating battery component info");
                }

                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.experiment_process
                        SET is_complete=true
                        WHERE fk_experiment=:eid AND
                        fk_battery_component_type=:bctid;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bctid", componentTypeId, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd, false);
                if (res <= 0)
                {
                    t.Rollback();
                    throw new Exception("Error updating experiment process info");
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
        }

        public static int UpdateBatteryComponent(BatteryComponent batteryComponent, NpgsqlCommand cmd)
        {
            try
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
                cmd.CommandText =
                    @"UPDATE public.battery_component
                        SET 
fk_experiment=:eid,
fk_battery_component_type=:bctid,
step=:step, 
fk_step_material=:smid,
fk_step_batch=:sbid,
weight=:w,
fk_function=:fid,

fk_stored_in_type =:ois,
fk_commercial_type =:comt,
is_complete =:iscomplete

                        WHERE battery_component_id=:bcid;";
                Db.CreateParameterFunc(cmd, "@eid", batteryComponent.fkExperiment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bctid", batteryComponent.fkBatteryComponentType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@step", batteryComponent.step, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@smid", batteryComponent.fkStepMaterial, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@sbid", batteryComponent.fkStepBatch, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@w", batteryComponent.weight, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@fid", batteryComponent.fkFunction, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@mtid", batteryComponent.fkMaterialType, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@sit", batteryComponent.fkStoredInType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@ois", batteryComponent.orderInStep, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@comt", batteryComponent.fkCommercialType, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@iscomplete", batteryComponent.isComplete, NpgsqlDbType.Boolean);
                Db.CreateParameterFunc(cmd, "@bcid", batteryComponent.batteryComponentId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Battery Component info", ex);
            }
            return 0;
        }
        /// <summary>
        /// Check if the combination of component id and step id exist in experiment
        /// </summary>
        /// <param name="experimentId"></param>
        /// <param name="batteryComponentTypeId"></param>
        /// <param name="stepId"></param>
        /// <returns>true if step number is found in experiment, false otherwise</returns>
        public static bool StepExists(int experimentId, int batteryComponentTypeId, int stepId)
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
                    @"SELECT battery_component_id
                              FROM battery_component bc

                              WHERE (bc.fk_experiment = :eid) AND
                                    (bc.fk_battery_component_type = :bctid) AND
                                    (bc.step = :sid)
                                    ;";
                //and                                     bc.is_complete = true

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bctid", batteryComponentTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@sid", stepId, NpgsqlDbType.Integer);

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

        public static BatteryComponent CreateBatteryComponentObject(DataRow dr)
        {
            var batteryComponent = new BatteryComponent
            {
                batteryComponentId = (long)dr["battery_component_id"],
                fkExperiment = dr["fk_experiment"] != DBNull.Value ? int.Parse(dr["fk_experiment"].ToString()) : (int?)null,
                fkBatteryComponentType = dr["fk_battery_component_type"] != DBNull.Value ? int.Parse(dr["fk_battery_component_type"].ToString()) : (int?)null,
                step = dr["step"] != DBNull.Value ? int.Parse(dr["step"].ToString()) : (int?)null,
                fkStepMaterial = dr["fk_step_material"] != DBNull.Value ? long.Parse(dr["fk_step_material"].ToString()) : (long?)null,
                fkStepBatch = dr["fk_step_batch"] != DBNull.Value ? int.Parse(dr["fk_step_batch"].ToString()) : (int?)null,
                weight = dr["weight"] != DBNull.Value ? double.Parse(dr["weight"].ToString()) : (double?)null,
                fkFunction = dr["fk_function"] != DBNull.Value ? int.Parse(dr["fk_function"].ToString()) : (int?)null,
                //fkMaterialType = dr["fk_material_type"] != DBNull.Value ? int.Parse(dr["fk_material_type"].ToString()) : (int?)null,
                //fkStoredInType = dr["fk_stored_in_type"] != DBNull.Value ? int.Parse(dr["fk_stored_in_type"].ToString()) : (int?)null,
                orderInStep = dr["order_in_step"] != DBNull.Value ? int.Parse(dr["order_in_step"].ToString()) : (int?)null,
                fkCommercialType = dr["fk_commercial_type"] != DBNull.Value ? long.Parse(dr["fk_commercial_type"].ToString()) : (long?)null,
                isComplete = dr["is_complete"] != DBNull.Value ? Boolean.Parse(dr["is_complete"].ToString()) : false,
                isSavedAsBatch = dr["is_saved_as_batch"] != DBNull.Value ? Boolean.Parse(dr["is_saved_as_batch"].ToString()) : false,
                percentageOfActive = dr["percentage_of_active"] != DBNull.Value ? double.Parse(dr["percentage_of_active"].ToString()) : (double?)null
            };
            return batteryComponent;
        }
        private static BatteryComponentExt CreateBatteryComponentObjectExt(DataRow dr)
        {
            var batteryComponent = CreateBatteryComponentObject(dr);

            string batteryComponentTypeVar = dr.Table.Columns.Contains("battery_component_type") ? dr["battery_component_type"].ToString() : null;
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
            //var measurementsVar = null;
            //if (measurements != null)
            //{
            //    measurementsVar = measurements;
            //}
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
            var batteryComponentExt = new BatteryComponentExt(batteryComponent)
            {
                batteryComponentType = batteryComponentTypeVar,
                materialName = dr["material_name"].ToString(),
                batchSystemLabel = dr["batch_system_label"].ToString(),
                batchPersonalLabel = dr["batch_personal_label"].ToString(),
                measurementUnitName = measurementUnitName,
                measurementUnitSymbol = measurementUnitSymbol,
                chemicalFormula = chemicalFormula,
                measurements = measurements,

                materialLabel = materialLabel,
                description = description,
                storedInType = storedInType,
                price = price,
                bulkPrice = bulkPrice,

                materialFunction = materialFunctionVar,
                batchOutput = batchOutputVar
            };
            return batteryComponentExt;
        }
    }
}