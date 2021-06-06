using System;
using System.ComponentModel.DataAnnotations;

namespace CrudSample.Models
{
    public class CourseViewModel
    {
        public CourseViewModel()
        {
        }

        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
