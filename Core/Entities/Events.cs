using System;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Coodes.Back.End.Challenge2021.CSharp.Core.Entities
{
    public class Events
    {
        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.Int32)]
        public int ID
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