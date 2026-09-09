using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using ProjectOrkestra.Domain.Entities;

namespace ProjectOrkestra.Infrastructure.Mappings;

public static class UserMap
{
    public static void Configure()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(User)))
            return;

        BsonClassMap.RegisterClassMap<User>(map =>
        {
            map.AutoMap();

            map.MapIdMember(x => x.Id).SetIdGenerator(CombGuidGenerator.Instance);
        });
    }
}
