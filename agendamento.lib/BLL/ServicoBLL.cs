using agendamento.lib.DAL;
using agendamento.lib.Models;
using API.Conexao;

namespace agendamento.lib.BLL
{
    public class ServicoBLL
    {
        private readonly ServicoDAL servicoDAL;

        public ServicoBLL()
        {
            servicoDAL = new ServicoDAL();
        }

        public List<Servico> GetAll(string? pesquisa)
        {
            List<Servico> list = new();

            using (Conexao cn = new Conexao())
            {
                list = servicoDAL.GetAll(cn, pesquisa);
            }
            return list;
        }
        public Servico GetById(int id)
        {
            Servico? retorno;

            using (Conexao cn = new Conexao())
            {
                retorno = servicoDAL.GetById(cn, id);
            }
            return retorno;
        }
    }
}