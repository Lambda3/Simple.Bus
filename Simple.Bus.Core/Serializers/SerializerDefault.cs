using System.Text.Json;

namespace Simple.Bus.Core.Serializers
{
    public class SerializerDefault : ISerializer
    {
        public T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public string Serialize<T>(T value)
        {
            return JsonSerializer.Serialize(value);
        }
    }
}
