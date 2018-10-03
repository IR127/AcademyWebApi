namespace UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Microsoft.AspNetCore.Http;
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
        public class Given_A_Valid_Request_To_Get_All_Tasks
        {
            [Test]
            public void When_A_User_Has_Tasks_Then_A_OK_Response_With_Tasks_Is_Returned()
            {
                // Arrange
                var tasks = new List<BasicTask>()
                {
                    new BasicTask { UserId = 1234, TaskId = Guid.NewGuid(), Description = "Clean Dishes", DueBy = new DateTime(2018, 12, 01), Completed = false },
                    new BasicTask { UserId = 1234, TaskId = Guid.NewGuid(), Description = "Do homework", DueBy = new DateTime(2018, 09, 21), Completed = true }
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
                Assert.That(okObjectResultValue[0].Description, Is.EqualTo("Clean Dishes"));
                Assert.That(okObjectResultValue[1].Description, Is.EqualTo("Do homework"));
            }

            [Test]
            public void When_A_User_Has_No_Tasks_Then_A_No_Content_Response_Is_Returned()
            {
                // Arrange
                var dataStore = new Mock<IDataStore>();
                dataStore.Setup(x => x.Read(It.IsAny<int>())).Returns((List<BasicTask>)null);
                var listController = new ListController(dataStore.Object);

                // Act
                var noContentResult = listController.Get(1234) as NoContentResult;

                // Assert
                Assert.That(noContentResult, Is.Not.Null);
                Assert.That(noContentResult.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
            }

            public List<AdvanceTask> ValidationSetup()
            {
                // Arrange
                var tasks = new List<BasicTask>()
                {
                    new BasicTask { DueBy = new DateTime(2018, 08, 01) },
                    new BasicTask { DueBy = DateTime.Now.AddHours(12) }
                };

                var dataStore = new Mock<IDataStore>();
                dataStore.Setup(x => x.Read(It.IsAny<int>())).Returns(tasks);
                var listController = new ListController(dataStore.Object);

                // Act
                var okObjectResult = listController.Get(1234) as OkObjectResult;

                // Assert
                Assert.That(okObjectResult, Is.Not.Null, "OkResponse is returning null");
                var okObjectResultValue = okObjectResult.Value as List<AdvanceTask>;
                Assert.That(okObjectResultValue, Is.Not.Null, "OkResponseValue is returning null");
                return okObjectResultValue;
            }

            [Test]
            public void When_Returning_Tasks_To_Client_Then_Return_Past_Due_Date()
            {
                var okObjectResultValue = this.ValidationSetup();
                Assert.That(okObjectResultValue[0].PastDueDate, Is.EqualTo(true));
                Assert.That(okObjectResultValue[1].PastDueDate, Is.EqualTo(false));
            }

            [Test]
            public void When_Returning_Tasks_To_Client_Then_Return_Whitin_24_Hours()
            {
                var okObjectResultValue = this.ValidationSetup();
                Assert.That(okObjectResultValue[0].DueWithin24, Is.EqualTo(true));
                Assert.That(okObjectResultValue[1].DueWithin24, Is.EqualTo(false));
            }
        }
    }
}