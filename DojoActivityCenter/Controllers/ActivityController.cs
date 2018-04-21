
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using DojoActivityCenter.Models;

namespace DojoActivityCenter.Controllers
{
    public class ActivityController : Controller
    {

        // DATABASE

        private readonly ActivityContext _context;
 
        public ActivityController(ActivityContext context)
        {
            _context = context;
        }

        // PRIVATE METHODS

        private bool UserLoggedIn()
        {
            var UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        [Route("/Home")]
        public IActionResult Index()
        {
            if(!UserLoggedIn())
            {
                return RedirectToAction("Index","User");
            }
            int UserId = (int)HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = UserId;
            ViewBag.User = _context.Users.SingleOrDefault(x => x.UserId == UserId);
            ViewBag.Users = _context.Users.ToList();
            ViewBag.Events = _context.Events.OrderBy(x => x.Date);
            ViewBag.RSVPs = _context.RSVPs.Where(x => x.UserId == UserId);
            ViewBag.AllRSVPs = _context.RSVPs.ToList();

            return View();
        }

        [Route("/Activity/{EventId}/Delete")]
        public IActionResult Delete(int EventId)
        {
            if(!UserLoggedIn())
            {
                return RedirectToAction("Index","User");
            }
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            var Event = _context.Events.SingleOrDefault(x => x.EventId == EventId);
            if(Event != null)
            {
                _context.Events.Remove(Event);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [Route("/Activity/{EventId}",Name="View Event")]
        public IActionResult Show(int EventId)
        {
            if(!UserLoggedIn())
            {
                return RedirectToAction("Index","User");
            }
            int UserId = (int)HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = UserId;
            ViewBag.User = _context.Users.SingleOrDefault(x => x.UserId == UserId);
            ViewBag.Event = _context.Events.SingleOrDefault(x => x.EventId == EventId);
            ViewBag.RSVPs = _context.RSVPs.Where(x => x.EventId == EventId);
            ViewBag.EventGuests = _context.Events.Include(g => g.Guests).ThenInclude(h => h.User).Where(g => g.EventId == EventId).SingleOrDefault();
            return View();
        }

        [Route("/New")]
        public IActionResult New()
        {
            if(!UserLoggedIn())
            {
                return RedirectToAction("Index","User");
            }
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            return View();
        }

        [Route("/Activity/Create")]
        public IActionResult Create(Event activity)
        {
            if(!UserLoggedIn())
            {
                return RedirectToAction("Index","User");
            }
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            if(ModelState.IsValid)
            {
                _context.Add(activity);
                _context.SaveChanges();
                return RedirectToRoute("View Event",new { EventId = activity.EventId });
            }
            return View("New");
        }

        [Route("/Activity/{EventId}/RSVP/Create")]
        public IActionResult RSVP(int EventId)
        {
            if(!UserLoggedIn())
            {
                return RedirectToAction("Index","User");
            }
            int UserId = (int)HttpContext.Session.GetInt32("UserId");
            RSVP newRSVP = new RSVP{
                EventId = EventId,
                UserId = UserId
            };
            ViewBag.UserId = UserId;
            _context.RSVPs.Add(newRSVP);
            _context.SaveChanges();
            return RedirectToAction("Index","Activity");
        }

        [Route("/Activity/{EventId}/RSVP/Delete")]

        public IActionResult UNRSVP(int EventId)
        {
            if(!UserLoggedIn())
            {
                return RedirectToAction("Index","User");
            }
            int UserId = (int)HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = UserId;
            var RSVP = _context.RSVPs.Where(x => x.UserId == UserId && x.EventId == EventId).SingleOrDefault();
            _context.RSVPs.Remove(RSVP);
            _context.SaveChanges();
            return RedirectToAction("Index","Activity");
        }
    }
}
