using Batteries.Dal.Base;
using Batteries.Models;
using Batteries.Models.Responses;
using NLog;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Batteries.Dal
{
    public class ProjectTestGroupDa
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
      
        public static List<ProjectTestGroupExt> GetAllTGProjects(int? projectTestGroupId = null, int? testGroupId = null)
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
                    @"SELECT *, u.username as operator_username, pr.project_name as project_namee
                              FROM project_test_group p
                              LEFT JOIN test_group tg ON p.fk_test_group = tg.test_group_id
                              LEFT JOIN users u on p.fk_user = u.user_id
                              LEFT JOIN project pr on p.fk_project = pr.project_id

                              WHERE (p.fk_test_group = :rgid or :rgid is null) and
                                    (p.project_test_group_id = :pid or :pid is null)
                                ORDER BY p.date_created DESC
                                    ;";

                Db.CreateParameterFunc(cmd, "@rgid", testGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pid", projectTestGroupId, NpgsqlDbType.Integer);

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

            List<ProjectTestGroupExt> list = (from DataRow dr in dt.Rows select CreateProjectObjectExt(dr)).ToList();

            return list;
        }


        public static List<ProjectTestGroupExt> GetProjectsByName(string search = null, int? testGroupId = null, int? page = 1)
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
                    FROM project_test_group p
                    WHERE (lower(p.fk_project) LIKE lower('%'|| :search ||'%') or :search is null) and
                    (p.fk_test_group = :rgid or :rgid is null)
                    ORDER BY p.date_created DESC
                    LIMIT 10 OFFSET :offset
                        ;";

                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@rgid", testGroupId, NpgsqlDbType.Integer);
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

            List<ProjectTestGroupExt> list = (from DataRow dr in dt.Rows select CreateProjectObjectExt(dr)).ToList();

            return list;
        }
       
        public static long AddProjectTG(ProjectTestGroup projectTG)
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
                            @"SELECT p.project_test_group_id 
                            FROM project_test_group p
                            LEFT JOIN test_group tg ON p.fk_test_group = tg.test_group_id
                            WHERE fk_project = :name AND
                            fk_test_group = :rgid; ";

                Db.CreateParameterFunc(cmd, "@name", projectTG.fkProject, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", projectTG.fkTestGroup, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count > 0)
                {
                    throw new Exception("Test group already exists in the project.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            //not updated
            //long result = 0;
            try
            {
                //var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                cmd.CommandText =
                  @"INSERT INTO public.project_test_group (fk_project, fk_test_group, fk_user, date_created)
                    VALUES (:mtid, :sit, :n, now()::timestamp)RETURNING project_test_group_id;";

                Db.CreateParameterFunc(cmd, "@mtid", projectTG.fkProject, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@sit", projectTG.fkTestGroup, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@n", projectTG.fkUser, NpgsqlDbType.Integer);


                Db.ExecuteNonQuery(cmd);
                //result = long.Parse(Db.ExecuteScalar(cmd));


            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting Project to Test group", ex);
            }

            return 0;
        }
        public static int UpdateProject(ProjectTestGroup project)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.project_test_group
                            SET fk_project=:name, fk_test_group=:rgid
                            WHERE project_test_group_id=:pid;";

                Db.CreateParameterFunc(cmd, "@name", project.fkProject, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@rgid", project.fkTestGroup, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);

            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Project info", ex);
            }
            return 0;
        }
        public static int DeleteProject(int projectTestGroupId)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.project_test_group
                                WHERE project_test_group_id=:pid;";

                Db.CreateParameterFunc(cmd, "@pid", projectTestGroupId, NpgsqlDbType.Integer);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }

        
        public static ProjectTestGroup CreateProjectObject(DataRow dr)
        {
            var projectTG = new ProjectTestGroup
            {
                projectTestGroupId = (int)dr["project_test_group_id"],
                fkProject = dr["fk_project"] != DBNull.Value ? int.Parse(dr["fk_project"].ToString()) : (int?)null,
                fkTestGroup = dr["fk_test_group"] != DBNull.Value ? int.Parse(dr["fk_test_group"].ToString()) : (int?)null,
                fkUser = dr["fk_user"] != DBNull.Value ? int.Parse(dr["fk_user"].ToString()) : (int?)null,
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null
            };
            return projectTG;
        }
        private static ProjectTestGroupExt CreateProjectObjectExt(DataRow dr)
        {
            var projectTG = CreateProjectObject(dr);

            string operatorUsernameVar = dr.Table.Columns.Contains("operator_username") ? dr["operator_username"].ToString() : null;
            string testGroupNameVar = dr.Table.Columns.Contains("test_group_name") ? dr["test_group_name"].ToString() : null;
            string projectNameVar = dr.Table.Columns.Contains("project_namee") ? dr["project_namee"].ToString() : null;


            var ProjectExt = new ProjectTestGroupExt(projectTG)
            {
                operatorUsername = operatorUsernameVar,
                testGroupName = testGroupNameVar,
                projectName = projectNameVar
            };
            return ProjectExt;
        }
    }
}