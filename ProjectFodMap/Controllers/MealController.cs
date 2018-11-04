using ProjectFodMap.Data.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

/// <summary>
/// Controller functions call specific functions in ProjectFodMap.Data.MealsManager to separare the code.
/// </summary>

namespace ProjectFodMap.Controllers
{
    public class MealController : Controller
    {
        // Called when starting program and also used for searching for meals.
        public ActionResult Meals(string searchStr = null)
        {
            List<Data.ViewModels.MealListingVM> model;

            if (searchStr == "")
            {
                model = Data.MealsManager.GetAllMeals();
                return PartialView("_MealsTable", model);
            }

            // Do AjaxRequest to only update the _MealsTable Partial View based of search string instead of refreshing the whole page.
            if (searchStr != null)
            {
                if (Request.IsAjaxRequest())
                {
                    model = Data.MealsManager.GetMealsByName(searchStr);
                    return PartialView("_MealsTable", model);
                }
            }
            model = Data.MealsManager.GetAllMeals();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: Meal/Create
        [HttpPost]
        public ActionResult Create(Meal meal)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ProjectFodMap.Data.MealsManager.CreateNewMeal(meal);
                    return RedirectToAction("Meals");
                }
                return View();
            }
            catch
            {
                ModelState.AddModelError("", "Unable to Create Meal");
                return View();
            }
        }

        // GET: Meal/Details/
        public ActionResult Details(int id)
        {
            Meal model = Data.MealsManager.GetMealDetails(id);
            return View(model);
        }

        // GET: Meal/Edit/
        public ActionResult Edit(int id)
        {
            Meal model = Data.MealsManager.GetMealDetails(id);
            return View(model);
        }

        // POST: Meal/Edit/
        [HttpPost]
        public ActionResult Edit(int id, Meal meal)
        {
            try
            {
                Data.MealsManager mealsManager = new Data.MealsManager();
                mealsManager.EditMealDetails(id, meal);
                return RedirectToAction("Meals");
            }
            catch
            {
                return View();
            }
        }

        // GET: Meal/Delete/
        // Loads delete page with data about the meal to be deleted based of its id.
        public ActionResult Delete(int id)
        {
            Meal model = Data.MealsManager.GetMealDetails(id);
            return View(model);
        }

        // POST:  Meal/Delete
        [HttpPost] 
        public ActionResult Delete(int id, Data.ViewModels.MealDataVM meal)
        {
            // meal parameter is empty. Im not sure how to send the values from the view to the controller propperly
            // So i use the id value to know which meal to delete. But It has to be possible to send the data 
            // back to the controller through databinding.
            try
            {
                Data.MealsManager.DeleteMeal(id, meal);
                return RedirectToAction("Meals");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
