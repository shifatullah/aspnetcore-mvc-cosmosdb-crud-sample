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
    public class StudentController : Controller
    {
        IStudentDbService _studentDbService;
        ICourseDbService _courseDbService;

        public StudentController(
            IStudentDbService studentDbService,
            ICourseDbService courseDbService)
        {
            _studentDbService = studentDbService;
            _courseDbService = courseDbService;
        }

        public async Task<ViewResult> Index()
        {
            StudentListViewModel model = new StudentListViewModel();
            model.Students = (await _studentDbService.GetItemsAsync("SELECT * FROM c")).ToList();
            return View(model);
        }
        public async Task<ViewResult> New()
        {
            StudentViewModel studentViewModel = new StudentViewModel();
            studentViewModel.Courses = (await _courseDbService.GetItemsAsync("SELECT * FROM c")).ToList();
            return View("Edit", studentViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                Student student = null;
                if (!string.IsNullOrWhiteSpace(model.Id))
                    student = (await _studentDbService.GetItemAsync(model.Id));

                if (student == null)
                {
                    student = new Student();
                    student.Id = Guid.NewGuid().ToString();
                    student.Name = model.Name;

                    if (model.CourseIds != null && model.CourseIds.Count > 0)
                    {
                        model.CourseIds.Remove("");
                        student.Courses = new List<Course>();
                        foreach (string cid in model.CourseIds)
                        {
                            Course sc = (await _courseDbService.GetItemAsync(cid));                            
                            student.Courses.Add(sc);
                        }
                    }

                    await _studentDbService.AddItemAsync(student);
                }
                else
                {
                    student.Name = model.Name;

                    if (model.CourseIds != null && model.CourseIds.Count > 0)
                    {
                        model.CourseIds.Remove("");
                        student.Courses = new List<Course>();
                        foreach (string cid in model.CourseIds)
                        {
                            Course sc = (await _courseDbService.GetItemAsync(cid));
                            student.Courses.Add(sc);
                        }
                    }

                    await _studentDbService.UpdateItemAsync(student.Id, student);
                }                

                return RedirectToAction("Index");
            }
            else
            {
                model.Courses = (await _courseDbService.GetItemsAsync("SELECT * FROM c")).ToList();
                return View(model);
            }
        }

        public async Task<ViewResult> Edit(string id)
        {
            Student student = (await _studentDbService.GetItemAsync(id));

            StudentViewModel studentViewModel = new StudentViewModel();
            studentViewModel.Id = student.Id;
            studentViewModel.Name = student.Name;
            if (student.Courses != null)
                studentViewModel.CourseIds = student.Courses.Select(sc => sc.Id).ToList();
            studentViewModel.Courses = (await _courseDbService.GetItemsAsync("SELECT * FROM c")).ToList();

            return View(studentViewModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            Student student = (await _studentDbService.GetItemAsync(id));

            if (student != null)
            {
                await _studentDbService.DeleteItemAsync(id);
            }

            return RedirectToAction("Index");
        }
    }
}