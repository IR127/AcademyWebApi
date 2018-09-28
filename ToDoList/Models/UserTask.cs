namespace ToDoList.Models
{
    using System;

    public class UserTask
    {
        public int UserId { get; set; }

        public int TaskId { get; set; }

        public string Description { get; set; }

        public DateTime DueBy { get; set; }

        public bool Completed { get; set; }
    }
}