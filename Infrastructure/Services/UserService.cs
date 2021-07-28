using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ApplicationCore.Exceptions;
using ApplicationCore.Entities;
using ApplicationCore.RepositoryInterfaces;
using ApplicationCore.ServiceInterfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using ApplicationCore.Models.Response;
using ApplicationCore.Models.Request;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDetailResponseModel> GetUserById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            var usermodel = new UserDetailResponseModel
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                JoinedOn = user.JoinedOn.GetValueOrDefault()
            };
            return usermodel;
        }

        public async Task<List<UserCardResponseModel>> GetAllUsers()
        {
            var users = await _userRepository.ListAllAsync();
            var userCards = new List<UserCardResponseModel>();
            foreach (var user in users)
            {
                userCards.Add(new UserCardResponseModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Income = user.TotalIncomes,
                    Expenditure = user.TotalExpends,
                    Email = user.Email,
                    JoinedOn = user.JoinedOn
                });
            }
            return userCards;
        }    

        public async Task<UserCardResponseModel> DeleteUser(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            var delete = await _userRepository.DeleteAsync(user);
            var deleteModel = new UserCardResponseModel()
            {
                Id = delete.Id,
                FullName = delete.FullName
            };
            return deleteModel;
        }

        public async Task<UserCardResponseModel> UpdateUser(UserUpdateRequestModel model)
        {
            var salt = CreateSalt();
            var hashedPassword = HashPassword(model.Password, salt);
            var user = new User
            {
                Id = model.Id,
                Email = model.Email,
                FullName = model.FullName,
                HashedPassword = hashedPassword
            };
            var update = await _userRepository.UpdateAsync(user);
            var userModel = new UserCardResponseModel
            {
                Id = update.Id,
                FullName = update.FullName
            };
            return userModel;
        }

        public async Task<UserDetailResponseModel> GetUserDetails(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            var totalcash = (user.TotalIncomes) - (user.TotalExpends);
            var userdetail = new UserDetailResponseModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                JoinedOn = user.JoinedOn.GetValueOrDefault(),
                TotalCash = (decimal)totalcash
            };
            userdetail.Incomes = new List<UserBudgetResponseModel>();
            userdetail.Expands = new List<UserBudgetResponseModel>();
            foreach (var income in user.Incomes)
            {
                userdetail.Incomes.Add(new UserBudgetResponseModel
                {
                    Id = income.Id,
                    Amount = income.Amount,
                    Description = income.Description,
                    IncomeOrExpDate = income.IncomeDate.GetValueOrDefault(),
                    Remarks = income.Remarks,
                });
            }
            foreach (var expend in user.Expenditures)
            {
                userdetail.Expands.Add(new UserBudgetResponseModel
                {
                    Amount = expend.Amount,
                    Description = expend.Description,
                    IncomeOrExpDate = expend.ExpDate.GetValueOrDefault(),
                    Remarks = expend.Remarks,
                });
            }
            return userdetail;
        }

        public Task<List<UserBudgetResponseModel>> GetUserExpenditure(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserBudgetResponseModel>> GetUserIncome(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserLoginResponseModel> Login(string email, string password)
        {
            var dbUser = await _userRepository.GetUserByEmail(email);
            if (dbUser == null)
            {
                throw new NotFoundException("Email does not exists, Please register first");
            }
            var hashedPassword = HashPassword(password, dbUser.Salt);
            if (hashedPassword == dbUser.HashedPassword)
            {
                //correct password
                var userLoginResponse = new UserLoginResponseModel
                {
                    Id = dbUser.Id,
                    Email = dbUser.Email,
                    FullName = dbUser.FullName,
                };
                return userLoginResponse;
            }
            return null;
        }

        public async Task<UserRegisterResponseModel> RegisterUser(UserRegisterRequestModel requestModel)
        {
            //make sure email dose not exist in database(User table)
            var dbUser = await _userRepository.GetUserByEmail(requestModel.Email);
            if (dbUser != null)
            {
                //we already have user with same email
                throw new ConflictException("Email Already Exists");
            }
            //create a unique salt, using Microsoft.AspNetCore.Cryptography.KeyDerivation;
            var salt = CreateSalt();
            var hashedPassword = HashPassword(requestModel.Password, salt);
            //save to db
            var user = new User
            {
                Email = requestModel.Email,
                Salt = salt,
                FullName = requestModel.FullName,
                HashedPassword = hashedPassword,
                JoinedOn = DateTime.Now,
            };
      
            var createUser = await _userRepository.AddAsync(user);
            var userResponse = new UserRegisterResponseModel
            {
                Id = createUser.Id,
                Email = createUser.Email,
                FullName = createUser.FullName
            };
            return userResponse;
        }
        private string CreateSalt()
        {
            byte[] randomBytes = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return Convert.ToBase64String(randomBytes);
        }
        private string HashPassword(string password, string salt)
        {
           
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                                                    password: password,
                                                                    salt: Convert.FromBase64String(salt),
                                                                    prf: KeyDerivationPrf.HMACSHA512,
                                                                    iterationCount: 10000,
                                                                    numBytesRequested: 256 / 8));
            return hashed;
        }


    }
}
