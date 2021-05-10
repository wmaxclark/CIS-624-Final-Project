using DataObjects;
using LogicLayer;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebPresentation.Controllers
{
    
    public class CustomerController : Controller
    {
        IOperationManager _operationManager = new OperationManager();
        IUserManager _userManager = new UserManager();
        // GET: Customer
        public ActionResult Index(string state)
        {
            try
            {
                List<Operation> operations = _operationManager.GetAllOperations().ToList();
                var statelist = _operationManager.GetAllStates();
                if (statelist.Contains(state))
                {
                    ViewData["StateFilter"] = state;
                    operations = operations.Where(o => o.AddressState == state).ToList();
                }
                else
                {
                    state = null;
                }
                List<OperationViewModel> operationViewModels = new List<OperationViewModel>();

                foreach (var operation in operations)
                {
                    operationViewModels.Add(_operationManager.GetOperationViewModelByOperation(operation));
                }
                ViewBag.CartLines = TempData["cartLines"];
                ViewBag.CartTotal = TempData["totalPrice"];
                ViewBag.States = statelist;
                return View(operationViewModels.OrderBy(o => o.OperationName));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
        }

        [HttpGet]
        [Authorize(Roles ="Customer")]
        public ActionResult Order(string id, string operationId)
        {
            try
            {
                List<Product> products =_operationManager.GetProductsByOperation(Int32.Parse(operationId));
                Product product = products.Where(p => p.ProductID == Int32.Parse(id)).Single();

                CartViewModel cart = (CartViewModel)Session["cart"] ?? new CartViewModel();
                cart.Products.Add(product);
                Session["cart"] = cart;
                TempData["cartLines"] = cart.Products.Count;
                Decimal totalPrice = 0.0M;
                foreach (var line in cart.Products)
                {
                    totalPrice += product.UnitPrice;
                }
                TempData["totalPrice"] = totalPrice;
            }
            catch (Exception)
            {
                //
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public ActionResult Order(CartViewModel model)
        {
            BindingList<Order> orders = new BindingList<Order>();
            try
            {
                User user = _userManager.GetUserByEmail(User.Identity.Name);
                ViewBag.Success = _operationManager.CreateOrder(user, model.Products[0].OperationID, DateTime.Now, new BindingList<Product>(model.Products));
                orders = _operationManager.GetOrderListByUser(user);
                Session["cart"] = null;
            }
            catch (Exception)
            {
                // Give a message
            }
            return View("Orders", orders);
        }
        [HttpGet]
        [Authorize(Roles = "Customer")]
        public ActionResult Orders()
        {
            BindingList<Order> orders = new BindingList<Order>();
            try
            {
                User user = _userManager.GetUserByEmail(User.Identity.Name);
                orders = _operationManager.GetOrderListByUser(user);
            }
            catch (Exception)
            {
                // Give a message
            }
            return View(orders);
        }

        [Authorize(Roles = "Customer")]
        public ActionResult Cart()
        {
            CartViewModel cart = (CartViewModel)Session["cart"] ?? new CartViewModel();
            return View(cart);
        }

        [Authorize(Roles = "Customer")]
        public ActionResult Subscribe(string id)
        {
            try
            {
                BindingList<Operation> operations = _operationManager.GetAllOperations();
                OperationViewModel model = _operationManager.GetOperationViewModelByOperation(operations.Where(o => o.OperationID == int.Parse(id)).Single());
                return View(new SubscribeOperationViewModel(model));
            }
            catch (Exception ex)
            {
                // Give a message
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
        }
        [HttpPost]
        public ActionResult Subscribe(SubscribeOperationViewModel target)
        {
            try
            {
                User customer = _userManager.GetUserByEmail(User.Identity.Name);
                Operation operation = (Operation)target;
                if (ModelState.IsValid && target.Selection)
                {
                    ViewBag.Success = _operationManager.CreateWeeklyShare(customer, operation.OperationID, 1.0m, 1);
                }
            }
            catch (Exception)
            {
                ViewBag.Success = false;
                return View(target);
            }
            return RedirectToAction("Index", "Customer");
        }

        [Authorize(Roles = "Customer")]
        public ActionResult Subscriptions()
        {
            List<WeeklyShareViewModel> weeklyShareViewModels = new List<WeeklyShareViewModel>();
            try
            {
                User customer = _userManager.GetUserByEmail(User.Identity.Name);
                BindingList<Operation> operations = _operationManager.GetAllOperations();
                foreach (var operation in operations)
                {
                    if(_operationManager.GetWeeklyShareByUser(customer, operation.OperationID))
                    {
                        weeklyShareViewModels.Add(new WeeklyShareViewModel(operation, customer.UserID));
                    }
                }
            }
            catch (Exception)
            {
                // Write a message
            }
            return View(weeklyShareViewModels);
        }
    }
}