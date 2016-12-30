using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class BusinessToolsController : Controller
    {
        // GET: BusinessTools
        public ActionResult Index()
        {
            return View("Jira");
        }


        public ActionResult CRM()
        {
            return View("CRM");
        }
    }
}