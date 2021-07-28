using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Models.Request;
using ApplicationCore.Models.Response;

namespace ApplicationCore.ServiceInterfaces
{
    public interface IExpenditureService
    {
        Task<UserBudgetResponseModel> AddExpendRecord(ExpendRequestModel model);
        Task<UserBudgetResponseModel> UpdateExpendRecord(ExpendUpdateRequestModel model);
        Task<UserBudgetResponseModel> DeleteExpendRecord(int id);
        Task<List<UserBudgetResponseModel>> GetExpendsByUserId(int id);
        Task<UserBudgetResponseModel> GetExpendById(int id);
    }
}
