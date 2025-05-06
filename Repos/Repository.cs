
using TaskManegmentProject.Repositiory;
using TaskManegmentProject.Models;
namespace TaskManegmentProject.Repos
{
    public class Repository<T> : IRepository<T> where T:class
    {
        private  TMContextDB  _context { get; set; }
        public Repository(TMContextDB DB)
        {
            _context = DB;
        }
        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete(string id)
        {
            T? entity = GetById(id);
            if(entity != null)
                _context.Set<T>().Remove(entity);
        }
        public void Delete(Guid id)
        {
            T? entity = GetById(id);
            if (entity != null)
                _context.Set<T>().Remove(entity);
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsQueryable();
        }

        public T? GetById(string id)
        {
             return _context.Set<T>().Find(id);
        }
        public T? GetById(Guid id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
