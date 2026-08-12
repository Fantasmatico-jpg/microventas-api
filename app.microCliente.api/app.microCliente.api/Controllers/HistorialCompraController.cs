using app.microCliente.common.DTOs;
using app.microCliente.services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace app.microCliente.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistorialCompraController : ControllerBase
    {
        private readonly IHistorialCompraService _historialService;

        public HistorialCompraController(
            IHistorialCompraService historialService)
        {
            _historialService = historialService;
        }

        // GET: api/HistorialCompra
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var result = await _historialService.SeleccionarTodos();

            if (result.Success)
                return Ok(result);

            return StatusCode(500, result);
        }

        // GET: api/HistorialCompra/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var result = await _historialService.SeleccionarUno(id);

            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }

        // GET: api/HistorialCompra/cliente/5
        [HttpGet("cliente/{clienteId}")]
        public async Task<IActionResult> ObtenerPorCliente(int clienteId)
        {
            var result =
                await _historialService.SeleccionarPorCliente(clienteId);

            if (result.Success)
                return Ok(result);

            return StatusCode(500, result);
        }

        // POST: api/HistorialCompra
        [HttpPost]
        public async Task<IActionResult> Crear(
            [FromBody] HistorialCompraDTO historialDTO)
        {
            var result =
                await _historialService.Insertar(historialDTO);

            if (!result.Success)
                return StatusCode(500, result);

            return Ok(result);
        }

        // PUT: api/HistorialCompra/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(
            int id,
            [FromBody] HistorialCompraDTO historialDTO)
        {
            var result =
                await _historialService.Actualizar(id, historialDTO);

            if (!result.Success)
                return StatusCode(500, result);

            return Ok(result);
        }

        // DELETE: api/HistorialCompra/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var result =
                await _historialService.Eliminar(id);

            if (!result.Success)
                return StatusCode(500, result);

            return Ok(result);
        }
    }
}