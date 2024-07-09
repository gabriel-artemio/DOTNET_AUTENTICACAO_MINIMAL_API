using agendaSalaoApi.Models;

namespace agendaSalaoApi.Interfaces
{
    public interface IServicosRepository
    {
        Task<IEnumerable<Servico>> GetServicos();
    }
}