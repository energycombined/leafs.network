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
    public class VendorDa
    {
        public static List<Vendor> GetAllVendors(int? vendorId = null)
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
                    FROM vendor v                        
                    WHERE (v.vendor_id = :vid or :vid is null);";

                Db.CreateParameterFunc(cmd, "@vid", vendorId, NpgsqlDbType.Integer);

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

            List<Vendor> list = (from DataRow dr in dt.Rows select CreateObject(dr)).ToList();

            return list;
        }
        public static List<Vendor> GetVendorsByName(string search = null, int? type = null)
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

                    FROM vendor v

                    WHERE ((lower(v.vendor_name) LIKE lower('%'|| :search ||'%')) or :search is null);";

                //Db.CreateParameterFunc(cmd, "@mid", vendorId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);

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

            List<Vendor> list = (from DataRow dr in dt.Rows select CreateObject(dr)).ToList();

            return list;
        }
        public static int AddVendor(Vendor vendor)
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
                    @"INSERT INTO public.vendor (vendor_name, vendor_site, contact_person, phone_number, comment, last_change)
                    VALUES (:name, :site, :cp, :pn, :comm, now()::timestamp) RETURNING vendor_id;";

                Db.CreateParameterFunc(cmd, "@name", vendor.vendorName, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@site", vendor.vendorSite, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@cp", vendor.contactPerson, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@pn", vendor.phoneNumber, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@comm", vendor.comment, NpgsqlDbType.Text);

                result = int.Parse(Db.ExecuteScalar(cmd));
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting vendor", ex);
            }

            return result;
        }
        public static int UpdateVendor(Vendor vendor)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.vendor
                        SET vendor_name=:name, vendor_site=:site, contact_person=:cp, phone_number=:pn, comment=:comm, last_change=now()::timestamp
                        WHERE vendor_id=:vid;";

                Db.CreateParameterFunc(cmd, "@name", vendor.vendorName, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@site", vendor.vendorSite, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@cp", vendor.contactPerson, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@pn", vendor.phoneNumber, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@comm", vendor.comment, NpgsqlDbType.Text);

                Db.CreateParameterFunc(cmd, "@vid", vendor.vendorId, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Vendor info", ex);
            }
            return 0;
        }
        public static int DeleteVendor(int vendorId)
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
                            @"SELECT st.stock_transaction_id 
                            FROM stock_transaction st
                            WHERE st.fk_vendor=:vid;";

                Db.CreateParameterFunc(cmd, "@vid", vendorId, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count > 0)
                {
                    throw new Exception("This vendor is related to stock data");
                }

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.vendor
                                WHERE vendor_id=:vid;";

                Db.CreateParameterFunc(cmd, "@vid", vendorId, NpgsqlDbType.Integer);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        public static Vendor CreateObject(DataRow dr)
        {
            var vendor = new Vendor
            {
                vendorId = (int)dr["vendor_id"],
                vendorName = dr["vendor_name"].ToString(),
                vendorSite = dr["vendor_site"].ToString(),
                lastChange = dr["last_change"] != DBNull.Value ? DateTime.Parse(dr["last_change"].ToString()) : (DateTime?)null,
                contactPerson = dr["contact_person"].ToString(),
                phoneNumber = dr["phone_number"].ToString(),
                comment = dr["comment"].ToString()
            };
            return vendor;
        }
    }
}