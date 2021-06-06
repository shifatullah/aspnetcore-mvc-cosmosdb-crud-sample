using CrudSample.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudSample.Services
{

    public interface ITeacherDbService
    {
        Task<IEnumerable<Teacher>> GetItemsAsync(string query);
        Task<Teacher> GetItemAsync(string id);
        Task AddItemAsync(Teacher item);
        Task UpdateItemAsync(string id, Teacher item);
        Task DeleteItemAsync(string id);
    }
}
