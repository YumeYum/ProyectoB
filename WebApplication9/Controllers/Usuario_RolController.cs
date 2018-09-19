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

        public ActionResult Index_URoles(int? page)
        {
            List<usuario_rol> ListaURol = new List<usuario_rol>();
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {

                return View(dbModel.usuario_rol.Include(x=>x.usuario.contactos).Include(x=>x.roles).ToList<usuario_rol>().ToPagedList(page ?? 1, 10));
            }


        }
        public ActionResult Create_Usuario_rol()
        {
            usuario_rol model = new usuario_rol();
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {

                model.rolesList = db.roles.OrderBy(x=>x.rol).ToList();
                model.usuarioList = db.usuario.Include(x=>x.contactos).OrderBy(x=>x.contactos.nombres).ThenBy(x=>x.contactos.apellidos).ToList();


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
                usuario_rol usuario_Rol = db.usuario_rol.Include(x=>x.roles).Include(x=>x.usuario).Where(x=> x.id == id).FirstOrDefault();
                if (usuario_Rol == null)
                {
                    return HttpNotFound();
                }

                return PartialView(usuario_Rol);
            }
        }
        public ActionResult Delete_URol(int id)
        {
            usuario_rol urolModel = new usuario_rol();
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                urolModel = dbModel.usuario_rol.Include(x => x.roles).Include(x=>x.usuario).Where(x => x.id == id).FirstOrDefault();

            }

            return PartialView(urolModel);
        }


        [HttpPost]
        public ActionResult Create_Usuario_rol(usuario_rol urolModel)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                urolModel.rolesList = dbModel.roles.ToList();
                urolModel.usuarioList = dbModel.usuario.ToList();
                dbModel.usuario_rol.Add(urolModel);
                dbModel.SaveChanges();
            }
            return RedirectToAction("/Index_URoles");
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
            return RedirectToAction("/Index_URoles");
        }
        public ActionResult Edit_Usuario_Rol(int id)
        {
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
            return RedirectToAction("/Index_URoles");
        }

        [HttpPost]
        public ActionResult Delete_URol(int id, FormCollection collection)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                usuario_rol urolModel = dbModel.usuario_rol.Where(x => x.id == id).FirstOrDefault();

                int test = urolModel.id_usuario;
                int test2 = urolModel.id_rol;

                try
                {

                    dbModel.usuario_rol.Remove(urolModel);
                    dbModel.SaveChanges();

                    return RedirectToAction("/Index_URoles");
                }


                catch (Exception ex)
                {

                    urolModel.id_usuario = test;
                    urolModel.id_rol = test2;
                    ModelState.AddModelError("error",
                   ex.Message + " No se pudo eliminar el registro");
                    return View(urolModel);
                }


            }

        }

    }
}