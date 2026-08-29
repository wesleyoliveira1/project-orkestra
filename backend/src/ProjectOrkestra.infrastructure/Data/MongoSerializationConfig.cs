using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ProjectOrkestra.Infrastructure.Data;

public static class MongoSerializationConfig
{
    private static bool _configured = false;
    private static readonly object _lock = new object();

    public static void Configure()
    {
        if (_configured)
            return;

        lock (_lock)
        {
            if (_configured)
                return;

            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            _configured = true;
        }
    }
}
