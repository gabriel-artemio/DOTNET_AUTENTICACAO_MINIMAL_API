using agendaSalaoApi.Models;
using MySql.Data.MySqlClient;

namespace agendaSalaoApi.Services
{
    public class SalaoService
    {
        private readonly string _connectionString;

        public SalaoService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Salao> GetAll()
        {
            var salao = new List<Salao>();
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string sql = "SELECT cd_salao, nm_salao, telefone_salao, responsavel FROM salao";
            using var cmd = new MySqlCommand(sql, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                salao.Add(new Salao
                {
                    cd_salao = reader.GetInt32("cd_salao"),
                    nm_salao = reader.GetString("nm_salao"),
                    telefone_salao = reader.GetString("telefone_salao"),
                    responsavel = reader.GetString("responsavel")
                });
            }
            return salao;
        }
        public Salao GetById(int cd_salao)
        {
            Salao salao = null;

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string sql = "SELECT cd_salao, nm_salao, telefone_salao, responsavel FROM salao WHERE cd_salao = @cd_salao";
            using var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@cd_salao", cd_salao);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                salao = new Salao
                {
                    cd_salao = reader.GetInt32("cd_salao"),
                    nm_salao = reader.GetString("nm_salao"),
                    telefone_salao = reader.GetString("telefone_salao"),
                    responsavel = reader.GetString("responsavel")
                };
            }
            return salao;
        }
        public void Insert(Salao salao)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string sql = "INSERT INTO salao (nm_salao, telefone_salao, responsavel) VALUES (@nm_salao, @telefone_salao, @responsavel)";
            using var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@nm_salao", salao.nm_salao);
            cmd.Parameters.AddWithValue("@telefone_salao", salao.telefone_salao);
            cmd.Parameters.AddWithValue("@responsavel", salao.responsavel);
            cmd.ExecuteNonQuery();
        }

        public void Update(int cd_salao, Salao salao)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string sql = "UPDATE salao SET nm_salao = @nm_salao, telefone_salao = @telefone_salao, responsavel = @responsavel WHERE cd_salao = @cd_salao";
            using var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@cd_salao", cd_salao);
            cmd.Parameters.AddWithValue("@nm_salao", salao.nm_salao);
            cmd.Parameters.AddWithValue("@telefone_salao", salao.telefone_salao);
            cmd.Parameters.AddWithValue("@responsavel", salao.responsavel);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int cd_salao)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string sql = "DELETE FROM salao WHERE cd_salao = @cd_salao";
            using var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@cd_salao", cd_salao);
            cmd.ExecuteNonQuery();
        }
    }
}