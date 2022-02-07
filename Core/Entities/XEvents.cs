using System;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Coodesh.Back.End.Challenge2021.CSharp.Entities.Notifications;

namespace Coodesh.Back.End.Challenge2021.CSharp.Entities.Entities
{
    public class XEvents : XArtifact
    {
        [JsonPropertyName("provider")]
        [BsonRepresentation(BsonType.String)]
        public string Provider
        {
            get; set;
        }
    }
}