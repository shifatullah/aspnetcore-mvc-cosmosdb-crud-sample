using System;
using System.Collections.Generic;
using CrudSample.Entities;

namespace CrudSample.Models
{
    public class TeacherListViewModel
    {
        public TeacherListViewModel()
        {
        }

        public List<Teacher> Teachers { get; set; }
    }
}
