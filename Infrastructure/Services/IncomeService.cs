using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Models;
using ApplicationCore.Models.Request;
using ApplicationCore.Models.Response;
using ApplicationCore.RepositoryInterfaces;
using ApplicationCore.ServiceInterfaces;

namespace Infrastructure.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly IIncomeRepository _incomeRepository;
        private readonly ICurrentUser _currentUser;
        public IncomeService(IIncomeRepository incomeRepository, ICurrentUser currentUser)
        {
            _incomeRepository = incomeRepository;
            _currentUser = currentUser;
        }

        public async Task<UserBudgetResponseModel> AddIncomeRecord(IncomeRequestModel model)
        {
            var income = new Income
            {
                UserId = model.UserId,
                Amount = model.Amount,
                Description = model.Description,
                IncomeDate = model.IncomeDate,
                Remarks = model.Remarks
            };
            var addIncome = await _incomeRepository.AddAsync(income);
            var incomeResponse = new UserBudgetResponseModel
            {
                Amount = addIncome.Amount,
                Description = addIncome.Description,
                IncomeOrExpDate = addIncome.IncomeDate.GetValueOrDefault(),
                Remarks = addIncome.Remarks
            };
            return incomeResponse;
        }

        public async Task<UserBudgetResponseModel> DeleteIncomeRecord(int id)
        {
            var income = await _incomeRepository.GetByIdAsync(id);
            var delete = await _incomeRepository.DeleteAsync(income);
            var deleteModel = new UserBudgetResponseModel
            {
                Amount = delete.Amount,
                Description = delete.Description,
                IncomeOrExpDate = delete.IncomeDate,
                Remarks = delete.Remarks
            };
            return deleteModel;
        }

        public Task<UserBudgetResponseModel> GetAllIncomeRecords()
        {
            throw new NotImplementedException();
        }

        public async Task<UserBudgetResponseModel> GetIncomeById(int id)
        {
            var income = await _incomeRepository.GetByIdAsync(id);
            var incomeModel = new UserBudgetResponseModel
            {
                Id = income.Id,
                Amount = income.Amount,
                Description = income.Description,
                IncomeOrExpDate = income.IncomeDate,
                Remarks = income.Remarks,
            };
            return incomeModel;
        }

        public async Task<List<UserBudgetResponseModel>> GetIncomesByUserId(int id)
        {
            var incomes = await _incomeRepository.ListAsync(i => i.UserId == id);
            var incomesModle = new List<UserBudgetResponseModel>();
            foreach (var income in incomes)
            {
                incomesModle.Add(new UserBudgetResponseModel
                {
                    Id = income.Id,
                    Amount = income.Amount,
                    Description = income.Description,
                    IncomeOrExpDate = income.IncomeDate,
                    Remarks = income.Remarks
                });
            }
            return incomesModle;
        }

        public async Task<UserBudgetResponseModel> UpdateIncomeRecord(IncomeUpdateRequestModel model)
        {
            var update = new Income
            {
                Id = model.Id,
                UserId = model.UserId,
                Amount = model.Amount,
                Description = model.Description,
                IncomeDate = model.IncomeDate,
                Remarks = model.Remarks
            };
            var addIncome = await _incomeRepository.UpdateAsync(update);
            var incomeResponse = new UserBudgetResponseModel
            {
                Amount = addIncome.Amount,
                Description = addIncome.Description,
                IncomeOrExpDate = addIncome.IncomeDate.GetValueOrDefault(),
                Remarks = addIncome.Remarks
            };
            return incomeResponse;
        }
    }
}
