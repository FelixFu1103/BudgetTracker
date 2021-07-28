using System;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Models.Request
{
    public class ExpendRequestModel
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        [Range(Double.MinValue, 0.0, ErrorMessage = "Please enter Amount greater than 0")]
        public decimal Amount { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime ExpDate { get; set; }
        public string Remarks { get; set; }
    }
}
