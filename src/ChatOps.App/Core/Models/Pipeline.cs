namespace ChatOps.App.Core.Models;

public sealed class Pipeline : ValueObject
{
    public long Id { get; }
    public PipelineLink Link { get; }

    public Pipeline(long id, PipelineLink link)
    {
        Id = id;
        Link = link;
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Id;
        yield return Link;
    }

    protected override string GetStringRepresentation()
    {
        return $"#{Id}: {Link}";
    }
}