using MongoDB.Bson;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Coodesh.Back.End.Challenge2021.CSharp.Entities.Notifications;

namespace Coodesh.Back.End.Challenge2021.CSharp.Entities.Entities
{
    public class XArticle : XArtifact
    {
        [BsonId]
        [JsonIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ObjectID
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
        public XLaunche[] Launches
        {
            get; set;
        }


        [JsonPropertyName("events")]
        public XEvents[] Events
        {
            get; set;
        }
    }
}