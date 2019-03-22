using System;
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
using Microsoft.Extensions.FileProviders;
using Fiver.Mvc.FileUpload.Models.IdagInatt;


namespace WebApplication1.Controllers
{
    public class IdagInattController : Controller
    {
        private readonly IFileProvider fileProvider;
        int tempID;
        static bool votingOpen = true;
        public IdagInattController(IFileProvider fileProvider)
        {
            this.fileProvider = fileProvider;
        }
        public IActionResult Index()
        {

            return View();
        }
        [HttpGet]
        public IActionResult InsertNominee()
        {
            if (HttpContext.Session.GetString("AdminID") != null)
            {
                return View();
            }
                 else return RedirectToAction("AdminLogin");
        }
    
        [HttpPost]
        public async Task<IActionResult> InsertNominee(NomineeDetail nd, IFormFile file)
        {
            if (HttpContext.Session.GetString("AdminID") != null)
            {
                if (file == null || file.Length == 0)
                return Content("file not selected");

            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot/images",
                        file.GetFilename());
            string s = "/images/" + file.GetFilename();
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            NomineeMethod nm = new NomineeMethod();
                int i = 0;
                string error = "";
                i = nm.InsertNominee(nd,s, out error);
                ViewBag.error = error;
                
                return RedirectToAction("NomineeList");
            }
            else return RedirectToAction("AdminLogin");

        }

        
        [HttpGet]
        public IActionResult EditNominee(int id)
        {
            if (HttpContext.Session.GetString("AdminID") != null)
            {
                NomineeMethod nm = new NomineeMethod();
            NomineeDetail nd = new NomineeDetail();
            nd = nm.GetNomineeById(id, out string errormsg);
            return View(nd);
        }
            else return RedirectToAction("AdminLogin");
    }
        [HttpPost]
        public IActionResult EditNominee(NomineeDetail nd)
        {
            if (HttpContext.Session.GetString("AdminID") != null)
            {
                NomineeMethod nm = new NomineeMethod();
            string error = "";
            int i = 0;
            i = nm.UpdateNominee(nd, out error);
            return RedirectToAction("NomineeList");
            }
            else return RedirectToAction("AdminLogin");
        }
        [HttpGet]
        public IActionResult NomineeList() {
            if (HttpContext.Session.GetString("AdminID") != null)
            {
            List<NomineeDetail> NomineeList = new List<NomineeDetail>();
            NomineeMethod nm = new NomineeMethod();
            string error = "";
            NomineeList = nm.GetNomineeList(out error);
            ViewBag.error = error;
            return View(NomineeList);
            }
            else return RedirectToAction("AdminLogin");
        }
        [HttpPost]
        public IActionResult NomineeList(string year)
        {
            if (HttpContext.Session.GetString("AdminID") != null)
            {
                List<NomineeDetail> NomineeList = new List<NomineeDetail>();
                NomineeMethod nm = new NomineeMethod();
                string error = "";
                NomineeList = nm.GetNomineeListByYear(Convert.ToInt16(year), out error);
                ViewBag.error = error;
                return View(NomineeList);
            }
            else return RedirectToAction("AdminLogin");
        }

        [HttpGet]
        public IActionResult DeleteNominee(int id)
        {
            if (HttpContext.Session.GetString("AdminID") != null)
            {
                NomineeMethod nm = new NomineeMethod();
                NomineeDetail nd = new NomineeDetail();
                nd = nm.GetNomineeById(id, out string errormsg);
                return View(nd);
            }
            else return RedirectToAction("AdminLogin");
        }

        [HttpPost, ActionName("DeleteNominee")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("AdminID") != null)
            {
                NomineeMethod nm = new NomineeMethod();
                string error = "";
                int i = 0;
                i = nm.DeleteNominee(id, out error);

                ViewData["error"] = error; 

            return RedirectToAction("NomineeList");
            }
            else return RedirectToAction("AdminLogin");
        }
        public ActionResult NomineeDetails(int id)
        {
            if (HttpContext.Session.GetString("AdminID") != null)
            {
                NomineeMethod nm = new NomineeMethod();
            NomineeDetail nd = new NomineeDetail();
            nd = nm.GetNomineeById(id, out string errormsg);
            return View(nd);
            }
            else return RedirectToAction("AdminLogin");
        }

        public IActionResult NomineesToVoteOn()
        {
            if(votingOpen == false)
            {
                return View("VotingClosed");
            }
            ViewBag.Name = HttpContext.Session.GetString("UserID");
            ViewBag.Admin = HttpContext.Session.GetString("AdminID");
            if (ViewBag.Name != null || ViewBag.Admin != null)
            {
               
                int year = DateTime.Now.Year;

                List<NomineeDetail> NomineeList = new List<NomineeDetail>();
                NomineeMethod nm = new NomineeMethod();
                UserMethod um = new UserMethod();
                
                string error = "";
                NomineeList = nm.GetNomineeListByYear(year, out error);
                ViewBag.error = error;
                if (um.GetIfUserHasVooted(HttpContext.Session.GetString("UserID"), out string msg1) || um.GetIfUserHasVooted((ViewBag.Admin), out string msg2))
                {
                    return RedirectToAction("AllNominees");
                }
                else return View(NomineeList);

            }
            else
            {
                HttpContext.Session.SetString("fromWhere", "FromVoteOn");
                return View("Login");
            }
        }
        public IActionResult AllNominees()
        {

            ViewBag.Name = HttpContext.Session.GetString("UserID");
            ViewBag.Admin = HttpContext.Session.GetString("AdminID");
            if (ViewBag.Name != null || ViewBag.Admin != null)
            {

                int year = DateTime.Now.Year;

                List<NomineeDetail> NomineeList = new List<NomineeDetail>();
                NomineeMethod nm = new NomineeMethod();
               
                string error = "";
                NomineeList = nm.GetNomineeListByYear(year, out error);
                ViewBag.error = error;
                return View(NomineeList);
            }
            else
            {
                HttpContext.Session.SetString("fromWhere", "FromVoteOn");
                return View("Login");
            }
        }

        [HttpGet]
        public IActionResult VoteSite(int id)
        {

            NomineeMethod nm = new NomineeMethod();
            NomineeDetail nd = nm.GetNomineeById(id, out string errormsg);
            ViewBag.Name = nd.Nominee_FirstName + nd.Nominee_LastName;
            tempID = nd.Nominee_Id;
            ViewBag.image = nd.Nominee_ImgLink;
            ViewBag.voteId = tempID;
            HttpContext.Session.SetInt32("extraInfo", tempID);

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
            UserMethod um = new UserMethod();
            um.SetHasVotedToTrue(HttpContext.Session.GetString("AdminID"), out string errormsg1);
            um.SetHasVotedToTrue(HttpContext.Session.GetString("UserID"),out string errormsg);
            return RedirectToAction("NomineesToVoteOn");
        }


        [HttpGet]
        public IActionResult NomineeScore()
        {
            if (HttpContext.Session.GetString("AdminID") != null)
            {
                List<NomineeDetail> NomineeList = new List<NomineeDetail>();
                NomineeMethod nm = new NomineeMethod();
                ViewBag.voting = votingOpen;
                NomineeList = nm.GetNomineeListWithVotes(out string msg1);

                return View(NomineeList);
            }
            else return RedirectToAction("AdminLogin"); 
        }
        
        [HttpPost]
        public IActionResult NomineeScore(string year)
        {
            if (HttpContext.Session.GetString("AdminID") != null)
            {
                List<NomineeDetail> NomineeList = new List<NomineeDetail>();
                NomineeMethod nm = new NomineeMethod();
                ViewBag.voting = votingOpen;
                NomineeList = nm.GetNomineeListWithVotesByYear(year,out string error);
                return View(NomineeList);
            }
            else return RedirectToAction("AdminLogin");
        }

        public IActionResult Motivations(int id)
        {
            if (HttpContext.Session.GetString("AdminID") != null)
            {
                NomineeMethod nm = new NomineeMethod();
            NomineeDetail nd = nm.GetNomineeById(id, out string errormsg);
            List<VoteDetail> voteList = new List<VoteDetail>();
            VoteMethod vm = new VoteMethod();
            voteList = vm.GetMotivationListById(id, out string msg);
            ViewBag.Name = nd.Nominee_FirstName + " " + nd.Nominee_LastName;
            return View(voteList);
            }
            else return RedirectToAction("AdminLogin");
        }

        public IActionResult Admin()
        {
            if (HttpContext.Session.GetString("AdminID") != null)
            {
                return View();
            }
            else return RedirectToAction("AdminLogin");
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
                HttpContext.Session.SetString("UserID", ud.User_UserName);
                if (HttpContext.Session.GetString("fromWhere") == "FromVoteOn")
                {
                    return RedirectToAction("NomineesToVoteOn");
                }
                else if (HttpContext.Session.GetString("fromWhere") == "attend")
                {
                    return RedirectToAction("Attend");  
                }
                else return View("Index");

            }
            ud.LogInErrorMessage = error;
           
            return View("Login", ud);
        }

        public IActionResult LogOut()
        {


            HttpContext.Session.Clear();
            return View();
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
                HttpContext.Session.SetString("AdminID", ud.User_UserName);
                return View("Admin");

            }
            ud.LogInErrorMessage = error;
            return View("AdminLogin", ud);
        }

        [HttpGet]
        public IActionResult Attend()
        {

            
            ViewBag.Name = HttpContext.Session.GetString("UserID");
            ViewBag.Admin = HttpContext.Session.GetString("AdminID");
            if (ViewBag.Name != null || ViewBag.Admin != null)
            {
                string username;
                if (ViewBag.Admin != null)
                {
                    username = HttpContext.Session.GetString("AdminID");

                }
                else
                {
                    username = HttpContext.Session.GetString("UserID");
                }
                
                string error = "";
                UserMethod um = new UserMethod();
                UserDetail ud = um.GetUserByUserName(username, out error);
                AttendingDetail ad = new AttendingDetail();
                ad.Attending_User = ud.User_UserName;
                ad.Attending_Firstname = ud.User_FirstName;
                ad.Attending_Lastname = ud.User_LastName;
                ad.Attending_Class = ud.User_Class;
                return View(ad);
            }

            else
            {
                HttpContext.Session.SetString("fromWhere", "attend");
                return RedirectToAction("login");
            }
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
        public IActionResult AdminWarning()
        {
            return View();
        }
        public IActionResult ViewAttending()
        {

            ViewBag.Admin = HttpContext.Session.GetString("AdminID");

            if (ViewBag.Admin != null)
            {
                List<AttendingDetail> AttendingList = new List<AttendingDetail>();
                AttendingMethod am = new AttendingMethod();
                string error = "";
                AttendingList = am.GetAttendingList(out error);
                return View(AttendingList);
            }
            else
            {
                return View("AdminLogin");
            }
        }

        
        public IActionResult AllUsers()
        {
            if (HttpContext.Session.GetString("AdminID") != null)
            {
                List<UserDetail> userList = new List<UserDetail>();
                UserMethod um = new UserMethod();

                string error = "";

                userList = um.SelectUsers(out error);

                ViewBag.error = error; 

                return View(userList);
            }
            else return RedirectToAction("AdminLogin");
    }

        [HttpGet]
        public IActionResult AddUser()
        {
            if (HttpContext.Session.GetString("AdminID") != null)
            {
                return View();
            }
            else return RedirectToAction("AdminLogin");
        }

        [HttpPost]
        public IActionResult AddUser(UserDetail ud)
        {
            if (HttpContext.Session.GetString("AdminID") != null)
            {
                string error = "";
            UserMethod um = new UserMethod();
            um.InsertUser(ud, out error);
            return RedirectToAction("AllUsers");
            }
            else return RedirectToAction("AdminLogin");
        }

        [HttpGet]
        public IActionResult MakeAdmin(string username)
        {
            if (HttpContext.Session.GetString("AdminID") != null)
            {
                UserDetail ud = new UserDetail();
            ud.User_UserName = username;
                if (ud.User_UserName == HttpContext.Session.GetString("AdminID"))
                {
                    return RedirectToAction("AdminWarning");
                }
                else
                {
                    return View(ud);
                }
            }
            else return RedirectToAction("AdminLogin");
        }

        [HttpPost]
        public IActionResult MakeAdmin(UserDetail ud)
        {
            if (HttpContext.Session.GetString("AdminID") != null)
            {
                UserMethod um = new UserMethod();
            um.MakeUserAdmin(ud.User_UserName, ud.User_Password, out string errormsg);

            return RedirectToAction("AllUsers");
            }
            else return RedirectToAction("AdminLogin");
        }

        public IActionResult RemoveAdmin(string username)
        {
            if (HttpContext.Session.GetString("AdminID") != null)
            {
                
                if (username == HttpContext.Session.GetString("AdminID"))
                {
                    return RedirectToAction("AdminWarning");
                }
                else
                {
                    UserMethod um = new UserMethod();
                    um.DeleteAdmin(username, out string errormsg);

                    ViewData["error"] = errormsg;
                    return RedirectToAction("AllUsers");
                }

                
            }
            else return RedirectToAction("AdminLogin");
        }

        [HttpGet]
        public IActionResult EditUser(string id)
        {
            if (HttpContext.Session.GetString("AdminID") != null)
            {
                UserMethod um = new UserMethod();
                UserDetail ud = new UserDetail();

                ud = um.GetUserByUserName(id, out string errormsg);
                ViewData["error"] = errormsg; 
        
                return View(ud);
            }
            else return RedirectToAction("AdminLogin");
        }

        [HttpPost]
        public IActionResult EditUser(UserDetail ud, string id)
        {

            if (HttpContext.Session.GetString("AdminID") != null)
            {
                UserMethod um = new UserMethod();

                um.EditUserInfo(ud, id, out string errormsg);

                ViewData["error"] = errormsg;

                return RedirectToAction("AllUsers");
            }
            else return RedirectToAction("AdminLogin");
        }

        public IActionResult CloseVote()
        {
            if (HttpContext.Session.GetString("AdminID") != null)
            {
                return View();
            }
            else return RedirectToAction("AdminLogin");
        }

        public IActionResult CloseVoteConfirmed()
        {
            if (HttpContext.Session.GetString("AdminID") != null)
            {
                votingOpen = false;
                ViewBag.voting = votingOpen;
                string error = "";
                UserMethod um = new UserMethod();
                um.resetVotes(out error);
            
                return RedirectToAction("NomineeScore");
            }
            else return RedirectToAction("AdminLogin");
        }

        public IActionResult VotingClosed()
        {
            if (HttpContext.Session.GetString("AdminID") != null)
            {
                return View();
            }
            else return RedirectToAction("AdminLogin");
        }

        public IActionResult OpenVote()
        {
            if (HttpContext.Session.GetString("AdminID") != null)
            {
                votingOpen = true;
            ViewBag.voting = votingOpen;
            return RedirectToAction("NomineeScore");
            }
            else return RedirectToAction("AdminLogin");
        }

        [HttpGet]
        public IActionResult RemoveUser(string id)
        {
            
            if (HttpContext.Session.GetString("AdminID") != null)
            {
                UserMethod um = new UserMethod();
                UserDetail ud = new UserDetail();

                ud = um.GetUserByUserName(id, out string errormsg);
                if (ud.User_UserName == HttpContext.Session.GetString("AdminID"))
                {
                    return RedirectToAction("AdminWarning");
                }
                else
                { 
                
                ViewData["error"] = errormsg;

                return View(ud);
            }
            }
            else return RedirectToAction("AdminLogin");
        }

        [HttpPost, ActionName("RemoveUser")]
        public IActionResult ConfirmDelete(string id)
        {
            if (HttpContext.Session.GetString("AdminID") != null)
            {
                ViewData["username"] = id;

                UserMethod um = new UserMethod();
                um.DeleteUser(id, out string errormsg);

                ViewData["error"] = errormsg;

                return RedirectToAction("AllUsers");
            }
            else return RedirectToAction("AdminLogin");
        }
    }
}
