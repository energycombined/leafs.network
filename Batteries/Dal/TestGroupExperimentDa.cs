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
    public class TestGroupExperimentDa
    {
        public static List<TestGroupExperimentExt> GetAllTestGroupExperiments(int? testGroupId = null, int? researchGroupId = null, int? experimentId = null, long? testGroupExperimentId = null)
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
                    @"SELECT tge.*, 
                             e.experiment_system_label, e.experiment_personal_label, rg.research_group_name, u.username as operator_username, e.date_created as date_created_experiment,
                             tg.test_group_name, tg.test_group_goal
                             FROM test_group_experiment tge
                             LEFT JOIN experiment e ON tge.fk_experiment = e.experiment_id
                             LEFT JOIN test_group tg ON tge.fk_test_group = tg.test_group_id
                             
                             LEFT JOIN research_group rg ON e.fk_research_group = rg.research_group_id
                             LEFT JOIN users u ON e.fk_user = u.user_id                              

                             WHERE (tge.fk_test_group = :tgid or :tgid is null) and                                    
                                   (e.fk_research_group = :rgid or :rgid is null) and
                                   (e.is_complete = true) and
                                   (tge.fk_experiment = :eid or :eid is null) and
                                   (tge.test_group_experiment_id = :tgeid or :tgeid is null);";

                Db.CreateParameterFunc(cmd, "@tgid", testGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@tgeid", testGroupExperimentId, NpgsqlDbType.Bigint);

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

            List<TestGroupExperimentExt> list = (from DataRow dr in dt.Rows select CreateTestGroupExperimentObjectExt(dr)).ToList();

            return list;
        }
        public static int AddTestGroupExperiment(TestGroupExperiment testGroupExperiment)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"INSERT INTO public.test_group_experiment (fk_test_group,
                    fk_experiment,
                    experiment_hypothesis, conclusion, fk_user
                    )
                    VALUES (:tgid, :eid, :esub, :conc, :uid);";

                Db.CreateParameterFunc(cmd, "@tgid", testGroupExperiment.fkTestGroup, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@eid", testGroupExperiment.fkExperiment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@esub", testGroupExperiment.experimentHypothesis, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@conc", testGroupExperiment.conclusion, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@uid", testGroupExperiment.fkUser, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting Test group experiment relation", ex);
            }

            return 0;
        }
        public static int UpdateTestGroupExperiment(TestGroupExperiment testGroupExperiment)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.test_group_experiment
                        SET fk_test_group=:tgid, fk_experiment=:eid, experiment_hypothesis=:esub, conclusion=:conc
                        WHERE test_group_experiment_id=:tgeid;";

                Db.CreateParameterFunc(cmd, "@tgid", testGroupExperiment.fkTestGroup, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@eid", testGroupExperiment.fkExperiment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@esub", testGroupExperiment.experimentHypothesis, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@conc", testGroupExperiment.conclusion, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@tgeid", testGroupExperiment.testGroupExperimentId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Test group experiment relation info", ex);
            }
            return 0;
        }
        public static int UpdateTestGroupExperimentConclusion(long testGroupExperimentId, string experimentHypothesis, string conclusion)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.test_group_experiment
                        SET experiment_hypothesis=:esub, conclusion=:conc
                        WHERE test_group_experiment_id=:tgeid;";

                Db.CreateParameterFunc(cmd, "@esub", experimentHypothesis, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@conc", conclusion, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@tgeid", testGroupExperimentId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Test group experiment relation info", ex);
            }
            return 0;
        }
        public static int DeleteTestGroupExperiment(long testGroupExperimentId)
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
                            @"SELECT COUNT (tge.test_group_experiment_id)
                            FROM test_group_experiment tge
                            WHERE tge.fk_experiment=
                                (SELECT tge2.fk_experiment
                                    FROM test_group_experiment tge2
                                    WHERE tge2.test_group_experiment_id=:tgeid
                                )
                            ;";

                Db.CreateParameterFunc(cmd, "@tgeid", testGroupExperimentId, NpgsqlDbType.Bigint);

                long res = int.Parse(Db.ExecuteScalar(cmd));

                if (res <= 1)
                {
                    throw new Exception("An experiment must be assigned to at least one test group.");
                }

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.test_group_experiment
                                WHERE test_group_experiment_id=:tgeid;";

                Db.CreateParameterFunc(cmd, "@tgeid", testGroupExperimentId, NpgsqlDbType.Bigint);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        public static TestGroupExperiment CreateTestGroupExperimentObject(DataRow dr)
        {
            var testGroupExperiment = new TestGroupExperiment
            {
                testGroupExperimentId = (long)dr["test_group_experiment_id"],
                fkTestGroup = dr["fk_test_group"] != DBNull.Value ? int.Parse(dr["fk_test_group"].ToString()) : (int?)null,
                fkExperiment = dr["fk_experiment"] != DBNull.Value ? int.Parse(dr["fk_experiment"].ToString()) : (int?)null,
                fkUser = dr["fk_user"] != DBNull.Value ? int.Parse(dr["fk_user"].ToString()) : (int?)null,
                experimentHypothesis = dr["experiment_hypothesis"].ToString(),
                conclusion = dr["conclusion"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,
            };
            return testGroupExperiment;
        }
        private static TestGroupExperimentExt CreateTestGroupExperimentObjectExt(DataRow dr)
        {
            var testGroupExperiment = CreateTestGroupExperimentObject(dr);

            string operatorUsernameVar = dr.Table.Columns.Contains("operator_username") ? dr["operator_username"].ToString() : null;
            string researchGroupNameVar = dr.Table.Columns.Contains("research_group_name") ? dr["research_group_name"].ToString() : null;
            DateTime? dateCreatedExperimentVar = dr["date_created_experiment"] != DBNull.Value ? DateTime.Parse(dr["date_created_experiment"].ToString()) : (DateTime?)null;

            var testGroupExperimentExt = new TestGroupExperimentExt(testGroupExperiment)
            {
                experimentOperatorUsername = operatorUsernameVar,
                experimentResearchGroupName = researchGroupNameVar,
                experimentSystemLabel = dr["experiment_system_label"].ToString(),
                experimentPersonalLabel = dr["experiment_personal_label"].ToString(),
                testGroupName = dr["test_group_name"].ToString(),
                testGroupGoal = dr["test_group_goal"].ToString(),
                dateCreatedExperiment = dateCreatedExperimentVar,
            };
            return testGroupExperimentExt;
        }
    }
}