using Batteries.Models.ProcessModels;
using Batteries.Models.Responses.ProcessModels;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Batteries.Dal.Base;
using Npgsql;
using NpgsqlTypes;

namespace Batteries.Dal.ProcessesDal
{
    public class FormationDa
    {
        public static List<FormationExt> GetAllFormations(long? formationId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM formation c
                        left join experiment_process ep on c.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on c.fk_batch_process = bp.batch_process_id
                        left join equipment eq on c.fk_equipment = eq.equipment_id

                    WHERE (c.formation_id = :cid or :cid is null) and
                        (c.fk_experiment_process = :epid or :epid is null) and
                        (c.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@cid", formationId, NpgsqlDbType.Bigint);
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

            List<FormationExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<FormationExt> GetRecentlyUsedFormations(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(formation_id) as formation_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
current,
voltage,
number_of_cycles,
charge_capacity,
dod,
time,
comments,
label
                      FROM formation
                          LEFT JOIN equipment e on formation.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name,
current,
voltage,
number_of_cycles,
charge_capacity,
dod,
time,
comments,
label
                      ORDER BY max(formation_id) DESC LIMIT 10;";

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

            List<FormationExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddFormation(Formation formation, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.formation (
fk_experiment_process, fk_batch_process, fk_equipment, date_created,
current,
voltage,
number_of_cycles,
charge_capacity,
dod,
time,
comments,
label
)
                    VALUES (:epid, :bpid, :eid, now()::timestamp,
:current,
:voltage,
:number_of_cycles,
:charge_capacity,
:dod,
:time,
:comments,
:label
);";

                Db.CreateParameterFunc(cmd, "@epid", formation.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", formation.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", formation.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@current", formation.current, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@voltage", formation.voltage, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@number_of_cycles", formation.numberOfCycles, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@charge_capacity", formation.chargeCapacity, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@dod", formation.dod, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@time", formation.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comments", formation.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", formation.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting process", ex);
            }

            return 0;
        }
        public static int UpdateFormation(Formation formation)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.formation
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
current=:current,
voltage=:voltage,
number_of_cycles=:number_of_cycles,
charge_capacity=:charge_capacity,
dod=:dod,
time=:time,
comments=:comments,
label=:label
                        WHERE formation_id=:cid;";
                Db.CreateParameterFunc(cmd, "@epid", formation.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", formation.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", formation.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@current", formation.current, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@voltage", formation.voltage, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@number_of_cycles", formation.numberOfCycles, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@charge_capacity", formation.chargeCapacity, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@dod", formation.dod, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@time", formation.time, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@comments", formation.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@label", formation.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@cid", formation.formationId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating process info", ex);
            }
            return 0;
        }
        public static Formation CreateObject(DataRow dr)
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

            var formation = new Formation
            {
                formationId = (long)dr["formation_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                current = dr["current"] != DBNull.Value ? double.Parse(dr["current"].ToString()) : (double?)null,
                voltage = dr["voltage"] != DBNull.Value ? double.Parse(dr["voltage"].ToString()) : (double?)null,
                numberOfCycles = dr["number_of_cycles"] != DBNull.Value ? int.Parse(dr["number_of_cycles"].ToString()) : (int?)null,
                chargeCapacity = dr["charge_capacity"] != DBNull.Value ? double.Parse(dr["charge_capacity"].ToString()) : (double?)null,
                dod = dr["dod"] != DBNull.Value ? double.Parse(dr["dod"].ToString()) : (double?)null,
                time = dr["time"] != DBNull.Value ? double.Parse(dr["time"].ToString()) : (double?)null,
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,

            };
            return formation;
        }
        private static FormationExt CreateObjectExt(DataRow dr)
        {
            var formation = CreateObject(dr);

            var formationExt = new FormationExt(formation)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return formationExt;
        }
    }
}