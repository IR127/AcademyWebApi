namespace UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Net.Cache;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using NUnit.Framework;
    using ToDoList.Controllers;
    using ToDoList.Interfaces;
    using ToDoList.Models;

    [TestFixture]
    public class PutTaskTests
    {
        [TestFixture]
        public class Given_A_Valid_Request
        {
            [Test]
            public void Then_An_Created_Response_Is_Returned()
            {
                // Arrange
                var task = new UserTask
                {
                    UserId = 1234,
                    TaskId = 1,
                    Description = "Clean Dishes",
                    DueBy = new DateTime(2018, 12, 01),
                    Completed = false
                };

                var dataStore = new Mock<IDataStore>();
                dataStore.Setup(x => x.Create(It.IsAny<UserTask>())).Returns(true);
                var listController = new ListController(dataStore.Object);
                listController.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
                listController.ControllerContext.HttpContext.Request.Path = $"/api/list/{task.UserId}";

                // Act
                var createdResult = listController.Put(task) as CreatedResult;

                // Assert
                Assert.That(createdResult, Is.Not.Null);
                Assert.That(createdResult.Location, Is.EqualTo($"/api/list/{task.UserId}"));
            }
        }

        [TestFixture]
        public class Given_A_User_With_No_Todo_List_Items
        {
            [Test]
            public void Then_An_OK_Response_Is_Returned()
            {
                // Arrange
                var dataStore = new Mock<IDataStore>();
                dataStore.Setup(x => x.Read(1234)).Returns(new List<UserTask>());
                var listController = new ListController(dataStore.Object);

                // Act
                var okObjectResult = listController.Get(1234) as OkObjectResult;

                // Assert
                Assert.That(okObjectResult, Is.Not.Null);
                Assert.That(okObjectResult.Value, Is.Empty);
            }
        }

        [TestFixture]
        public class Given_An_Invalid_Customer
        {
            [Test]
            public void Then_A_Not_Found_Result_Is_Returned()
            {
                // Arrange
                var dataStore = new Mock<IDataStore>();
                dataStore.Setup(x => x.Read(2345)).Returns((List<UserTask>)null);
                var listController = new ListController(dataStore.Object);

                // Act
                var notFoundResult = listController.Get(2345) as NotFoundResult;

                // Assert
                Assert.That(notFoundResult, Is.Not.Null, "httpResponseNotFound is returning null");
            }
        }
    }
}