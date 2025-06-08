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

        public class RentalOrderRequest
        {
            public List<RentalItemDto> Items { get; set; } = new List<RentalItemDto>();
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] RentRequest request)
        {
            int userId = GetUserIdFromToken();

            var order = new RentalOrder
            {
                UserId = userId,
                RentalDate = DateTime.Now, // или DateTime.Now, если нужен локальный
                Items = request.Items.Select(item => new RentalItem
                {
                    MovieId = item.MovieId,
                    Quantity = item.Quantity,
                    ReturnDate = item.ReturnDate  // передаем дату возврата
                }).ToList()
            };

            _context.RentalOrders.Add(order);
            await _context.SaveChangesAsync();

            var orderDto = new RentalOrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                RentalDate = order.RentalDate,
                Items = order.Items.Select(i => new RentalItemResponseDto
                {
                    MovieId = i.MovieId,
                    Quantity = i.Quantity
                }).ToList()
            };

            return Ok(orderDto);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetUserOrders()
        {
            try
            {
                int userId = GetUserIdFromToken();

                var orders = await _context.RentalOrders
                    .Where(o => o.UserId == userId)
                    .Include(o => o.Items)
                        .ThenInclude(i => i.Movie)
                    .AsNoTracking()
                    .ToListAsync();

                var ordersDto = orders.Select(o => new RentalOrderDto
                {
                    Id = o.Id,
                    UserId = o.UserId,
                    RentalDate = o.RentalDate,
                    Items = o.Items.Select(i => new RentalItemResponseDto
                    {
                        MovieId = i.MovieId,
                        MovieTitle = i.Movie?.Title ?? "",
                        ReleaseYear = i.Movie?.ReleaseYear ?? 0,  // год выпуска
                        MovieDescription = i.Movie?.Description ?? "",
                        Quantity = i.Quantity,
                        ReturnDate = i.ReturnDate // дата возврата из RentalItem
                    }).ToList()

                }).ToList();

                return Ok(ordersDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении заказов пользователя");
                return StatusCode(500, new { message = "Внутренняя ошибка при получении заказов", details = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var userId = GetUserIdFromToken();
            var order = _context.RentalOrders.Include(r => r.Items).FirstOrDefault(r => r.Id == id && r.UserId == userId);
            if (order == null)
                return NotFound("Order not found");

            _context.RentalItems.RemoveRange(order.Items);
            _context.RentalOrders.Remove(order);
            _context.SaveChanges();
            return Ok();
        }

        // ✅ Добавлен недостающий метод
        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("Невалидный идентификатор пользователя");
            }
            return userId;
        }
    }
}
