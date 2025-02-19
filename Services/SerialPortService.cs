using System.IO.Ports;

namespace IntegraArduinoApi.Services
{
    public class SerialPortService
    {
        private readonly SerialPort _serialPort;
        private string _ultimaLeitura = "0,0";

        public SerialPortService()
        {
            // Definindo a porta que irei utilizar para conectar no arduinos
            _serialPort = new SerialPort("COM4", 9600);
            _serialPort.DataReceived += DataReceivedHandler;
            _serialPort.Open();
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string leitura = _serialPort.ReadLine().Trim();
                if (!string.IsNullOrEmpty(leitura) && leitura.Contains(","))
                {
                    _ultimaLeitura = leitura;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro na leitura serial: {ex.Message}");
            }
        }

        public (float temperatura, float umidade) ObterUltimaLeitura()
        {
            var dados = _ultimaLeitura.Split(",");
            if (dados.Length == 2 && float.TryParse(dados[0], out float temperatura) 
                && float.TryParse(dados[1], out float umidade))
            {
                return (temperatura, umidade);
            }
            return (0, 0);
        }
    }
}