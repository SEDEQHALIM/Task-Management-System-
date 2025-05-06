namespace TaskManegmentProject.Repos
{
    public interface IRepository<T>
    {
        Task CreateAsync(T entity);
        Task DeleteAsync(string id);
        Task DeleteAsync(Guid id);
        IQueryable<T> GetAll();
        Task<T?> GetByIdAsync(string id);
        Task<T?> GetByIdAsync(Guid id);
        Task SaveAsync();
        Task UpdateAsync(T entity);

        //IQueryable<T> GetAll();
        //T? GetById(string id);
        //T? GetById(Guid id);

        //void Create(T entity);
        //void Update(T entity);
        //void Delete(string id);
        //void Delete(Guid id);


        //void Save();

    }
}
