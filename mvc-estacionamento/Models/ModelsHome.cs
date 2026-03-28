using System.Collections.Generic;

namespace mvc_estacionamento.Models
{
    public class ModelsHome
    {
        public StatusVagas StatusVagas { get; set; }
        public List<Veiculo> Veiculos { get; set; }
        public List<Vagas> Vagas { get; set; }
    }
}