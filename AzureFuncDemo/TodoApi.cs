using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AzureFuncDemo
{
    public static class TodoApi
    {
        static List<Todo> todoList = new List<Todo>();

        [FunctionName("CreateTodo")]
        public static async Task<IActionResult> CreateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todo")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Creating a new todo list item.");
            
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            dynamic input = JsonConvert.DeserializeObject<TodoCreateModel>(requestBody);

            var todo = new Todo() { TaskDescription = input.TaskDescription };

            todoList.Add(todo);

            return new OkObjectResult(todo);
        }

        
    }
}
