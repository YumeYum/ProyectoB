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
    public class EmpresasController : Controller
    {
        // GET: Empresas
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Crear_Empresa()
        {

            return PartialView(new empresa());
        }

        public ActionResult Crear_EmpresaF()
        {

            return View(new empresa());
        }
        public ActionResult Index_Empresa(int? page, string search)
        {
            List<empresa> listaEmpresas = new List<empresa>();
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                if (search!= null && search!= string.Empty)
                {
                    listaEmpresas = dbModel.empresa.Where(x => x.razon_social.Contains(search)||x.comuna.Contains(search)||x.convenio.Contains(search)||x.numero.ToString().Contains(search)||x.resto_direccion.Contains(search)||x.rubro.Contains(search)||x.rut.Contains(search)||x.zona.Contains(search)).ToList();
                }

                else
                {
                     listaEmpresas = dbModel.empresa.Include(x => x.contacto_empresa.Select(z => z.oportunidades)).OrderBy(x => x.razon_social).ToList<empresa>();

                }




                foreach (var item in listaEmpresas.ToList())
                {
                    int count = dbModel.oportunidades.Where(x => x.contacto_empresa.id_empresa == item.id).Count();
                    item.OposN = count;
                }

                return View(listaEmpresas.ToPagedList(page ?? 1, 10));
            }
        }




        [HttpPost]
        public ActionResult Crear_Empresa(empresa empresaModel)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                dbModel.empresa.Add(empresaModel);
                dbModel.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());
            }
            //return RedirectToAction("/Index_Empresa");
        }
        [HttpPost]
        public ActionResult Crear_EmpresaF(empresa empresaModel)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                if (ModelState.IsValid)
                {
                    dbModel.empresa.Add(empresaModel);
                    dbModel.SaveChanges();
                    return Redirect(Request.UrlReferrer.ToString());

                }
                else
                {
                    return View("Crear_EmpresaF", empresaModel);

                }
            }
        }

        public ActionResult Edit_Empresa(int id)
        {
            empresa empresaModel = new empresa();
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                empresaModel = dbModel.empresa.Where(x => x.id == id).FirstOrDefault();


            }

            return PartialView(empresaModel);
        }

        [HttpPost]
        public ActionResult Edit_Empresa(empresa empresaModel)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                dbModel.Entry(dbModel).State = System.Data.Entity.EntityState.Modified;
                dbModel.SaveChanges();

            }
            return Redirect(Request.UrlReferrer.ToString());
        }


        // GET: Proyectob/Delete/5
        public ActionResult Delete_Empresa(int id)
        {
            empresa empresaModel = new empresa();
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                empresaModel = dbModel.empresa.Where(x => x.id == id).FirstOrDefault();

            }

            return PartialView(empresaModel);
        }

        // POST: Proyectob/Delete/5
        [HttpPost]
        public ActionResult Delete_Empresa(int id, FormCollection collection)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                empresa empresaModel = dbModel.empresa.Where(x => x.id == id).FirstOrDefault();

                try
                {

                    dbModel.empresa.Remove(empresaModel);
                    dbModel.SaveChanges();

                    return Redirect(Request.UrlReferrer.ToString());
                }


                catch (Exception ex)
                {

                    ModelState.AddModelError("error",
                   ex.Message + " No se pudo eliminar el registro");
                    return View(empresaModel);
                }


            }

        }

        public ActionResult Details_Empresa(int id)
        {
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {
                //List<contacto_empresa> contactosM = new List<contacto_empresa>();


                empresa Empresa = new empresa();
                //List<oportunidades> Ops = new List<oportunidades>();
                using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
                {



                    Empresa = dbModel.empresa.Where(x => x.id == id).FirstOrDefault();
                    Empresa.listaContactos = dbModel.contactos.OrderBy(x=>x.nombres).ThenBy(x=>x.apellidos).ToList();
                    Empresa.contactosM = dbModel.contacto_empresa.Where(x => x.id_empresa == id).Include(x=>x.contactos).Include(x=>x.empresa).Include(x=>x.oportunidades).ToList<contacto_empresa>();
                    Empresa.Opos = dbModel.oportunidades.Include(x=>x.usuario.contactos).Where(x=>x.contacto_empresa.empresa.id == id ).ToList();
                    Empresa.Activs = dbModel.actividad.Include(x=>x.usuario.contactos).Where(x => x.oportunidades.contacto_empresa.empresa.id == id).ToList();

                }

                return PartialView(Empresa);
            }
        }


        [HttpPost]
        public ActionResult Details_Empresa(int id, int id_contacto)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                //Creando Contacto Empresa
                contacto_empresa cE = new contacto_empresa();
                cE.id_empresa = id;
                cE.id_contacto =  id_contacto;


                dbModel.contacto_empresa.Add(cE);
                dbModel.SaveChanges();
                return Json(new { Url = "/Empresas/Details_Empresa/"+id});
            }
        }
    }

}