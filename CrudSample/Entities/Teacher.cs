using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace CrudSample.Entities
{
    public class Teacher
    {
        public Teacher()
        {
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public Course Course { get; set; }
    }
}
