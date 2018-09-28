using System;

namespace UnitTests
{
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;

    public class GetTaskTests
    {
        [TestFixture]
        public class GetTodoListTests
        {

            [TestFixture]
            public class Given_A_Valid_Request
            {
                [Test]
                public void Then_An_OK_Response_Is_Returned()
                {
                    //Arrange
                    List<UserTask> tasks = new List<UserTask>()
                    {
                        new UserTask(){ UserId = 1234, TaskId = 1, Description = "Clean Dishes", DueBy = new DateTime(2018,12,01), Completed = false},
                        new UserTask(){ UserId = 1234, TaskId = 2, Description = "Do homework", DueBy = new DateTime(2018,09,21), Completed = true}
                    };
                    var dataStore = new Mock<IDataStore>();
                    dataStore.Setup(x => x.Read(1234)).Returns(tasks);

                    //Act
                    var getTodoList = new TodoListController(dataStore.Object);

                    //Assert
                    var okResponse = getTodoList.Get(1234) as OkObjectResult;
                    Assert.That(okResponse, Is.Not.Null, "OkResponse is returning null");
                    var okResponseValue = okResponse.Value as List<UserTask>;
                    Assert.That(okResponseValue, Is.Not.Null, "OkResponseValue is returning null");
                    Assert.That(okResponseValue.Count, Is.EqualTo(2));
                    Assert.That(okResponseValue[0].TaskId, Is.EqualTo(1));
                    Assert.That(okResponseValue[1].TaskId, Is.EqualTo(2));

                }
            }

            [TestFixture]
            public class Given_A_User_With_No_Todo_List_Items
            {
                [Test]
                public void Then_An_OK_Response_Is_Returned()
                {
                    //Arrange
                    var dataStore = new Mock<IDataStore>();
                    dataStore.Setup(x => x.Read(1234)).Returns(new List<UserTask>());

                    //Act
                    var getTodoList = new TodoListController(dataStore.Object);

                    //Assert
                    var okResponse = getTodoList.Get(1234) as OkObjectResult;
                    Assert.That(okResponse, Is.Not.Null);
                    Assert.That(okResponse.Value, Is.Empty);
                }
            }

            [TestFixture]
            public class Given_An_Invalid_Customer
            {
                [Test]
                public void Then_A_Not_Found_Result_Is_Returned()
                {
                    var dataStore = new Mock<IDataStore>();
                    dataStore.Setup(x => x.Read(2345)).Returns((List<UserTask>)null);

                    //Act
                    var getTodoList = new TodoListController(dataStore.Object);
                    var httpResponseNotFound = getTodoList.Get(2345) as NotFoundResult;
                    Assert.That(httpResponseNotFound, Is.Not.Null, "httpResponseNotFound is returning null");
                }
            }
        }
    }
}
