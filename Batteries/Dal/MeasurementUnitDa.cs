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
    public class MeasurementUnitDa
    {
        public static List<MeasurementUnit> GetAllMeasurementUnits(int? measurementUnitId = null)
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
                    FROM measurement_unit mu                        
                    WHERE (mu.measurement_unit_id = :muid or :muid is null);";

                Db.CreateParameterFunc(cmd, "@muid", measurementUnitId, NpgsqlDbType.Integer);

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

            List<MeasurementUnit> list = (from DataRow dr in dt.Rows select CreateObject(dr)).ToList();

            return list;
        }
        public static List<MeasurementUnit> GetMeasurementUnitsByName(string search = null)
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
                    FROM measurement_unit mu
                    WHERE (lower(mu.measurement_unit_name) LIKE lower('%'|| :search ||'%') or :search is null)
                    LIMIT 10 
                    ;";

                Db.CreateParameterFunc(cmd, "@search", search, NpgsqlDbType.Text);

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

            List<MeasurementUnit> list = (from DataRow dr in dt.Rows select CreateObject(dr)).ToList();

            return list;
        }
        public static int AddMeasurementUnit(MeasurementUnit measurementUnit)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"INSERT INTO public.measurement_unit (measurement_unit_name, measurement_unit_symbol, last_change)
                    VALUES (:mun, :mus, now()::timestamp);";

                Db.CreateParameterFunc(cmd, "@mun", measurementUnit.measurementUnitName, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@mus", measurementUnit.measurementUnitSymbol, NpgsqlDbType.Text);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting measurement unit", ex);
            }

            return 0;
        }
        public static int UpdateMeasurementUnit(MeasurementUnit measurementUnit)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"UPDATE public.measurement_unit
                        SET measurement_unit_name=:mun, measurement_unit_symbol=:mus, last_change=now()::timestamp
                        WHERE measurement_unit_id=:muid;";

                Db.CreateParameterFunc(cmd, "@mun", measurementUnit.measurementUnitName, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@mus", measurementUnit.measurementUnitSymbol, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@muid", measurementUnit.measurementUnitId, NpgsqlDbType.Integer);

                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating measurement unit info", ex);
            }
            return 0;
        }
        public static int DeleteMeasurementUnit(int measurementUnitId)
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
                            @"SELECT m.material_id 
                            FROM material m
                            WHERE m.fk_measurement_unit=:muid;";

                Db.CreateParameterFunc(cmd, "@muid", measurementUnitId, NpgsqlDbType.Integer);

                dt = Db.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count > 0)
                {
                    throw new Exception("There is some material associated to this measurement unit!");
                }

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.measurement_unit
                                WHERE measurement_unit_id=:muid;";

                Db.CreateParameterFunc(cmd, "@muid", measurementUnitId, NpgsqlDbType.Integer);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
        public static MeasurementUnit CreateObject(DataRow dr)
        {
            var measurementUnit = new MeasurementUnit
            {
                measurementUnitId = (int)dr["measurement_unit_id"],
                measurementUnitName = dr["measurement_unit_name"].ToString(),
                measurementUnitSymbol = dr["measurement_unit_symbol"].ToString()
            };
            return measurementUnit;
        }
    }
}