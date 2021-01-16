using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Security;
using System.Data.Sql;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
namespace Projet.Models
{
    public class MyRoleProvider : RoleProvider
    {
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Retourne le nom du role que le user a
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Retourne string[]</returns>
        public override string[] GetRolesForUser(string username)
        {
            using (DBGroupe2Entities dbContext = new DBGroupe2Entities())
            {
                Utilisateur user = dbContext.Utilisateur.FirstOrDefault(u => u.Nom_Utilisateur_U.Equals(username, StringComparison.CurrentCulture));
                var roles = (from ur in dbContext.Utilisateur
                             where ur.Code_U == user.Code_U
                             from r in dbContext.Role_User
                             where r.Code_Role == ur.Code_Role
                             select r.Nom_Role);
                if (roles != null)
                    return roles.ToArray();
                else
                    return new string[] { };
            }
        }
        /// <summary>
        /// Permet de connaître le role de l'utilisateur
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
        /// <returns>true ou false</returns>
        public override bool IsUserInRole(string username, string roleName)
        {
            using (DBGroupe2Entities dbContext = new DBGroupe2Entities())
            {
                Utilisateur user = dbContext.Utilisateur.FirstOrDefault(u => u.Nom_Utilisateur_U.Equals(username, StringComparison.CurrentCulture));
                var roles = (from ur in dbContext.Utilisateur
                             where ur.Code_Role == user.Code_Role
                             from r in dbContext.Role_User
                             where r.Code_Role == ur.Code_Role
                             select r.Nom_Role);
                if (user != null)
                    return roles.Any(ro => ro.Equals(roleName, StringComparison.CurrentCultureIgnoreCase));
                else
                    return false;
            }
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {

            SqlConnection strConnexion = new SqlConnection(WebConfigurationManager.ConnectionStrings["CnHopital"].ConnectionString);//Prend la connexion
            SqlCommand cmd = new SqlCommand("update Utilisateur set Code_Role = @CodeRole where Nom_Utilisateur_U = '@NomUser'", strConnexion);//Requete sql poour updater le role d'un role
            SqlParameter codeParam = cmd.Parameters.Add("@CodeRole", SqlDbType.Int);//Paramètre du code du role
            SqlParameter userParam = cmd.Parameters.Add("@NomUser", SqlDbType.VarChar, 50);//Nom du User
            strConnexion.Open();
            using (DBGroupe2Entities dbContext = new DBGroupe2Entities())//En utilisant la base de donner
            {
                foreach (string username in usernames)
                {
                    foreach (string role in roleNames)
                    {
                        var roles = dbContext.Role_User.FirstOrDefault(r => r.Code_Role.ToString() == role);
                        userParam.Value = username;
                        codeParam.Value = roles.Code_Role;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            strConnexion.Close();
        }
        #region

        public override void CreateRole(string roleName)
        {
            using (DBGroupe2Entities db = new DBGroupe2Entities())
            {
                var listeRole = db.Role_User.ToList();
                int codeRole = listeRole.Max(c => c.Code_Role);
                Role_User role = new Role_User();
                role.Code_Role = codeRole + 1;
                role.Nom_Role = roleName;
                db.Role_User.Add(role);
                db.SaveChanges();
                
            }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            using (DBGroupe2Entities db = new DBGroupe2Entities())
            {
                db.Role_User.Remove(db.Role_User.FirstOrDefault(r => r.Nom_Role == roleName));
                db.SaveChanges();
                return true;
            }
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }
        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}