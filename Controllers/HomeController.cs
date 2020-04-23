using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
		public HomeController(MyContext context)
		{
			dbContext = context;
		}
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("Register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();
                HttpContext.Session.SetString("FirstName", newUser.FirstName);
                HttpContext.Session.SetInt32("UserId", newUser.UserId);
                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUser userSubmission)
        {
            if(ModelState.IsValid)
            {
                // List<User>AllUsers=dbContext.Users.ToList();
                var hasher = new PasswordHasher<LoginUser>();
                var signedInUser = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.LoginEmail);
                
                if(signedInUser == null)
                {
                    ViewBag.Message="Email/Password is invalid";
                    return View("Index");
                }
                else
                {
                    var result = hasher.VerifyHashedPassword(userSubmission, signedInUser.Password, userSubmission.LoginPassword);
                    if(result==0)
                    {
                        ViewBag.Message="Email/Password is invalid";
                        return View("Index");
                    }
                }
                
                
                HttpContext.Session.SetInt32("UserId", signedInUser.UserId);
                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("Index");
            }
            
        }
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            
            User userInDb=dbContext.Users.FirstOrDefault(u=>u.UserId==(int)HttpContext.Session.GetInt32("UserId"));
            List<Wedding> AllWeddings=dbContext.Weddings.Include(w=>w.Creator).Include(w=>w.Guests).ThenInclude(c=>c.NavUser).ToList();
            if(userInDb==null)
            {
                return RedirectToAction("Logout");
            }
            else
            {
                ViewBag.User=userInDb;
                return View(AllWeddings);
            }
        }
        [HttpGet("wedding/new")]
        public IActionResult AddWed()
        {
            return View();
        }

        [HttpPost("create/wedding")]
        public IActionResult Create(Wedding newWedding)
        {
            if(ModelState.IsValid)
            {
                int? userInDb=HttpContext.Session.GetInt32("UserId");
                newWedding.UserId= (int)userInDb;
                dbContext.Weddings.Add(newWedding);
                dbContext.SaveChanges();
                return Redirect($"/show/{newWedding.WedId}");
            }
            else
            {
                return View("AddWed");
            }
        }
        [HttpGet("Cancel/{WedId}")]
        public IActionResult CancelWedding (int WedId)
        {
            Wedding toCancel = dbContext.Weddings.FirstOrDefault(f=>f.WedId == WedId);
            dbContext.Weddings.Remove(toCancel);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpGet("notGoing/{WedId}/{UserId}")]
        public IActionResult NotGoing(int WedId, int UserId)
        {
            Card didCancel = dbContext.Cards.FirstOrDefault(d=>d.UserId == UserId && d.WedId==WedId);
            dbContext.Cards.Remove(didCancel);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpGet("Join/{WedId}/{UserId}")]
        public IActionResult Join(int WedId, int UserId)
        {
            Card didJoin = new Card();
            didJoin.UserId = UserId;
            didJoin.WedId = WedId;
            dbContext.Cards.Add(didJoin);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("show/{WedId}")]
        public IActionResult Display(int WedId)
        {
            Wedding retrievedWed=dbContext.Weddings
                                            .Include(f=>f.Guests).
                                            ThenInclude(f=>f.NavUser)
                                            .FirstOrDefault(w=>w.WedId==WedId);
            return View(retrievedWed);
        }
        

















        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
