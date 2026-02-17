using ChatOps.App.Core;

namespace ChatOps.Infra.Integrations.GitLab.Models;

internal sealed class GitLabProjectId : ValueObject
{
    public string Value { get; }
    
    public GitLabProjectId(NamespacedPath path)
    {
        Value = path.Value;
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    protected override string GetStringRepresentation()
    {
        return Value;
    }
}