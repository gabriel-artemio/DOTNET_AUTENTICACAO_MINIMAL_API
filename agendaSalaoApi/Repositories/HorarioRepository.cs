using agendaSalaoApi.Interfaces;
using agendaSalaoApi.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace agendaSalaoApi.Repositories
{
    public class HorarioRepository : IHorarioRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public HorarioRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public async Task<IEnumerable<Horario>> GetHorarios()
        {
            using (var connection = GetConnection())
            {
                var sql = "SELECT * FROM Horarios";
                return await connection.QueryAsync<Horario>(sql);
            }
        }

        public async Task<Horario> GetHorarioById(int id)
        {
            using (var connection = GetConnection())
            {
                var sql = "SELECT * FROM Horarios WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<Horario>(sql, new { Id = id });
            }
        }

        public async Task AddHorario(Horario horario)
        {
            using (var connection = GetConnection())
            {
                var sql = "INSERT INTO Horarios (DataHora, ServicoId, TelefoneCabelereiro) VALUES (@DataHora, @ServicoId, @TelefoneCabelereiro)";
                await connection.ExecuteAsync(sql, horario);
            }
        }

        public async Task UpdateHorario(Horario horario)
        {
            using (var connection = GetConnection())
            {
                var sql = "UPDATE Horarios SET DataHora = @DataHora, ServicoId = @ServicoId, TelefoneCabelereiro = @TelefoneCabelereiro WHERE Id = @Id";
                await connection.ExecuteAsync(sql, horario);
            }
        }

        public async Task DeleteHorario(int id)
        {
            using (var connection = GetConnection())
            {
                var sql = "DELETE FROM Horarios WHERE Id = @Id";
                await connection.ExecuteAsync(sql, new { Id = id });
            }
        }

        public async Task<IEnumerable<Horario>> GetHorariosByTelefone(string telefone)
        {
            using (var connection = GetConnection())
            {
                var sql = "SELECT * FROM Horarios WHERE TelefoneCabelereiro = @Telefone";
                return await connection.QueryAsync<Horario>(sql, new { Telefone = telefone });
            }
        }
    }
}