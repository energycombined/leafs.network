using Batteries.Dal.Base;
using Batteries.Models;
using Batteries.Models.ProcessModels;
using Batteries.Models.Responses;
using Batteries.Models.Responses.ProcessModels;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Batteries.Dal.ProcessesDal
{
    public class PhaseInversionDa
    {
        public static List<PhaseInversionExt> GetAllPhaseInversions(long? phaseInversionId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM phase_inversion pi
                        left join experiment_process ep on pi.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on pi.fk_batch_process = bp.batch_process_id
                        left join equipment eq on pi.fk_equipment = eq.equipment_id

                    WHERE (pi.phase_inversion_id = :piid or :piid is null) and
                        (pi.fk_experiment_process = :epid or :epid is null) and
                        (pi.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@piid", phaseInversionId, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@epid", experimentProcessId, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", batchProcessId, NpgsqlDbType.Bigint);

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

            List<PhaseInversionExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<PhaseInversionExt> GetRecentlyUsedPhaseInversions(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(phase_inversion_id) as phase_inversion_id, max(date_created) as date_created, coagulation_bath, additives, temperature, time, stirring, stirring_speed,
comments,
label
                      FROM phase_inversion
                      GROUP BY coagulation_bath, additives, temperature, time, stirring, stirring_speed,
comments,
label
                      ORDER BY max(phase_inversion_id) DESC LIMIT 10;";

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

            List<PhaseInversionExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddPhaseInversion(PhaseInversion phaseInversion, NpgsqlCommand cmd)
        {
            try
            {
                if (cmd != null)
                {
                    cmd.Parameters.Clear();
                }
                else
                {
                    cmd = Db.CreateCommand();

                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                }

                cmd.CommandText =
                    @"INSERT INTO public.phase_inversion (
fk_experiment_process,
fk_batch_process,
fk_equipment,
coagulation_bath,
additives,
temperature,
time,
stirring,
stirring_speed,
date_created,
comments,
label
)
                    VALUES (:epid, :bpid, :eid, :cb, :a, :temp, :time, :s, :ss, now()::timestamp,
:comments,
:label
);";

                Db.CreateParameterFunc(cmd, "@epid", phaseInversion.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", phaseInversion.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", phaseInversion.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@cb", phaseInversion.coagulationBath, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@a", phaseInversion.additives, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@temp", phaseInversion.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@time", phaseInversion.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@s", phaseInversion.stirring, NpgsqlDbType.Boolean);
                Db.CreateParameterFunc(cmd, "@ss", phaseInversion.stirringSpeed, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comments", phaseInversion.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", phaseInversion.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting phase inversion", ex);
            }

            return 0;
        }
        public static int UpdatePhaseInversion(PhaseInversion phaseInversion)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.phase_inversion
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid, 
coagulation_bath=:cb, 
additives=:a, 
temperature=:temp, 
time=:time, 
stirring=:s, 
stirring_speed=:ss,
date_created=now()::timestamp,
comments=:comments,
label=:label

                        WHERE phase_inversion_id=:piid;";
                Db.CreateParameterFunc(cmd, "@epid", phaseInversion.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", phaseInversion.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", phaseInversion.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@cb", phaseInversion.coagulationBath, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@a", phaseInversion.additives, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@temp", phaseInversion.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@time", phaseInversion.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@s", phaseInversion.stirring, NpgsqlDbType.Boolean);
                Db.CreateParameterFunc(cmd, "@ss", phaseInversion.stirringSpeed, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comments", phaseInversion.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", phaseInversion.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@piid", phaseInversion.phaseInversionId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating phase inversion info", ex);
            }
            return 0;
        }

        public static PhaseInversion CreateObject(DataRow dr)
        {
            long? fkExperimentProcessVar = (long?)null;
            if (dr.Table.Columns.Contains("fk_experiment_process"))
            {
                fkExperimentProcessVar = dr["fk_experiment_process"] != DBNull.Value ? long.Parse(dr["fk_experiment_process"].ToString()) : (long?)null;
            }
            long? fkBatchProcessVar = (long?)null;
            if (dr.Table.Columns.Contains("fk_batch_process"))
            {
                fkBatchProcessVar = dr["fk_batch_process"] != DBNull.Value ? long.Parse(dr["fk_batch_process"].ToString()) : (long?)null;
            }
            int? fkEquipmentVar = (int?)null;
            if (dr.Table.Columns.Contains("fk_equipment"))
            {
                fkEquipmentVar = dr["fk_equipment"] != DBNull.Value ? int.Parse(dr["fk_equipment"].ToString()) : (int?)null;
            }
            var phaseInversion = new PhaseInversion
            {
                phaseInversionId = (long)dr["phase_inversion_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                coagulationBath = dr["coagulation_bath"].ToString(),
                additives = dr["additives"].ToString(),
                temperature = dr["temperature"] != DBNull.Value ? double.Parse(dr["temperature"].ToString()) : (double?)null,
                time = dr["time"] != DBNull.Value ? double.Parse(dr["time"].ToString()) : (double?)null,
                stirring = dr["stirring"] != DBNull.Value ? Boolean.Parse(dr["stirring"].ToString()) : (Boolean?)null,
                stirringSpeed = dr["stirring_speed"] != DBNull.Value ? double.Parse(dr["stirring_speed"].ToString()) : (double?)null,
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null
            };
            return phaseInversion;
        }
        private static PhaseInversionExt CreateObjectExt(DataRow dr)
        {
            var phaseInversion = CreateObject(dr);

            var phaseInversionExt = new PhaseInversionExt(phaseInversion)
            {
                //public string equipmentName { get; set; }
            };
            return phaseInversionExt;
        }
    }
}