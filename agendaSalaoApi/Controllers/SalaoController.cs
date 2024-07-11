using agendaSalaoApi.Models;
using agendaSalaoApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace agendaSalaoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaoController : ControllerBase
    {
        private readonly SalaoService _service;

        public SalaoController(SalaoService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Salao>> GetAll()
        {
            return _service.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<Salao> GetById(int id)
        {
            var salao = _service.GetById(id);

            if (salao == null)
                return NotFound();

            return salao;
        }
        [HttpPost]
        public IActionResult Create(Salao salao)
        {
            _service.Insert(salao);
            return CreatedAtAction(nameof(GetById), new { id = salao.cd_salao }, salao);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Salao salao)
        {
            if (id != salao.cd_salao)
                return BadRequest();

            var existingSalao = _service.GetById(id);
            if (existingSalao == null)
                return NotFound();

            _service.Update(id, salao);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var salao = _service.GetById(id);
            if (salao == null)
                return NotFound();

            _service.Delete(id);
            return NoContent();
        }
    }
}