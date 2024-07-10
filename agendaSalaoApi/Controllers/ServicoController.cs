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
    }
}