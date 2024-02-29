using Batteries.Dal.Base;
using Batteries.Models;
using Batteries.Models.Responses;
using NLog;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;

namespace Batteries.Dal
{
    public class ProjectResearchGroupDa
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static List<ProjectResearchGroupExt> GetAllProjectResearchGroups(int projectId)
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
                    @"SELECT prg.*, p.project_name, rg.*
                      FROM project_research_group prg
                      LEFT JOIN project p ON prg.fk_project = p.project_id
                      LEFT JOIN research_group rg ON prg.fk_research_group = rg.research_group_id
                      WHERE (prg.fk_project = :pid);";

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

            List<ProjectResearchGroupExt> list = (from DataRow dr in dt.Rows select CreateProjectResearchGroupObjectExt(dr)).ToList();

            return list;
        }
        public static int AddProjectResearchGroup(ProjectResearchGroup projectRG)
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
                            @"SELECT p.project_research_group_id 
                            FROM project_research_group p
                            WHERE fk_project = :pid AND
                            fk_research_group = :rgid; ";

                Db.CreateParameterFunc(cmd, "@pid", projectRG.fkProject, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", projectRG.fkResearchGroup, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count > 0)
                {
                    throw new Exception("Research group already exists in the project.");
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
                cmd.Parameters.Clear();
                cmd.CommandText =
                  @"INSERT INTO public.project_research_group (fk_project, fk_research_group, fk_user)
                    VALUES (:pid, :rgid, :uid)
                    RETURNING project_research_group_id;";

                Db.CreateParameterFunc(cmd, "@pid", projectRG.fkProject, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", projectRG.fkResearchGroup, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@uid", projectRG.fkUser, NpgsqlDbType.Integer);


                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting Project to Research group", ex);
            }

            return 0;
        }
        public static int DeleteProjectResearchGroup(int projectResearchGroupId)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.project_research_group
                                WHERE project_research_group_id=:pid;";

                Db.CreateParameterFunc(cmd, "@pid", projectResearchGroupId, NpgsqlDbType.Integer);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        public static ProjectResearchGroup CreateProjectResearchGroupObject(DataRow dr)
        {
            var projectRG = new ProjectResearchGroup
            {
                projectResearchGroupId = (int)dr["project_research_group_id"],
                fkProject = dr["fk_project"] != DBNull.Value ? int.Parse(dr["fk_project"].ToString()) : (int?)null,
                fkResearchGroup = dr["fk_research_group"] != DBNull.Value ? int.Parse(dr["fk_research_group"].ToString()) : (int?)null,
                fkUser = dr["fk_user"] != DBNull.Value ? int.Parse(dr["fk_user"].ToString()) : (int?)null,
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null
            };
            return projectRG;
        }
        private static ProjectResearchGroupExt CreateProjectResearchGroupObjectExt(DataRow dr)
        {
            var projectRG = CreateProjectResearchGroupObject(dr);

            string operatorUsernameVar = dr.Table.Columns.Contains("operator_username") ? dr["operator_username"].ToString() : null;
            string researchGroupNameVar = dr.Table.Columns.Contains("research_group_name") ? dr["research_group_name"].ToString() : null;
            string researchGroupAcronymVar = dr.Table.Columns.Contains("acronym") ? dr["acronym"].ToString() : null;
            string projectNameVar = dr.Table.Columns.Contains("project_name") ? dr["project_name"].ToString() : null;

            var ProjectExt = new ProjectResearchGroupExt(projectRG)
            {
                operatorUsername = operatorUsernameVar,
                researchGroupName = researchGroupNameVar,
                researchGroupAcronym = researchGroupAcronymVar,
                projectName = projectNameVar
            };
            return ProjectExt;
        }
    }
}