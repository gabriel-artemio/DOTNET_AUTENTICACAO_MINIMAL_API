namespace agendamento.lib.Models
{
    public class Agendamento
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public int FuncionarioId { get; set; }
        public Funcionario? Funcionario { get; set; }
        public int ServicoId { get; set; }
        public Servico? Servico { get; set; }
    }
}