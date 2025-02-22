using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models;

namespace RestaurantManagement.Controllers
{
    public class ReservationWebController : Controller
    {
        private readonly RestaurantManagementContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public ReservationWebController(RestaurantManagementContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles ="Administrator,Chelner")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var userRoles = await _userManager.GetRolesAsync(user);

            IQueryable<Reservation> reservations = _context.Reservations;
            return View(await reservations.ToListAsync());
        }
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return NotFound();

            if (status != "Accepted" && status != "Rejected")
            {
                return BadRequest("Invalid status update.");
            }

            reservation.Status = status;
            _context.Update(reservation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
