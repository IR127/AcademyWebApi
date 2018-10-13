namespace ToDoList
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using System;
    using System.Net;

    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}