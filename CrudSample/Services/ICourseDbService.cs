using CrudSample.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudSample.Services
{

    public interface ICourseDbService
    {
        Task<IEnumerable<Course>> GetItemsAsync(string query);
        Task<Course> GetItemAsync(string id);
        Task AddItemAsync(Course item);
        Task UpdateItemAsync(string id, Course item);
        Task DeleteItemAsync(string id);
    }
}
