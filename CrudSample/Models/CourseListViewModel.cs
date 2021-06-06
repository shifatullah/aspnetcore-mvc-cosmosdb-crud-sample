using System;
using System.Collections.Generic;
using CrudSample.Entities;

namespace CrudSample.Models
{
    public class CourseListViewModel
    {
        public CourseListViewModel()
        {
        }

        public List<Course> Courses { get; set; }
    }
}
