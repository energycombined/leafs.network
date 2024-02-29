using Batteries.Models.ProcessModels;
using Batteries.Models.Responses.ProcessModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Batteries.Dal.Base;
using Npgsql;
using NpgsqlTypes;

namespace Batteries.Dal.ProcessesDal
{
    public class AtomicLayerDepositionDa
    {
        public static List<AtomicLayerDepositionExt> GetAllAtomicLayerDepositions(long? atomicLayerDepositionId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    FROM atomic_layer_deposition c
                        left join experiment_process ep on c.fk_experiment_process = ep.experiment_process_id
                        left join batch_process bp on c.fk_batch_process = bp.batch_process_id
                        left join equipment eq on c.fk_equipment = eq.equipment_id

                    WHERE (c.atomic_layer_deposition_id = :cid or :cid is null) and
                        (c.fk_experiment_process = :epid or :epid is null) and
                        (c.fk_batch_process = :bpid or :bpid is null);";

                Db.CreateParameterFunc(cmd, "@cid", atomicLayerDepositionId, NpgsqlDbType.Bigint);
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

            List<AtomicLayerDepositionExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<AtomicLayerDepositionExt> GetRecentlyUsedAtomicLayerDepositions(int? researchGroupId = null, long? experimentProcessId = null, long? batchProcessId = null)
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
                    @"SELECT max(atomic_layer_deposition_id) as atomic_layer_deposition_id, max(date_created) as date_created, fk_equipment, e.equipment_name,
thickness,
temperature,
pressure,
gas,
comments,
label
                      FROM atomic_layer_deposition
                          LEFT JOIN equipment e on atomic_layer_deposition.fk_equipment = e.equipment_id
                      GROUP BY fk_equipment, e.equipment_name,
thickness,
temperature,
pressure,
gas,
comments,
label
                      ORDER BY max(atomic_layer_deposition_id) DESC LIMIT 10;";

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

            List<AtomicLayerDepositionExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddAtomicLayerDeposition(AtomicLayerDeposition atomicLayerDeposition, NpgsqlCommand cmd)
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
                    @"INSERT INTO public.atomic_layer_deposition (
fk_experiment_process, fk_batch_process, fk_equipment, date_created,
thickness,
temperature,
pressure,
gas,
comments,
label
)
                    VALUES (:epid, :bpid, :eid, now()::timestamp,
:th,
:temp,
:pr,
:gas,
:com,
:lab
);";

                Db.CreateParameterFunc(cmd, "@epid", atomicLayerDeposition.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", atomicLayerDeposition.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", atomicLayerDeposition.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@th", atomicLayerDeposition.thickness, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@temp", atomicLayerDeposition.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@pr", atomicLayerDeposition.pressure, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@gas", atomicLayerDeposition.gas, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@com", atomicLayerDeposition.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", atomicLayerDeposition.label, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting process", ex);
            }

            return 0;
        }
        public static int UpdateAtomicLayerDeposition(AtomicLayerDeposition atomicLayerDeposition)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.atomic_layer_deposition
                        SET 

fk_experiment_process=:epid,
fk_batch_process=:bpid, 
fk_equipment=:eid,
date_created=now()::timestamp,
thickness=:th
temperature=:temp
pressure=:pr
gas=:gas
comments=:com
label=:lab
                        WHERE atomic_layer_deposition_id=:cid;";
                Db.CreateParameterFunc(cmd, "@epid", atomicLayerDeposition.fkExperimentProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@bpid", atomicLayerDeposition.fkBatchProcess, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@eid", atomicLayerDeposition.fkEquipment, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@th", atomicLayerDeposition.thickness, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@temp", atomicLayerDeposition.temperature, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@pr", atomicLayerDeposition.pressure, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@gas", atomicLayerDeposition.gas, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@com", atomicLayerDeposition.comments, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lab", atomicLayerDeposition.label, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@cid", atomicLayerDeposition.atomicLayerDepositionId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating process info", ex);
            }
            return 0;
        }
        public static AtomicLayerDeposition CreateObject(DataRow dr)
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

            var atomicLayerDeposition = new AtomicLayerDeposition
            {
                atomicLayerDepositionId = (long)dr["atomic_layer_deposition_id"],
                fkExperimentProcess = fkExperimentProcessVar,
                fkBatchProcess = fkBatchProcessVar,
                fkEquipment = fkEquipmentVar,
                thickness = dr["thickness"] != DBNull.Value ? double.Parse(dr["thickness"].ToString()) : (double?)null,
                temperature = dr["temperature"] != DBNull.Value ? double.Parse(dr["temperature"].ToString()) : (double?)null,
                pressure = dr["pressure"] != DBNull.Value ? double.Parse(dr["pressure"].ToString()) : (double?)null,
                gas = dr["gas"].ToString(),
                comments = dr["comments"].ToString(),
                label = dr["label"].ToString(),
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,

            };
            return atomicLayerDeposition;
        }
        private static AtomicLayerDepositionExt CreateObjectExt(DataRow dr)
        {
            var atomicLayerDeposition = CreateObject(dr);

            var atomicLayerDepositionExt = new AtomicLayerDepositionExt(atomicLayerDeposition)
            {
                equipmentName = dr["equipment_name"].ToString()
            };
            return atomicLayerDepositionExt;
        }
    }
}