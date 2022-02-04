using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coodesh.Back.End.Challenge2021.CSharp.Cron.Entities
{
    public class Logs
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