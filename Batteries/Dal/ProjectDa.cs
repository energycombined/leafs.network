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
    public class ProjectDa
    {
        public static string GetAllProjectsJsonForDropdown(int? projectId = null, int? researchGroupId = null)
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
                      FROM ( select project_id as value, COALESCE(pj.project_name, '') as text
                              from project pj
                              where (pj.fk_research_group = :rgid or :rgid is null) and
                                    (pj.project_id = :pjid or :pjid is null)
                          ) as t;";

                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pjid", projectId, NpgsqlDbType.Integer);

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

        public static List<ProjectExt> CountAllProjects()
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
                    @"SELECT * FROM public.project;";

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

            List<ProjectExt> list = (from DataRow dr in dt.Rows select CreateProjectObjectExt(dr)).ToList();

            return list;
        }

        /// <summary>
        /// Get a list of projects where researchGroupId is participant
        /// </summary>
        /// <param name="researchGroupId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public static List<ProjectExt> GetProjectsByRGParticipant(int researchGroupId, int? projectId = null)
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
                    @"SELECT *, u.username as operator_username, uo.username as editing_operator_username
                              FROM project p                    
                              LEFT JOIN research_group rg ON p.fk_research_group = rg.research_group_id
                              LEFT JOIN project_research_group prg ON p.project_id = prg.fk_project
                              LEFT JOIN users u on p.fk_operator = u.user_id
                              LEFT JOIN users uo on p.fk_edited_by = uo.user_id

                              WHERE (prg.fk_research_group = :rgid) AND
                                    (p.project_id = :pid or :pid is null)
                                ORDER BY p.date_created DESC
                                    ;";

                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
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

            List<ProjectExt> list = (from DataRow dr in dt.Rows select CreateProjectObjectExt(dr)).ToList();

            return list;
        }
        /// <summary>
        /// Get project by id
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public static ProjectExt GetProjectById(int projectId)
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
                    @"SELECT *, u.username as operator_username, uo.username as editing_operator_username
                              FROM project p                    
                              LEFT JOIN research_group rg ON p.fk_research_group = rg.research_group_id                              
                              LEFT JOIN users u on p.fk_operator = u.user_id
                              LEFT JOIN users uo on p.fk_edited_by = uo.user_id

                              WHERE (p.project_id = :pid or :pid is null)
                              ORDER BY p.date_created DESC
                                    ;";
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

            ProjectExt result = CreateProjectObjectExt(dt.Rows[0]);

            return result;
        }
        public static Project GetLastUsedProject(int userId)
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
                    @"SELECT * FROM experiment e
                    LEFT JOIN project p ON e.fk_project=p.project_id
                    WHERE e.fk_user=:uid
                    ORDER BY e.experiment_id DESC
                    LIMIT 1;";
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

            Project result = CreateProjectObject(dt.Rows[0]);

            return result;
        }
        /// <summary>
        /// Get all projects where researchGroupId is participant
        /// </summary>
        /// <param name="search"></param>
        /// <param name="researchGroupId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static List<ProjectExt> GetProjectsByRGParticipantPaged(string search = null, int? researchGroupId = null, int? page = 1)
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
                    FROM project_research_group pr
                    LEFT JOIN project p ON p.project_id = pr.fk_project                    
                    WHERE ((lower(p.project_name) LIKE lower('%'|| :search ||'%') OR lower(p.project_acronym) LIKE lower('%'|| :search ||'%')) OR :search is null)
                    AND
                    (pr.fk_research_group = :rgid or :rgid is null)
                    ORDER BY p.date_created DESC
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

            List<ProjectExt> list = (from DataRow dr in dt.Rows select CreateProjectObjectExt(dr)).ToList();

            return list;
        }
        public static bool IsParticipant(int? projectId = null, int? researchGroupId = null)
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
                    @"SELECT * FROM project_research_group prg
                        LEFT JOIN project pr ON prg.fk_project = pr.project_id   
						 	 
							   WHERE prg.fk_project = :pid  
                               AND   prg.fk_research_group = :rgid;";

                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pid", projectId, NpgsqlDbType.Integer);

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
        public static List<ProjectExt> GetProjectsByRGCreatorPaged(string search = null, int? researchGroupCreatorId = null, int? page = 1)
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
                    FROM project p
                    WHERE (lower(p.project_name) LIKE lower('%'|| :search ||'%') or :search is null) and
                    (p.fk_research_group = :rgid or :rgid is null)
                    ORDER BY p.date_created DESC
                    LIMIT 10 OFFSET :offset
                        ;";

                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupCreatorId, NpgsqlDbType.Integer);
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

            List<ProjectExt> list = (from DataRow dr in dt.Rows select CreateProjectObjectExt(dr)).ToList();

            return list;
        }
        public static List<ProjectExt> GetProjectsByExperimentExcluded(string search, int experimentId, int? researchGroupId = null, int? page = 1)
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
                    FROM project p
                    WHERE
                      p.project_id NOT IN(
                        SELECT pe.fk_project
                              FROM project_experiment pe

                              WHERE
                                    (pe.fk_experiment = :eid)
                      ) and
                      (lower(p.project_name) LIKE lower('%'|| :search ||'%') or :search is null) and
                    (p.fk_research_group = :rgid or :rgid is null)

                    ORDER BY p.date_created DESC
                    LIMIT 10 OFFSET :offset
                        ;";

                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
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

            List<ProjectExt> list = (from DataRow dr in dt.Rows select CreateProjectObjectExt(dr)).ToList();

            return list;
        }

        public static int AddProject(Project project)
        {
            if (project.endProject < project.startProject)
            {
                throw new Exception("Тhe end date cannot be before the start date.");
            }

            int returnetProjectId = 0;
            var cmd = Db.CreateCommand();
            try
            {
                DataTable dt;
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                            @"SELECT p.project_id 
                            FROM project p
                            WHERE (lower(p.project_name) = lower(:name)) AND
                            fk_research_group = :rgid;";

                Db.CreateParameterFunc(cmd, "@name", project.projectName, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@rgid", project.fkResearchGroup, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count > 0)
                {
                    throw new Exception("Project name already exists.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }

            NpgsqlTransaction t = cmd.Connection.BeginTransaction();

            try
            {

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"INSERT INTO public.project (
                        project_name, project_acronym, administrative_coordinator, administrative_coordinator_contact, administrative_coordinator_email, technical_coordinator,
                        technical_coordinator_contact, technical_coordinator_email, innovation_manager, innovation_manager_contact, dissemination_coordinator, 
                        dissemination_coordinator_contact, grant_funding_organisation, funding_programme, call_Identifier, call_Topic, fixed_keywords, free_keywords, 
                        start_project, end_project,                         
                        fk_research_group,
                        fk_operator,
                        project_description,
                        list_of_partners 
                        )
                    VALUES (:name, :acr, :adc, :acc, :ace, :tec, :tcc, :tce, :inm, :imc, :dic, :dcc, :gfo, :fpr, :cai, :cat, 
                            :fik, :frk, :spr, :epr, :rgid, :uid, :goal, :lop)
                    RETURNING project_id;";

                Db.CreateParameterFunc(cmd, "@name", project.projectName, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@acr", project.projectAcronym, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@adc", project.administrativeCoordinator, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@acc", project.administrativeCoordinatorContact, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@ace", project.administrativeCoordinatorEmail, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@tec", project.technicalCoordinator, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@tcc", project.technicalCoordinatorContact, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@tce", project.technicalCoordinatorEmail, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@inm", project.innovationManager, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@imc", project.innovationManagerContact, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@dic", project.disseminationCoordinator, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@dcc", project.disseminationCoordinatorContact, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@gfo", project.grantFundingOrganisation, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@fpr", project.fundingProgramme, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@cai", project.callIdentifier, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@cat", project.callTopic, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@fik", project.fixedKeywords, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@frk", project.freeKeywords, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@spr", project.startProject, NpgsqlDbType.Date);
                Db.CreateParameterFunc(cmd, "@epr", project.endProject, NpgsqlDbType.Date);
                Db.CreateParameterFunc(cmd, "@rgid", project.fkResearchGroup, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@uid", project.fkOperator, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@goal", project.projectDescription, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lop", project.listOfPartners, NpgsqlDbType.Integer);


                returnetProjectId = (int)cmd.ExecuteScalar();
                if (returnetProjectId <= 0)
                {
                    t.Rollback();
                    throw new Exception("Error inserting new experiment");
                }

                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.project
                        SET date_created=now()::timestamp 
                        WHERE project_id=:eid;";

                Db.CreateParameterFunc(cmd, "@eid", returnetProjectId, NpgsqlDbType.Integer);
                var res = Db.ExecuteNonQuery(cmd, false);
                if (res <= 0)
                {
                    t.Rollback();
                    throw new Exception("Error updating experiment system label info");
                }
                t.Commit();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                t.Rollback();
                throw new Exception(ex.Message);
            }
            return returnetProjectId;

            //    Db.ExecuteNonQuery(cmd);
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("Error inserting Project", ex);
            //}

            //return 0;
        }
        public static int UpdateProject(Project project)
        {
            if (project.endProject < project.startProject)
            {
                throw new Exception("Тhe end date cannot be before the start date.");
            }

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                cmd.CommandText =
                    @"UPDATE public.project
                    SET project_name=:name, project_acronym=:acr, administrative_coordinator=:adc, administrative_coordinator_contact=:acc, administrative_coordinator_email=:ace, technical_coordinator=:tec, technical_coordinator_contact=:tcc, technical_coordinator_email=:tce, innovation_manager=:inm, innovation_manager_contact=:imc, dissemination_coordinator=:dic, dissemination_coordinator_contact=:dcc, grant_funding_organisation=:gfo, funding_programme=:fpr, call_identifier=:cai, call_topic=:cat, fixed_keywords=:fik, free_keywords=:frk, start_project=:spr, end_project=:epr, fk_research_group=:rgid, project_description=:goal, list_of_partners=:lop, fk_edited_by=:uid, last_change=now()::timestamp                       
                    WHERE project_id=:pid;";


                Db.CreateParameterFunc(cmd, "@name", project.projectName, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@acr", project.projectAcronym, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@adc", project.administrativeCoordinator, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@acc", project.administrativeCoordinatorContact, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@ace", project.administrativeCoordinatorEmail, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@tec", project.technicalCoordinator, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@tcc", project.technicalCoordinatorContact, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@tce", project.technicalCoordinatorEmail, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@inm", project.innovationManager, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@imc", project.innovationManagerContact, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@dic", project.disseminationCoordinator, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@dcc", project.disseminationCoordinatorContact, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@gfo", project.grantFundingOrganisation, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@fpr", project.fundingProgramme, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@cai", project.callIdentifier, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@cat", project.callTopic, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@fik", project.fixedKeywords, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@frk", project.freeKeywords, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@spr", project.startProject, NpgsqlDbType.Date);
                Db.CreateParameterFunc(cmd, "@epr", project.endProject, NpgsqlDbType.Date);
                Db.CreateParameterFunc(cmd, "@rgid", project.fkResearchGroup, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@goal", project.projectDescription, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lop", project.listOfPartners, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@uid", project.fkEditedBy, NpgsqlDbType.Integer);

                Db.CreateParameterFunc(cmd, "@pid", project.projectId, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Project info", ex);
            }
            return 0;
        }
        public static int DeleteProject(int projectId)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                cmd.CommandText =
                    @"DELETE FROM public.project
                                WHERE project_id=:pid;";

                Db.CreateParameterFunc(cmd, "@pid", projectId, NpgsqlDbType.Integer);
                Db.ExecuteNonQuery(cmd);

            }
            catch (Exception ex)
            {
                throw new Exception("Please, first remove Research groups added to this project!");
                //throw new Exception(ex.Message);
            }
            return 0;
        }

        public static Project CreateProjectObject(DataRow dr)
        {
            var project = new Project
            {
                projectId = (int)dr["project_id"],
                projectDescription = dr["project_description"].ToString(),
                projectName = dr["project_name"].ToString(),
                projectAcronym = dr["project_acronym"].ToString(),
                administrativeCoordinator = dr["administrative_coordinator"].ToString(),
                administrativeCoordinatorContact = dr["administrative_coordinator_contact"].ToString(),
                administrativeCoordinatorEmail = dr["administrative_coordinator_email"].ToString(),
                technicalCoordinator = dr["technical_coordinator"].ToString(),
                technicalCoordinatorContact = dr["technical_coordinator_contact"].ToString(),
                technicalCoordinatorEmail = dr["technical_coordinator_email"].ToString(),
                innovationManager = dr["innovation_Manager"].ToString(),
                innovationManagerContact = dr["innovation_Manager_contact"].ToString(),
                disseminationCoordinator = dr["dissemination_coordinator"].ToString(),
                disseminationCoordinatorContact = dr["dissemination_coordinator_contact"].ToString(),
                grantFundingOrganisation = dr["grant_funding_organisation"].ToString(),
                fundingProgramme = dr["funding_Programme"].ToString(),
                callIdentifier = dr["call_Identifier"].ToString(),
                callTopic = dr["call_Topic"].ToString(),
                fixedKeywords = dr["fixed_keywords"].ToString(),
                freeKeywords = dr["free_keywords"].ToString(),
                listOfPartners = dr["list_of_partners"] != DBNull.Value ? int.Parse(dr["list_of_partners"].ToString()) : (int?)null,
                startProject = dr["start_Project"] != DBNull.Value ? DateTime.Parse(dr["start_Project"].ToString()) : (DateTime?)null,
                endProject = dr["end_Project"] != DBNull.Value ? DateTime.Parse(dr["end_Project"].ToString()) : (DateTime?)null,
                fkResearchGroup = dr["fk_research_group"] != DBNull.Value ? int.Parse(dr["fk_research_group"].ToString()) : (int?)null,
                fkOperator = dr["fk_operator"] != DBNull.Value ? int.Parse(dr["fk_operator"].ToString()) : (int?)null,
                fkEditedBy = dr["fk_edited_by"] != DBNull.Value ? int.Parse(dr["fk_edited_by"].ToString()) : (int?)null,
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,
                lastChange = dr["last_change"] != DBNull.Value ? DateTime.Parse(dr["last_change"].ToString()) : (DateTime?)null
            };
            return project;
        }
        private static ProjectExt CreateProjectObjectExt(DataRow dr)
        {
            var project = CreateProjectObject(dr);

            string operatorUsernameVar = dr.Table.Columns.Contains("operator_username") ? dr["operator_username"].ToString() : null;
            string researchGroupNameVar = dr.Table.Columns.Contains("research_group_name") ? dr["research_group_name"].ToString() : null;
            string researchGroupAcronymVar = dr.Table.Columns.Contains("acronym") ? dr["acronym"].ToString() : null;
            string editingOperatorUsernameVar = dr.Table.Columns.Contains("editing_operator_username") ? dr["editing_operator_username"].ToString() : null;

            var ProjectExt = new ProjectExt(project)
            {
                operatorUsername = operatorUsernameVar,
                researchGroupName = researchGroupNameVar,
                researchGroupAcronym = researchGroupAcronymVar,
                editingOperatorUsername = editingOperatorUsernameVar
            };
            return ProjectExt;
        }
    }
}