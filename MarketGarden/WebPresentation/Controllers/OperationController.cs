using DataObjects;
using LogicLayer;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebPresentation.Controllers
{
    [Authorize(Roles ="Farmer")]
    public class OperationController : Controller
    {
        IOperationManager _operationManager = new OperationManager();
        IUserManager _oldUserManager = new UserManager();
        private ApplicationUserManager _userManager;
        public OperationController()
        {
        }

        public OperationController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Operation

        public ActionResult Index()
        {
            try
            {
                User farmer = _oldUserManager.GetUserByEmail(User.Identity.Name);
                OperationVM operationVM = _operationManager.GetOperationVMByOperator(farmer);
                return Dashboard(operationVM);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        
        public ActionResult Dashboard(OperationVM operationVM)
        {
            if (operationVM.OperationName == null)
            {
                return Index();
            }
            ViewBag.Title = operationVM.OperationName;
            return View("Dashboard", operationVM);
        }
    }
}