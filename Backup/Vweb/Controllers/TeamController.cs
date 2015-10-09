using System;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vweb.Models;

namespace Vweb.Controllers
{
    public class TeamController : Controller
    {
        private YiaContext db = new YiaContext();

        //
        // GET: /Team/

        public ActionResult Index()
        {
            var team = from t in db.Teams select t;

            return View(db.Teams.ToList());
        }

        //
        // GET: /Team/Details/5

        public ActionResult Details(int id = 0)
        {
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        //
        // GET: /Team/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Team/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Team team)
        {
            if (ModelState.IsValid)
            {
                db.Teams.Add(team);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(team);
        }

        //
        // GET: /Team/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        //
        // POST: /Team/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Team team)
        {
            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(team);
        }

        public ActionResult DisplayTeam(int id = 0, string Month = "0")
        {
            DisplayTeam model = new DisplayTeam();
            List<TeamSchedule> team_schedules = new List<TeamSchedule>();
            List<Player> Roster = db.Players.ToList();
            Team team = db.Teams.Find(id);


            //Find out if the View is for specific month.
            if (Month == "0")
            {
                Month = DateTime.Now.Month.ToString();
            }

            //Grab all the schedule for this team.
            List<Schedule> schedules = db.Schedules.ToList().FindAll(o => (o.HomeTeam == id || o.RoadTeam == id) && o.GameDateTime.Month.ToString() == Month);

            foreach (Schedule item in schedules)
            {
                TeamSchedule ts = new TeamSchedule();
                ts.GameDateTime = item.GameDateTime;
                ts.Home_Team = db.Teams.Find(item.HomeTeam);
                ts.Road_Team = db.Teams.Find(item.RoadTeam);
                ts.HomeScore = item.HomeScore;
                ts.RoadScore = item.RoadScore;

                team_schedules.Add(ts);
            }

            if (team != null)
            {
                model.ID = id;
                model.Name = team.Name;
                model.Players = Roster.FindAll(o => o.TeamID == id);
                model.Schedules = team_schedules;
                model.ShowMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(System.Convert.ToInt32(Month));
            }
            else
                return View(team);

            return View(model);
        }
        //public ActionResult View(int id = 0, string Month = "0")
        //{
        //    DisplayTeam model = new DisplayTeam();
        //    List<TeamSchedule> team_schedules = new List<TeamSchedule>();
        //    List<Player> Roster = db.Players.ToList();
        //    Team team = db.Teams.Find(id);


        //    //Find out if the View is for specific month.
        //    if (Month == "0")
        //    {
        //        Month = DateTime.Now.Month.ToString();
        //    }

        //    //Grab all the schedule for this team.
        //    List<Schedule> schedules = db.Schedules.ToList().FindAll(o=>( o.HomeTeam == id || o.RoadTeam == id) && o.GameDateTime.Month.ToString() == Month);

        //    foreach (Schedule item in schedules)
        //    {
        //        TeamSchedule ts = new TeamSchedule();
        //        ts.GameDateTime = item.GameDateTime;
        //        ts.Home_Team = db.Teams.Find(item.HomeTeam);
        //        ts.Road_Team = db.Teams.Find(item.RoadTeam);
        //        ts.HomeScore = item.HomeScore;
        //        ts.RoadScore = item.RoadScore;

        //        team_schedules.Add(ts);
        //    }

        //    if (team != null)
        //    {
        //        model.ID = id;
        //        model.Name = team.Name;
        //        model.Players = Roster.FindAll(o => o.TeamID == id);
        //        model.Schedules = team_schedules;
        //        model.ShowMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(System.Convert.ToInt32(Month));
        //    }
        //    else
        //        return View(team);

        //    return View(model);
        //}

        //
        // GET: /Team/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        //
        // POST: /Team/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Team team = db.Teams.Find(id);
            db.Teams.Remove(team);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}