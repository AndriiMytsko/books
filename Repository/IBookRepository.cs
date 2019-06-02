using System.Collections.Generic;

namespace Repository
{
    public interface IBookRepository
    {
        int Add(BookEntity book);
        void Delete(int id);
        void Update(BookEntity book);
        BookEntity Get(int id);
        IList<BookEntity> Get();
    }
}
