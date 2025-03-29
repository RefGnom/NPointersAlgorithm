using System;

namespace NPointersAlgorithm.MergerExampleOnFiles;

public class UserItem
{
    public required Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required Address Address { get; set; }
    public required Education[] Educations { get; set; }
    public required long Timestamp { get; set; }
}

public class Address
{
    public required string Country { get; set; }
    public required string Region { get; set; }
    public required string City { get; set; }
    public required string Street { get; set; }
    public required string House { get; set; }
}

public class Education
{
    public required string InstitutionName { get; set; }
    public required Address InstitutionAddress { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime? EndDate { get; set; }
    public required bool IsEnded { get; set; }
}