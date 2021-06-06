using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrudSample.Entities
{
    public class Course
    {
        public Course()
        {
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
