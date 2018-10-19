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

        public ActionResult Index_Empresa_Contacto(int? page, string search, int? nn, string sortBy)

        {
            List<contacto_empresa> ListacEmpresa = new List<contacto_empresa>();

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
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {

                if (search != String.Empty && search != null)
                {
                    var strings = search.ToLower().Split(' ');
                    ListacEmpresa = dbModel.contacto_empresa.Include(x => x.empresa).Include(x => x.contactos).ToList();

                    foreach (var splitString in strings)
                    {
                        ListacEmpresa = ListacEmpresa.Where(x => x.contactos.nombres.ToLower().Contains(splitString) || x.contactos.apellidos.ToLower().Contains(splitString) || x.empresa.razon_social.ToLower().Contains(splitString) 
                        || x.cargo != null && x.cargo.ToLower().Contains(splitString) || x.telefono_celular.ToString().Contains(splitString) 
                        || x.telefono_fijo.ToString().Contains(splitString) || x.contactos.rut.ToLower().Contains(splitString) 
                        || x.empresa.rut.ToLower().Contains(splitString)).ToList();
                    }
                }
                else
                {
                    ListacEmpresa = dbModel.contacto_empresa.Include(x => x.empresa).Include(x => x.contactos).ToList();

                }

                ViewBag.SortNEmpresa = string.IsNullOrEmpty(sortBy) ? "name_empresa_desc" : "";
                ViewBag.SortNContacto = sortBy == "nombre_contacto" ? "nombre_contacto_desc" : "nombre_contacto";
                ViewBag.SortFechaI = sortBy == "fechaI" ? "fechaI_desc" : "fechaI";
                ViewBag.SortFechaT = sortBy == "fechaT" ? "fechaT_desc" : "fechaT";
                ViewBag.SortCargo = sortBy == "cargo" ? "cargo_desc" : "cargo";
                ViewBag.SortTFijo = sortBy == "tfijo" ? "tfijo_desc" : "tfijo";
                ViewBag.SortTCelular = sortBy == "tcelular" ? "tcelular_desc" : "tcelular";
                ViewBag.SortRutEmpresa = sortBy == "rut_empresa" ? "rut_empresa_desc" : "rut_empresa";
                ViewBag.SortRutContacto = sortBy == "rut_contacto" ? "rut_contacto_desc" : "rut_contacto";

                switch (sortBy)
                {
                    case "name_empresa_desc":
                        ListacEmpresa = ListacEmpresa.OrderByDescending(x => x.empresa.razon_social).ToList();
                        break;
                    case "nombre_contacto":
                        ListacEmpresa = ListacEmpresa.OrderBy(x => x.contactos.nombres).ThenBy(x=>x.contactos.nombres).ToList();
                        break;
                    case "nombre_contacto_desc":
                        ListacEmpresa = ListacEmpresa.OrderByDescending(x => x.contactos.nombres).ThenBy(x => x.contactos.nombres).ToList();
                        break;
                    case "fechaI_desc":
                        ListacEmpresa = ListacEmpresa.OrderByDescending(x => x.fecha_inicio_rel).ToList();
                        break;
                    case "fechaI":
                        ListacEmpresa = ListacEmpresa.OrderBy(x => x.fecha_inicio_rel).ToList();
                        break;
                    case "fechaT_desc":
                        ListacEmpresa = ListacEmpresa.OrderByDescending(x => x.fecha_termino_rel).ToList();
                        break;
                    case "fechaT":
                        ListacEmpresa = ListacEmpresa.OrderBy(x => x.fecha_termino_rel).ToList();
                        break;
                    case "cargo_desc":
                        ListacEmpresa = ListacEmpresa.OrderByDescending(x => x.cargo).ToList();
                        break;
                    case "cargo":
                        ListacEmpresa = ListacEmpresa.OrderBy(x => x.cargo).ToList();
                        break;
                    case "tfijo_desc":
                        ListacEmpresa = ListacEmpresa.OrderByDescending(x => x.telefono_fijo).ToList();
                        break;
                    case "tfijo":
                        ListacEmpresa = ListacEmpresa.OrderBy(x => x.telefono_fijo).ToList();
                        break;
                    case "tcelular_desc":
                        ListacEmpresa = ListacEmpresa.OrderByDescending(x => x.telefono_celular).ToList();
                        break;
                    case "tcelular":
                        ListacEmpresa = ListacEmpresa.OrderBy(x => x.telefono_celular).ToList();
                        break;
                    case "rut_empresa_desc":
                        ListacEmpresa = ListacEmpresa.OrderByDescending(x => x.empresa.rut).ToList();
                        break;
                    case "rut_empresa":
                        ListacEmpresa = ListacEmpresa.OrderBy(x => x.empresa.rut).ToList();
                        break;
                    case "rut_contacto":
                        ListacEmpresa = ListacEmpresa.OrderBy(x => x.contactos.rut).ToList();
                        break;
                    case "rut_contacto_desc":
                        ListacEmpresa = ListacEmpresa.OrderByDescending(x => x.contactos.rut).ToList();
                        break;
                    default:
                        ListacEmpresa = ListacEmpresa.OrderBy(x => x.empresa.razon_social).ToList();
                        break;
                }

                return View(ListacEmpresa.ToList<contacto_empresa>().ToPagedList(page ?? 1, nn ?? 10));
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
        [ValidateAjax]
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

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);

            }
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
                    ModelState.Clear();


                    contactoEmpresa = new contacto_empresa();
                    contactoEmpresa.empresaListI = dbModel.empresa.OrderBy(x => x.razon_social).ToList();
                    contactoEmpresa.contactoListI = dbModel.contactos.OrderBy(x => x.nombres).ThenBy(x => x.apellidos).ToList();
                    ViewBag.TheResult = true;
                    return View(contactoEmpresa);

                }
                else
                {
                    contactoEmpresa = new contacto_empresa();
                    contactoEmpresa.empresaListI = dbModel.empresa.OrderBy(x => x.razon_social).ToList();
                    contactoEmpresa.contactoListI = dbModel.contactos.OrderBy(x => x.nombres).ThenBy(x => x.apellidos).ToList();
                    return View("Crear_contacto_empresaF", contactoEmpresa);
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