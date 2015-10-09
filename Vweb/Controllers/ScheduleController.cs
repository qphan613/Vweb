using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vweb.Models;

namespace Vweb.Controllers
{
    public class ScheduleController : Controller
    {
        private YiaContext db = new YiaContext();

        //
        // GET: /Schedule/

        public ActionResult Index()
        {
            return View(db.Schedules.ToList());
        }

        //
        // GET: /Schedule/Details/5

        public ActionResult Details(int id = 0)
        {
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        //
        // GET: /Schedule/Create

        public ActionResult Create()
        {

            List<Team> sTeams = db.Teams.ToList();
            ViewBag.Teams = sTeams;

            return View();
        }

        //
        // POST: /Schedule/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Schedule schedule)
        {

            schedule.RoadScore = 0;
            schedule.HomeScore = 0;

            if (ModelState.IsValid)
            {
                db.Schedules.Add(schedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(schedule);
        }

        //
        // GET: /Schedule/Edit/5
        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            Schedule schedule = db.Schedules.Find(id);

            if (schedule == null)
            {
                return HttpNotFound();
            }

            List<Team> sTeams = db.Teams.ToList();
            ViewBag.Teams = sTeams;

            return View(schedule);
        }

        //
        // POST: /Schedule/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(schedule);
        }

        //
        // GET: /Schedule/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        //
        // POST: /Schedule/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Schedule schedule = db.Schedules.Find(id);
            db.Schedules.Remove(schedule);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DisplaySchedule(int id = 0, string Month = "0")
        {
            TeamSchedule ts;
            List<TeamSchedule> Model = new List<TeamSchedule>();


            List<Schedule> schedules = new List<Schedule>();

            if (id == 0)
            {
                if (Month == "0")
                    schedules = db.Schedules.ToList();
                else
                    schedules = db.Schedules.ToList().FindAll(o => o.GameDateTime.Month.ToString() == Month);
            }
            else
            {
                schedules = db.Schedules.ToList().FindAll(o => (o.HomeTeam == id || o.RoadTeam == id));
            }

            foreach (Schedule item in schedules)
            {
                ts = new TeamSchedule();
                ts.GameDateTime = item.GameDateTime;
                ts.Home_Team = db.Teams.Find(item.HomeTeam);
                ts.Road_Team = db.Teams.Find(item.RoadTeam);
                ts.HomeScore = item.HomeScore;
                ts.RoadScore = item.RoadScore;

                Model.Add(ts);
            }
            return View(Model);
        }



        //public ActionResult View(string Month = "0")
        //{
        //    List<TeamSchedule> team_schedules = new List<TeamSchedule>();


        //    //Find out if the View is for specific month.
        //    if (Month == "0")
        //    {
        //        Month = DateTime.Now.Month.ToString();
        //    }

        //    //Grab all the schedule for this team.
        //    List<Schedule> schedules = db.Schedules.ToList().FindAll(o => o.GameDateTime.Month.ToString() == Month);

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
        //    return View(team_schedules);
        //}
        public ActionResult UpdateScore(int HomeTeamID, int RoadTeamID, DateTime date)
        {
            TeamSchedule Model = new TeamSchedule();
            //QP
            Schedule schedule = db.Schedules.ToList().FindAll(o => (o.HomeTeam == HomeTeamID &&
                o.RoadTeam == RoadTeamID &&
                o.GameDateTime == date)).First();

            Model.GameDateTime = schedule.GameDateTime;
            Model.Home_Team = db.Teams.Find(schedule.HomeTeam);
            Model.Road_Team = db.Teams.Find(schedule.RoadTeam);
            Model.HomeScore = schedule.HomeScore;
            Model.RoadScore = schedule.RoadScore;

            return PartialView(Model);

        }

        [HttpPost]
        public ActionResult UpdateScore(TeamSchedule Model)
        {
            Schedule schedule = db.Schedules.ToList().FindAll(o => (o.HomeTeam == Model.Home_Team.ID &&
                o.RoadTeam == Model.Road_Team.ID &&
                o.GameDateTime == Model.GameDateTime)).First();

            schedule.HomeScore = Model.HomeScore;
            schedule.RoadScore = Model.RoadScore;

            db.Entry(schedule).State = EntityState.Modified;
            db.SaveChanges();

            

            Record RoadTeamRecord = db.Records.ToList().Find(o => o.TeamID == Model.Road_Team.ID);
            Record HomeTeamRecord = db.Records.ToList().Find(o => o.TeamID == Model.Home_Team.ID);

            RoadTeamRecord.Win = RoadTeamRecord.Win + Model.RoadScore;
            RoadTeamRecord.Lost = RoadTeamRecord.Lost + Model.HomeScore;

            HomeTeamRecord.Win = HomeTeamRecord.Win + Model.HomeScore;
            HomeTeamRecord.Lost = HomeTeamRecord.Lost + Model.RoadScore;

            db.Entry(RoadTeamRecord).State = EntityState.Modified;
            db.SaveChanges();

            db.Entry(HomeTeamRecord).State = EntityState.Modified;
            db.SaveChanges();


            return RedirectToAction("Standing","Home");

        }

        public ActionResult EditScore(int HomeTeamID, int RoadTeamID, DateTime date)
        {
            TeamSchedule Model = new TeamSchedule();
            //QP
            Schedule schedule = db.Schedules.ToList().FindAll(o => (o.HomeTeam == HomeTeamID &&
                o.RoadTeam == RoadTeamID &&
                o.GameDateTime == date)).First();

            Model.GameDateTime = schedule.GameDateTime;
            Model.Home_Team = db.Teams.Find(schedule.HomeTeam);
            Model.Road_Team = db.Teams.Find(schedule.RoadTeam);
            Model.HomeScore = schedule.HomeScore;
            Model.RoadScore = schedule.RoadScore;

            return PartialView(Model);
        }

        [HttpPost]
        public ActionResult EditScore(TeamSchedule Model)
        {
            Schedule schedule = db.Schedules.ToList().FindAll(o => (o.HomeTeam == Model.Home_Team.ID &&
                o.RoadTeam == Model.Road_Team.ID &&
                o.GameDateTime == Model.GameDateTime)).First();

            //Take out the previous score from the standing.
            Record RoadTeamRecord = db.Records.ToList().Find(o => o.TeamID == Model.Road_Team.ID);
            Record HomeTeamRecord = db.Records.ToList().Find(o => o.TeamID == Model.Home_Team.ID);

            RoadTeamRecord.Lost = RoadTeamRecord.Lost  - schedule.HomeScore;
            RoadTeamRecord.Win = RoadTeamRecord.Win - schedule.RoadScore;

            HomeTeamRecord.Lost = HomeTeamRecord.Lost - schedule.RoadScore;
            HomeTeamRecord.Win = HomeTeamRecord.Win - schedule.HomeScore;


            //Now readjust the standing with the latest Score Edit.
            schedule.HomeScore = Model.HomeScore;
            schedule.RoadScore = Model.RoadScore;

            db.Entry(schedule).State = EntityState.Modified;
            db.SaveChanges();

            RoadTeamRecord.Win = RoadTeamRecord.Win + Model.RoadScore;
            RoadTeamRecord.Lost = RoadTeamRecord.Lost + Model.HomeScore;

            HomeTeamRecord.Win = HomeTeamRecord.Win + Model.HomeScore;
            HomeTeamRecord.Lost = HomeTeamRecord.Lost + Model.RoadScore;

            db.Entry(RoadTeamRecord).State = EntityState.Modified;
            db.SaveChanges();

            db.Entry(HomeTeamRecord).State = EntityState.Modified;
            db.SaveChanges();


            return RedirectToAction("Standing", "Home");
        }

        //public ActionResult PartialUpdate()
        //{
        //    return View();
        //}

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}