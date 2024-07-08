namespace agendaSalaoApi.Models
{
    public class Horario
    {
        public int Id { get; set; }
        public DateTime DataHora { get; set; }
        public int ServicoId { get; set; }
        public string? TelefoneCabelereiro { get; set; }
    }
}
