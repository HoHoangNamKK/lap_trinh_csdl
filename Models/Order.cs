using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace demo2.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public Guid UserId { get; set; }
        // Navigation property
        public User User { get; set; }

        public DateTime OrderDate { get; set; }

        // Navigation property
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}