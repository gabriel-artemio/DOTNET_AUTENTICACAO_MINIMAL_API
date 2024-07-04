using agendamento.lib.BLL;
using agendamento.lib.Models;
using Microsoft.AspNetCore.Mvc;

namespace agendamento.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicoController : ControllerBase
    {
        private ServicoBLL servicoBLL;
        public ServicoController()
        {
            servicoBLL = new ServicoBLL();
        }

        [HttpGet]
        public List<Servico> Get([FromQuery] string? pesquisa)
        {
            return servicoBLL.GetAll(pesquisa);
        }

        [HttpGet("{id}")]
        public Servico? Get(int id)
        {
            return servicoBLL.GetById(id);
        }
    }
}