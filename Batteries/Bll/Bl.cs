using Batteries.Dal;
using Batteries.Models;
using Batteries.Models.Responses;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;

namespace Batteries.Bll
{
    public class Bl
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Kreira reset token na za promena na lozinka
        /// </summary>
        /// <param name="usermail">korisnicko ime ili lozinka</param>
        /// <returns>uspesnost na rezultatot na zapisuvanje</returns>
        public static bool CreateResetPasswordToken(string usermail)
        {
            var result = new UserDa().CreateResetPasswordToken(usermail);
            return result;
        }

        /// <summary>
        /// Resetira lozinka
        /// </summary>
        /// <param name="email">E-mail</param>
        /// <param name="token">Token</param>
        /// <param name="password">Lozinka</param>
        /// <returns>uspesnost na rezultatot na zapisuvanje</returns>
        public static bool ResetPassword(string email, string token, string password)
        {
            var result = new UserDa().ResetPassword(email, token, password);
            return result;
        }

        /// <summary>
        /// Zemi gi site korisnici
        /// </summary>
        /// <returns>Lista od User objekti</returns>
        public static List<User> GetAllUsers()
        {
            var users = new UserDa().GetAll();
            return users;
        }
        /// <summary>
        /// Zemi gi site rolji
        /// </summary>
        /// <returns>Lista od Role objekti</returns>
        public static List<Role> GetAllRoles()
        {
            var roles = new UserDa().GetAllRoles();
            return roles;
        }

        /// <summary>
        /// Vnesi nov korisnik
        /// </summary>
        /// <param name="roleId">ID na rolja</param>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <param name="fullName">Celosno ime</param>
        /// <param name="phone">Telefon</param>
        /// <param name="email">E-mail</param>
        /// <returns>uspesnost na rezultatot na zapisuvanje</returns>
        public static bool InsertUser(int roleId, string userName, string password, string firstName, string lastName, string phone,
            string email, int researchGroupId)
            //research group
        {
            var result = new UserDa().Insert(roleId, userName, password, firstName, lastName, phone, email, researchGroupId);
            return result;
        }

        /// <summary>
        /// Promena na korisnik
        /// </summary>
        /// <param name="userId">ID na korisnik</param>
        /// <param name="userName">Korisnicko ime</param>
        /// <param name="fullName">Celosno ime</param>
        /// <param name="phone">Telefon</param>
        /// <param name="email">Email</param>
        /// <param name="active">Dali e aktiven</param>
        /// <returns>uspesnost na rezultatot na zapisuvanje</returns>
        public static bool UpdateUser(int userId, string userName, string firstName, string lastName, string phone, string email,  bool active = true)
        {
            var result = new UserDa().Update(userId, userName, firstName, lastName, phone, email,  active);
            return result;
        }

        /// <summary>
        /// Zemi korisnik
        /// </summary>
        /// <param name="userId">ID na korisnikot</param>
        /// <returns>User objekt</returns>
        public static UserExt GetUser(int userId)
        {
            var user = new UserDa().Get(userId);
            return user;
        }

       
    }
}