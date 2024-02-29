using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.FriendlyUrls;
using Batteries.Models;
using Batteries.Dal;
using Batteries.Helpers;

namespace Batteries.DocumentTypes
{
    public partial class Edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            var documentType = GetDocumentType(GetDocumentTypeIdFromUrl());
            Fill(documentType);
        }
        private int GetDocumentTypeIdFromUrl()
        {
            IList<string> segments = Request.GetFriendlyUrlSegments();
            int pId = -1;
            if (segments.Count != 0)
                int.TryParse(segments[0], out pId);
            return pId;
        }
        private DocumentType GetDocumentType(int documentTypeId)
        {
            var documentType = DocumentTypeDa.GetAllDocumentTypes(documentTypeId);
            return documentType[0];
        }
        private void Fill(DocumentType documentType)
        {
            TxtDocumentTypeName.Text = documentType.documentTypeName;
        }
        protected void UpdateButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                var documentType = new DocumentType
                {
                    documentTypeId = GetDocumentTypeIdFromUrl(),
                    documentTypeName = TxtDocumentTypeName.Text
                };
                var result = DocumentTypeDa.UpdateDocumentType(documentType);
                if (result == 0)
                {
                    NotifyHelper.Notify("Success", NotifyHelper.NotifyType.success, "");
                    RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
                }
                else
                    NotifyHelper.Notify("Document type info not updated", NotifyHelper.NotifyType.danger, "");
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                NotifyHelper.Notify(ex.Message, NotifyHelper.NotifyType.danger, "");
            }
        }
        protected void CancelButton_OnClick(object sender, EventArgs e)
        {
            RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
        }
    }
}