using Batteries.Dal.Base;
using Batteries.Models.Responses;
using Batteries.Models.TestResultsModels;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Batteries.Dal
{
    public class TestDataDa
    {
        public static TestExt GetTestDataForCharts(int testTypeId, int? experimentId, int? batchId)
        {
            DataTable dt;
            //test type id = 1 

            try
            {
                var cmd = Db.CreateCommand();
                if (experimentId != null)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.CommandText =
                     @"SELECT t.*, tt.*, td.test_data_id, td.fk_test, td.data, td.date_created as test_data_date_created, u.username as operator_username, rg.*
                        FROM test_data td
                        LEFT JOIN test t ON t.test_id = td.fk_test
                        LEFT JOIN test_type tt on t.fk_test_type = tt.test_type_id
                        LEFT JOIN users u ON u.user_id = t.fk_user
                        LEFT JOIN research_group rg ON rg.research_group_id = t.fk_research_group
                        WHERE t.fk_experiment = :eid
                        AND t.fk_test_type = :ttid
                        AND t.fk_measurement_level_type = 6
                      ;";
                    Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@ttid", testTypeId, NpgsqlDbType.Integer);

                    dt = Db.ExecuteSelectCommand(cmd);
                }
                else
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.CommandText =
                     @"SELECT t.*, tt.*, td.test_data_id, td.fk_test, td.data, td.date_created as test_data_date_created, u.username as operator_username, rg.*
                        FROM test_data td
                        LEFT JOIN test t ON t.test_id = td.fk_test
                        LEFT JOIN test_type tt on t.fk_test_type = tt.test_type_id
                        LEFT JOIN users u ON u.user_id = t.fk_user
                        LEFT JOIN research_group rg ON rg.research_group_id = t.fk_research_group
                        WHERE t.fk_batch = :bid
                        AND t.fk_test_type = :ttid
                        AND t.fk_measurement_level_type = 5
                      ;";
                    Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@ttid", testTypeId, NpgsqlDbType.Integer);

                    dt = Db.ExecuteSelectCommand(cmd);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            //MeasurementData result = CreateObject(dt.Rows[0]);
            TestExt result = CreateObject(dt.Rows[0]);
            return result;
        }

        public static TestData GetTestDataForChartsOld(int testTypeId, int? experimentId, int? batchId)
        {
            DataTable dt;
            //test type id = 1 

            try
            {
                var cmd = Db.CreateCommand();
                if (experimentId != 0)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.CommandText =
                     @"SELECT *
FROM measurement_data
WHERE fk_experiment=:eid
AND fk_test_type = :ttid
                      ;";
                    /* @"SELECT arr.test_data_id, 
    (arr.item_object->>'testTime') :: numeric as test_time, 
    (arr.item_object->>'voltage') :: numeric as voltage,
    (arr.item_object->>'current') :: numeric as current,
    (arr.item_object->>'chargeCapacity') :: numeric as charge_capacity,
    (arr.item_object->>'dischargeCpacity') :: numeric as discharge_capacity,
    (arr.item_object->>'chargeEnergy') :: numeric as charge_energy,
    (arr.item_object->>'dischargeEnergy') :: numeric as discharge_energy,
    (arr.item_object->>'cycleIndex') :: integer as cycle_index
    FROM measurement_data,
    jsonb_array_elements(data) with ordinality arr(item_object, test_data_id) 
    WHERE fk_experiment=:eid
    ORDER BY cycle_index, test_time
                           ;";*/
                    //and (t.current != 0)
                    Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@ttid", testTypeId, NpgsqlDbType.Integer);

                    dt = Db.ExecuteSelectCommand(cmd);
                }
                else
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.CommandText =
                     @"SELECT *
FROM measurement_data
WHERE fk_batch=:bid
AND fk_test_type = :ttid
                      ;";
                    Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@ttid", testTypeId, NpgsqlDbType.Integer);

                    dt = Db.ExecuteSelectCommand(cmd);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            //TestData result = CreateObjectOld(dt.Rows[0]);
            return null;
        }

        public static TestExt CreateObject(DataRow dr)
        {
            TestExt test = TestDa.CreateObjectExt(dr);

            TestData testData = new TestData
            {
                testDataId = int.Parse(dr["test_data_id"].ToString()),
                fkTest = dr["fk_test"] != DBNull.Value ? int.Parse(dr["fk_test"].ToString()) : (int?)null,
                data = dr["data"].ToString(),
                dateCreated = dr["test_data_date_created"] != DBNull.Value ? DateTime.Parse(dr["test_data_date_created"].ToString()) : (DateTime?)null,

            };
            test.testData = testData;
            return test;
        }
    }
}