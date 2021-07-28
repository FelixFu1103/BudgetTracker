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
    public class ExpenditureController : Controller
    {
        private readonly IExpenditureService _expenditureService;

        public ExpenditureController(IExpenditureService expenditureService)
        {
            _expenditureService = expenditureService;
        }
        [HttpGet]
        public async Task<IActionResult> EditExpenditure(int id)
        {
            if (id == null)
            {
                return NotFound("No User");
            }
            var expends = await _expenditureService.GetExpendsByUserId(id);
            return View(expends);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (id == null)
            {
                return NotFound("No Id");
            }
            var income = await _expenditureService.GetExpendById(id);
            if (income == null)
            {
                return NotFound("No Income");
            }

            var expendToUpdate = new ExpendUpdateRequestModel
            {
                Id = income.Id,
                Amount = income.Amount,
                Description = income.Description,
                ExpDate = income.IncomeOrExpDate.GetValueOrDefault(),
                Remarks = income.Remarks,
            };
            return View(expendToUpdate);
        }

        [HttpPost, ActionName("Update")]
        public async Task<IActionResult> UpdateConfirm(ExpendUpdateRequestModel model)
        {
            var updateincome = await _expenditureService.UpdateExpendRecord(model);
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

            var income = await _expenditureService.GetExpendById(id);
            if (income == null)
            {
                return NotFound();
            }
            return View(income);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var movie = await _expenditureService.DeleteExpendRecord(id);
            return LocalRedirect("~/");
        }
    }
}
