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
    public class ExpenditureService : IExpenditureService
    {
        private readonly IExpenditureRepository _expenditureRepository;
        private readonly ICurrentUser _currentUser;
        public ExpenditureService(IExpenditureRepository expenditureRepository, ICurrentUser currentUser)
        {
            _expenditureRepository = expenditureRepository;
            _currentUser = currentUser;
        }

        public async Task<UserBudgetResponseModel> AddExpendRecord(ExpendRequestModel model)
        {
            var expend = new Expenditure
            {
                UserId = model.UserId,
                Amount = model.Amount,
                Description = model.Description,
                ExpDate = model.ExpDate,
                Remarks = model.Remarks
            };
            var addExpend = await _expenditureRepository.AddAsync(expend);
            var expResponse = new UserBudgetResponseModel
            {
                Amount = addExpend.Amount,
                Description = addExpend.Description,
                IncomeOrExpDate = addExpend.ExpDate.GetValueOrDefault(),
                Remarks = addExpend.Remarks
            };
            return expResponse;
        }

        public async Task<UserBudgetResponseModel> DeleteExpendRecord(int id)
        {
            var expenditure = await _expenditureRepository.GetByIdAsync(id);
            var delete = await _expenditureRepository.DeleteAsync(expenditure);
            var deleteModel = new UserBudgetResponseModel
            {
                Amount = delete.Amount,
                Description = delete.Description,
                IncomeOrExpDate = delete.ExpDate,
                Remarks = delete.Remarks
            };
            return deleteModel;
        }

        public async Task<List<UserBudgetResponseModel>> GetExpendsByUserId(int id)
        {
            var expends = await _expenditureRepository.ListAsync(i => i.UserId == id);
            var expendsModle = new List<UserBudgetResponseModel>();
            foreach (var expend in expends)
            {
                expendsModle.Add(new UserBudgetResponseModel
                {
                    Id = expend.Id,
                    Amount = expend.Amount,
                    Description = expend.Description,
                    IncomeOrExpDate = expend.ExpDate,
                    Remarks = expend.Remarks
                });
            }
            return expendsModle;
        }

        public async Task<UserBudgetResponseModel> GetExpendById(int id)
        {
            var expenditure = await _expenditureRepository.GetByIdAsync(id);
            var expendModel = new UserBudgetResponseModel
            {
                Id = expenditure.Id,
                Amount = expenditure.Amount,
                Description = expenditure.Description,
                IncomeOrExpDate = expenditure.ExpDate,
                Remarks = expenditure.Remarks,
            };
            return expendModel;
        }

        public async Task<UserBudgetResponseModel> UpdateExpendRecord(ExpendUpdateRequestModel model)
        {
            var expend = new Expenditure
            {
                Id = model.Id,
                UserId = model.UserId,
                Amount = model.Amount,
                Description = model.Description,
                ExpDate = model.ExpDate,
                Remarks = model.Remarks
            };
            var update = await _expenditureRepository.UpdateAsync(expend);
            var updateResponse = new UserBudgetResponseModel
            {
                Amount = update.Amount,
                Description = update.Description,
                IncomeOrExpDate = update.ExpDate.GetValueOrDefault(),
                Remarks = update.Remarks
            };
            return updateResponse;
        }

    }
}
