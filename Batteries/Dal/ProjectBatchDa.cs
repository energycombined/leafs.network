using Batteries.Dal.Base;
using Batteries.Models;
using Batteries.Models.Responses;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Batteries.Dal
{
    public class ProjectBatchDa
    {
        public static List<ProjectBatchExt> GetAllProjectBatches(int? projectId = null)
        {
            DataTable dt;
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                //LEFT JOIN test_group tg e ON pe.fk_test_group = e.test_group_id
                cmd.CommandText =
                    @"SELECT *
                              FROM project_batch pb
                              LEFT JOIN project p ON pb.fk_project = p.project_id

                              WHERE (pb.fk_project = :pid or :pid is null);";

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

            List<ProjectBatchExt> list = (from DataRow dr in dt.Rows select CreateProjectBatchObjectExt(dr)).ToList();

            return list;
        }

        public static List<ProjectBatchExt> GetAllBatches(int? batchId = null)
        {
            DataTable dt;
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                //LEFT JOIN test_group tg e ON pe.fk_test_group = e.test_group_id
                cmd.CommandText =
                    @"SELECT *
                              FROM project_batch pb
                              LEFT JOIN batch b ON pb.fk_batch = b.batch_id
                              LEFT JOIN project p ON pb.fk_project = p.project_id

                              WHERE (pb.fk_batch = :bid or :bid is null);";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);

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

            List<ProjectBatchExt> list = (from DataRow dr in dt.Rows select CreateProjectBatchObjectExt(dr)).ToList();

            return list;
        }

        public static List<ProjectBatchExt> GetAllProjectBatchesExperiment(int? projectId = null, int? experimentId = null)
        {
            DataTable dt;
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                //LEFT JOIN test_group tg e ON pe.fk_test_group = e.test_group_id
                cmd.CommandText =
                    @"SELECT *
                              FROM project_batch pb
                              LEFT JOIN project p ON pb.fk_project = p.project_id
                              LEFT JOIN experiment e ON pb.fk_coming_experiment = e.experiment_id
                              

                              WHERE (pb.fk_project = :pid or :pid is null) and
                                    (pb.fk_coming_experiment = :eid or :eid is null);";

                Db.CreateParameterFunc(cmd, "@pid", projectId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);

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

            List<ProjectBatchExt> list = (from DataRow dr in dt.Rows select CreateProjectBatchObjectExt(dr)).ToList();

            return list;
        }

        public static int DeleteProjectBatch(int projectBatchId)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                            @"SELECT COUNT (pb.project_batch_id)
                            FROM project_batch pb
                            WHERE pb.fk_project=
                                (SELECT pb2.fk_project
                                    FROM project_batch pb2
                                    WHERE pb2.project_batch_id=:peid
                                )
                            ;";

                Db.CreateParameterFunc(cmd, "@peid", projectBatchId, NpgsqlDbType.Integer);

                long res = int.Parse(Db.ExecuteScalar(cmd));



                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.project_batch
                                WHERE project_batch_id=:peid;";

                Db.CreateParameterFunc(cmd, "@peid", projectBatchId, NpgsqlDbType.Bigint);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        public static bool BatchExistsInProject(int batchId, int projectId)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"SELECT project_batch_id
                      FROM public.project_batch
                      WHERE fk_batch = :bid AND
                            fk_project = :pid AND
                            fk_coming_experiment IS NULL AND 
                            fk_coming_batch IS NULL
                    ;";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pid", projectId, NpgsqlDbType.Integer);

                var result = Db.ExecuteScalar(cmd);
                if (result != "") return true;
                else return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting Project batch relation", ex);
            }
        }

        public static int AddProjectBatch(ProjectBatch projectBatch, NpgsqlCommand cmd)
        {
            bool isEnclosedInTransaction = true;

            try
            {
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

                cmd.CommandText =
                    @"INSERT INTO public.project_batch (fk_project,
                        fk_coming_experiment,
                        fk_batch,
                        fk_coming_batch,
                        fk_user,
                        added_manually
                    )
                    VALUES (:pid, :eid, :bid, :cbid, :uid, :manual);";

                Db.CreateParameterFunc(cmd, "@pid", projectBatch.fkProject, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@eid", projectBatch.fkComingExperiment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bid", projectBatch.fkBatch, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@cbid", projectBatch.fkComingBatch, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@uid", projectBatch.fkUser, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@manual", projectBatch.addedManually, NpgsqlDbType.Boolean);

                var result = Db.ExecuteNonQuery(cmd, false);

                if (!isEnclosedInTransaction)
                {
                    cmd.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting project Batch", ex);
            }

            return 0;
        }

        public static int AddProjectBatchList(List<ProjectBatch> projectBatchList)
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
                foreach (ProjectBatch projectBatch in projectBatchList)
                {
                    var result = AddProjectBatch(projectBatch, cmd);
                }

                t.Commit();
                cmd.Connection.Close();
            }
            catch (Exception e)
            {
                t.Rollback();
                //return 5;
                throw new Exception("Error when adding batch to project");
            }

            return 0;
        }
        public static ProjectBatch CreateProjectBatchObject(DataRow dr)
        {

            var projectBatch = new ProjectBatch
            {
                projectBatchId = (int)dr["project_batch_id"],
                fkBatch = dr["fk_batch"] != DBNull.Value ? int.Parse(dr["fk_batch"].ToString()) : (int?)null,
                fkComingExperiment = dr["fk_coming_experiment"] != DBNull.Value ? int.Parse(dr["fk_coming_experiment"].ToString()) : (int?)null,
                fkProject = dr["fk_project"] != DBNull.Value ? int.Parse(dr["fk_project"].ToString()) : (int?)null,
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,
                fkComingBatch = dr["fk_coming_batch"] != DBNull.Value ? int.Parse(dr["fk_coming_batch"].ToString()) : (int?)null
            };
            return projectBatch;

        }


        public static ProjectBatchExt CreateProjectBatchObjectExt(DataRow dr)
        {
            var projectBatch = CreateProjectBatchObject(dr);

            string projectNameVar = dr.Table.Columns.Contains("fk_project_name") ? dr["fk_project_name"].ToString() : null;
            string batchNameVar = dr.Table.Columns.Contains("fk_batch") ? dr["fk_batch"].ToString() : null;
            string experimentNameVar = dr.Table.Columns.Contains("fk_coming_experiment") ? dr["fk_coming_experiment"].ToString() : null;

            var projectBatchExt = new ProjectBatchExt(projectBatch)
            {
                projectName = projectNameVar,
                batchName = batchNameVar,
                experimentName = experimentNameVar,
            };
            return projectBatchExt;
        }
    }
}