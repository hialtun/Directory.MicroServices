using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MicroServices.Core.Entity
{
    public abstract class DocumentEntity : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}