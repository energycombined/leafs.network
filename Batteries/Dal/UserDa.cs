using Batteries.Dal.Base;
using Batteries.Helpers;
using Batteries.Models;
using Batteries.Models.Responses;
using NLog;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;

namespace Batteries.Dal
{
    public class UserDa
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public User ValidateUser(string username, string password)
        {
            DataTable dt;
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = "SELECT salt FROM users WHERE username=:un AND active='true';";
                Db.CreateParameterFunc(cmd, "@un", username, NpgsqlDbType.Text);
                var salt = Db.ExecuteScalar(cmd);
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"SELECT u.*, r.role_name, rg.*
                    FROM users u
                    INNER JOIN roles r ON u.fk_role = r.role_id
                    LEFT JOIN research_group rg ON u.fk_research_group = rg.research_group_id 
                    WHERE lower(u.username)=lower(:un) AND u.pass= :p AND u.active=TRUE;";
                Db.CreateParameterFunc(cmd, "@p", CreatePasswordHash(password, salt), NpgsqlDbType.Text);
                dt = Db.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                //throw new NotFoundException(string.Format("Wrong username or password"), ErrorCodes.ErrorCodeItemNotFound);
                return null;
            }
            return CreateObjectExt(dt.Rows[0]);
        }
        public bool ChangeUserPassword(int userId, string oldPassword, string newPassword)
        {
            int rowsAffected;
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                cmd.CommandText = "SELECT salt FROM users WHERE user_id=:id;";
                Db.CreateParameterFunc(cmd, "@id", userId, NpgsqlDbType.Integer);

                var salt = Db.ExecuteScalar(cmd);

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = "SELECT user_id FROM users WHERE user_id=:id AND pass=:op;";
                Db.CreateParameterFunc(cmd, "@id", userId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@op", CreatePasswordHash(oldPassword, salt), NpgsqlDbType.Text);
                var returnedUserId = Db.ExecuteScalar(cmd);
                if (returnedUserId == "")
                {
                    throw new Exception("Incorrect old password");
                }

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    "UPDATE users SET pass=:np WHERE user_id=:id AND pass=:op;";
                Db.CreateParameterFunc(cmd, "@op", CreatePasswordHash(oldPassword, salt), NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@np", CreatePasswordHash(newPassword, salt), NpgsqlDbType.Text);

                rowsAffected = Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
            return rowsAffected == 1;
        }


        private static string CreateSalt(int size)
        {
            //Generate a cryptographic random number.
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number.
            return Convert.ToBase64String(buff);
        }

        /// <summary>
        /// Changes password hash.
        /// </summary>
        /// <param name="pwd">The password</param>
        /// <param name="salt">The salt</param>
        /// <returns>SHA1 of the password</returns>
        private static string CreatePasswordHash(string pwd, string salt)
        {
            var saltAndPwd = String.Concat(pwd, salt);
            var hashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPwd, "sha1");

            return hashedPwd;
        }

        public List<User> GetAll()
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
                    "SELECT u.*, r.role_name FROM users u INNER JOIN roles r ON u.fk_role = r.role_id;";

                dt = Db.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }

            List<User> list = (from DataRow dr in dt.Rows select CreateObject(dr)).ToList();

            return list;
        }
        public static List<UserExt> GetUsers(int? userId = null, int? roleId = null, int? researchGroupId = null, string name = null)
        {
            DataTable dt;
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = @"SELECT *
                                    FROM users u
                                        INNER JOIN roles r ON u.fk_role = r.role_id 
                                        LEFT JOIN research_group rg ON u.fk_research_group = rg.research_group_id
                                    WHERE (u.user_id = :uid or :uid is null) and
                                          (r.role_id = :roleid or :roleid is null) and
                                          (rg.research_group_id = :rgid or :rgid is null) and
                                          ((lower(u.firstname) like lower('%'|| :name ||'%') or (lower(u.lastname) like lower('%'|| :name ||'%')) or :name is null))
                                    ORDER BY u.user_id DESC
                                    ;";

                Db.CreateParameterFunc(cmd, "@uid", userId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@roleid", roleId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@name", name, NpgsqlDbType.Text);

                dt = Db.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                //logger.Error(ex.Message, ex);
                throw new Exception(ex.Message);
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                //throw new NotFoundException(string.Format("Wrong username or password"), ErrorCodes.ErrorCodeItemNotFound);
                return null;
            }

            List<UserExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }
        public static List<UserExt> GetUsersByName(int? userId = null, int? roleId = null, int? researchGroupId = null, string name = null, int? page = 1)
        {
            DataTable dt;
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = @"SELECT *
                                    FROM users u
                                        INNER JOIN roles r ON u.fk_role = r.role_id 
                                        LEFT JOIN research_group rg ON u.fk_research_group = rg.research_group_id 
                                    WHERE (u.user_id = :uid or :uid is null) and
                                          (u.user_id != 1) and
                                          (r.role_id = :roleid or :roleid is null) and
                                          (rg.research_group_id = :rgid or :rgid is null) and
                                          ((lower(u.firstname) like lower('%'|| :name ||'%') or (lower(u.lastname) like lower('%'|| :name ||'%')) or :name is null))
                                            ORDER BY u.username ASC
                                            LIMIT 10 OFFSET :offset
                                            ;";

                Db.CreateParameterFunc(cmd, "@uid", userId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@roleid", roleId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@name", name, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@offset", (page - 1) * 10, NpgsqlDbType.Integer);


                dt = Db.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                //logger.Error(ex.Message, ex);
                throw new Exception(ex.Message);
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                //throw new NotFoundException(string.Format("Wrong username or password"), ErrorCodes.ErrorCodeItemNotFound);
                return null;
            }

            List<UserExt> list = (from DataRow dr in dt.Rows select CreateObjectExt(dr)).ToList();

            return list;
        }

        public bool Insert(int roleId, string userName, string password, string firstName, string lastName, string phone, string email, int researchGroupId)
        {
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = "SELECT * FROM users WHERE LOWER(email)=LOWER(:email);";
                Db.CreateParameterFunc(cmd, "@email", email, NpgsqlDbType.Text);

                var dt = Db.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count == 0)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.CommandText = "SELECT * FROM users WHERE LOWER(username)=LOWER(:un);";
                    Db.CreateParameterFunc(cmd, "@un", userName, NpgsqlDbType.Text);

                    dt = Db.ExecuteSelectCommand(cmd);

                    if (dt.Rows.Count == 0)
                    {
                        if (cmd.Connection.State != ConnectionState.Open)
                        {
                            cmd.Connection.Open();
                        }

                        var salt = CreateSalt(8);

                        cmd.CommandText =
                            "INSERT INTO users(username, pass, fk_role, firstname, lastname, phone, email, salt, fk_research_group) VALUES (:un, :pass, :rid, :fn, :ln, :phone, :email, :salt, :rgid);";
                        Db.CreateParameterFunc(cmd, "@rid", roleId, NpgsqlDbType.Integer);
                        Db.CreateParameterFunc(cmd, "@pass", CreatePasswordHash(password, salt), NpgsqlDbType.Text);
                        Db.CreateParameterFunc(cmd, "@fn", firstName, NpgsqlDbType.Text);
                        Db.CreateParameterFunc(cmd, "@ln", lastName, NpgsqlDbType.Text);
                        Db.CreateParameterFunc(cmd, "@phone", phone, NpgsqlDbType.Text);
                        Db.CreateParameterFunc(cmd, "@salt", salt, NpgsqlDbType.Text);
                        Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);

                        var rowsAffected = Db.ExecuteNonQuery(cmd);
                        return rowsAffected == 1;
                    }
                    throw new Exception("The username already exists");
                }
                throw new Exception("The email address already exists");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public bool Update(int userId, string userName, string firstName, string lastName, string phone, string email, bool active)
        {
            int rowsAffected;
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                //fk_research_group=:rgid
                cmd.CommandText =
                    "UPDATE users SET firstname=:fn, lastname=:ln, phone=:phone, email=:email, active=:active WHERE user_id=:id;";
                Db.CreateParameterFunc(cmd, "@id", userId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@fn", firstName, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@ln", lastName, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@phone", phone, NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@email", email, NpgsqlDbType.Text);
                //Db.CreateParameterFunc(cmd, "@rgid", researchGroupId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@active", active, NpgsqlDbType.Boolean);

                rowsAffected = Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
            return rowsAffected == 1;
        }
        public static int Delete(int userId)
        {
            DataTable dt;
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
                //                            WHERE st.fk_material=:mid;";

                //                Db.CreateParameterFunc(cmd, "@mid", materialId, NpgsqlDbType.Integer);

                //                dt = Db.ExecuteSelectCommand(cmd);

                //                if (dt.Rows.Count > 0)
                //                {
                //                    throw new Exception("This material is related to stock data");
                //                }

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    @"DELETE FROM public.users
                                WHERE user_id=:uid;";

                Db.CreateParameterFunc(cmd, "@uid", userId, NpgsqlDbType.Integer);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }

        public List<Role> GetAllRoles()
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
                    "SELECT * FROM roles;";

                dt = Db.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }

            List<Role> list = (from DataRow dr in dt.Rows select CreateRoleObject(dr)).ToList();

            return list;
        }

        private static Role CreateRoleObject(DataRow dr)
        {
            var role = new Role
            {
                roleId = int.Parse(dr["role_id"].ToString()),
                roleName = dr["role_name"].ToString()
            };
            return role;
        }

        public UserExt Get(int userId)
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
                    @"SELECT u.*, r.role_name, rg.research_group_name FROM users u 
                    INNER JOIN roles r ON u.fk_role = r.role_id
                    INNER JOIN research_group rg ON rg.research_group_id = u.fk_research_group
                    WHERE u.user_id=:id;";
                Db.CreateParameterFunc(cmd, "@id", userId, NpgsqlDbType.Integer);
                dt = Db.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
            return CreateObjectExt(dt.Rows[0]);
        }
        private static User CreateObject(DataRow dr)
        {
            var user = new User
            {
                //UserId = int.Parse(dr["user_id"].ToString()),
                //UserName = dr["username"].ToString(),
                //FullName = dr["fullname"].ToString(),
                //Phone = dr["phone"].ToString(),
                //Email = dr["email"].ToString(),
                //Active = bool.Parse(dr["active"].ToString()),
                //UserRole = new Role
                //{
                //    RoleId = int.Parse(dr["fk_role"].ToString()),
                //    RoleName = dr["rolename"].ToString()
                //}
                userId = int.Parse(dr["user_id"].ToString()),
                userName = dr["username"].ToString(),
                firstName = dr["firstname"].ToString(),
                lastName = dr["lastname"].ToString(),
                phone = dr["phone"].ToString(),
                email = dr["email"].ToString(),
                active = bool.Parse(dr["active"].ToString()),
                userRole = new Role
                {
                    roleId = int.Parse(dr["fk_role"].ToString()),
                    roleName = dr["role_name"].ToString()
                },
                fkResearchGroup = dr["fk_research_group"] != DBNull.Value ? int.Parse(dr["fk_research_group"].ToString()) : (int?)null,
            };
            return user;
        }
        private static UserExt CreateObjectExt(DataRow dr)
        {
            var user = CreateObject(dr);

            string researchGroupNameVar = dr.Table.Columns.Contains("research_group_name") ? dr["research_group_name"].ToString() : null;
            string researchGroupAcronymVar = dr.Table.Columns.Contains("acronym") ? dr["acronym"].ToString() : null;

            var userExt = new UserExt(user)
            {
                researchGroupName = researchGroupNameVar,
                researchGroupAcronym = researchGroupAcronymVar
            };
            return userExt;
        }

        public bool CreateResetPasswordToken(string usermail)
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
                    @"SELECT u.*, r.role_name FROM users u INNER JOIN roles r ON u.fk_role = r.role_id WHERE u.email ilike :u;";

                Db.CreateParameterFunc(cmd, "@u", usermail, NpgsqlDbType.Text);
                dt = Db.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                //throw new Exception(ex.Message);
                return false;
            }
            if (dt == null || dt.Rows.Count == 0)
            {
                return false;
            }

            var userObj = CreateObject(dt.Rows[0]);

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = @"DELETE FROM recovery WHERE fk_user = :u;";
                Db.CreateParameterFunc(cmd, "@u", userObj.userId, NpgsqlDbType.Integer);
                Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                //throw new Exception(ex.Message);
                return false;
            }
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = @"INSERT INTO recovery(fk_user, token) values (:u, :t) RETURNING *;";
                Db.CreateParameterFunc(cmd, "@u", userObj.userId, NpgsqlDbType.Integer);
                Db.CreateParameterFunc(cmd, "@t", Guid.NewGuid().ToString(), NpgsqlDbType.Varchar);
                dt = Db.ExecuteSelectCommand(cmd);
                var recoveryObj = CreateRecoveryObject(dt.Rows[0]);
                string msgBody = String.Format(
                    "Click the link to reset your password <a href=\"{0}\">{1}</a>",
                    HttpUtility.HtmlEncode(
                        "http://" + ConfigurationManager.AppSettings["server"] + ":" +
                        ConfigurationManager.AppSettings["httpPort"] + "/Account/ResetPassword.aspx?token=" +
                        recoveryObj.Token), HttpUtility.HtmlEncode(userObj.userName + ", click here"));
                var result = Mail.SendMail(userObj.email, ConfigurationManager.AppSettings["mailUser"],
                    "Recovery password", msgBody);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                //throw new Exception(ex.Message);
                return false;
            }
        }

        private Recovery CreateRecoveryObject(DataRow dr)
        {
            var r = new Recovery
            {
                RecoveryId = int.Parse(dr["recovery_id"].ToString()),
                UserId = int.Parse(dr["fk_user"].ToString()),
                Token = dr["token"].ToString(),
                ValidThrough = DateTime.Parse(dr["valid_through"].ToString())
            };
            return r;
        }

        public bool ResetPassword(string email, string token, string password)
        {
            DataTable dt;
            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = @"SELECT * FROM recovery WHERE token = :t AND now() < valid_through;";

                Db.CreateParameterFunc(cmd, "@t", token, NpgsqlDbType.Varchar);

                //cmd.CommandText = @"SELECT * FROM recovery WHERE token::uuid = :t AND now() < valid_through;";
                //Db.CreateParameterFunc(cmd, "@t", token, NpgsqlDbType.Uuid);
                dt = Db.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
            if (dt == null || dt.Rows.Count == 0)
            {
                return false;
            }

            var recoveryObj = CreateRecoveryObject(dt.Rows[0]);

            try
            {
                var cmd = Db.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                cmd.CommandText = "SELECT salt FROM users WHERE user_id=:u;";
                Db.CreateParameterFunc(cmd, "@u", recoveryObj.UserId, NpgsqlDbType.Integer);

                var salt = Db.ExecuteScalar(cmd);

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = "UPDATE users SET pass=:np WHERE user_id=:u;";
                Db.CreateParameterFunc(cmd, "@np", CreatePasswordHash(password, salt), NpgsqlDbType.Text);
                Db.CreateParameterFunc(cmd, "@u", recoveryObj.UserId, NpgsqlDbType.Integer);
                int rowsAffected = Db.ExecuteNonQuery(cmd);
                if (rowsAffected == 1)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.CommandText = "DELETE FROM recovery WHERE fk_user = :u;";
                    Db.CreateParameterFunc(cmd, "@u", recoveryObj.UserId, NpgsqlDbType.Integer);
                    Db.ExecuteNonQuery(cmd);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }
    }
}