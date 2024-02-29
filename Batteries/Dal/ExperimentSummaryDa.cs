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
    public class ExperimentSummaryDa
    {

        public static List<ExperimentSummaryExt> GetExperimentSummary(int? experimentId = null)
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
                              FROM experiment_summary es

                              LEFT JOIN battery_component_commercial_type ct ON es.fk_commercial_type = ct.battery_component_commercial_type_id
                              WHERE 
                                    (es.fk_experiment = :eid or :eid is null);";

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

            List<ExperimentSummaryExt> list = (from DataRow dr in dt.Rows select CreateExperimentSummaryObjectExt(dr)).ToList();

            return list;
        }


        public static ExperimentSummary CreateExperimentSummaryObject(DataRow dr)
        {
            var experimentSummary = new ExperimentSummary
            {
                experimentSummaryId = long.Parse(dr["experiment_summary_id"].ToString()),
                fkExperiment = dr["fk_experiment"] != DBNull.Value ? int.Parse(dr["fk_experiment"].ToString()) : (int?)null,
                componentEmpty = dr["component_empty"] != DBNull.Value ? Boolean.Parse(dr["component_empty"].ToString()) : (Boolean?)null,
                totalWeight = dr["total_weight"] != DBNull.Value ? double.Parse(dr["total_weight"].ToString()) : (double?)null,
                totalLabeledMaterials = dr["total_labeled_materials"] != DBNull.Value ? double.Parse(dr["total_labeled_materials"].ToString()) : (double?)null,
                labeledMaterials = dr["labeled_materials"].ToString(),
                labeledPercentages = dr["labeled_percentages"].ToString(),
                totalActiveMaterials = dr["total_active_materials"] != DBNull.Value ? double.Parse(dr["total_active_materials"].ToString()) : (double?)null,
                totalActiveMaterialsPercentage = dr["total_active_materials_percentage"] != DBNull.Value ? double.Parse(dr["total_active_materials_percentage"].ToString()) : (double?)null,
                activeMaterials = dr["active_materials"].ToString(),
                activePercentages = dr["active_percentages"].ToString(),
                fkBatteryComponentType = dr["fk_battery_component_type"] != DBNull.Value ? int.Parse(dr["fk_battery_component_type"].ToString()) : (int?)null,
                fkCommercialType = dr["fk_commercial_type"] != DBNull.Value ? long.Parse(dr["fk_commercial_type"].ToString()) : (long?)null,
                //mass1 = dr["mass1"] != DBNull.Value ? double.Parse(dr["mass1"].ToString()) : (double?)null,
                //mass2 = dr["mass2"] != DBNull.Value ? double.Parse(dr["mass2"].ToString()) : (double?)null,
                //mass3 = dr["mass3"] != DBNull.Value ? double.Parse(dr["mass3"].ToString()) : (double?)null,
                //mass4 = dr["mass4"] != DBNull.Value ? double.Parse(dr["mass4"].ToString()) : (double?)null,
                //mass5 = dr["mass5"] != DBNull.Value ? double.Parse(dr["mass5"].ToString()) : (double?)null,
                //mass6 = dr["mass6"] != DBNull.Value ? double.Parse(dr["mass6"].ToString()) : (double?)null,
            };
            return experimentSummary;
        }

        public static ExperimentSummaryExt CreateExperimentSummaryObjectExt(DataRow dr)
        {
            var experimentSummaryObject = CreateExperimentSummaryObject(dr);

            var experimentSummaryExt = new ExperimentSummaryExt(experimentSummaryObject)
            {
                commercialTypeName = dr["battery_component_commercial_type"].ToString()
            };

            return experimentSummaryExt;

        }
    }
}