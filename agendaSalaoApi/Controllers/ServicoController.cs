using agendaSalaoApi.Models;
using agendaSalaoApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace agendaSalaoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicoController : ControllerBase
    {
        private readonly ServicoService _service;

        public ServicoController(ServicoService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Servico>> GetAll()
        {
            return _service.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<Servico> GetById(int id)
        {
            var servico = _service.GetById(id);

            if (servico == null)
                return NotFound();

            return servico;
        }
        [HttpPost]
        public IActionResult Create(Servico servico)
        {
            _service.Insert(servico);
            return CreatedAtAction(nameof(GetById), new { id = servico.cd_servico }, servico);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Servico servico)
        {
            if (id != servico.cd_servico)
                return BadRequest();

            var existingServico = _service.GetById(id);
            if (existingServico == null)
                return NotFound();

            _service.Update(id, servico);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var servico = _service.GetById(id);
            if (servico == null)
                return NotFound();

            _service.Delete(id);
            return NoContent();
        }
    }
}