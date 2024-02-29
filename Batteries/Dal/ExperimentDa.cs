using Batteries.Dal.Base;
using Batteries.Dal.EquipmentDal;
using Batteries.Dal.ProcessesDal;
using Batteries.Helpers;
using Batteries.Models;
using Batteries.Models.EquipmentModels;
using Batteries.Models.ProcessModels;
using Batteries.Models.Requests;
using Batteries.Models.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Batteries.Dal
{
    public class ExperimentDa
    {
        public static string GetAllExperimentsJsonForDropdown(int? experimentId = null, int? researchGroupId = null)
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
                      FROM ( select experiment_id as value, COALESCE(tg.experiment_personal_label, '') as text
                              from experiment e
                              where (tg.fk_research_group = :rgid or :rgid is null) and
                                    (e.experiment_id = :eid or :eid is null)
                          ) as t;";

                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@tgid", experimentId, NpgsqlDbType.Integer);

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
        /// <summary>
        /// Get ALL experiments that research group Id is meant to see
        /// </summary>
        /// <param name="researchGroupId"></param>
        /// <param name="experimentId"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static List<ExperimentExt> GetAllExperimentsListForResearchGroup(int researchGroupId, int? experimentId = null, bool desc = true)
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
                    @"SELECT *, u.username as operator_username, edu.username as editing_operator_username, pr.project_name as project_name, e.fk_sharing_type as fk_sharing_type,
                    esa.total_active_materials as anode_total_active_materials, esa.active_materials as anode_active_materials, esa.active_percentages as anode_active_percentages,
                    esc.total_active_materials as cathode_total_active_materials, esc.active_materials as cathode_active_materials, esc.active_percentages as cathode_active_percentages

                              FROM experiment e
                              LEFT JOIN project pr ON e.fk_project = pr.project_id
                              LEFT JOIN project_research_group prg ON pr.project_id = prg.fk_project

                              LEFT JOIN research_group rg ON e.fk_research_group = rg.research_group_id
                              LEFT JOIN users u ON e.fk_user = u.user_id
                              LEFT JOIN users edu on e.fk_edited_by = edu.user_id

                              LEFT JOIN experiment_summary esa ON esa.fk_experiment = e.experiment_id AND esa.fk_battery_component_type=1
                              LEFT JOIN experiment_summary esc ON esc.fk_experiment = e.experiment_id AND esc.fk_battery_component_type=2

                              WHERE (e.is_complete = true) and
                                    (e.experiment_id = :eid or :eid is null) and
                                    (prg.fk_research_group = :rgid)
                              ORDER BY e.date_modified " + (desc ? @"DESC" : @"ASC") + @", e.experiment_id " + (desc ? @"DESC" : @"ASC") + @"
                        ;";

                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@ord", order, NpgsqlDbType.);

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

            List<ExperimentExt> list = (from DataRow dr in dt.Rows select CreateExperimentObjectExt(dr)).ToList();

            return list;
        }
        /// <summary>
        /// Get public experiments
        /// </summary>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static List<ExperimentExt> GetAllPublicExperimentsList(bool desc = true)
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
                    @"SELECT *, pr.project_name as project_name, e.fk_sharing_type as fk_sharing_type,
                    esa.total_active_materials as anode_total_active_materials, esa.active_materials as anode_active_materials, esa.active_percentages as anode_active_percentages,
                    esc.total_active_materials as cathode_total_active_materials, esc.active_materials as cathode_active_materials, esc.active_percentages as cathode_active_percentages

                              FROM experiment e
                              LEFT JOIN project pr ON e.fk_project = pr.project_id                              
                              LEFT JOIN research_group rg ON e.fk_research_group = rg.research_group_id

                              LEFT JOIN experiment_summary esa ON esa.fk_experiment = e.experiment_id AND esa.fk_battery_component_type=1
                              LEFT JOIN experiment_summary esc ON esc.fk_experiment = e.experiment_id AND esc.fk_battery_component_type=2

                              WHERE (e.is_complete = true) AND
                                    (e.fk_sharing_type = 3) 
                              ORDER BY e.date_modified " + (desc ? @"DESC" : @"ASC") + @", e.experiment_id " + (desc ? @"DESC" : @"ASC") + @"
                        ;";

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

            List<ExperimentExt> list = (from DataRow dr in dt.Rows select CreateExperimentObjectExt(dr)).ToList();

            return list;
        }
        /// <summary>
        /// Get experiments from (all) other research groups, experiments from own RG are excluded
        /// </summary>
        /// <param name="myResearchGroupId"></param>
        /// <param name="experimentId"></param>
        /// <param name="otherResearchGroupId"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static List<ExperimentExt> GetExperimentsListFromOtherRG(int myResearchGroupId, int? experimentId = null, int? otherResearchGroupId = null, bool desc = true)
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
                    @"SELECT *, u.username as operator_username, edu.username as editing_operator_username, 
esa.total_active_materials as anode_total_active_materials, esa.active_materials as anode_active_materials, esa.active_percentages as anode_active_percentages,
esc.total_active_materials as cathode_total_active_materials, esc.active_materials as cathode_active_materials, esc.active_percentages as cathode_active_percentages

                              FROM experiment e
                              LEFT JOIN research_group rg ON e.fk_research_group = rg.research_group_id
                              LEFT JOIN users u ON e.fk_user = u.user_id
LEFT JOIN users edu on e.fk_edited_by = edu.user_id
LEFT JOIN experiment_summary esa ON esa.fk_experiment = e.experiment_id AND esa.fk_battery_component_type=1
LEFT JOIN experiment_summary esc ON esc.fk_experiment = e.experiment_id AND esc.fk_battery_component_type=2

                              WHERE (e.is_complete = true) and
                                    (e.fk_research_group != :myrgid) and
                                    (e.fk_research_group = :orgid or :orgid is null) and
                                    (e.experiment_id = :eid or :eid is null)
                              ORDER BY e.date_created " + (desc ? @"DESC" : @"ASC") + @"
                        ;";

                Db.CreateParameterFunc(cmd, "@myrgid", myResearchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@orgid", otherResearchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@ord", order, NpgsqlDbType.);

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

            List<ExperimentExt> list = (from DataRow dr in dt.Rows select CreateExperimentObjectExt(dr)).ToList();

            return list;
        }
        /// <summary>
        /// Get experiment by id, limited to what researchGroupId can see
        /// </summary>
        /// <param name="experimentId"></param>
        /// <param name="researchGroupId"></param>
        /// <returns></returns>
        public static ExperimentExt GetExperimentById(int experimentId, int researchGroupId)
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
                    @"SELECT *, u.username as operator_username
                              FROM experiment e
                              LEFT JOIN project pr ON e.fk_project = pr.project_id
                              LEFT JOIN project_research_group prg ON pr.project_id = prg.fk_project

                              LEFT JOIN research_group rg ON e.fk_research_group = rg.research_group_id
                              LEFT JOIN users u ON e.fk_user = u.user_id
                              WHERE (e.is_complete = true) and
                                    (prg.fk_research_group = :rgid) and
                                    (e.experiment_id = :eid or :eid is null)
                              ORDER BY e.date_created DESC
                        ;";

                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
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
            //List<ExperimentExt> list = (from DataRow dr in dt.Rows select CreateExperimentObjectExt(dr)).ToList();

            return CreateExperimentObjectExt(dt.Rows[0]);
        }
        /// <summary>
        /// Get experiment by id, and Research group creator
        /// </summary>
        /// <param name="experimentId"></param>
        /// <param name="researchGroupCreatorId"></param>
        /// <returns></returns>
        public static ExperimentExt GetExperimentByIdAndRGCreator(int experimentId, int researchGroupCreatorId)
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
                    @"SELECT *, u.username as operator_username
                              FROM experiment e
                              LEFT JOIN project pr ON e.fk_project = pr.project_id
                              LEFT JOIN research_group rg ON e.fk_research_group = rg.research_group_id
                              LEFT JOIN users u ON e.fk_user = u.user_id
                              WHERE (e.is_complete = true) and
                                    (e.fk_research_group = :rgid) and
                                    (e.experiment_id = :eid or :eid is null)
                              ORDER BY e.date_created DESC
                        ;";

                Db.CreateParameterFunc(cmd, "@rgid", researchGroupCreatorId, NpgsqlDbType.Integer);
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
            //List<ExperimentExt> list = (from DataRow dr in dt.Rows select CreateExperimentObjectExt(dr)).ToList();

            return CreateExperimentObjectExt(dt.Rows[0]);
        }
        /// <summary>
        /// Get ALL completed experiments by id list
        /// </summary>
        /// <param name="experimentIdArray"></param>
        /// <param name="researchGroupIdCreator"></param>
        /// <returns></returns>
        public static List<ExperimentExt> GetExperimentsById(int[] experimentIdArray = null, int? researchGroupIdCreator = null)
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
                    @"SELECT *, u.username as operator_username
                                  FROM experiment e
                                  LEFT JOIN project pr ON e.fk_project = pr.project_id                                  
                                  LEFT JOIN research_group rg ON e.fk_research_group = rg.research_group_id
                                  LEFT JOIN users u ON e.fk_user = u.user_id
                                  WHERE (e.is_complete = true) and
                                        (e.fk_research_group = :rgid or :rgid is null) and
                                        (e.experiment_id = ANY(:eidList) or :eidList is null)
                                  ORDER BY e.date_created DESC
                            ;";

                Db.CreateParameterFunc(cmd, "@rgid", researchGroupIdCreator, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@eidList", experimentIdArray, NpgsqlDbType.Array | NpgsqlDbType.Integer);

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

            List<ExperimentExt> list = (from DataRow dr in dt.Rows select CreateExperimentObjectExt(dr)).ToList();

            return list;
        }

        /// <summary>
        /// Get unfinished experiments from research group id creator
        /// </summary>
        /// <param name="experimentId"></param>
        /// <param name="researchGroupIdCreator"></param>
        /// <returns></returns>
        public static List<ExperimentExt> GetUnfinishedExperimentsList(int? experimentId = null, int? researchGroupIdCreator = null)
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
                    @"SELECT *, u.username as operator_username, edu.username as editing_operator_username, p.project_name as project_name
                              FROM experiment e
                              LEFT JOIN project p ON e.fk_project = p.project_id
                              LEFT JOIN research_group rg ON e.fk_research_group = rg.research_group_id
                              LEFT JOIN users u ON e.fk_user = u.user_id
                              LEFT JOIN users edu on e.fk_edited_by = edu.user_id
                              WHERE (e.is_complete = false) and
                                    (e.fk_research_group = :rgid or :rgid is null) and
                                    (e.experiment_id = :eid or :eid is null)
                              ORDER BY e.date_created DESC
                        ;";

                Db.CreateParameterFunc(cmd, "@rgid", researchGroupIdCreator, NpgsqlDbType.Integer);
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

            List<ExperimentExt> list = (from DataRow dr in dt.Rows select CreateExperimentObjectExt(dr)).ToList();

            return list;
        }

        /// <summary>
        /// Get all experiments filtered, paged
        /// </summary>
        /// <param name="researchGroupIdCreator"></param>
        /// <param name="operatorId"></param>
        /// <param name="projectId"></param>
        /// <param name="testGroupId"></param>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static List<ExperimentExt> GetExperimentsFilteredPaged(int researchGroupIdCreator, int? operatorId = null, int? projectId = null, int? testGroupId = null, string search = null, int? page = 1)
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
                    @"SELECT max(e.experiment_id) as experiment_id, e.fk_user, e.fk_research_group, e.experiment_description, e.date_created, e.is_complete, e.experiment_system_label, e.experiment_personal_label,
                        e.date_modified,
                        e.is_complete,
                        e.fk_template,
                        e.fk_edited_by,
                        e.fk_project,                       
                        u.username as operator_username,
                        p.project_name,
                        e.fk_sharing_type
                      FROM experiment e 
                        LEFT JOIN test_group_experiment tge on e.experiment_id = tge.fk_experiment
                        LEFT JOIN project p on e.fk_project = p.project_id
                        LEFT JOIN users u on u.user_id = e.fk_user
                        WHERE (tge.fk_test_group = :tgid or :tgid is null) AND
                        (e.is_complete = true) AND
                        (e.fk_user = :uid or :uid is null) AND
                        (e.fk_research_group = :rgid or :rgid is null) AND
                        ((lower(e.experiment_personal_label) LIKE lower('%'|| :search ||'%') or lower(e.experiment_system_label) LIKE lower('%'|| :search ||'%')) or :search is null)
                                            GROUP BY e.fk_user, e.fk_research_group, e.experiment_description, e.date_created, e.is_complete, e.experiment_system_label, e.experiment_personal_label,
                        e.date_modified,
                        e.is_complete,
                        e.fk_template,
                        e.fk_edited_by,
                        e.fk_project,
                        u.username,
                        p.project_name,
                        e.fk_sharing_type
                        ORDER BY max(experiment_id) DESC LIMIT 10 OFFSET :offset;";

                Db.CreateParameterFunc(cmd, "@rgid", researchGroupIdCreator, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@uid", operatorId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pid", projectId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@tgid", testGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
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

            List<ExperimentExt> list = (from DataRow dr in dt.Rows select CreateExperimentObjectExt(dr)).ToList();

            return list;
        }
        /// <summary>
        /// Get all experiments that are NOT added to the test group: testGroupId
        /// </summary>
        /// <param name="testGroupId"></param>
        /// <param name="projectId"></param>
        /// <param name="researchGroupIdCreator"></param>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static List<ExperimentExt> GetExperimentsOutsideTestGroupByProjectPaged(int testGroupId, int? projectId = null, int? researchGroupIdCreator = null, string search = null, int? page = 1)
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
                      FROM experiment e
                        LEFT JOIN test_group_experiment tge on e.experiment_id = tge.fk_experiment and (tge.fk_test_group = :tgid)
                        WHERE tge.fk_experiment is null AND
                        (e.is_complete = true) AND
                        (e.fk_project = :pid or :pid is null) AND
                        (e.fk_research_group = :rgid or :rgid is null) AND
                        ((lower(e.experiment_personal_label) LIKE lower('%'|| :search ||'%') or lower(e.experiment_system_label) LIKE lower('%'|| :search ||'%')) or :search is null)
                    
                        ORDER BY experiment_id DESC LIMIT 10 OFFSET :offset;
                        ";

                Db.CreateParameterFunc(cmd, "@tgid", testGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pid", projectId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupIdCreator, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
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

            List<ExperimentExt> list = (from DataRow dr in dt.Rows select CreateExperimentObjectExt(dr)).ToList();

            return list;
        }
        /// <summary>
        /// Get experiments by project Id
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="researchGroupIdCreator">Research group creator of experiment</param>
        /// <returns></returns>
        public static List<ExperimentExt> GetAllExperimentsByProject(int projectId, int? researchGroupIdCreator = null)
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
                    @"SELECT rg.research_group_name as research_group_name, *, u.username as operator_username
                              FROM experiment e
                              LEFT JOIN project p ON e.fk_project = p.project_id

                              LEFT JOIN research_group rg ON e.fk_research_group = rg.research_group_id
                              LEFT JOIN users u ON e.fk_user = u.user_id

                              WHERE (e.fk_project = :pid) AND
                                    (e.fk_research_group = :rgid or :rgid is null) AND
                                    (e.is_complete = true);";

                Db.CreateParameterFunc(cmd, "@pid", projectId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupIdCreator, NpgsqlDbType.Integer);

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

            List<ExperimentExt> list = (from DataRow dr in dt.Rows select CreateExperimentObjectExt(dr)).ToList();

            return list;
        }
        /// <summary>
        /// Get ALL experiments, regardless if complete or not
        /// </summary>
        /// <param name="experimentId"></param>
        /// <param name="projectId"></param>
        /// <param name="researchGroupIdCreator"></param>
        /// <returns></returns>
        public static List<ExperimentExt> GetAllExperimentsGeneralData(int? experimentId = null, int? projectId = null, int? researchGroupIdCreator = null)
        {
            //DOES NOT MATTER IF COMPLETE
            DataTable dt;
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                //(e.is_complete = true) and
                cmd.CommandText =
                    @"SELECT *, u.username as operator_username, pr.project_name as fk_project_name
                              FROM experiment e
                              LEFT JOIN project pr ON e.fk_project = pr.project_id
                              LEFT JOIN research_group rg ON e.fk_research_group = rg.research_group_id
                              LEFT JOIN users u ON e.fk_user = u.user_id
                              WHERE 
                                    (e.fk_research_group = :rgid or :rgid is null) and
                                    (e.fk_project = :pid or :pid is null) and
                                    (e.experiment_id = :eid or :eid is null);";

                Db.CreateParameterFunc(cmd, "@rgid", researchGroupIdCreator, NpgsqlDbType.Integer);
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

            List<ExperimentExt> list = (from DataRow dr in dt.Rows select CreateExperimentObjectExt(dr)).ToList();

            return list;
        }
        /// <summary>
        /// Get ALL completed experiments
        /// </summary>
        /// <param name="experimentId"></param>
        /// <param name="researchGroupIdCreator"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static List<ExperimentExt> GetAllCompleteExperimentsGeneralData(int? experimentId = null, int? researchGroupIdCreator = null, bool desc = true)
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
                    @"SELECT *, u.username as operator_username, edu.username as editing_operator_username
                              FROM experiment e
                              LEFT JOIN project pr ON e.fk_project = pr.project_id
                              LEFT JOIN research_group rg ON e.fk_research_group = rg.research_group_id
                              LEFT JOIN users u ON e.fk_user = u.user_id
                              LEFT JOIN users edu on e.fk_edited_by = edu.user_id
                              WHERE 
                                    (e.is_complete = true) and
                                    (e.fk_research_group = :rgid or :rgid is null) and
                                    (e.experiment_id = :eid or :eid is null)
                              ORDER BY e.date_created " + (desc ? @"DESC" : @"ASC") + @";"
                            ;

                Db.CreateParameterFunc(cmd, "@rgid", researchGroupIdCreator, NpgsqlDbType.Integer);
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

            List<ExperimentExt> list = (from DataRow dr in dt.Rows select CreateExperimentObjectExt(dr)).ToList();

            return list;
        }
        /// <summary>
        /// Get all completed experiments by research group creator
        /// </summary>
        /// <param name="researchGroupIdCreator"></param>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static List<ExperimentExt> GetCompleteExperimentsPaged(int researchGroupIdCreator, string search = null, int? page = 1)
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
                    @"SELECT *, u.username as operator_username
                      FROM experiment e
                      LEFT JOIN project p on e.fk_project = p.project_id
                      LEFT JOIN users u on u.user_id = e.fk_user
                      WHERE (e.is_complete = true) AND
                        (e.fk_research_group = :rgid) AND
                        ((lower(e.experiment_personal_label) LIKE lower('%'|| :search ||'%') or lower(e.experiment_system_label) LIKE lower('%'|| :search ||'%')) or :search is null)
                    
                        ORDER BY experiment_id DESC LIMIT 10 OFFSET :offset;
                        ";

                // Db.CreateParameterFunc(cmd, "@tgid", projectId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupIdCreator, NpgsqlDbType.Integer);
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

            List<ExperimentExt> list = (from DataRow dr in dt.Rows select CreateExperimentObjectExt(dr)).ToList();

            return list;
        }

        public static string GetAllFinishedBatteryComponentsListJsonForDropdown(int? researchGroupId = null, int? experimentId = null, int? batteryComponentTypeId = null)
        {
            //experimentId - to exclude from results
            //batteryComponentTypeId - wanted battery component type

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
                      FROM
                        (
                          SELECT DISTINCT bc.fk_experiment, bc.fk_battery_component_type, t.battery_component_type, COALESCE(e.experiment_personal_label, '') as experiment_personal_label, COALESCE(e.experiment_system_label, '') as experiment_system_label, e.date_created
                         FROM public.battery_component bc
                         LEFT JOIN experiment e on bc.fk_experiment = e.experiment_id
                         LEFT JOIN battery_component_type t on bc.fk_battery_component_type = t.battery_component_type_id
                         WHERE bc.fk_experiment != :eid and
                               bc.fk_commercial_type is null and
                              (bc.fk_battery_component_type = :bctid or :bctid is null) and
                               e.is_complete = true and
                               e.fk_research_group = :rgid
                      UNION
                        SELECT DISTINCT ep.fk_experiment, ep.fk_battery_component_type, t.battery_component_type, COALESCE(e.experiment_personal_label, '') as experiment_personal_label, COALESCE(e.experiment_system_label, '') as experiment_system_label, e.date_created
                        FROM public.experiment_process ep
                          LEFT JOIN experiment e on ep.fk_experiment = e.experiment_id
                          LEFT JOIN battery_component_type t on ep.fk_battery_component_type = t.battery_component_type_id
                        WHERE ep.fk_experiment != :eid and
                             (ep.fk_battery_component_type = :bctid or :bctid is null) and
                              e.is_complete = true and
                              e.fk_research_group = :rgid

                            ORDER BY fk_experiment DESC, fk_battery_component_type ASC
                        )
                        as t;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bctid", batteryComponentTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);


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


        #region Test results/measurements and plotting

        /// <summary>
        /// Get experiments for showing test results charts
        /// </summary>
        /// <param name="currentRGId">The current RG id of the user</param>
        /// <param name="researchGroupIdCreator">The RG id of the creator of the experiment</param>
        /// <param name="userId">user Id of the user that created the experiment</param>
        /// <param name="projectId">the project id where the experiment belongs</param>
        /// <param name="testGroupId">the test group id where the experiment belongs</param>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static List<ExperimentExt> GetExperimentsForCharts(int currentRGId, int? researchGroupIdCreator = null, int? userId = null, int? projectId = null, int? testGroupId = null, int? testTypeId = null, string search = null, int? page = 1)
        {
            //ONLY THOSE THAT HAVE RESULTS DOC UPLOADED

            DataTable dt;

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"SELECT max(e.experiment_id) as experiment_id, e.fk_user, e.fk_research_group, e.experiment_description, e.date_created, e.is_complete, e.experiment_system_label, e.experiment_personal_label,
                                            e.date_modified,
                                            e.is_complete,
                                            e.fk_template,
                                            e.fk_project,
                                            fk_sharing_type,
                                            e.fk_edited_by
                    FROM
	                    (
	                    SELECT e2.*
	                    FROM experiment e2
	                    LEFT JOIN project_research_group prg ON e2.fk_project = prg.fk_project
	                    WHERE 
	                    (e2.is_complete = true)
	                    AND	(e2.has_test_results_doc = true)
	                    AND (prg.fk_research_group = :rgidcurrent)
	                    ) e
	
                    LEFT JOIN test_group_experiment tge on e.experiment_id = tge.fk_experiment
                    LEFT JOIN test t on e.experiment_id = t.fk_experiment

                    WHERE
                    (e.fk_project = :pid or :pid is null) AND
                    (e.fk_research_group = :rgidCreator or :rgidCreator is null) AND
                    (e.fk_user = :uidCreator or :uidCreator is null) AND
                    (tge.fk_test_group = :tgid or :tgid is null) AND		
                    (t.fk_test_type = :ttype or :ttype is null) AND
                    ((lower(e.experiment_personal_label) LIKE lower('%'|| :search ||'%') or lower(e.experiment_system_label) LIKE lower('%'|| :search ||'%')) or :search is null)

                    GROUP BY e.fk_user, e.fk_research_group, e.experiment_description, e.date_created, e.is_complete, e.experiment_system_label, e.experiment_personal_label,
                                            e.date_modified,
                                            e.is_complete,
                                            e.fk_template,
                                            e.fk_project,
                                            fk_sharing_type,
                                            e.fk_edited_by

                    ORDER BY max(experiment_id) DESC LIMIT 10 OFFSET :offset;";

                Db.CreateParameterFunc(cmd, "@rgidcurrent", currentRGId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pid", projectId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgidCreator", researchGroupIdCreator, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@uidCreator", userId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@tgid", testGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@ttype", testTypeId, NpgsqlDbType.Integer);

                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
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

            List<ExperimentExt> list = (from DataRow dr in dt.Rows select CreateExperimentObjectExt(dr)).ToList();

            return list;
        }
        /// <summary>
        /// Get experiments by id list, that only researchGroupId is meant to see
        /// </summary>
        /// <param name="researchGroupId"></param>
        /// <param name="experimentIdArray"></param>
        /// <returns></returns>
        public static List<ExperimentExt> GetExperimentsByIdForCharts(int researchGroupId, int[] experimentIdArray = null)
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
                    @"SELECT *, u.username as operator_username
                              FROM experiment e
                              LEFT JOIN project pr ON e.fk_project = pr.project_id
                              LEFT JOIN project_research_group prg ON pr.project_id = prg.fk_project

                              LEFT JOIN research_group rg ON e.fk_research_group = rg.research_group_id
                              LEFT JOIN users u ON e.fk_user = u.user_id
                              WHERE (e.is_complete = true) and
                                    (prg.fk_research_group = :rgid) and
                                    (e.experiment_id = ANY(:eidList) or :eidList is null)
                              ORDER BY e.date_created DESC
                        ;";

                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@eidList", experimentIdArray, NpgsqlDbType.Array | NpgsqlDbType.Integer);

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

            List<ExperimentExt> list = (from DataRow dr in dt.Rows select CreateExperimentObjectExt(dr)).ToList();

            return list;
        }

        #endregion

        //to be removed
        public static int OpenNewBlankExperiment(Experiment experiment)
        {
            int returnedExperimentId = 0;
            try
            {
                var guid = Guid.NewGuid().ToString();
                //return 0;
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"INSERT INTO public.experiment (experiment_system_label, fk_research_group, fk_user, is_complete, date_created)
                    VALUES (:slabel, :rgid, :uid, :iscomplete, now()::timestamp)
                    RETURNING experiment_id
                    ;";

                Db.CreateParameterFunc(cmd, "@slabel", guid, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@plabel", experiment.experimentPersonalLabel, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@desc", experiment.experimentDescription, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@rgid", experiment.fkResearchGroup, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@uid", experiment.fkUser, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@iscomplete", experiment.isComplete, NpgsqlDbType.Boolean);
                //Db.CreateParameterFunc(cmd, "@datem", experiment.dateModified, NpgsqlDbType.Date);

                //returnedExperimentId = Db.ExecuteNonQuery(cmd);
                returnedExperimentId = int.Parse(Db.ExecuteScalar(cmd));
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting experiment general info", ex);
            }

            return returnedExperimentId;
        }
        /// <summary>
        /// Insert new experiment - only general information
        /// </summary>
        /// <param name="experiment"></param>
        /// <returns></returns>
        public static int AddExperimentGeneralInfo(Experiment experiment, string acronym, int lastExperimentNumber)
        {
            int returnedExperimentId = 0;
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }

            NpgsqlTransaction t = cmd.Connection.BeginTransaction();

            try
            {
                string systemLabel = acronym + "_" + lastExperimentNumber;
                //var guid = Guid.NewGuid().ToString();
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"INSERT INTO public.experiment (experiment_personal_label, experiment_description, fk_research_group, fk_project, fk_user, is_complete, date_created, fk_template, experiment_system_label)
                    VALUES (:plabel, :desc, :rgid, :pid, :uid, :iscomplete, now()::timestamp, :tid, :slabel)
                    RETURNING experiment_id
                    ;";

                Db.CreateParameterFunc(cmd, "@plabel", experiment.experimentPersonalLabel, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@desc", experiment.experimentDescription, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@rgid", experiment.fkResearchGroup, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pid", experiment.fkProject, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@uid", experiment.fkUser, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@iscomplete", experiment.isComplete, NpgsqlDbType.Boolean);
                Db.CreateParameterFunc(cmd, "@tid", experiment.fkTemplate, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@slabel", systemLabel, NpgsqlDbType.Text);

                returnedExperimentId = int.Parse(Db.ExecuteScalar(cmd, false));
                if (returnedExperimentId <= 0)
                {
                    t.Rollback();
                    throw new Exception("Error inserting new experiment");
                }
                //string systemLabel = returnedExperimentId + "_ES";
                //string systemLabel = returnedExperimentId + "_" + acronym;
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.research_group
                        SET last_experiment_number=:len
                        WHERE research_group_id=:rgid;";

                Db.CreateParameterFunc(cmd, "@len", lastExperimentNumber + 1, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", experiment.fkResearchGroup, NpgsqlDbType.Integer);
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
            return returnedExperimentId;
        }
        /// <summary>
        /// Update experiment general data
        /// </summary>
        /// <param name="experimentId"></param>
        /// <param name="req"></param>
        /// <param name="researchGroupId"></param>
        /// <returns></returns>
        public static int UpdateExperimentGeneralData(int experimentId, AddExperimentRequest req, int researchGroupId)
        {
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandType = CommandType.Text;

            int res = 0;

            ExperimentExt experiment = req.experimentInfo;

            try
            {
                //CHECK EXPERIMENT REQUEST
                //CheckExperimentGeneralInfoForErrors(req, researchGroupId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.experiment
                        SET experiment_personal_label=:plabel, experiment_description=:desc, date_modified=now()::timestamp
                        WHERE experiment_id=:eid;";

                Db.CreateParameterFunc(cmd, "@plabel", experiment.experimentPersonalLabel, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@desc", experiment.experimentDescription, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating experiment info", ex);
            }

            return 0;
        }
        /// <summary>
        /// Update experiment general info and set complete flag to true
        /// </summary>
        /// <param name="experimentId"></param>
        /// <param name="req"></param>
        /// <param name="researchGroupId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int UpdateExperimentGeneralDataAndSetComplete(int experimentId, AddExperimentRequest req, int researchGroupId, int userId)
        {
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandType = CommandType.Text;

            int res = 0;

            ExperimentExt experiment = req.experimentInfo;

            try
            {
                //NOTHING TO CHECK FOR NOW
                //CHECK EXPERIMENT REQUEST
                //CheckExperimentGeneralInfoForErrors(req, researchGroupId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.experiment
                        SET experiment_personal_label=:plabel, experiment_description=:desc, is_complete=true, date_modified=now()::timestamp, fk_edited_by=:uid
                        WHERE experiment_id=:eid;";

                Db.CreateParameterFunc(cmd, "@plabel", experiment.experimentPersonalLabel, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@desc", experiment.experimentDescription, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@uid", userId, NpgsqlDbType.Integer);

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating experiment info", ex);
            }

            return 0;
        }

        /// <summary>
        /// Validate the stock quantity and insert new stock transactions
        /// </summary>
        /// <param name="reqList"></param>
        /// <param name="researchGroupId"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static int AddAllBatteryComponentsStockData(List<AddBatteryComponentRequest> reqList, int researchGroupId, NpgsqlCommand cmd)
        {
            //RETURNS FAULTY COMPONENT ID

            if (cmd != null)
            {
                cmd.Parameters.Clear();
            }
            else
            {
                cmd = Db.CreateCommand();

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
            }
            cmd.CommandType = CommandType.Text;

            int res = 0;
            int experimentId = 0;
            int operatorId = 0;

            int faultyComponentId = 0;

            try
            {
                foreach (AddBatteryComponentRequest req in reqList)
                {
                    bool materialQuantityOk = true;
                    bool batchQuantityOk = true;

                    int componentTypeId = 0;
                    switch (req.componentType)
                    {
                        case "Anode":
                            componentTypeId = 1;
                            break;
                        case "Cathode":
                            componentTypeId = 2;
                            break;
                        case "Separator":
                            componentTypeId = 3;
                            break;
                        case "Electrolyte":
                            componentTypeId = 4;
                            break;
                        case "ReferenceElectrode":
                            componentTypeId = 5;
                            break;
                        case "Casing":
                            componentTypeId = 6;
                            break;
                    }
                    List<AddBatteryComponentStepRequest> componentStepsContentList;

                    string notEnoughMessage = "" + req.componentType + ": Not enough ";
                    string invalidWeightMessage = "" + req.componentType + ": Invalid quantity value for ";
                    string invalidPercentageMessage = "" + req.componentType + ": Invalid percentage of active value for ";

                    experimentId = (int)req.experimentId;
                    operatorId = (int)req.userId;
                    componentStepsContentList = req.componentStepsContentList;

                    //STOCK VALIDATION
                    foreach (AddBatteryComponentStepRequest step in componentStepsContentList)
                    {
                        List<string> materials = new List<string>();
                        List<string> batches = new List<string>();

                        if (step.isSavedAsBatch == false)
                        {
                            foreach (BatteryComponentExt stepContent in step.stepContent)
                            {
                                if (stepContent.weight == null)
                                {
                                    if (stepContent.fkStepMaterial != null)
                                    {
                                        invalidWeightMessage += stepContent.materialName;
                                        //throw new Exception(stepContent.materialName + ": Invalid quantity value");
                                    }

                                    else
                                    {
                                        invalidWeightMessage += stepContent.batchSystemLabel;
                                        //throw new Exception(stepContent.batchSystemLabel + ": Invalid quantity value");
                                    }
                                    faultyComponentId = componentTypeId;
                                    Exception ex = new Exception(invalidWeightMessage);
                                    ex.Data.Add("faultyComponentId", componentTypeId);
                                    //throw new Exception(stepContent.materialName + ": Invalid quantity value");
                                    throw ex;
                                }
                                //if (stepContent.percentageOfActive != null)
                                //{
                                //    if (stepContent.percentageOfActive < 0 || stepContent.percentageOfActive > 100)
                                //    {
                                //        if (stepContent.fkStepMaterial != null)
                                //        {
                                //            invalidPercentageMessage += stepContent.materialName;
                                //        }
                                //        else
                                //        {
                                //            invalidPercentageMessage += stepContent.batchSystemLabel;
                                //        }
                                //        faultyComponentId = componentTypeId;
                                //        Exception ex = new Exception(invalidPercentageMessage);
                                //        ex.Data.Add("faultyComponentId", componentTypeId);
                                //        throw ex;
                                //    }
                                //}
                                double wantedQuantity = (double)stepContent.weight;
                                if (stepContent.fkStepMaterial != null)
                                {
                                    int materialId = (int)stepContent.fkStepMaterial;
                                    bool result = StockTransactionDa.CheckMaterialStockQuantityEnough(materialId, wantedQuantity, researchGroupId, cmd);

                                    if (result == false)
                                    {
                                        materialQuantityOk = false;
                                        materials.Add(stepContent.materialName);
                                    }
                                    else
                                    {
                                        //INSERT IN STOCK - MATERIAL
                                        cmd.Parameters.Clear();
                                        cmd.CommandText =
                                    @"INSERT INTO public.stock_transaction (
                                        fk_material,
                                        stock_transaction_element_type,
                                        amount,
                                        fk_operator,
                                        fk_research_group,
                                        fk_experiment_coming,
                                        transaction_direction,
                                        fk_battery_component_type,
                                        date_created

                                        )
                                                            VALUES (
                                        :mid,
                                        1,
                                        :a,
                                        :oid,
                                        :rgid,
                                        :ecomid,
                                        -1,
                                        :bctid,
                                        now()::timestamp);";

                                        Db.CreateParameterFunc(cmd, "@mid", stepContent.fkStepMaterial, NpgsqlDbType.Integer);
                                        Db.CreateParameterFunc(cmd, "@a", stepContent.weight, NpgsqlDbType.Double);
                                        Db.CreateParameterFunc(cmd, "@oid", operatorId, NpgsqlDbType.Integer);
                                        Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                                        Db.CreateParameterFunc(cmd, "@ecomid", experimentId, NpgsqlDbType.Integer);
                                        Db.CreateParameterFunc(cmd, "@bctid", componentTypeId, NpgsqlDbType.Integer);

                                        res = Db.ExecuteNonQuery(cmd, false);
                                        if (res <= 0)
                                        {
                                            //t.Rollback();
                                            throw new Exception("Error inserting material stock info");
                                        }
                                    }
                                }
                                else
                                {
                                    //IT's A BATCH
                                    int batchId = (int)stepContent.fkStepBatch;
                                    bool result = StockTransactionDa.CheckBatchStockQuantityEnough(batchId, wantedQuantity, researchGroupId, cmd);
                                    if (result == false)
                                    {
                                        batchQuantityOk = false;
                                        batches.Add(stepContent.batchSystemLabel);
                                    }
                                    else
                                    {
                                        //INSERT IN STOCK - BATCH
                                        cmd.Parameters.Clear();
                                        cmd.CommandText =
                                        @"INSERT INTO public.stock_transaction (
                                            fk_batch,
                                            stock_transaction_element_type,
                                            amount,
                                            fk_operator,
                                            fk_research_group,
                                            fk_experiment_coming,
                                            transaction_direction,
                                            fk_battery_component_type,
                                            date_created

                                            )
                                                                VALUES (
                                            :bid,
                                            2,
                                            :a,
                                            :oid,
                                            :rgid,
                                            :ecomid,
                                            -1,
                                            :bctid,
                                            now()::timestamp);";

                                        Db.CreateParameterFunc(cmd, "@bid", stepContent.fkStepBatch, NpgsqlDbType.Integer);
                                        Db.CreateParameterFunc(cmd, "@a", stepContent.weight, NpgsqlDbType.Double);
                                        Db.CreateParameterFunc(cmd, "@oid", operatorId, NpgsqlDbType.Integer);
                                        Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                                        Db.CreateParameterFunc(cmd, "@ecomid", experimentId, NpgsqlDbType.Integer);
                                        Db.CreateParameterFunc(cmd, "@bctid", componentTypeId, NpgsqlDbType.Integer);

                                        res = Db.ExecuteNonQuery(cmd, false);
                                        if (res <= 0)
                                        {
                                            //t.Rollback();
                                            throw new Exception("Error inserting batch stock info");
                                        }
                                    }
                                }
                            }
                            if (materialQuantityOk == false)
                            {
                                string materialsString = "";
                                foreach (string m in materials)
                                {
                                    if (materials.IndexOf(m) == materials.Count - 1)
                                    {
                                        materialsString += m;
                                    }
                                    else
                                    {
                                        materialsString += m + ", ";
                                    }
                                }
                                notEnoughMessage += "Material: " + materialsString;
                            }
                            if (batchQuantityOk == false)
                            {
                                string batchesString = "";
                                foreach (string b in batches)
                                {
                                    if (batches.IndexOf(b) == batches.Count - 1)
                                    {
                                        batchesString += b;
                                    }
                                    else
                                    {
                                        batchesString += b + ", ";
                                    }
                                }
                                notEnoughMessage += " Batch: " + batchesString;
                            }
                            if (!materialQuantityOk || !batchQuantityOk)
                            {
                                notEnoughMessage += " in stock!";
                                faultyComponentId = componentTypeId;
                                Exception ex = new Exception(notEnoughMessage);
                                ex.Data.Add("faultyComponentId", componentTypeId);
                                throw ex;
                                //throw new Exception(notEnoughMessage);
                            }
                        }

                    }
                }
            }
            catch (ValidationException ve)
            {
                //do whatever
                throw new Exception(ve.Message);
            }
            catch (Exception ex)
            {
                //t.Rollback();

                //if (ex.Data.Count > 0)
                //{
                //    throw ex;
                //}
                //ex.Data.Add("faultyComponentId", componentTypeId);
                //throw new Exception(ex.Message);
                throw ex;
            }

            return faultyComponentId;
        }
        /// <summary>
        /// Finish experiment
        /// </summary>
        /// <param name="req"></param>
        /// <param name="experimentId"></param>
        /// <param name="researchGroupId"></param>
        /// <param name="userId"></param>
        /// <param name="editing"></param>
        /// <returns></returns>
        public static int FinishExperimentCreation(AddExperimentRequest req, int experimentId, int researchGroupId, int userId, bool editing)
        {
            //ADD STOCK AND FINISH EXPERIMENT CREATION
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandType = CommandType.Text;

            NpgsqlTransaction t = cmd.Connection.BeginTransaction();

            int res = 0;

            List<AddBatteryComponentRequest> batteryComponentsForStock = req.batteryComponents;

            try
            {
                //REMOVE ALL STOCK
                res = RemoveExperimentStock(experimentId, researchGroupId, cmd);

                //if (res <= 0)
                //{
                //    t.Rollback();
                //    throw new Exception("Error reverting stock transactions");
                //}

                //CHECK REQUEST AND INSERT STOCK
                if (batteryComponentsForStock != null)
                {
                    AddAllBatteryComponentsStockData(batteryComponentsForStock, researchGroupId, cmd); //MIGHT RETURN FAULTY COMPONENT ID IN EXCEPTION DATA
                }

                //UPDATE EXPERIMENT GENERAL INFO
                if (editing == true)
                {
                    //SET UPDATED BY AND DATETIME WHEN UPDATED
                    cmd.Parameters.Clear();
                    cmd.CommandText =
                        @"UPDATE public.experiment
                        SET experiment_personal_label=:plabel, experiment_description=:desc, fk_edited_by=:uid, date_modified=now()::timestamp
                        WHERE experiment_id=:eid;";

                    Db.CreateParameterFunc(cmd, "@plabel", req.experimentInfo.experimentPersonalLabel, NpgsqlDbType.Text);
                    Db.CreateParameterFunc(cmd, "@desc", req.experimentInfo.experimentDescription, NpgsqlDbType.Text);
                    Db.CreateParameterFunc(cmd, "@uid", userId, NpgsqlDbType.Integer);

                    Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                }
                else
                {
                    //SET CREATED BY AND DATETIME WHEN CREATED
                    cmd.Parameters.Clear();
                    cmd.CommandText =
                        @"UPDATE public.experiment 
                        SET experiment_personal_label=:plabel, experiment_description=:desc, fk_user=:uid, date_created=now()::timestamp
                        WHERE experiment_id=:eid;";

                    Db.CreateParameterFunc(cmd, "@plabel", req.experimentInfo.experimentPersonalLabel, NpgsqlDbType.Text);
                    Db.CreateParameterFunc(cmd, "@desc", req.experimentInfo.experimentDescription, NpgsqlDbType.Text);
                    Db.CreateParameterFunc(cmd, "@uid", userId, NpgsqlDbType.Integer);

                    Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                }

                res = Db.ExecuteNonQuery(cmd, false);
                if (res <= 0)
                {
                    throw new Exception("Error updating experiment general info");
                }

                //SET EXPERIMENT COMPLETE TRUE
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.experiment
                        SET is_complete=true
                        WHERE experiment_id=:eid;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd, false);
                if (res <= 0)
                {
                    throw new Exception("Error updating experiment general info");
                }



                ////SET EXPERIMENT STATUS PRIVATE
                //cmd.Parameters.Clear();
                //cmd.CommandText =
                //    @"UPDATE public.experiment
                //        SET fk_sharing_type=1
                //        WHERE experiment_id=:eid;";

                //Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);

                //res = Db.ExecuteNonQuery(cmd, false);
                //if (res <= 0)
                //{
                //    throw new Exception("Error updating experiment general info");
                //}

                t.Commit();
                cmd.Connection.Close();

            }
            catch (Exception ex)
            {
                t.Rollback();
                cmd.Connection.Close();
                //throw new Exception(ex.Message);
                throw ex;
            }
            return 0;
        }
        /// <summary>
        /// Insert experiment summary with calculations
        /// </summary>
        /// <param name="experimentId"></param>
        /// <param name="researchGroupId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int InsertExperimentSummary(int experimentId, int researchGroupId, int userId)
        {
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandType = CommandType.Text;

            dynamic experimentSummaryData;
            try
            {
                experimentSummaryData = Helpers.GeneralHelper.GetExperimentSummary(experimentId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            NpgsqlTransaction t = cmd.Connection.BeginTransaction();

            int res = 0;

            try
            {

                //REMOVE EXPERIMENT SUMMARY
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"DELETE FROM public.experiment_summary
                                WHERE fk_experiment=:eid;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd, false);

                //INSERT NEW EXPERIMENT SUMMARY DATA
                dynamic calculations = experimentSummaryData.calculations;
                dynamic anode = calculations.anode;
                dynamic cathode = calculations.cathode;
                dynamic separator = calculations.separator;
                dynamic electrolyte = calculations.electrolyte;
                dynamic referenceElectrode = calculations.referenceElectrode;
                dynamic casing = calculations.casing;

                List<dynamic> allComponentsList = new List<dynamic>();
                allComponentsList.Add(anode);
                allComponentsList.Add(cathode);
                allComponentsList.Add(separator);
                allComponentsList.Add(electrolyte);
                allComponentsList.Add(referenceElectrode);
                allComponentsList.Add(casing);



                double mass1 = calculations.mass1;
                double mass2 = calculations.mass2;
                double mass3 = calculations.mass3;
                double mass4 = calculations.mass4;
                double mass5 = calculations.mass5;
                double mass6 = calculations.mass6;
                //fk_experiment integer
                //    constraint experiment_summary_experiment_experiment_id_fk
                //        references experiment,
                //component_empty boolean,
                //total_weight numeric,
                //total_labeled_materials numeric,
                //labeled_materials text,
                //labeled_percentages text,
                //total_active_materials numeric,
                //total_active_materials_percentage numeric,
                //active_materials text,
                //active_percentages text,
                //fk_battery_component_type integer
                //    constraint experiment_summary_battery_component_type_battery_component_typ
                //        references battery_component_type,
                //fk_commercial_type integer
                //    constraint experiment_summary_battery_component_commercial_type_battery_co
                //        references battery_component_commercial_type,
                //mass1 numeric,
                //mass2 numeric,
                //mass3 numeric,
                //mass4 numeric,
                //mass5 numeric,
                //mass6 numeric
                foreach (dynamic component in allComponentsList)
                {
                    IDictionary<string, object> componentAttributes = (IDictionary<string, object>)component;


                    //int fkExperiment = experimentId;
                    Boolean componentEmpty = component.componentEmpty;
                    double? totalWeight = componentAttributes.ContainsKey("totalWeight") ? component.totalWeight : (double?)null;
                    double? totalLabeledMaterials = componentAttributes.ContainsKey("totalLabeledMaterials") ? double.Parse(component.totalLabeledMaterials.ToString()) : (double?)null;
                    string labeledMaterials = componentAttributes.ContainsKey("labeledMaterials") ? component.labeledMaterials : null;
                    string labeledPercentages = componentAttributes.ContainsKey("labeledPercentages") ? component.labeledPercentages : null;
                    double? totalActiveMaterials = componentAttributes.ContainsKey("totalActiveMaterials") ? double.Parse(component.totalActiveMaterials.ToString()) : (double?)null;
                    double? totalActiveMaterialsPercentage = componentAttributes.ContainsKey("totalActiveMaterialsPercentage") ? double.Parse(component.totalActiveMaterialsPercentage.ToString()) : (double?)null;
                    string activeMaterials = componentAttributes.ContainsKey("activeMaterials") ? component.activeMaterials : null;
                    string activePercentages = componentAttributes.ContainsKey("activePercentages") ? component.activePercentages : null;
                    int? fkBatteryComponentType = componentAttributes.ContainsKey("componentTypeId") ? int.Parse(component.componentTypeId.ToString()) : (int?)null;
                    long? fkCommercialType = componentAttributes.ContainsKey("commercialType") ? long.Parse(component.commercialType.fkCommercialType.ToString()) : (long?)null;

                    if (fkCommercialType != null || componentEmpty == true)
                    {
                        totalWeight = (double?)null;
                        totalActiveMaterials = (double?)null;
                    }
                    //var a = JsonConvert.SerializeObject(component);

                    cmd.Parameters.Clear();
                    cmd.CommandText =
                @"INSERT INTO public.experiment_summary (
                        fk_experiment,
                        component_empty,
                        total_weight,
                        total_labeled_materials,
                        labeled_materials,
                        labeled_percentages,
                        total_active_materials,
                        total_active_materials_percentage,
                        active_materials,
                        active_percentages,
                        fk_battery_component_type,
                        fk_commercial_type
                        )
                    VALUES (:eid, :cempty, :tw, :tlab, :lab, :labp, :tact, :tactmp, :actm, :actp, :bctid, :comtypeid);";

                    Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@cempty", componentEmpty, NpgsqlDbType.Boolean);
                    Db.CreateParameterFunc(cmd, "@tw", totalWeight, NpgsqlDbType.Double);
                    Db.CreateParameterFunc(cmd, "@tlab", totalLabeledMaterials, NpgsqlDbType.Double);
                    Db.CreateParameterFunc(cmd, "@lab", labeledMaterials, NpgsqlDbType.Text);
                    Db.CreateParameterFunc(cmd, "@labp", labeledPercentages, NpgsqlDbType.Text);
                    Db.CreateParameterFunc(cmd, "@tact", totalActiveMaterials, NpgsqlDbType.Double);
                    Db.CreateParameterFunc(cmd, "@tactmp", totalActiveMaterialsPercentage, NpgsqlDbType.Double);
                    Db.CreateParameterFunc(cmd, "@actm", activeMaterials, NpgsqlDbType.Text);
                    Db.CreateParameterFunc(cmd, "@actp", activePercentages, NpgsqlDbType.Text);
                    Db.CreateParameterFunc(cmd, "@bctid", fkBatteryComponentType, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@comtypeid", fkCommercialType, NpgsqlDbType.Bigint);

                    res = Db.ExecuteNonQuery(cmd, false);
                }

                t.Commit();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                t.Rollback();
                cmd.Connection.Close();
                throw new Exception(ex.Message);
            }
            return 0;
        }
        /// <summary>
        /// Get all batches in experiment content
        /// </summary>
        /// <param name="experimentId"></param>
        /// <returns></returns>
        public static List<int> GetBatchesInExperiment(int experimentId)
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
                       @"SELECT DISTINCT bc.fk_step_batch
                         FROM public.battery_component bc
                         WHERE fk_step_batch IS NOT NULL AND
                               bc.fk_experiment = :eid
                        ;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            List<int> batchIds = new List<int>();

            if (dt == null || dt.Rows.Count == 0)
            {
                return batchIds;
            }

            foreach (DataRow dr in dt.Rows)
            {
                int id = int.Parse(dr["fk_step_batch"].ToString());
                batchIds.Add(id);
            }

            return batchIds;
        }
        /// <summary>
        /// Get all completed component types for experiment id
        /// </summary>
        /// <param name="experimentId"></param>
        /// <returns></returns>
        public static List<int> GetAllExperimentComponentsCompletedIds(int experimentId)
        {
            DataTable dt;
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                //staro pred da moze step bez material ili proces

                //                cmd.CommandText =
                //                       @"SELECT DISTINCT fk_battery_component_type
                //                            FROM public.battery_component bc
                //                            WHERE bc.fk_experiment = :eid
                //                            ORDER BY fk_battery_component_type ASC
                //                            ;";

                cmd.CommandText =
                       @"SELECT DISTINCT bc.fk_battery_component_type
                         FROM public.battery_component bc
                         WHERE bc.fk_experiment = :eid
                      UNION
                        SELECT DISTINCT ep.fk_battery_component_type
                        FROM public.experiment_process ep
                        WHERE ep.fk_experiment = :eid

                            ORDER BY fk_battery_component_type ASC
                            ;";


                //AND                                   bc.is_complete = true

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            List<int> componentTypeIdsList = new List<int>();

            if (dt == null || dt.Rows.Count == 0)
            {
                return componentTypeIdsList;
            }

            //List<ExperimentExt> list = (from DataRow dr in dt.Rows select CreateExperimentObjectExt(dr)).ToList();
            foreach (DataRow dr in dt.Rows)
            {
                //int id = dr["fk_battery_component_type"] != DBNull.Value ? int.Parse(dr["fk_battery_component_type"].ToString()) : (int?)null;
                int id = int.Parse(dr["fk_battery_component_type"].ToString());
                componentTypeIdsList.Add(id);
            }
            return componentTypeIdsList;
        }
        public static int MarkExperimentComplete(int experimentId)
        {
            //Mark experiment as complete
            int res = 0;
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.experiment
                        SET is_complete=true --,date_modified=now()::timestamp
                        WHERE experiment_id=:eid;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd);
                if (res <= 0)
                {
                    throw new Exception("Error updating experiment info");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        /// <summary>
        /// Set complete status flag to false for experiment
        /// </summary>
        /// <param name="experimentId"></param>
        /// <returns></returns>
        public static int UnMarkExperimentComplete(int experimentId)
        {
            int res = 0;
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.experiment
                        SET is_complete = false --,date_modified=now()::timestamp
                        WHERE experiment_id=:eid;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd);
                if (res <= 0)
                {
                    throw new Exception("Error updating experiment info");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        public static int MarkExperimentAndComponentsComplete(int experimentId)
        {
            //Mark experiment and it's content as complete
            int res = 0;
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            NpgsqlTransaction t = cmd.Connection.BeginTransaction();

            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.battery_component
                        SET is_complete=true
                        WHERE fk_experiment=:eid;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd, false);
                if (res <= 0)
                {
                    t.Rollback();
                    throw new Exception("Error updating battery component info");
                }

                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.experiment_process
                        SET is_complete=true
                        WHERE fk_experiment=:eid;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd, false);
                if (res <= 0)
                {
                    t.Rollback();
                    throw new Exception("Error updating experiment process info");
                }

                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.experiment
                        SET is_complete=true
                        WHERE experiment_id=:eid;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd, false);
                if (res <= 0)
                {
                    t.Rollback();
                    throw new Exception("Error updating experiment info");
                }

                t.Commit();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                t.Rollback();
                throw new Exception(ex.Message);
                //throw new Exception("Error saving experiment", ex);
            }
            return 0;
        }


        #region Experiment sharing status
        public static int SetStatusOfExperimentPrivate(int experimentId)
        {
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandType = CommandType.Text;

            NpgsqlTransaction t = cmd.Connection.BeginTransaction();

            int res = 0;
            try
            {
                //SET EXPERIMENT STATUS PRIVATE
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.experiment
                        SET fk_sharing_type=1
                        WHERE experiment_id=:eid;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd, false);
                if (res <= 0)
                {
                    throw new Exception("Error updating experiment general info");
                }

                t.Commit();
                cmd.Connection.Close();

            }
            catch (Exception ex)
            {
                t.Rollback();
                cmd.Connection.Close();
                //throw new Exception(ex.Message);
                throw ex;
            }
            return 0;
        }
        public static int SetStatusOfExperimentShared(int experimentId)
        {
            //ADD STOCK AND FINISH EXPERIMENT CREATION
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandType = CommandType.Text;

            NpgsqlTransaction t = cmd.Connection.BeginTransaction();

            int res = 0;
            try
            {
                //SET EXPERIMENT STATUS PRIVATE
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.experiment
                        SET fk_sharing_type=2
                        WHERE experiment_id=:eid;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd, false);
                if (res <= 0)
                {
                    throw new Exception("Error updating experiment general info");
                }

                t.Commit();
                cmd.Connection.Close();

            }
            catch (Exception ex)
            {
                t.Rollback();
                cmd.Connection.Close();
                //throw new Exception(ex.Message);
                throw ex;
            }
            return 0;
        }
        public static int UpdateExperimentStatus(int projectId)
        {
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandType = CommandType.Text;

            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.experiment
                        SET fk_sharing_type = 2
                        WHERE fk_project =:pid;";

                Db.CreateParameterFunc(cmd, "@pid", projectId, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating experiment info", ex);
            }

            return 0;
        }
        public static int UpdateStatusOfExperimentAsPublic(Experiment experiment)
        {
            int res = 0;
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            NpgsqlTransaction t = cmd.Connection.BeginTransaction();

            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.experiment
                       SET fk_sharing_type = 3
                        WHERE experiment_id=:eid;";

                Db.CreateParameterFunc(cmd, "@eid", experiment.experimentId, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd, false);
                t.Commit();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                t.Rollback();
                throw new Exception(ex.Message);
                //throw new Exception("Error saving experiment", ex);
            }
            return 0;
        }

        #endregion

        #region Permissions
        /// <summary>
        /// Check if research group id can view experiment id
        /// </summary>
        /// <param name="experimentId"></param>
        /// <param name="researchGroupId"></param>
        /// <returns></returns>
        public static bool HasViewPermission(int experimentId, int researchGroupId)
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
						 	   WHERE fk_project = (							
							   SELECT fk_project FROM experiment
							   WHERE experiment_id = :eid ) 
                               AND fk_research_group = :rgid;";

                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);

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
        #endregion

        public static int EditExperiment(int experimentId, int userId)
        {
            int res = 0;
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            NpgsqlTransaction t = cmd.Connection.BeginTransaction();

            try
            {

                //Mark experiment and it's content as not complete

                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.battery_component
                        SET is_complete=false
                        WHERE fk_experiment=:eid;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd, false);
                //if (res <= 0)
                //{
                //    t.Rollback();
                //    throw new Exception("Error updating battery component info");
                //}

                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.experiment_process
                        SET is_complete=false
                        WHERE fk_experiment=:eid;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd, false);
                //if (res <= 0)
                //{
                //    t.Rollback();
                //    throw new Exception("Error updating experiment process info");
                //}

                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.experiment
                        SET is_complete=false, fk_edited_by=:euid, date_modified=now()::timestamp
                        WHERE experiment_id=:eid;";

                Db.CreateParameterFunc(cmd, "@euid", userId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd, false);
                if (res <= 0)
                {
                    t.Rollback();
                    throw new Exception("Error setting experiment to edit mode");
                }

                //remove all batches from the same project
                cmd.Parameters.Clear();
                cmd.CommandText =
                   @"DELETE FROM public.project_batch
                                WHERE fk_coming_experiment =:eid;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                res = Db.ExecuteNonQuery(cmd, false);


                //                //Remove all stock transactions connected to experiment
                //                cmd.Parameters.Clear();
                //                cmd.CommandText =
                //                    @"DELETE FROM public.stock_transaction
                //                        WHERE fk_experiment_coming=:ecomid;";

                //                Db.CreateParameterFunc(cmd, "@ecomid", experimentId, NpgsqlDbType.Integer);

                //                res = Db.ExecuteNonQuery(cmd, false);
                //                //if (res <= 0)
                //                //{
                //                //    t.Rollback();
                //                //    throw new Exception("Error reverting stock transactions");
                //                //}


                t.Commit();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                t.Rollback();
                throw new Exception(ex.Message);
                //throw new Exception("Error saving experiment", ex);
            }
            return 0;
        }
        /// <summary>
        /// Check if battery component id is obligatory to fill
        /// </summary>
        /// <param name="batteryComponentId"></param>
        /// <returns></returns>
        public static bool BatteryComponentObligatory(int batteryComponentId)
        {
            if (batteryComponentId == 1 || batteryComponentId == 2 || batteryComponentId == 4 || batteryComponentId == 6)
                return true;
            return false;
        }

        public static int AddBatteryComponent(int experimentId, AddBatteryComponentRequest req, int researchGroupId)
        {
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandType = CommandType.Text;

            int componentTypeId = 0;
            switch (req.componentType)
            {
                case "Anode":
                    componentTypeId = 1;
                    break;
                case "Cathode":
                    componentTypeId = 2;
                    break;
                case "Separator":
                    componentTypeId = 3;
                    break;
                case "Electrolyte":
                    componentTypeId = 4;
                    break;
                case "ReferenceElectrode":
                    componentTypeId = 5;
                    break;
                case "Casing":
                    componentTypeId = 6;
                    break;
            }

            var res = 0;
            NpgsqlTransaction t = cmd.Connection.BeginTransaction();

            try
            {
                //REMOVE CONTENT
                res = RemoveComponentContents(experimentId, componentTypeId, cmd);

                //CHECK CONTENT BUT NOT AGAINST STOCK
                //INSERT CONTENT
                bool componentEmpty = (bool)req.componentEmpty;
                if (!BatteryComponentObligatory(componentTypeId) && !componentEmpty || BatteryComponentObligatory(componentTypeId))
                    res = AddBatteryComponentContent(req, componentTypeId, researchGroupId, cmd);


                t.Commit();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                t.Rollback();
                cmd.Connection.Close();
                throw new Exception(ex.Message);
            }

            return 0;
        }

        public static int AddBatteryComponentCommercialType(int experimentId, int componentTypeId, int? commercialTypeId, int researchGroupId)
        {
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandType = CommandType.Text;

            var res = 0;

            NpgsqlTransaction t = cmd.Connection.BeginTransaction();

            try
            {
                //REMOVE CONTENT
                res = RemoveComponentContents(experimentId, componentTypeId, cmd);

                //INSERT COMMERCIAL COMPONENT
                //ONLY IF NOT EMPTY
                if (commercialTypeId != null)
                    res = AddBatteryComponentContentCommercialType(experimentId, componentTypeId, (int)commercialTypeId, researchGroupId, cmd);


                t.Commit();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                t.Rollback();
                cmd.Connection.Close();
                throw new Exception(ex.Message);
            }

            return 0;
        }
        /// <summary>
        /// Remove all content in battery component
        /// </summary>
        /// <param name="experimentId"></param>
        /// <param name="componentTypeId"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static int RemoveComponentContents(int experimentId, int componentTypeId, NpgsqlCommand cmd)
        {
            //Delete battery component entries, experiment_process entries, and particular process enties (e.g. Milling)
            int res = 0;

            if (cmd != null)
            {
                cmd.Parameters.Clear();
            }
            else
            {
                cmd = Db.CreateCommand();

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
            }
            try
            {
                cmd.CommandText =
                    @"DELETE FROM public.battery_component
                                WHERE fk_experiment=:eid and
                                      fk_battery_component_type=:bctid;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bctid", componentTypeId, NpgsqlDbType.Integer);
                Db.ExecuteNonQuery(cmd, false);

                //REMOVE ALL EQUIPMENT ATTRIBUTES
                cmd.CommandText =
                                  @"DELETE FROM public.equipment_attribute_values
                                    WHERE fk_experiment_process IN (
                                      SELECT experiment_process_id
                                      FROM public.experiment_process
                                      WHERE fk_experiment=:eid and
                                        fk_battery_component_type=:bctid
                                      );";

                Db.ExecuteNonQuery(cmd, false);

                //REMOVE ALL OF THE PROCESSES

                cmd.CommandText =
                    @"DELETE FROM public.experiment_process
                                WHERE fk_experiment=:eid and
                                      fk_battery_component_type=:bctid;";

                Db.ExecuteNonQuery(cmd, false);

                //Delete measurements for entire component
                cmd.CommandText =
                    @"DELETE FROM public.measurements
                                WHERE fk_experiment=:eid and
                                      fk_battery_component_type=:bctid;";
                Db.ExecuteNonQuery(cmd, false);

                //t.Commit();
                //cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        /// <summary>
        /// Remove all stock transactions for experiment
        /// </summary>
        /// <param name="experimentId"></param>
        /// <param name="researchGroupId"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static int RemoveExperimentStock(int experimentId, int researchGroupId, NpgsqlCommand cmd)
        {
            int res = 0;

            if (cmd != null)
            {
                cmd.Parameters.Clear();
            }
            else
            {
                cmd = Db.CreateCommand();

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
            }
            try
            {
                //REMOVE EXPERIMENT STOCK ENTRIES
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"DELETE FROM public.stock_transaction
                                        WHERE fk_experiment_coming=:ecomid;";

                Db.CreateParameterFunc(cmd, "@ecomid", experimentId, NpgsqlDbType.Integer);

                res = Db.ExecuteNonQuery(cmd, false);
                //if (res <= 0)
                //{
                //    throw new Exception("Error removing batch stock info");
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return 0;
        }
        public static int AddBatteryComponentContent(AddBatteryComponentRequest req, int componentTypeId, int researchGroupId, NpgsqlCommand cmd)
        {
            if (cmd != null)
            {
                cmd.Parameters.Clear();
            }
            else
            {
                cmd = Db.CreateCommand();

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
            }
            cmd.CommandType = CommandType.Text;

            //NpgsqlTransaction t = cmd.Connection.BeginTransaction();

            int res = 0;
            int experimentId = 0;
            int returnedExperimentProcessID = 0;
            int operatorId = 0;


            List<AddBatteryComponentStepRequest> componentStepsContentList;

            try
            {
                experimentId = (int)req.experimentId;
                operatorId = (int)req.userId;
                componentStepsContentList = req.componentStepsContentList;
                if (componentStepsContentList.Count <= 0)
                {
                    throw new Exception(req.componentType + ": " + "At least one step needs to be filled out");
                }
                foreach (AddBatteryComponentStepRequest step in componentStepsContentList)
                {
                    List<BatteryComponentExt> stepContentList = step.stepContent;
                    //if (stepContentList.Count <= 0)
                    //{
                    //    throw new Exception(req.componentType + ": " + "One of the steps is missing a material or batch");
                    //}

                    List<dynamic> stepProcessList = step.stepProcesses;
                    //if (stepProcessList.Count <= 0)
                    //{
                    //    throw new Exception(req.componentType + ": " + "One of the steps is missing a process");
                    //}
                    if (stepContentList.Count <= 0 && stepProcessList.Count <= 0)
                    {
                        throw new Exception(req.componentType + ": " + "A step needs to contain at least one material/batch or a process");
                    }

                    if (stepContentList.Count > 0)
                    {
                        string invalidPercentageMessage = "" + req.componentType + ": Invalid percentage of active value for ";
                        foreach (BatteryComponentExt stepContent in step.stepContent)
                        {
                            if (stepContent.weight != null && stepContent.weight <= 0)
                            {
                                throw new Exception("Material/Batch quantity must be greater then zero!");
                            }
                            if (stepContent.percentageOfActive != null)
                            {
                                if (stepContent.percentageOfActive < 0 || stepContent.percentageOfActive > 100)
                                {
                                    if (stepContent.fkStepMaterial != null)
                                    {
                                        invalidPercentageMessage += stepContent.materialName;
                                    }
                                    else
                                    {
                                        invalidPercentageMessage += stepContent.batchSystemLabel;
                                    }
                                    //faultyComponentId = componentTypeId;
                                    Exception ex = new Exception(invalidPercentageMessage);
                                    //ex.Data.Add("faultyComponentId", componentTypeId);
                                    throw ex;
                                }
                            }
                        }
                    }

                }
            }
            catch (ValidationException ve)
            {
                //do whatever
                throw new Exception(ve.Message);
            }

            try
            {
                //INSERT COMPONENT STEP CONTENTS
                foreach (AddBatteryComponentStepRequest step in componentStepsContentList)
                {
                    foreach (BatteryComponentExt batteryComponent in step.stepContent)
                    {
                        //fk_material_type,
                        //fk_stored_in_type,
                        cmd.Parameters.Clear();
                        cmd.CommandText =
                    @"INSERT INTO public.battery_component (
                        fk_experiment,
                        fk_battery_component_type,
                        step,
                        fk_step_material,
                        fk_step_batch,
                        weight,
                        fk_function,
                        percentage_of_active,
                        order_in_step,
                        fk_commercial_type,
                        is_complete
                        )
                    VALUES (:eid, :bctid, :step, :smid, :sbid, :w, :fid, :poa, :ois, :comt, :iscomplete)
                        RETURNING battery_component_id;";

                        Db.CreateParameterFunc(cmd, "@eid", batteryComponent.fkExperiment, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@bctid", componentTypeId, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@step", batteryComponent.step, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@smid", batteryComponent.fkStepMaterial, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@sbid", batteryComponent.fkStepBatch, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@w", batteryComponent.weight, NpgsqlDbType.Double);
                        Db.CreateParameterFunc(cmd, "@fid", batteryComponent.fkFunction, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@poa", batteryComponent.percentageOfActive, NpgsqlDbType.Double);
                        //Db.CreateParameterFunc(cmd, "@mtid", batteryComponent.fkMaterialType, NpgsqlDbType.Integer);
                        //Db.CreateParameterFunc(cmd, "@sit", batteryComponent.fkStoredInType, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@ois", batteryComponent.orderInStep, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@comt", batteryComponent.fkCommercialType, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@iscomplete", false, NpgsqlDbType.Boolean);

                        int returnedBatteryComponentID = int.Parse(Db.ExecuteScalar(cmd, false));
                        if (returnedBatteryComponentID <= 0)
                        {
                            //t.Rollback();
                            throw new Exception(req.componentType + ": " + "Error inserting component step content");
                        }
                        //res = Db.ExecuteNonQuery(cmd, false);
                        //if (res <= 0)
                        //{
                        //    t.Rollback();
                        //    throw new Exception(req.componentType + ": " + "Error inserting component step content");
                        //}



                        if (batteryComponent.measurements != null)
                        {
                            MeasurementsExt measurements = (MeasurementsExt)batteryComponent.measurements;
                            measurements.fkExperiment = experimentId;
                            measurements.fkBatteryComponentType = componentTypeId;
                            measurements.fkBatteryComponentContent = returnedBatteryComponentID;
                            measurements.fkMeasurementLevelType = 1;
                            //measurement level type id: content=1; step=2; component=3; batch_content=4; batch=5;

                            //processValidationResult = ValidationHelper.IsModelValidWithErrors(heating);
                            //if (processValidationResult.Count != 0)
                            //{
                            //    throw new Exception(processValidationResult[0].ErrorMessage);
                            //}

                            MeasurementsDa.AddMeasurements(measurements, cmd);
                        }

                    }
                    object submittedItemExperimentProcess;
                    /* object submittedItemProcessAttributes;
                     object submittedItemEquipmentSettings;*/
                    foreach (dynamic experimentProcessData in step.stepProcesses)
                    {
                        string processType = experimentProcessData.processType;
                        int processTypeId = (int)experimentProcessData.fkProcessType;
                        string experimentProcessDataString = Convert.ToString(experimentProcessData);
                        //string experimentProcessAttributesString = Convert.ToString(experimentProcessData.processAttributes);
                        string equipmentAttributes = Convert.ToString(experimentProcessData.equipmentSettings);
                        List<EquipmentSettingsValue> equipmentAttributeList = JsonConvert.DeserializeObject<List<EquipmentSettingsValue>>(equipmentAttributes) as List<EquipmentSettingsValue>;
                        // equipmentAttributeList = equipmentAttributeList.Where(x => x.isParent == false).ToList();
                        //var equipmentId = (int)experimentProcessData.processAttributes.fkEquipment;
                        int? equipmentModelId = null;
                        equipmentModelId = experimentProcessData.equipmentModelId;
                        int? equipmentId;
                        equipmentId = experimentProcessData.equipmentId;
                        //string equipmentSettingsString = Convert.ToString(experimentProcessData.equipmentSettings);

                        submittedItemExperimentProcess = JsonConvert.DeserializeObject<ExperimentProcessExt>(experimentProcessDataString);
                        ExperimentProcessExt experimentProcess = (ExperimentProcessExt)submittedItemExperimentProcess;
                        experimentProcess.fkExperiment = experimentId;
                        var validationResult = ValidationHelper.IsModelValidWithErrors(experimentProcess);
                        if (validationResult.Count != 0)
                        {
                            throw new Exception(validationResult[0].ErrorMessage);
                        }
                        int returnedProcessId = ProcessTypeDa.GetProcess(equipmentId, experimentProcess.fkProcessType, equipmentModelId);
                        cmd.Parameters.Clear();
                        cmd.CommandText =
                        @"INSERT INTO public.experiment_process (fk_experiment, fk_battery_component_type, step, process_order_in_step, label, is_complete, fk_process) 
                        VALUES (:eid, :bctid, :step, :pois, :label, :iscomplete, :process)
                        RETURNING experiment_process_id;";

                        Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@bctid", componentTypeId, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@step", experimentProcess.step, NpgsqlDbType.Integer);
                        //Db.CreateParameterFunc(cmd, "@ptid", experimentProcess.fkProcessType, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@pois", experimentProcess.processOrderInStep, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@label", experimentProcess.label, NpgsqlDbType.Text);
                        //Db.CreateParameterFunc(cmd, "@eq", equipmentId, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@iscomplete", false, NpgsqlDbType.Boolean);
                        Db.CreateParameterFunc(cmd, "@process", returnedProcessId, NpgsqlDbType.Integer);

                        returnedExperimentProcessID = int.Parse(Db.ExecuteScalar(cmd, false));
                        if (returnedExperimentProcessID <= 0)
                        {
                            //t.Rollback();
                            throw new Exception(req.componentType + ": " + "Error inserting experiment process");
                        }
                        var processValidationResult = new List<ValidationResult>();
                        processValidationResult = ValidationHelper.IsModelValidWithErrors(equipmentAttributeList);
                        if (processValidationResult.Count != 0)
                        {
                            throw new Exception(processValidationResult[0].ErrorMessage);
                        }

                        EquipmentSettingsDa.AddEquipmentAttributes(cmd, equipmentAttributeList, returnedExperimentProcessID);
                    }

                    if (step.measurements != null)
                    {
                        MeasurementsExt measurements = (MeasurementsExt)step.measurements;
                        measurements.fkExperiment = experimentId;
                        measurements.fkBatteryComponentType = componentTypeId;
                        measurements.stepId = step.stepNumber;
                        measurements.fkBatteryComponentContent = null;
                        measurements.fkMeasurementLevelType = 2;
                        MeasurementsDa.AddMeasurements(measurements, cmd);
                    }
                }
                if (req.measurements != null)
                {
                    MeasurementsExt measurements = (MeasurementsExt)req.measurements;
                    measurements.fkExperiment = experimentId;
                    measurements.fkBatteryComponentType = componentTypeId;
                    measurements.stepId = null;
                    measurements.fkBatteryComponentContent = null;
                    measurements.fkMeasurementLevelType = 3;
                    MeasurementsDa.AddMeasurements(measurements, cmd);
                }

                //cmd.Parameters.Clear();
                //cmd.CommandText =
                //    @"UPDATE public.experiment
                //        //SET date_modified=now()::timestamp
                //        WHERE experiment_id=:eid;";

                //Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                //res = Db.ExecuteNonQuery(cmd, false);
                //if (res <= 0)
                //{
                //    throw new Exception("Error updating experiment info");
                //}

                //t.Commit();
                //cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                //t.Rollback();
                throw new Exception(ex.Message);
            }
            return 0;
        }

        public static int AddBatteryComponentContentCommercialType(int experimentId, int componentTypeId, int commercialTypeId, int researchGroupId, NpgsqlCommand cmd)
        {
            if (cmd != null)
            {
                cmd.Parameters.Clear();
            }
            else
            {
                cmd = Db.CreateCommand();

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
            }
            cmd.CommandType = CommandType.Text;

            //NpgsqlTransaction t = cmd.Connection.BeginTransaction();

            int res = 0;
            try
            {
                cmd.CommandText =
                                    @"INSERT INTO public.battery_component (
                                    fk_experiment,
                                    fk_battery_component_type,
                                    fk_commercial_type,
                                    is_complete
                                    )
                                    VALUES (:eid, :bctid, :comt, :iscomplete);";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bctid", componentTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@comt", commercialTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@iscomplete", false, NpgsqlDbType.Boolean);

                res = Db.ExecuteNonQuery(cmd, false);
                if (res <= 0)
                {
                    throw new Exception("Error inserting commercial type component");
                }

                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.experiment
                        SET date_modified=now()::timestamp
                        WHERE experiment_id=:eid;";

                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                res = Db.ExecuteNonQuery(cmd, false);
                if (res <= 0)
                {
                    throw new Exception("Error updating experiment info");
                }

                //t.Commit();
                //cmd.Connection.Close();

            }
            catch (Exception ex)
            {
                //t.Rollback();
                throw new Exception(ex.Message);
            }
            return 0;
        }

        public static int CopyExperimentContents(int templateId, int newExperimentId)
        {
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            NpgsqlTransaction t = cmd.Connection.BeginTransaction();

            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"INSERT INTO public.battery_component(
                             fk_experiment, is_complete, fk_battery_component_type, step, fk_step_material, fk_step_batch, weight, fk_function, percentage_of_active, order_in_step, fk_commercial_type, is_saved_as_batch)
                      SELECT :newExperimentId, false, fk_battery_component_type, step, fk_step_material, fk_step_batch, weight, fk_function, percentage_of_active, order_in_step, fk_commercial_type, is_saved_as_batch
	                      FROM public.battery_component
                        WHERE fk_experiment = :templateId;";

                Db.CreateParameterFunc(cmd, "@newExperimentId", newExperimentId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@templateId", templateId, NpgsqlDbType.Integer);


                var res = Db.ExecuteNonQuery(cmd, false);
                //if (res <= 0)
                //{
                //    t.Rollback();
                //    throw new Exception("Error inserting new experiment");
                //}

                List<ExperimentProcessExt> experimentProcessList = ExperimentProcessDa.GetAllExperimentProcesses(null, templateId);
                if (experimentProcessList != null)
                {
                    foreach (ExperimentProcessExt experimentProcess in experimentProcessList)
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText =
                            @"INSERT INTO public.experiment_process(
                             fk_experiment, is_complete, fk_battery_component_type, step, fk_process, process_order_in_step, label)
                      VALUES( :newExperimentId, :iscomplete, :bctid, :step, :ptid, :pois, :label)	                  
                      RETURNING experiment_process_id;";

                        Db.CreateParameterFunc(cmd, "@newExperimentId", newExperimentId, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@iscomplete", false, NpgsqlDbType.Boolean);

                        Db.CreateParameterFunc(cmd, "@bctid", experimentProcess.fkBatteryComponentType, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@step", experimentProcess.step, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@ptid", experimentProcess.fkProcess, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@pois", experimentProcess.processOrderInStep, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@label", experimentProcess.label, NpgsqlDbType.Text);

                        int returnedExProcessId = int.Parse(Db.ExecuteScalar(cmd, false));
                        //get equipmnt attributes from template
                        List<EquipmentSettingsValue> equipmentAttributeList;
                        equipmentAttributeList = EquipmentSettingsDa.GetAllEquipmentSettingsValue(templateId, experimentProcess.experimentProcessId, null, null);
                        //insert equipment attributes 
                        EquipmentSettingsDa.AddEquipmentAttributes(cmd, equipmentAttributeList, returnedExProcessId);
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
            return 0;
        }
        public static int currentUserId;
        public static Experiment CreateExperimentObject(DataRow dr)
        {
            Boolean? hasTestResultsDocVar = (Boolean?)null;
            if (dr.Table.Columns.Contains("has_test_results_doc"))
            {
                hasTestResultsDocVar = dr["has_test_results_doc"] != DBNull.Value ? Boolean.Parse(dr["has_test_results_doc"].ToString()) : false;
            }


            var experiment = new Experiment
            {
                experimentId = (int)dr["experiment_id"],
                experimentSystemLabel = dr["experiment_system_label"].ToString(),
                experimentPersonalLabel = dr["experiment_personal_label"].ToString(),
                experimentDescription = dr["experiment_description"].ToString(),
                fkResearchGroup = dr["fk_research_group"] != DBNull.Value ? int.Parse(dr["fk_research_group"].ToString()) : (int?)null,
                fkUser = dr["fk_user"] != DBNull.Value ? int.Parse(dr["fk_user"].ToString()) : (int?)null,
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,
                dateModified = dr["date_modified"] != DBNull.Value ? DateTime.Parse(dr["date_modified"].ToString()) : (DateTime?)null,
                isComplete = dr["is_complete"] != DBNull.Value ? Boolean.Parse(dr["is_complete"].ToString()) : false,
                fkTemplate = dr["fk_template"] != DBNull.Value ? int.Parse(dr["fk_template"].ToString()) : (int?)null,
                hasTestResultsDoc = hasTestResultsDocVar,
                fkEditedBy = dr["fk_edited_by"] != DBNull.Value ? int.Parse(dr["fk_edited_by"].ToString()) : (int?)null,
                fkProject = dr["fk_project"] != DBNull.Value ? int.Parse(dr["fk_project"].ToString()) : (int?)null,
                fkSharingType = dr["fk_sharing_type"] != DBNull.Value ? int.Parse(dr["fk_sharing_type"].ToString()) : (int?)null

            };
            return experiment;

        }
        public static ExperimentExt CreateExperimentObjectExt(DataRow dr)
        {
            var experiment = CreateExperimentObject(dr);

            string operatorUsernameVar = dr.Table.Columns.Contains("operator_username") ? dr["operator_username"].ToString() : null;
            string researchGroupNameVar = dr.Table.Columns.Contains("research_group_name") ? dr["research_group_name"].ToString() : null;
            string researchGroupAcronymVar = dr.Table.Columns.Contains("acronym") ? dr["acronym"].ToString() : null;
            string editingOperatorUsernameVar = dr.Table.Columns.Contains("editing_operator_username") ? dr["editing_operator_username"].ToString() : null;
            string anodeTotalActiveMaterialsVar = dr.Table.Columns.Contains("anode_total_active_materials") ? dr["anode_total_active_materials"].ToString() : null;
            string anodeActiveMaterialsVar = dr.Table.Columns.Contains("anode_active_materials") ? dr["anode_active_materials"].ToString() : null;
            string anodeActivePercentagesVar = dr.Table.Columns.Contains("anode_active_percentages") ? dr["anode_active_percentages"].ToString() : null;
            string cathodeTotalActiveMaterialsVar = dr.Table.Columns.Contains("cathode_total_active_materials") ? dr["cathode_total_active_materials"].ToString() : null;
            string cathodeActiveMaterialsVar = dr.Table.Columns.Contains("cathode_active_materials") ? dr["cathode_active_materials"].ToString() : null;
            string cathodeActivePercentagesVar = dr.Table.Columns.Contains("cathode_active_percentages") ? dr["cathode_active_percentages"].ToString() : null;
            string projectNameVar = dr.Table.Columns.Contains("project_name") ? dr["project_name"].ToString() : null;
            string projectAcronymVar = dr.Table.Columns.Contains("project_acronym") ? dr["project_acronym"].ToString() : null;
            string testGroupNameVar = dr.Table.Columns.Contains("test_group_name") ? dr["test_group_name"].ToString() : null;

            var experimentExt = new ExperimentExt(experiment)
            {
                operatorUsername = operatorUsernameVar,
                researchGroupName = researchGroupNameVar,
                researchGroupAcronym = researchGroupAcronymVar,
                editingOperatorUsername = editingOperatorUsernameVar,
                projectName = projectNameVar,
                projectAcronym = projectAcronymVar,
                testGroupName = testGroupNameVar,
                anodeTotalActiveMaterials = anodeTotalActiveMaterialsVar,
                anodeActiveMaterials = anodeActiveMaterialsVar,
                anodeActivePercentages = anodeActivePercentagesVar,
                cathodeTotalActiveMaterials = cathodeTotalActiveMaterialsVar,
                cathodeActiveMaterials = cathodeActiveMaterialsVar,
                cathodeActivePercentages = cathodeActivePercentagesVar
            };
            return experimentExt;
        }
    }
}