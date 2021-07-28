using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Models.Request;
using ApplicationCore.Models.Response;

namespace ApplicationCore.ServiceInterfaces
{
    public interface IUserService
    {
        Task<UserRegisterResponseModel> RegisterUser(UserRegisterRequestModel requestModel);
        Task<UserLoginResponseModel> Login(string email, string password);
        Task<List<UserBudgetResponseModel>> GetUserIncome(int id);
        Task<List<UserBudgetResponseModel>> GetUserExpenditure(int id);
        Task<UserDetailResponseModel> GetUserDetails(int id);
        Task<UserDetailResponseModel> GetUserById(int id);
        Task<List<UserCardResponseModel>> GetAllUsers();
        Task<UserCardResponseModel> DeleteUser(int id);
        Task<UserCardResponseModel> UpdateUser(UserUpdateRequestModel model);

    }
}
