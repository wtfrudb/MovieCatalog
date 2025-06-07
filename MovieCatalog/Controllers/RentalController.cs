using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.Data;
using MovieCatalog.Models;
using System.Security.Claims;

namespace MovieCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RentalController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RentalController> _logger;

        public RentalController(ApplicationDbContext context, ILogger<RentalController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public class RentalItemDto
        {
            public int MovieId { get; set; }
            public int Quantity { get; set; }
        }

        // DTO для запроса создания заказа
        public class RentalOrderRequest
        {
            public List<RentalItemDto> Items { get; set; } = new List<RentalItemDto>();
        }


        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] RentalOrderRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return Unauthorized();

            var order = new RentalOrder
            {
                UserId = int.Parse(userId),
                RentalDate = DateTime.UtcNow,
                Items = request.Items.Select(item => new RentalItem
                {
                    MovieId = item.MovieId,
                    Quantity = item.Quantity
                }).ToList()
            };

            _context.RentalOrders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(order);
        }


        // GET: api/rental/my
        [HttpGet("my")]
        public async Task<IActionResult> GetUserOrders()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized("Не удалось определить пользователя");
                }

                var orders = await _context.RentalOrders
                    .Where(o => o.UserId == userId)
                    .Include(o => o.Items)
                        .ThenInclude(i => i.Movie)
                    .AsNoTracking()
                    .ToListAsync();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении заказов пользователя");
                return StatusCode(500, "Внутренняя ошибка при получении заказов");
            }
        }
    }
}
