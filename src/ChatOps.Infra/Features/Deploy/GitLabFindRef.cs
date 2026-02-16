using ChatOps.App.Core.Models;
using ChatOps.App.Features.Deploy;
using ChatOps.Infra.Integrations.GitLab;
using Microsoft.Extensions.Options;

using FindRefResult = OneOf.OneOf<
    ChatOps.App.Features.Deploy.FindRefSuccess,
    ChatOps.App.Features.Deploy.FindRefNotFound,
    ChatOps.App.Features.Deploy.FindRefFailure
>;

namespace ChatOps.Infra.Features.Deploy;

internal sealed class GitLabFindRef : IFindRef
{
    private readonly GitLabOptions _options;
    private readonly IFindBranchByName _findBranchByName;
    private readonly IFindTagByName _findTagByName;

    public GitLabFindRef(
        IOptions<GitLabOptions> gitLabOptions,
        IFindBranchByName findBranchByName,
        IFindTagByName findTagByName)
    {
        _options = gitLabOptions.Value;
        _findBranchByName = findBranchByName;
        _findTagByName = findTagByName;
    }
    
    public async Task<FindRefResult> Execute(
        RefName refName,
        CancellationToken ct = default)
    {
        var findBranch = await _findBranchByName.Execute(refName, ct);
        return await findBranch.Match<Task<FindRefResult>>(
            success => Task.FromResult<FindRefResult>(success),
            _ => _findTagByName.Execute(refName, ct),
            failure => Task.FromResult<FindRefResult>(failure));
    }
}