using System;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Coodesh.Back.End.Challenge2021.CSharp.Core.Entities
{
    public class Launche
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