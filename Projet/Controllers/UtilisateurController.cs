using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Projet.Models;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace Projet.Controllers
{
    public class UtilisateurController : Controller
    {
        DBGroupe2Entities dbContext = new DBGroupe2Entities();

        // GET: Utilisateur
        #region Section pour User Annonyme
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous, HttpPost]
        public ActionResult Index(Utilisateur u)
        {
            var liste = dbContext.Utilisateur.ToList();
            var user = liste.FirstOrDefault(us => us.Nom_Utilisateur_U == u.Nom_Utilisateur_U && us.Mdp_U == EncryteMdp(u.Mdp_U));
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(u.Nom_Utilisateur_U, true);

                return RedirectToAction("Index", "Patient");
            }
            else
            {
                ModelState.AddModelError("", "Le nom d'utilisateur ou le mot de passe est incorrect");
                return View();
            }
        }
        [AllowAnonymous]
        public ActionResult Inscription()
        {
            return View();
        }
        [HttpPost, AllowAnonymous]
        public ActionResult Inscription(Utilisateur u)
        {
            List<Utilisateur> liste = dbContext.Utilisateur.ToList();
            u.Date_U = DateTime.Now;
            u.Mdp_U = EncryteMdp(u.Mdp_U);
            if (dbContext.Utilisateur.Count() != 0)
            {
                int codeU = dbContext.Utilisateur.Max(us => us.Code_U);
                u.Code_U = codeU + 1;
            }
            else
                u.Code_U = 1;
            u.Code_Role = 4;
            if (isExist(u))
            {
                ModelState.AddModelError("", "Le nom d'utilisateur existe déjà");
                return View();
            }
            else
            {
                dbContext.Utilisateur.Add(u);
                dbContext.SaveChanges();
                FormsAuthentication.SetAuthCookie(u.Nom_Utilisateur_U, true);
                return RedirectToAction("Index", "Patient");

            }
        }
        #endregion
        #region Section pour la gestion de compte
        /// <summary>
        /// La view principal pour la gestion de compte d'un usager
        /// </summary>
        /// <returns></returns>
        public ActionResult MonCompte()
        {
            var user = dbContext.Utilisateur.ToList().FirstOrDefault(u => u.Nom_Utilisateur_U == User.Identity.Name);
            return View(user);
        }
        /// <summary>
        /// Ici la fonction va afficher une partialView avec les informations "personnel" du user il pourra faire les modification a l'intérieur
        /// </summary>
        /// <returns></returns>
        public PartialViewResult InformationPersonne()
        {
            var user = dbContext.Utilisateur.ToList().FirstOrDefault(u => u.Nom_Utilisateur_U == User.Identity.Name);
            return PartialView(user);
        }
        /// <summary>
        /// Ici la partialView va permettre a l'usager de modifier son mot de passe.
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ModificationMdp()
        {
            //Va chercher les informations utilisateur dans la session et l'envoie dans la vue partielle
            var user = dbContext.Utilisateur.ToList().FirstOrDefault(u => u.Nom_Utilisateur_U == User.Identity.Name);
            return PartialView(user);
        }
        [HttpPost]
        public PartialViewResult ModificationMdp(Utilisateur u)
        {
            dbContext.Utilisateur.FirstOrDefault(us => us.Code_U == u.Code_U).Mdp_U = EncryteMdp(u.Mdp_U);
            dbContext.SaveChanges();
            return PartialView();
        }
        /// <summary>
        /// Well, ici le user ce déconnecte.
        /// </summary>
        /// <returns></returns>
        public ActionResult Deconnexion()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Index", "Utilisateur");
        }
        #endregion
        #region Fonctions Divers
        /// <summary>
        /// Encrypte le mot de passe avec SHA512
        /// </summary>
        /// <param name="mdp">Le mot de passe a encrypter</param>
        /// <returns>Le mot de passe encrypter</returns>
        public string EncryteMdp(string mdp)
        {
            SHA512 crypt = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(mdp);
            byte[] hash = crypt.ComputeHash(bytes);
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                result.Append(hash[i].ToString("X2"));
            return result.ToString();

        }
        /// <summary>
        /// Regarde si le nom d'utilisateur existe déjà
        /// </summary>
        /// <param name="u">Un Utilisateur</param>
        /// <returns>True si il existe False s'il n'existe pas</returns>
        public bool isExist(Utilisateur u)
        {
            if (dbContext.Utilisateur.ToList().Find(items => items.Nom_Utilisateur_U == u.Nom_Utilisateur_U) != null)
                return true;
            else
                return false;
        }
        #endregion
    }
}