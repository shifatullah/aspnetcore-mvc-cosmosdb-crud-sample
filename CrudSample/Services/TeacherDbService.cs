namespace CrudSample.Services
{
    using CrudSample.Entities;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TeacherDbService : ITeacherDbService
    {
        private Container _container;
        private IConfigurationSection _configurationSection;
        private const string containerName = "Teacher";

        public TeacherDbService(
            CosmosClient client,
            IConfigurationSection configurationSection)
        {
            _configurationSection = configurationSection;

            string databaseName = _configurationSection.GetSection("DatabaseName").Value;            

            this._container = client.GetContainer(databaseName, containerName);

            DatabaseResponse database = client.CreateDatabaseIfNotExistsAsync(databaseName).Result;
            database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
        }

        public async Task AddItemAsync(Teacher teacher)
        {
            await this._container.CreateItemAsync<Teacher>(teacher, new PartitionKey(teacher.Id));
        }

        public async Task DeleteItemAsync(string id)
        {
            await this._container.DeleteItemAsync<Teacher>(id, new PartitionKey(id));
        }

        public async Task<Teacher> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<Teacher> response = await this._container.ReadItemAsync<Teacher>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IEnumerable<Teacher>> GetItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<Teacher>(new QueryDefinition(queryString));
            List<Teacher> results = new List<Teacher>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateItemAsync(string id, Teacher teacher)
        {
            await this._container.UpsertItemAsync<Teacher>(teacher, new PartitionKey(id));
        }
    }
}