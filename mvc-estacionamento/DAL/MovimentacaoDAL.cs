using mvc_estacionamento.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace mvc_estacionamento.DAL
{
    public class MovimentacaoDAL
    {
        private string connectionString = ConfigurationManager
        .ConnectionStrings["MySqlConn"]
        .ConnectionString;

        public List<Movimentacao> ListarMovimentacoes()
        {
            var lista = new List<Movimentacao>();

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT m.id, v.modelo, v.placa, m.vaga_id, m.data_entrada, m.data_saida, m.valor_pago, m.status " +
                    "FROM movimentacoes m " +
                    "INNER JOIN veiculos v ON m.veiculo_id = v.id";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var movimentacao = new Movimentacao
                        {
                            id = Convert.ToInt32(reader["id"]),
                            modelo = reader["modelo"].ToString(),
                            placa = reader["placa"].ToString(),
                            vaga_id = Convert.ToInt32(reader["vaga_id"]),
                            data_entrada = Convert.ToDateTime(reader["data_entrada"]),
                            data_saida = Convert.ToDateTime(reader["data_saida"]),
                            valor_pago = Convert.ToDecimal(reader["valor_pago"]),
                            status = reader["status"].ToString()
                        };

                        lista.Add(movimentacao);
                    }
                }
            }

            return lista;
        }
    }
}