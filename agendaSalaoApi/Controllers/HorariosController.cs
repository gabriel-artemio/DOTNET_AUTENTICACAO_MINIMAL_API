using agendaSalaoApi.Models;
using agendaSalaoApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace agendaSalaoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorariosController : ControllerBase
    {
        private readonly IHorarioRepository _repository;

        public HorariosController(IHorarioRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetHorarios()
        {
            var horarios = await _repository.GetHorarios();
            return Ok(horarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHorario(int id)
        {
            var horario = await _repository.GetHorarioById(id);
            if (horario == null)
            {
                return NotFound();
            }
            return Ok(horario);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHorario([FromBody] Horario horario)
        {
            await _repository.AddHorario(horario);
            return CreatedAtAction(nameof(GetHorario), new { id = horario.Id }, horario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHorario(int id, [FromBody] Horario horario)
        {
            if (id != horario.Id)
            {
                return BadRequest();
            }

            var existingHorario = await _repository.GetHorarioById(id);
            if (existingHorario == null)
            {
                return NotFound();
            }

            await _repository.UpdateHorario(horario);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHorario(int id)
        {
            var existingHorario = await _repository.GetHorarioById(id);
            if (existingHorario == null)
            {
                return NotFound();
            }

            await _repository.DeleteHorario(id);
            return NoContent();
        }

        [HttpGet("telefone/{telefone}")]
        public async Task<IActionResult> GetHorariosByTelefone(string telefone)
        {
            var horarios = await _repository.GetHorariosByTelefone(telefone);
            return Ok(horarios);
        }
    }
}