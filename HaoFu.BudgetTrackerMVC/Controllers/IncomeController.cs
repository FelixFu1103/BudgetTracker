using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models.Request;
using ApplicationCore.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HaoFu.BudgetTrackerMVC.Controllers
{
    public class IncomeController : Controller
    {
        private readonly IIncomeService _incomeService;

        public IncomeController(IIncomeService incomeService)
        {
            _incomeService = incomeService;
        }

        [HttpGet]
        public async Task<IActionResult> EditIncome(int id)
        {
            if (id == null)
            {
                return NotFound("No User");
            }
            var incomes = await _incomeService.GetIncomesByUserId(id);
            return View(incomes);
        }
        //Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (id == null)
            {
                return NotFound("No Id");
            }
            var income = await _incomeService.GetIncomeById(id);
            if (income == null)
            {
                return NotFound("No Income");
            }

            var incomeToUpdate = new IncomeUpdateRequestModel
            {
                Id = income.Id,
                Amount = income.Amount,
                Description = income.Description,
                IncomeDate = income.IncomeOrExpDate.GetValueOrDefault(),
                Remarks = income.Remarks,
            };
            return View(incomeToUpdate);
        }

        [HttpPost, ActionName("Update")]
        public async Task<IActionResult> UpdateConfirm(IncomeUpdateRequestModel model)
        {
            var updateincome = await _incomeService.UpdateIncomeRecord(model);
            return LocalRedirect("~/");
        }
        //Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var income = await _incomeService.GetIncomeById(id);
            if (income == null)
            {
                return NotFound();
            }
            return View(income);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var movie = await _incomeService.DeleteIncomeRecord(id);
            return LocalRedirect("~/");
        }

    }
}
