namespace ECommerceApp.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        void Update(T entity);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}
