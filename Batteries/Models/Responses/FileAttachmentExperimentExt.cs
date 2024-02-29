using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class FileAttachmentExperimentExt : FileAttachmentExperiment
    {
        public string documentTypeName { get; set; }
        public FileAttachmentExperimentExt(FileAttachmentExperiment e)
        {
            if (e != null)
            {
                this.fileAttachmentId = e.fileAttachmentId;
                this.filename = e.filename;
                this.serverFilename = e.serverFilename;
                this.extension = e.extension;
                this.elementType = e.elementType;
                this.experimentId = e.experimentId;
                this.componentTypeId = e.componentTypeId;
                this.stepId = e.stepId;
                this.componentContentId = e.componentContentId;
                this.description = e.description;
                this.createdOn = e.createdOn;
                this.fkUploadedBy = e.fkUploadedBy;
                this.fkDeletedBy = e.fkDeletedBy;
                this.deletedOn = e.deletedOn;
                this.fkType = e.fkType;
                this.isDeleted = e.isDeleted;
            }
        }
    }
}