using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication9.Models;
using System.Web.Security;
using System.Data;
using System.Data.Entity;
using PagedList;

namespace WebApplication9.Controllers
{
    public class ProyectobController : Controller
    {
        // GET: Proyectob

            public ActionResult CheckIfUser(string id, DateTime fecha_termino_rel)
        {
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {
                var SearchData = db.usuario_rol.Where(x => x.id_usuario.ToString() == id && x.fecha_termino_rel >= fecha_termino_rel).SingleOrDefault();
                Mensajes mensaje = new Mensajes();

                if (SearchData == null)
                {   
                    mensaje.MensajeRol = "No es User";
                    return View(mensaje);

                }
                else if (SearchData != null && SearchData.id_rol.Equals("x"))
                {
                    mensaje.MensajeRol = "Es User y Admin";
                    return PartialView(mensaje);
                }

                else 
                {

                    mensaje.MensajeRol = "Es User";
                    return PartialView(mensaje);
                }
            }

            
        }
        /* public ActionResult CheckIfAdmin(string id, DateTime fecha_termino_rel)
         {
             using (proyectob_dbEntities db = new proyectob_dbEntities())
             {
                 var SearchData = db.usuario_rol.Where(x => x.id_usuario == id && x.fecha_termino_rel > fecha_termino_rel).SingleOrDefault();

                 if (SearchData != null)
                 {
                     return Content("Es User");

                 }
                 else
                 {
                     return Content("No es User");

                 }
             }


         }

     */
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();

        }

        public ActionResult Home()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Authorize(usuario usuarioModel)
        {



                using (proyectob_dbEntities db = new proyectob_dbEntities())
                {
                    var usuarioDetails = db.usuario.Where(x => x.id == usuarioModel.id).FirstOrDefault();
                    if (usuarioDetails == null)
                    {
                        usuarioModel.LoginError = "Rut o Password incorrectos";
                        return View("Login", usuarioModel);
                    }
                    else
                    {

                        //FormsAuthentication.SetAuthCookie(contactosModel.rut, rememberMe = true);
                        Session["userID"] = usuarioDetails.id;
                        Session["Name"] = usuarioDetails.FullName;
                    return RedirectToAction("/Home", "Proyectob");
                    }
                }

        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Proyectob");

        }

    
      


      
    }
}
