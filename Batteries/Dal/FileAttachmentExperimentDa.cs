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
    public class FileAttachmentExperimentDa
    {
        public static List<FileAttachmentExperimentExt> GetFileAttachmentExperiments(long? fileAttachmentId = null, string elementType = null, long? experimentId = null, int? componentTypeId = null, int? stepId = null, int? documentTypeId = null)
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
                    FROM file_attachment_experiment f
                    LEFT JOIN document_type dt ON f.fk_type = dt.document_type_id
                    WHERE (f.file_attachment_id = :fid or :fid is null) AND
                          (f.element_type = :etype or :etype is null) AND
                          (f.experiment_id = :eid or :eid is null) AND
                          (f.component_type_id = :ctid or :ctid is null) AND
                          (f.step_id = :sid or :sid is null) AND
                          (f.fk_type = :dtid or :dtid is null);";

                Db.CreateParameterFunc(cmd, "@fid", fileAttachmentId, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@etype", elementType, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@ctid", componentTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@sid", stepId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@dtid", documentTypeId, NpgsqlDbType.Bigint);

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

            List<FileAttachmentExperimentExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<FileAttachmentExperimentExt> GetFileAttachmentExperimentForComponentSteps(string elementType = null, long? experimentId = null, int? componentTypeId = null, int? documentTypeId = null)
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
                    FROM file_attachment_experiment f
                    LEFT JOIN document_type dt ON f.fk_type = dt.document_type_id
                    WHERE 
                          (f.experiment_id = :eid or :eid is null) AND
                          (f.component_type_id = :ctid or :ctid is null) AND
                          (f.fk_type = :dtid or :dtid is null)
                    ORDER BY f.component_type_id, f.step_id, f.file_attachment_id
                        ;";

                //Db.CreateParameterFunc(cmd, "@etype", elementType, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@ctid", componentTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@dtid", documentTypeId, NpgsqlDbType.Bigint);

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

            List<FileAttachmentExperimentExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        
        public static int AddFileAttachmentExperiment(FileAttachmentExperiment file)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"INSERT INTO file_attachment_experiment(experiment_id, component_type_id, step_id, element_type, description, filename, server_filename, extension, created_on, fk_uploaded_by, fk_type)
                    VALUES (:eid, :ctid, :sid, :etype, :desc, :fname, :sfname, :ext, now(), :uploadedBy, :typeId);";

                Db.CreateParameterFunc(cmd, "@eid", file.experimentId, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@ctid", file.componentTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@sid", file.stepId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@etype", file.elementType, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@desc", file.description, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@fname", file.filename, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@sfname", file.serverFilename, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@ext", file.extension, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@uploadedBy", file.fkUploadedBy, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@typeId", file.fkType, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@deleted", file.description, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting file attachment", ex);
            }

            return 0;
        }
        public static int UpdateFileAttachmentExperimentStep(string elementType, int experimentId, int componentTypeId, int stepId, int newStepId)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.file_attachment_experiment
                        SET 
                        step_id=:nsid

                        WHERE element_type=:etype AND
                        experiment_id=:eid AND
                        component_type_id=:ctid AND
                        step_id = sid
                        ;";

                Db.CreateParameterFunc(cmd, "@etype", elementType, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@ctid", componentTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@sid", stepId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@nsid", newStepId, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating file attachment", ex);
            }
            return 0;
        }

        public static int DeleteFileAttachmentExperiment(long fileAttachmentId)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM file_attachment_experiment
                      WHERE file_attachment_id=:fid;";

                Db.CreateParameterFunc(cmd, "@fid", fileAttachmentId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting file attachment", ex);
            }

            return 0;
        }

        private static FileAttachmentExperiment CreateObject(DataRow dr)
        {
            var file = new FileAttachmentExperiment
            {
                fileAttachmentId = (long)dr["file_attachment_id"],
                experimentId = dr["experiment_id"] != DBNull.Value ? int.Parse(dr["experiment_id"].ToString()) : (int?)null,
                componentTypeId = dr["component_type_id"] != DBNull.Value ? int.Parse(dr["component_type_id"].ToString()) : (int?)null,
                stepId = dr["step_id"] != DBNull.Value ? int.Parse(dr["step_id"].ToString()) : (int?)null,
                elementType = dr["element_type"].ToString(),
                description = dr["description"].ToString(),
                filename = dr["filename"].ToString(),
                serverFilename = dr["server_filename"].ToString(),
                extension = dr["extension"].ToString(),
                createdOn = dr["created_on"] != DBNull.Value ? DateTime.Parse(dr["created_on"].ToString()) : (DateTime?)null,
                fkUploadedBy = dr["fk_uploaded_by"] != DBNull.Value ? int.Parse(dr["fk_uploaded_by"].ToString()) : (int?)null,
                fkDeletedBy = dr["fk_deleted_by"] != DBNull.Value ? int.Parse(dr["fk_deleted_by"].ToString()) : (int?)null,
                deletedOn = dr["deleted_on"] != DBNull.Value ? DateTime.Parse(dr["deleted_on"].ToString()) : (DateTime?)null,
                fkType = dr["fk_type"] != DBNull.Value ? int.Parse(dr["fk_type"].ToString()) : (int?)null,
                isDeleted = dr["is_deleted"] != DBNull.Value ? Boolean.Parse(dr["is_deleted"].ToString()) : false,
            };
            return file;
        }
        private static FileAttachmentExperimentExt CreateObjectExt(DataRow dr)
        {
            var documentType = CreateObject(dr);

            var documentTypeExt = new FileAttachmentExperimentExt(documentType)
            {
                documentTypeName = dr["document_type_name"].ToString(),
            };
            return documentTypeExt;
        }
    }
}