using agendaSalaoApi.Models;

namespace agendaSalaoApi.Repositories
{
    public interface IHorarioRepository
    {
        Task<IEnumerable<Horario>> GetHorarios();
        Task<Horario> GetHorarioById(int id);
        Task AddHorario(Horario horario);
        Task UpdateHorario(Horario horario);
        Task DeleteHorario(int id);
        Task<IEnumerable<Horario>> GetHorariosByTelefone(string telefone);
    }
}