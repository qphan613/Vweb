using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vweb.Models;

namespace Vweb.Controllers
{
    public class HomeController : Controller
    {
        YiaContext db = new YiaContext();
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Standing()
        {
            List<Standing> model = new List<Standing>();
            Standing st;
            List<Team> Teams = db.Teams.ToList();
            List<Record> Records = db.Records.ToList();


            foreach (Team tr in db.Teams)
            {
                st = new Standing();
                st.Name = tr.Name;
                st.TeamRecord = Records.Find(o => o.TeamID == tr.ID);
                model.Add(st);


            }

            var s = model.OrderByDescending(o => o.TeamRecord.Win);
            return View(s.ToList());
        }

        public ActionResult Admin()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}
