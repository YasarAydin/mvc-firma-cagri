using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcFirmaCagri.Models.Entity;


namespace MvcFirmaCagri.Controllers
{
    [Authorize]
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        DbisTakipEntities1 db = new DbisTakipEntities1();

        public ActionResult AktifCagrilar()
        {
            var mail = (string) Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID)
                .FirstOrDefault();
            var cagrilar = db.TblCagrilar.Where(x=>x.Durum==true && x.CagriFirma == id).ToList();
            return View(cagrilar);
        }

        public ActionResult PasifCagrilar()
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID)
                .FirstOrDefault();
            var cagrilar = db.TblCagrilar.Where(x => x.Durum == false && x.CagriFirma == id).ToList();
            return View(cagrilar);
        }

        [HttpGet]
        public ActionResult YeniCagri()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult YeniCagri(TblCagrilar p)
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID)
                .FirstOrDefault();

            p.Durum = true;
            p.Tarih = DateTime.Parse(DateTime.Now.ToShortDateString());
            p.CagriFirma = id;
            db.TblCagrilar.Add(p);
            db.SaveChanges();
            return RedirectToAction("AktifCagrilar");
        }

        
        public ActionResult CagriDetay(int id)
        {
            var cagri = db.TblCagriDetay.Where(x => x.Cagri == id).ToList();
            return View(cagri);
        }

        public ActionResult CagriGetir(int id)
        {
            var cagri = db.TblCagrilar.Find(id);
            return View("CagriGetir", cagri);
        }

        public ActionResult CagriDuzenle(TblCagrilar p)
        {
            var cagri = db.TblCagrilar.Find(p.ID);
            cagri.Konu = p.Konu;
            cagri.Aciklama = p.Aciklama;
            db.SaveChanges();
            return RedirectToAction("AktifCagrilar");
        }

        [HttpGet]
        public ActionResult ProfilDuzenle()
        {
            var mail = (string) Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID)
                .FirstOrDefault();
            var profil = db.TblFirmalar.Where(x => x.ID == id).FirstOrDefault();
            return View(profil);
        }

        public ActionResult AnaSayfa()
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID)
                .FirstOrDefault();

            var toplamcagri = db.TblCagrilar.Where(x => x.CagriFirma == id).Count();
            var aktifcagri = db.TblCagrilar.Where(x => x.CagriFirma == id && x.Durum==true).Count();
            var pasifcagri = db.TblCagrilar.Where(x => x.CagriFirma == id && x.Durum == false).Count();
            var yetkili = db.TblFirmalar.Where(x => x.ID == id).Select(y => y.Yetkili).FirstOrDefault();
            var sektor = db.TblFirmalar.Where(x => x.ID == id).Select(y => y.Sektor).FirstOrDefault();
            var firmaadi = db.TblFirmalar.Where(x => x.ID == id).Select(y => y.Ad).FirstOrDefault();
            var firmaGorsel = db.TblFirmalar.Where(x => x.ID == id).Select(y => y.Gorsel).FirstOrDefault();
            
            ViewBag.c1 = toplamcagri;
            ViewBag.c2 = aktifcagri;
            ViewBag.c3 = pasifcagri;
            ViewBag.c4 = yetkili;
            ViewBag.c5 = firmaadi;
            ViewBag.c6 = firmaGorsel;
            return View();
        }

        public PartialViewResult Partial1()
        {
            // true okunmamış mesaj - false okunmuş mesaj
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID)
                .FirstOrDefault();
            var mesajlar = db.TblMesajlar.Where(x => x.Alici == id && x.Durum == true).ToList();
            var mesajSayisi = db.TblMesajlar.Where(x => x.Alici == id && x.Durum == true).Count();
            ViewBag.m1 = mesajSayisi;
            return PartialView(mesajlar);
        }

        public PartialViewResult Partial2()
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID)
                .FirstOrDefault();
            var cagrilar = db.TblCagrilar.Where(x => x.CagriFirma == id && x.Durum == true).ToList();
            var cagriSayisi = db.TblCagrilar.Where(x => x.CagriFirma == id && x.Durum == true).Count();
            ViewBag.c1 = cagriSayisi;
            return PartialView(cagrilar);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }

        public PartialViewResult Partial3()
        {
            return PartialView();
        }
    }
}