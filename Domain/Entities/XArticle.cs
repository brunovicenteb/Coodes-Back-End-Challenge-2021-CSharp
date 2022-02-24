using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Coodesh.Back.End.Challenge2021.CSharp.Domain.Entities
{
    /// <summary>Representation of an article.</summary>
    public class XArticle : XBaseEntity
    {
        [BsonId]
        [JsonIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ObjectID
        {
            get; set;
        }

        /// <summary>Defines whether the article is featured</summary>
        /// <example>false</example>
        [JsonPropertyName("featured")]
        [BsonRepresentation(BsonType.Boolean)]
        public bool Featured
        {
            get; set;
        }

        /// <summary>Article title</summary>
        /// <example>James Webb arrives at Lagrange Point 2</example>
        [JsonPropertyName("title")]
        [BsonRepresentation(BsonType.String)]
        public string Title
        {
            get; set;
        }


        /// <summary>Url of article on web</summary>
        /// <example>https://www.space.com/james-webb-arrives-destination-l2</example>
        [JsonPropertyName("url")]
        [BsonRepresentation(BsonType.String)]
        public string Url
        {
            get; set;
        }

        /// <summary>Url of image article</summary>
        /// <example>https://cdn.mos.cms.futurecdn.net/z8sf5yaERm5hCoeAaikmSX-970-80.jpg.webp</example>
        [JsonPropertyName("imageUrl")]
        [BsonRepresentation(BsonType.String)]
        public string ImageUrl
        {
            get; set;
        }

        /// <summary>website that informs the news on the subject</summary>
        /// <example>https://www.space.com/</example>
        [JsonPropertyName("newsSite")]
        [BsonRepresentation(BsonType.String)]
        public string NewsSite
        {
            get; set;
        }

        /// <summary>Summary of article</summary>
        /// <example>After traveling almost a million miles, NASA's James Webb Space Telescope reached its final destination...</example>
        [JsonPropertyName("summary")]
        [BsonRepresentation(BsonType.String)]
        public string Summary
        {
            get; set;
        }

        /// <summary>When the article was published</summary>
        /// <example>2021-11-02T20:29:04.000Z</example>
        [JsonPropertyName("publishedAt")]
        [BsonRepresentation(BsonType.String)]
        public string PublishedAt
        {
            get; set;
        }

        /// <summary>When the article was updated</summary>
        /// <example>2021-10-03T16:46:08.347Z</example>
        [JsonPropertyName("updatedAt")]
        [BsonRepresentation(BsonType.String)]
        public string UpdatedAt
        {
            get; set;
        }

        /// <summary>Article launches.</summary>
        [JsonPropertyName("launches")]
        public XLaunche[] Launches
        {
            get; set;
        }

        /// <summary>Article events.</summary>
        [JsonPropertyName("events")]
        public XEvents[] Events
        {
            get; set;
        }
    }
}