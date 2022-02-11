using MongoDB.Bson;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Coodesh.Back.End.Challenge2021.CSharp.Domain.Entities
{
    public abstract class XBaseEntity
    {
        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.Int32)]
        public virtual int ID { get; set; }
    }
}