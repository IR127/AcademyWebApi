namespace ToDoList.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class AdvanceTask
    {
        public string UserId { get; set; }

        public Guid TaskId { get; set; }

        public string Description { get; set; }

        public DateTime DueBy { get; set; }

        public bool Completed { get; set; }

        public DateTime Added { get; set; }

        public bool PastDueDate { get; set; }

        public bool DueWithin24 { get; set; }
    }
}