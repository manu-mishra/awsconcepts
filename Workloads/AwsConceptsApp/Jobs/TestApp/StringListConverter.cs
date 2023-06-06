using Newtonsoft.Json;

namespace TestApp
{
    public class StringListConverter : JsonConverter<List<string>>
    {
        public override List<string> ReadJson(JsonReader reader, Type objectType, List<string> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                // Deserialize the JSON array as a list of strings
                return serializer.Deserialize<List<string>>(reader);
            }

            // Deserialize the JSON string as a single-item list
            var value = serializer.Deserialize<string>(reader);
            return new List<string> { value };
        }

        public override void WriteJson(JsonWriter writer, List<string> value, JsonSerializer serializer)
        {
            // Serialize the list of strings as a JSON array
            serializer.Serialize(writer, value);
        }
    }
}
