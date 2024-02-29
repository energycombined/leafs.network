using Batteries.Dal.Base;
using Batteries.Models;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Batteries.Dal
{
    public class BatteryComponentTypeDa
    {
        public static string GetAllBatteryComponentTypesJsonForDropdown(int? batteryComponentTypeId = null)
        {
            string json = "";
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                cmd.CommandText =
                    @"SELECT json_agg(row_to_json(t))
                      FROM ( select battery_component_type_id as value, COALESCE(bct.battery_component_type, '') as text
                              from battery_component_type bct
                              where (bct.battery_component_type_id = :bctid or :bctid is null)
                          ) as t;";

                Db.CreateParameterFunc(cmd, "@bct", batteryComponentTypeId, NpgsqlDbType.Integer);

                json = Db.ExecuteScalar(cmd);

                if (json == null || json == "")
                {
                    json = "[]";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return json;
        }
        public static List<BatteryComponentType> GetAllBatteryComponentTypes(int? batteryComponentTypeId = null)
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
                              FROM battery_component_type bct
                              WHERE (bct.battery_component_type_id = :bctid or :bctid is null);";

                Db.CreateParameterFunc(cmd, "@bct", batteryComponentTypeId, NpgsqlDbType.Integer);

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

            List<BatteryComponentType> list = (from DataRow dr in dt.Rows select CreateBatteryComponentTypeObject(dr)).ToList();

            return list;
        }
        public static BatteryComponentType CreateBatteryComponentTypeObject(DataRow dr)
        {
            var batteryComponentType = new BatteryComponentType
            {
                batteryComponentTypeId = (int)dr["battery_component_type_id"],
                batteryComponentType = dr["battery_component_type"].ToString(),
            };
            return batteryComponentType;
        }
    }
}