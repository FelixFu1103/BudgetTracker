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
    public class ExpenditureRepository : EfRepository<Expenditure>, IExpenditureRepository
    {
        public ExpenditureRepository(BudgetTrackerDbContext dbContext) : base(dbContext)
        {
        }
        public async override Task<Expenditure> GetByIdAsync(int id)
        {
            var expenditure = await _dbContext.Expenditures.FirstOrDefaultAsync(i => i.Id == id);
            return expenditure;
        }
        public async override Task<Expenditure> UpdateAsync(Expenditure entity)
        {
            var update = await _dbContext.Expenditures.FirstOrDefaultAsync(i => i.Id == entity.Id);
            update.Amount = entity.Amount;
            update.Description = entity.Description;
            update.ExpDate = entity.ExpDate;
            update.Remarks = entity.Remarks;
            await _dbContext.SaveChangesAsync();
            return update;
        }
        public async override Task<IEnumerable<Expenditure>> ListAsync(Expression<Func<Expenditure, bool>> filter)
        {
            var expends = await _dbContext.Expenditures.Where(filter).ToListAsync();
            return expends;
        }
    }
}
