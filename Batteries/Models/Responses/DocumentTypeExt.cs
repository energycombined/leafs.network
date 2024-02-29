using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class DocumentTypeExt : DocumentType
    {
        public DocumentTypeExt(DocumentType e)
        {
            if (e != null)
            {
                this.documentTypeId = e.documentTypeId;
                this.documentTypeName = e.documentTypeName;
            }
        }
    }
}