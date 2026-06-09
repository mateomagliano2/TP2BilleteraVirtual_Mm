using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BilleteraVirtualAPI_Mm.Data;
using BilleteraVirtualAPI_Mm.Models;
using BilleteraVirtualAPI_Mm.DTOs;

namespace BilleteraVirtualAPI_Mm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransactionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransaccionBV>>> GetTransactions()
        {
            var transacciones = await _context.TransaccionBVs
                .OrderByDescending(t => t.datetime)
                .ToListAsync();

            return Ok(transacciones);
        }

        // GET: api/transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransaccionBV>> GetTransaction(int id)
        {
            var transaccion = await _context.TransaccionBVs.FindAsync(id);

            if (transaccion == null)
                return NotFound();

            return Ok(transaccion);
        }

        // POST: api/transactions
        [HttpPost]
        public async Task<IActionResult> PostTransaction([FromBody] TransaccionBvDTO dto)
        {
            try
            {
                decimal cantidadDecimal;
                if (!decimal.TryParse(dto.crypto_amount, System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out cantidadDecimal) || cantidadDecimal <= 0)
                {
                    return BadRequest("Cantidad invalida: " + dto.crypto_amount);
                }

                decimal precioARS = ObtenerPrecioCripto(dto.crypto_code);
                DateTime fecha = DateTime.Parse(dto.datetime);
                var transaccion = new TransaccionBV
                {
                    crypto_code = dto.crypto_code.ToLower(),
                    action = dto.action.ToLower(),
                    client_id = dto.client_id,
                    crypto_amount = dto.crypto_amount,
                    money = Math.Round(cantidadDecimal * precioARS, 2),
                    datetime = fecha
                };

                _context.TransaccionBVs.Add(transaccion);
                await _context.SaveChangesAsync();

                return Ok(transaccion);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        // PUT: api/transactions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, TransaccionBvDTO dto)
        {
            var transaccion = await _context.TransaccionBVs.FindAsync(id);

            if (transaccion == null)
                return NotFound();

            decimal cantidadDecimal;
            decimal.TryParse(dto.crypto_amount, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out cantidadDecimal);

            decimal precioARS = ObtenerPrecioCripto(dto.crypto_code);
            decimal montoTotal = cantidadDecimal * precioARS;

            transaccion.crypto_code = dto.crypto_code.ToLower();
            transaccion.action = dto.action.ToLower();
            transaccion.client_id = dto.client_id;
            transaccion.crypto_amount = dto.crypto_amount;
            transaccion.money = Math.Round(montoTotal, 2);
            DateTime fecha;
            if(!DateTime.TryParse(dto.datetime, out fecha))
            {
                return BadRequest("Fecha invalida");
            }
            transaccion.datetime = fecha;
            

            await _context.SaveChangesAsync();

            return Ok(transaccion);
        }

        // DELETE: api/transactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaccion = await _context.TransaccionBVs.FindAsync(id);

            if (transaccion == null)
                return NotFound();

            _context.TransaccionBVs.Remove(transaccion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Precios fijos de las criptomonedas
        private decimal ObtenerPrecioCripto(string cryptoCode)
        {
            var precios = new Dictionary<string, decimal>
            {
                { "bitcoin", 95000000m },
                { "ethereum", 5000000m },
                { "usdc", 1200m }
            };

            string codigo = cryptoCode.ToLower();

            if (precios.ContainsKey(codigo))
                return precios[codigo];

            return 0;
        }
    }
}