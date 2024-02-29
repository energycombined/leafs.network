using Batteries.Dal.Base;
using Batteries.Models;
using Batteries.Models.Responses;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Dal
{
    public class TestDa
    {
        public static List<TestExt> GetAllTests(int? testId = null, int? experimentId = null, int? testTypeId = null)
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
                    @"SELECT *, u.username as operator_username, rg.research_group_name as research_group_name
                    FROM test t
                        left join users u on t.fk_user = u.user_id
                        left join research_group rg on t.fk_research_group = rg.research_group_id
                        left join test_type tt on t.fk_test_type = tt.test_type_id
                        left join battery_component_type bc on t.fk_battery_component_type = bc.battery_component_type_id

                    WHERE (t.test_id = :tid or :tid is null) and
                        (t.fk_experiment = :eid or :eid is null) and
                        (t.fk_test_type = :ttid or :ttid is null)
                    ORDER BY t.date_created ASC, t.fk_test_type ASC
                    ;";

                Db.CreateParameterFunc(cmd, "@tid", testId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@ttid", testTypeId, NpgsqlDbType.Integer);

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

            List<TestExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }

        public static long AddTestDataConvertedJson(string jsonb, long fkTest, Test test)
        {
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 240;
            long returnedTestDataID = 0;

            NpgsqlTransaction t = cmd.Connection.BeginTransaction();
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"INSERT INTO public.test_data
                    (
                    fk_test,
                    data
                    )
                    VALUES (:ttid, :data)
                    RETURNING test_data_id
                ;";

                Db.CreateParameterFunc(cmd, "@ttid", fkTest, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@data", jsonb, NpgsqlDbType.Jsonb);

                returnedTestDataID = long.Parse(Db.ExecuteScalar(cmd, false));
                if (returnedTestDataID <= 0)
                {
                    throw new Exception("Error inserting test data");
                }

                if (test.fkMeasurementLevelType == 6)
                {
                    //experiment level
                    cmd.Parameters.Clear();
                    cmd.CommandText =
                        @"UPDATE public.experiment
                        SET has_test_results_doc=true, date_modified=now()::timestamp
                        WHERE experiment_id=:eid;";

                    Db.CreateParameterFunc(cmd, "@eid", test.fkExperiment, NpgsqlDbType.Integer);
                    var resUpdate = Db.ExecuteNonQuery(cmd, false);
                    if (resUpdate <= 0)
                    {
                        t.Rollback();
                        throw new Exception("Error updating experiment info");
                    }
                }
                else if (test.fkMeasurementLevelType == 5)
                {
                    //batch level
                    cmd.Parameters.Clear();
                    cmd.CommandText =
                        @"UPDATE public.batch
                        SET has_test_results_doc=true
                        WHERE batch_id=:bid;";

                    Db.CreateParameterFunc(cmd, "@bid", test.fkBatch, NpgsqlDbType.Integer);
                    var resUpdate = Db.ExecuteNonQuery(cmd, false);
                    if (resUpdate <= 0)
                    {
                        t.Rollback();
                        throw new Exception("Error updating batch info");
                    }
                }

                t.Commit();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                t.Rollback();
                throw new Exception(ex.Message);
            }

            return returnedTestDataID;
        }
        public static long AddTest(Test test)
        {
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 240;

            long returnedTestID = 0;

            NpgsqlTransaction t = cmd.Connection.BeginTransaction();
            try
            {
                //REMOVE ALL PREVIOUS DATA FOR THE SAME TEST AND THE SAME SUBJECT

                //cmd.Parameters.Clear();
                //cmd.CommandText =
                //@"SELECT test_id FROM public.test
                //  WHERE fk_experiment=:eid
                //  AND fk_test_type = :ttid
                //  AND fk_measurement_level_type = 6
                //;";

                //Db.CreateParameterFunc(cmd, "@eid", test.fkExperiment, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@ttid", test.fkTestType, NpgsqlDbType.Integer);
                //var previousTestIdRes = Db.ExecuteScalar(cmd, false);
                //int previousTestId = int.Parse(previousTestIdRes);

                string whereSubject = "";
                if (test.fkMeasurementLevelType == 6)
                {
                    whereSubject = @"WHERE fk_experiment=:eid 
                                        ";
                }
                else if (test.fkMeasurementLevelType == 5)
                {
                    whereSubject = @"WHERE fk_batch=:bid 
                                        ";
                }
                else if (test.fkMeasurementLevelType == 7)
                {
                    whereSubject = @"WHERE fk_material=:mid  
                                        ";
                }
                else if (test.fkMeasurementLevelType == 3)
                {
                    //component level
                    whereSubject = @"WHERE fk_battery_component_type=:comtype
                                        AND fk_experiment=:eid 
                                        ";
                }
                else if (test.fkMeasurementLevelType == 2)
                {
                    //step level
                    whereSubject = @"WHERE step_id=:step 
                                    AND fk_battery_component_type=:comtype
                                    AND fk_experiment=:eid
                                        ";
                }

                cmd.Parameters.Clear();
                cmd.CommandText =
                @"DELETE FROM public.test_data
                               WHERE fk_test IN (
                               SELECT test_id FROM public.test                               
                               ";

                cmd.CommandText += whereSubject;

                cmd.CommandText += @"AND fk_test_type = :ttid
                               AND fk_measurement_level_type = :mtype
                          )
                    ;";
                Db.CreateParameterFunc(cmd, "@eid", test.fkExperiment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bid", test.fkBatch, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@mid", test.fkMaterial, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@comtype", test.fkBatteryComponentType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@step", test.stepId, NpgsqlDbType.Integer);

                Db.CreateParameterFunc(cmd, "@ttid", test.fkTestType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@mtype", test.fkMeasurementLevelType, NpgsqlDbType.Integer);
                var res = Db.ExecuteNonQuery(cmd, false);

                cmd.Parameters.Clear();
                cmd.CommandText =
                @"DELETE FROM public.file_attachment
                                WHERE element_type = 'Test'
                                AND element_id IN 
                                    (SELECT test_id FROM public.test
                                    --WHERE fk_experiment=:eid
                                    ";
                cmd.CommandText += whereSubject;

                cmd.CommandText += @"AND fk_test_type = :ttid
                               AND fk_measurement_level_type = :mtype
                          )
                    ;";

                Db.CreateParameterFunc(cmd, "@eid", test.fkExperiment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bid", test.fkBatch, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@mid", test.fkMaterial, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@comtype", test.fkBatteryComponentType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@step", test.stepId, NpgsqlDbType.Integer);

                Db.CreateParameterFunc(cmd, "@ttid", test.fkTestType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@mtype", test.fkMeasurementLevelType, NpgsqlDbType.Integer);

                var res2 = Db.ExecuteNonQuery(cmd, false);

                cmd.Parameters.Clear();
                cmd.CommandText =
                @"DELETE FROM public.test

                 ";
                cmd.CommandText += whereSubject;
                cmd.CommandText += @"AND fk_test_type = :ttid
                               AND fk_measurement_level_type = :mtype                          
                    ;";

                Db.CreateParameterFunc(cmd, "@eid", test.fkExperiment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bid", test.fkBatch, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@mid", test.fkMaterial, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@comtype", test.fkBatteryComponentType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@step", test.stepId, NpgsqlDbType.Integer);

                Db.CreateParameterFunc(cmd, "@ttid", test.fkTestType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@mtype", test.fkMeasurementLevelType, NpgsqlDbType.Integer);
                var res3 = Db.ExecuteNonQuery(cmd, false);

                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"INSERT INTO public.test 
                    (
                    fk_test_type,
                    fk_test_equipment_model,
                    fk_measurement_level_type,
                    fk_experiment,
                    fk_batch,
                    fk_material,
                    fk_battery_component_type,
                    step_id,
                    fk_battery_component_content,
                    fk_batch_content,
                    fk_research_group,
                    fk_user,
                    test_label,
                    comment
                    )
                    VALUES (:ttid, :teqmodel, :mltype, :eid, :bid, :mid, :comptype, :step, :ccontent, :bcontent, :rgid, :uid, :tlabel, :comm)
                RETURNING test_id
                ;";

                Db.CreateParameterFunc(cmd, "@ttid", test.fkTestType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@teqmodel", test.fkTestEquipmentModel, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@mltype", test.fkMeasurementLevelType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@eid", test.fkExperiment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bid", test.fkBatch, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@mid", test.fkMaterial, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@comptype", test.fkBatteryComponentType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@step", test.stepId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@ccontent", test.fkBatteryComponentContent, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bcontent", test.fkBatchContent, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", test.fkResearchGroup, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@uid", test.fkUser, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@tlabel", test.testLabel, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@comm", test.comment, NpgsqlDbType.Text);

                returnedTestID = long.Parse(Db.ExecuteScalar(cmd, false));
                if (returnedTestID <= 0)
                {
                    t.Rollback();
                    throw new Exception("Error inserting test data");
                }

                t.Commit();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                t.Rollback();
                throw new Exception(ex.Message);
            }

            return returnedTestID;
        }

        public static int UpdateTest(Test test)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.test
                        SET 

fk_experiment=:eid,
test_label=:tlabel, 
fk_operator=:uid,
fk_test_type=:ttid,
last_change=now()::timestamp,
comment=:comm

                        WHERE test_id=:tid;";
                Db.CreateParameterFunc(cmd, "@eid", test.fkExperiment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@tlabel", test.testLabel, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@uid", test.fkUser, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@ttid", test.fkTestType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@comm", test.comment, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@tid", test.testId, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating test info", ex);
            }
            return 0;
        }

        public static Test CreateObject(DataRow dr)
        {
            Test test = new Test
            {
                testId = (long)dr["test_id"],
                fkTestType = dr["fk_test_type"] != DBNull.Value ? int.Parse(dr["fk_test_type"].ToString()) : (int?)null,
                fkTestEquipmentModel = dr["fk_test_equipment_model"] != DBNull.Value ? int.Parse(dr["fk_test_equipment_model"].ToString()) : (int?)null,
                fkMeasurementLevelType = dr["fk_measurement_level_type"] != DBNull.Value ? int.Parse(dr["fk_measurement_level_type"].ToString()) : (int?)null,
                fkExperiment = dr["fk_experiment"] != DBNull.Value ? int.Parse(dr["fk_experiment"].ToString()) : (int?)null,
                fkBatch = dr["fk_batch"] != DBNull.Value ? int.Parse(dr["fk_batch"].ToString()) : (int?)null,
                fkMaterial = dr["fk_material"] != DBNull.Value ? int.Parse(dr["fk_material"].ToString()) : (int?)null,
                fkBatteryComponentType = dr["fk_battery_component_type"] != DBNull.Value ? int.Parse(dr["fk_battery_component_type"].ToString()) : (int?)null,
                stepId = dr["step_id"] != DBNull.Value ? int.Parse(dr["step_id"].ToString()) : (int?)null,
                fkBatteryComponentContent = dr["fk_battery_component_content"] != DBNull.Value ? int.Parse(dr["fk_battery_component_content"].ToString()) : (int?)null,
                fkBatchContent = dr["fk_batch_content"] != DBNull.Value ? int.Parse(dr["fk_batch_content"].ToString()) : (int?)null,
                fkResearchGroup = dr["fk_research_group"] != DBNull.Value ? int.Parse(dr["fk_research_group"].ToString()) : (int?)null,
                fkUser = dr["fk_user"] != DBNull.Value ? int.Parse(dr["fk_user"].ToString()) : (int?)null,
                testLabel = dr["test_label"].ToString(),
                comment = dr["comment"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,
                lastChange = dr["last_change"] != DBNull.Value ? DateTime.Parse(dr["last_change"].ToString()) : (DateTime?)null
            };
            return test;
        }
        public static TestExt CreateObjectExt(DataRow dr)
        {
            var test = CreateObject(dr);
            string operatorUsernameVar = dr.Table.Columns.Contains("operator_username") ? dr["operator_username"].ToString() : null;
            string researchGroupAcronymVar = dr.Table.Columns.Contains("acronym") ? dr["acronym"].ToString() : null;

            var testExt = new TestExt(test)
            {
                testType = dr["test_type"].ToString(),
                operatorUsername = operatorUsernameVar,
                researchGroupAcronym = researchGroupAcronymVar,
                researchGroupName = dr.Table.Columns.Contains("research_group_name") ? dr["research_group_name"].ToString() : null,
                testEquipmentBrand = dr.Table.Columns.Contains("brand_name") ? dr["brand_name"].ToString() : null,
                testEquipmentModel = dr.Table.Columns.Contains("test_equipment_model_name") ? dr["operatortest_equipment_model_name_username"].ToString() : null,
                testTypeSubcategory = dr.Table.Columns.Contains("test_type_subcategory") ? dr["test_type_subcategory"].ToString() : null,
                batteryComponentType = dr.Table.Columns.Contains("battery_component_type") ? dr["battery_component_type"].ToString() : null,
                materialName = dr.Table.Columns.Contains("material_name") ? dr["material_name"].ToString() : null,
                experimentSystemLabel = dr.Table.Columns.Contains("experiment_system_label") ? dr["experiment_system_label"].ToString() : null,
                experimentPersonalLabel = dr.Table.Columns.Contains("experiment_personal_label") ? dr["experiment_personal_label"].ToString() : null,
                batchSystemLabel = dr.Table.Columns.Contains("batch_system_label") ? dr["batch_system_label"].ToString() : null,
                batchPersonalLabel = dr.Table.Columns.Contains("batch_personal_label") ? dr["batch_personal_label"].ToString() : null,
            };
            return testExt;
        }
    }
}