using NUnit.Framework;
using Repository;
using System;

namespace Tests
{
    public class MemoryBookRepositoryTests
    {
        private IBookRepository _bookRepository;

        [SetUp]
        public void Setup()
        {
            _bookRepository = new MemoryBookRepository();
        }

        [Test]
        public void Add_Ok()
        {
            var book10 = new BookEntity();
            var book11 = new BookEntity();

            var id0 = _bookRepository.Add(book10);
            var id1 = _bookRepository.Add(book11);

            Assert.IsTrue(id0 == 0);
            Assert.IsTrue(id1 == 1);
        }

        [Test]
        public void Delete_Ok()
        {
            var bookEntity = new BookEntity();
            var id = _bookRepository.Add(bookEntity);
            _bookRepository.Delete(id);
        }

        [Test]
        public void Delete_ExceptionNotExist()
        {
            Assert.Throws<InvalidOperationException>(
                () => _bookRepository.Delete(10));
        }

        [Test]
        public void GetById_Ok()
        {
            var bookEntity = new BookEntity();
            var id = _bookRepository.Add(bookEntity);
            var book = _bookRepository.Get(id);

            Assert.IsNotNull(book);
        }

        [Test]
        public void GetById_Null()
        {
            var book = _bookRepository.Get(100);

            Assert.IsNull(book);
        }

        [Test]
        public void GetAll_Empty()
        {
            var books = _bookRepository.Get();

            Assert.IsEmpty(books);
        }

        [Test]
        public void GetAll_Ok()
        {
            var book0 = new BookEntity();
            var book1 = new BookEntity();
            _bookRepository.Add(book0);
            _bookRepository.Add(book1);

            var books = _bookRepository.Get();

            Assert.IsNotEmpty(books);
            Assert.IsTrue(books.Count == 2);
        }

        [Test]
        public void Update_NotExist()
        {
            var bookEntity = new BookEntity
            {
                Id = 10,
                Title = "title",
                Author = "author"
            };

            Assert.Throws<InvalidOperationException>(
                () => _bookRepository.Update(bookEntity));
        }

        [Test]
        public void Update()
        {
            var bookEntity = new BookEntity
            {
                Title = "title",
                Author = "author"
            };

            var id = _bookRepository.Add(bookEntity);

            var newBookEntity = new BookEntity
            {
                Id = id,
                Title = "title-new",
                Author = "author-new"
            };

            _bookRepository.Update(newBookEntity);

            var updatedBookEntity = _bookRepository.Get(newBookEntity.Id);

            Assert.IsNotNull(updatedBookEntity);
            Assert.IsTrue(updatedBookEntity.Id == newBookEntity.Id);
            Assert.IsTrue(updatedBookEntity.Title == newBookEntity.Title);
            Assert.IsTrue(updatedBookEntity.Author == newBookEntity.Author);
        }
    }
}