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
    public class TestEquipmentModelDa
    {
        public static List<TestEquipmentModel> GetTestEquipmentModelsPaged(string search = null, int? page = 1)
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
                    FROM test_equipment_models
                    WHERE ((lower(test_equipment_model_name) LIKE lower('%'|| :search ||'%') or lower(brand_name) LIKE lower('%'|| :search ||'%')) or :search is null)                       
                        LIMIT 10 OFFSET :offset;";

                //Db.CreateParameterFunc(cmd, "@ttid", testTypeId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@offset", (page - 1) * 10, NpgsqlDbType.Integer);

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

            List<TestEquipmentModel> list = (from DataRow dr in dt.Rows select CreateObject(dr)).ToList();

            return list;
        }
        public static List<TestEquipmentModel> GetTestEquipmentModels()
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
                    FROM test_equipment_models;";

                //Db.CreateParameterFunc(cmd, "@ttid", testTypeId, NpgsqlDbType.Integer);

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

            List<TestEquipmentModel> list = (from DataRow dr in dt.Rows select CreateObject(dr)).ToList();

            return list;
        }
        public static TestEquipmentModel GetTestEquipmentModelById(int modelId)
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
                    FROM test_equipment_models
                    WHERE test_equipment_model_id = :mid
                    ;";

                Db.CreateParameterFunc(cmd, "@mid", modelId, NpgsqlDbType.Integer);

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

            TestEquipmentModel item = CreateObject(dt.Rows[0]);

            return item;
        }
        public static TestEquipmentModel CreateObject(DataRow dr)
        {
            var testEquipmentModel = new TestEquipmentModel
            {
                testEquipmentModelId = (int)dr["test_equipment_model_id"],
                testEquipmentModelName = dr["test_equipment_model_name"].ToString(),
                brandName = dr["brand_name"].ToString(),
            };
            return testEquipmentModel;
        }
    }
}