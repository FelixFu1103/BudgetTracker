using System;
namespace ApplicationCore.Models.Response
{
    public class UserCardResponseModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public decimal Income { get; set; }
        public decimal Expenditure { get; set; }
        public string Email { get; set; }
        public DateTime? JoinedOn { get; set; }
    }
}
