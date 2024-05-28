using System.Text;
using MQTTnet;
using MQTTnet.Client;
using System.Threading.Tasks;
using System;

namespace backend_lembrol.MQTT
{
    public class MqttService
    {
        private IMqttClient _mqttClient;
        private MqttClientOptions _mqttOptions;

        public MqttService()
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();
            _mqttOptions = new MqttClientOptionsBuilder()
                .WithClientId("ClientID")
                .WithTcpServer("test.mosquitto.org", 1883)
                .WithCleanSession()
                .Build();

            _mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                var payload = e.ApplicationMessage.PayloadSegment;
                Console.WriteLine($"Received message: {Encoding.UTF8.GetString(payload.Array, payload.Offset, payload.Count)}");
                return Task.CompletedTask;
            };

            _mqttClient.ConnectedAsync += async e =>
            {
                Console.WriteLine("Connected to MQTT Broker");
                await _mqttClient.SubscribeAsync(new MqttClientSubscribeOptionsBuilder()
                    .WithTopicFilter(f => f.WithTopic("lembrol/getesp"))
                    .Build());
                Console.WriteLine("Subscribed to topic 'lembrol/getesp'");
            };

            _mqttClient.DisconnectedAsync += async e =>
            {
                Console.WriteLine("Disconnected from MQTT Broker");
                await Task.Delay(TimeSpan.FromSeconds(5));
                try
                {
                    await _mqttClient.ConnectAsync(_mqttOptions);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Reconnecting failed: {ex.Message}");
                }
            };

            ConnectAsync().GetAwaiter().GetResult();
        }

        public async Task ConnectAsync()
        {
            try
            {
                await _mqttClient.ConnectAsync(_mqttOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
            }
        }

        public async Task PublishMessageAsync(string topic, string payload)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                .WithRetainFlag(false)
                .Build();

            await _mqttClient.PublishAsync(message);
        }
    }
}