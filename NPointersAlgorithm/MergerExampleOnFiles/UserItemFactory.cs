using System.Linq;
using AutoFixture;

namespace NPointersAlgorithm.MergerExampleOnFiles;

public static class UserItemFactory
{
    private static readonly Fixture Fixture = new();

    public static UserItem Create(long timestamp)
    {
        var educations = Fixture.CreateMany<Education>(5).ToArray();
        return Fixture.Build<UserItem>()
            .With(x => x.Educations, educations)
            .With(x => x.Timestamp, timestamp)
            .Create();
    }
}