using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Api.Dto;
using CarRental.Api.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalController : ControllerBase
    {
        private readonly AppDBContext _context;

        public RentalController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> CreateRental([FromBody] RentalDto rentalDto)
        {
            if (rentalDto.StartDate >= rentalDto.EndDate)
            {
                return BadRequest("La fecha de inicio debe ser anterior a la fecha de fin.");
            }

            var car = await _context.Cars.FindAsync(rentalDto.CarId);
            if (car == null)
            {
                return NotFound("Coche no encontrado.");
            }

            bool isCarAviailable = !await _context.Rentals.AnyAsync(r =>
                r.CarId == rentalDto.CarId &&
                (
                    (rentalDto.StartDate >= r.StartDate && rentalDto.StartDate < r.EndDate) ||
                    (rentalDto.EndDate > r.StartDate && rentalDto.EndDate <= r.EndDate) ||
                    (rentalDto.StartDate <= r.StartDate && rentalDto.EndDate >= r.EndDate)
            ));

            if (!isCarAviailable)
            {
                return BadRequest("El coche no está disponible en las fechas seleccionadas.");
            }

            var totalDays = (rentalDto.EndDate - rentalDto.StartDate).Days;
            if (totalDays <= 0)
            {
                return BadRequest("El período de alquiler debe ser de al menos un día.");
            }

            var totalPrice = totalDays * car.Price;
            var rental = new Rental
            {
                UserId = rentalDto.UserId,
                CarId = rentalDto.CarId,
                StartDate = rentalDto.StartDate,
                EndDate = rentalDto.EndDate,
                TotalPrice = totalPrice
            };

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            return Ok(new { RentalId = rental.Id, TotalPrice = totalPrice });
        }
        
    }
}