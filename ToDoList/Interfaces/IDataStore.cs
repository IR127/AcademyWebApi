namespace ToDoList.Interfaces
{
    using System.Collections.Generic;

    public interface IDataStore
    {
        IEnumerable<UserTask> Read(int userId);

        bool Create(UserTask task);
    }
}
