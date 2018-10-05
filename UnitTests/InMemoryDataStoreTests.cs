using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    using System.Linq;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Remotion.Linq.Parsing.Structure.IntermediateModel;
    using ToDoList.Concrete_Types;
    using ToDoList.Models;

    [TestFixture]
    public class InMemoryDataStoreTests
    {
        [TestFixture]
        public class Given_A_User_Has_Been_Identified
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
                            UserId = 1234,
                            TaskId = Guid.NewGuid(),
                            Description = "Clean Dishes",
                            DueBy = new DateTime(2018, 12, 01),
                            Completed = false
                        },
                        new BasicTask
                        {
                            UserId = 1234,
                            TaskId = Guid.NewGuid(),
                            Description = "Do homework",
                            DueBy = new DateTime(2018, 09, 21),
                            Completed = true
                        },
                        new BasicTask
                        {
                            UserId = 2345,
                            TaskId = Guid.NewGuid(),
                            Description = "Do homework",
                            DueBy = new DateTime(2018, 09, 21),
                            Completed = true
                        }
                    }
                };
            }

            [Test]
            public void Then_Return_Tasks_For_Sepecifc_User()
            {
                // Act
                var response = this.inMemoryDataStore.Read(1234);

                // Assert
                Assert.That(response.Count(), Is.EqualTo(2));
            }
        }
    }
}
