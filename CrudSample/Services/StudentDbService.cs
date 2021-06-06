namespace CrudSample.Services
{
    using CrudSample.Entities;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class StudentDbService : IStudentDbService
    {
        private Container _container;
        private IConfigurationSection _configurationSection;
        private const string containerName = "Student";

        public StudentDbService(
            CosmosClient client,
            IConfigurationSection configurationSection)
        {
            _configurationSection = configurationSection;

            string databaseName = _configurationSection.GetSection("DatabaseName").Value;            

            this._container = client.GetContainer(databaseName, containerName);

            DatabaseResponse database = client.CreateDatabaseIfNotExistsAsync(databaseName).Result;
            database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
        }

        public async Task AddItemAsync(Student student)
        {
            await this._container.CreateItemAsync<Student>(student, new PartitionKey(student.Id));
        }

        public async Task DeleteItemAsync(string id)
        {
            await this._container.DeleteItemAsync<Student>(id, new PartitionKey(id));
        }

        public async Task<Student> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<Student> response = await this._container.ReadItemAsync<Student>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IEnumerable<Student>> GetItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<Student>(new QueryDefinition(queryString));
            List<Student> results = new List<Student>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateItemAsync(string id, Student student)
        {
            await this._container.UpsertItemAsync<Student>(student, new PartitionKey(id));
        }
    }
}