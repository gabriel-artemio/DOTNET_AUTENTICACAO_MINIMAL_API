using agendaSalaoApi.Models;
using MySql.Data.MySqlClient;

namespace agendaSalaoApi.Services
{
    public class ServicoService
    {
        private readonly string _connectionString;

        public ServicoService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Servico> GetAll()
        {
            var servicos = new List<Servico>();
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string sql = "SELECT cd_servico, nm_servico, valor FROM servico";
            using var cmd = new MySqlCommand(sql, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                servicos.Add(new Servico
                {
                    cd_servico = reader.GetInt32("cd_servico"),
                    nm_servico = reader.GetString("nm_servico"),
                    valor = reader.GetDecimal("valor")
                });
            }
            return servicos;
        }
        public Servico GetById(int cd_servico)
        {
            Servico servico = null;

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string sql = "SELECT cd_servico, nm_servico, valor FROM servico WHERE cd_servico = @cd_servico";
            using var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@cd_servico", cd_servico);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                servico = new Servico
                {
                    cd_servico = reader.GetInt32("cd_servico"),
                    nm_servico = reader.GetString("nm_servico"),
                    valor = reader.GetDecimal("valor")
                };
            }
            return servico;
        }
        public void Insert(Servico servico)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string sql = "INSERT INTO servico (nm_servico, valor) VALUES (@nm_servico, @valor)";
            using var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@nm_servico", servico.nm_servico);
            cmd.Parameters.AddWithValue("@valor", servico.valor);
            cmd.ExecuteNonQuery();
        }

        public void Update(int cd_servico, Servico servico)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string sql = "UPDATE servico SET nm_servico = @nm_servico, valor = @valor WHERE cd_servico = @cd_servico";
            using var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@cd_servico", cd_servico);
            cmd.Parameters.AddWithValue("@nm_servico", servico.nm_servico);
            cmd.Parameters.AddWithValue("@valor", servico.valor);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int cd_servico)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string sql = "DELETE FROM servico WHERE cd_servico = @cd_servico";
            using var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@cd_servico", cd_servico);
            cmd.ExecuteNonQuery();
        }
    }
}