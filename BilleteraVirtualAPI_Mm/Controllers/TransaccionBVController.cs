using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BilleteraVirtualAPI_Mm.Data;
using BilleteraVirtualAPI_Mm.DTOs;
using BilleteraVirtualAPI_Mm.Models;

namespace BilleteraVirtualAPI_Mm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;

        public TransactionsController(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
        }

        // GET: api/transactions
        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            var transacciones = await _context.TransaccionBVs
                .OrderByDescending(t => t.datetime)
                .Select(t => new
                {
                    t.id,
                    t.crypto_code,
                    t.action,
                    t.crypto_amount,
                    t.money,
                    t.datetime,
                    t.client_id,
                    client_name = _context.ClienteBVs
                        .Where(c => c.id == t.client_id)
                        .Select(c => c.name)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(transacciones);
        }

        // GET: api/transactions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction(int id)
        {
            var transaccion = await _context.TransaccionBVs
                .Where(t => t.id == id)
                .Select(t => new
                {
                    t.id,
                    t.crypto_code,
                    t.crypto_amount,
                    t.client_id,
                    client_name = _context.ClienteBVs
                        .Where(c => c.id == t.client_id)
                        .Select(c => c.name)
                        .FirstOrDefault(),
                    t.money,
                    t.action,
                    t.datetime
                })
                .FirstOrDefaultAsync();

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
                var cliente = await _context.ClienteBVs.FindAsync(dto.client_id);

                if (cliente == null)
                    return BadRequest("El cliente no existe");

                if (string.IsNullOrWhiteSpace(dto.crypto_code))
                    return BadRequest("Cripto inválida");

                if (string.IsNullOrWhiteSpace(dto.action))
                    return BadRequest("Acción inválida");

                var accion = dto.action.ToLower();

                if (accion != "purchase" && accion != "sale")
                    return BadRequest("La acción debe ser purchase o sale");

                decimal cantidadDecimal;
                if (!decimal.TryParse(
                        dto.crypto_amount,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out cantidadDecimal) || cantidadDecimal <= 0)
                {
                    return BadRequest("Cantidad inválida: " + dto.crypto_amount);
                }

                DateTime fecha;
                if (!DateTime.TryParse(dto.datetime, out fecha))
                    return BadRequest("Fecha inválida");

                // VALIDAR SALDO SOLO SI ES VENTA
                if (accion == "sale")
                {
                    decimal saldoDisponible = await ObtenerSaldoDisponible(dto.client_id, dto.crypto_code);

                    if (cantidadDecimal > saldoDisponible)
                    {
                        return BadRequest($"No se puede vender esa cantidad. Saldo disponible: {saldoDisponible}");
                    }
                }

                decimal precioARS = await ObtenerPrecioCripto(dto.crypto_code, accion);

                if (precioARS <= 0)
                    return BadRequest("No se pudo obtener el precio de la criptomoneda");

                var transaccion = new TransaccionBV
                {
                    crypto_code = dto.crypto_code.ToLower(),
                    action = accion,
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
        public async Task<IActionResult> PutTransaction(int id, [FromBody] TransaccionBvDTO dto)
        {
            try
            {
                var transaccion = await _context.TransaccionBVs.FindAsync(id);

                if (transaccion == null)
                    return NotFound();

                var cliente = await _context.ClienteBVs.FindAsync(dto.client_id);

                if (cliente == null)
                    return BadRequest("El cliente no existe");

                if (string.IsNullOrWhiteSpace(dto.crypto_code))
                    return BadRequest("Cripto inválida");

                if (string.IsNullOrWhiteSpace(dto.action))
                    return BadRequest("Acción inválida");

                var accion = dto.action.ToLower();

                if (accion != "purchase" && accion != "sale")
                    return BadRequest("La acción debe ser purchase o sale");

                decimal cantidadDecimal;
                if (!decimal.TryParse(
                        dto.crypto_amount,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out cantidadDecimal) || cantidadDecimal <= 0)
                {
                    return BadRequest("Cantidad inválida: " + dto.crypto_amount);
                }

                DateTime fecha;
                if (!DateTime.TryParse(dto.datetime, out fecha))
                    return BadRequest("Fecha inválida");

                // VALIDAR SALDO EN VENTA (excluyendo esta transacción actual)
                if (accion == "sale")
                {
                    decimal saldoDisponible = await ObtenerSaldoDisponible(dto.client_id, dto.crypto_code, id);

                    if (cantidadDecimal > saldoDisponible)
                    {
                        return BadRequest($"No se puede vender esa cantidad. Saldo disponible: {saldoDisponible}");
                    }
                }

                decimal precioARS = await ObtenerPrecioCripto(dto.crypto_code, accion);

                if (precioARS <= 0)
                    return BadRequest("No se pudo obtener el precio de la criptomoneda");

                transaccion.crypto_code = dto.crypto_code.ToLower();
                transaccion.action = accion;
                transaccion.client_id = dto.client_id;
                transaccion.crypto_amount = dto.crypto_amount;
                transaccion.money = Math.Round(cantidadDecimal * precioARS, 2);
                transaccion.datetime = fecha;

                await _context.SaveChangesAsync();

                return Ok(transaccion);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTransaction(int id, [FromBody] EditarTransaccionBvDTO dto)
        {
            try
            {
                var transaccion = await _context.TransaccionBVs.FindAsync(id);

                if (transaccion == null)
                    return NotFound();

                if (!string.IsNullOrWhiteSpace(dto.crypto_code))
                    transaccion.crypto_code = dto.crypto_code.ToLower();

                if (!string.IsNullOrWhiteSpace(dto.crypto_amount))
                    transaccion.crypto_amount = dto.crypto_amount;

                if (dto.client_id.HasValue)
                    transaccion.client_id = dto.client_id.Value;

                if (dto.money.HasValue)
                    transaccion.money = dto.money.Value;

                if (!string.IsNullOrWhiteSpace(dto.action))
                    transaccion.action = dto.action.ToLower();

                if (!string.IsNullOrWhiteSpace(dto.datetime))
                {
                    DateTime fecha;
                    if (!DateTime.TryParse(dto.datetime, out fecha))
                        return BadRequest("Fecha inválida");

                    transaccion.datetime = fecha;
                }

                await _context.SaveChangesAsync();

                return Ok(transaccion);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
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

        // Calcula cuánto tiene disponible el cliente de una cripto
        private async Task<decimal> ObtenerSaldoDisponible(int clientId, string cryptoCode, int? excluirId = null)
        {
            var movimientos = await _context.TransaccionBVs
                .Where(t => t.client_id == clientId && t.crypto_code.ToLower() == cryptoCode.ToLower())
                .ToListAsync();

            if (excluirId.HasValue)
            {
                movimientos = movimientos.Where(t => t.id != excluirId.Value).ToList();
            }

            decimal compras = 0;
            decimal ventas = 0;

            foreach (var mov in movimientos)
            {
                decimal cantidad = 0;

                decimal.TryParse(
                    mov.crypto_amount,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out cantidad
                );

                if (mov.action.ToLower() == "purchase")
                    compras += cantidad;

                if (mov.action.ToLower() == "sale")
                    ventas += cantidad;
            }

            return compras - ventas;
        }

        // Obtiene el precio actual desde CriptoYa
        private async Task<decimal> ObtenerPrecioCripto(string cryptoCode, string action)
        {
            try
            {
                string exchange = "satoshitango";
                string coin = MapearCriptoACodigoCriptoYa(cryptoCode);

                if (string.IsNullOrEmpty(coin))
                    return 0;

                string url = $"https://criptoya.com/api/{exchange}/{coin}/ARS/1";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return 0;

                var json = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                // Compra = ask / Venta = bid
                if (action == "purchase" && root.TryGetProperty("ask", out var ask))
                    return ask.GetDecimal();

                if (action == "sale" && root.TryGetProperty("bid", out var bid))
                    return bid.GetDecimal();

                // fallback
                if (action == "purchase" && root.TryGetProperty("totalAsk", out var totalAsk))
                    return totalAsk.GetDecimal();

                if (action == "sale" && root.TryGetProperty("totalBid", out var totalBid))
                    return totalBid.GetDecimal();

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        private string MapearCriptoACodigoCriptoYa(string cryptoCode)
        {
            switch (cryptoCode.ToLower())
            {
                case "bitcoin":
                    return "BTC";
                case "ethereum":
                    return "ETH";
                case "usdc":
                    return "USDC";
                default:
                    return string.Empty;
            }
        }
    }
}