using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using ProjectOrkestra.Domain.Entities;

namespace ProjectOrkestra.Infrastructure.Mappings;

public static class OrganizationMap {
    public static void Configure() {
        if(BsonClassMap.IsClassMapRegistered(typeof(Organization)))
            return;

        BsonClassMap.RegisterClassMap<Organization>(map => {
            map.AutoMap();

            map.MapIdMember(x => x.Id)
                .SetIdGenerator(CombGuidGenerator.Instance);
        });
    }
}