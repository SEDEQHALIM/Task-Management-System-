namespace TaskManegmentProject.Repos
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        T? GetById(string id);
        T? GetById(Guid id);

        void Create(T entity);
        void Update(T entity);
        void Delete(string id);
        void Delete(Guid id);


        void Save();

    }
}
