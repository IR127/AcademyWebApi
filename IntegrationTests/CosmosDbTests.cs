namespace IntegrationTests
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using NUnit.Framework;
    using ToDoList.Concrete_Types;
    using ToDoList.Models;

    [TestFixture]
    public class CosmosDbTests
    {
        private CosmosDataStore cosmosDataStore;

        [SetUp]
        public void Setup()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("secrets.json");

            var config = configBuilder.Build();

            var cosmosDataStoreSettings = new CosmosDataStoreSettings();
            config.Bind(nameof(cosmosDataStoreSettings), cosmosDataStoreSettings);

            this.cosmosDataStore = new CosmosDataStore(cosmosDataStoreSettings);
        }

        [Test]
        public async Task Unsuccesfully_Read_Task_In_Database()
        {
            var response = await this.cosmosDataStore.Read(Guid.NewGuid().ToString());
            Assert.That(response.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task Succesfully_Create_And_Read_Task_In_Database()
        {
            // Arrange
            var task = new BasicTask
            {
                UserId = Guid.NewGuid().ToString(),
                TaskId = Guid.NewGuid(),
                Description = "Clean Dishes",
                DueBy = new DateTime(2018, 12, 01),
                IsComplete = false
            };

            // Act
            await this.cosmosDataStore.Create(task);
            var response = await this.cosmosDataStore.Read(task.UserId);

            // Assert
            Assert.That(response.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task Succesfully_Update_Task_Database()
        {
            // Arrange
            var task = new BasicTask
            {
                UserId = Guid.NewGuid().ToString(),
                TaskId = Guid.NewGuid(),
                Description = "Clean Dishes",
                DueBy = new DateTime(2018, 12, 01),
                IsComplete = true
            };

            var task2 = new BasicTask
            {
                UserId = task.UserId,
                TaskId = task.TaskId,
                Description = "Update Works",
                DueBy = new DateTime(2017, 12, 01),
                IsComplete = false
            };

            // Act
            await this.cosmosDataStore.Create(task);
            await this.cosmosDataStore.Update(task2);
            var response = await this.cosmosDataStore.Read(task2.UserId);

            // Assert
            Assert.That(response[0].Description, Is.Not.EqualTo(task.Description));
        }
    }
}