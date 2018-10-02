using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using PagedList;
using WebApplication9.Models;
using MySql.Data.MySqlClient;
using System.Net;
using System.IO;

namespace WebApplication9.Controllers
{
    public class UsuariosController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }

        //[Authorize(Roles = "Administrator")]
        //nn = Número de registros
        public ActionResult Index_Usuarios(int? page, string search, string search2, ModelBindingContext bindingContext, int? nn, int? activ, string sortBy)
        {


            List<usuario> listaContactos = new List<usuario>();
            ViewBag.SearchL = new List<SelectListItem>()


                  {
                    new SelectListItem() {Text="Todos los campos", Value="todos"},
                  };
            ViewBag.Estados = new List<SelectListItem>()

                  {
                    new SelectListItem() {Text="Todas los usuarios", Value="0"},
                    new SelectListItem() {Text="Activos", Value="1"},
                    new SelectListItem() {Text="Inactivos", Value="2"},
                  };
            //Numero de páginas
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

            ViewBag.SortNC = string.IsNullOrEmpty(sortBy) ? "name_desc" : "";
            ViewBag.SortEstado = sortBy == "estado" ? "estado_desc" : "estado";
            ViewBag.SortFechaI = sortBy == "fecha_inicio" ? "fecha_inicio_desc" : "fecha_inicio";
            ViewBag.SortFechaT = sortBy == "fecha_termino" ? "fecha_termino_desc" : "fecha_termino";
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {

                        if (search == String.Empty || search == null)
                        {

                            listaContactos = dbModel.usuario.Include(x => x.contactos).ToList();
                            
                            //return Redirect("Index_Usuarios?nn="+ nn );

                        }
                        else
                        {
                            listaContactos = dbModel.usuario.Where(x => x.estado.Contains(search) || x.contactos.nombres.Contains(search) || x.contactos.apellidos.Contains(search)).Include(x => x.contactos).ToList();
                           

                        }

                
                switch (activ)
                {
                    case 1:
                        listaContactos = listaContactos.Where(x => x.estado == "activo").ToList();
                        break;
                    case 2:
                        listaContactos = listaContactos.Where(x => x.estado == "inactivo").ToList();
                        break;

                    default:
                        break;
                }
                switch (sortBy)
                {
                    case "name_desc":
                        listaContactos = listaContactos.OrderByDescending(x => x.contactos.nombres).ThenBy(x => x.contactos.apellidos).ToList();
                        break;
                    case "estado_desc":
                        listaContactos = listaContactos.OrderByDescending(x => x.estado).ToList();
                        break;
                    case "estado":
                        listaContactos = listaContactos.OrderBy(x => x.estado).ToList();
                        break;
                    case "fecha_inicio_desc":
                        listaContactos = listaContactos.OrderByDescending(x => x.fecha_inicio_rel).ToList();
                        break;
                    case "fecha_inicio":
                        listaContactos = listaContactos.OrderBy(x => x.fecha_inicio_rel).ToList();
                        break;
                    case "fecha_termino_desc":
                        listaContactos = listaContactos.OrderByDescending(x => x.fecha_termino_rel).ToList();
                        break;
                    case "fecha_termino":
                        listaContactos = listaContactos.OrderBy(x => x.fecha_termino_rel).ToList();
                        break;
                    default:
                        listaContactos = listaContactos.OrderBy(x => x.contactos.nombres).ThenBy(x=>x.contactos.apellidos).ToList();
                        break;
                }


                return View(listaContactos.ToList<usuario>().ToPagedList(page ?? 1, nn ?? 10));
            }
        }

        // GET: Proyectob/Details/5
        public ActionResult Details2_Usuario(int id)
        {

            //List<usuario_rol> uRolesM = new List<usuario_rol>();

            usuario User = new usuario();

            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                User = dbModel.usuario.Where(x => x.id == id).Include(x => x.contactos).Include(x=>x.contactos.contacto_empresa.Select(z=>z.empresa)).FirstOrDefault();
                User.uRolesM = dbModel.usuario_rol.Where(x => x.id_usuario == id).Include(x=>x.usuario.contactos).Include(x => x.usuario).Include(x => x.roles).ToList<usuario_rol>();
                User.Ops = dbModel.oportunidades.Where(x => x.usuario.id == id).ToList();
                User.Activs = dbModel.actividad.Where(x => x.id_contacto == id).ToList();

            }

            return PartialView(User);
        }


        // GET: Proyectob/Create

        public ActionResult Create_Usuario()
        {
            ViewBag.EstadoL = new List<SelectListItem>()

                  {
                    new SelectListItem() {Text="Activo", Value="activo"},
                    new SelectListItem() {Text="Inactivo", Value="inactivo"}
                  };
            usuario model = new usuario();
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {
                model.contactoList = db.contactos.OrderBy(x=>x.nombres).ToList();
                model.rolesList = db.roles.OrderBy(x=>x.rol).ToList();
            }
            return View(model);
        }

        public ActionResult CreateP_Usuario()
        {
            ViewBag.EstadoL = new List<SelectListItem>()

                  {
                    new SelectListItem() {Text="Activo", Value="activo"},
                    new SelectListItem() {Text="Inactivo", Value="inactivo"}
                  };
            usuario model = new usuario();
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {
                model.contactoList = db.contactos.OrderBy(x=>x.nombres).ThenBy(x=>x.apellidos).ToList();
                model.rolesList = db.roles.OrderBy(x => x.rol).ToList();
            }
            return PartialView(model);
        }

        // POST: Proyectob/Create
        [HttpPost]
        public ActionResult Create_Usuario(usuario usuarioModel, string EstadoL)
        {
            ViewBag.EstadoL = new List<SelectListItem>()

                  {
                    new SelectListItem() {Text="Activo", Value="activo"},
                    new SelectListItem() {Text="Inactivo", Value="inactivo"}
                  };
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {



                if (ModelState.IsValid)
                {
                    if (EstadoL == "inactivo")
                    {
                        usuarioModel.fecha_termino_rel = System.DateTime.Now;
                        usuarioModel.estado = "inactivo";

                    }
                    dbModel.usuario.Add(usuarioModel);
                    foreach (var item in usuarioModel.rolesList.Where(c => c.IsSelected))
                    {
                        usuario_rol urol = new usuario_rol();
                        urol.id = urol.id;
                        urol.id_rol = item.id;
                        urol.id_usuario = usuarioModel.id;
                        urol.fecha_inicio_rel = System.DateTime.Now;
                        if (EstadoL == "inactivo")
                        {
                            urol.fecha_termino_rel = System.DateTime.Now;
                        }

                        dbModel.usuario_rol.Add(urol);

                    }

                    dbModel.SaveChanges();
                    ModelState.Clear();

                    usuarioModel = new usuario();
                    usuarioModel.contactoList = dbModel.contactos.OrderBy(x => x.nombres).ThenBy(x => x.apellidos).ToList();
                    usuarioModel.rolesList = dbModel.roles.OrderBy(x => x.rol).ToList();
                    ViewBag.TheResult = true;
                    return View("Create_Usuario",usuarioModel);

                }
                return View("Create_Usuario", usuarioModel);

            }

        }
        [HttpPost]
        public ActionResult CreateP_Usuario(usuario usuarioModel, string EstadoL)
        {
            ViewBag.EstadoL = new List<SelectListItem>()

                  {
                    new SelectListItem() {Text="Activo", Value="activo"},
                    new SelectListItem() {Text="Inactivo", Value="inactivo"}
                  };
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                usuarioModel.contactoList = dbModel.contactos.OrderBy(x=>x.nombres).ThenBy(x=>x.apellidos).ToList();

                if (ModelState.IsValid)
         
                {

                    if (EstadoL == "inactivo")
                    {
                        usuarioModel.fecha_termino_rel = System.DateTime.Now;
                        usuarioModel.estado = "inactivo";

                    }
                    dbModel.usuario.Add(usuarioModel);
                    foreach (var item in usuarioModel.rolesList.Where(c => c.IsSelected))
                    {
                        usuario_rol urol = new usuario_rol();
                        urol.id = urol.id;
                        urol.id_rol = item.id;
                        urol.id_usuario = usuarioModel.id;
                        urol.fecha_inicio_rel = System.DateTime.Now;
                        urol.fecha_termino_rel = System.DateTime.Now;

                        if (EstadoL == "inactivo")
                        {
                            urol.fecha_termino_rel = System.DateTime.Now;
                        }

                        dbModel.usuario_rol.Add(urol);

                    }

                    dbModel.SaveChanges();
                    return Redirect(Request.UrlReferrer.ToString());

                }
                usuarioModel.rolesList = dbModel.roles.OrderBy(x=>x.rol).ToList();
                return View("Create_Usuario", usuarioModel);

            }
        }

        // GET: Proyectob/Edit/5
        public ActionResult Edit_Usuario(int id)
        {
            usuario usuarioModel = new usuario();
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                usuarioModel = dbModel.usuario.Where(x => x.id == id).FirstOrDefault();
                usuarioModel.contactoListI = dbModel.contactos.Select(x => new SelectListItem
                {
                    Text = x.nombres + " " + x.apellidos,
                    Value = x.id.ToString()


                }).OrderBy(x=>x.Text).ToList();

            }

            return PartialView(usuarioModel);
        }


        // POST: Proyectob/Edit/5
        [HttpPost]
        public ActionResult Edit_Usuario(usuario usuarioModel)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                dbModel.Entry(usuarioModel).State = System.Data.Entity.EntityState.Modified;
                dbModel.SaveChanges();
                if (usuarioModel.estado =="inactivo")
                {
                    List<usuario_rol> urol = new List<usuario_rol>();
                    urol = dbModel.usuario_rol.Where(x => x.id_usuario == usuarioModel.id).ToList();

                    foreach (var item in urol)
                    {
                        item.fecha_termino_rel = usuarioModel.fecha_termino_rel;
                    }
                    dbModel.SaveChanges();

                }

            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        // GET: Proyectob/Delete/5
        public ActionResult Delete_Usuario(int id)
        {
            usuario usuarioModel = new usuario();
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                usuarioModel = dbModel.usuario.Where(x => x.id == id).FirstOrDefault();

            }

            return PartialView(usuarioModel);
        }

        // POST: Proyectob/Delete/5
        [HttpPost]
        public ActionResult Delete_Usuario(int id, FormCollection collection)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                usuario usuarioModel = dbModel.usuario.Where(x => x.id == id).FirstOrDefault();
                List<usuario_rol> urol = new List<usuario_rol>();


                int test = usuarioModel.id_contacto;
                try
                {
                    urol = dbModel.usuario_rol.Where(x => x.id_usuario == usuarioModel.id).ToList();

                    foreach (var item in urol)
                    {
                        dbModel.usuario_rol.Remove(item);
                    }

                    dbModel.usuario.Remove(usuarioModel);
                    dbModel.SaveChanges();
                    return Redirect(Request.UrlReferrer.ToString());
                }


                catch (Exception ex)
            {

                    usuarioModel.id_contacto = test;
                    ModelState.AddModelError("error",
                   ex.Message + " No se pudo eliminar el registro" );
                return View(usuarioModel);
            }


            }

        }




    }
}