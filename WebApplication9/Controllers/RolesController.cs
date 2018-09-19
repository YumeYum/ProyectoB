using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using PagedList;
using WebApplication9.Models;


namespace WebApplication9.Controllers
{
    public class RolesController : Controller
    {
        // GET: Roles
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index_Roles()
        {
            List<roles> listaRoles = new List<roles>();
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {

                listaRoles = dbModel.roles.ToList<roles>();

            }
            return View(listaRoles);

        }
        public ActionResult Create_Roles()
        {
            return View(new roles());
        }

        [HttpPost]
        public ActionResult Create_Roles(roles rolesModel)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                dbModel.roles.Add(rolesModel);
                dbModel.SaveChanges();
            }
            return RedirectToAction("/Index_Roles");
        }

        // GET: Proyectob/Delete/5
        public ActionResult DeleteRoles(int id)
        {
            roles rolesModel = new roles();
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                rolesModel = dbModel.roles.Where(x => x.id == id).FirstOrDefault();

            }

            return PartialView(rolesModel);
        }

        // POST: Proyectob/Delete/5
        [HttpPost]
        public ActionResult DeleteRoles(int id, FormCollection collection)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                roles rolesModel = dbModel.roles.Where(x => x.id == id).FirstOrDefault();
                dbModel.roles.Remove(rolesModel);
                dbModel.SaveChanges();
            }
            return RedirectToAction("/Index_Roles");
        }
        public ActionResult EditRoles(int id)
        {
            roles rolesModel = new roles();
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                rolesModel = dbModel.roles.Where(x => x.id == id).FirstOrDefault();

            }

            return PartialView(rolesModel);
        }
        [HttpPost]
        public ActionResult EditRoles(roles rolesModel)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                dbModel.Entry(rolesModel).State = System.Data.Entity.EntityState.Modified;
                dbModel.SaveChanges();

            }
            return RedirectToAction("/Index_roles");
        }

    }
}