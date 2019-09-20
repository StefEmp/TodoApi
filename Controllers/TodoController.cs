using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;
using System;
using System.Threading.Tasks;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : Controller
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        // create a form which posts to an endpoint on the server
        // send a name attribute(string) from the form to the server
        // persist a new todo item to the database

        [HttpGet]
        public ActionResult<string> GetAll()
          
        {
            // return _context.TodoItems.ToList();
            //ViewData["Greeting"] = "Goodbye";
            var items = _context.TodoItems.ToList();
            return View(items);
        }
        // retun a view which just says hello world - done
        // return a view which has a dynamic variable (string) - done
        // return a view which has one TodoItem
        // return a view which has a list TodoItem

        [HttpGet("{id}", Name = "GetTodo")]
        public ActionResult<TodoItem> GetById(long id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }
        [HttpPost]
        public ActionResult<string> NewToDoItem([FromForm] string name)
        {
            _context.TodoItems.Add(
                new TodoItem { Name = name,
                CreatedAt = DateTime.Now }
                );
            _context.SaveChanges();
            return Redirect("/api/todo");
        }
        // DELETE: api/Todo/5
        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteTodoItem([FromForm] long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            //return NoContent();
            return Redirect("/api/todo");
        }
    }
}