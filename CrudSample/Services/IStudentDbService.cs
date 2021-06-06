using CrudSample.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudSample.Services
{

    public interface IStudentDbService
    {
        Task<IEnumerable<Student>> GetItemsAsync(string query);
        Task<Student> GetItemAsync(string id);
        Task AddItemAsync(Student item);
        Task UpdateItemAsync(string id, Student item);
        Task DeleteItemAsync(string id);
    }
}
