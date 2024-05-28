using Microsoft.AspNetCore.Mvc;
using backend_lembrol.MQTT;
using System.Threading.Tasks;

namespace backend_lembrol.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MqttController : ControllerBase
    {
        private readonly MqttService _mqttService;

        public MqttController(MqttService mqttService)
        {
            _mqttService = mqttService;
        }

        [HttpPost("publish")]
        public async Task<IActionResult> PublishMessage([FromBody] MqttMessageRequest request)
        {
            await _mqttService.PublishMessageAsync(request.Topic, request.Payload);
            return Ok("Message published");
        }
    }

    public class MqttMessageRequest
    {
        public string Topic { get; set; }
        public string Payload { get; set; }
    }
}
