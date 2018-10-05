namespace ToDoList.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class BasicTask
    {
        [Required]
        public int UserId { get; set; }

        [BindNever]
        public Guid TaskId { get; set; }

        [Required]
        [MinLength(5)]
        public string Description { get; set; }

        public DateTime DueBy { get; set; }

        public bool Completed { get; set; }

        [BindNever]
        public DateTime Added { get; set; }
    }
}