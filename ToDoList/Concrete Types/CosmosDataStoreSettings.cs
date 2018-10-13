namespace ToDoList.Concrete_Types
{
    using System;

    public class CosmosDataStoreSettings
    {
        public string PrimaryKey { get; set; }

        public Uri EndpointUri { get; set; }
    }
}