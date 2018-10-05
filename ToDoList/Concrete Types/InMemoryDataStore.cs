namespace ToDoList.Concrete_Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ToDoList.Interfaces;
    using ToDoList.Models;

    public class InMemoryDataStore : IDataStore
    {
        public InMemoryDataStore()
        {
            this.Tasks = new List<BasicTask>()
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
            };
        }

        public List<BasicTask> Tasks { get; set; }

        public IEnumerable<BasicTask> Read(int userId)
        {
            return this.Tasks.Where(x => x.UserId == userId);
        }

        public bool Create(BasicTask task)
        {
            throw new NotImplementedException();
        }

        public bool Update(BasicTask task)
        {
            throw new NotImplementedException();
        }
    }
}
