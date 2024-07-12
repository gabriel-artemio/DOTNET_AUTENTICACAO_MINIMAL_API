using agendaSalaoApi.Models;
using MySql.Data.MySqlClient;

namespace agendaSalaoApi.Services
{
    public class HorarioService
    {
        private readonly string _connectionString;

        public HorarioService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Horario> GetAll()
        {
            var horarios = new List<Horario>();
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string sql = "SELECT cd_horario, dt_atendimento, telefone_cliente, nome_cliente, cd_servico, cd_salao, horario FROM horario";
            using var cmd = new MySqlCommand(sql, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                horarios.Add(new Horario
                {
                    cd_horario = reader.GetInt32("cd_horario"),
                    dt_atendimento = reader.GetDateTime("dt_atendimento"),
                    telefone_cliente = reader.GetString("telefone_cliente"),
                    nome_cliente = reader.GetString("nome_cliente"),
                    cd_servico = reader.GetInt32("cd_servico"),
                    cd_salao = reader.GetInt32("cd_salao"),
                    horario = reader.GetString("horario")
                });
            }
            return horarios;
        }
        public Horario GetById(int cd_horario)
        {
            Horario horario = null;

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string sql = "SELECT cd_horario, dt_atendimento, telefone_cliente, nome_cliente, cd_servico, cd_salao, horario FROM horario WHERE cd_horario = @cd_horario";
            using var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@cd_horario", cd_horario);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                horario = new Horario
                {
                    cd_horario = reader.GetInt32("cd_horario"),
                    dt_atendimento = reader.GetDateTime("dt_atendimento"),
                    telefone_cliente = reader.GetString("telefone_cliente"),
                    nome_cliente = reader.GetString("nome_cliente"),
                    cd_servico = reader.GetInt32("cd_servico"),
                    cd_salao = reader.GetInt32("cd_salao"),
                    horario = reader.GetString("horario")
                };
            }
            return horario;
        }
        public Horario GetByHorario(string horaAtendimento)
        {
            Horario horario = null;

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string sql = "SELECT * FROM horario WHERE horario = @horario";
            using var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@horario", horaAtendimento);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                horario = new Horario
                {
                    cd_horario = reader.GetInt32("cd_horario"),
                    dt_atendimento = reader.GetDateTime("dt_atendimento"),
                    telefone_cliente = reader.GetString("telefone_cliente"),
                    nome_cliente = reader.GetString("nome_cliente"),
                    cd_servico = reader.GetInt32("cd_servico"),
                    cd_salao = reader.GetInt32("cd_salao"),
                    horario = reader.GetString("horario")
                };
            }
            return horario;
        }
        public void Insert(Horario horario)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string sql = "INSERT INTO horario (dt_atendimento, telefone_cliente, nome_cliente, cd_servico, cd_salao, horario) VALUES (@dt_atendimento, @telefone_cliente, @nome_cliente, @cd_servico, @cd_salao, @horario)";
            using var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@dt_atendimento", horario.dt_atendimento);
            cmd.Parameters.AddWithValue("@telefone_cliente", horario.telefone_cliente);
            cmd.Parameters.AddWithValue("@nome_cliente", horario.nome_cliente);
            cmd.Parameters.AddWithValue("@cd_servico", horario.cd_servico);
            cmd.Parameters.AddWithValue("@cd_salao", horario.cd_salao);
            cmd.Parameters.AddWithValue("@horario", horario.horario);
            cmd.ExecuteNonQuery();
        }

        public void Update(int cd_horario, Horario horario)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string sql = "UPDATE horario SET dt_atendimento = @dt_atendimento, telefone_cliente = @telefone_cliente," +
                "nome_cliente = @nome_cliente, cd_servico = @cd_servico, cd_salao = @cd_salao, horario = @horario WHERE cd_horario = @cd_horario";
            using var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@cd_horario", cd_horario);
            cmd.Parameters.AddWithValue("@dt_atendimento", horario.dt_atendimento);
            cmd.Parameters.AddWithValue("@telefone_cliente", horario.telefone_cliente);
            cmd.Parameters.AddWithValue("@nome_cliente", horario.nome_cliente);
            cmd.Parameters.AddWithValue("@cd_servico", horario.cd_servico);
            cmd.Parameters.AddWithValue("@cd_salao", horario.cd_salao);
            cmd.Parameters.AddWithValue("@horario", horario.horario);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int cd_horario)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string sql = "DELETE FROM horario WHERE cd_horario = @cd_horario";
            using var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@cd_horario", cd_horario);
            cmd.ExecuteNonQuery();
        }
    }
}