using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class BatchExt : Batch
    {
        public List<BatchContentExt> batchContentList { get; set; }
        public List<BatchProcessExt> batchProcessList { get; set; }
        public string operatorUsername { get; set; }
        //public string operatorResearchGroupName { get; set; }
        public double? availableQuantity { get; set; }
        public string measurementUnitName { get; set; }
        public string measurementUnitSymbol { get; set; }
        public string materialType { get; set; }

        public string editingOperatorUsername { get; set; }
        public string researchGroupName { get; set; }
        public string researchGroupAcronym { get; set; }

        //ovde ke treba atributi od join so content i process -type
        //a ke treba i batch weight od nekoja tabela (stock transaction isto ko za material)

        public BatchExt(Batch e = null)
        {
            if (e != null)
            {
                this.batchId = e.batchId;
                this.batchSystemLabel = e.batchSystemLabel;
                this.batchPersonalLabel = e.batchPersonalLabel;
                this.fkUser = e.fkUser;
                this.description = e.description;
                this.dateCreated = e.dateCreated;
                this.lastChange = e.lastChange;
                this.batchOutput = e.batchOutput;
                this.fkMeasurementUnit = e.fkMeasurementUnit;
                this.chemicalFormula = e.chemicalFormula;
                this.fkMaterialType = e.fkMaterialType;
                this.fkTemplate = e.fkTemplate;
                this.isComplete = e.isComplete;
                this.hasTestResultsDoc = e.hasTestResultsDoc;
                this.fkEditedBy = e.fkEditedBy;
                this.fkProject = e.fkProject;
                this.fkResearchGroup = e.fkResearchGroup;
                this.totalBatchOutput = e.totalBatchOutput;
                this.releasedAs = e.releasedAs;
                this.wasteAmount = e.wasteAmount;
                this.wasteChemicalComposition = e.wasteChemicalComposition;
                this.wasteComment = e.wasteComment;
            }
        }
    }
}