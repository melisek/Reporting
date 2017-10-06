using System.Collections.Generic;

namespace szakdoga.Models
{
    public interface IBaseRepositoryInterface<T>
    {
        void Add(T entity);

        T Get(int id);

        IEnumerable<T> GetAll();

        bool Remove(int id);

        void Update(T entity);
    }
}