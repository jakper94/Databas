using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class IdagInattController : Controller
    {
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

        public IActionResult NomineeList() { 
            List<NomineeDetail> NomineeList = new List<NomineeDetail>();
            NomineeMethod nm = new NomineeMethod();
            string error = "";
            NomineeList = nm.GetNomineeList(out error);
            ViewBag.error = error;
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
