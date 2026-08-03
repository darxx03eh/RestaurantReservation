namespace RestaurantReservation.Infrastructure.IRepositories.Generic;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<ICollection<TEntity>> GetAllAsync();
    Task<TEntity> GetByIdAsync(int id);
    Task AddAsync(TEntity entity);
    Task AddRangeAsync(ICollection<TEntity> entities);
    Task UpdateAsync(TEntity entity);
    Task UpdateRangeAsync(ICollection<TEntity> entities);
    Task DeleteAsync(TEntity entity);
    Task DeleteRangeAsync(ICollection<TEntity> entities);
}