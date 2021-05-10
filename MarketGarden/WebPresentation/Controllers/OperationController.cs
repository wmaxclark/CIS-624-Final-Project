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
                OperationViewModel operationVM = _operationManager.GetOperationVMByOperator(farmer);
                return Dashboard(operationVM);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        
        public ActionResult Dashboard(OperationViewModel operationVM)
        {
            if (operationVM.OperationName == null)
            {
                return Index();
            }
            ViewBag.Title = operationVM.OperationName;
            try
            {
                operationVM.WeeklyShares = _operationManager.RefreshWeeklyShares(operationVM);
                var totalPortion = 0;
                var weeklyShareProfit = 0.0m;
                foreach (WeeklyShare share in operationVM.WeeklyShares)
                {
                    totalPortion++;
                    foreach (var product in operationVM.Products)
                    {
                        weeklyShareProfit += product.UnitPrice - product.InputCost;
                    }
                }
                ViewBag.WeeklyShareSubscribers = totalPortion;
                ViewBag.WeeklyShareProfit = weeklyShareProfit;

                operationVM.Orders = _operationManager.RefreshOrderList(operationVM);
                var totalOrders = 0;
                var directSaleProfit = 0.0m;
                foreach (Order order in operationVM.Orders)
                {
                    totalOrders++;
                    foreach (var line in order.Lines)
                    {
                        foreach (var product in operationVM.Products)
                        {
                            if (line.ProductID == product.ProductID)
                            {
                                directSaleProfit += line.PriceCharged - product.InputCost;
                            }
                        }

                    }
                }
                ViewBag.DirecSaleOrders = totalOrders;
                ViewBag.DirectSaleProfit = directSaleProfit;
            }
            catch (Exception)
            {
            }
            
            return View("Dashboard", operationVM);
        }

        public ActionResult Dashboard()
        {
            return Index();
        }
    }
}