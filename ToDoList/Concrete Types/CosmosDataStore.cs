namespace ToDoList.Concrete_Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using Microsoft.Azure.Documents.SystemFunctions;
    using Microsoft.Extensions.Options;
    using ToDoList.Interfaces;
    using ToDoList.Models;

    public class CosmosDataStore : IDataStore
    {
        private readonly DocumentClient client;
        private readonly FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
        private readonly TelemetryClient telemetryClient = new TelemetryClient();

        public CosmosDataStore(CosmosDataStoreSettings cosmosDataStoreSettings)
        {
            this.client = new DocumentClient(cosmosDataStoreSettings.EndpointUri, cosmosDataStoreSettings.PrimaryKey);
            this.client.CreateDatabaseIfNotExistsAsync(new Database { Id = "Task" }).Wait();
        }

        public async Task<List<BasicTask>> Read(string userId)
        {
            var taskQuery = this.client.CreateDocumentQuery<BasicTask>(UriFactory.CreateDocumentCollectionUri("Task", "Items"), this.queryOptions)
                .Where(t => t.UserId == userId)
                .AsDocumentQuery();

            return (await taskQuery.ExecuteNextAsync<BasicTask>()).ToList();
        }

        public async Task<bool> Create(BasicTask task)
        {
            try
            {
                await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri("Task", "Items"), task);
                return true;
            }
            catch (Exception ex)
            {
                this.telemetryClient.TrackException(ex);
                return false;
            }
        }

        public async Task<bool> Update(BasicTask task)
        {
            try
            {
                BasicTask taskQuery = this.client
                    .CreateDocumentQuery<BasicTask>(
                        UriFactory.CreateDocumentCollectionUri("Task", "Items"),
                        this.queryOptions)
                    .Where(t => t.UserId == task.UserId)
                    .AsEnumerable()
                    .SingleOrDefault();

                taskQuery.Description = task.Description;

                taskQuery.IsComplete = task.IsComplete;

                taskQuery.DueBy = task.DueBy;

                await this.client.ReplaceDocumentAsync(
                    UriFactory.CreateDocumentUri("Task", "Items", task.TaskId.ToString()),
                    taskQuery);

                return true;
            }
            catch (Exception ex)
            {
                this.telemetryClient.TrackException(ex);
                return false;
            }
        }
    }
}