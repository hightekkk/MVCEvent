using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCEventCalendar.Controllers
{
    public class ContactsController : Controller
    {
        // GET: Contacts
        public ActionResult IndexPr()
        {
            List<Contact> allContacts = null;
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var v = (from a in dc.Contacts
                         join b in dc.Predmetis on a.PredmetId equals b.PredmetId
                         join c in dc.Prepods on a.PrepodId equals c.PrepodId
                         select new
                         {
                             a,
                             b.Predmet,
                             c.Prepodavatel
                         });
                if (v != null)
                {
                    allContacts = new List<Contact>();
                    foreach (var i in v)
                    {
                        Contact c = i.a;
                        c.Predmet = i.Predmet;
                        c.Prepodavatel = i.Prepodavatel;
                        allContacts.Add(c);
                    }
                }
            }


                return View(allContacts);
        }

        private List<Predmeti> GetPredmeti()
        {
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                return dc.Predmetis.OrderBy(a => a.Predmet).ToList();
            }
        }

        private List<Prepod> GetPrepods(int predmetId)
        {
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                return dc.Prepods.Where(a => a.PredmetId.Equals(predmetId)).OrderBy(a => a.Prepodavatel).ToList();
            }
        }

        public JsonResult GetPrepodList(int predmetId)
        {
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                return new JsonResult { Data = GetPrepods(predmetId), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public Contact GetContact(int ContactId)
        {
            Contact contact = null;
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var v = (from a in dc.Contacts
                         join b in dc.Predmetis on a.PredmetId equals b.PredmetId
                         join c in dc.Prepods on a.PrepodId equals c.PrepodId
                         where a.ContactId.Equals(ContactId)
                         select new
                         {
                             a,
                             b.Predmet,
                             c.Prepodavatel
                         }).FirstOrDefault();
                if (v != null)
                {
                    contact = v.a;
                    contact.Predmet = v.Predmet;
                    contact.Prepodavatel = v.Prepodavatel;
                }
                return contact;
            }
        }

        public ActionResult Save(int id = 0)
        {
            List<Predmeti> predmetis = GetPredmeti();
            List<Prepod> prepods = new List<Prepod>();
            if (id > 0)
            {
                var c = GetContact(id);
                if (c != null)
                {
                    ViewBag.Predmetis = new SelectList(predmetis, "PredmetId", "Predmet", c.PredmetId);
                    ViewBag.Prepods = new SelectList(GetPrepods(c.PredmetId), "PrepodId", "Prepodavatel", c.PrepodId);
                }
                else
                {
                    return HttpNotFound();
                }
                return View(c);
            }
            else
            {
                ViewBag.Predmetis = new SelectList(predmetis, "PredmetId", "Predmet");
                ViewBag.Prepods = new SelectList(prepods, "PrepodId", "Prepodavatel");
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Contact c)
        {
            
            string message = "";
            bool status = false;

            if (ModelState.IsValid)
            {
                using (MyDatabaseEntities dc = new MyDatabaseEntities())
                {
                    if (c.ContactId > 0)
                    {
                        var v = dc.Contacts.Where(a => a.ContactId.Equals(c.ContactId)).FirstOrDefault();
                        if (v != null)
                        {
                            v.PredmetId = c.PredmetId;
                            v.PrepodId = c.PrepodId;
                        }
                        else
                        {
                            return HttpNotFound();
                        }
                    }
                    else
                    {
                        dc.Contacts.Add(c);
                    }
                    dc.SaveChanges();
                    status = true;
                    return RedirectToAction("IndexPr");
                }
            }
            else
            {
                message = "Error! Please try again";
                ViewBag.Predmetis = new SelectList(GetPredmeti(), "PredmetId", "Predmet", c.PredmetId);
                ViewBag.Prepods = new SelectList(GetPrepods(c.PredmetId), "PrepodId", "Prepodavatel", c.PrepodId);
            }
            ViewBag.Message = message;
            ViewBag.Status = status;
            return View(c);
        }
    }
}