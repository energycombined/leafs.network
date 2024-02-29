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
    public class BatteryComponentCommercialTypeDa
    {
        public static List<BatteryComponentCommercialTypeExt> GetBatteryComponentCommercialTypes(long? batteryComponentCommercialTypeId = null, int? batteryComponentTypeId = null, int? researchGroupId = null)
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
                    FROM battery_component_commercial_type bcct

                    WHERE (bcct.battery_component_commercial_type_id = :bcctid or :bcctid is null) and
                        (bcct.fk_battery_component_type = :typeid or :typeid is null) and
                        (bcct.fk_research_group = :rgid or :rgid is null) ;";

                Db.CreateParameterFunc(cmd, "@bcctid", batteryComponentCommercialTypeId, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@typeid", batteryComponentTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);

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

            List<BatteryComponentCommercialTypeExt> list = (from DataRow dr in dt.Rows select CreateBatteryComponentCommercialTypeObjectExt(dr)).ToList();

            return list;
        }

        public static List<BatteryComponentCommercialTypeExt> GetBatteryComponentCommercialTypesByName(string search = null, int? componentTypeId = null, int? researchGroupId = null)
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
                    FROM battery_component_commercial_type bcct                        
                    WHERE (lower(bcct.battery_component_commercial_type) LIKE lower('%'|| :search ||'%') or :search is null) and
                    (bcct.fk_battery_component_type = :typeid or :typeid is null) and
                    (bcct.fk_research_group = :rgid or :rgid is null)
                    LIMIT 10
                        ;";

                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@typeid", componentTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);

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

            List<BatteryComponentCommercialTypeExt> list = (from DataRow dr in dt.Rows select CreateBatteryComponentCommercialTypeObjectExt(dr)).ToList();

            return list;
        }
        public static int AddBatteryComponentCommercialType(BatteryComponentCommercialType batteryComponentCommercialType)
        {
            int result = 0;
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"INSERT INTO public.battery_component_commercial_type (fk_battery_component_type, battery_component_commercial_type, model, fk_research_group)
                    VALUES (:typeid, :comtype, :model, :rgid) RETURNING battery_component_commercial_type_id;";

                Db.CreateParameterFunc(cmd, "@typeid", batteryComponentCommercialType.fkBatteryComponentType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@comtype", batteryComponentCommercialType.batteryComponentCommercialType, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@model", batteryComponentCommercialType.model, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@rgid", batteryComponentCommercialType.fkResearchGroup, NpgsqlDbType.Integer);

                result = int.Parse(Db.ExecuteScalar(cmd));
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting Battery Component Commercial Type", ex);
            }

            return result;
        }
        public static int UpdateBatteryComponentCommercialType(BatteryComponentCommercialType batteryComponentCommercialType)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.battery_component_commercial_type
                        SET battery_component_commercial_type=:comtype, model=:model, fk_research_group=:rgid
                        WHERE battery_component_commercial_type_id=:bcctid;";

                //Db.CreateParameterFunc(cmd, "@typeid", batteryComponentCommercialType.fkBatteryComponentType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@comtype", batteryComponentCommercialType.batteryComponentCommercialType, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@model", batteryComponentCommercialType.model, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@bcctid", batteryComponentCommercialType.batteryComponentCommercialTypeId, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@rgid", batteryComponentCommercialType.fkResearchGroup, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Battery Component Commercial Type info", ex);
            }
            return 0;
        }
        public static int DeleteBatteryComponentCommercialType(long batteryComponentCommercialTypeId)
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
                            @"SELECT bc.battery_component_id
                            FROM battery_component bc
                            WHERE bc.fk_commercial_type=:bcctid;";

                Db.CreateParameterFunc(cmd, "@bcctid", batteryComponentCommercialTypeId, NpgsqlDbType.Bigint);

                dt = Db.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count > 0)
                {
                    throw new Exception("This Commercial Type Component is related to an experiment");
                }

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.battery_component_commercial_type
                                WHERE battery_component_commercial_type_id=:bcctid;";

                Db.CreateParameterFunc(cmd, "@bcctid", batteryComponentCommercialTypeId, NpgsqlDbType.Bigint);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        public static BatteryComponentCommercialType CreateBatteryComponentCommercialTypeObject(DataRow dr)
        {
            var batteryComponentCommercialType = new BatteryComponentCommercialType
            {
                batteryComponentCommercialTypeId = (long)dr["battery_component_commercial_type_id"],
                fkBatteryComponentType = dr["fk_battery_component_type"] != DBNull.Value ? int.Parse(dr["fk_battery_component_type"].ToString()) : (int?)null,
                batteryComponentCommercialType = dr["battery_component_commercial_type"].ToString(),
                model = dr["model"].ToString(),
                fkResearchGroup = dr["fk_research_group"] != DBNull.Value ? int.Parse(dr["fk_research_group"].ToString()) : (int?)null
            };
            return batteryComponentCommercialType;
        }
        private static BatteryComponentCommercialTypeExt CreateBatteryComponentCommercialTypeObjectExt(DataRow dr)
        {
            var batteryComponentCommercialType = CreateBatteryComponentCommercialTypeObject(dr);

            var batteryComponentCommercialTypeExt = new BatteryComponentCommercialTypeExt(batteryComponentCommercialType)
            {

            };
            return batteryComponentCommercialTypeExt;
        }
    }
}