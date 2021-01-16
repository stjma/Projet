using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Projet.Models;
using PagedList;

namespace Projet.Controllers
{
    public class AdministrationController : Controller
    {
        // GET: Administration
        DBGroupe2Entities dbContext = new DBGroupe2Entities();
        public ActionResult Index()
        {
            return View();
        }
        #region Gestion des Users
        //[Authorize(Roles = "Administrateur")]
        public PartialViewResult ListeUser()
        {
            return PartialView(dbContext.Utilisateur.ToList());
        }

        public PartialViewResult AfficherUser(string nomUser)
        {
            var user = dbContext.Utilisateur.ToList().FirstOrDefault(u => u.Nom_Utilisateur_U == nomUser);
            var role = dbContext.Role_User.Select(x => new SelectListItem { Value = x.Code_Role.ToString(), Text = x.Nom_Role });
            ViewBag.list = new SelectList(role, "Value","Text");
            return PartialView(user);
        }
        [Authorize(Roles = "Administrateur"), HttpPost]
        public PartialViewResult SetUserRole(string username, string roleName)
        {
            Roles.AddUsersToRoles(new string[] { username }, new string[] { roleName });
            ViewBag.View = "ListeUser";
            return PartialView();
        }
        public ActionResult SupprimeUser(int codeU)
        {
            DBGroupe2Entities db = new DBGroupe2Entities();
            db.Utilisateur.Remove(db.Utilisateur.ToList().FirstOrDefault(u => u.Code_U == codeU));
            db.SaveChanges();
            ViewBag.View = "ListeUser";
            return View("Index");
        }
        [HttpPost]
        public ActionResult AfficherUser(Utilisateur u)
        {
            dbContext.Utilisateur.ToList().FirstOrDefault(ut => ut.Code_U == u.Code_U).Nom_U = u.Nom_U;
            dbContext.Utilisateur.ToList().FirstOrDefault(ut => ut.Code_U == u.Code_U).Prenom_U = u.Prenom_U;
            dbContext.Utilisateur.ToList().FirstOrDefault(ut => ut.Code_U == u.Code_U).Nom_Utilisateur_U = u.Nom_Utilisateur_U;
            //dbContext.Utilisateur.ToList().FirstOrDefault(ut => ut.Code_U == u.Code_U).Code_Role = u.Code_Role;
            dbContext.Utilisateur.ToList().FirstOrDefault(ut => ut.Code_U == u.Code_U).Email_U = u.Email_U;
            dbContext.SaveChanges();
            ViewBag.View = "ListeUser";
            return View("Index");
        }
        #endregion
        #region Gestion des Patient
        public PartialViewResult AfficherPatients(int? page)
        {
            return PartialView(dbContext.Patient.ToList().ToPagedList(page ?? 1, 10));
        }
        public PartialViewResult AfficherPatient(int codeP)
        {
            return PartialView(dbContext.Patient.FirstOrDefault(p => p.Code_Patient == codeP));
        }
        public PartialViewResult AjouterPatient()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult AjouterPatient(Patient p)
        {
            dbContext.Patient.Add(p);
            dbContext.SaveChanges();
            ViewBag.View = "AfficherPatients";
            return View("Index");
        }
        public PartialViewResult ArchiverPatient()
        {
            return PartialView();
        }
        [HttpPost]
        public PartialViewResult SupprimerPatient(int codeP)
        {
            dbContext.Patient.Remove(dbContext.Patient.ToList().FirstOrDefault(p => p.Code_Patient == codeP));
            dbContext.SaveChanges();
            return PartialView();
        }
        /// <summary>
        /// J'entend par ici la modification des informations de base du patient (Genre s'il y une allergie découverte en cours de route mettons)
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ModifierPatient(int codeP)
        {
            var patient = dbContext.Patient.ToList().FirstOrDefault(p => p.Code_Patient == codeP);
            return PartialView(patient);
        }
        #endregion
        #region Gestion des categorie
        public PartialViewResult AjouterCategorie()
        {
            return PartialView();
        }
        public PartialViewResult ListeCategorie()
        {
            return PartialView(dbContext.Categorie.ToList());
        }
        public ActionResult SupprimerCategorie(int codeC)
        {
            dbContext.Categorie.Remove(dbContext.Categorie.ToList().FirstOrDefault(c => c.Code_Categorie == codeC));
            dbContext.SaveChanges();
            ViewBag.View = "ListeCategorie";
            return View("Index");
        }
        public PartialViewResult ModifierCategorie(int codeC)
        {
            return PartialView(dbContext.Categorie.ToList().FirstOrDefault(c => c.Code_Categorie == codeC));
        }
        [HttpPost]
        public ActionResult ModifierCategorie(FormCollection collection)
        {
            var categorie = dbContext.Categorie.ToList().FirstOrDefault(cat => cat.Code_Categorie == Convert.ToInt32(collection["Code_Categorie"])).Nom_Categorie = collection["Nom_Categorie"];
            dbContext.SaveChanges();
            ViewBag.View = "ListeCategorie";
            return View("Index");
        }
        [HttpPost]
        public ActionResult AjouterCategorie(Categorie c)
        {
            dbContext.Categorie.Add(c);
            dbContext.SaveChanges();
            ViewBag.View = "ListeCategorie";
            return View("Index");
        }
        #endregion
        #region Gestion des Test
        public PartialViewResult ListeTests()
        {
            return PartialView(dbContext.Tests.ToList());
        }
        public PartialViewResult AjouterTest()
        {
            var cat = dbContext.Categorie.Select(x => new SelectListItem { Value = x.Code_Categorie.ToString(), Text = x.Nom_Categorie });
            ViewBag.list = new SelectList(cat, "Value", "Text");
            return PartialView();
        }
        public PartialViewResult ModifierTest(int codeT)
        {
            return PartialView(dbContext.Tests.ToList().FirstOrDefault(t => t.Code_Test == codeT));
        }
        public ActionResult SupprimerTest(int codeT)
        {
            dbContext.Tests.Remove(dbContext.Tests.ToList().FirstOrDefault(t => t.Code_Test == codeT));
            dbContext.SaveChanges();
            ViewBag.View = "ListeTests";
            return View("Index");
        }
        [HttpPost]
        public ActionResult ModifierTest(Tests t)
        {
            dbContext.Tests.ToList().FirstOrDefault(te => te.Code_Test == t.Code_Test).Nom_test = t.Nom_test;
            dbContext.Tests.ToList().FirstOrDefault(te => te.Code_Test == t.Code_Test).Code_Categorie = t.Code_Categorie;
            dbContext.SaveChanges();
            ViewBag.View = "ListeTests";
            return View("Index");
        }
        [HttpPost]
        public ActionResult AjouterTest(Tests t)
        {
            dbContext.Tests.Add(t);
            dbContext.SaveChanges();
            ViewBag.View = "ListeTests";
            return View("Index");
        }
        #endregion
    }
}