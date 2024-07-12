namespace agendaSalaoApi.Models
{
    public class Horario
    {
        public int cd_horario { get; set; }
        public DateTime dt_atendimento { get; set; }
        public string? telefone_cliente { get; set; }
        public string? nome_cliente { get; set; }
        public int cd_servico { get; set; }
        public int cd_salao { get; set; }
        public string? horario { get; set; }
    }
}
