namespace ToDoList.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using ToDoList.Models;

    public interface IDataStore
    {
        Task<List<BasicTask>> Read(string userId);

        Task<bool> Create(BasicTask task);

        Task<bool> Update(BasicTask task);
    }
}
