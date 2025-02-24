using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;
using Presentation.Model.Clientes;

namespace Presentation.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private static List<Cliente> clientes = new List<Cliente>();

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromBody] ClienteViewModel viewModel)
        {
            if (viewModel == null)
            {
                return BadRequest("Cliente inválido.");
            }

            var cliente = new Cliente
            {
                Id = Guid.NewGuid(),
                Name = viewModel.Name,
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                BirthDate = viewModel.BirthDate,
            };

            clientes.Add(cliente);

            return CreatedAtAction(nameof(Find), new { id = cliente.Id }, cliente);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult Find(Guid id)
        {
            var cliente = clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            return Ok(cliente);
        }

        [HttpGet]
        [Authorize]
        public IActionResult FindAll()
        {
            return Ok(clientes);
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Update(Guid id, [FromBody] ClienteViewModel viewModel)
        {
            var cliente = clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            cliente.Name = viewModel.Name ?? cliente.Name;
            cliente.Email = viewModel.Email ?? cliente.Email;
            cliente.Phone = viewModel.Phone ?? cliente.Phone;
            cliente.BirthDate = viewModel.BirthDate != default ? viewModel.BirthDate : cliente.BirthDate;
            cliente.Active = viewModel.Active;

            return NoContent();
        }

        [HttpPatch("{id}/activate")]
        [Authorize]
        public IActionResult Activate(Guid id)
        {
            var cliente = clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            cliente.Active = true;
            return NoContent();
        }

        [HttpPatch("{id}/deactivate")]
        [Authorize]
        public IActionResult Deactivate(Guid id)
        {
            var cliente = clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            cliente.Active = false;
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(Guid id)
        {
            var cliente = clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            clientes.Remove(cliente);
            return NoContent();
        }
    }
}
