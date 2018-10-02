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
    public class ActividadesController : Controller
    {
        string creada = System.Web.HttpContext.Current.Session["Name"].ToString();

        // GET: Actividades
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index_Actividades(int? page,string search, int? nn, string sortBy)
        {
            List<actividad> listaActividad = new List<actividad>();
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

                if (search != null && search != string.Empty)
                {
                    listaActividad = dbModel.actividad.Where(x => x.creada_por.Contains(search) || x.descripcion.Contains(search) || x.estado.Contains(search) || x.titulo.ToString().Contains(search) || x.tipo_actividad.Contains(search) || x.oportunidades.tema.Contains(search) || x.usuario.contactos.nombres.Contains(search) || x.usuario.contactos.apellidos.Contains(search)).Include(x => x.oportunidades).Include(x => x.usuario.contactos).ToList();
                }

                else
                {
                    listaActividad = dbModel.actividad.Include(x => x.oportunidades).Include(x => x.usuario.contactos).ToList<actividad>();

                }


                ViewBag.SortCreadaPor = string.IsNullOrEmpty(sortBy) ? "creada_por_desc" : "";
                ViewBag.SortFechaI = sortBy == "fechaI" ? "fechaI_desc" : "fechaI";
                ViewBag.SortFechaV = sortBy == "fechaV" ? "fechaV_desc" : "fechaV";
                ViewBag.SortTitulo = sortBy == "titulo" ? "titulo_desc" : "titulo";
                ViewBag.SortDescr = sortBy == "descripcion" ? "descripcion_descr" : "descripcion";
                ViewBag.SortCreadaPor = sortBy == "creada_por" ? "creada_por_desc" : "creada_por";
                ViewBag.SortEstado = sortBy == "estado" ? "estado_desc" : "estado";
                ViewBag.SorTipoAct = sortBy == "tipo_actividad" ? "tipo_actividad_desc" : "tipo_actividad";
                ViewBag.SortTema = sortBy == "tema" ? "tema_desc"  : "tema";
                ViewBag.SortContacto = sortBy == "contacto" ? "contacto_desc" : "contacto";
                switch (sortBy)
                {
                    case "creada_por_desc":
                        listaActividad = listaActividad.OrderByDescending(x => x.creada_por).ToList();
                        break;
                    case "fechaI":
                        listaActividad = listaActividad.OrderBy(x => x.fecha_inicio).ToList();
                        break;
                    case "fechaI_desc":
                        listaActividad = listaActividad.OrderByDescending(x => x.fecha_inicio).ToList();
                        break;
                    case "fechaV":
                        listaActividad = listaActividad.OrderBy(x => x.fecha_vencimiento).ToList();
                        break;
                    case "fechaV_desc":
                        listaActividad = listaActividad.OrderByDescending(x => x.fecha_vencimiento).ToList();
                        break;
                    case "titulo":
                        listaActividad = listaActividad.OrderBy(x => x.titulo).ToList();
                        break;
                    case "titulo_desc":
                        listaActividad = listaActividad.OrderByDescending(x => x.titulo).ToList();
                        break;
                    case "estado":
                        listaActividad = listaActividad.OrderBy(x => x.estado).ToList();
                        break;
                    case "estado_desc":
                        listaActividad = listaActividad.OrderByDescending(x => x.estado).ToList();
                        break;
                    case "tipo_actividad":
                        listaActividad = listaActividad.OrderBy(x => x.tipo_actividad).ToList();
                        break;
                    case "tipo_actividad_desc":
                        listaActividad = listaActividad.OrderByDescending(x => x.tipo_actividad).ToList();
                        break;
                    case "tema":
                        listaActividad = listaActividad.OrderBy(x => x.oportunidades.tema).ToList();
                        break;
                    case "tema_desc":
                        listaActividad = listaActividad.OrderByDescending(x => x.oportunidades.tema).ToList();
                        break;
                    case "contacto":
                        listaActividad = listaActividad.OrderBy(x => x.usuario.contactos.nombres).ThenBy(x=>x.usuario.contactos.apellidos).ToList();
                        break;
                    case "contacto_desc":
                        listaActividad = listaActividad.OrderByDescending(x => x.usuario.contactos.nombres).ThenBy(x=>x.usuario.contactos.apellidos).ToList();
                        break;
                    case "descripcion":
                        listaActividad = listaActividad.OrderBy(x => x.descripcion).ToList();
                        break;
                    case "descripcion_descr":
                        listaActividad = listaActividad.OrderByDescending(x => x.descripcion).ToList();
                        break;
                    default:
                        listaActividad = listaActividad.OrderBy(x => x.creada_por).ToList();
                        break;
                }


                return View(listaActividad.ToList<actividad>().ToPagedList(page ?? 1, nn ?? 10));
            }
        }

        public ActionResult Crear_Actividad(int? id_op)
        {
            actividad model = new actividad();
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {
                model.usuarioList = db.usuario.Where(x=>x.estado=="activo").OrderBy(x=>x.contactos.nombres).ThenBy(x=>x.contactos.apellidos).Include(x=>x.contactos).ToList();
                //model.empresaList = db.empresa.ToList();
                model.oList = db.oportunidades.ToList();

                model.id_oportunidad = id_op ?? default(int);




                return PartialView(model);
            }

        }

        public ActionResult Crear_ActividadF()
        {
            actividad model = new actividad();
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {
                model.oList = db.oportunidades.ToList();
                //model.empresaList = db.empresa.ToList();
                model.usuarioList = db.usuario.Where(x=>x.estado=="activo").OrderBy(x => x.contactos.nombres).ThenBy(x => x.contactos.apellidos).Include(x=>x.contactos).ToList();

                return PartialView(model);
            }

        }
        public ActionResult Crear_Llamada(int? id_op)
        {
            actividad model = new actividad();
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {
                model.usuarioList = db.usuario.Where(x=>x.estado=="activo").OrderBy(x => x.contactos.nombres).ThenBy(x => x.contactos.apellidos).Include(x => x.contactos).ToList();
                //model.empresaList = db.empresa.ToList();
                model.oList = db.oportunidades.ToList();

                model.id_oportunidad = id_op ?? default(int);


                return PartialView(model);
            }
        }
        public ActionResult Crear_Mail(int? id_op)
        {
            actividad model = new actividad();
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {
                model.usuarioList = db.usuario.Where(x => x.estado == "activo").OrderBy(x => x.contactos.nombres).ThenBy(x => x.contactos.apellidos).Include(x => x.contactos).ToList();
                //model.empresaList = db.empresa.ToList();
                model.oList = db.oportunidades.ToList();

                model.id_oportunidad = id_op ?? default(int);


                return PartialView(model);
            }
        }
        public ActionResult Crear_Reunion(int? id_op)
        {
            actividad model = new actividad();
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {
                model.usuarioList = db.usuario.Where(x => x.estado == "activo").OrderBy(x => x.contactos.nombres).ThenBy(x => x.contactos.apellidos).Include(x => x.contactos).ToList();
                //model.empresaList = db.empresa.ToList();
                model.oList = db.oportunidades.ToList();

                model.id_oportunidad = id_op ?? default(int);


                return PartialView(model);
            }
        }
        public ActionResult Finalizar_Actividad(int id)
        {
            actividad aModel = new actividad();
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                aModel = dbModel.actividad.Where(x => x.id == id).FirstOrDefault();
                //aModel.fecha_inicio = System.DateTime.Now;
                aModel.ContactoList = dbModel.contactos.Select(x => new SelectListItem
                {
                    Text = x.nombres,
                    Value = x.id.ToString()


                }).ToList();


            }

            return PartialView(aModel);
        }


        [HttpPost]
        public ActionResult Crear_Actividad(actividad aModel)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                aModel.creada_por = creada;
                aModel.estado = "activa";
                dbModel.actividad.Add(aModel);
                dbModel.SaveChanges();
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Crear_ActividadF(actividad aModel)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {

                aModel.creada_por = creada;
                aModel.tipo_actividad = "tarea";
                aModel.estado = "activa";

                dbModel.actividad.Add(aModel);
                dbModel.SaveChanges();
                aModel = new actividad();
                aModel.oList = dbModel.oportunidades.ToList();
                //model.empresaList = db.empresa.ToList();
                aModel.usuarioList = dbModel.usuario.Where(x => x.estado == "activo").Include(x => x.contactos).ToList();

                ModelState.Clear();


                ViewBag.TheResult = true;
            }
            return View("Crear_ActividadF", aModel);
        }

        [HttpPost]
        public ActionResult Finalizar_Actividad(actividad actividadModel)
        {
            //string uOwner;
            //DateTime fechaC;
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                //uOwner = actividadModel.id_usuario_owner;
                //fechaC = actividadModel.fecha_inicio;
                //actividadModel = dbModel.actividad.Where(x => x.id == actividadModel.id).FirstOrDefault();
                actividadModel.estado = "Finalizada";
                //actividadModel.id_usuario_owner = uOwner;
                //actividadModel.fecha_inicio = fechaC;
                dbModel.Entry(actividadModel).State = System.Data.Entity.EntityState.Modified;
                dbModel.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult Details_Actividad(int id)
        {
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {

                if (id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                actividad acti = db.actividad.Include(x => x.usuario).Include(x => x.oportunidades).Where(x => x.id == id).FirstOrDefault();
                if (acti == null)
                {
                    return HttpNotFound();
                }

                return PartialView(acti);
            }
        }
        public ActionResult Edit_Actividad(int id)
        {
            actividad aModel = new actividad();

            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                aModel = dbModel.actividad.Where(x => x.id == id).FirstOrDefault();

                aModel.opList = dbModel.oportunidades.Select(x => new SelectListItem
                {
                    Text = x.id.ToString(),
                    Value = x.id.ToString()


                }).ToList();

                aModel.UsuarioList = dbModel.usuario.Select(x => new SelectListItem
                {
                    Text = x.contactos.nombres + " " + x.contactos.apellidos,
                    Value = x.id.ToString()


                }).ToList();
            }

            return PartialView(aModel);
        }

        [HttpPost]
        public ActionResult Edit_Actividad(actividad aModel)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {

                dbModel.Entry(aModel).State = System.Data.Entity.EntityState.Modified;
                dbModel.SaveChanges();

            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        [HttpPost]
        public ActionResult Delete_Actividad(int id, FormCollection collection)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                actividad aModel = dbModel.actividad.Where(x => x.id == id).FirstOrDefault();

                int test = aModel.id_contacto;
                int test2 = aModel.id_oportunidad;

                try
                {

                    dbModel.actividad.Remove(aModel);
                    dbModel.SaveChanges();

                    return Redirect(Request.UrlReferrer.ToString());
                }


                catch (Exception ex)
                {

                    aModel.id_contacto = test;
                    aModel.id_oportunidad = test2;
                    ModelState.AddModelError("error",
                   ex.Message + " No se pudo eliminar el registro");
                    return View(aModel);
                }


            }

        }
        public ActionResult Delete_Actividad(int id)
        {
            actividad aModel = new actividad();
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                aModel = dbModel.actividad.Where(x => x.id == id).Include(x => x.usuario).Include(x => x.oportunidades).FirstOrDefault();

            }

            return PartialView(aModel);
        }


    }
}