using agendaSalaoApi.Interfaces;
using agendaSalaoApi.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace agendaSalaoApi.Repositories
{
    public class ServicoRepository : IServicosRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ServicoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public async Task<IEnumerable<Servico>> GetServicos()
        {
            using (var connection = GetConnection())
            {
                var sql = "SELECT * FROM Servicos";
                return await connection.QueryAsync<Servico>(sql);
            }
        }
    }
}