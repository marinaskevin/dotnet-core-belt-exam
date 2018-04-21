
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
                return RedirectToAction("Index","Activity");
            }
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            return View();
        }

        public IActionResult Create(Register user)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            if(ModelState.IsValid)
            {
                bool EmailMatch = _context.Users.Where(x => x.Email == user.Email).Any();
                if(EmailMatch)
                {
                    ModelState.AddModelError("Email","Registered user with email "+user.Email+" already exists.");
                    return View("Index");
                }
                User newUser = new User(user);
                _context.Add(newUser);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("UserId",newUser.UserId);
                return RedirectToAction("Index","Activity");
            }
            return View("Index");
        }

        [HttpPost]
        public IActionResult Login(Login user)
        {
            bool EmailMatch = _context.Users.Where(x => x.Email == user.LoginEmail).Any();
            bool PasswordMatch = _context.Users.Where(x => x.Email == user.LoginEmail && x.Password == user.LoginPassword).AsEnumerable().Any();
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            if(ModelState.IsValid)
            {
                if(EmailMatch && PasswordMatch)
                {
                    int UserId = _context.Users.Where(x => x.Email == user.LoginEmail).SingleOrDefault().UserId;
                    HttpContext.Session.SetInt32("UserId",UserId);
                    return RedirectToAction("Index","Activity");
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
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
