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
        public ActionResult Index_Actividades(int? page)
        {
            List<actividad> listaActividad = new List<actividad>();
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {




                return View(dbModel.actividad.Include(x=>x.oportunidades).Include(x=>x.usuario).ToList<actividad>().ToPagedList(page ?? 1, 10));
            }
        }

        public ActionResult Crear_Actividad()
        {
            actividad model = new actividad();
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {
                model.usuarioList = db.usuario.Include(x=>x.contactos).ToList();
                //model.empresaList = db.empresa.ToList();
                model.oList = db.oportunidades.ToList();
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
                model.usuarioList = db.usuario.Include(x=>x.contactos).ToList();

                return PartialView(model);
            }

        }
        public ActionResult Crear_Llamada()
        {
            actividad model = new actividad();
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {
                model.usuarioList = db.usuario.Include(x => x.contactos).ToList();
                //model.empresaList = db.empresa.ToList();
                model.oList = db.oportunidades.ToList();

                return PartialView(model);
            }
        }
        public ActionResult Crear_Mail()
        {
            actividad model = new actividad();
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {
                model.usuarioList = db.usuario.Include(x => x.contactos).ToList();
                //model.empresaList = db.empresa.ToList();
                model.oList = db.oportunidades.ToList();

                return PartialView(model);
            }
        }
        public ActionResult Crear_Reunion()
        {
            actividad model = new actividad();
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {
                model.usuarioList = db.usuario.Include(x => x.contactos).ToList();
                //model.empresaList = db.empresa.ToList();
                model.oList = db.oportunidades.ToList();

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
                aModel.tipo_actividad = "tarea";
                aModel.estado = "activa";
                dbModel.actividad.Add(aModel);
                dbModel.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
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
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        [HttpPost]
        public ActionResult Crear_Llamada(actividad actividadModel)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                actividadModel.creada_por = creada;
                actividadModel.tipo_actividad = "llamada";
                actividadModel.estado = "activa";
                dbModel.actividad.Add(actividadModel);
                dbModel.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }



        [HttpPost]
        public ActionResult Crear_Mail(actividad actividadModel)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                actividadModel.creada_por = creada;
                actividadModel.tipo_actividad = "mail";
                actividadModel.estado = "activa";
                dbModel.actividad.Add(actividadModel);
                dbModel.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public ActionResult Crear_Reunion(actividad actividadModel)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                actividadModel.creada_por = creada;
                actividadModel.tipo_actividad = "reunión";
                actividadModel.estado = "activa";
                dbModel.actividad.Add(actividadModel);
                dbModel.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
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
                actividadModel.estado = "cumplida";
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