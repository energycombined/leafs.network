using Batteries.Dal.Base;
using Batteries.Models;
using Batteries.Models.Responses;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Batteries.Dal
{
    public class StockTransactionDa
    {
        public static List<StockTransactionExt> GetAllMaterialStockTransactions(int? stockTransactionId = null, int? materialId = null, int? researchGroupId = null, short? transactionDirection = null)
        {
            DataTable dt;
            //stock_transaction_element_type 1-material, 2-batch
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"SELECT *
                    FROM stock_transaction st
                        left join material m on st.fk_material = m.material_id
                        left join material_type mt on m.fk_material_type = mt.material_type_id
                        left join measurement_unit mu on m.fk_measurement_unit = mu.measurement_unit_id
                        left join vendor v on st.fk_vendor = v.vendor_id
                        left join users u on st.fk_operator = u.user_id
                        left join research_group rg on st.fk_research_group = rg.research_group_id
                        left join experiment e on st.fk_experiment_coming = e.experiment_id

                    WHERE 
                        (st.stock_transaction_element_type = 1) and
                        (st.stock_transaction_id = :stid or :stid is null) and
                        (st.fk_material = :mid or :mid is null) and
                        (st.fk_research_group = :rgid or :rgid is null) and
                        (st.transaction_direction = :td or :td is null)
                    ORDER BY st.stock_transaction_id DESC
                    ;";

                Db.CreateParameterFunc(cmd, "@stid", stockTransactionId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@mid", materialId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@td", transactionDirection, NpgsqlDbType.Smallint);

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

            List<StockTransactionExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<StockTransactionExt> GetAllBatchStockTransactions(int? stockTransactionId = null, int? batchId = null, int? researchGroupId = null, short? transactionDirection = null)
        {
            DataTable dt;

            //left join batch bat on st.fk_batch_coming = bat.batch_id
            //if needed will have to separate attributes' names

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"SELECT *
                    FROM stock_transaction st
                        left join batch b on st.fk_batch = b.batch_id
                        left join material_type mt on b.fk_material_type = mt.material_type_id
                        left join measurement_unit mu on b.fk_measurement_unit = mu.measurement_unit_id                        
                        left join users u on st.fk_operator = u.user_id
                        left join research_group rg on st.fk_research_group = rg.research_group_id
                        left join experiment e on st.fk_experiment_coming = e.experiment_id
                        
                    WHERE 
                        (st.stock_transaction_element_type = 2) and
                        (st.stock_transaction_id = :stid or :stid is null) and
                        (st.fk_batch = :bid or :bid is null) and
                        (st.fk_research_group = :rgid or :rgid is null) and
                        (st.transaction_direction = :td or :td is null)
                    ORDER BY st.stock_transaction_id DESC
                    ;";

                Db.CreateParameterFunc(cmd, "@stid", stockTransactionId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@td", transactionDirection, NpgsqlDbType.Smallint);

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

            List<StockTransactionExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static int AddMaterialStockTransaction(StockTransaction stockTransaction)
        {
            //stock_transaction_element_type = 1
            try
            {
                if (stockTransaction.dateBought > DateTime.Today.Date)
                {
                    throw new Exception("Date bought cannot be later that today\\'s date.");
                }

                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                if (stockTransaction.transactionDirection == -1)
                {
                    cmd.CommandText =
                            @"SELECT SUM(stock_transaction.amount * stock_transaction.transaction_direction)
                            FROM stock_transaction
                            WHERE stock_transaction.fk_research_group = :rgid AND
                                   stock_transaction.fk_material = :mid;";

                    Db.CreateParameterFunc(cmd, "@rgid", stockTransaction.fkResearchGroup, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@mid", stockTransaction.fkMaterial, NpgsqlDbType.Integer);

                    var res = Db.ExecuteScalar(cmd);

                    double availableQuantity = res != null ? double.Parse(res) : 0;
                    if (availableQuantity < stockTransaction.amount)
                    {
                        throw new Exception("Amount not valid. There is less available in stock!");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            int result = 0;
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                cmd.CommandText =
                    @"INSERT INTO public.stock_transaction (
fk_material,
fk_batch,
stock_transaction_element_type,
amount,
fk_vendor,
fk_operator,
fk_research_group,
fk_experiment_coming,
fk_batch_coming,
transaction_direction,
fk_battery_component_type,
date_bought,
date_created

)
                    VALUES (
:mid,
:bid,
1,
:a,
:vid,
:oid,
:rgid,
:ecomid,
:bcomid,
:td,
:bctid,
:dateb,
now()::timestamp)
RETURNING stock_transaction_id
;";

                Db.CreateParameterFunc(cmd, "@mid", stockTransaction.fkMaterial, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bid", stockTransaction.fkBatch, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@stet", stockTransaction.stockTransactionElementType, NpgsqlDbType.Smallint);
                Db.CreateParameterFunc(cmd, "@a", stockTransaction.amount, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@vid", stockTransaction.fkVendor, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@oid", stockTransaction.fkOperator, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", stockTransaction.fkResearchGroup, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@ecomid", stockTransaction.fkExperimentComing, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bcomid", stockTransaction.fkBatchComing, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@td", stockTransaction.transactionDirection, NpgsqlDbType.Smallint);
                Db.CreateParameterFunc(cmd, "@bctid", stockTransaction.fkBatteryComponentType, NpgsqlDbType.Smallint);
                Db.CreateParameterFunc(cmd, "@dateb", stockTransaction.dateBought, NpgsqlDbType.Date);

                result = int.Parse(Db.ExecuteScalar(cmd));
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting material stock information", ex);
            }

            return result;
        }

        public static int AddBatchStockTransaction(StockTransaction stockTransaction)
        {
            //stock_transaction_element_type = 2
            try
            {
                //if (stockTransaction.dateBought > DateTime.Today.Date)
                //{
                //    throw new Exception("Date bought cannot be later that today\\'s date.");
                //}

                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                if (stockTransaction.transactionDirection == -1)
                {
                    cmd.CommandText =
                            @"SELECT SUM(stock_transaction.amount * stock_transaction.transaction_direction)
                            FROM stock_transaction
                            WHERE stock_transaction.fk_research_group = :rgid AND
                                   stock_transaction.fk_batch = :bid;";

                    Db.CreateParameterFunc(cmd, "@rgid", stockTransaction.fkResearchGroup, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@bid", stockTransaction.fkBatch, NpgsqlDbType.Integer);

                    var res = Db.ExecuteScalar(cmd);

                    double availableQuantity = res != null ? double.Parse(res) : 0;
                    if (availableQuantity < stockTransaction.amount)
                    {
                        throw new Exception("Amount not valid. There is less available in stock!");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                cmd.CommandText =
                    @"INSERT INTO public.stock_transaction (
fk_material,
fk_batch,
stock_transaction_element_type,
amount,
fk_vendor,
fk_operator,
fk_research_group,
fk_experiment_coming,
fk_batch_coming,
transaction_direction,
fk_battery_component_type,
date_bought,
date_created

)
                    VALUES (
:mid,
:bid,
2,
:a,
:vid,
:oid,
:rgid,
:ecomid,
:bcomid,
:td,
:bctid,
:dateb,
now()::timestamp);";

                Db.CreateParameterFunc(cmd, "@mid", stockTransaction.fkMaterial, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bid", stockTransaction.fkBatch, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@stet", stockTransaction.stockTransactionElementType, NpgsqlDbType.Smallint);
                Db.CreateParameterFunc(cmd, "@a", stockTransaction.amount, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@vid", stockTransaction.fkVendor, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@oid", stockTransaction.fkOperator, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", stockTransaction.fkResearchGroup, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@ecomid", stockTransaction.fkExperimentComing, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bcomid", stockTransaction.fkBatchComing, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@td", stockTransaction.transactionDirection, NpgsqlDbType.Smallint);
                Db.CreateParameterFunc(cmd, "@bctid", stockTransaction.fkBatteryComponentType, NpgsqlDbType.Smallint);
                Db.CreateParameterFunc(cmd, "@dateb", stockTransaction.dateBought, NpgsqlDbType.Date);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting batch stock information", ex);
            }

            return 0;
        }
        public static int EmptyBatchStockTransaction(int batchId, int userId, int fkResearchGroup)
        {
            double availableQuantity = 0;
            var transactionDirection = -1;
            //stock_transaction_element_type = 2
            try
            {
                
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                
                    cmd.CommandText =
                            @"SELECT SUM(stock_transaction.amount * stock_transaction.transaction_direction)
                            FROM stock_transaction
                            WHERE stock_transaction.fk_research_group = :rgid AND
                                   stock_transaction.fk_batch = :bid;";

                    Db.CreateParameterFunc(cmd, "@rgid", fkResearchGroup, NpgsqlDbType.Integer);
                    Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);

                    var res = Db.ExecuteScalar(cmd);

                    availableQuantity = res != null ? double.Parse(res) : 0;
                    if (availableQuantity < 0)
                    {
                        throw new Exception("Amount not valid. There is less available in stock!");
                    }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                cmd.CommandText =
                    @"INSERT INTO public.stock_transaction (
fk_batch,
stock_transaction_element_type,
amount,
fk_operator,
fk_research_group,
transaction_direction,
fk_batch_coming,
date_created,
manual_empty

)
                    VALUES (
:bid,
2,
:a,
:uid,
:rgid,
:td,
:cbid,
now()::timestamp,
:empty);";

                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@a", availableQuantity, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@uid", userId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", fkResearchGroup, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@cbid", batchId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@td", transactionDirection, NpgsqlDbType.Smallint);
                Db.CreateParameterFunc(cmd, "@empty", true, NpgsqlDbType.Boolean);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting batch stock information", ex);
            }

            return 0;
        }
        public static bool CheckMaterialStockQuantityEnough(int materialId, double wantedQuantity, int researchGroupId, NpgsqlCommand cmd)
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
                        @"SELECT SUM(stock_transaction.amount * stock_transaction.transaction_direction)
                            FROM stock_transaction
                            WHERE stock_transaction.fk_research_group = :rgid AND
                                   stock_transaction.fk_material = :mid;";

                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@mid", materialId, NpgsqlDbType.Integer);

                var res = Db.ExecuteScalar(cmd, false);

                double availableQuantity = 0;
                if (res != null && res != "")
                {
                    availableQuantity = double.Parse(res);
                }
                if (availableQuantity < wantedQuantity)
                {
                    //throw new Exception("Amount not valid. There is less available in stock!");
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public static bool CheckBatchStockQuantityEnough(int batchId, double wantedQuantity, int researchGroupId, NpgsqlCommand cmd)
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
                        @"SELECT SUM(stock_transaction.amount * stock_transaction.transaction_direction)
                            FROM stock_transaction
                            WHERE stock_transaction.fk_research_group = :rgid AND
                                   stock_transaction.fk_batch = :bid;";

                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bid", batchId, NpgsqlDbType.Integer);

                var res = Db.ExecuteScalar(cmd, false);

                //double availableQuantity = res != null ? double.Parse(res) : 0;
                double availableQuantity = 0;
                if (res != null && res != "")
                {
                    availableQuantity = double.Parse(res);
                }
                if (availableQuantity < wantedQuantity)
                {
                    //throw new Exception("Amount not valid. There is less available in stock!");
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public static int UpdateStockTransaction(StockTransaction stockTransaction)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.stock_transaction
                        SET 
fk_material=:mid,
fk_batch=:bid,
stock_transaction_element_type=:stet,
amount=:a,
fk_vendor=:vid,
fk_operator=:oid,
fk_research_group=:rgid,
fk_experiment_coming=:ecomid,
fk_batch_coming=:bcomid,
transaction_direction=:td,
fk_battery_component_type=:bctid,
date_bought=:dateb,
                        WHERE stock_transaction_id=:stid;";

                Db.CreateParameterFunc(cmd, "@mid", stockTransaction.fkMaterial, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bid", stockTransaction.fkBatch, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@stet", stockTransaction.stockTransactionElementType, NpgsqlDbType.Smallint);
                Db.CreateParameterFunc(cmd, "@a", stockTransaction.amount, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@vid", stockTransaction.fkVendor, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@oid", stockTransaction.fkOperator, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", stockTransaction.fkResearchGroup, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@ecomid", stockTransaction.fkExperimentComing, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@bcomid", stockTransaction.fkBatchComing, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@td", stockTransaction.transactionDirection, NpgsqlDbType.Smallint);
                Db.CreateParameterFunc(cmd, "@bctid", stockTransaction.fkBatteryComponentType, NpgsqlDbType.Smallint);
                Db.CreateParameterFunc(cmd, "@dateb", stockTransaction.dateBought, NpgsqlDbType.Date);

                Db.CreateParameterFunc(cmd, "@stid", stockTransaction.stockTransactionId, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating stock transaction info", ex);
            }
            return 0;
        }
        public static int DeleteStockTransaction(int stockTransactionId)
        {
            //DataTable dt;
            try
            {
                var cmd = Db.CreateCommand();
                //                if (cmd.Connection.State != ConnectionState.Open)
                //                {
                //                    cmd.Connection.Open();
                //                }
                //                cmd.CommandText =
                //                            @"SELECT st.stock_transaction_id 
                //                            FROM stock_transaction st
                //                            WHERE st.fk_stockTransaction=:stid;";

                //                Db.CreateParameterFunc(cmd, "@stid", stockTransactionId, NpgsqlDbType.Integer);

                //                dt = Db.ExecuteSelectCommand(cmd);

                //                if (dt.Rows.Count > 0)
                //                {
                //                    throw new Exception("This stockTransaction is related to stock data");
                //                }

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.stock_transaction
                                WHERE stock_transaction_id=:stid;";

                Db.CreateParameterFunc(cmd, "@stid", stockTransactionId, NpgsqlDbType.Integer);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }

        public static StockTransaction CreateObject(DataRow dr)
        {
            var stockTransaction = new StockTransaction
            {
                stockTransactionId = (long)dr["stock_transaction_id"],
                fkMaterial = dr["fk_material"] != DBNull.Value ? long.Parse(dr["fk_material"].ToString()) : (long?)null,
                fkBatch = dr["fk_batch"] != DBNull.Value ? int.Parse(dr["fk_batch"].ToString()) : (int?)null,

                stockTransactionElementType = dr["stock_transaction_element_type"] != DBNull.Value ? short.Parse(dr["stock_transaction_element_type"].ToString()) : (short?)null,

                amount = dr["amount"] != DBNull.Value ? double.Parse(dr["amount"].ToString()) : (double?)null,
                fkVendor = dr["fk_vendor"] != DBNull.Value ? int.Parse(dr["fk_vendor"].ToString()) : (int?)null,
                fkOperator = dr["fk_operator"] != DBNull.Value ? int.Parse(dr["fk_operator"].ToString()) : (int?)null,
                fkResearchGroup = dr["fk_research_group"] != DBNull.Value ? int.Parse(dr["fk_research_group"].ToString()) : (int?)null,

                fkExperimentComing = dr["fk_experiment_coming"] != DBNull.Value ? int.Parse(dr["fk_experiment_coming"].ToString()) : (int?)null,
                fkBatchComing = dr["fk_batch_coming"] != DBNull.Value ? int.Parse(dr["fk_batch_coming"].ToString()) : (int?)null,
                
                transactionDirection = dr["transaction_direction"] != DBNull.Value ? short.Parse(dr["transaction_direction"].ToString()) : (short?)null,
                fkBatteryComponentType = dr["fk_battery_component_type"] != DBNull.Value ? int.Parse(dr["fk_battery_component_type"].ToString()) : (int?)null,
                dateBought = dr["date_bought"] != DBNull.Value ? DateTime.Parse(dr["date_bought"].ToString()) : (DateTime?)null,
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,
            };
            return stockTransaction;
        }
        private static StockTransactionExt CreateObjectExt(DataRow dr)
        {
            var StockTransaction = CreateObject(dr);

            string materialNameVar = null;
            //string materialTypeVar = null;
            string materialChemicalFormulaVar = null;
            string vendorNameVar = null;

            string batchSystemLabelVar = null;
            string batchPersonalLabelVar = null;

            string batchChemicalFormulaVar = null;
            if (dr["fk_material"] != DBNull.Value)
            {
                materialNameVar = dr["material_name"].ToString();
                //materialTypeVar = dr["material_type"].ToString();
                materialChemicalFormulaVar = dr["chemical_formula"].ToString();
                vendorNameVar = dr["vendor_name"].ToString();
            }
            else if (dr["fk_batch"] != DBNull.Value)
            {
                //batchSystemLabelVar = "Batch_" + dr["fk_batch"].ToString();
                batchSystemLabelVar = dr["batch_system_label"].ToString();
                batchPersonalLabelVar = dr["batch_personal_label"].ToString();
                batchChemicalFormulaVar = dr["chemical_formula"].ToString();
            }


            //double? availableQuantityVar = (double?)null;
            //if (dr.Table.Columns.Contains("available_quantity"))
            //{
            //    availableQuantityVar = dr["available_quantity"] != DBNull.Value ? double.Parse(dr["available_quantity"].ToString()) : (double?)null;
            //}
            //string operatorUsernameVar = dr.Table.Columns.Contains("operator_username") ? dr["operator_username"].ToString() : null;

            var StockTransactionExt = new StockTransactionExt(StockTransaction)
            {
                materialName = materialNameVar,
                materialType = dr["material_type"].ToString(),
                materialChemicalFormula = materialChemicalFormulaVar,
                vendorName = vendorNameVar,

                batchSystemLabel = batchSystemLabelVar,
                batchPersonalLabel = batchPersonalLabelVar,
                batchChemicalFormula = batchChemicalFormulaVar,


                operatorUsername = dr["username"].ToString(),
                measurementUnitName = dr["measurement_unit_name"].ToString(),
                measurementUnitSymbol = dr["measurement_unit_symbol"].ToString()
            };
            return StockTransactionExt;
        }
    }
}