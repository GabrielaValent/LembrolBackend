using System.Text;
using MQTTnet;
using MQTTnet.Client;
using System.Text.Json;
using backend_lembrol.Dto;
using backend_lembrol.Repository;
using System.Globalization;

namespace backend_lembrol.MQTT
{
    public class MqttService
    {
        private readonly IMqttClient _mqttClient;
        private readonly MqttClientOptions _mqttOptions;
        private readonly TagRepositoryFactory _tagRepositoryFactory;

        public MqttService(TagRepositoryFactory tagRepositoryFactory)
        {
            _tagRepositoryFactory = tagRepositoryFactory;

            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();
            _mqttOptions = new MqttClientOptionsBuilder()
                .WithClientId("ClientID")
                .WithTcpServer("test.mosquitto.org", 1883)
                .WithCleanSession()
                .Build();

            _mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                var payload = e.ApplicationMessage.Payload;

                if (payload == null)
                {
                    Console.WriteLine("Received empty message");
                    return;
                }
                
                Console.WriteLine($"Received message: {Encoding.UTF8.GetString(payload)}");
                
                try
                {
                    var data = JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, string>>>>(payload);

                    if (data != null && data.ContainsKey("tags"))
                    {
                        var tags = data["tags"];

                        foreach (var tag in tags)
                        {
                            if (tag != null && tag.ContainsKey("tagId") && tag.ContainsKey("lat") && tag.ContainsKey("lng"))
                            {
                                double.TryParse(tag["lat"], CultureInfo.InvariantCulture,out double latitude);
                                double.TryParse(tag["lng"], CultureInfo.InvariantCulture, out double longitude);

                                var messageDto = new ReceivedFromEspDto
                                {
                                    TagId = tag["tagId"],
                                    Lat = latitude,
                                    Lng = longitude
                                };

                                using (var scope = _tagRepositoryFactory.CreateScope())
                                {
                                    try
                                    {
                                        var tagRepository = scope.ServiceProvider.GetRequiredService<TagRepository>();
                                        await tagRepository.InsertEspData(messageDto.TagId, messageDto.Lat, messageDto.Lng);
                                    }
                                    catch (InvalidOperationException ex)
                                    {
                                        Console.WriteLine($"Error: {ex.Message}");
                                        Console.WriteLine(ex.StackTrace);
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Received malformed message for a tag");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Received malformed message");
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Error parsing message: {ex.Message}");
                }
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
