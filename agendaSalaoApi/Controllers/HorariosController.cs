using agendaSalaoApi.Models;
using agendaSalaoApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace agendaSalaoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorariosController : ControllerBase
    {
        private readonly HorarioService _service;

        public HorariosController(HorarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Horario>> GetAll()
        {
            return _service.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<Horario> GetById(int id)
        {
            var horario = _service.GetById(id);

            if (horario == null)
                return NotFound();

            return horario;
        }
        [HttpGet("byHorario/{horaAtendimento}")]
        public ActionResult<Horario> GetByHorario(string horaAtendimento)
        {
            var horario = _service.GetByHorario(horaAtendimento);

            if (horario == null)
                return NotFound();

            return Ok(horario);
        }

        [HttpPost]
        public IActionResult Create(Horario horario)
        {
            string? horaAtendimento = horario.horario;
            var verificaHorario = _service.GetByHorario(horaAtendimento);
            if (verificaHorario == null)
            {
                _service.Insert(horario);
                return CreatedAtAction(nameof(GetById), new { id = horario.cd_horario }, horario);
            }
            else
            {
                return Conflict(new { message = "Horário já está reservado. Por favor, escolha outro horário." });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Horario horario)
        {
            if (id != horario.cd_horario)
                return BadRequest();

            var existingServico = _service.GetById(id);
            if (existingServico == null)
                return NotFound();

            _service.Update(id, horario);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var horario = _service.GetById(id);
            if (horario == null)
                return NotFound();

            _service.Delete(id);
            return NoContent();
        }
    }
}