using MongoDB.Bson;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Coodesh.Back.End.Challenge2021.CSharp.Entities.Notifications;

namespace Coodesh.Back.End.Challenge2021.CSharp.Entities.Entities
{
    public abstract class XArtifact
    {
        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.Int32)]
        public int ID
        {
            get; set;
        }
    }
}