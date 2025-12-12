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
    public class CarsController : ControllerBase
    {
        private readonly AppDBContext _context;

        public CarsController(AppDBContext context)
        {
            _context = context;
        }

        private bool IsValidYear(int year)
        {
            int currentYear = DateTime.Now.Year;
            return year >= currentYear - 5;
        }

        private CarDto ToDto(Car car) =>
            new CarDto
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Price = car.Price,
                Year = car.Year,
                Color = car.Color
            };
           
        private Car ToEntity(CarDto carDto) =>
            new Car
            {
                Id = carDto.Id,
                Brand = carDto.Brand,
                Model = carDto.Model,
                Price = carDto.Price,
                Year = carDto.Year,
                Color = carDto.Color
            };

        [HttpPost]
        public async Task<ActionResult<CarDto>> CreateCar([FromBody] CarDto carDto)
        {
            //crear nuevo coche
            if (!IsValidYear(carDto.Year))
            
                return BadRequest("The car cannot be older than 5 years.");
            

            var car = ToEntity(carDto);
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            carDto.Id = car.Id;
            return CreatedAtAction(nameof(GetCar), new { id = car.Id }, carDto);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CarDto>> GetCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return ToDto(car);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarDto>>> GetCars()
        {
            var cars = await _context.Cars.ToListAsync();
            return cars.Select(ToDto).ToList();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(int id, CarDto carDto)
        {
            if (id != carDto.Id)
            {
                return BadRequest();
            }

            if (!IsValidYear(carDto.Year))
            {
                return BadRequest("The car cannot be older than 5 years.");
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            car.Brand = carDto.Brand;
            car.Model = carDto.Model;
            car.Price = carDto.Price;
            car.Year = carDto.Year;
            car.Color = carDto.Color;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}