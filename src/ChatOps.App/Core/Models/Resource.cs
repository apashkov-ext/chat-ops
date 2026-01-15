namespace ChatOps.App.Core.Models;

public sealed class Resource
{
    public ResourceId Id { get; }
    public ResourceState State { get; private set; }
    public HolderId? Holder { get; private set; }

    public Resource(ResourceId id, ResourceState state, HolderId? holder)
    {
        Id = id;
        State = state;
        Holder = holder;
    }

    public void Reserve(HolderId holder)
    {
        State = ResourceState.Reserved;
        Holder = holder;
    }

    public void Free()
    {
        State = ResourceState.Free;
        Holder = null;
    }
}

public enum ResourceState
{
    Free,
    Reserved
}