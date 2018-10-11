using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents.Client;
    using NUnit.Framework;
    using ToDoList.Concrete_Types;
    using ToDoList.Models;

    [TestFixture]
    public class CosmosDbTests
    {
        [Test]
        public async Task Unsuccesfully_Read_Task_In_Database()
        {
            var dataStore = new CosmosDataStore();
            var response = await dataStore.Read(Guid.NewGuid().ToString());
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

            var dataStore = new CosmosDataStore();

            // Act
            await dataStore.Create(task);
            var response = await dataStore.Read(task.UserId);

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

            var dataStore = new CosmosDataStore();

            // Act
            await dataStore.Create(task);
            await dataStore.Update(task2);
            var response = await dataStore.Read(task2.UserId);

            // Assert
            Assert.That(response[0].Description, Is.Not.EqualTo(task.Description));
        }
    }
}
