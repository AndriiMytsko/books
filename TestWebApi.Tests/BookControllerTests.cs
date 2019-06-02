using NUnit.Framework;
using Moq;
using TestWebApi.Controllers;
using System.Collections.Generic;
using Repository;
using Microsoft.AspNetCore.Mvc;
using TestWebApi.Models;

namespace Tests
{
    [TestFixture]
    public class BookControllerTests
    {
        private Mock<IBookRepository> _bookRepository;
        private BookController _bookController;

        [SetUp]
        public void Setup()
        {
            _bookRepository = new Mock<IBookRepository>();
            _bookController = new BookController(_bookRepository.Object);
        }

        [Test]
        public void  Get_ReturnsBooks()
        {
            var bookEntities = new List<BookEntity>
            {
                new BookEntity { Title="B1", Author="A1", Id = 0 },
                new BookEntity { Title="B2", Author="A2", Id = 1 },
                new BookEntity { Title="B3", Author="A3", Id = 2 },
                new BookEntity { Title="B4", Author="A4", Id = 3 }
            };

            _bookRepository.Setup(x => x.Get())
                .Returns(bookEntities);

            var response = _bookController.Get();

            Assert.IsNotNull(response);
            var okResult = response.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var books = okResult.Value as IEnumerable<Book>;
            Assert.IsNotNull(books);
            var list = new List<Book>(books);
            Assert.IsTrue(list.Count == 4);
        }

        [Test]
        public void Get_ReturnsEmpty()
        {
            _bookRepository.Setup(repo => repo.Get()).Returns(new List<BookEntity>());
            var response = _bookController.Get();

            Assert.IsNotNull(response);
            var okResult = response.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var books = okResult.Value as IEnumerable<Book>;
            Assert.IsNotNull(books);
            var list = new List<Book>(books);
            Assert.IsEmpty(list);
        }

        [Test]
        public void GetById_NotFount()
        {
            _bookRepository.Setup(repo => repo.Get(1))
                .Returns<BookEntity>(null);

            var response = _bookController.Get(1);

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<NotFoundResult>(response.Result);
        }

        [Test]
        public void GetById_Ok()
        {
            _bookRepository.Setup(repo => repo.Get(1))
                .Returns(new BookEntity());

            var response = _bookController.Get(1);

            Assert.IsNotNull(response);
            var okResult = response.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var book = okResult.Value as Book;
            Assert.IsNotNull(book);
        }

        [Test]
        public void Post_201()
        {
            var book = new Book
            {
                Id = 1,
                Title = "test-title",
                Author = "test-author"
            };

            var bookEntity = new BookEntity
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author
            };

            _bookRepository.Setup(x => x.Add(It.Is<BookEntity>(
                p => p.Author == book.Author
                && p.Title == book.Title)))
                .Returns(1);

            _bookRepository.Setup(x => x.Get(1))
                .Returns(bookEntity);

            var response = _bookController.Post(book);

            Assert.IsNotNull(response);
            var createdResult = response.Result as CreatedResult;
            Assert.IsNotNull(createdResult);
            var createdBook = createdResult.Value as Book;
            Assert.IsNotNull(createdBook);
            Assert.IsTrue(book.Id == createdBook.Id);
            Assert.IsTrue(book.Title == createdBook.Title);
            Assert.IsTrue(book.Author == createdBook.Author);
        }

        [Test]
        public void Put_NotFound()
        {
            var book = new Book
            {
                Id = 1,
                Title = "test-title",
                Author = "test-author"
            };

            _bookRepository.Setup(x => x.Get(book.Id))
                .Returns<BookEntity>(null);

            var response = _bookController.Put(book);
            Assert.IsNotNull(response);
            Assert.IsInstanceOf<NotFoundResult>(response);
        }

        [Test]
        public void Put_NoContent()
        {
            var book = new Book
            {
                Id = 1,
                Title = "test-title",
                Author = "test-author"
            };

            var bookEntity = new BookEntity
            {
                Id = book.Id,
                Title = "test-title-old",
                Author = "test-authror-old"
            };

            _bookRepository.Setup(x => x.Get(1))
                .Returns(bookEntity);

            var response = _bookController.Put(book);

            _bookRepository.Verify(x => x.Update(It.Is<BookEntity>(
               p => p.Author == book.Author
               && p.Id == book.Id
               && p.Title == book.Title)));

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<NoContentResult>(response);
        }

        [Test]
        public void Delete_NoFound()
        {
            _bookRepository.Setup(x => x.Get(1))
                .Returns<BookEntity>(null);

            var response = _bookController.Delete(1);

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<NotFoundResult>(response);
        }

        [Test]
        public void Delete_NoContent()
        {
            _bookRepository.Setup(x => x.Get(1))
               .Returns(new BookEntity { Id = 1 });

            var response = _bookController.Delete(1);

            _bookRepository.Verify(x => x.Delete(1));

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<NoContentResult>(response);
        }
    }
}