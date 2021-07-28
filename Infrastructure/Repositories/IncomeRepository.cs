using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.RepositoryInterfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class IncomeRepository : EfRepository<Income>, IIncomeRepository
    {
        public IncomeRepository(BudgetTrackerDbContext dbContext) : base(dbContext)
        {
        }


        public async override Task<Income> GetByIdAsync(int id)
        {
            var income = await _dbContext.Incomes.FirstOrDefaultAsync(i => i.Id == id);
            return income;
        }
        public async override Task<Income> UpdateAsync(Income entity)
        {
            var update = await _dbContext.Incomes.FirstOrDefaultAsync(i => i.Id == entity.Id);
            update.Amount = entity.Amount;
            update.Description = entity.Description;
            update.IncomeDate = entity.IncomeDate;
            update.Remarks = entity.Remarks;
            await _dbContext.SaveChangesAsync();
            return update;
        }

        public async override Task<IEnumerable<Income>> ListAsync(Expression<Func<Income, bool>> filter)
        {
            var incomes = await _dbContext.Incomes.Where(filter).ToListAsync();
            return incomes;
        }
    }
}
