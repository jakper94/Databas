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

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot/images",
                        file.GetFilename());

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return RedirectToAction("Files");
        }

        [HttpPost]
       
        [HttpPost]
        public async Task<IActionResult> UploadFileViaModel(FileInputModel model)
        {
            if (model == null ||
                model.FileToUpload == null || model.FileToUpload.Length == 0)
                return Content("file not selected");

            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot/images",
                        model.FileToUpload.GetFilename());

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await model.FileToUpload.CopyToAsync(stream);
            }

            return RedirectToAction("Files");
        }

        public IActionResult Files()
        {
            var model = new FilesViewModel();
            foreach (var item in this.fileProvider.GetDirectoryContents(""))
            {
                model.Files.Add(
                    new FileDetails { Name = item.Name, Path = item.PhysicalPath });
            }
            return View(model);
        }

        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
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
        [HttpGet]
        public IActionResult Attend()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Attend(AttendingDetail ad)
        {
            return View();
        }

    }
}
