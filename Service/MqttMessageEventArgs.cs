using MQTTnet;
using System;

namespace backend_lembrol.Service
{
public class MqttMessageEventArgs : EventArgs
{
    public string Topic { get; set; }
    public MqttApplicationMessage Message { get; set; }

    public MqttMessageEventArgs(string topic, MqttApplicationMessage message)
    {
        Topic = topic;
        Message = message;
    }
}
}