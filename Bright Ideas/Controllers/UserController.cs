using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Bright_Ideas.Models;  
using Microsoft.EntityFrameworkCore;
namespace Bright_Ideas.Controllers
{
    public class UserController : Controller
    {
        private readonly UserContext _context;
 
        public UserController(UserContext context)
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

        public IActionResult Index()
        {
            if(UserLoggedIn())
            {
                return RedirectToAction("Index","Idea");
            }
            return View();
        }

        [HttpPost]
        [Route("/register")]
        public IActionResult Create(Register user)
        {
            if(ModelState.IsValid)
            {
                bool EmailMatch = _context.Users.Where(x => x.Email == user.Email).Any();
                if(EmailMatch)
                {
                    ModelState.AddModelError("Email","Registered user with email "+user.Email+" already exists.");
                    return View("Index");
                }
                User NewUser = new User(user);
                _context.Add(NewUser);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("UserId",NewUser.UserId);
                return RedirectToAction("Index","Idea");
            }
            return View("Index");
        }

        [HttpPost]
        [Route("/login")]
        public IActionResult Login(Login user)
        {
            bool EmailMatch = _context.Users.Where(x => x.Email == user.LoginEmail).Any();
            bool PasswordMatch = _context.Users.Where(x => x.Email == user.LoginEmail && x.Password == user.LoginPassword).AsEnumerable().Any();
            if(ModelState.IsValid)
            {
                if(EmailMatch && PasswordMatch)
                {
                    int UserId = _context.Users.Where(x => x.Email == user.LoginEmail).SingleOrDefault().UserId;
                    HttpContext.Session.SetInt32("UserId",UserId);
                    return RedirectToAction("Index","Idea");
                }
                else if(!EmailMatch)
                {
                    ModelState.AddModelError("LoginEmail","No registered user with email "+user.LoginEmail+" exists.");
                }
                else if(!PasswordMatch)
                {
                    ModelState.AddModelError("LoginPassword","Incorrect Password");
                }
            }
            else{
                if(!EmailMatch)
                {
                    if (ModelState.ContainsKey("LoginPassword"))
                    {
                        ModelState["LoginPassword"].Errors.Clear();
                    }
                    ModelState.AddModelError("LoginEmail","No registered user with email "+user.LoginEmail+" exists.");
                }
            }
            return View("Index");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("/users/{ViewUserId}")]
        public IActionResult ViewUser(int ViewUserId)
        {
            if(UserLoggedIn())
            {
                int UserId = (int)HttpContext.Session.GetInt32("UserId");
                ViewBag.User = _context.Users.SingleOrDefault(x => x.UserId == UserId);
                var ViewUser = _context.Users.Where(x => x.UserId == ViewUserId).SingleOrDefault();
                if(ViewUser != null)
                {
                    ViewBag.ViewUser = ViewUser;
                    ViewBag.NumIdeas = _context.Ideas.Count(x => x.UserId == ViewUserId);
                    ViewBag.NumLikes = _context.Likes.Count(x => x.UserId == ViewUserId);
                    return View("User");
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }
   }
}