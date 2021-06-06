using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrudSample.Entities
{
    public class Student
    {
        public Student()
        {
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}