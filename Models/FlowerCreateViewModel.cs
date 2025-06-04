using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace demo2.ViewModels
{
    public class FlowerCreateViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        public IFormFile ImageFile { get; set; }

   
        [Required(ErrorMessage = "Vui lòng chọn loại hoa")]
        public int CategoryId { get; set; }

    }
}
