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
        public ActionResult Index_Roles(int? page, int? nn, string search, string sortBy)
        {
            ViewBag.NumeroR = new List<SelectListItem>()

                  {
                    new SelectListItem() {Text="10", Value="10"},
                    new SelectListItem() {Text="20", Value="20"},
                    new SelectListItem() {Text="40", Value="40"},
                    new SelectListItem() {Text="60", Value="60"},
                    new SelectListItem() {Text="100", Value="100"},
                    new SelectListItem() {Text="200", Value="200"},
                    new SelectListItem() {Text="500", Value="500"},
                    new SelectListItem() {Text="1000", Value="1000"},
                    new SelectListItem() {Text="2000", Value="2000"},
                    new SelectListItem() {Text="5000", Value="5000"},



                  };

            List<roles> listaRoles = new List<roles>();
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                if (search != String.Empty && search != null)
                {
                    var strings = search.ToLower().Split(' ');
                    listaRoles = dbModel.roles.ToList();

                    foreach (var splitString in strings)
                    {
                        listaRoles = listaRoles.Where(x =>x.descripcion!=null && x.descripcion.ToLower().Contains(splitString) || x.rol.ToLower().Contains(splitString)).ToList();
                    }
                }
                else
                {
                    listaRoles = dbModel.roles.ToList();

                }


            }
            ViewBag.SortRol = string.IsNullOrEmpty(sortBy) ? "rol_desc" : "";
            ViewBag.SortDescr = sortBy == "descr" ? "descr_desc" : "descr";


            switch (sortBy)
            {
                case "rol_desc":
                    listaRoles = listaRoles.OrderByDescending(x => x.rol).ToList();
                    break;
                case "descr_desc":
                    listaRoles = listaRoles.OrderByDescending(x => x.descripcion).ToList();
                    break;
                case "descr":
                    listaRoles = listaRoles.OrderBy(x => x.descripcion).ToList();
                    break;
                default:
                    listaRoles = listaRoles.OrderBy(x => x.rol).ToList();
                    break;
            }




            return View(listaRoles.ToList<roles>().ToPagedList(page?? 1, nn ?? 10));


        }
        public ActionResult Create_Roles()
        {
            return View(new roles());
        }

        public ActionResult Create_RolesP()
        {
            return PartialView(new roles());
        }

        [HttpPost]
        public ActionResult Create_Roles(roles rolesModel)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                dbModel.roles.Add(rolesModel);
                dbModel.SaveChanges();
                ModelState.Clear();
                ViewBag.TheResult = true;

            }
            return View("Create_Roles");
        }
        [HttpPost]
        public ActionResult Create_RolesP(roles rolesModel)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                dbModel.roles.Add(rolesModel);
                dbModel.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
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