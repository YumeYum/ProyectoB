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
    public class Contacto_EmpresaController : Controller
    {
        // GET: Contacto_Empresa
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index_Empresa_Contacto(int? page)
        {
            List<contacto_empresa> ListacEmpresa = new List<contacto_empresa>();
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {

                return View(dbModel.contacto_empresa.Include(x => x.empresa).Include(x => x.contactos).OrderBy(x=>x.contactos.nombres).ThenBy(x=>x.contactos.apellidos).ToList<contacto_empresa>().ToPagedList(page ?? 1, 10));
            }

            //return View(ListacEmpresa);




        }
        public ActionResult Crear_contacto_empresa()
        {
            contacto_empresa model = new contacto_empresa();
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {
                //model.rolesList = db.roles.ToList();
                model.empresaListI = db.empresa.OrderBy(x=>x.razon_social).ToList();
                model.contactoListI = db.contactos.OrderBy(x=>x.nombres).ThenBy(x=>x.apellidos).ToList();


            }
            return PartialView(model);
        }
        public ActionResult Crear_contacto_empresaF()
        {
            contacto_empresa model = new contacto_empresa();
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {
                //model.rolesList = db.roles.ToList();
                model.empresaListI = db.empresa.OrderBy(x=>x.razon_social).ToList();
                model.contactoListI = db.contactos.OrderBy(x => x.nombres).ThenBy(x=>x.apellidos).ToList();


            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Crear_contacto_empresa(contacto_empresa contactoEmpresa)
        {


            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {


                //contactos contacto = new contactos();
                //contacto = contactoEmpresa.contactos;
                //contactoEmpresa.contactos.id = contactoEmpresa.id_contacto;
                //contactoEmpresa.contactos.curriculum = "";


                //dbModel.contactos.Add(contactoEmpresa.contactos);

                //contactoEmpresa.id_empresa = contactoEmpresa.id_empresa;
                //contactoEmpresa.fecha_inicio_rel = System.DateTime.Today;
                //contactoEmpresa.id_contacto = contactoEmpresa.id_contacto;
                dbModel.contacto_empresa.Add(contactoEmpresa);

                dbModel.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        [HttpPost]
        public ActionResult Crear_contacto_empresaF(contacto_empresa contactoEmpresa)
        {


            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {


                //contactos contacto = new contactos();
                //contacto = contactoEmpresa.contactos;
                // contactoEmpresa.contactos.id = contactoEmpresa.id_contacto;
                //contactoEmpresa.contactos.curriculum = "";


                //dbModel.contactos.Add(contactoEmpresa.contactos);

                //contactoEmpresa.id_empresa = contactoEmpresa.id_empresa;
                //contactoEmpresa.fecha_inicio_rel = System.DateTime.Today;
                //contactoEmpresa.id_contacto = contactoEmpresa.id_contacto;
                if (ModelState.IsValid)
                {
                    dbModel.contacto_empresa.Add(contactoEmpresa);
                    dbModel.SaveChanges();
                    return Redirect(Request.UrlReferrer.ToString());

                }
                else
                {
                    contactoEmpresa.empresaListI = dbModel.empresa.OrderBy(x=>x.razon_social).ToList();
                    contactoEmpresa.contactoListI = dbModel.contactos.OrderBy(x => x.nombres).ThenBy(x=>x.apellidos).ToList();
                    return Redirect(Request.UrlReferrer.ToString());
                }

            }
        }
        public ActionResult Details_Contacto_Empresa(int id)
        {
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {

                if (id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                contacto_empresa contacto_Empresa = db.contacto_empresa.Include(x => x.contactos).Include(x => x.empresa).Where(x => x.id == id).FirstOrDefault();
                if (contacto_Empresa == null)
                {
                    return HttpNotFound();
                }

                return PartialView(contacto_Empresa);
            }
        }

        public ActionResult Delete_Contacto_Empresa(int id)
        {
            contacto_empresa cEmpresaModel = new contacto_empresa();
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                cEmpresaModel = dbModel.contacto_empresa.Include(x => x.contactos).Include(x => x.empresa).Where(x => x.id == id).FirstOrDefault();

            }

            return PartialView(cEmpresaModel);
        }

        [HttpPost]
        public ActionResult Delete_Contacto_Empresa(int id, FormCollection collection)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                contacto_empresa cEmpresaModel = dbModel.contacto_empresa.Where(x => x.id == id).FirstOrDefault();

                int test = cEmpresaModel.id_contacto;
                int test2 = cEmpresaModel.id_empresa;

                try
                {

                    dbModel.contacto_empresa.Remove(cEmpresaModel);
                    dbModel.SaveChanges();

                    return Redirect(Request.UrlReferrer.ToString());
                }


                catch (Exception ex)
                {

                    cEmpresaModel.id_contacto = test;
                    cEmpresaModel.id_empresa = test2;
                    ModelState.AddModelError("error",
                   ex.Message + " No se pudo eliminar el registro");
                    return View(cEmpresaModel);
                }


            }

        }

        public ActionResult Edit_Contacto_Empresa(int id)
        {
            contacto_empresa cEmpresaModel = new contacto_empresa();
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                cEmpresaModel = dbModel.contacto_empresa.Where(x => x.id == id).FirstOrDefault();
                cEmpresaModel.contactosListSI = dbModel.contactos.Select(x => new SelectListItem
                {
                    Text = x.nombres +" "+x.apellidos,
                    Value = x.id.ToString()


                }).OrderBy(x=>x.Text).ToList();
                cEmpresaModel.empresaListSI = dbModel.empresa.Select(x => new SelectListItem
                {
                    Text = x.razon_social,
                    Value = x.id.ToString()


                }).OrderBy(x=>x.Text).ToList();
            }

            return PartialView(cEmpresaModel);
        }
        [HttpPost]
        public ActionResult Edit_Contacto_Empresa(contacto_empresa cEmpresaModel)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                dbModel.Entry(cEmpresaModel).State = System.Data.Entity.EntityState.Modified;
                dbModel.SaveChanges();

            }
            return Redirect(Request.UrlReferrer.ToString());
        }

    }

}