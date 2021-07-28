using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.RepositoryInterfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repositories
{
    public class UserRepository : EfRepository<User>, IUserRepository
    {
        public UserRepository(BudgetTrackerDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public override async Task<User> GetByIdAsync(int id)
        {
            var user = await _dbContext.Users.Include(u => u.Incomes).Include(u => u.Expenditures).FirstOrDefaultAsync(u => u.Id == id);
            var incomes = await _dbContext.Incomes.Where(i => i.UserId == user.Id).SumAsync(i => i == null ? 0 : i.Amount);
            if (incomes >= 0)
            {
                user.TotalIncomes = incomes;
            }
            var expends = await _dbContext.Expenditures.Where(e => e.UserId == user.Id).SumAsync(e => e == null ? 0 : e.Amount);
            if (expends <= 0)
            {
                user.TotalExpends = expends;
            }
            return user;
        }

        public async override Task<IEnumerable<User>> ListAllAsync()
        {
            var users = await _dbContext.Users.Include(u => u.Incomes).Include(u => u.Expenditures).ToListAsync();
            foreach (var user in users)
            {
                var incomes = await _dbContext.Incomes.Where(i => i.UserId == user.Id).SumAsync(i => i == null ? 0 : i.Amount);
                if (incomes >= 0)
                {
                    user.TotalIncomes = incomes;
                }
                var expends = await _dbContext.Expenditures.Where(e => e.UserId == user.Id).SumAsync(e => e == null ? 0 : e.Amount);
                if (expends <= 0)
                {
                    user.TotalExpends = expends;
                }
            }
            return users;
        }

        public async override Task<User> UpdateAsync(User entity)
        {
            var update = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == entity.Id);
            update.Email = entity.Email;
            update.FullName = entity.FullName;
            update.JoinedOn = entity.JoinedOn;
            await _dbContext.SaveChangesAsync();
            return update;
        }
    }
}
