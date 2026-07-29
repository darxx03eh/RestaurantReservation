using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.IRepositories;
using RestaurantReservation.Infrastructure.IRepositories.Generic;

namespace RestaurantReservation.Infrastructure.Repositories.Generic;

public class GenericRepository<TEntity> (RestaurantReservationDbContext context)
    : IGenericRepository<TEntity> where TEntity : class
{
    private readonly RestaurantReservationDbContext _context = context;

    public virtual async Task<ICollection<TEntity>> GetAllAsync()
        => await _context.Set<TEntity>().ToListAsync();

    public virtual async Task<TEntity> GetByIdAsync(int id)
        => await _context.Set<TEntity>().FindAsync(id);

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task AddRangeAsync(ICollection<TEntity> entities)
        => await _context.BulkInsertAsync(entities);

    public virtual async Task<int> UpdateAsync(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        return await _context.SaveChangesAsync();
    }

    public virtual async Task UpdateRangeAsync(ICollection<TEntity> entities)
        => await _context.BulkInsertOrUpdateAsync(entities);

    public virtual async Task<int> DeleteAsync(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        return await context.SaveChangesAsync();
    }

    public virtual async Task DeleteRangeAsync(ICollection<TEntity> entities)
        => await _context.BulkDeleteAsync(entities);
}