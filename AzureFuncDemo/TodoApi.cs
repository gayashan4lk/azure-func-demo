using System;
using System.IO;
using System.Linq;
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
        public static List<Todo> todoList = new List<Todo>();

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

        [FunctionName("GetAllTodos")]
        public static async Task<IActionResult> GetAllTodos(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Getting all the todo list items.");

            return new OkObjectResult(todoList);
        }

        [FunctionName("GetTodoById")]
        public static async Task<IActionResult> GetTodoById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("Get a single todo item by id.");
            
            var todoItem = todoList.FirstOrDefault(x => x.Id == id);

            if(todoItem == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(todoItem);
        }

        /*[FunctionName("UpdateTodoItem")]
        public async Task<IActionResult> UpdateTodoItem(
            [HttpTrigger(AuthorizationLevel.Anonymous,  "put", Route = "todo/{id")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Update a todo item.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            
            dynamic data = JsonConvert.DeserializeObject<TodoUpdateModel>(requestBody);

            var todoItem = todoList.FirstOrDefault(x=>x.Id == data.Id);

            //todoList.;

            todoItem.TaskDescription = data.TaskDescription;
            todoItem.IsCompleted = data.IsCompleted;

            //todoList.Add(todoItem);

            return new OkObjectResult("done");
        }*/

    }
}
