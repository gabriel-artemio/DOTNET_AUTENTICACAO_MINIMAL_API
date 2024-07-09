using agendaSalaoApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace agendaSalaoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicoController : ControllerBase
    {
        private readonly IServicosRepository _repository;

        public ServicoController(IServicosRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetHorarios()
        {
            var servicos = await _repository.GetServicos();
            return Ok(servicos);
        }
    }
}