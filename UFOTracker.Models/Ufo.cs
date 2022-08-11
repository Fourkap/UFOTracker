using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UFOTracker.Models
{
    [BsonIgnoreExtraElements]
    public class Ufo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("datetime")]
        public DateTime? DateAndTime { get; set; }
        [BsonElement("city")]
        public string? City { get; set; }
        [BsonElement("state")]
        public string? State { get; set; }
        [BsonElement("country")]
        public string? Country { get; set; }
        [BsonElement("shape")]
        public string? Shape { get; set; }
        [BsonElement("comments")]
        public string? Comments { get; set; }
        [BsonElement("date posted")]
        public DateTime? DatePosted { get; set; }
        [BsonElement("latitude")]
        public string? Latitude { get; set; }
        [BsonElement("longitude")]
        public string? Longitude { get; set; }
    }
}
