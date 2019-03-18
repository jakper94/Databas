﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using System.Web;
using Microsoft.IdentityModel.Protocols;


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
            if(year != 0) {
            HttpContext.Session.SetInt32("year", year);
            }
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

            ViewBag.Name = HttpContext.Session.GetString("UserID");
            if (ViewBag.Name = null)
            {
                return View("Login");
            }
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


        [HttpGet]
        public IActionResult NomineeScore()
        {
            List<NomineeDetail> NomineeList = new List<NomineeDetail>();  
            NomineeMethod nm = new NomineeMethod();

            NomineeList = nm.GetNomineeListWithVotes(out string msg1);
                     
            return View(NomineeList);
        }
        [HttpPost]
        public IActionResult NomineeScore(string ByYear)
        {
            int i = Convert.ToInt32(ByYear);
            List<NomineeDetail> NomineeList = new List<NomineeDetail>();
            NomineeMethod nm = new NomineeMethod();
            NomineeList = nm.GetNomineeListWithVotes(out string error);
            ViewData["ByYear"] = ByYear;
            return View(NomineeList);
        }
        public IActionResult Motivations(int id)
        {
            NomineeMethod nm = new NomineeMethod();
            NomineeDetail nd = nm.GetNomineeById(id, out string errormsg);
            List<VoteDetail> voteList = new List<VoteDetail>();
            VoteMethod vm = new VoteMethod();
            voteList = vm.GetMotivationListById(id, out string msg);
            ViewBag.Name = nd.Nominee_FirstName + " " + nd.Nominee_LastName;
            return View(voteList);
        }
        public IActionResult Admin()
        {
            return View();
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
            if (um.LogIn(ud.User_UserName, out error) == true)
            {
                HttpContext.Session.SetString("UserID", ud.User_FirstName);
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
            if (um.AdminLogIn(ud.User_UserName, ud.User_Password, out error) == true)
            {
                return View("Index");
            }
            ud.LogInErrorMessage = error;
            return View("AdminLogin", ud);
        }

        [HttpGet]
        public IActionResult Attend()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Attend(AttendingDetail ad)
        {
            string error = "";
            AttendingMethod am = new AttendingMethod();
            UserMethod um = new UserMethod();
            UserDetail ud = new UserDetail();
            ad.Attending_Year = DateTime.Now.Year;
            ud.User_UserName = ad.Attending_User;
            ud.User_FirstName = ad.Attending_Firstname;
            ud.User_LastName = ad.Attending_Lastname;
            ud.User_Class = ad.Attending_Class;

            um.UpdateUserInfo(ud, out error);
            am.InsertAttending(ad, out error);

            return RedirectToAction("AttendingConfirmed",ad);
        }

        public IActionResult AttendingConfirmed(AttendingDetail ad)
        {
            return View(ad);
        }

        public IActionResult ViewAttending()
        {
            List<AttendingDetail> AttendingList = new List<AttendingDetail>();
            AttendingMethod am = new AttendingMethod();
            string error = "";
            AttendingList = am.GetAttendingList(out error);
            return View(AttendingList);
        }

    }
}
