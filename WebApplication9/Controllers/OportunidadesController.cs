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
        public ActionResult Index_Oportunidades(string searchBy, string search, int? page, int? estado)
        {

            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                List<oportunidades> listaOps = new List<oportunidades>();

                ViewBag.Hola = new SelectList(dbModel.usuario.OrderBy(x=>x.contactos.nombres).Include(x=>x.contactos).ToList(), "id", "FullName");
                ViewBag.Estados =  new List<SelectListItem>()

                  {
                    new SelectListItem() {Text="Todas las Oportunidades", Value="0"},
                    new SelectListItem() {Text="Activas", Value="1"},
                    new SelectListItem() {Text="Logradas", Value="2"},
                    new SelectListItem() {Text="Perdidas", Value="3"}
                  };
                ViewBag.SearchL = new List<SelectListItem>()

                  {
                    new SelectListItem() {Text="Todos los Campos", Value="todos"},
                    new SelectListItem() {Text="Empresa", Value="Empresa"},
                    new SelectListItem() {Text="Contacto", Value="Contacto"},
                    new SelectListItem() {Text="Oportunidades Activas", Value="activa"},
                    new SelectListItem() {Text="Tema", Value="Tema"}
                  };
                switch (searchBy)
                {
                    case "todos":
                        if (search == String.Empty || search == null)
                        {
                            listaOps = dbModel.oportunidades.OrderBy(x => x.contacto_empresa.empresa.razon_social).Include(x => x.usuario.contactos).Include(x => x.contacto_empresa.empresa).Include(x => x.contacto_empresa.contactos).Include(x => x.usuario).ToList();
                            break;
                        }
                        else
                        {
                            listaOps = dbModel.oportunidades.Where(x => x.contacto_empresa.empresa.razon_social.Contains(search) || x.contacto_empresa.contactos.nombres.Contains(search) || x.contacto_empresa.contactos.apellidos.Contains(search) || x.usuario.contactos.nombres.Contains(search)||x.usuario.contactos.apellidos.Contains(search)|| x.tema.Contains(search))
                                .Include(x => x.usuario.contactos).Include(x => x.contacto_empresa.empresa).Include(x => x.contacto_empresa.contactos).Include(x => x.usuario).ToList();
                            break;
                        }
                    case "":
                        listaOps = dbModel.oportunidades.Include(x => x.usuario.contactos).Include(x => x.contacto_empresa.empresa).Include(x => x.contacto_empresa.contactos).Include(x => x.usuario).ToList();
                        break;
                    case "Contacto":
                        listaOps = dbModel.oportunidades.Where(x => x.usuario.contactos.nombres.Contains(search) || x.usuario.contactos.apellidos.Contains(search)).Include(x => x.usuario.contactos).Include(x => x.contacto_empresa.empresa).Include(x => x.contacto_empresa.contactos).Include(x => x.usuario).ToList();
                        break;
                    case "Empresa":
                        listaOps = dbModel.oportunidades.Where(x => x.contacto_empresa.empresa.razon_social.Contains(search)).Include(x => x.usuario.contactos).Include(x => x.contacto_empresa.empresa).Include(x => x.contacto_empresa.contactos).Include(x => x.usuario).ToList();
                        break;
                    case "Tema":
                        listaOps = dbModel.oportunidades.Where(x => x.tema.StartsWith(search)).Include(x => x.usuario.contactos).Include(x => x.contacto_empresa.empresa).Include(x => x.contacto_empresa.contactos).Include(x => x.usuario).ToList();
                        break;
                    default:
                        listaOps = dbModel.oportunidades.Include(x => x.usuario.contactos).Include(x => x.contacto_empresa.empresa).Include(x => x.contacto_empresa.contactos).Include(x => x.usuario).ToList();
                        break;

                       
                }
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

                return View(listaOps.OrderBy(x=>x.contacto_empresa.empresa.razon_social).ToPagedList(page ?? 1, 10));
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

                oModel.EmpresaModel = new List<empresa>();
                oModel.EmpresaModel = dbModel.empresa.OrderBy(x => x.razon_social).ToList();
                oModel.usuarioList = dbModel.usuario.Include(x => x.contactos).OrderBy(x=>x.contactos.nombres).ToList();
                oModel.contacto_empresaList = dbModel.contacto_empresa.Include(x => x.contactos).Include(x => x.empresa).ToList();

                if (ModelState.IsValid)
                {
                    oModel.estado = "activa";
                    dbModel.oportunidades.Add(oModel);
                    dbModel.SaveChanges();
                    return RedirectToAction("/Index_Oportunidades");
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
        public ActionResult Crear_OportunidadP(oportunidades oModel)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                if (ModelState.IsValid)
                {
                    oModel.estado = "Activa";
                    dbModel.oportunidades.Add(oModel);
                    dbModel.SaveChanges();
                    return RedirectToAction("/Index_Oportunidades");

                }

                //oModel.usuarioList = dbModel.usuario.Where(x => dbModel.usuario.Select(b => b.id_contacto.ToString()).Contains(x.id)).ToList();
                oModel.contacto_empresaList = dbModel.contacto_empresa.ToList();
                oModel.usuarioList = dbModel.usuario.ToList();
                return PartialView("Crear_OportunidadP", oModel);

            }

        }
        public ActionResult Edit_Oportunidad(int id)
        {
            oportunidades oModel = new oportunidades();

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
            return RedirectToAction("/Index_Oportunidades");
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

                    return RedirectToAction("/Index_Oportunidades");
                }


                catch (Exception ex)
                {

                    opModel.id_contacto_asignado = test;
                    opModel.id_contacto_empresa = test2;
                    ModelState.AddModelError("error",
                   ex.Message + " No se pudo eliminar el registro");
                    return View(opModel);
                }


            }

        }
        public ActionResult Delete_Oportunidad(int id)
        {
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
    }
}