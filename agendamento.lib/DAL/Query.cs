using API.Conexao;
using System.Data.Common;
using System.Reflection;

namespace agendamento.lib.DAL
{
    internal class Query<T>
    {
        public void GetQuery(Conexao cn, string pesquisa, ref string query, ref List<DbParameter> p)
        {
            if (string.IsNullOrEmpty(pesquisa))
            {
                return;
            }
            Type objTipo = typeof(T);
            string nm_model = typeof(T).Name;
            string alias = null;
            bool condicaoOr = false;
            List<string> condicoes = new()
            {
                "AND",
                "="
            };
            string[] campos = pesquisa.Split('|');
            IList<PropertyInfo> props = new List<PropertyInfo>(objTipo.GetProperties());
            foreach (string campo in campos)
            {
                string[] chave_valor = campo.Split(':');
                if (chave_valor.Length != 2)
                {
                    continue;
                }
                if (chave_valor[1].Contains('*'))
                {
                    condicoes[1] = "LIKE";
                    chave_valor[1] = chave_valor[1].Replace("*", "%");
                }
                if (chave_valor[0].Contains('@'))
                {
                    condicaoOr = true;
                    condicoes[0] = "OR";
                    chave_valor[0] = chave_valor[0].Remove(chave_valor[0].IndexOf('@'), 1);
                }
                PropertyInfo prop = props.Where(i => i.Name.ToLower().Equals(chave_valor[0].ToLower())).FirstOrDefault();
                if (prop == null)
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(alias))
                {
                    query += string.Format("{0} {1}.{2} {3} {4} ", condicoes[0], alias, chave_valor[0], condicoes[1], chave_valor[0].ParameterName());
                }
                else
                {
                    query += string.Format("{0} {1} {2} {3} ", condicoes[0], chave_valor[0], condicoes[1], chave_valor[0].ParameterName());
                }
                if (condicaoOr)
                {
                    query = query.Insert(4, "(").Insert(query.Length, ")");
                    condicaoOr = false;
                }
                p.Add(cn.criarParametro(chave_valor[0], chave_valor[1]));
            }
        }
    }
}
