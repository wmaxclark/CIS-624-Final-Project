using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using DataObjects;
using LogicLayer;
using System.Net;

namespace WebPresentation.Controllers
{
    [Authorize(Roles = "Farmer")]
    public class ProductController : Controller
    {
        IOperationManager _operationManager = new OperationManager();
        IUserManager _oldUserManager = new UserManager();
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateProductViewModel model)
        {
            try
            {
                User farmer = _oldUserManager.GetUserByEmail(User.Identity.Name);
                Operation operation = _operationManager.GetOperationByOperator(farmer);
                model.OperationID = operation.OperationID;
                if (ModelState.IsValid)
                {
                    _operationManager.AddProduct(model.OperationID, model.ProductName, model.ProductDescription, model.Unit, model.InputCost, model.UnitPrice, model.GerminationDate, model.PlantDate, model.TransplantDate, model.HarvestDate);
                    ViewBag.Success = true;
                }
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(model);
        }

        [HttpGet]
        public ViewResult Edit(string id)
        {
            try
            {
                User farmer = _oldUserManager.GetUserByEmail(User.Identity.Name);
                Operation operation = _operationManager.GetOperationByOperator(farmer);
                Product product = _operationManager.GetProductsByOperation(operation.OperationID).Where(p => p.ProductID == Int32.Parse(id)).Single();
                return View(new EditProductViewModel(product));
            }
            catch (Exception)
            {
                return View("Dashboard", "Operation");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ViewResult Edit(EditProductViewModel product)
        {
            try
            {
                User farmer = _oldUserManager.GetUserByEmail(User.Identity.Name);
                Operation operation = _operationManager.GetOperationByOperator(farmer);
                Product oldProduct = _operationManager.GetProductsByOperation(operation.OperationID).Where(p => p.ProductID == product.ProductID).Single();
                if (ModelState.IsValid)
                {
                    _operationManager.UpdateProduct(operation.OperationID,
                    oldProduct,
                    product.ProductName,
                    product.ProductDescription,
                    product.Unit,
                    product.InputCost,
                    product.UnitPrice,
                    product.GerminationDate,
                    product.PlantDate,
                    product.TransplantDate,
                    product.HarvestDate);
                    ViewBag.Success = true;
                }
            }
            catch (Exception)
            {
                ViewBag.Success = false;
            }
            return View(product);
        }

        [HttpGet]
        public ViewResult Clone(string id)
        {
            try
            {
                User farmer = _oldUserManager.GetUserByEmail(User.Identity.Name);
                Operation operation = _operationManager.GetOperationByOperator(farmer);
                Product product = _operationManager.GetProductsByOperation(operation.OperationID).Where(p => p.ProductID == Int32.Parse(id)).Single();
                return View(new CloneProductViewModel(product));
            }
            catch (Exception)
            {
                return View("Dashboard", "Operation");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ViewResult Clone(CloneProductViewModel product)
        {
            try
            {
                User farmer = _oldUserManager.GetUserByEmail(User.Identity.Name);
                Operation operation = _operationManager.GetOperationByOperator(farmer);
                Product oldProduct = _operationManager.GetProductsByOperation(operation.OperationID).Where(p => p.ProductID == product.ProductID).Single();

                if (ModelState.IsValid)
                {
                    _operationManager.CloneProduct(oldProduct, operation.OperationID,
                    product.ProductName,
                    product.ProductDescription,
                    product.Unit,
                    product.InputCost,
                    product.UnitPrice,
                    product.GerminationDate);
                    ViewBag.Success = true;
                }
            }
            catch (Exception)
            {
                ViewBag.Success = false;
            }
            return View(product);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            try
            {
                User farmer = _oldUserManager.GetUserByEmail(User.Identity.Name);
                Operation operation = _operationManager.GetOperationByOperator(farmer);
                Product product = _operationManager.GetProductsByOperation(operation.OperationID).Where(p => p.ProductID == Int32.Parse(id)).Single();
                return View(new DeleteProductViewModel(product));
            }
            catch (Exception)
            {
                return RedirectToAction("Dashboard", "Operation");
            }
        }

        [HttpPost]
        public ActionResult Delete(DeleteProductViewModel target)
        {
            try
            {
                User farmer = _oldUserManager.GetUserByEmail(User.Identity.Name);
                Operation operation = _operationManager.GetOperationByOperator(farmer);
                Product product = _operationManager.GetProductsByOperation(operation.OperationID).Where(p => p.ProductID == target.ProductID).Single();
                target.ProductID = product.ProductID;
                target.ProductName = product.ProductName;
                if (ModelState.IsValid && target.DeleteSelection)
                {
                    _operationManager.DeleteProduct(product);
                }
            }
            catch (Exception)
            {
                ViewBag.Success = false;
                return View(target);
            }
            return RedirectToAction("Index", "Operation", null);
        }
    }
}
