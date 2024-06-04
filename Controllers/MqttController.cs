using Microsoft.AspNetCore.Mvc;
using backend_lembrol.MQTT;
using backend_lembrol.Dto;

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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PublishMessage([FromBody] MqttMessageDto request)
        {
            try
            {
                await _mqttService.PublishMessageAsync(request.Topic, request.Payload);
                return Ok("Message published");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
