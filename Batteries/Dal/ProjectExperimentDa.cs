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
    public class ProjectExperimentDa
    {
        //public static List<ProjectExperimentExt> GetAllProjectExperiments(int? projectId = null, int? researchGroupId = null, int? experimentId = null, int? projectExperimentId = null)
        //{
        //    DataTable dt;
        //    try
        //    {
        //        var cmd = Db.CreateCommand();
        //        if (cmd.Connection.State != ConnectionState.Open)
        //        {
        //            cmd.Connection.Open();
        //        }
        //        //LEFT JOIN test_group tg e ON pe.fk_test_group = e.test_group_id
        //        cmd.CommandText =
        //            @"SELECT *, u.username as operator_username
        //                      FROM project_experiment pe
        //                      LEFT JOIN project p ON pe.fk_project = p.project_id
        //                      LEFT JOIN experiment e ON pe.fk_experiment = e.experiment_id


        //                      LEFT JOIN research_group rg ON e.fk_research_group = rg.research_group_id
        //                      LEFT JOIN users u ON e.fk_user = u.user_id

        //                      WHERE (pe.fk_project = :pid or :pid is null) and
        //                            (e.fk_research_group = :rgid or :rgid is null) and
        //                            (pe.fk_experiment = :eid or :eid is null) and
        //                            (pe.project_experiment_id = :peid or :peid is null);";

        //        Db.CreateParameterFunc(cmd, "@pid", projectId, NpgsqlDbType.Integer);
        //        Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
        //        Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
        //        Db.CreateParameterFunc(cmd, "@peid", projectExperimentId, NpgsqlDbType.Integer);

        //        dt = Db.ExecuteSelectCommand(cmd);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //    if (dt == null || dt.Rows.Count == 0)
        //    {
        //        return null;
        //    }

        //    List<ProjectExperimentExt> list = (from DataRow dr in dt.Rows select CreateProjectExperimentObjectExt(dr)).ToList();

        //    return list;
        //}


        ////public static List<ProjectExperimentExt> GetProjectsExperimentByName(string search = null, int? researchGroupId = null, int? page = 1)
        ////{
        ////    DataTable dt;

        ////    try
        ////    {
        ////        var cmd = Db.CreateCommand();
        ////        if (cmd.Connection.State != ConnectionState.Open)
        ////        {
        ////            cmd.Connection.Open();
        ////        }

        ////        cmd.CommandText =
        ////            @"SELECT *
        ////            FROM project_eperiment p
        ////            WHERE (lower(p.project_name) LIKE lower('%'|| :search ||'%') or :search is null) and
        ////            (p.fk_research_group = :rgid or :rgid is null)
        ////            ORDER BY p.date_created DESC
        ////            LIMIT 10 OFFSET :offset
        ////                ;";

        ////        Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
        ////        Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
        ////        Db.CreateParameterFunc(cmd, "@offset", (page - 1) * 10, NpgsqlDbType.Integer);


        ////        dt = Db.ExecuteSelectCommand(cmd);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw new Exception(ex.Message);
        ////    }
        ////    if (dt == null || dt.Rows.Count == 0)
        ////    {
        ////        return null;
        ////    }

        ////    List<ProjectExperimentExt> list = (from DataRow dr in dt.Rows select CreateProjectExperimentObjectExt(dr)).ToList();

        ////    return list;
        ////}





        //public static int AddProjectExperiment(ProjectExperiment projectExperiment)
        //{
        //    try
        //    {
        //        var cmd = Db.CreateCommand();
        //        if (cmd.Connection.State != ConnectionState.Open)
        //        {
        //            cmd.Connection.Open();
        //        }
        //        cmd.CommandText =
        //            @"INSERT INTO public.project_experiment (fk_project,
        //                fk_experiment, fk_test_group,
        //                experiment_hypothesis, conclusion)
        //            VALUES (:pid, :eid, :tgid, :esub, :conc);";

        //        Db.CreateParameterFunc(cmd, "@pid", projectExperiment.fkProject, NpgsqlDbType.Integer);
        //        Db.CreateParameterFunc(cmd, "@eid", projectExperiment.fkExperiment, NpgsqlDbType.Integer);
        //        Db.CreateParameterFunc(cmd, "@tgid", projectExperiment.fkTestGroup, NpgsqlDbType.Integer);
        //        Db.CreateParameterFunc(cmd, "@esub", projectExperiment.experimentHypothesis, NpgsqlDbType.Text);
        //        Db.CreateParameterFunc(cmd, "@conc", projectExperiment.conclusion, NpgsqlDbType.Text);

        //        Db.ExecuteNonQuery(cmd);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error inserting Project experiment relation", ex);
        //    }

        //    return 0;
        //}
        //public static int UpdateProjectExperiment(ProjectExperiment projectExperiment)
        //{
        //    try
        //    {
        //        var cmd = Db.CreateCommand();
        //        if (cmd.Connection.State != ConnectionState.Open)
        //        {
        //            cmd.Connection.Open();
        //        }
        //        cmd.CommandText =
        //            @"UPDATE public.project_experiment
        //                SET fk_project=:pid, fk_experiment=:eid, fk_test_group=:tgid, experiment_hypothesis=:esub, conclusion=:conc
        //                WHERE project_experiment_id=:peid;";

        //        Db.CreateParameterFunc(cmd, "@pid", projectExperiment.fkProject, NpgsqlDbType.Integer);
        //        Db.CreateParameterFunc(cmd, "@eid", projectExperiment.fkExperiment, NpgsqlDbType.Integer);
        //        Db.CreateParameterFunc(cmd, "@eid", projectExperiment.fkTestGroup, NpgsqlDbType.Integer);
        //        Db.CreateParameterFunc(cmd, "@esub", projectExperiment.experimentHypothesis, NpgsqlDbType.Text);
        //        Db.CreateParameterFunc(cmd, "@conc", projectExperiment.conclusion, NpgsqlDbType.Text);
        //        Db.CreateParameterFunc(cmd, "@peid", projectExperiment.projectExperimentId, NpgsqlDbType.Bigint);

        //        Db.ExecuteNonQuery(cmd);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error updating Project experiment relation info", ex);
        //    }
        //    return 0;
        //}

        //public static int UpdateProjectExperimentConclusion(long projectExperimentId, string experimentHypothesis, string conclusion)
        //{
        //    try
        //    {
        //        var cmd = Db.CreateCommand();
        //        if (cmd.Connection.State != ConnectionState.Open)
        //        {
        //            cmd.Connection.Open();
        //        }
        //        cmd.CommandText =
        //            @"UPDATE public.project_experiment
        //                SET experiment_hypothesis=:esub, conclusion=:conc
        //                WHERE project_experiment_id=:peid;";

        //        Db.CreateParameterFunc(cmd, "@esub", experimentHypothesis, NpgsqlDbType.Text);
        //        Db.CreateParameterFunc(cmd, "@conc", conclusion, NpgsqlDbType.Text);
        //        Db.CreateParameterFunc(cmd, "@peid", projectExperimentId, NpgsqlDbType.Bigint);

        //        Db.ExecuteNonQuery(cmd);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error updating Project experiment relation info", ex);
        //    }
        //    return 0;
        //}
        //public static int DeleteProjectExperiment(long projectExperimentId)
        //{
        //    try
        //    {
        //        var cmd = Db.CreateCommand();
        //        if (cmd.Connection.State != ConnectionState.Open)
        //        {
        //            cmd.Connection.Open();
        //        }
        //        cmd.CommandText =
        //                    @"SELECT COUNT (pe.project_experiment_id)
        //                    FROM project_experiment pe
        //                    WHERE pe.fk_experiment=
        //                        (SELECT pe2.fk_experiment
        //                            FROM project_experiment pe2
        //                            WHERE pe2.project_experiment_id=:peid
        //                        )
        //                    ;";

        //        Db.CreateParameterFunc(cmd, "@peid", projectExperimentId, NpgsqlDbType.Bigint);

        //        long res = int.Parse(Db.ExecuteScalar(cmd));

        //        if (res <= 1)
        //        {
        //            throw new Exception("An experiment must be assigned to at least one project.");
        //        }

        //        if (cmd.Connection.State != ConnectionState.Open)
        //        {
        //            cmd.Connection.Open();
        //        }
        //        cmd.CommandText =
        //            @"DELETE FROM public.project_experiment
        //                        WHERE project_experiment_id=:peid;";

        //        Db.CreateParameterFunc(cmd, "@peid", projectExperimentId, NpgsqlDbType.Bigint);
        //        Db.ExecuteNonQuery(cmd);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    return 0;
        //}
        //public static ProjectExperiment CreateProjectExperimentObject(DataRow dr)
        //{
        //    var ProjectExperiment = new ProjectExperiment
        //    {
        //        projectExperimentId = (int)dr["project_experiment_id"],
        //        fkProject = dr["fk_project"] != DBNull.Value ? int.Parse(dr["fk_project"].ToString()) : (int?)null,
        //        fkExperiment = dr["fk_experiment"] != DBNull.Value ? int.Parse(dr["fk_experiment"].ToString()) : (int?)null,
        //        //fkTestGroup = dr["fk_test_group"] != DBNull.Value ? int.Parse(dr["fk_test_group"].ToString()) : (int?)null,
        //        experimentHypothesis = dr["experiment_hypothesis"].ToString(),
        //        conclusion = dr["conclusion"].ToString(),
        //    };
        //    return ProjectExperiment;
        //}
        //private static ProjectExperimentExt CreateProjectExperimentObjectExt(DataRow dr)
        //{
        //    var projectExperiment = CreateProjectExperimentObject(dr);

        //    string operatorUsernameVar = dr.Table.Columns.Contains("operator_username") ? dr["operator_username"].ToString() : null;
        //    string researchGroupNameVar = dr.Table.Columns.Contains("research_group_name") ? dr["research_group_name"].ToString() : null;

        //    var projectExperimentExt = new ProjectExperimentExt(projectExperiment)
        //    {
        //        operatorUsername = operatorUsernameVar,
        //        researchGroupName = researchGroupNameVar,
        //        experimentSystemLabel = dr["experiment_system_label"].ToString(),
        //        experimentPersonalLabel = dr["experiment_personal_label"].ToString(),
        //        experimentDescription = dr["experiment_description"].ToString(),
        //        projectDescription = dr["project_description"].ToString(),
        //        projectName = dr["project_name"].ToString(),
        //        dateCreated = dr["date_created"].ToString(),
        //        dateModified = dr["date_modified"].ToString()

        //    };
        //    return projectExperimentExt;
        //}
    }
}