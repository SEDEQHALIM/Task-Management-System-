

using TaskManegmentProject.DBcontcion;
namespace TaskManegmentProject.Repos
{
    public class Repository<T> : IRepository<T> where T:class
    {
        private  TMContextDB  _context { get; set; }
        public Repository(TMContextDB DB)
        {
            _context = DB;
        }
        //public  void Create(T entity)
        //{
        //    _context.Set<T>().Add(entity);
        //}

        //public void Delete(string id)
        //{
        //    T? entity = GetById(id);
        //    if(entity != null)
        //        _context.Set<T>().Remove(entity);
        //}
        //public void Delete(Guid id)
        //{
        //    T? entity = GetById(id);
        //    if (entity != null)
        //        _context.Set<T>().Remove(entity);
        //}

        //public IQueryable<T> GetAll()
        //{
        //    return _context.Set<T>().AsQueryable();
        //}

        //public T? GetById(string id)
        //{
        //     return _context.Set<T>().Find(id);
        //}
        //public T? GetById(Guid id)
        //{
        //    return _context.Set<T>().Find(id);
        //}

        //public void Save()
        //{
        //    _context.SaveChanges();
        //}

        //public void Update(T entity)
        //{
        //    _context.Set<T>().Update(entity);
        //}

        public async Task CreateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }

            await _context.Set<T>().AddAsync(entity);
            //await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
            }
        }
        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
            }
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }

            _context.Set<T>().Update(entity);
            //await _context.SaveChangesAsync();
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsQueryable();
        }
    }
}
