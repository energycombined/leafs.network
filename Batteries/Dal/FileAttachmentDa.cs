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
    public class FileAttachmentDa
    {
        public static List<FileAttachmentExt> GetFileAttachments(long? fileAttachmentId = null, string elementType = null, long? elementId = null, int? documentTypeId = null)
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
                    FROM file_attachment f
                    LEFT JOIN document_type dt ON f.fk_type = dt.document_type_id
                    WHERE (f.file_attachment_id = :fid or :fid is null) AND
                          (f.element_type = :etype or :etype is null) AND
                          (f.element_id = :eid or :eid is null) AND
                          (f.fk_type = :dtid or :dtid is null);";

                Db.CreateParameterFunc(cmd, "@fid", fileAttachmentId, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@etype", elementType, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@eid", elementId, NpgsqlDbType.Bigint);
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

            List<FileAttachmentExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }

        public static List<FileAttachmentExt> GetFileAttachmentsMeasurementData(int? testTypeId = null, int? experimentId = null, int? batchId = null, int? materialId = null,
            int? componentTypeId = null, int? stepId = null)
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
                    FROM file_attachment fa
                    LEFT JOIN test t ON fa.element_id = t.test_id 
                    LEFT JOIN document_type dt ON dt.document_type_id = fa.fk_type
                    LEFT JOIN test_type tt ON tt.test_type_id = t.fk_test_type
                    "
                    ;

                if (experimentId != null)
                {
                    if (componentTypeId != null && stepId != null)
                        cmd.CommandText += @"WHERE (t.fk_experiment = :eid) AND fk_battery_component_type=:comtype AND step_id=:step AND fk_measurement_level_type = 2"; //step level
                    else if (componentTypeId != null)
                        cmd.CommandText += @"WHERE (t.fk_experiment = :eid) AND fk_battery_component_type=:comtype AND fk_measurement_level_type = 3"; //component level
                    else
                        cmd.CommandText += @"WHERE (t.fk_experiment = :eid) AND fk_measurement_level_type = 6"; //experiment level
                }
                else if (batchId != null)
                {
                    cmd.CommandText += @"WHERE (t.fk_batch = :bid) AND fk_measurement_level_type = 5";
                }
                else
                {
                    cmd.CommandText += @"WHERE (t.fk_material = :mid) AND fk_measurement_level_type = 7";
                }

                cmd.CommandText += @"AND (t.fk_test_type = :ttid or :ttid is null)";

                Db.CreateParameterFunc(cmd, "@ttid", testTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@eid", experimentId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@mid", materialId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@comtype", componentTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@step", stepId, NpgsqlDbType.Integer);

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

            List<FileAttachmentExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }

        public static int AddFileAttachment(FileAttachment file)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"INSERT INTO file_attachment(element_id, element_type, description, filename, server_filename, extension, created_on, fk_uploaded_by, fk_type)
                    VALUES (:eid, :etype, :desc, :fname, :sfname, :ext, now(), :uploadedBy, :typeId);";

                Db.CreateParameterFunc(cmd, "@eid", file.elementId, NpgsqlDbType.Bigint);
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

        public static int DeleteFileAttachment(long fileAttachmentId)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM file_attachment
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

        public static int DeleteAllFileAttachmentByElement(string elementType, int elementId, NpgsqlCommand cmd)
        {
            bool isEnclosedInTransaction = true;

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
            try
            {
                cmd.CommandText =
                    @"DELETE FROM file_attachment
                      WHERE element_type=:etype AND
                      element_id=:eid
                    ;";

                Db.CreateParameterFunc(cmd, "@etype", elementType, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@eid", elementId, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd, false);

                if (!isEnclosedInTransaction)
                {
                    cmd.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting file attachment", ex);
            }

            return 0;
        }

        private static FileAttachment CreateObject(DataRow dr)
        {
            var file = new FileAttachment
            {
                fileAttachmentId = (long)dr["file_attachment_id"],
                elementId = (long)dr["element_id"],
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
        private static FileAttachmentExt CreateObjectExt(DataRow dr)
        {
            var documentType = CreateObject(dr);

            var documentTypeExt = new FileAttachmentExt(documentType)
            {
                documentTypeName = dr.Table.Columns.Contains("document_type_name") ? dr["document_type_name"].ToString() : null,
                testType = dr.Table.Columns.Contains("test_type") ? dr["test_type"].ToString() : null,
                testTypeSubcategory = dr.Table.Columns.Contains("test_type_subcategory") ? dr["test_type_subcategory"].ToString() : null
            };
            return documentTypeExt;
        }
    }
}