using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace srcds_control_api.Models
{
    //public enum Mod
    //{
    //    TF2 = 440,
    //    CSGO = 730
    //}

    //public class MongoServer
    //{
    //    [BsonId]
    //    [BsonRepresentation(BsonType.ObjectId)]
    //    public string Id { get; set; }

    //    [BsonElement("ServerId")]
    //    public int ServerId { get; set; }

    //    [BsonElement("Name")]
    //    public string Name { get; set; }

    //    [BsonElement("IpAddress")]
    //    public string IpAddress { get; set; }

    //    [BsonElement("HostName")]
    //    public string Hostname { get; set; }

    //    [BsonElement("Srcd_servers")]
    //    public MongoSrcdServer[] SrcdServers { get; set; }
    //}

    //public class MongoSrcdServer
    //{
    //    [BsonElement("SrcdId")]
    //    public int SrcdId { get; set; }

    //    [BsonElement("Name")]
    //    public string Name { get; set; }

    //    [BsonElement("Port")]
    //    public int Port { get; set; }

    //    [BsonElement("Status")]
    //    public string Status { get; set; }

    //    [BsonElement("Mod")]
    //    public Mod Mod { get; set; }

    //    [BsonElement("User")]
    //    public string User { get; set; }
    //}
}
