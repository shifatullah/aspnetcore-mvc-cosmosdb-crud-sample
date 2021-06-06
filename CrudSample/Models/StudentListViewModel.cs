using System;
using System.Collections.Generic;
using CrudSample.Entities;

namespace CrudSample.Models
{
    public class StudentListViewModel
    {
        public StudentListViewModel()
        {
        }

        public List<Student> Students { get; set; }
    }
}
