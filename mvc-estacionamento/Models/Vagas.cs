namespace mvc_estacionamento.Models
{
    public class Vagas
    {
        public int id { get; set; }
        public int numero { get; set; }
        public string status { get; set; }
    }
    public class StatusVagas
    {
        public int total_vagas { get; set; }
        public int ocupadas { get; set; }
        public int livres { get; set; }
    }
}