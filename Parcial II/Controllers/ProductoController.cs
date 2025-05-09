using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parcial_II.Models;

namespace Parcial_II.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly ParcialContext _contexto;
        public ProductoController(ParcialContext contexto)
        {
            _contexto = contexto;
        }

        // Get: api/Producto
        [HttpGet]
        public async Task<IEnumerable<Producto>> Get() {
            return await _contexto.Productos.ToListAsync();
        }

        // Get: api/Producto/id?id=3
        [HttpGet("id")]
        [ProducesResponseType(typeof(Producto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var producto = await _contexto.Productos.FindAsync(id);
            return producto == null ? NotFound() : Ok(producto);
        }

        // PUT: api/Product/4
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, Producto producto)
        {
            if (id != producto.Id) return BadRequest();
            _contexto.Entry(producto).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Product
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Crear(Producto producto)
        {
            await _contexto.Productos.AddAsync(producto);
            await _contexto.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto);
        }

        // DELETE: api/Product/4
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Borrar(int id)
        {
            var productToDelete = await _contexto.Productos.FindAsync(id);
            if (productToDelete == null) return NotFound();

            _contexto.Productos.Remove(productToDelete);
            await _contexto.SaveChangesAsync();

            return NoContent();
        }
    }
}
