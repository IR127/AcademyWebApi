namespace ToDoList.Concrete_Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
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
            };
        }

        public List<BasicTask> Tasks { get; set; }

        public Task<List<BasicTask>> Read(string userId)
        {
            return Task.FromResult(this.Tasks.Where(x => x.UserId == userId.ToString()).ToList());
        }

        public Task<bool> Create(BasicTask task)
        {
            try
            {
                this.Tasks.Add(task);
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public Task<bool> Update(BasicTask task)
        {
            throw new NotImplementedException();
        }
    }
}
