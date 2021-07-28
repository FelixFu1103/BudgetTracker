using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public string? FullName { get; set; }
        public DateTime? JoinedOn { get; set; }

        public decimal TotalIncomes { get; set; }
        public decimal TotalExpends { get; set; }

        public ICollection<Income> Incomes { get; set; }
        public ICollection<Expenditure> Expenditures { get; set; }
    }
}
