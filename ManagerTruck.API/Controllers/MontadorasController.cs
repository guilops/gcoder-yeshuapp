using ManagerTruck.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagerTruck.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MontadorasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MontadorasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("/montadoras")]
        public async Task<IActionResult> Get()
        {
            var montadoras = await _context.Montadoras.ToListAsync();

            if (!montadoras.Any())
                return NotFound("Nenhuma montadora encontrada.");

            return Ok(montadoras);
        }
    }
}
