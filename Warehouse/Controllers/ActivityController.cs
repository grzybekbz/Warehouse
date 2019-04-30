using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Collections.Generic;
using Warehouse.Models;

namespace Warehouse.Controllers {

    public class ActivityController : Controller {

        private readonly IRepository repository;

        public ActivityController(IRepository repo) {

            repository = repo;
        }

        public ViewResult ShowActivities(string activityType) {

            return View(new ShowAcivitiesViewModel() {
                ActivityType = activityType,
                Activities = repository.Activities
                        .Where(p => p.Type.Equals(activityType))
            });
        }

        public ViewResult AddActivity(string activityType) {

            return View("EditActivity", new EditActivityViewModel() {
                ListOfProducts = new SelectList(
                    repository.Products, "ProductID", "Name"),
                ActivityToAdd = new Activity() { Type = activityType },
                ActivityProducts = new List<ProductValues>()
            });
        }

        [HttpPost]
        public IActionResult AddActivity(EditActivityViewModel activity) {

            activity.ActivityToAdd.Date = DateTime.Now;
            activity.ListOfProducts = new SelectList(
                repository.Products, "ProductID", "Name");

            ModelState.Remove("ProductQuantity");
            ModelState.Remove("ProductID");

            if (ModelState.IsValid) {

                repository.SaveActivity(activity.ActivityToAdd);
                repository.SaveProductsInActivity(activity.ActivityProducts,
                                                  activity.ActivityToAdd);

                if (activity.ActivityToAdd.Type.Equals("Admission")) {

                    TempData["message"] = $"Dodano przyjęcie na magazyn nr " +
                            $"{activity.ActivityToAdd.ActivityID}";

                } else {

                    TempData["message"] = $"Dodano wydanie z magazynu nr " +
                            $"{activity.ActivityToAdd.ActivityID}";
                }
                return RedirectToAction("ShowActivities",
                        new { activityType = activity.ActivityToAdd.Type });

            } else {

                return View("EditActivity", activity);
            }
        }

        [HttpPost]
        public ViewResult AddProductToActivity(EditActivityViewModel activity) {

            activity.ListOfProducts = new SelectList(
                        repository.Products, "ProductID", "Name");

            if (activity.ActivityProducts == null)
                activity.ActivityProducts = new List<ProductValues>();

            ModelState.Remove("ActivityProducts");
            ModelState.Remove("ActivityToAdd.Where");
            ModelState.Remove("ActivityToAdd.Description");

            if (ModelState.IsValid) {

                var prod = repository.Products
                            .Where(p => p.ProductID.Equals(activity.ProductID))
                            .FirstOrDefault();

                activity.ActivityProducts.Add(new ProductValues() {
                    ID = prod.ProductID,
                    Name = prod.Name,
                    Quantity = activity.ProductQuantity
                });
            }

            return View("EditActivity", activity);
        }

        [HttpPost]
        public ViewResult DeleteProductFromActivity(EditActivityViewModel activity,
                                                    int activityProductId) {

            activity.ListOfProducts = new SelectList(
                        repository.Products, "ProductID", "Name");

            ModelState.Remove("ActivityToAdd.Where");
            ModelState.Remove("ActivityToAdd.Description");
            ModelState.Remove("ProductQuantity");

            activity.ActivityProducts.RemoveAt(activityProductId);

            return View("EditActivity", activity);
        }

        public ViewResult EditActivity(int activityId) {

            var activityToAdd = repository.Activities
                                    .Where(a => a.ActivityID == activityId)
                                    .FirstOrDefault();

            var listOfProducts = new SelectList(
                                repository.Products, "ProductID", "Name");

            var query = from product in repository.Products
                        join productLine in repository.ProductLines
                        on product.ProductID equals productLine.ProductID
                        where productLine.ActivityID == activityId
                        select new { id = product.ProductID,
                            n = product.Name, q = productLine.Quantity };

            var activityProducts = query.Select(x => new ProductValues() {
                ID = x.id,
                Name = x.n,
                Quantity = x.q
            }).ToList();

            return View(new EditActivityViewModel() {
                ActivityToAdd = activityToAdd,
                ListOfProducts = listOfProducts,
                ActivityProducts = activityProducts
            });
        }

        public ViewResult DeleteActivity(int activityId,
                                         string activityType) {

            var activity = repository.Activities
                    .Where(a => a.ActivityID == activityId)
                    .FirstOrDefault();

            var products = repository.ProductLines
                    .Where(p => p.ActivityID == activityId)
                    .ToList();

            repository.DeleteActivityAndProducts(products, activity);

            return View("ShowActivities", new ShowAcivitiesViewModel() {
                ActivityType = activityType,
                Activities = repository.Activities
            .Where(p => p.Type.Equals(activityType))
            });
        }

        public ViewResult ActivityDetails(int activityId) {

            var activity = repository.Activities
                .Where(a => a.ActivityID == activityId)
                .FirstOrDefault();

            var products = repository.ProductLines
                .Where(p => p.ActivityID == activityId)
                .OrderBy(p => p.ProductLineID);

            var query = from prod in repository.Products
                        join productLine in products
                        on prod.ProductID equals productLine.ProductID
                        select new { productName = prod.Name,
                            productQuantity = productLine.Quantity };

            return View(new ActivityDetailsViewModel() {
                Activity = activity,
                Products = query.Select(x => new ProductAndQuantity {
                    Product = x.productName,
                    Quantity = x.productQuantity
                }).ToList()
            });
        }


    }
}