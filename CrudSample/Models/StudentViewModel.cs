using System;
using System.Collections.Generic;
using System.Linq;
using CrudSample.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrudSample.Models
{
    public class StudentViewModel
    {
        public StudentViewModel()
        {
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public List<string> CourseIds { get; set; }

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
