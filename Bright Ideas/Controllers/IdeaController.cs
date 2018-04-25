using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Bright_Ideas.Models;

namespace Bright_Ideas.Controllers
{
    public class IdeaController : Controller
    {
        private readonly IdeaContext _context;
 
        public IdeaController(IdeaContext context)
        {
            _context = context;
        }

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

        [HttpGet]
        [Route("/bright_ideas")]
        public IActionResult Index()
        {
            if(!UserLoggedIn())
            {
                return RedirectToAction("Index","User");
            }
            int UserId = (int)HttpContext.Session.GetInt32("UserId");
            ViewBag.Bright_Ideas = true;
            ViewBag.User = _context.Users.SingleOrDefault(x => x.UserId == UserId);
            //ViewData["Ideas"] = _context.Ideas.OrderByDescending(x => x.Likes).Include(x => x.UserId);
            ViewBag.Ideas = _context.Ideas.Include(x => x.User).OrderByDescending(x => x.Likes.Count).ToList();
            ViewBag.Likes = _context.Likes.Include(x => x.User).ToList();
            ViewBag.Liked = _context.Likes.Where(x => x.UserId == UserId).Include(x => x.User).ToList();
            return View();
        }

        [HttpGet]
        [Route("/bright_ideas/{IdeaId}",Name="View Idea")]
        public IActionResult Idea(int IdeaId)
        {
            if(!UserLoggedIn())
            {
                return RedirectToAction("Index","User");
            }
            int UserId = (int)HttpContext.Session.GetInt32("UserId");
            ViewBag.User = _context.Users.SingleOrDefault(x => x.UserId == UserId);
            ViewBag.Idea = _context.Ideas.Include(x => x.User).SingleOrDefault(x => x.IdeaId == IdeaId);
            ViewBag.Likes = _context.Likes.Where(x => x.IdeaId == IdeaId).Include(x => x.User).ToList();
            return View();
        }

        [HttpPost]
        [Route("/bright_ideas/create")]
        public IActionResult Create(Idea idea)
        {
            if(!UserLoggedIn())
            {
                return RedirectToAction("Index","User");
            }
            if(ModelState.IsValid)
            {
                _context.Add(idea);
                _context.SaveChanges();
                int IdeaId = idea.IdeaId;
                int UserId = (int)HttpContext.Session.GetInt32("UserId");
                var Like = _context.Likes.Where(x => x.UserId == UserId && x.IdeaId == IdeaId).SingleOrDefault();
                return RedirectToRoute("View Idea",new { IdeaId = IdeaId });
            }
            return RedirectToAction("Index","Idea");
        }

        [HttpGet]
        [Route("like/{IdeaId}/create")]
        public IActionResult Like(int IdeaId)
        {
            if(!UserLoggedIn())
            {
                return RedirectToAction("Index","User");
            }
            int UserId = (int)HttpContext.Session.GetInt32("UserId");
            Like NewLike = new Like{
                UserId = UserId,
                IdeaId = IdeaId
            };
            var Like = _context.Likes.Where(x => x.UserId == NewLike.UserId && x.IdeaId == NewLike.IdeaId).SingleOrDefault();
            if(Like == null)
            {
                _context.Likes.Add(NewLike);
                _context.SaveChanges();
            }
            return RedirectToAction("Index","Idea");
        }

        [HttpGet]
        [Route("/bright_ideas/{IdeaId}/delete")]
        public IActionResult DeleteIdea(int IdeaId)
        {
            if(!UserLoggedIn())
            {
                return RedirectToAction("Index","User");
            }
            int UserId = (int)HttpContext.Session.GetInt32("UserId");
            var Idea = _context.Ideas.Where(x => x.UserId == UserId && x.IdeaId == IdeaId).SingleOrDefault();
            if(Idea != null)
            {
                _context.Ideas.Remove(Idea);
                _context.SaveChanges();
            }
            return RedirectToAction("Index","Idea");
        }

        [HttpGet]
        [Route("/like/{LikeId}/delete")]
        public IActionResult DeleteLike(int LikeId)
        {
            if(!UserLoggedIn())
            {
                return RedirectToAction("Index","User");
            }
            int UserId = (int)HttpContext.Session.GetInt32("UserId");
            var Like = _context.Likes.Where(x => x.UserId == UserId && x.LikeId == LikeId).SingleOrDefault();
            if(Like != null)
            {
                _context.Likes.Remove(Like);
                _context.SaveChanges();
            }
            return RedirectToAction("Index","Idea");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}