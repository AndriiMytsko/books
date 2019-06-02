using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Repository
{
    public class MemoryBookRepository : IBookRepository
    {
        private readonly IList<BookEntity> _books = new List<BookEntity>();

        public int Add(BookEntity book)
        {
            int nextId = 0;
            if (_books.Any())
            {
                nextId = _books.Max(x => x.Id);
                book.Id = ++nextId;
            }

            _books.Add(book);

            return book.Id;
        }

        public void Delete(int id)
        {
            var book = _books.First(x => x.Id == id);
            _books.Remove(book);
        }

        public BookEntity Get(int id)
        {
            return _books.FirstOrDefault(x => x.Id == id);
        }

        public IList<BookEntity> Get()
        {
            return new ReadOnlyCollection<BookEntity>(_books); 
        }

        public void Update(BookEntity book)
        {
            var entity = _books.First(x => x.Id == book.Id);
            _books.Remove(entity);
            _books.Add(book);
        }
    }
}
