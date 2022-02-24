using System;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Coodesh.Back.End.Challenge2021.CSharp.Domain.Entities
{
    /// <summary>Representation of an article launche.</summary>
    public class XLaunche
    {
        [JsonPropertyName("id")]
        public Guid ID
        {
            get; set;
        }

        /// <summary>Launch Library.</summary>
        [JsonPropertyName("provider")]
        [BsonRepresentation(BsonType.String)]
        public string Provider
        {
            get; set;
        }
    }
}