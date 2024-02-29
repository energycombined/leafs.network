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
    public class TestTypeDa
    {
        public static bool IsSuportGraph(int? testTypeId = null)
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
                    FROM test_type tt
                    WHERE (tt.test_type_id = :ttid or :ttid is null)
                    AND tt.supports_graphing = true";

                Db.CreateParameterFunc(cmd, "@ttid", testTypeId, NpgsqlDbType.Integer);
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
        public static TestType GetTestTypeById(int testTypeId, bool? supportsGraphing = null)
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
                        FROM test_type tt
                        WHERE (tt.test_type_id = :ttid)
                              AND (tt.supports_graphing = :graphing or :graphing is null)
                        ";

                Db.CreateParameterFunc(cmd, "@ttid", testTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@graphing", supportsGraphing, NpgsqlDbType.Boolean);

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

            TestType res = CreateObject(dt.Rows[0]);
            return res;
        }
        public static List<TestTypeExt> GetAllTestTypes(string search = null, int? testTypeId = null, bool? supportsGraphing = null)
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
                    FROM test_type tt
                    WHERE (tt.test_type_id = :ttid or :ttid is null)
                          AND (tt.supports_graphing = :graphing or :graphing is null)
                          AND ((lower(tt.test_type) LIKE lower('%'|| :search ||'%')) or :search is null)

                    ORDER BY tt.supports_graphing DESC, tt.test_type ASC";

                Db.CreateParameterFunc(cmd, "@ttid", testTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@graphing", supportsGraphing, NpgsqlDbType.Boolean);
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

            List<TestTypeExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<TestTypeExt> GetTestTypesByExperimentOrBatchIds(string search = null, bool? supportsGraphing = null, int[] experimentIds = null, int[] batchIds = null)
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
                    @"SELECT DISTINCT tt.test_type_id, tt.test_type, tt.supports_graphing, tt.test_type_subcategory
                    FROM test t 
                    LEFT JOIN test_type tt ON test_type_id=t.fk_test_type
                    "
                    ;
                if (experimentIds.Length != 0)
                {
                    cmd.CommandText += @"WHERE t.fk_measurement_level_type=6
                          AND (tt.supports_graphing = :graphing or :graphing is null)
                          AND ((lower(tt.test_type) LIKE lower('%'|| :search ||'%')) or :search is null)

                          GROUP BY tt.test_type_id
						  having array_agg(t.fk_experiment) @> array[:eidList]";
                }
                else if (batchIds.Length != 0)
                {
                    cmd.CommandText += @"WHERE t.fk_measurement_level_type=5
                          AND (tt.supports_graphing = :graphing or :graphing is null)
                          AND ((lower(tt.test_type) LIKE lower('%'|| :search ||'%')) or :search is null)

                          GROUP BY tt.test_type_id
					      having array_agg(t.fk_batch) @> array[:bidList]";
                }
                

                Db.CreateParameterFunc(cmd, "@graphing", supportsGraphing, NpgsqlDbType.Boolean);
                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@eidList", experimentIds, NpgsqlDbType.Array | NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bidList", batchIds, NpgsqlDbType.Array | NpgsqlDbType.Integer);

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

            List<TestTypeExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<TestTypeExt> GetTestTypesByName(string search)
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
                    FROM test_type tt
                    
                    WHERE (lower(tt.test_type) LIKE lower('%'|| :search ||'%') or :search is null)
                    
                    LIMIT 10
                        ;";

                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
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

            List<TestTypeExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }

        public static int AddTestType(TestType testType)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"INSERT INTO public.test_type (test_type)
                    VALUES (:ttype);";

                Db.CreateParameterFunc(cmd, "@ttype", testType.testType, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting test type", ex);
            }

            return 0;
        }
        public static int UpdateTestType(TestType testType)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.test_type
                        SET test_type=:ttype
                        WHERE test_type_id=:ttid;";

                Db.CreateParameterFunc(cmd, "@ttype", testType.testType, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@ttid", testType.testTypeId, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating test type info", ex);
            }
            return 0;
        }
        public static int DeleteTestType(int testTypeId)
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
                            @"SELECT t.test_id 
                            FROM test t
                            WHERE t.fk_test_type=:ttid;";

                Db.CreateParameterFunc(cmd, "@ttid", testTypeId, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count > 0)
                {
                    throw new Exception("There are some tests connected to this test type!");
                }

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.test_type
                                WHERE test_type_id=:ttid;";

                Db.CreateParameterFunc(cmd, "@ttid", testTypeId, NpgsqlDbType.Integer);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        public static TestType CreateObject(DataRow dr)
        {
            var testType = new TestType
            {
                testTypeId = (int)dr["test_type_id"],
                testType = dr["test_type"].ToString(),
                supportsGraphing = bool.Parse(dr["supports_graphing"].ToString()),
                testTypeSubcategory = dr["test_type_subcategory"].ToString(),
            };
            return testType;
        }
        private static TestTypeExt CreateObjectExt(DataRow dr)
        {
            var testType = CreateObject(dr);

            var testTypeExt = new TestTypeExt(testType)
            {
            };
            return testTypeExt;
        }
    }
}