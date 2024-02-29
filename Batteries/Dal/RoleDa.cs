using Batteries.Dal.Base;
using Batteries.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Batteries.Dal
{
    public class RoleDa
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Get all user roles
        /// </summary>
        /// <returns></returns>
        public static List<Role> GetAllRoles()
        {
            DataTable dt;
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                cmd.CommandText = @"SELECT * FROM roles;";

                dt = Db.ExecuteSelectCommand(cmd);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            List<Role> list = (from DataRow dr in dt.Rows select CreateObject(dr)).ToList();

            return list;
        }

        private static Role CreateObject(DataRow dr)
        {
            var role = new Role
            {
                roleId = int.Parse(dr["role_id"].ToString()),
                roleName = dr["role_name"].ToString()
            };
            return role;
        }
    }
}