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
    public class OportunidadesController : Controller
    {
        // GET: Oportunidades
        public ActionResult Index_Oportunidades(string search, int? page, int? estado, string sortBy, int? nn)
        {

            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                List<oportunidades> listaOps = new List<oportunidades>();

                //Lista de Contactos a asignad
                ViewBag.Hola = new SelectList(dbModel.usuario.Where(x => x.estado == "activo").OrderBy(x=>x.contactos.nombres).Include(x=>x.contactos).ToList(), "id", "FullName");

                //Lista de páginas
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

                //Lista de estados de la oportunidad
                ViewBag.Estados =  new List<SelectListItem>()

                  {
                    new SelectListItem() {Text="Todas las Oportunidades", Value="0"},
                    new SelectListItem() {Text="Activas", Value="1"},
                    new SelectListItem() {Text="Logradas", Value="2"},
                    new SelectListItem() {Text="Perdidas", Value="3"}
                  };

                //SortBy
                ViewBag.SortNempresa = string.IsNullOrEmpty(sortBy) ? "Nempresa_desc" : "";
                ViewBag.SortId = sortBy == "Id" ? "Id_desc" : "Id";
                ViewBag.SortCempresa = sortBy == "Cempresa" ? "Cempresa_desc" : "Cempresa";
                ViewBag.SortCasignado = sortBy == "Casignado" ? "Casignado_desc" : "Casignado";
                ViewBag.SortFechaC = sortBy == "FechaC" ? "FechaC_desc" : "FechaC";
                ViewBag.SortFechaV = sortBy == "FechaV" ? "FechaV_desc" : "FechaV";
                ViewBag.SortEstado = sortBy == "Estado" ? "Estado_desc" : "Estado";
                ViewBag.SortCupos = sortBy == "Cupos" ? "Cupos_desc" : "Cupos";
                ViewBag.SortTema = sortBy == "Tema" ? "Tema_desc" : "Tema";
                ViewBag.SortGestiones = sortBy == "Gestiones" ? "Gestiones" : "Gestiones";



                //Busqueda

                if (search == String.Empty || search == null)
                {
                    listaOps = dbModel.oportunidades.Include(x => x.usuario.contactos)
                .Include(x => x.contacto_empresa.empresa).Include(x => x.contacto_empresa.contactos).Include(x => x.usuario).ToList();
                }
                else
                {
                    var strings = search.ToLower().Split(' ');
                    listaOps = dbModel.oportunidades.Include(x => x.usuario.contactos).Include(x => x.contacto_empresa.empresa).Include(x => x.contacto_empresa.contactos).Include(x => x.usuario).ToList();

                    foreach (var splitString in strings)
                    {
                        listaOps = listaOps.Where(x => x.contacto_empresa.empresa.razon_social.ToLower().Contains(splitString) || x.contacto_empresa.contactos.nombres.ToLower().Contains(splitString)
                    || x.contacto_empresa.contactos.apellidos.ToLower().Contains(splitString) || x.usuario.contactos.nombres.ToLower().Contains(splitString) || x.usuario.contactos.apellidos.ToLower().Contains(splitString)
                    || x.tema.ToLower().Contains(splitString)||x.estado.ToLower().Contains(splitString)||x.cupos.ToString().Contains(splitString)).ToList();
                    }

              
                }

                //filtro por estado
                switch (estado)
                {
                    case 1:
                        listaOps = listaOps.Where(x => x.estado == "activa").ToList();
                        break;
                    case 2:
                        listaOps = listaOps.Where(x => x.estado == "lograda").ToList();
                        break;
                    case 3:
                        listaOps = listaOps.Where(x => x.estado == "perdida").ToList();
                        break;

                    default:
                        break;
                }

                //filtro por sortBy
                switch (sortBy)
                {
                    case "Cempresa_desc":
                        listaOps = listaOps.OrderByDescending(x => x.contacto_empresa.contactos.nombres).ThenBy(x => x.contacto_empresa.contactos.apellidos).ToList();
                        break;
                    case "Cempresa":
                        listaOps = listaOps.OrderBy(x => x.contacto_empresa.contactos.nombres).ThenBy(x => x.contacto_empresa.contactos.apellidos).ToList();
                        break;
                    case "Nempresa_desc":
                        listaOps = listaOps.OrderByDescending(x => x.contacto_empresa.empresa.razon_social).ToList();
                        break;
                    case "Casignado_desc":
                        listaOps = listaOps.OrderByDescending(x => x.usuario.contactos.nombres).ThenBy(x=>x.usuario.contactos.apellidos).ToList();
                        break;
                    case "Casignado":
                        listaOps = listaOps.OrderBy(x => x.usuario.contactos.nombres).ThenBy(x => x.usuario.contactos.apellidos).ToList();
                        break;
                    case "FechaC_desc":
                        listaOps = listaOps.OrderByDescending(x => x.fecha_creacion).ToList();
                        break;
                    case "FechaC":
                        listaOps = listaOps.OrderBy(x => x.fecha_creacion).ToList();
                        break;
                    case "FechaV_desc":
                        listaOps = listaOps.OrderByDescending(x => x.fecha_vencimiento).ToList();
                        break;
                    case "FechaV":
                        listaOps = listaOps.OrderBy(x => x.fecha_vencimiento).ToList();
                        break;
                    case "Tema_desc":
                        listaOps = listaOps.OrderByDescending(x => x.tema).ToList();
                        break;
                    case "Tema":
                        listaOps = listaOps.OrderBy(x => x.tema).ToList();
                        break;
                    case "Estado_desc":
                        listaOps = listaOps.OrderByDescending(x => x.estado).ToList();
                        break;
                    case "Estado":
                        listaOps = listaOps.OrderBy(x => x.estado).ToList();
                        break;
                    case "Cupos_desc":
                        listaOps = listaOps.OrderByDescending(x => x.cupos).ToList();
                        break;
                    case "Cupos":
                        listaOps = listaOps.OrderBy(x => x.cupos).ToList();
                        break;
                    case "Id_desc":
                        listaOps = listaOps.OrderByDescending(x => x.id).ToList();
                        break;
                    case "Id":
                        listaOps = listaOps.OrderBy(x => x.id).ToList();
                        break;
                    default:
                        listaOps = listaOps.OrderBy(x => x.contacto_empresa.empresa.razon_social).ToList();
                        break;
                }


                return View(listaOps.ToPagedList(page ?? 1, nn ?? 10));
            }


        }


        public ActionResult Detalle_Oportunidad(int id)
        {
            ViewBag.idOp = id.ToString();
            List<actividad> opModel = new List<actividad>();
            ViewBag.ListaL = new List<SelectListItem>()
                  {
                    new SelectListItem() {Text="Todas", Value="todas"},
                    new SelectListItem() {Text="Activas", Value="activa"},
                    new SelectListItem() {Text="Cumplida", Value="cumplida"},
                    new SelectListItem() {Text="Cancelada", Value="cancelada"},
                    new SelectListItem() {Text="Mis Activas", Value="mactivas"},
                    new SelectListItem() {Text="Mis Cumplidas", Value="mcumplidas"},
                    new SelectListItem() {Text="Mis Canceladas", Value="mcanceladas"}
                  };
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {

                opModel = dbModel.actividad.Where(x => x.id_oportunidad == id).Include(x => x.usuario).Include(x=>x.oportunidades).Include(x=>x.usuario.contactos).ToList<actividad>();

            }

            return PartialView(opModel);
        }



        public ActionResult Crear_Oportunidad()
        {
            oportunidades model = new oportunidades();
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {
                model.EmpresaModel = new List<empresa>();
                model.EmpresaModel = db.empresa.OrderBy(x=>x.razon_social).ToList();
                model.usuarioList = db.usuario.Include(x=>x.contactos).ToList();
                model.contacto_empresaList = db.contacto_empresa.Include(x=>x.contactos).Include(x=>x.empresa).ToList();
                //model.fecha_creacion = DateTime.Now;
                return View(model);
            }

        }

        public ActionResult Crear_OportunidadP()
        {
            oportunidades model = new oportunidades();
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {
                model.EmpresaModel = new List<empresa>();
                model.EmpresaModel = db.empresa.OrderBy(x=>x.razon_social).ToList();

                // model.usuarioList = db.usuario.Where(x => db.contacto_empresa.Select(b => b.id_contacto.ToString()).Contains(x.id.ToString())).ToList();
                model.contacto_empresaList = db.contacto_empresa.Include(x => x.contactos).Include(x => x.empresa).ToList();
                model.usuarioList = db.usuario.Include(x => x.contactos).OrderBy(x=>x.contactos.nombres).ToList();

                return PartialView(model);
            }


        }


        [HttpPost]
        public ActionResult Index_Oportunidades(IList<oportunidades> opModel)
        {
            if (opModel == null)
            {
                return RedirectToAction("/Index_Oportunidades");

            }
            oportunidades opor = new oportunidades();
            if (Request.Form["Lograda"] != null)
            {
                using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
                {
                    foreach (var item in opModel.Where(c => c.isSelected))
                    {
                        opor = dbModel.oportunidades.Where(x => x.id == item.id).FirstOrDefault();
                        opor.estado = "lograda";

                        dbModel.Entry(opor).State = System.Data.Entity.EntityState.Modified;
                    }
                    dbModel.SaveChanges();
                }

            }
            else if (Request.Form["Perdida"] != null)
            {
                using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
                {
                    foreach (var item in opModel.Where(c => c.isSelected))
                    {
                        opor = dbModel.oportunidades.Where(x => x.id == item.id).FirstOrDefault();
                        opor.estado = "perdida";

                        dbModel.Entry(opor).State = System.Data.Entity.EntityState.Modified;
                    }
                    dbModel.SaveChanges();
                }

            }
            else if (Request.Form["Asignar"] != null)
            {
                using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
                {
                    string strDDLValue = Request.Form["lala"].ToString();
                    if (strDDLValue == null || strDDLValue == String.Empty)
                    {
                        return Redirect(Request.UrlReferrer.ToString());

                    }

                    foreach (var item in opModel.Where(c => c.isSelected))
                    {

                        opor = dbModel.oportunidades.Where(x => x.id == item.id).FirstOrDefault();
                        opor.id_contacto_asignado = int.Parse(strDDLValue);

                        dbModel.Entry(opor).State = System.Data.Entity.EntityState.Modified;
                    }
                    dbModel.SaveChanges();
                }

            }
            return Redirect(Request.UrlReferrer.ToString());

        }



        [HttpPost]
        public ActionResult Crear_Oportunidad(oportunidades oModel)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {


                if (ModelState.IsValid)
                {
                    oModel.estado = "activa";
                    dbModel.oportunidades.Add(oModel);
                    dbModel.SaveChanges();
                    ModelState.Clear();

                    oModel = new oportunidades();
                    oModel.EmpresaModel = dbModel.empresa.OrderBy(x => x.razon_social).ToList();
                    oModel.usuarioList = dbModel.usuario.Include(x => x.contactos).OrderBy(x => x.contactos.nombres).ToList();
                    oModel.contacto_empresaList = dbModel.contacto_empresa.Include(x => x.contactos).Include(x => x.empresa).ToList();
                    ViewBag.TheResult = true;

                    return View("Crear_Oportunidad", oModel);
                }
                else
                {
                    return View("Crear_Oportunidad", oModel);

                }

            }
        }
        [HttpPost]
        public ActionResult GetContactobyEmpresa(int id)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                List<contacto_empresa> objCEmpresa = new List<contacto_empresa>();
                objCEmpresa = dbModel.contacto_empresa.Where(m => m.id_empresa == id).Include(x=>x.empresa).Include(x=>x.contactos).ToList();
                SelectList obCEmpresa = new SelectList(objCEmpresa, "id", "FullName", 0);

                return Json(obCEmpresa);
            }
        }

        [HttpPost]
        public ActionResult Crear_OportunidadP(oportunidades oModel, int? id_empresa)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                if (ModelState.IsValid)
                {
                    oModel.estado = "Activa";
                    dbModel.oportunidades.Add(oModel);
                    dbModel.SaveChanges();
                    return Redirect(Request.UrlReferrer.ToString());

                }

                //oModel.usuarioList = dbModel.usuario.Where(x => dbModel.usuario.Select(b => b.id_contacto.ToString()).Contains(x.id)).ToList();
                oModel.contacto_empresaList = dbModel.contacto_empresa.ToList();
                oModel.usuarioList = dbModel.usuario.ToList();
                return PartialView("Crear_OportunidadP", oModel);

            }

        }
        public ActionResult Edit_Oportunidad(int id, int? id_empresa, int? id_usuario)
        {
            oportunidades oModel = new oportunidades();
            ViewBag.id_usuario = id_usuario;
            ViewBag.id_empresa = id_empresa;
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                oModel = dbModel.oportunidades.Where(x => x.id == id).FirstOrDefault();

                oModel.contacto_empresaListI = dbModel.contacto_empresa.Select(x => new SelectListItem
                {
                    Text = x.contactos.nombres + " " + x.contactos.apellidos,
                    Value = x.id.ToString()


                }).ToList();    

                oModel.usuarioListI = dbModel.usuario.Select(x => new SelectListItem
                {
                    Text = x.contactos.nombres + " " + x.contactos.apellidos,
                    Value = x.id.ToString()


                }).ToList();
            }

            return PartialView(oModel);
        }

        [HttpPost]
        public ActionResult Edit_Oportunidad(oportunidades oModel)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                dbModel.Entry(oModel).State = System.Data.Entity.EntityState.Modified;
                dbModel.SaveChanges();

            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Delete_Oportunidad(int id, FormCollection collection)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                oportunidades opModel = dbModel.oportunidades.Where(x => x.id == id).FirstOrDefault();

                int test = opModel.id_contacto_asignado;
                int test2 = opModel.id_contacto_empresa;

                try
                {

                    dbModel.oportunidades.Remove(opModel);
                    dbModel.SaveChanges();

                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }


                catch (Exception ex)
                {

                    opModel.id_contacto_asignado = test;
                    opModel.id_contacto_empresa = test2;
                    ModelState.AddModelError("error",
                   ex.Message + " No se pudo eliminar el registro");
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }


            }

        }
        public ActionResult Delete_Oportunidad(int id, int? id_empresa, int? id_usuario)
        {
            ViewBag.id_usuario = id_usuario;
            ViewBag.id_empresa = id_empresa;
            oportunidades oModel = new oportunidades();
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                oModel = dbModel.oportunidades.Where(x => x.id == id).Include(x => x.usuario).Include(x => x.contacto_empresa).FirstOrDefault();

            }

            return PartialView(oModel);
        }
        [HttpPost]
        public ActionResult Detalle_Oportunidad(string estado, int? id_ops)

        {

            ViewBag.ListaL = new List<SelectListItem>()

                  {
                    new SelectListItem() {Text="Todas Las Oportunidades", Value="todas"},
                    new SelectListItem() {Text="Activas", Value="activa"},
                    new SelectListItem() {Text="Cumplidas", Value="cumplida"},
                    new SelectListItem() {Text="Canceladas", Value="cancelada"},
                    new SelectListItem() {Text="Mis Activas", Value="mactivas"},
                    new SelectListItem() {Text="Mis Cumplidas", Value="mcumplidas"},
                    new SelectListItem() {Text="Mis Canceladas", Value="mcanceladas"}
                  };
    
            ViewBag.idOp = id_ops.ToString();
            string sessionID = System.Web.HttpContext.Current.Session["userId"].ToString();
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())

            {
                List<actividad> opModel = new List<actividad>();
                if (estado == "todas")
                {
                    opModel = dbModel.actividad.Where(x => x.id_oportunidad == id_ops).Include(x => x.usuario).Include(x => x.oportunidades).Include(x => x.usuario.contactos).Include(x => x.oportunidades.contacto_empresa.empresa).ToList<actividad>();

                }
                else if (estado == "mcumplidas" || estado == "mactivas" || estado=="mcanceladas" )
                {
                    switch (estado)
                    {   case "mcumplidas": estado = "cumplida";
                                break;
                        case "mactivas": estado = "activa";
                            break;
                        case "mcanceladas": estado = "cancelada";
                            break;
                        default:
                            break;
                    }
                    opModel = dbModel.actividad.Where(x => x.id_oportunidad == id_ops && x.usuario.id.ToString() == sessionID && x.estado == estado).Include(x => x.usuario).Include(x => x.oportunidades).Include(x => x.usuario.contactos).Include(x => x.oportunidades.contacto_empresa.empresa).ToList<actividad>();

                }
                else
                {
                    opModel = dbModel.actividad.Where(x => x.id_oportunidad == id_ops && x.estado == estado).Include(x => x.usuario).Include(x => x.oportunidades).Include(x => x.usuario.contactos).Include(x => x.oportunidades.contacto_empresa.empresa).ToList<actividad>();

                }
                return PartialView(opModel);


                //dbModel.Configuration.LazyLoadingEnabled = false;

                //objCEmpresa = dbModel.actividad.Where(m => m.estado == id).Include(x => x.oportunidades.contacto_empresa.empresa).Include(x => x.usuario.contactos).Include(x => x.oportunidades).Include(x=>x.usuario).ToList<actividad>();
                //return new JsonResult { Data = objCEmpresa,  };


            }
        }

        public ActionResult Crear_ContactoEmpresaContactoP()
        {
            ContactoEmpresaContacto model = new ContactoEmpresaContacto();
            model.cEmpresa = new contacto_empresa();
            model.contacto = new contactos();
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {

                model.cEmpresa.empresaListI = db.empresa.ToList();
                return PartialView(model);
            }

        }
        [HttpPost]
        public ActionResult Crear_ContactoEmpresaContactoP(ContactoEmpresaContacto CEC)
        {


            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                CEC.cEmpresa.id_contacto = CEC.contacto.id;

                dbModel.contactos.Add(CEC.contacto);
                dbModel.contacto_empresa.Add(CEC.cEmpresa);

                dbModel.SaveChanges();
            }
            return RedirectToAction("/Index_Oportunidades");
        }
        public ActionResult Agregar_Contacto(int? id_empresa)
        {


            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                TempData["id_empresa"] = id_empresa;
                contactos contacto = new contactos();
                return PartialView(contacto);

            }
        }
        [HttpPost]
        public ActionResult Agregar_Contacto(contactos contacto)
        {


            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                contacto_empresa Cempresa = new contacto_empresa();
                //Cempresa.id_empresa = TempData["id_empresa"];
                dbModel.contactos.Add(contacto);
                //dbModel.contacto_empresa.Add(contacto);

                dbModel.SaveChanges();
            }
            return RedirectToAction("/Index_Oportunidades");
        }
    }
}