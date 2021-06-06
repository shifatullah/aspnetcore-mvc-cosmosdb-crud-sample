using System;
using System.Linq;
using System.Threading.Tasks;
using CrudSample.Entities;
using CrudSample.Models;
using CrudSample.Services;
using Microsoft.AspNetCore.Mvc;

namespace CrudSample.Controllers
{
    public class TeacherController : Controller
    {
        ITeacherDbService _teacherDbService;
        ICourseDbService _courseDbService;

        public TeacherController(
            ITeacherDbService teacherDbService,
            ICourseDbService courseDbService)
        {
            _teacherDbService = teacherDbService;
            _courseDbService = courseDbService;
        }


        public async Task<ViewResult> Index()
        {
            TeacherListViewModel model = new TeacherListViewModel();
            model.Teachers = (await _teacherDbService.GetItemsAsync("SELECT * FROM c")).ToList();
            return View(model);
        }

        public async Task<ViewResult> New()
        {
            TeacherViewModel teacherViewModel = new TeacherViewModel();
            teacherViewModel.Courses = (await _courseDbService.GetItemsAsync("SELECT * FROM c")).ToList();
            return View("Edit", teacherViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TeacherViewModel model)
        {
            if (ModelState.IsValid)
            {
                Teacher teacher = null;
                if (!string.IsNullOrWhiteSpace(model.Id))
                    teacher = (await _teacherDbService.GetItemAsync(model.Id));

                if (teacher == null)
                {
                    teacher = new Teacher();
                    teacher.Id = Guid.NewGuid().ToString();
                    teacher.Name = model.Name;
                    teacher.Course = (await _courseDbService.GetItemAsync(model.Course));
                    await _teacherDbService.AddItemAsync(teacher);
                }
                else
                {
                    teacher.Name = model.Name;
                    teacher.Course = (await _courseDbService.GetItemAsync(model.Course));
                    await _teacherDbService.UpdateItemAsync(teacher.Id, teacher);
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
            Teacher teacher = (await _teacherDbService.GetItemAsync(id));

            TeacherViewModel teacherViewModel = new TeacherViewModel();
            teacherViewModel.Courses = (await _courseDbService.GetItemsAsync("SELECT * FROM c")).ToList();
            teacherViewModel.Id = teacher.Id;
            teacherViewModel.Name = teacher.Name;
            if (teacher.Course != null)
                teacherViewModel.Course = teacher.Course.Id;

            return View(teacherViewModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            Teacher teacher = (await _teacherDbService.GetItemAsync(id));

            if (teacher != null)
            {
                await _teacherDbService.DeleteItemAsync(id);
            }

            return RedirectToAction("Index");
        }
    }
}