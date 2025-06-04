using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace demo2.Models
{
    public class Flower
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public int CategoryId { get; set; } // ID của Loại hoa
        public Category Category { get; set; } // Điều hướng qua bảng Category

    }

}