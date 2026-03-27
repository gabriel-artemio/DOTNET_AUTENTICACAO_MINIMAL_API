using mvc_estacionamento.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace mvc_estacionamento.DAL
{
    public class VeiculoDAL
    {
        private string connectionString = ConfigurationManager
        .ConnectionStrings["MySqlConn"]
        .ConnectionString;

        public List<Veiculo> ListarVeiculos()
        {
            var lista = new List<Veiculo>();

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT id, modelo, placa, cor FROM veiculos";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var veiculo = new Veiculo
                        {
                            id = Convert.ToInt32(reader["id"]),
                            modelo = reader["modelo"].ToString(),
                            placa = reader["placa"].ToString(),
                            cor = reader["cor"].ToString()
                        };
                        
                        lista.Add(veiculo);
                    }
                }
            }

            return lista;
        }
        public void Inserir(Veiculo veiculo)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "INSERT INTO veiculos (modelo, placa, cor) VALUES (@modelo, @placa, @cor)";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@modelo", veiculo.modelo);
                    cmd.Parameters.AddWithValue("@placa", veiculo.placa);
                    cmd.Parameters.AddWithValue("@cor", veiculo.cor);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}