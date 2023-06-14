namespace Application.FileHelpers
{
    public static class FileHelpersExtensions
    {
        public static async Task<List<T>> WriteToJsonFile<T>(this List<T> collection, string filePath)
        {
            using (var outputFileStream = File.CreateText("filePath"))
            using (var jsonWriter = new JsonTextWriter(outputFileStream))
            {
                var jsonSerializer = new JsonSerializer();

                await jsonWriter.WriteStartArrayAsync();

                foreach (var blog in collection)
                {
                    jsonSerializer.Serialize(jsonWriter, blog);
                    await outputFileStream.WriteLineAsync();
                }

                await jsonWriter.WriteEndArrayAsync();
                return collection;
            }
        }

        public static List<T> FillFromFile<T>(this List<T> colelction, string filePath)
        {
            using (var inputFileStream = File.OpenRead(filePath))
            using (var streamReader = new StreamReader(inputFileStream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var jsonSerializer = new JsonSerializer();
                colelction = jsonSerializer.Deserialize<List<T>>(jsonReader);
                return colelction;
            }
        }
    }
}
