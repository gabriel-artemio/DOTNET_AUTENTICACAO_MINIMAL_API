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

        public List<StatusVagas> ListarVeiculos()
        {
            var lista = new List<StatusVagas>();

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT id, modelo, placa, cor FROM veiculos";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var veiculo = new StatusVagas
                        {
                            total_vagas = Convert.ToInt32(reader["id"]),
                            ocupadas = Convert.ToInt32(reader["id"]),
                            livres = Convert.ToInt32(reader["id"])
                        };

                        lista.Add(veiculo);
                    }
                }
            }

            return lista;
        }
    }
}