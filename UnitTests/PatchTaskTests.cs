namespace UnitTests
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using NUnit.Framework;
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
            public async Task When_Updating_Then_Return_Ok_Response_Is_Returned()
            {
                var task = new BasicTask
                {
                    UserId = "1234",
                    Description = "Clean Dishes"
                };
                var dataStore = new Mock<IDataStore>();
                dataStore.Setup(x => x.Update(task)).Returns(Task.FromResult(true));
                var listController = new ListController(dataStore.Object);

                // Act
                var okObjectResult = await listController.Patch(task);

                // Assert
                Assert.That(okObjectResult, Is.InstanceOf<OkResult>());
            }

            [Test]
            public async Task When_Persistant_Store_Cant_Find_Record_Then_A_Not_Found_Response_Is_Returned()
            {
                // Arrange
                var dataStore = new Mock<IDataStore>();
                dataStore.Setup(x => x.Update(It.IsAny<BasicTask>())).Returns(Task.FromResult(false));

                var listController = new ListController(dataStore.Object);

                // Act
                var requestResult = await listController.Patch(new BasicTask { Description = "Clean Dishes" });

                // Assert
                Assert.That(requestResult, Is.InstanceOf<NotFoundResult>());
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
            public async Task When_Description_Is_Short_Then_An_Bad_Request_Response_Is_Returned()
            {
                // Arrange
                var dataStore = new Mock<IDataStore>();
                var listController = new ListController(dataStore.Object);

                // Act
                var badRequestResult = await listController.Patch(new BasicTask {Description = "Hell"});

                // Assert
                Assert.That(badRequestResult, Is.InstanceOf<BadRequestObjectResult>());
            }

            [Test]
            public async Task When_Description_Is_Missing_Then_An_Bad_Request_Response_Is_Returned()
            {
                // Arrange
                var dataStore = new Mock<IDataStore>();
                var listController = new ListController(dataStore.Object);

                // Act
                var badRequestResult = await listController.Patch(new BasicTask());

                // Assert
                Assert.That(badRequestResult, Is.InstanceOf<BadRequestObjectResult>());
            }

            [Test]
            public async Task When_Task_Supplied_Is_Null_Then_An_Bad_Request_Response_Is_Returned()
            {
                // Arrange
                var dataStore = new Mock<IDataStore>();
                var listController = new ListController(dataStore.Object);

                // Act
                var badRequestResult = await listController.Patch(null);

                // Assert
                Assert.That(badRequestResult, Is.InstanceOf<BadRequestObjectResult>());
            }
        }
    }
}
