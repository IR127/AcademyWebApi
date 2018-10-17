namespace UnitTests
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using NUnit.Framework;
    using ToDoList.Controllers;
    using ToDoList.Interfaces;
    using ToDoList.Models;

    [TestFixture]
    public class PostTaskTests
    {
        [TestFixture]
        public class Given_A_Valid_Request_To_Create_A_Task
        {
            [Test]
            public async Task When_Creating_Then_A_Created_Response_Is_Returned()
            {
                // Arrange
                var task = new BasicTask
                {
                    UserId = "1234",
                    Description = "Clean Dishes",
                    DueBy = new DateTime(2018, 12, 01),
                    IsComplete = false
                };

                var dataStore = new Mock<IDataStore>();
                dataStore.Setup(x => x.Create(It.IsAny<BasicTask>())).Returns(Task.FromResult(true));

                var listController = new ListController(dataStore.Object);
                listController.ControllerContext = new ControllerContext {HttpContext = new DefaultHttpContext()};
                listController.ControllerContext.HttpContext.Request.Path = $"/api/list/{task.UserId}";

                // Act
                var createdResult = await listController.Post(task) as CreatedResult;

                // Assert
                Assert.That(createdResult, Is.Not.Null);
                Assert.That(createdResult.Location, Is.EqualTo($"/api/list/{task.UserId}"));
                var createdResultValue = createdResult.Value as BasicTask;
                Assert.That(createdResultValue, Is.Not.Null);
                Assert.That(createdResultValue.UserId, Is.EqualTo(task.UserId), "User values don't match up");
                Assert.That(createdResultValue.Description, Is.EqualTo(task.Description), "Description values do not match up");
                Assert.That(createdResultValue.DueBy, Is.EqualTo(task.DueBy), "DueBy values do not match up");
                Assert.That(createdResultValue.IsComplete, Is.EqualTo(task.IsComplete), "isComplete values do not match up");
            }

            [Test]
            public async Task When_Creating_Then_The_Added_Field_Is_Generated()
            {
                // Arrange
                var randomDate = new DateTime(2018, 12, 01, 13, 10, 01);

                var task = new BasicTask
                {
                    UserId = "1234",
                    Description = "Clean Dishes",
                    DueBy = new DateTime(2018, 12, 01),
                    IsComplete = false,
                    Added = randomDate
                };

                var dataStore = new Mock<IDataStore>();
                dataStore.Setup(x => x.Create(It.IsAny<BasicTask>())).Returns(Task.FromResult(true));

                var listController = new ListController(dataStore.Object);
                listController.ControllerContext = new ControllerContext {HttpContext = new DefaultHttpContext()};
                listController.ControllerContext.HttpContext.Request.Path = $"/api/list/{task.UserId}";

                // Act
                var createdResult = await listController.Post(task) as CreatedResult;

                // Assert
                Assert.That(createdResult, Is.Not.Null);
                var createdResultValue = createdResult.Value as BasicTask;
                Assert.That(createdResultValue, Is.Not.Null);
                Assert.That(createdResultValue.Added, Is.Not.EqualTo(randomDate));
            }

            [Test]
            public void When_Creating_Then_The_Task_Is_Added_To_The_Persistent_Store()
            {
                var task = new BasicTask
                {
                    Description = "Clean Dishes"
                };

                var dataStore = new Mock<IDataStore>();
                var listController = new ListController(dataStore.Object);
                listController.Post(task);
                dataStore.Verify(x => x.Create(task), Times.Once);
            }

            [Test]
            public async Task When_Persistant_Store_Fails_Then_A_Bad_Request_Is_Returned()
            {
                // Arrange
                var dataStore = new Mock<IDataStore>();
                dataStore.Setup(x => x.Create(It.IsAny<BasicTask>())).Returns(Task.FromResult(false));

                var listController = new ListController(dataStore.Object);

                // Act
                var requestResult = await listController.Post(new BasicTask {Description = "Hello"});

                // Assert
                Assert.That(requestResult, Is.InstanceOf<BadRequestObjectResult>());
            }
        }

        [TestFixture]
        public class Given_An_Invalid_Request_To_Create_A_Task
        {
            [Test]
            public async Task When_Description_Is_Missing_Then_An_Bad_Request_Response_Is_Returned()
            {
                // Arrange
                var dataStore = new Mock<IDataStore>();
                var listController = new ListController(dataStore.Object);

                // Act
                var badRequestResult = await listController.Post(new BasicTask()) as BadRequestObjectResult;

                // Assert
                Assert.That(badRequestResult, Is.Not.Null);
            }

            [Test]
            public async Task When_Description_Is_Short_Then_An_Bad_Request_Response_Is_Returned()
            {
                // Arrange
                var dataStore = new Mock<IDataStore>();
                var listController = new ListController(dataStore.Object);

                // Act
                var badRequestResult =
                    await listController.Post(new BasicTask {Description = "Hell"}) as BadRequestObjectResult;

                // Assert
                Assert.That(badRequestResult, Is.Not.Null);
            }

            [Test]
            public async Task When_Task_Supplied_Is_Null_Then_An_Bad_Request_Response_Is_Returned()
            {
                // Arrange
                var dataStore = new Mock<IDataStore>();
                var listController = new ListController(dataStore.Object);

                // Act
                var badRequestResult = await listController.Post(null);

                // Assert
                Assert.That(badRequestResult, Is.InstanceOf<BadRequestObjectResult>());
            }
        }
    }
}