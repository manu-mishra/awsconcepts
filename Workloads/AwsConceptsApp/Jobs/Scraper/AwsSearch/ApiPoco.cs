namespace Scraper.AwsSearch;

public class Item
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("locale")]
    public string Locale { get; set; }

    [JsonProperty("directoryId")]
    public string DirectoryId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("author")]
    [JsonConverter(typeof(StringListConverter))]
    public List<string> Author { get; set; }

    [JsonProperty("createdBy")]
    [JsonConverter(typeof(StringListConverter))]
    public List<string> CreatedBy { get; set; }

    [JsonProperty("lastUpdatedBy")]
    [JsonConverter(typeof(StringListConverter))]
    public List<string> LastUpdatedBy { get; set; }

    [JsonProperty("numImpressions")]
    public int NumImpressions { get; set; }

    [JsonProperty("score")]
    public double Score { get; set; }

    [JsonProperty("dateCreated")]
    public DateTime DateCreated { get; set; }

    [JsonProperty("dateUpdated")]
    public DateTime DateUpdated { get; set; }

    [JsonProperty("additionalFields")]
    public Dictionary<string, object> AdditionalFields { get; set; }
}

public class Tag
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("locale")]
    public string Locale { get; set; }

    [JsonProperty("tagNamespaceId")]
    public string TagNamespaceId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("createdBy")]
    public string CreatedBy { get; set; }

    [JsonProperty("lastUpdatedBy")]
    public string LastUpdatedBy { get; set; }

    [JsonProperty("dateCreated")]
    public DateTime DateCreated { get; set; }

    [JsonProperty("dateUpdated")]
    public DateTime DateUpdated { get; set; }
}

public class Record
{
    [JsonProperty("item")]
    public Item Item { get; set; }

    [JsonProperty("tags")]
    public List<Tag> Tags { get; set; }
}

public class Metadata
{
    [JsonProperty("count")]
    public int Count { get; set; }

    [JsonProperty("totalHits")]
    public int TotalHits { get; set; }
}

public class SearchResponse
{
    [JsonProperty("items")]
    public List<Record> Items { get; set; }

    [JsonProperty("metadata")]
    public Metadata Metadata { get; set; }

    [JsonProperty("fieldTypes")]
    public Dictionary<string, string> FieldTypes { get; set; }
}
