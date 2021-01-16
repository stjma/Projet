using Projet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projet.Controllers
{
    public class PatientController : Controller
    {
        DBGroupe2Entities dbContext = new DBGroupe2Entities();
        // GET: Patient
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult RecherchePatient()
        {
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult RecherchePatient(int codePatient)
        {
            var  patient = dbContext.Patient.ToList().FirstOrDefault(x => x.Code_Patient == codePatient);
            if(patient != null)
            {
                return PartialView(patient);
            }
            else
            {
                ModelState.AddModelError("", "Aucun patient ne correspond a ce numéro");
                return PartialView();
            } 
        }
        public PartialViewResult AjouterCategorie(Patient patient)
        {
            //TODO Verifier si la categorie existe deja
            dbContext.Patient.Add(patient);
            dbContext.SaveChanges();
            return PartialView();
        }

        public ActionResult AfficherFiche(int codeP)
        {
                var patient = dbContext.Patient.ToList().FirstOrDefault(p => p.Code_Patient == codeP);
                return PartialView(patient);

           
        }
        //modifier teste 
        //ajouter teste (testPatient)

        //Envoie la liste de tests a la vue partielle AfficherFiche
        public PartialViewResult afficherTest(int codePatient)
        {
            //Recherche les test que le patient a subi
            var patient = dbContext.Patient.ToList().FirstOrDefault(p => p.Code_Patient == codePatient);
            return PartialView(patient);
        }
    }
}