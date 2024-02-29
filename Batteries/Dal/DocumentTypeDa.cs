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
    public class DocumentTypeDa
    {
        public static List<DocumentTypeExt> GetAllDocumentTypes(int? documentTypeId = null)
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
                    FROM document_type dt
                    WHERE (dt.document_type_id = :dtid or :dtid is null);";

                Db.CreateParameterFunc(cmd, "@dtid", documentTypeId, NpgsqlDbType.Integer);

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

            List<DocumentTypeExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<DocumentTypeExt> GetDocumentTypesByName(string search)
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
                    FROM document_type dt
                    
                    WHERE (lower(dt.document_type_name) LIKE lower('%'|| :search ||'%') or :search is null)
                    AND dt.document_type_id <> 3 AND dt.document_type_id <> 4
                    
                    LIMIT 10
                        ;";

                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);

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

            List<DocumentTypeExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }

        public static int AddDocumentType(DocumentType documentType)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"INSERT INTO public.document_type (document_type_name)
                    VALUES (:dtn);";

                Db.CreateParameterFunc(cmd, "@dtn", documentType.documentTypeName, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting document type", ex);
            }

            return 0;
        }
        public static int UpdateDocumentType(DocumentType documentType)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.document_type
                        SET document_type_name=:dtn
                        WHERE document_type_id=:dtid;";

                Db.CreateParameterFunc(cmd, "@dtn", documentType.documentTypeName, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@dtid", documentType.documentTypeId, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating document type info", ex);
            }
            return 0;
        }
        public static int DeleteDocumentType(int documentTypeId)
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
                            @"SELECT fa.file_attachment_id 
                            FROM file_attachment fa
                            WHERE fa.fk_type=:dtid;";

                Db.CreateParameterFunc(cmd, "@dtid", documentTypeId, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count > 0)
                {
                    throw new Exception("There is some file attachment associated to this document type!");
                }

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.document_type
                                WHERE document_type_id=:dtid;";

                Db.CreateParameterFunc(cmd, "@dtid", documentTypeId, NpgsqlDbType.Integer);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        public static DocumentType CreateObject(DataRow dr)
        {
            var documentType = new DocumentType
            {
                documentTypeId = (int)dr["document_type_id"],
                documentTypeName = dr["document_type_name"].ToString(),
            };
            return documentType;
        }
        private static DocumentTypeExt CreateObjectExt(DataRow dr)
        {
            var documentType = CreateObject(dr);

            var documentTypeExt = new DocumentTypeExt(documentType)
            {
            };
            return documentTypeExt;
        }
    }
}