using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebPresentation.Controllers
{
    public class OperationController : Controller
    {
        // GET: Operation
        public ActionResult Index()
        {
            return View();
        }

        public ViewResult Dashboard(OperationVM operationVM)
        {
            ViewBag.Title = operationVM.OperationName;
            return View(operationVM);
        }
    }
}