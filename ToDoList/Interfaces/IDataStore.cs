namespace ToDoList.Interfaces
{
    using System.Collections.Generic;
    using ToDoList.Models;

    public interface IDataStore
    {
        IEnumerable<UserTask> Read(int userId);

        bool Create(UserTask task);
    }
}
