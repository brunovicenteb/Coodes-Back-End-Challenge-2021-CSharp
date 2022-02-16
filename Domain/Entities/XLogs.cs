using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Coodesh.Back.End.Challenge2021.CSharp.Domain.Entities
{
    public class XLogs
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ObjectID
        {
            get; set;
        }

        [BsonRepresentation(BsonType.String)]
        public string Title
        {
            get; set;
        }

        [BsonRepresentation(BsonType.String)]
        public string Details
        {
            get; set;
        }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime ExecAt
        {
            get; set;
        }
    }
}