using System.Collections.Generic;
using System.Linq;
using ActivityCenter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;

namespace ActivityCenter.Controllers
{
    public class PlanController : Controller
    {
        private int? uid
        {
            get
            {
                return HttpContext.Session.GetInt32("userId");
            }
        }

        private bool isLoggedIn
        {
            get
            {
                return uid != null;
            }
        }

        private ActivityCenterContext db;

        public PlanController(ActivityCenterContext context)
        {
            db = context;
        }
        
        [HttpGet("home")]
        public IActionResult Dashboard()
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Plan> AllPlans = db.Plans
                .Include(p => p.Coordinator)
                .Include(p => p.PlanUser)
                .OrderBy(p => p.Date)
                .ToList();
            User currentUser = db.Users.FirstOrDefault(u => u.UserId == (int)uid);
            ViewBag.CurrentUser = currentUser;
            return View("Dashboard", AllPlans);
        }

        [HttpGet("new")]
        public IActionResult New()
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("New");
        }

        [HttpPost("create")]
        public IActionResult Create(Plan newPlan)
        {
            if (ModelState.IsValid)
            {
                DateTime planDate = newPlan.Date;
                if ((DateTime.Now - planDate).TotalMinutes > 0)
                {
                    ModelState.AddModelError("Date", "The date must be a future date.");
                    return View("New");
                }
                newPlan.UserId = (int)uid;
                db.Plans.Add(newPlan);
                db.SaveChanges();
                return RedirectToAction("Details", "Plan", new { planId = newPlan.PlanId });
            }
            return View("New");
        }

        [HttpGet("activity/{planId}")]
        public IActionResult Details(int planId)
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            Plan currentPlan = db.Plans
                .Include(p => p.Coordinator)
                .Include(p => p.PlanUser)
                .ThenInclude(pu => pu.User)
                .FirstOrDefault(p => p.PlanId == planId);
            return View("PlanDetails", currentPlan);
        }

        [HttpPost("activity/{planId}")]
        public IActionResult CreateParticipant(Participant newParticipant, int planId)
        {
            newParticipant.UserId = (int)uid;
            newParticipant.PlanId = planId;
            db.Participants.Add(newParticipant);
            db.SaveChanges();
            return RedirectToAction("Details", "Plan", new { planId = planId });
        }

        [HttpGet("{participantId}/remove")]
        public IActionResult Unparticipate(int participantId)
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            Participant participantToDelete = db.Participants.FirstOrDefault(part => part.ParticipantId == participantId);
            db.Participants.Remove(participantToDelete);
            db.SaveChanges();
            return RedirectToAction("Dashboard", "Plan");
        }

        [HttpGet("{planId}/delete")]
        public IActionResult Delete(int planId)
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            Plan planToDelete = db.Plans.FirstOrDefault(p => p.PlanId == planId);
            db.Plans.Remove(planToDelete);
            db.SaveChanges();
            return RedirectToAction("Dashboard", "Plan");
        }
    }
}