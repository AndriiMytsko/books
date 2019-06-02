using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TestWebApi.Models;
using System.Linq;
using Repository;

namespace TestWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;        

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Book>> Get()
        {
            var entities = _bookRepository.Get();
            var books = entities.Select(x => Map(x));

            return Ok(books);
        }

        [HttpGet("{id}")]
        public ActionResult<Book> Get(int id)
        {
            var entity = _bookRepository.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            return Ok(Map(entity));
        }

        [HttpPost]
        public ActionResult<Book> Post([FromBody]Book book)
        {
            var entity = new BookEntity
            {
                Title = book.Title,
                Author = book.Author
            };

            var id = _bookRepository.Add(entity);

            entity = _bookRepository.Get(id);

            return Created(string.Empty, Map(entity));
        }
        
        [HttpPut]
        public IActionResult Put([FromBody] Book book)
        {
            var entity = _bookRepository.Get(book.Id);
            if (entity == null)
            {
                return NotFound();
            }

            entity.Author = book.Author;
            entity.Title = book.Title;
            _bookRepository.Update(entity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _bookRepository.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            _bookRepository.Delete(entity.Id);

            return NoContent();
        }

        private BookEntity Map(Book book)
        {
            return new BookEntity
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author
            };
        }

        private Book Map(BookEntity entity)
        {
            return new Book
            {
                Id = entity.Id,
                Title = entity.Title,
                Author = entity.Author
            };
        }
    }
}