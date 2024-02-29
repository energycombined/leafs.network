using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class FileAttachment
    {
        public long fileAttachmentId { get; set; }
        public string filename { get; set; }
        public string serverFilename { get; set; }
        public string extension { get; set; }
        public string elementType { get; set; }
        public long? elementId { get; set; }
        public string description { get; set; }
        public DateTime? createdOn { get; set; }
        public int? fkUploadedBy { get; set; }
        public int? fkDeletedBy { get; set; }
        public DateTime? deletedOn { get; set; }
        public int? fkType { get; set; }
        public Boolean? isDeleted { get; set; }

    }
}