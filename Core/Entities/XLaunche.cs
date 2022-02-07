using System;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Coodesh.Back.End.Challenge2021.CSharp.Entities.Notifications;

namespace Coodesh.Back.End.Challenge2021.CSharp.Entities.Entities
{
    public class XLaunche : XNotifies
    {
        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.String)]
        public Guid ID
        {
            get; set;
        }

        [JsonPropertyName("provider")]
        [BsonRepresentation(BsonType.String)]
        public string Provider
        {
            get; set;
        }
    }
}