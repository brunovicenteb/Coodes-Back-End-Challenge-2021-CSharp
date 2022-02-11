﻿using MongoDB.Bson;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Coodesh.Back.End.Challenge2021.CSharp.Domain.Entities
{
    public class XEvents : XBaseEntity
    {
        [JsonPropertyName("provider")]
        [BsonRepresentation(BsonType.String)]
        public string Provider
        {
            get; set;
        }
    }
}