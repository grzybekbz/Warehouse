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

        public ViewResult Index() => View();

        public IActionResult ShowActivities(string activityType) {

            if (activityType != null &&
               (activityType.Equals("Shipment") || 
                activityType.Equals("Admission")))

                return View(new ShowAcivitiesViewModel() {
                        ActivityType = activityType,
                        Activities = repository.Activities
                            .Where(p => p.Type.Equals(activityType))
                });
            else
                return RedirectToAction("Index");
        }

        public IActionResult AddActivity(string activityType) {

            if (activityType != null && 
               (activityType.Equals("Shipment") || 
                activityType.Equals("Admission")))

                return View("AddActivity", new AddActivityViewModel() {
                    ListOfProducts = new SelectList(
                        repository.Products, "ProductID", "Name"),
                    ActivityToAdd = new Activity() { Type = activityType },
                    ActivityProducts = new List<ProductValues>()
                });
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddActivity(AddActivityViewModel activity) {

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

                return View(activity);
            }
        }

        [HttpPost]
        public ViewResult AddProductToActivity(AddActivityViewModel activity) {

            activity.ListOfProducts = new SelectList(
                        repository.Products, "ProductID", "Name");

            if (activity.ActivityProducts == null)
                activity.ActivityProducts = new List<ProductValues>();

            ModelState.Remove("ActivityProducts");
            ModelState.Remove("ActivityToAdd.Where");
            ModelState.Remove("ActivityToAdd.Description");

            if (ModelState.IsValid) {

                Product prod = repository.Products
                            .Where(p => p.ProductID.Equals(activity.ProductID))
                            .FirstOrDefault();

                activity.ActivityProducts.Add(new ProductValues() {
                    ID = prod.ProductID,
                    Name = prod.Name,
                    Quantity = activity.ProductQuantity
                });

                return View("AddActivity", activity);

            } else {

                return View("AddActivity", activity);
            }
        }

        [HttpPost]
        public ViewResult DeleteProductFromActivity(AddActivityViewModel activity,
                                                    int activityProductId) {

            activity.ListOfProducts = new SelectList(
                        repository.Products, "ProductID", "Name");

            activity.ActivityProducts.RemoveAt(activityProductId);

            return View("AddActivity", activity);
        }
    }
}