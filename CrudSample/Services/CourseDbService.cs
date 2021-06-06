namespace CrudSample.Services
{
    using CrudSample.Entities;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CourseDbService : ICourseDbService
    {
        private Container _container;
        private IConfigurationSection _configurationSection;
        private const string containerName = "Course";

        public CourseDbService(
            CosmosClient client,
            IConfigurationSection configurationSection)
        {
            _configurationSection = configurationSection;

            string databaseName = _configurationSection.GetSection("DatabaseName").Value;            

            this._container = client.GetContainer(databaseName, containerName);

            DatabaseResponse database = client.CreateDatabaseIfNotExistsAsync(databaseName).Result;
            database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
        }

        public async Task AddItemAsync(Course course)
        {
            await this._container.CreateItemAsync<Course>(course, new PartitionKey(course.Id));
        }

        public async Task DeleteItemAsync(string id)
        {
            await this._container.DeleteItemAsync<Course>(id, new PartitionKey(id));
        }

        public async Task<Course> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<Course> response = await this._container.ReadItemAsync<Course>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IEnumerable<Course>> GetItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<Course>(new QueryDefinition(queryString));
            List<Course> results = new List<Course>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateItemAsync(string id, Course course)
        {
            await this._container.UpsertItemAsync<Course>(course, new PartitionKey(id));
        }
    }
}