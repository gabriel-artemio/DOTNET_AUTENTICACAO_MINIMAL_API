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
            var servico = _service.GetById(id);

            if (servico == null)
                return NotFound();

            return servico;
        }
        [HttpPost]
        public IActionResult Create(Horario horario)
        {
            _service.Insert(horario);
            return CreatedAtAction(nameof(GetById), new { id = horario.cd_horario }, horario);
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