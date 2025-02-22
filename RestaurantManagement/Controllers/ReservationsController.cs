using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using RestaurantManagement.Models;
using Microsoft.EntityFrameworkCore;
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly RestaurantManagementContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ReservationsController(RestaurantManagementContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserReservations(string userId)
    {
        Console.WriteLine($"Fetching reservations for UserID: {userId}");

        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest("User ID is required.");
        }

        var reservations = await _context.Reservations
            .Where(r => r.UserId == userId)
            .ToListAsync();

        return Ok(reservations);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReservation([FromBody] ReservationInput model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null)
        {
            Console.WriteLine($"Error: User ID {model.UserId} does not exist in IdentityUser table.");
            return BadRequest("Invalid User ID. No matching user found.");
        }

        Console.WriteLine($"Received CreateReservation request from UserID: {model.UserId}");

        
        var reservation = new Reservation
        {
            UserId = model.UserId,
            Date = model.Date,
            NumberOfPeople = model.NumberOfPeople,
            Status = "Pending"
        };

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();

        Console.WriteLine("Reservation created successfully.");
        return CreatedAtAction(nameof(GetUserReservations), new { id = reservation.Id }, reservation);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelReservation(int id, [FromBody] string UserId)
    {
        if (string.IsNullOrEmpty(UserId))
        {
            return Unauthorized("User ID is required.");
        }

        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation == null) return NotFound();

        if (reservation.UserId != UserId)
        {
            return Forbid();
        }

        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}