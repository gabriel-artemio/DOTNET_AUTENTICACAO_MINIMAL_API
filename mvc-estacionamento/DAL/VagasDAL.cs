using mvc_estacionamento.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace mvc_estacionamento.DAL
{
    public class VagasDAL
    {
        private string connectionString = ConfigurationManager
        .ConnectionStrings["MySqlConn"]
        .ConnectionString;

        public List<Vagas> ListarVagas()
        {
            var lista = new List<Vagas>();

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT id, numero, status FROM vagas";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var vaga = new Vagas
                        {
                            id = Convert.ToInt32(reader["id"]),
                            numero = Convert.ToInt32(reader["numero"]),
                            status = reader["status"].ToString()
                        };

                        lista.Add(vaga);
                    }
                }
            }

            return lista;
        }

        public StatusVagas ListarStatusVagas()
        {
            var vagas = new StatusVagas();

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT COUNT(*) AS total_vagas, SUM(CASE WHEN status = 'ocupada' THEN 1 ELSE 0 END) AS ocupadas, " +
                    "SUM(CASE WHEN status = 'livre' THEN 1 ELSE 0 END) AS livres FROM vagas";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        vagas = new StatusVagas
                        {
                            total_vagas = Convert.ToInt32(reader["total_vagas"]),
                            ocupadas = Convert.ToInt32(reader["ocupadas"]),
                            livres = Convert.ToInt32(reader["livres"])
                        };
                    }
                }
            }

            return vagas;
        }
    }
}