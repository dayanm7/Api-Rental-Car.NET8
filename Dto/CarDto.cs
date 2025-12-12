using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace CarRental.Api.Dto
{
    public class CarDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Brand { get; set; } = string.Empty;
        [Required]
        public string Model { get; set; } = string.Empty;
        [Required]
        public int Price { get; set; } //precio por dia
        [Required]
        public int Year { get; set; }
        [Required]
        public string Color { get; set; }  = string.Empty;

    }
}