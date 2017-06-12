
// 4. Add a controller
// 4.1 Add CRUD methods (get, create, update, delete)
// 4.2 Test in Postman software

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using System.Linq;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new TodoItem { TaskContent = "Item1" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            return _context.TodoItems.ToList();    // MVC automatically serializes the object to JSON and writes it into the body of the response message
        }

        // assign id in URL to method id parameter
        // Name = "" creates a named route and allows you to link to this route in an HTTP response.
        [HttpGet("{id}", Name = "GetTodo")]     
        public IActionResult GetById(long id)
        {
            var item = _context.TodoItems.FirstOrDefault(x => x.Id == id);
            if (item == null) return NotFound();
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item)   // [FromBody] attribute tells MVC to get the value of the TodoItem from the body of the HTTP request
        {
            if (item == null) return BadRequest();

            _context.TodoItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);  
            // returns a 201 response; adds a location header to the response
            // "GetTodo" is created in the above GetById method
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] TodoItem item)
        {
            if (item == null || item.Id != id)
                return BadRequest();

            var todo = _context.TodoItems.FirstOrDefault(x => x.Id == id);
            if (todo == null)
                return NotFound();

            todo.IsComplete = item.IsComplete;
            todo.TaskContent = item.TaskContent;

            _context.TodoItems.Update(todo);
            _context.SaveChanges();

            return new NoContentResult();   // 204 response
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var todo = _context.TodoItems.First(x => x.Id == id);
            if (todo == null) return NotFound();

            _context.TodoItems.Remove(todo);
            _context.SaveChanges();

            return new NoContentResult();   // 204 response
        }
    }
}