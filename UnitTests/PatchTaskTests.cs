using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using NUnit.Framework;
    using NUnit.Framework.Internal;
    using ToDoList.Controllers;
    using ToDoList.Interfaces;
    using ToDoList.Models;

    [TestFixture]
    public class PatchTaskTests
    {
        [TestFixture]
        public class Given_A_Valid_Request_To_Update_A_Task
        {
            [Test]
            public void When_Updating_Then_Return_Ok_Response_Is_Returned()
            {
                var task = new BasicTask
                {
                    UserId = 1234,
                    Description = "Clean Dishes"
                };
                var dataStore = new Mock<IDataStore>();
                dataStore.Setup(x => x.Update(task)).Returns(true);
                var listController = new ListController(dataStore.Object);

                // Act
                var okObjectResult = listController.Patch(task) as OkResult;

                // Assert
                Assert.That(okObjectResult, Is.Not.Null, "OkResponse is returning null");
            }

            [Test]
            public void When_Persistant_Store_Cant_Find_Record_Then_A_Not_Found_Response_Is_Returned()
            {
                // Arrange
                var dataStore = new Mock<IDataStore>();
                dataStore.Setup(x => x.Update(It.IsAny<BasicTask>())).Returns(false);

                var listController = new ListController(dataStore.Object);

                // Act
                var requestResult = listController.Patch(new BasicTask { Description = "Clean Dishes" }) as StatusCodeResult;

                // Assert
                Assert.That(requestResult, Is.Not.Null);
                Assert.That(requestResult.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            }

            [Test]
            public void When_Updating_Then_The_Task_Is_Updated_In_The_Persistent_Store()
            {
                // Arrange
                var task = new BasicTask
                {
                    Description = "Clean Dishes",
                };

                var dataStore = new Mock<IDataStore>();
                var listController = new ListController(dataStore.Object);

                // Act
                listController.Patch(task);

                // Assert
                dataStore.Verify(x => x.Update(task), Times.Once);
            }
        }

        [TestFixture]
        public class Given_An_Invalid_Request_To_Update_A_Task
        {
            [Test]
            public void When_Description_Is_Short_Then_An_Bad_Request_Response_Is_Returned()
            {
                // Arrange
                var dataStore = new Mock<IDataStore>();
                var listController = new ListController(dataStore.Object);

                // Act
                var badRequestResult = listController.Patch(new BasicTask { Description = "Hell" }) as BadRequestObjectResult;

                // Assert
                Assert.That(badRequestResult, Is.Not.Null);
            }
        }
    }
}
