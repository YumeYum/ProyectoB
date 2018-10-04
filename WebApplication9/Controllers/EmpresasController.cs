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
        public ActionResult Index_Empresa(int? page, string search, int? nn, string sortBy, string convenio, int? opsn)
        {
            List<empresa> listaEmpresas = new List<empresa>();
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
                ViewBag.Convenio = new List<SelectListItem>()

                  {
                    new SelectListItem() {Text="Convenios: Todos", Value= "0"},
                    new SelectListItem() {Text="Sí", Value="1"},
                    new SelectListItem() {Text="No", Value="2"},


                  };


                if (search != null && search != string.Empty)
                {
                    var strings = search.ToLower().Split(' ');
                    listaEmpresas = dbModel.empresa.ToList();

                    foreach (var splitString in strings)
                    {
                        listaEmpresas = listaEmpresas.Where(x => x.razon_social.ToLower().Contains(splitString) || x.comuna.ToLower().Contains(splitString) || x.convenio.ToLower().Contains(splitString) || x.numero.ToString().ToLower().Contains(splitString) || x.resto_direccion.ToLower().Contains(splitString) || x.rubro.ToLower().Contains(splitString) || x.rut.ToLower().Contains(splitString) || x.zona.ToLower().Contains(splitString)).ToList();
                    }
                }

                else
                {
                    listaEmpresas = dbModel.empresa.Include(x => x.contacto_empresa.Select(z => z.oportunidades)).OrderBy(x => x.razon_social).ToList<empresa>();

                }


                ViewBag.SortRSocial = string.IsNullOrEmpty(sortBy) ? "razon_social_desc" : "";
                ViewBag.SortRut = sortBy == "rut" ? "rut_desc" : "rut";
                ViewBag.SortRubro = sortBy == "rubro" ? "rubro_desc" : "rubro";
                ViewBag.SortCalle = sortBy == "calle" ? "calle_desc" : "calle";
                ViewBag.SortNumero = sortBy == "numero" ? "numero_desc" : "numero";
                ViewBag.SortRDireccion = sortBy == "rdireccion" ? "rdireccion_desc" : "rdireccion";
                ViewBag.SortComuna = sortBy == "comuna" ? "comuna_desc" : "comuna";
                ViewBag.SortCTG = sortBy == "ctg" ? "ctg_desc" : "ctg";
                ViewBag.SortConvenio = sortBy == "convenio" ? "convenio_desc" : "convenio";
                ViewBag.SortZona = sortBy == "zona" ? "zona_desc" : "zona";
                ViewBag.SortCuposO = sortBy == "cupos_o" ? "cupos_o_desc" : "cupos_o";
                ViewBag.SortCuposD = sortBy == "cupos_d" ? "cupos_d_desc" : "cupos_d";
                ViewBag.SortOps = sortBy == "ops" ? "ops_desc" : "ops";

                foreach (var item in listaEmpresas.ToList())
                {
                    int count = dbModel.oportunidades.Where(x => x.contacto_empresa.id_empresa == item.id).Count();
                    item.OposN = count;
                }
                switch (convenio)
                {
                    case "1":
                        listaEmpresas = listaEmpresas.Where(x => x.convenio == "Sí").ToList();
                        break;
                    case "2":
                        listaEmpresas = listaEmpresas.Where(x => x.convenio == "No").ToList();
                        break;
                    default:
                        listaEmpresas = listaEmpresas.ToList();
                        break;
                }

                if (opsn>0)
                {
                    listaEmpresas = listaEmpresas.Where(x => x.OposN <= opsn && x.OposN>0).ToList();
                }

                switch (sortBy)
                {
                    case "razon_social_desc":
                        listaEmpresas = listaEmpresas.OrderByDescending(x => x.razon_social).ToList();
                        break;
                    case "rut":
                        listaEmpresas = listaEmpresas.OrderBy(x => x.rut).ToList();
                        break;
                    case "rut_desc":
                        listaEmpresas = listaEmpresas.OrderByDescending(x => x.rut).ToList();
                        break;
                    case "rubro_desc":
                        listaEmpresas = listaEmpresas.OrderByDescending(x => x.rubro).ToList();
                        break;
                    case "rubro":
                        listaEmpresas = listaEmpresas.OrderBy(x => x.rubro).ToList();
                        break;
                    case "calle_desc":
                        listaEmpresas = listaEmpresas.OrderByDescending(x => x.calle).ToList();
                        break;
                    case "calle":
                        listaEmpresas = listaEmpresas.OrderBy(x => x.calle).ToList();
                        break;
                    case "numero":
                        listaEmpresas = listaEmpresas.OrderBy(x => x.numero).ToList();
                        break;
                    case "numero_desc":
                        listaEmpresas = listaEmpresas.OrderByDescending(x => x.numero).ToList();
                        break;
                    case "rdireccion":
                        listaEmpresas = listaEmpresas.OrderBy(x => x.resto_direccion).ToList();
                        break;
                    case "rdireccion_desc":
                        listaEmpresas = listaEmpresas.OrderByDescending(x => x.resto_direccion).ToList();
                        break;
                    case "comuna":
                        listaEmpresas = listaEmpresas.OrderBy(x => x.comuna).ToList();
                        break;
                    case "comuna_desc":
                        listaEmpresas = listaEmpresas.OrderByDescending(x => x.comuna).ToList();
                        break;
                    case "ctg":
                        listaEmpresas = listaEmpresas.OrderBy(x => x.ctg_empleados).ToList();
                        break;
                    case "ctg_desc":
                        listaEmpresas = listaEmpresas.OrderByDescending(x => x.ctg_empleados).ToList();
                        break;
                    case "convenio":
                        listaEmpresas = listaEmpresas.OrderBy(x => x.convenio).ToList();
                        break;
                    case "convenio_desc":
                        listaEmpresas = listaEmpresas.OrderByDescending(x => x.convenio).ToList();
                        break;
                    case "zona":
                        listaEmpresas = listaEmpresas.OrderBy(x => x.zona).ToList();
                        break;
                    case "zona_desc":
                        listaEmpresas = listaEmpresas.OrderByDescending(x => x.zona).ToList();
                        break;
                    case "ops":
                        listaEmpresas = listaEmpresas.OrderBy(x => x.OposN).ToList();
                        break;
                    case "ops_desc":
                        listaEmpresas = listaEmpresas.OrderByDescending(x => x.OposN).ToList();
                        break;
                    default:
                        listaEmpresas = listaEmpresas.OrderBy(x => x.razon_social).ToList();
                        break;
                }







                return View(listaEmpresas.ToPagedList(page ?? 1, nn ?? 10));
            }
        }




        [HttpPost]
        public ActionResult Crear_Empresa(empresa empresaModel)

        {

            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                dbModel.empresa.Add(empresaModel);
                dbModel.SaveChanges();
                return Json(new { success = true}, JsonRequestBehavior.AllowGet);
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
                    ModelState.Clear();
                    ViewBag.TheResult = true;
                    return View("Crear_EmpresaF");

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
                    Empresa.listaContactos = dbModel.contactos.OrderBy(x => x.nombres).ThenBy(x => x.apellidos).ToList();
                    Empresa.contactosM = dbModel.contacto_empresa.Where(x => x.id_empresa == id).Include(x => x.contactos).Include(x => x.empresa).Include(x => x.oportunidades).ToList<contacto_empresa>();
                    Empresa.Opos = dbModel.oportunidades.Include(x => x.usuario.contactos).Where(x => x.contacto_empresa.empresa.id == id).ToList();
                    Empresa.Activs = dbModel.actividad.Include(x => x.usuario.contactos).Where(x => x.oportunidades.contacto_empresa.empresa.id == id).ToList();


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
                cE.id_contacto = id_contacto;


                dbModel.contacto_empresa.Add(cE);
                dbModel.SaveChanges();
                return Json(new { Url = "/Empresas/Details_Empresa/" + id });
            }
        }
    }

}