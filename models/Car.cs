using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Api.models
{
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Price { get; set; } //precio por dia
        public int Year { get; set; }
        public string Color { get; set; } = string.Empty;
        public ICollection<Rental> Rentals { get; set; }
    }
}