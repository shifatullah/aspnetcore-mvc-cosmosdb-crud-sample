using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CrudSample.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrudSample.Models
{
    public class TeacherViewModel
    {
        public TeacherViewModel()
        {
        }

        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Course { get; set; }

        public Course CourseDetail { get; set; }

        public List<Course> Courses { get; set; }

        public List<SelectListItem> CoursesListItem
        {
            get
            {
                return Courses == null ? null : Courses.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();
            }
        }
    }
}
