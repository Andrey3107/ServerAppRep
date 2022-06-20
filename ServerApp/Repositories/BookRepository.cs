using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class BookRepository<T> : IRepository<T> where T : Book
{
    private readonly string Path;
    private List<T> _books;
    public BookRepository(string path)
    {
        Path = path;
        _books = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(path));
    }
    public List<T> GetAll()
    {
        NoContentCheck();
        return _books;
    }
    public T GetById(int id)
    {
        NoContentCheck();
        var book = _books.FirstOrDefault(x => x.Id == id);
        if (book != null)
        {
            return book;
        }
        else
        {
            throw new HttpException("Couldn not find the book!", 404);
        }
    }
    public void Create(T book)
    {
        if(_books == null)
        {
            _books = new List<T>();
        }
        book.Id = _books.Any() ? _books.Max(x => x.Id) + 1 : 1;
        _books.Add(book);
        SaveChanges();
    }
    public void Update(T book)
    {
        T bookForUpdate = GetById(book.Id);
        _books[_books.IndexOf(bookForUpdate)] = book;
        SaveChanges();
    }
    public void Delete(int id)
    {
        T bookForDelete = GetById(id);
        _books.Remove(bookForDelete);
        SaveChanges();
    }
    public void SaveChanges()
    {
        var booksForSave = JsonConvert.SerializeObject(_books);
        File.WriteAllText(Path, booksForSave);
    }
    public void NoContentCheck()
    {
        if (_books == null)
        {
            throw new HttpException("File is empty!", 204);
        }
        if (!_books.Any())
        {
            throw new HttpException("File is empty!", 204);
        }
    }
}
