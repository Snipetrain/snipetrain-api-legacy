using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace srcds_control_api.Models
{
    public class MongoUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("UserName")]
        public string UserName { get; set; }

        [BsonElement("Salt")]
        public string Salt { get; set; }

        [BsonElement("HashedPassword")]
        public string HashedPassword { get; set; }

        [BsonElement("Role")]
        public string Role { get; set; }

        [BsonElement("Enabled")]
        public bool Enabled { get; set; }
    }
}
