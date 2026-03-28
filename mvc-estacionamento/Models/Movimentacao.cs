using System;

namespace mvc_estacionamento.Models
{
    public class Movimentacao
    {
        public int id { get; set; }
        public string modelo { get; set; }
        public string placa { get; set; }
        public int vaga_id { get; set; }
        public DateTime data_entrada { get; set; }
        public DateTime data_saida { get; set; }
        public Decimal valor_pago { get; set; }
        public string status { get; set; }
    }
}