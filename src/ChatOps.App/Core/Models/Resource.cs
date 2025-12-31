namespace ChatOps.App.Core.Models;

public class Resource
{
    public required ResourceId Id { get; init; }
    public required string Name { get; init; } 
    public required ResourceState State { get; init; }
    public string? Holder { get; init; }
}

public enum ResourceState
{
    Free,
    Reserved
}