namespace UnitTests
{
    using System;
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
        //TODO: Implement when the task was added.

        [TestFixture]
        public class Given_A_Valid_Request_To_Create_A_Task
        {
            [Test]
            public void When_Creating_Then_A_Created_Response_Is_Returned()
            {
                // Arrange
                var task = new BasicTask
                {
                    UserId = 1234,
                    Description = "Clean Dishes",
                    DueBy = new DateTime(2018, 12, 01),
                    Completed = false
                };

                var dataStore = new Mock<IDataStore>();
                dataStore.Setup(x => x.Create(It.IsAny<BasicTask>())).Returns(true);

                var listController = new ListController(dataStore.Object);
                listController.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
                listController.ControllerContext.HttpContext.Request.Path = $"/api/list/{task.UserId}";

                // Act
                var createdResult = listController.Post(task) as CreatedResult;

                // Assert
                Assert.That(createdResult, Is.Not.Null);
                Assert.That(createdResult.Location, Is.EqualTo($"/api/list/{task.UserId}"));
                var createdResultValue = createdResult.Value as BasicTask;
                Assert.That(createdResultValue, Is.Not.Null);
                Assert.That(createdResultValue.UserId, Is.EqualTo(task.UserId), "User values don't match up");
                Assert.That(createdResultValue.Description, Is.EqualTo(task.Description), "Description values do not match up");
                Assert.That(createdResultValue.DueBy, Is.EqualTo(task.DueBy), "DueBy values do not match up");
                Assert.That(createdResultValue.Completed, Is.EqualTo(task.Completed), "Completed values do not match up");
            }

            [Test]
            public void When_A_Task_Id_Is_Included_Then_Repalce_With_Unique_Identifier()
            {
                // Arrange
                var taskId = Guid.NewGuid();

                var task = new BasicTask
                {
                    Description = "Hello",
                    TaskId = taskId
                };

                var dataStore = new Mock<IDataStore>();
                dataStore.Setup(x => x.Create(It.IsAny<BasicTask>())).Returns(true);

                var listController = new ListController(dataStore.Object);
                listController.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
                listController.ControllerContext.HttpContext.Request.Path = $"/api/list/{task.UserId}";

                // Act
                var createdResult = listController.Post(task) as CreatedResult;

                // Assert
                Assert.That(createdResult, Is.Not.Null);
                var createdResultValue = createdResult.Value as BasicTask;
                Assert.That(createdResultValue, Is.Not.Null);
                Assert.That(createdResultValue.TaskId, Is.Not.EqualTo(taskId));
            }

            [Test]
            public void When_Creating_Then_The_Task_Is_Added_To_The_Persistent_Store()
            {
                var task = new BasicTask
                {
                    Description = "Clean Dishes",
                };

                var dataStore = new Mock<IDataStore>();
                var listController = new ListController(dataStore.Object);
                listController.Post(task);
                dataStore.Verify(x => x.Create(task), Times.Once);
            }

            [Test]
            public void When_Persistant_Store_Fails_Then_A_Internal_Server_Error_Is_Returned()
            {
                // Arrange
                var dataStore = new Mock<IDataStore>();
                dataStore.Setup(x => x.Create(It.IsAny<BasicTask>())).Returns(false);

                var listController = new ListController(dataStore.Object);

                // Act
                var requestResult = listController.Post(new BasicTask() { Description = "Hello" }) as StatusCodeResult;

                // Assert
                Assert.That(requestResult, Is.Not.Null);
                Assert.That(requestResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
            }
        }

        [TestFixture]
        public class Given_An_Invalid_Request_To_Create_A_Task
        {
            [Test]
            public void When_Description_Is_Incorrect_Then_An_Bad_Request_Response_Is_Returned()
            {
                // Arrange
                var dataStore = new Mock<IDataStore>();
                var listController = new ListController(dataStore.Object);

                // Act
                var badRequestResult = listController.Post(new BasicTask { Description = "Hell" }) as BadRequestObjectResult;

                // Assert
                Assert.That(badRequestResult, Is.Not.Null);
            }

            [Test]
            public void When_Description_Is_Missing_Then_An_Bad_Request_Response_Is_Returned()
            {
                // Arrange
                var dataStore = new Mock<IDataStore>();
                var listController = new ListController(dataStore.Object);

                // Act
                var badRequestResult = listController.Post(new BasicTask()) as BadRequestObjectResult;

                // Assert
                Assert.That(badRequestResult, Is.Not.Null);
            }
        }
    }
}