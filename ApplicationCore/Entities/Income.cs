using System;
namespace ApplicationCore.Entities
{
    public class Income
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime? IncomeDate { get; set; }
        public string? Remarks { get; set; }

        public User User { get; set; }
    }
}
