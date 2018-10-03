namespace ToDoList.Interfaces
{
    using System.Collections.Generic;
    using ToDoList.Models;

    public interface IDataStore
    {
        IEnumerable<BasicTask> Read(int userId);

        bool Create(BasicTask task);
    }
}
