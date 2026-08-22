using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using ProjectOrkestra.Domain.Entities;

namespace ProjectOrkestra.Infrastructure.Mappings;

public static class TenantMap
{
    public static void Configure()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(Tenant)))
            return;

        BsonClassMap.RegisterClassMap<Tenant>(map =>
        {
            map.AutoMap();

            map.MapIdMember(x => x.Id).SetIdGenerator(CombGuidGenerator.Instance);
        });
    }
}
