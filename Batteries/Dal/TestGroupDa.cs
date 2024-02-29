using Batteries.Dal.Base;
using Batteries.Helpers;
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
    public class TestGroupDa
    {
        public static string GetAllTestGroupsJsonForDropdown(int? testGroupId = null, int? researchGroupId = null)
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
                      FROM ( select test_group_id as value, COALESCE(tg.test_group_name, '') as text
                              from test_group tg
                              where (tg.fk_research_group = :rgid or :rgid is null) and
                                    (tg.test_group_id = :tgid or :tgid is null)
                          ) as t;";

                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@tgid", testGroupId, NpgsqlDbType.Integer);

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

        public static List<TestGroupExt> GetAllTestGroupsByProjectForDropdown(string search, int? page = null, int? projectId = null)
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
                              FROM test_group tg
                             
                              WHERE 
                                    (lower(tg.test_group_name) LIKE lower('%'|| :search ||'%') or :search is null) and
                                    (tg.fk_project = :tgid)
                              ORDER BY tg.date_created DESC
                              LIMIT 10 OFFSET :offset
                                    ;";


                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@tgid", projectId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@offset", (page - 1) * 10, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                NotifyHelper.Notify("Please Enter Test group for new Project", NotifyHelper.NotifyType.danger, "");
            }

            List<TestGroupExt> list = (from DataRow dr in dt.Rows select CreateTestGroupObjectExt(dr)).ToList();

            return list;
        }
        /// <summary>
        /// Get test groups by researchGroupIdCreator or by id of the test group
        /// </summary>
        /// <param name="testGroupId"></param>
        /// <param name="researchGroupId"></param>
        /// <returns></returns>
        public static List<TestGroupExt> GetAllTestGroups(int? testGroupId = null, int? researchGroupId = null)
        {
            DataTable dt;
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                //, u.username as operator_username
                cmd.CommandText =
                    @"SELECT  tg.*, rg.research_group_name, u.username as operator_username, p.project_name, p.project_acronym
                              FROM test_group tg
                              LEFT JOIN research_group rg ON tg.fk_research_group = rg.research_group_id
                              LEFT JOIN project p ON tg.fk_project = p.project_id
                              LEFT JOIN users u on tg.fk_operator = u.user_id

                              WHERE (tg.fk_research_group = :rgid or :rgid is null) and
                                    (tg.test_group_id = :tgid or :tgid is null)
                                ORDER BY tg.date_created DESC
                                    ;";

                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@tgid", testGroupId, NpgsqlDbType.Integer);

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

            List<TestGroupExt> list = (from DataRow dr in dt.Rows select CreateTestGroupObjectExt(dr)).ToList();

            return list;
        }
        public static List<TestGroupExt> GetTestGroupsByResearchGroupCreator(string search = null, int? researchGroupId = null, int? page = 1)
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
                    FROM test_group tg
                    WHERE (lower(tg.test_group_name) LIKE lower('%'|| :search ||'%') or :search is null) and
                    (tg.fk_research_group = :rgid or :rgid is null)
                    ORDER BY tg.date_created DESC
                    LIMIT 10 OFFSET :offset
                        ;";

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

            List<TestGroupExt> list = (from DataRow dr in dt.Rows select CreateTestGroupObjectExt(dr)).ToList();

            return list;
        }
        public static List<TestGroupExt> GetTestGroupsForResearchGroup(string search = null, int? currentResearchGroup = null, int? researchGroupId = null, int? projectId = null, int? page = 1)
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
                    FROM test_group tg
                    WHERE (lower(tg.test_group_name) LIKE lower('%'|| :search ||'%') or :search is null)
                    AND tg.fk_project IN (
								  SELECT fk_project FROM
									project_research_group
									WHERE (fk_research_group = :crgid or :crgid is null)
							  ) 
                    AND (tg.fk_project = :pid or :pid is null) 
                    AND (tg.fk_research_group = :rgid or :rgid is null)
                    ORDER BY tg.date_created DESC
                    LIMIT 10 OFFSET :offset
                        ;";

                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@crgid", currentResearchGroup, NpgsqlDbType.Integer);
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

            List<TestGroupExt> list = (from DataRow dr in dt.Rows select CreateTestGroupObjectExt(dr)).ToList();

            return list;
        }
        /// <summary>
        /// Get all test groups that belong to the project, but experimentId is not yet added to the test group
        /// </summary>
        /// <param name="search"></param>
        /// <param name="experimentId"></param>
        /// <param name="researchGroupId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static List<TestGroupExt> GetTestGroupsByProjectExperimentExcluded(string search, int experimentId, int projectId, int? researchGroupId = null, int? page = 1)
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
                    FROM test_group tg
                    WHERE
                    tg.test_group_id NOT IN(
                        SELECT tge.fk_test_group
                              FROM test_group_experiment tge

                              WHERE
                                    (tge.fk_experiment = :eid)
                      ) 
                    AND (lower(tg.test_group_name) LIKE lower('%'|| :search ||'%') or :search is null)
                    AND (tg.fk_research_group = :rgid or :rgid is null)
                    AND tg.fk_project = :pid

                    ORDER BY tg.date_created DESC
                    LIMIT 10 OFFSET :offset
                        ;";

                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pid", projectId, NpgsqlDbType.Integer);
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

            List<TestGroupExt> list = (from DataRow dr in dt.Rows select CreateTestGroupObjectExt(dr)).ToList();

            return list;
        }
        /// <summary>
        /// Get test groups list that researchGroupId is meant to see.
        /// Get a list of test groups, including the test groups coming from a shared project.
        /// </summary>
        /// <param name="testGroupId"></param>
        /// <param name="researchGroupId"></param>
        /// <returns></returns>
        public static List<TestGroupExt> GetTestGroupListForResearchGroup(int researchGroupId, int? testGroupId = null)
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
                    @"SELECT *, u.username as operator_username, p.project_name as fk_project_name
                              FROM test_group tg
                              LEFT JOIN research_group rg ON tg.fk_research_group = rg.research_group_id
                              LEFT JOIN project p ON tg.fk_project = p.project_id
                              LEFT JOIN users u on tg.fk_operator = u.user_id

                              WHERE
							  tg.fk_project IN (
								  SELECT fk_project FROM
									project_research_group
									WHERE (fk_research_group = :rgid or :rgid is null)
							  )
                              AND (tg.test_group_id = :tgid or :tgid is null)
                              ORDER BY tg.date_created DESC
                                    ;";

                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@tgid", testGroupId, NpgsqlDbType.Integer);

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

            List<TestGroupExt> list = (from DataRow dr in dt.Rows select CreateTestGroupObjectExt(dr)).ToList();

            return list;
        }
        public static TestGroup GetLastUsedTestGroup(int userId)
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
                    @"SELECT * FROM test_group_experiment tge
                    LEFT JOIN test_group tg ON tge.fk_test_group=tg.test_group_id
                    WHERE tge.fk_user=:uid AND
                    tge.fk_experiment IN (SELECT e.experiment_id FROM experiment e
                    WHERE e.fk_user=:uid
                    ORDER BY e.experiment_id DESC
                    LIMIT 1 );";
                Db.CreateParameterFunc(cmd, "@uid", userId, NpgsqlDbType.Integer);
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

            TestGroup result = CreateTestGroupObject(dt.Rows[0]);

            return result;
        }
        public static int AddTestGroup(TestGroup testGroup)
        {
            DataTable dt;
            var cmd = Db.CreateCommand();
            try
            {
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                            @"SELECT tg.test_group_id 
                            FROM test_group tg
                            WHERE (lower(tg.test_group_name) = lower(:name)) AND
                            fk_research_group = :rgid;";

                Db.CreateParameterFunc(cmd, "@name", testGroup.testGroupName, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@rgid", testGroup.fkResearchGroup, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count > 0)
                {
                    throw new Exception("Test group name already exists.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            try
            {

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"INSERT INTO public.test_group (test_group_goal,
                    test_group_name,
                    fk_research_group,
                    fk_project,
                    fk_operator)
                    VALUES (:goal, :name, :rgid, :proj, :uid);";

                Db.CreateParameterFunc(cmd, "@goal", testGroup.testGroupGoal, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@name", testGroup.testGroupName, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@rgid", testGroup.fkResearchGroup, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@proj", testGroup.fkProject, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@uid", testGroup.fkUser, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting Test group", ex);
            }

            return 0;
        }
        public static int UpdateTestGroup(TestGroup testGroup)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.test_group
                        SET test_group_goal=:goal, test_group_name=:name, fk_research_group=:rgid, fk_project=:proj, last_change=now()::timestamp
                        WHERE test_group_id=:tgid;";

                Db.CreateParameterFunc(cmd, "@goal", testGroup.testGroupGoal, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@name", testGroup.testGroupName, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@rgid", testGroup.fkResearchGroup, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@proj", testGroup.fkProject, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@tgid", testGroup.testGroupId, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Test group info", ex);
            }
            return 0;
        }
        public static int DeleteTestGroup(int testGroupId)
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
                            @"SELECT tge.test_group_experiment_id 
                            FROM test_group_experiment tge
                            WHERE tge.fk_test_group=:tgid;";

                Db.CreateParameterFunc(cmd, "@tgid", testGroupId, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count > 0)
                {
                    throw new Exception("There is an experiment associated to this test group.");
                }

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.test_group
                                WHERE test_group_id=:tgid;";

                Db.CreateParameterFunc(cmd, "@tgid", testGroupId, NpgsqlDbType.Integer);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        public static TestGroup CreateTestGroupObject(DataRow dr)
        {
            var testGroup = new TestGroup
            {
                testGroupId = (int)dr["test_group_id"],
                testGroupGoal = dr["test_group_goal"].ToString(),
                testGroupName = dr["test_group_name"].ToString(),
                fkResearchGroup = dr["fk_research_group"] != DBNull.Value ? int.Parse(dr["fk_research_group"].ToString()) : (int?)null,
                fkProject = dr["fk_project"] != DBNull.Value ? int.Parse(dr["fk_project"].ToString()) : (int?)null,
                fkUser = dr["fk_operator"] != DBNull.Value ? int.Parse(dr["fk_operator"].ToString()) : (int?)null,
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,
                lastChange = dr["last_change"] != DBNull.Value ? DateTime.Parse(dr["last_change"].ToString()) : (DateTime?)null
            };
            return testGroup;
        }
        private static TestGroupExt CreateTestGroupObjectExt(DataRow dr)
        {
            var testGroup = CreateTestGroupObject(dr);

            string operatorUsernameVar = dr.Table.Columns.Contains("operator_username") ? dr["operator_username"].ToString() : null;
            string researchGroupNameVar = dr.Table.Columns.Contains("research_group_name") ? dr["research_group_name"].ToString() : null;
            string projectNameVar = dr.Table.Columns.Contains("project_name") ? dr["project_name"].ToString() : null;
            string projectAcronymVar = dr.Table.Columns.Contains("project_acronym") ? dr["project_acronym"].ToString() : null;

            var testGroupExt = new TestGroupExt(testGroup)
            {
                operatorUsername = operatorUsernameVar,
                researchGroupName = researchGroupNameVar,
                projectName = projectNameVar,
                projectAcronym = projectAcronymVar
            };
            return testGroupExt;
        }
    }
}