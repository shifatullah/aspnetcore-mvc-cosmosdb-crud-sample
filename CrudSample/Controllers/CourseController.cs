using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudSample.Entities;
using CrudSample.Models;
using CrudSample.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;

namespace CrudSample.Controllers
{
    public class CourseController : Controller
    {
        ICourseDbService _courseDbService;

        public CourseController(
            ICourseDbService courseDbService)
        {
            _courseDbService = courseDbService;
        }

        public async Task<ViewResult> Index()
        {
            CourseListViewModel model = new CourseListViewModel();
            model.Courses = (await _courseDbService.GetItemsAsync("SELECT * FROM c")).ToList();
            return View(model);
        }
        public ViewResult New()
        {
            CourseViewModel courseViewModel = new CourseViewModel();            
            return View("Edit", courseViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                Course course = null;
                if (!string.IsNullOrWhiteSpace(model.Id))
                    course = (await _courseDbService.GetItemAsync(model.Id));

                if (course == null)
                {
                    course = new Course();
                    course.Id = Guid.NewGuid().ToString();
                    course.Name = model.Name;                    
                    await _courseDbService.AddItemAsync(course);
                }
                else
                {
                    course.Name = model.Name;
                    await _courseDbService.UpdateItemAsync(course.Id, course);
                }

                return RedirectToAction("Index");
            }
            else
            {                
                return View(model);
            }
        }

        public async Task<ViewResult> Edit(string id)
        {
            Course course = (await _courseDbService.GetItemAsync(id));

            CourseViewModel courseViewModel = new CourseViewModel();
            courseViewModel.Id = course.Id;
            courseViewModel.Name = course.Name;
            return View(courseViewModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            Course course = (await _courseDbService.GetItemAsync(id));

            if (course != null)
            {
                await _courseDbService.DeleteItemAsync(id);
            }

            return RedirectToAction("Index");
        }
    }
}