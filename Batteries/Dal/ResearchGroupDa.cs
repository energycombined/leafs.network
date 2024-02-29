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
    public class ResearchGroupDa
    {
        public static List<ResearchGroupExt> GetAllResearchGroups(int? researchGroupId = null)
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
                    FROM research_group rg
                    LEFT JOIN users u on rg.fk_operator = u.user_id
                    WHERE (rg.research_group_id = :rgid or :rgid is null);";

                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);

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

            List<ResearchGroupExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static string GetAllResearchGroupsJsonForDropdown(int? researchGroupId = null)
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
                      FROM ( select research_group_id as id, COALESCE(rg.research_group_name, '') as text
                              from research_group rg
                              where (rg.research_group_id = :rgid or :rgid is null)
                          ) as t;";

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
        public static List<ResearchGroup> GetResearchGroupsByName(string search = null, int? type = null, int? page = 1, int? projectId = null)
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

                    FROM research_group rg
                        
                    WHERE (lower(rg.research_group_name) LIKE lower('%'|| :search ||'%') or :search is null)
                    LIMIT 10 OFFSET :offset
                        ;";

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

            List<ResearchGroup> list = (from DataRow dr in dt.Rows select CreateObject(dr)).ToList();

            return list;
        }
        public static List<ResearchGroup> GetResearchGroupsByProjectId(string search = null, int? type = null, int? page = 1, int? projectId = null)
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
                    @"SELECT DISTINCT rg.research_group_id, rg.research_group_name, rg.date_created, rg.last_change, rg.fk_operator, rg.acronym FROM project_research_group prg
                    LEFT JOIN project p ON p.project_id = prg.fk_project
                    LEFT JOIN research_group rg ON rg.research_group_id = prg.fk_research_group
                        
                    WHERE (lower(rg.research_group_name) LIKE lower('%'|| :search ||'%') or :search is null)
                    AND (p.project_id = :pid OR :pid is null )
                    LIMIT 10 OFFSET :offset
                        ;";

                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@offset", (page - 1) * 10, NpgsqlDbType.Integer);
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

            List<ResearchGroup> list = (from DataRow dr in dt.Rows select CreateObject(dr)).ToList();

            return list;
        }
        public static List<ResearchGroupExt> GetResearchGroupsOutsideProjectPaged(int projectId, string search = null, int? page = 1)
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
                      FROM research_group rg
                        LEFT JOIN project_research_group prg on rg.research_group_id = prg.fk_research_group and (prg.fk_project = :pid)
                        WHERE prg.fk_project is null AND
                        ((lower(rg.research_group_name) LIKE lower('%'|| :search ||'%')) or :search is null)
                    
                        ORDER BY research_group_id DESC LIMIT 10 OFFSET :offset;
                        ";

                Db.CreateParameterFunc(cmd, "@pid", projectId, NpgsqlDbType.Integer);
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

            List<ResearchGroupExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<ResearchGroup> GetOtherResearchGroupsByName(int myResearchGroup, string search = null, int? type = null, int? page = 1)
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

                    FROM research_group rg
                        
                    WHERE (lower(rg.research_group_name) LIKE lower('%'|| :search ||'%') or :search is null) AND
                    rg.research_group_id != :rgid
                    LIMIT 10 OFFSET :offset
                        ;";

                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@offset", (page - 1) * 10, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", myResearchGroup, NpgsqlDbType.Integer);

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

            List<ResearchGroup> list = (from DataRow dr in dt.Rows select CreateObject(dr)).ToList();

            return list;
        }
        public static int AddResearchGroup(ResearchGroup researchGroup)
        {
            int result = 0;
            try
            {

                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"SELECT research_group_id FROM public.research_group                        
                        WHERE (lower(research_group_name) LIKE lower(:name)) OR (lower(acronym) LIKE lower(:acronym))                        
                    ;";

                Db.CreateParameterFunc(cmd, "@name", researchGroup.researchGroupName, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@acronym", researchGroup.acronym, NpgsqlDbType.Text);

                var res = Db.ExecuteScalar(cmd);
                if (res != "")
                {
                    throw new Exception("Research group name/acronym already exists!");
                }

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"INSERT INTO public.research_group (research_group_name, acronym, fk_operator, last_experiment_number, last_batch_number)
                    VALUES (:name, :acronym, :oid, 1, 1) RETURNING research_group_id;";

                Db.CreateParameterFunc(cmd, "@name", researchGroup.researchGroupName, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@acronym", researchGroup.acronym, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@oid", researchGroup.fkOperator, NpgsqlDbType.Integer);

                result = int.Parse(Db.ExecuteScalar(cmd));
            }
            catch (Exception ex)
            {
                //throw new Exception("Error inserting research group", ex);
                throw new Exception(ex.Message, ex);
            }

            return result;
        }
        public static int UpdateResearchGroup(ResearchGroup researchGroup)
        {
            try
            {
                var cmd = Db.CreateCommand();
                //Check if RG name exists
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"SELECT research_group_id FROM public.research_group                        
                        WHERE (lower(research_group_name) LIKE lower(:name))
                        AND research_group_id <> :rgid
                    ;";

                Db.CreateParameterFunc(cmd, "@name", researchGroup.researchGroupName, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroup.researchGroupId, NpgsqlDbType.Integer);

                var res = Db.ExecuteScalar(cmd);
                if (res != "")
                {
                    throw new Exception("Research group name already exists!");
                }

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.Parameters.Clear();
                cmd.CommandText =
                    @"UPDATE public.research_group
                        SET research_group_name=:name, last_change=now()::timestamp
                        WHERE research_group_id=:rgid;";

                Db.CreateParameterFunc(cmd, "@name", researchGroup.researchGroupName, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroup.researchGroupId, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                //throw new Exception("Error updating research group info", ex);
                throw new Exception(ex.Message, ex);
            }
            return 0;
        }
        public static int DeleteResearchGroup(int researchGroupId)
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
                            @"SELECT u.user_id 
                            FROM users u
                            WHERE u.fk_research_group=:rgid;";

                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count > 0)
                {
                    throw new Exception("This research group is connected to some users");
                }


                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.research_group
                                WHERE research_group_id=:rgid;";

                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }

        public static ResearchGroup CreateObject(DataRow dr)
        {
            int? lastExperimentNumber = (int?)null;
            if (dr.Table.Columns.Contains("last_experiment_number"))
            {
                lastExperimentNumber = dr["last_experiment_number"] != DBNull.Value ? int.Parse(dr["last_experiment_number"].ToString()) : (int?)null;
            }
            int? lastBatchNumber = (int?)null;
            if (dr.Table.Columns.Contains("last_batch_number"))
            {
                lastBatchNumber = dr["last_batch_number"] != DBNull.Value ? int.Parse(dr["last_batch_number"].ToString()) : (int?)null;
            }
            var researchGroup = new ResearchGroup
            {
                researchGroupId = int.Parse(dr["research_group_id"].ToString()),
                researchGroupName = dr["research_group_name"].ToString(),
                acronym = dr["acronym"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,
                lastChange = dr["last_change"] != DBNull.Value ? DateTime.Parse(dr["last_change"].ToString()) : (DateTime?)null,
                fkOperator = dr["fk_operator"] != DBNull.Value ? int.Parse(dr["fk_operator"].ToString()) : (int?)null,
                lastExperimentNumber = lastExperimentNumber,
                lastBatchNumber = lastBatchNumber
            };
            return researchGroup;
        }
        private static ResearchGroupExt CreateObjectExt(DataRow dr)
        {
            var researchGroup = CreateObject(dr);
            string operatorUsernameVar = dr.Table.Columns.Contains("operator_username") ? dr["operator_username"].ToString() : null;

            var researchGroupExt = new ResearchGroupExt(researchGroup)
            {
                operatorUsername = operatorUsernameVar
            };
            return researchGroupExt;
        }
    }
}