/*
    baseRepository.cs
    Provide CRUD operations (Generic simple queries applying to all entities) facing the database for any entity.
    getbyID, getall, add, update, delete, savechanges
    Can be used by anytype of entity (borrower, loan, payment) by inheriting from this base repository.
*/

using LoanDash.Domain.Entities;
using LoanDash.Domain.Enums;
using LoanDash.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LoanDash.Infrastructure.Repositories;

public class BaseRepository<T> where T : class
{
    protected readonly LoanDashDbContext _context;
    protected readonly DbSet<T> _dbSet;
    public BaseRepository(LoanDashDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }
    
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }
    // SaveChanges method seperate
    public async Task saveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}

    
     