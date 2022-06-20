using System.Collections.Generic;

public interface IRepository<T> where T : BaseModel
{
    List<T> GetAll();
    T GetById(int id);
    void Create(T item);
    void Update(T item);
    void Delete(int id);
    void SaveChanges();
}
