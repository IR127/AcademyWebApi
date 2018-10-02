namespace UnitTests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using NUnit.Framework;
    using ToDoList.Controllers;
    using ToDoList.Interfaces;
    using ToDoList.Models;

    [TestFixture]
    public class GetTaskTests
    {
        [TestFixture]
        public class Given_A_Valid_Request
        {
            [Test]
            public void Then_An_OK_Response_Is_Returned()
            {
                // Arrange
                var tasks = new List<AdvanceTask>()
                {
                    new AdvanceTask { UserId = 1234, TaskId = Guid.NewGuid(), Description = "Clean Dishes", DueBy = new DateTime(2018, 12, 01), Completed = false },
                    new AdvanceTask { UserId = 2345, TaskId = Guid.NewGuid(), Description = "Do homework", DueBy = new DateTime(2018, 09, 21), Completed = true }
                };
                var dataStore = new Mock<IDataStore>();
                dataStore.Setup(x => x.Read(1234)).Returns(tasks);
                var listController = new ListController(dataStore.Object);

                // Act
                var okObjectResult = listController.Get(1234) as OkObjectResult;

                // Assert
                Assert.That(okObjectResult, Is.Not.Null, "OkResponse is returning null");
                var okObjectResultValue = okObjectResult.Value as List<AdvanceTask>;
                Assert.That(okObjectResultValue, Is.Not.Null, "OkResponseValue is returning null");
                Assert.That(okObjectResultValue.Count, Is.EqualTo(2));
                Assert.That(okObjectResultValue[0].UserId, Is.EqualTo(1234));
                Assert.That(okObjectResultValue[1].UserId, Is.EqualTo(2345));
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
                dataStore.Setup(x => x.Read(1234)).Returns(new List<AdvanceTask>());
                var listController = new ListController(dataStore.Object);

                // Act
                var okResult = listController.Get(1234) as OkObjectResult;

                // Assert
                Assert.That(okResult, Is.Not.Null);
                Assert.That(okResult.Value, Is.Empty);
            }
        }

        [TestFixture]
        public class Given_An_Invalid_Request
        {
            [Test]
            public void Then_A_Not_Found_Result_Is_Returned()
            {
                // Arrange
                var dataStore = new Mock<IDataStore>();
                dataStore.Setup(x => x.Read(2345)).Returns((List<AdvanceTask>)null);
                var listController = new ListController(dataStore.Object);

                // Act
                var notFoundResult = listController.Get(2345) as NotFoundResult;

                // Aseert
                Assert.That(notFoundResult, Is.Not.Null, "httpResponseNotFound is returning null");
            }
        }
    }
}