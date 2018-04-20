using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BeltLogin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BeltLogin.Controllers
{
    public class ActivitieController : Controller
    {

        private BeltContext _context;

        public ActivitieController(BeltContext context){
            _context = context;
        }

        [HttpGet]
        [Route("home")]
        public IActionResult Index(int urlid)
        {
            // check if logged in, otherwise reset to index page.
            var sessuid = HttpContext.Session.GetInt32("user_id");
            if (sessuid == null)
            {
                return RedirectToAction("Index", "Home");
            }
            User User = _context.Users.SingleOrDefault(a => a.UserId == sessuid);
            ViewBag.user = User;
            List<Activitie> ActList = _context.Activities.Include(b => b.Creator).Include(p => p.Participant).ThenInclude(u => u.PartId).ToList();
            ActList.OrderByDescending(d => d.Date);
            ViewBag.activities = ActList;
            return View("Index");
        }

        [HttpGet]
        [Route("new")]
        public IActionResult NewActivity()
        {
            // check if logged in, otherwise reset to index page.
            var sessuid = HttpContext.Session.GetInt32("user_id");
            if (sessuid == null)
            {
                return RedirectToAction("Index", "Home");
            }
            User User = _context.Users.SingleOrDefault(a => a.UserId == sessuid);
            ViewBag.user = User;
            return View("New");
        }

        [HttpPost]
        [Route("CreateActivity")]
        public IActionResult CreateActivity(ValidActivitie activitie)
        {
            if(ModelState.IsValid)
            {
                if (activitie.Date < DateTime.Now)
                {
                    ModelState.AddModelError("Date", "Date must be in the future.");
                    return View("New");
                }
                else{
                    // go through all of this users events and see if the requested time conflicts with any of their current times
                    // set conflictCheck to 1 if conflict, or 0 if no conflict
                    int conflict = 0;
                    var sessuid = HttpContext.Session.GetInt32("user_id");
                    List<Activitie> ActList = _context.Activities.Include(p => p.Participant).ThenInclude(u => u.PartId).ToList();
                    System.Console.WriteLine("Before crazy FOreach Bug..... ");
                    foreach(var item in ActList)
                    {
                        if (item.Date.Day == activitie.Date.Day && item.Date.Hour == item.Date.Hour && item.Date.Minute == activitie.Date.Minute && item.Date.Month == activitie.Date.Month)
                        {
                            conflict = 1;
                        }
                        // if (item.DurationMod == "Hours"){
                        //     int Temp = item.Date.AddHours(item.Duration);
                        //     {
                        //         if(item.Date.Hour < activitie.Date.Hour && Temp > activitie.Date.Hour)
                        //         {
                        //             conflict = 1;
                        //         }

                        //     }
                        // }
                        //  if (item.DurationMod == "minutes"){
                        //     //  add minutes and calc
                        //  }
                        //  if (item.DurationMod == "Days")
                        //  {
                        //     //  add days and calc
                        //  }
                    }
                    if (conflict == 1)
                    {
                        ModelState.AddModelError("Date", "You're already committed at that time!");
                        return View("New");
                    }
                    else
                    {
                        BuildActivity(activitie);
                        return RedirectToAction("Index");
                    }
                }
            }
            return RedirectToAction("NewActivity");
        }

        private void BuildActivity(ValidActivitie activity)
        {

            Activitie NewActivity = new Activitie{
                Title = activity.Title,
                Description = activity.Description,
                Date = activity.Date,
                Time = activity.Time,
                Duration = activity.Duration,
                DurationMod = activity.DurationMod,
                CreatorId = activity.CreatorId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _context.Activities.Add(NewActivity);
            _context.SaveChanges();
        }

        [HttpGet]
        [Route("activity/{actid}")]
        public IActionResult ShowActivitie(int actid)
        {
            // check if logged in, otherwise reset to index page.
            var sessuid = HttpContext.Session.GetInt32("user_id");
            if (sessuid == null)
            {
                return RedirectToAction("Index", "Home");
            }
            User User = _context.Users.SingleOrDefault(a => a.UserId == sessuid);
            ViewBag.user = User;
            // query for activity and participant list.
            Activitie Activitie = _context.Activities.Include(b => b.Creator).Include(p => p.Participant).ThenInclude(u => u.PartId).SingleOrDefault(a =>a.ActivitieId == actid);
            ViewBag.act = Activitie;
            // need viewbag to check if logged in user matches the participant list

            return View("Activitie");
        }

        [HttpPost]
        [Route("JoinActivity")]
        public IActionResult JoinActivity(int actid, int participantId)
        {
            Participant NewPart = new Participant{
                Users_UserId = participantId,
                Activities_ActivitieId = actid
            };
            _context.Participants.Add(NewPart);
            _context.SaveChanges();
            int b = actid;
            return RedirectToAction("ShowActivitie", "Activitie", new{actid = b});
        }

        [HttpPost]
        [Route("DeleteActivity")]
        public IActionResult DeleteActivity(int actId)
        {
            Activitie ThisAct = _context.Activities.SingleOrDefault(a => a.ActivitieId == actId);
            _context.Activities.Remove(ThisAct);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("LeaveActivity")]
        public IActionResult LeaveActivity(int actId)
        {
            var sessuid = HttpContext.Session.GetInt32("user_id");
            User User = _context.Users.SingleOrDefault(a => a.UserId == sessuid);
            List<Participant> ThisPart = _context.Participants.Where(u => u.Activities_ActivitieId == actId).ToList();
            foreach (var part in ThisPart)
            {
                if (part.Users_UserId == sessuid)
                {
                    _context.Participants.Remove(part);
                    _context.SaveChanges();
                    int b = actId;
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }



        [HttpGet]
        [Route("logoff")]
        public IActionResult LogOff()
        {
            System.Console.WriteLine("Inside LogOff route");
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

         public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
