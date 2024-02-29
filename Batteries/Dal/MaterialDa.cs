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
    public class MaterialDa
    {
        public static List<MaterialExt> GetAllMaterialsWithQuantity(int? researchGroupId = null, long? materialId = null, int? materialTypeId = null)
        {
            DataTable dt;

            //if (researchGroupId == null)
            //{
            //    researchGroupId = 1;
            //}

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"SELECT *,
                      u.username as operator_username,
                      (
                        SELECT SUM(stock_transaction.amount * stock_transaction.transaction_direction)
                        FROM stock_transaction
                        WHERE stock_transaction.fk_material = m.material_id AND
                          stock_transaction.fk_research_group = :rgid
                      ) as available_quantity

                    FROM material m
                        LEFT JOIN material_type mt ON m.fk_material_type = mt.material_type_id
                        LEFT JOIN stored_in_type sit ON m.fk_stored_in_type = sit.stored_in_type_id
                        LEFT JOIN measurement_unit mu ON m.fk_measurement_unit = mu.measurement_unit_id
                        LEFT JOIN material_function mf ON m.fk_function = mf.material_function_id
                        LEFT JOIN vendor v ON m.fk_vendor = v.vendor_id
                        LEFT JOIN users u ON m.fk_operator = u.user_id

                    WHERE (m.material_id = :mid or :mid is null) and
                        (m.fk_material_type = :mtid or :mtid is null) and
                        (u.fk_research_group = :rgid or :rgid is null)
ORDER BY m.material_id DESC
;";

                Db.CreateParameterFunc(cmd, "@mid", materialId, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@mtid", materialTypeId, NpgsqlDbType.Integer);
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

            List<MaterialExt> list = (from DataRow dr in dt.Rows select CreateMaterialObjectExt(dr)).ToList();

            return list;
        }

        public static List<MaterialExt> GetAllMaterialsGeneralData(int? materialId = null, int? researchGroupId = null)
        {
            //DOES NOT MATTER IF COMPLETE
            DataTable dt;

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"SELECT *, u.username as operator_username
                      
                    FROM material m
                        LEFT JOIN users u ON m.fk_operator = u.user_id
                        LEFT JOIN material_type mt ON m.fk_material_type = mt.material_type_id
                        LEFT JOIN stored_in_type sit ON m.fk_stored_in_type = sit.stored_in_type_id
                        LEFT JOIN measurement_unit mu ON m.fk_measurement_unit = mu.measurement_unit_id
                        LEFT JOIN material_function mf ON m.fk_function = mf.material_function_id
                        LEFT JOIN research_group rg ON u.fk_research_group = rg.research_group_id

                    WHERE (m.material_id = :mid) and
                            (u.fk_research_group = :rgid or :rgid is null)
                    ORDER BY m.date_created DESC;";

                Db.CreateParameterFunc(cmd, "@mid", materialId, NpgsqlDbType.Integer);
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

            List<MaterialExt> list = (from DataRow dr in dt.Rows select CreateMaterialObjectExt(dr)).ToList();

            return list;
        }

        public static List<MaterialExt> GetMaterialWithQuantity(int? researchGroupId = null, long? materialId = null, int? materialTypeId = null)
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
                    @"SELECT *,
                      u.username as operator_username,
                      (
                        SELECT SUM(stock_transaction.amount * stock_transaction.transaction_direction)
                        FROM stock_transaction
                        WHERE stock_transaction.fk_material = m.material_id AND
                          stock_transaction.fk_research_group = :rgid
                      ) as available_quantity

                    FROM material m
                        LEFT JOIN material_type mt ON m.fk_material_type = mt.material_type_id
                        LEFT JOIN stored_in_type sit ON m.fk_stored_in_type = sit.stored_in_type_id
                        LEFT JOIN measurement_unit mu ON m.fk_measurement_unit = mu.measurement_unit_id
                        LEFT JOIN material_function mf ON m.fk_function = mf.material_function_id
                        LEFT JOIN vendor v ON m.fk_vendor = v.vendor_id
                        LEFT JOIN users u ON m.fk_operator = u.user_id

                    WHERE (m.material_id = :mid or :mid is null) and
                        (m.fk_material_type = :mtid or :mtid is null)
ORDER BY m.material_id DESC
;";

                Db.CreateParameterFunc(cmd, "@mid", materialId, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@mtid", materialTypeId, NpgsqlDbType.Integer);
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

            List<MaterialExt> list = (from DataRow dr in dt.Rows select CreateMaterialObjectExt(dr)).ToList();

            return list;
        }

        public static List<MaterialExt> GetMaterialsByName(string search = null, int? researchGroupId = null, int? page = 1)
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
                    @"SELECT *, u.username as operator_username

                    FROM material m
                        LEFT JOIN material_type mt ON m.fk_material_type = mt.material_type_id
                        LEFT JOIN stored_in_type sit ON m.fk_stored_in_type = sit.stored_in_type_id
                        LEFT JOIN measurement_unit mu ON m.fk_measurement_unit = mu.measurement_unit_id
                        LEFT JOIN material_function mf ON m.fk_function = mf.material_function_id
                        LEFT JOIN vendor v ON m.fk_vendor = v.vendor_id
                        LEFT JOIN users u ON m.fk_operator = u.user_id

                        WHERE ((lower(m.material_name) LIKE lower('%'|| :search ||'%') or lower(m.chemical_formula) LIKE lower('%'|| :search ||'%')) or :search is null) AND
                       (u.fk_research_group = :rgid or :rgid is null)
                        ORDER BY m.material_id DESC 
                        LIMIT 10 OFFSET :offset;";

                //Db.CreateParameterFunc(cmd, "@mid", materialId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
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

            List<MaterialExt> list = (from DataRow dr in dt.Rows select CreateMaterialObjectExt(dr)).ToList();

            return list;
        }
        public static List<MaterialExt> GetMaterialsByNameWithQuantity(string search = null, int? materialFunction = null, int? researchGroupId = null, int? page = 1)
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
                    @"SELECT *, u.username as operator_username,
                      (
                        SELECT SUM(stock_transaction.amount * stock_transaction.transaction_direction)
                        FROM stock_transaction
                        WHERE stock_transaction.fk_material = m.material_id AND
                          stock_transaction.fk_research_group = :rgid
                      ) as available_quantity

                    FROM material m
                        LEFT JOIN material_type mt ON m.fk_material_type = mt.material_type_id
                        LEFT JOIN stored_in_type sit ON m.fk_stored_in_type = sit.stored_in_type_id
                        LEFT JOIN measurement_unit mu ON m.fk_measurement_unit = mu.measurement_unit_id
                        LEFT JOIN material_function mf ON m.fk_function = mf.material_function_id
                        LEFT JOIN vendor v ON m.fk_vendor = v.vendor_id
                        LEFT JOIN users u ON m.fk_operator = u.user_id

                    WHERE ((lower(m.material_name) LIKE lower('%'|| :search ||'%') or lower(m.chemical_formula) LIKE lower('%'|| :search ||'%')) or :search is null) and
                          (m.fk_function = :mfid or :mfid is null) and
                        (u.fk_research_group = :rgid)
ORDER BY m.material_id DESC
    LIMIT 10 OFFSET :offset
                        ;";

                //Db.CreateParameterFunc(cmd, "@mid", materialId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@mfid", materialFunction, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
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

            List<MaterialExt> list = (from DataRow dr in dt.Rows select CreateMaterialObjectExt(dr)).ToList();

            return list;
        }
        public static long AddMaterial(Material material)
        {
            //not updated
            long result = 0;
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"INSERT INTO public.material (fk_material_type, fk_stored_in_type, material_name, material_label, description, chemical_formula, fk_operator, fk_measurement_unit, date_bought, first_use, fk_vendor, cas_number, lot_number, reference, date_created)
                    VALUES (:mtid, :sit, :n, :ml, :desc :cf, :o, :muid, :db, :fu, :vid, :ref, :cnum, :lnum, now()::timestamp) RETURNING material_id;";

                Db.CreateParameterFunc(cmd, "@mtid", material.fkMaterialType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@sit", material.fkStoredInType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@n", material.materialName, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@ml", material.materialLabel, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@desc", material.description, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@cf", material.chemicalFormula, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@o", material.fkOperator, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@muid", material.fkMeasurementUnit, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@db", material.dateBought, NpgsqlDbType.Date);
                Db.CreateParameterFunc(cmd, "@fu", material.firstUse, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@vid", material.fkVendor, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@cnum", material.casNumber, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lnum", material.lotNumber, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@poa", material.percentageOfActive, NpgsqlDbType.Double);

                Db.CreateParameterFunc(cmd, "@ref", material.reference, NpgsqlDbType.Text);


                //Db.ExecuteNonQuery(cmd);
                result = long.Parse(Db.ExecuteScalar(cmd));
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting material", ex);
            }

            return result;
        }
        public static long AddMaterialWithStock(Material material, double amount, int researchGroupId)
        {
            long returnedMaterialId = 0;
            long result = 0;
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"INSERT INTO public.material (fk_material_type, fk_stored_in_type, material_name, material_label, description, chemical_formula, fk_operator, fk_measurement_unit, fk_function, percentage_of_active, date_bought, fk_vendor, price, bulk_price, reference, cas_number, lot_number, date_created)
                    VALUES (:mtid, :sit, :n, :ml, :desc, :cf, :o, :muid, :fid, :poa, :dateb, :vid, :pr, :bpr, :ref, :cnum, :lnum, now()::timestamp) RETURNING material_id;";

                Db.CreateParameterFunc(cmd, "@mtid", material.fkMaterialType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@sit", material.fkStoredInType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@n", material.materialName, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@ml", material.materialLabel, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@desc", material.description, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@cf", material.chemicalFormula, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@o", material.fkOperator, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@muid", material.fkMeasurementUnit, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@fid", material.fkFunction, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@dateb", material.dateBought, NpgsqlDbType.Date);
                //Db.CreateParameterFunc(cmd, "@fu", material.firstUse, NpgsqlDbType.Date);
                Db.CreateParameterFunc(cmd, "@vid", material.fkVendor, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pr", material.price, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@bpr", material.bulkPrice, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@ref", material.reference, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@cnum", material.casNumber, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lnum", material.lotNumber, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@poa", material.percentageOfActive, NpgsqlDbType.Double);

                //Db.ExecuteNonQuery(cmd);
                returnedMaterialId = long.Parse(Db.ExecuteScalar(cmd));
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting material", ex);
            }

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                cmd.CommandText =
                    @"INSERT INTO public.stock_transaction (fk_material, amount, date_bought, fk_vendor, fk_operator, fk_research_group, transaction_direction, stock_transaction_element_type, date_created)
                    VALUES (:mid, :a, :db, :vid, :oid, :rgid, 1, 1, now()::timestamp) RETURNING stock_transaction_id;";

                Db.CreateParameterFunc(cmd, "@mid", returnedMaterialId, NpgsqlDbType.Bigint);
                Db.CreateParameterFunc(cmd, "@a", amount, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@db", material.dateBought, NpgsqlDbType.Date);
                Db.CreateParameterFunc(cmd, "@vid", material.fkVendor, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@oid", material.fkOperator, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@rgid", stockTransaction.fkResearchGroup, NpgsqlDbType.Integer);
                //Db.CreateParameterFunc(cmd, "@eid", stockTransaction.fkExperiment, NpgsqlDbType.Integer);

                result = long.Parse(Db.ExecuteScalar(cmd));
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting material stock information", ex);
            }

            return returnedMaterialId;
        }
        public static int UpdateMaterial(Material material)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.material
                        SET material_name=:n, material_label=:ml, description=:desc, chemical_formula=:cf, fk_material_type=:mt, fk_stored_in_type=:sit, fk_operator=:o, fk_measurement_unit=:muid, fk_function=:fid, percentage_of_active=:poa, date_bought=:db, fk_vendor=:vid, price=:pr, bulk_price=:bpr, reference=:ref, cas_number=:cnum, lot_number=:lnum, last_change=now()::timestamp
                        WHERE material_id=:mid;";


                Db.CreateParameterFunc(cmd, "@n", material.materialName, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@ml", material.materialLabel, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@desc", material.description, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@cf", material.chemicalFormula, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@mt", material.fkMaterialType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@sit", material.fkStoredInType, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@o", material.fkOperator, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@muid", material.fkMeasurementUnit, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@fid", material.fkFunction, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@db", material.dateBought, NpgsqlDbType.Date);
                Db.CreateParameterFunc(cmd, "@vid", material.fkVendor, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@pr", material.price, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@bpr", material.bulkPrice, NpgsqlDbType.Double);
                Db.CreateParameterFunc(cmd, "@ref", material.reference, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@cnum", material.casNumber, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@lnum", material.lotNumber, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@poa", material.percentageOfActive, NpgsqlDbType.Double);

                Db.CreateParameterFunc(cmd, "@mid", material.materialId, NpgsqlDbType.Bigint);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Material info", ex);
            }
            return 0;
        }
        public static int DeleteMaterial(long materialId)
        {
            DataTable dt;
            var cmd = Db.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            NpgsqlTransaction t = cmd.Connection.BeginTransaction();

            try
            {
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                            @"SELECT st.stock_transaction_id 
                            FROM stock_transaction st
                            WHERE st.fk_material=:mid AND
                            st.transaction_direction = -1
                            ;";

                Db.CreateParameterFunc(cmd, "@mid", materialId, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd, false);

                if (dt.Rows.Count > 0)
                {
                    throw new Exception("This material is related to stock data");
                }

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                            @"SELECT battery_component_id 
                            FROM battery_component
                            WHERE fk_step_material=:mid
                            ;";

                Db.CreateParameterFunc(cmd, "@mid", materialId, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd, false);

                if (dt.Rows.Count > 0)
                {
                    throw new Exception("This material is used in some experiment");
                }
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                            @"SELECT batch_content_id 
                            FROM batch_content
                            WHERE fk_step_material=:mid
                            ;";

                Db.CreateParameterFunc(cmd, "@mid", materialId, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd, false);

                if (dt.Rows.Count > 0)
                {
                    throw new Exception("This material is used in some batch");
                }

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.stock_transaction
                                WHERE fk_material=:mid;";

                Db.CreateParameterFunc(cmd, "@mid", materialId, NpgsqlDbType.Bigint);
                Db.ExecuteNonQuery(cmd, false);

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.material
                                WHERE material_id=:mid;";

                Db.CreateParameterFunc(cmd, "@mid", materialId, NpgsqlDbType.Bigint);
                Db.ExecuteNonQuery(cmd, false);

                t.Commit();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                t.Rollback();
                throw new Exception(ex.Message);
            }
            return 0;
        }

        //        public static int DeleteMaterial(int materialId)
        //        {
        //DataTable dt;
        //try
        //{
        //var cmd = Db.CreateCommand();
        //if (cmd.Connection.State != ConnectionState.Open)
        //{
        //    cmd.Connection.Open();
        //}
        //                        cmd.CommandText =
        //                            @"SELECT st.stock_transaction_id 
        //                            FROM stock_transaction st
        //                            WHERE st.fk_material=:mid;";

        //                        Db.CreateParameterFunc(cmd, "@snid", streetNameId, NpgsqlDbType.Integer);

        //                        dt = Db.ExecuteSelectCommand(cmd);

        //                        if (dt.Rows.Count > 0)
        //                        {
        //                            throw new Exception("There is a customer connected to this street name");
        //                        }
        //                var cmd2 = Db.CreateCommand("SewageConnection");
        //                if (cmd2.Connection.State != ConnectionState.Open)
        //                {
        //                    cmd2.Connection.Open();
        //                }
        //                cmd2.CommandText =
        //                    @"SELECT c.customer_id 
        //                    FROM customer c
        //                    WHERE c.fk_street_name=:snid;";

        //                Db.CreateParameterFunc(cmd2, "@snid", streetNameId, NpgsqlDbType.Integer);

        //                dt = Db.ExecuteSelectCommand(cmd2);

        //                if (dt.Rows.Count > 0)
        //                {
        //                    throw new Exception("There is a customer connected to this street name");
        //                }
        //                if (cmd.Connection.State != ConnectionState.Open)
        //                {
        //                    cmd.Connection.Open();
        //                }
        //                cmd.CommandText =
        //                    @"SELECT cp.connection_point_id 
        //                    FROM connection_point cp
        //                    WHERE cp.fk_street_name=:snid;";

        //                Db.CreateParameterFunc(cmd, "@snid", streetNameId, NpgsqlDbType.Integer);

        //                dt = Db.ExecuteSelectCommand(cmd);

        //                if (dt.Rows.Count > 0)
        //                {
        //                    throw new Exception("There is a connection point connected to this street name");
        //                }
        //                        if (cmd.Connection.State != ConnectionState.Open)
        //                        {
        //                            cmd.Connection.Open();
        //                        }
        //                        cmd.CommandText =
        //                            @"DELETE FROM public.street_name
        //                                WHERE  street_name_id=:snid;";

        //                        Db.CreateParameterFunc(cmd, "@snid", streetNameId, NpgsqlDbType.Integer);
        //                        Db.ExecuteNonQuery(cmd);
        //            }
        //catch (Exception ex)
        //{
        //    throw new Exception(ex.Message);
        //}
        //return 0;
        //        }

        public static Material CreateMaterialObject(DataRow dr)
        {
            var material = new Material
            {
                materialId = (long)dr["material_id"],
                materialName = dr["material_name"].ToString(),
                materialLabel = dr["material_label"].ToString(),
                description = dr["description"].ToString(),
                chemicalFormula = dr["chemical_formula"].ToString(),
                fkMaterialType = dr["fk_material_type"] != DBNull.Value ? int.Parse(dr["fk_material_type"].ToString()) : (int?)null,
                fkStoredInType = dr["fk_stored_in_type"] != DBNull.Value ? int.Parse(dr["fk_stored_in_type"].ToString()) : (int?)null,
                fkOperator = dr["fk_operator"] != DBNull.Value ? int.Parse(dr["fk_operator"].ToString()) : (int?)null,
                fkMeasurementUnit = dr["fk_measurement_unit"] != DBNull.Value ? int.Parse(dr["fk_measurement_unit"].ToString()) : (int?)null,
                dateBought = dr["date_bought"] != DBNull.Value ? DateTime.Parse(dr["date_bought"].ToString()) : (DateTime?)null,
                firstUse = dr["first_use"] != DBNull.Value ? DateTime.Parse(dr["first_use"].ToString()) : (DateTime?)null,
                fkVendor = dr["fk_vendor"] != DBNull.Value ? int.Parse(dr["fk_vendor"].ToString()) : (int?)null,
                price = dr["price"] != DBNull.Value ? double.Parse(dr["price"].ToString()) : (double?)null,
                bulkPrice = dr["bulk_price"] != DBNull.Value ? double.Parse(dr["bulk_price"].ToString()) : (double?)null,
                reference = dr["reference"].ToString(),
                fkFunction = dr["fk_function"] != DBNull.Value ? int.Parse(dr["fk_function"].ToString()) : (int?)null,
                casNumber = dr["cas_number"].ToString(),
                lotNumber = dr["lot_number"].ToString(),
                percentageOfActive = dr["percentage_of_active"] != DBNull.Value ? double.Parse(dr["percentage_of_active"].ToString()) : (double?)null,
                dateCreated = dr["date_created"] != DBNull.Value ? DateTime.Parse(dr["date_created"].ToString()) : (DateTime?)null,
                lastChange = dr["last_change"] != DBNull.Value ? DateTime.Parse(dr["last_change"].ToString()) : (DateTime?)null

            };
            return material;
        }
        private static MaterialExt CreateMaterialObjectExt(DataRow dr)
        {
            var material = CreateMaterialObject(dr);

            double? availableQuantityVar = (double?)null;
            if (dr.Table.Columns.Contains("available_quantity"))
            {
                availableQuantityVar = dr["available_quantity"] != DBNull.Value ? double.Parse(dr["available_quantity"].ToString()) : 0;
            }

            //double? availableQuantityVar =
            //    dr.Table.Columns.Contains("available_quantity") ?
            //    double.Parse(dr["available_quantity"].ToString()) : (double?)null;

            string operatorUsernameVar = dr.Table.Columns.Contains("operator_username") ? dr["operator_username"].ToString() : null;
            string materialFunctionVar = dr.Table.Columns.Contains("material_function_name") ? dr["material_function_name"].ToString() : null;
            string vendorNameVar = dr.Table.Columns.Contains("vendor_name") ? dr["vendor_name"].ToString() : null;
            string vendorSiteVar = dr.Table.Columns.Contains("vendor_site") ? dr["vendor_site"].ToString() : null;

            var materialExt = new MaterialExt(material)
            {
                materialType = dr["material_type"].ToString(),
                storedInType = dr["stored_in_type"].ToString(),
                measurementUnitName = dr["measurement_unit_name"].ToString(),
                measurementUnitSymbol = dr["measurement_unit_symbol"].ToString(),
                materialFunction = materialFunctionVar,
                operatorUsername = operatorUsernameVar,
                availableQuantity = availableQuantityVar,
                vendorName = vendorNameVar,
                vendorSite = vendorSiteVar,
                fkResearchGroup = dr["fk_research_group"] != DBNull.Value ? int.Parse(dr["fk_research_group"].ToString()) : (int?)null,
            };
            return materialExt;
        }
    }
}