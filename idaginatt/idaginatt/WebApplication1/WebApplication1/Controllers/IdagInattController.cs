using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class IdagInattController : Controller
    {
        int tempID;
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
        [HttpGet]
        public IActionResult InsertNominee()
        {
            return View();
        }
        [HttpPost]
        public IActionResult InsertNominee(NomineeDetail nd)
        {
          
                NomineeMethod nm = new NomineeMethod();
                int i = 0;
                string error = "";
                i = nm.InsertNominee(nd, out error);
                ViewBag.error = error;
                
                return RedirectToAction("NomineeList"); 
            
        }

        public IActionResult NomineeList(int year) { 
            List<NomineeDetail> NomineeList = new List<NomineeDetail>();
            NomineeMethod nm = new NomineeMethod();
            string error = "";
            NomineeList = nm.GetNomineeList(out error);
            ViewBag.error = error;
            HttpContext.Session.SetInt32("year", year);
            ViewBag.year = Convert.ToInt32(HttpContext.Session.GetInt32("year"));

            return View(NomineeList);
        }
      

        [HttpGet]
        public IActionResult DeleteNominee(int id)
        {
            NomineeMethod nm = new NomineeMethod();
            NomineeDetail nd = new NomineeDetail();
            nd = nm.GetNomineeById(id, out string errormsg);
            return View(nd);
        }

        [HttpPost, ActionName("DeleteNominee")]
        public IActionResult DeleteConfirmed(int id)
        {
            NomineeMethod nm = new NomineeMethod();
            string error = "";
            int i = 0;
            i = nm.DeleteNominee(id, out error);
            return RedirectToAction("NomineeList");
        }
        public ActionResult NomineeDetails(int id)
        {
            NomineeMethod nm = new NomineeMethod();
            NomineeDetail nd = new NomineeDetail();
            nd = nm.GetNomineeById(id, out string errormsg);


            return View(nd);
        }
        public IActionResult NomineesToVoteOn()
        {
            int year = Convert.ToInt32(HttpContext.Session.GetInt32("year"));

            List<NomineeDetail> NomineeList = new List<NomineeDetail>();
            NomineeMethod nm = new NomineeMethod();
            string error = "";
            NomineeList = nm.GetNomineeListByYear(year,out error);
            ViewBag.error = error;
            return View(NomineeList);
        }
        [HttpGet]
        public IActionResult VoteSite(int id)
        {
            NomineeMethod nm = new NomineeMethod();
            NomineeDetail nd = nm.GetNomineeById(id, out string errormsg);
            ViewBag.Name = nd.Nominee_FirstName + nd.Nominee_LastName;
            tempID = nd.Nominee_Id;
            ViewBag.voteId = tempID;
            HttpContext.Session.SetInt32("extraInfo",tempID);
            return View();
        }
        [HttpPost]
        public IActionResult VoteSite(VoteDetail vd)
        {
            
            VoteMethod vm = new VoteMethod();
            int i = 0;
            string error = "";
            int temp = Convert.ToInt32(HttpContext.Session.GetInt32("extraInfo"));

            i = vm.InsertVote(vd,temp, out error);
            ViewBag.error = error;
            return RedirectToAction("NomineesToVoteOn");

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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserDetail ud)
        {
            UserMethod um = new UserMethod();
            string error = "";
            if (um.LogIn(ud.User_UserName, ud.User_Password, out error) == true)
            {
                return View("Index");
            }
            ud.LogInErrorMessage = error;
            return View("Login", ud);
        }

        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminLogin(UserDetail ud)
        {
            UserMethod um = new UserMethod();
            string error = "";
            if (um.LogIn(ud.User_UserName, ud.User_Password, out error) == true)
            {
                return View("Index");
            }
            ud.LogInErrorMessage = error;
            return View("AdminLogin", ud);
        }

    }
}
