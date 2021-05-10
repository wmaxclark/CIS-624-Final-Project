using DataObjects;
using LogicLayer;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebPresentation.Controllers
{
    
    public class CustomerController : Controller
    {
        IOperationManager _operationManager = new OperationManager();
        IUserManager _userManager = new UserManager();
        // GET: Customer
        public ActionResult Index()
        {
            List<Operation> operations = _operationManager.GetAllOperations().ToList();
            List<OperationViewModel> operationViewModels = new List<OperationViewModel>();
            foreach (var operation in operations)
            {
                operationViewModels.Add(_operationManager.GetOperationViewModelByOperation(operation));
            }
            ViewBag.CartLines = TempData["cartLines"];
            ViewBag.CartTotal = TempData["totalPrice"];
            return View(operationViewModels.OrderBy(o => o.OperationName));
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
            catch (Exception ex)
            {

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

            }
            return View(orders);
        }

        [Authorize(Roles = "Customer")]
        public ActionResult Cart()
        {
            CartViewModel cart = (CartViewModel)Session["cart"] ?? new CartViewModel();
            return View(cart);
        }
    }
}