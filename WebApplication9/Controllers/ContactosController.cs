﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using PagedList;
using WebApplication9.Models;
using System.Net;
using System.IO;

namespace WebApplication9.Controllers
{
    public class ContactosController : Controller
    {
        // GET: Contactos
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index_Contacto(int? page, string search, int? nn, string sortBy)
        {

            ViewBag.SortName = string.IsNullOrEmpty(sortBy) ? "name_desc" : "";
            ViewBag.SortRut = sortBy == "rut" ? "rut_desc" : "rut";
            ViewBag.SortApellido = sortBy == "apellido" ? "apellido_desc" : "apellido";
            ViewBag.SortCel = sortBy == "celular" ? "celular_desc" : "celular";
            ViewBag.SortEmail = sortBy == "email" ? "email_desc" : "email";
            ViewBag.SortComentario = sortBy == "comentario" ? "comentario_desc" : "comentario";


            List<contactos> listaContactos = new List<contactos>();
            ViewBag.SearchL = new List<SelectListItem>()


                  {
                    new SelectListItem() {Text="Todos los campos", Value="todos"},
                    new SelectListItem() {Text="Nombre", Value="nombre"},
                  };
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
                    listaContactos = dbModel.contactos.Where(x => x.apellidos.Contains(search) || x.comentario.Contains(search) || x.email.Contains(search) || x.nombres.Contains(search) || x.rut.Contains(search) || x.tcel.ToString().Contains(search)).ToList();
                }
                else
                {
                    listaContactos = dbModel.contactos.OrderBy(x => x.nombres).ToList();

                }

                switch (sortBy)
                {
                    case "rut_desc":
                        listaContactos = listaContactos.OrderByDescending(x => x.rut).ToList();
                        break;
                    case "rut":
                        listaContactos = listaContactos.OrderBy(x => x.rut).ToList();
                        break;
                    case "name_desc":
                        listaContactos = listaContactos.OrderByDescending(x => x.nombres).ToList();
                        break;
                    case "apellido_desc":
                        listaContactos = listaContactos.OrderByDescending(x => x.apellidos).ToList();
                        break;
                    case "apellido":
                        listaContactos = listaContactos.OrderBy(x => x.apellidos).ToList();
                        break;
                    case "celular_desc":
                        listaContactos = listaContactos.OrderByDescending(x => x.tcel).ToList();
                        break;
                    case "celular":
                        listaContactos = listaContactos.OrderBy(x => x.tcel).ToList();
                        break;
                    case "email_desc":
                        listaContactos = listaContactos.OrderByDescending(x => x.email).ToList();
                        break;
                    case "email":
                        listaContactos = listaContactos.OrderBy(x => x.email).ToList();
                        break;
                    case "comentario_desc":
                        listaContactos = listaContactos.OrderByDescending(x => x.comentario).ToList();
                        break;
                    case "comentario":
                        listaContactos = listaContactos.OrderBy(x => x.comentario).ToList();
                        break;
                    default:
                        listaContactos = listaContactos.OrderBy(x => x.nombres).ToList();
                        break;
                }


            }



                return View(listaContactos.ToList<contactos>().ToPagedList(page ?? 1, nn ?? 10));
            }
        


        public ActionResult Create_Contacto()
        {
            contactos model = new contactos();
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {
            }   
            return View(model);
        }

        public ActionResult CreateP_Contacto()
        {
            contactos model = new contactos();
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {
            }
            return PartialView(model);
        }
        public ActionResult Details_Contacto(int id)
        {
            using (proyectob_dbEntities db = new proyectob_dbEntities())
            {

                if (id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                contactos contacto = db.contactos.Find(id);
                if (contacto == null)
                {
                    return HttpNotFound();
                }
            
            return PartialView(contacto);
            }
        }
        [HttpPost]
        public ActionResult Create_Contacto(contactos contactosModel, HttpPostedFileBase file)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                if (file != null && file.ContentLength > 0)
                    try
                    {
                        string path = Path.Combine(Server.MapPath("~/Content/imagenes"),
                                                   Path.GetFileName(file.FileName));
                        ViewBag.Message = "File uploaded successfully";
                        contactosModel.curriculum = path;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }

                if (ModelState.IsValid)
                {
                    dbModel.contactos.Add(contactosModel);
                    dbModel.SaveChanges();
                    return Redirect(Request.UrlReferrer.ToString());

                }
                return View("Create_Contacto", contactosModel);

            }

        }
        [HttpPost]
        public ActionResult CreateP_Contacto(contactos contactosModel)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                dbModel.contactos.Add(contactosModel);
                dbModel.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult Edit_Contacto(int id)
        {
            contactos contactoModel = new contactos();
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                contactoModel = dbModel.contactos.Where(x => x.id == id).FirstOrDefault();

            }

            return PartialView(contactoModel);
        }
        [HttpPost]
        public ActionResult Edit_Contacto(contactos contactoModel)
        {

            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                dbModel.Entry(contactoModel).State = System.Data.Entity.EntityState.Modified;
                dbModel.SaveChanges();

            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult Delete_Contacto(int id)
        {
            contactos contactoModel = new contactos();
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                contactoModel = dbModel.contactos.Where(x => x.id == id).FirstOrDefault();

            }

            return PartialView(contactoModel);
        }
        [HttpPost]
        public ActionResult Delete_Contacto(int id, FormCollection collection)
        {
            using (proyectob_dbEntities dbModel = new proyectob_dbEntities())
            {
                contactos contactoModel = dbModel.contactos.Where(x => x.id == id).FirstOrDefault();
                dbModel.contactos.Remove(contactoModel);
                dbModel.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

    }
}