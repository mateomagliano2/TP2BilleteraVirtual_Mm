using BilleteraVirtualAPI_Mm.Data;
using BilleteraVirtualAPI_Mm.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BilleteraVirtualAPI_Mm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesBVController : Controller
    {
            private readonly AppDbContext _context;

            public ClientesBVController(AppDbContext context)
            {
                _context = context;
            }

            // POST: api/clientes
            [HttpPost]
            public async Task<IActionResult> CrearCliente(ClienteBV cliente)
            {
                if (string.IsNullOrEmpty(cliente.name))
                    return BadRequest("El nombre es obligatorio");

                if (string.IsNullOrEmpty(cliente.email) || !cliente.email.Contains("@"))
                    return BadRequest("Email inválido");

                _context.ClienteBVs.Add(cliente);
                await _context.SaveChangesAsync();

                return Ok(cliente);
            }

            // GET: api/clientesbv
            [HttpGet]
            public async Task<ActionResult<IEnumerable<ClienteBV>>> GetClientes()
            {
                return await _context.ClienteBVs.ToListAsync();
            }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.ClienteBVs.FindAsync(id);

            if (cliente == null)
                return NotFound();

            _context.ClienteBVs.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, ClienteBV clienteActualizado)
        {
            var cliente = await _context.ClienteBVs.FindAsync(id);

            if (cliente == null)
                return NotFound();

            cliente.name = clienteActualizado.name;
            cliente.email = clienteActualizado.email;

            await _context.SaveChangesAsync();

            return Ok(cliente);
        }
    }
    }

