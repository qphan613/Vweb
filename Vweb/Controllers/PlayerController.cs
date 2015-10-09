using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vweb.Models
{
    public class PlayerController : Controller
    {
        private YiaContext db = new YiaContext();

        //
        // GET: /Player/

        public ActionResult Index()
        {
            List<Team> sTeams = db.Teams.ToList();
            List<Player>  sPlayers = db.Players.ToList();
            List<DisplayPlayer> sDisplayPlayer = new List<DisplayPlayer>();
            
            ViewBag.Teams = sTeams;

            foreach (Player p in sPlayers)
            {
                DisplayPlayer player = new DisplayPlayer();
                player.ID = p.ID;
                player.LastName = p.LastName;
                player.Sex = p.Sex;
                player.TeamID = p.TeamID;
                player.FirstName = p.FirstName;
                player.TeamName = sTeams.Find(o => o.ID == p.TeamID).Name;

                sDisplayPlayer.Add(player);
                     
            }

            return View(sDisplayPlayer);
        }

        //
        // GET: /Player/Details/5

        public ActionResult Details(int id = 0)
        {
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        //
        // GET: /Player/Create

        public ActionResult Create()
        {
            List<Team> sTeams = db.Teams.ToList();
            ViewBag.Teams = sTeams;

            return View();
        }

        //
        // POST: /Player/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Player player)
        {
            if (ModelState.IsValid)
            {
                db.Players.Add(player);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            List<Team> sTeams = db.Teams.ToList();
            ViewBag.Teams = sTeams;

            return View(player);
        }

        //
        // GET: /Player/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }

            List<Team> sTeams = db.Teams.ToList();
            ViewBag.Teams = sTeams;

            return View(player);
        }

        //
        // POST: /Player/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Player player)
        {
            if (ModelState.IsValid)
            {
                db.Entry(player).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(player);
        }

        //
        // GET: /Player/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        //
        // POST: /Player/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Player player = db.Players.Find(id);
            db.Players.Remove(player);
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