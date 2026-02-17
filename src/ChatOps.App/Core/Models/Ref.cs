namespace ChatOps.App.Core.Models;

/// <summary>
/// Ветка или тег.
/// </summary>
public sealed class Ref
{
    public RefName Name { get; }

    public Ref(RefName name)
    {
        Name = name;
    }
}