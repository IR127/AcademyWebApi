namespace ToDoList.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Newtonsoft.Json;

    public class BasicTask
    {
        [Required]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "id")]
        [BindNever]
        public Guid TaskId { get; set; }

        [Required]
        [MinLength(5)]
        public string Description { get; set; }

        public DateTime DueBy { get; set; }

        public bool IsComplete { get; set; }

        [BindNever]
        public DateTime Added { get; set; }
    }
}