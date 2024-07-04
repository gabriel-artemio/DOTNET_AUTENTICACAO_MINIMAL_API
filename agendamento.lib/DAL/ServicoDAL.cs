using agendamento.lib.Models;
using API.Conexao;
using System.Data.Common;
using System.Text;

namespace agendamento.lib.DAL
{
    internal class ServicoDAL
    {
        public Servico? GetById(Conexao cn, int id)
        {
            return GetAll(cn, id, string.Empty).FirstOrDefault();
        }
        public List<Servico> GetAll(Conexao cn, string pesquisa)
        {
            return GetAll(cn, 0, pesquisa);
        }
        private List<Servico> GetAll(Conexao cn, int id, string pesquisa)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            sb.Append("Id, Nome, Preco ");
            sb.Append("FROM Servico s ");
            sb.Append("WHERE 1 = 1 ");

            List<DbParameter> p = new List<DbParameter>();
            if (id > 0)
            {
                sb.Append("AND s.Id = " + id);
            }
            else if (!string.IsNullOrEmpty(pesquisa))
            {
                string query = "";
                new Query<Servico>().GetQuery(cn, pesquisa, ref query, ref p);
                sb.Append(query);
            }

            List<Servico> list = new List<Servico>();
            using (DbCommand cmd = cn.cmd())
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddRange(p.ToArray());
                using (DbDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new Servico
                        {
                            Id = dr.GetInt32(0),
                            Nome = dr.GetString(1),
                            Preco = dr.GetDecimal(2)
                        });
                    }
                }
            }
            return list;
        }
    }
}