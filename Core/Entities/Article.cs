using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace Coodes.Back.End.Challenge2021.CSharp.Core.Entities
{
    public class Article
    {
        [BsonId]
        [JsonIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ObjectID
        {
            get; set;
        }

        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.Int32)]
        public int ID
        {
            get; set;
        }

        [JsonPropertyName("featured")]
        [BsonRepresentation(BsonType.Boolean)]
        public bool Featured
        {
            get; set;
        }

        [JsonPropertyName("title")]
        [BsonRepresentation(BsonType.String)]
        public string Title
        {
            get; set;
        }

        [JsonPropertyName("url")]
        [BsonRepresentation(BsonType.String)]
        public string Url
        {
            get; set;
        }

        [JsonPropertyName("imageUrl")]
        [BsonRepresentation(BsonType.String)]
        public string ImageUrl
        {
            get; set;
        }

        [JsonPropertyName("newsSite")]
        [BsonRepresentation(BsonType.String)]
        public string NewsSite
        {
            get; set;
        }

        [JsonPropertyName("summary")]
        [BsonRepresentation(BsonType.String)]
        public string Summary
        {
            get; set;
        }

        [JsonPropertyName("publishedAt")]
        [BsonRepresentation(BsonType.String)]
        public string PublishedAt
        {
            get; set;
        }

        [JsonPropertyName("updatedAt")]
        [BsonRepresentation(BsonType.String)]
        public string UpdatedAt
        {
            get; set;
        }

        [JsonPropertyName("launches")]
        public Launche[] Launches
        {
            get; set;
        }


        [JsonPropertyName("events")]
        public Events[] Events
        {
            get; set;
        }
    }
}