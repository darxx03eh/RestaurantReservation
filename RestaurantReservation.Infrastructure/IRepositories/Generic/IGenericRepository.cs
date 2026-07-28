namespace RestaurantReservation.Infrastructure.IRepositories.Generic;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<ICollection<TEntity>> GetAllAsync();
    Task<TEntity> GetByIdAsync(int id);
    Task<TEntity> AddAsync(TEntity entity);
    Task AddRangeAsync(ICollection<TEntity> entities);
    public Task<int> UpdateAsync(TEntity entity);
    public Task UpdateRangeAsync(ICollection<TEntity> entities);
    Task<int> DeleteAsync(TEntity entity);
    Task DeleteRangeAsync(ICollection<TEntity> entities);
}