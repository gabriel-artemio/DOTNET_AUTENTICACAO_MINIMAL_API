using IntegraArduinoApi.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/sensor")]
[ApiController]
public class SensorController : ControllerBase
{
    private readonly SerialPortService _serialService;

    public SensorController(SerialPortService serialService)
    {
        _serialService = serialService;
    }

    [HttpGet]
    public IActionResult GetSensorData()
    {
        var (temperatura, umidade) = _serialService.ObterUltimaLeitura();
        return Ok(new { temperatura, umidade });
    }
}