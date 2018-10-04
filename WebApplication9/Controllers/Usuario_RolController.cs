using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using PagedList;
using WebApplication9.Models;
using System.Net;

namespace WebApplication9.Controllers
{
    public class Usuario_RolController : Controller
    {
        // GET: Usuario_Rol
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index_URoles(int? page, string search, int? nn, string sortBy)
        {
            List<usuario_rol> ListaURol = new List<usuario_rol>();
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
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

                if (search != String.Empty && search != null)
                {
                    var strings = search.ToLower().Split(' ');
                    ListaURol = dbModel.usuario_rol.Include(x => x.usuario.contactos).Include(x => x.roles).ToList();

                    foreach (var splitString in strings)
                    {
                        ListaURol = ListaURol.Where(x => x.roles.rol.ToLower().Contains(splitString) || x.usuario.contactos.nombres.ToLower().Contains(splitString) || x.usuario.contactos.apellidos.ToLower().Contains(splitString)).ToList();
                    }
                }
                else
                {
                    ListaURol = dbModel.usuario_rol.Include(x => x.usuario.contactos).Include(x => x.roles).ToList();

                }
                ViewBag.SortName = string.IsNullOrEmpty(sortBy) ? "name_desc" : "";
                ViewBag.SortRol = sortBy == "rol" ? "rol_desc" : "rol";
                ViewBag.SortFechaI = sortBy == "fechaI" ? "fechaI_desc" : "fechaI";
                ViewBag.SortFechaT = sortBy == "fechaT" ? "fechaT_desc" : "fechaT";

                switch (sortBy)
                {
                    case "name_desc":
                        ListaURol = ListaURol.OrderByDescending(x => x.usuario.contactos.nombres).ThenBy(x => x.usuario.contactos.apellidos).ToList();
                        break;
                    case "rol_desc":
                        ListaURol = ListaURol.OrderByDescending(x => x.roles.rol).ToList();
                        break;
                    case "rol":
                        ListaURol = ListaURol.OrderBy(x => x.roles.rol).ToList();
                        break;
                    case "fechaI_desc":
                        ListaURol = ListaURol.OrderByDescending(x => x.fecha_inicio_rel).ToList();
                        break;
                    case "fechaI":
                        ListaURol = ListaURol.OrderBy(x => x.fecha_inicio_rel).ToList();
                        break;
                    case "fechaT_desc":
                        ListaURol = ListaURol.OrderByDescending(x => x.fecha_termino_rel).ToList();
                        break;
                    case "fechaT":
                        ListaURol = ListaURol.OrderBy(x => x.fecha_termino_rel).ToList();
                        break;
                    default:
                        ListaURol = ListaURol.OrderBy(x => x.usuario.contactos.nombres).ThenBy(x => x.usuario.contactos.apellidos).ToList();
                        break;

                }

                return View(ListaURol.ToList<usuario_rol>().ToPagedList(page ?? 1, nn ?? 10));
            }


        }
        public ActionResult Create_Usuario_rol()
        {
            usuario_rol model = new usuario_rol();
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {

                model.rolesList = db.roles.OrderBy(x => x.rol).ToList();
                model.usuarioList = db.usuario.Include(x => x.contactos).OrderBy(x => x.contactos.nombres).ThenBy(x => x.contactos.apellidos).ToList();


                //roles model2 = new roles();
                //model.roles.rolesl = db.roles;
            }
            return View(model);


        }
        public ActionResult Create_Usuario_rolP()
        {
            usuario_rol model = new usuario_rol();
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {

                model.rolesList = db.roles.ToList();
                model.usuarioList = db.usuario.Include(x => x.contactos).ToList();


                //roles model2 = new roles();
                //model.roles.rolesl = db.roles;
            }
            return PartialView(model);


        }
        public ActionResult Details_URoles(int id)
        {
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {

                if (id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                usuario_rol usuario_Rol = db.usuario_rol.Include(x => x.roles).Include(x => x.usuario).Where(x => x.id == id).FirstOrDefault();
                if (usuario_Rol == null)
                {
                    return HttpNotFound();
                }

                return PartialView(usuario_Rol);
            }
        }
        public ActionResult Delete_URol(int id, int? id_usuario)
        {
            ViewBag.id_usuario = id_usuario;
            usuario_rol urolModel = new usuario_rol();
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                urolModel = dbModel.usuario_rol.Include(x => x.roles).Include(x => x.usuario).Where(x => x.id == id).Include(x => x.usuario.contactos).FirstOrDefault();

            }

            return PartialView(urolModel);
        }


        [HttpPost]
        public ActionResult Create_Usuario_rol(usuario_rol urolModel)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                dbModel.usuario_rol.Add(urolModel);
                dbModel.SaveChanges();
                ModelState.Clear();


                urolModel = new usuario_rol();
                urolModel.rolesList = dbModel.roles.ToList();
                urolModel.usuarioList = dbModel.usuario.Include(x => x.contactos).ToList();
                ViewBag.TheResult = true;
            }
            return View("Create_Usuario_rol", urolModel);
        }

        [HttpPost]
        public ActionResult Create_Usuario_rolP(usuario_rol urolModel)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                urolModel.rolesList = dbModel.roles.ToList();
                urolModel.usuarioList = dbModel.usuario.ToList();
                dbModel.usuario_rol.Add(urolModel);
                dbModel.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult Edit_Usuario_Rol(int id, int? id_usuario2)
        {
            ViewBag.id_usuario2 = id_usuario2;
            usuario_rol u_rolesModel = new usuario_rol();
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                u_rolesModel = dbModel.usuario_rol.Where(x => x.id == id).FirstOrDefault();
                u_rolesModel.usuarioListI = dbModel.usuario.Select(x => new SelectListItem
                {
                    Text = x.contactos.nombres + " " + x.contactos.apellidos,
                    Value = x.id.ToString()


                }).ToList();
                u_rolesModel.rolesListI = dbModel.roles.Select(x => new SelectListItem
                {
                    Text = x.rol,
                    Value = x.id.ToString()


                }).ToList();
            }

            return PartialView(u_rolesModel);
        }
        [HttpPost]
        public ActionResult Edit_Usuario_Rol(usuario_rol u_rolesModel)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                dbModel.Entry(u_rolesModel).State = System.Data.Entity.EntityState.Modified;
                dbModel.SaveChanges();

            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete_URol(int id, FormCollection collection)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                usuario_rol urolModel = dbModel.usuario_rol.Where(x => x.id == id).Include(x => x.usuario.contactos).FirstOrDefault();

                int test = urolModel.id_usuario;
                int test2 = urolModel.id_rol;

                try
                {

                    dbModel.usuario_rol.Remove(urolModel);
                    dbModel.SaveChanges();

                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }


                catch (Exception ex)
                {
                    //antiguo metodo de eliminar BORRAR?
                    urolModel.id_usuario = test;
                    urolModel.id_rol = test2;
                    ModelState.AddModelError("error",
                   ex.Message + " No se pudo eliminar el registro");


                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }


            }

        }

    }
}