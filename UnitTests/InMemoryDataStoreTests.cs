namespace UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using ToDoList.Concrete_Types;
    using ToDoList.Models;

    [TestFixture]
    public class InMemoryDataStoreTests
    {
        [TestFixture]
        public class Given_A_Get_Request_From_Client
        {
            private InMemoryDataStore inMemoryDataStore;

            [SetUp]
            public void When_Reading_Data_From_The_Persistant_Store()
            {
                // Arrange
                this.inMemoryDataStore = new InMemoryDataStore
                {
                    Tasks = new List<BasicTask>()
                    {
                        new BasicTask
                        {
                            UserId = "1234",
                            TaskId = Guid.NewGuid(),
                            Description = "Clean Dishes",
                            DueBy = new DateTime(2018, 12, 01),
                            IsComplete = false
                        },
                        new BasicTask
                        {
                            UserId = "1234",
                            TaskId = Guid.NewGuid(),
                            Description = "Do homework",
                            DueBy = new DateTime(2018, 09, 21),
                            IsComplete = true
                        },
                        new BasicTask
                        {
                            UserId = "2345",
                            TaskId = Guid.NewGuid(),
                            Description = "Do homework",
                            DueBy = new DateTime(2018, 09, 21),
                            IsComplete = true
                        }
                    }
                };
            }

            [Test]
            public async Task Then_Return_Tasks_For_Sepecifc_User()
            {
                // Act
                var response = await this.inMemoryDataStore.Read("1234");

                // Assert
                Assert.That(response.Count(), Is.EqualTo(2));
            }

            [Test]
            public async Task Then_Return_Empty_For_Non_Existant_User()
            {
                // Act
                var response = await this.inMemoryDataStore.Read("8910");

                // Assert
                Assert.That(response, Is.Empty);
            }
        }

        [TestFixture]
        public class Given_A_Post_Request_From_Client
        {
            private InMemoryDataStore inMemoryDataStore;

            [SetUp]
            public void When_Creating_A_New_Task_In_The_Persistant_Store()
            {
                // Arrange
                this.inMemoryDataStore = new InMemoryDataStore
                {
                    Tasks = new List<BasicTask>()
                    {
                        new BasicTask
                        {
                            UserId = "1234",
                            TaskId = Guid.NewGuid(),
                            Description = "Clean Dishes",
                            DueBy = new DateTime(2018, 12, 01),
                            IsComplete = false
                        },
                        new BasicTask
                        {
                            UserId = "1234",
                            TaskId = Guid.NewGuid(),
                            Description = "Do homework",
                            DueBy = new DateTime(2018, 09, 21),
                            IsComplete = true
                        },
                        new BasicTask
                        {
                            UserId = "2345",
                            TaskId = Guid.NewGuid(),
                            Description = "Do homework",
                            DueBy = new DateTime(2018, 09, 21),
                            IsComplete = true
                        }
                    }
                };
            }

            [Test]
            public void Then_Add_New_task_To_Store_And_Return_True()
            {
                // Arrange
                var newTask = new BasicTask
                {
                    UserId = "1234",
                    TaskId = Guid.NewGuid(),
                    Description = "Test Task",
                    DueBy = new DateTime(2018, 12, 12),
                    IsComplete = true
                };

                // Act
                this.inMemoryDataStore.Create(newTask);

                // Assert
                Assert.That(this.inMemoryDataStore.Tasks.Count, Is.EqualTo(4));
            }
        }
    }
}
