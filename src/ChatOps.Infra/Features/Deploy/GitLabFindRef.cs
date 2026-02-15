using ChatOps.App.Core.Models;
using ChatOps.App.Features.Deploy;
using ChatOps.Infra.Integrations.GitLab;
using ChatOps.Infra.Integrations.GitLab.Http;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ChatOps.Infra.Features.Deploy;

internal sealed class GitLabFindRef : IFindRef
{
    private readonly GitLabOptions _options;
    private readonly IRefApi _refApi;
    // TODO
    private readonly IValidator<BranchDto> _validator;
    private readonly ILogger<GitLabFindRef> _logger;

    public GitLabFindRef(
        IOptions<GitLabOptions> gitLabOptions,
        IRefApi pipelineApi,
        IValidator<BranchDto> validator,
        ILogger<GitLabFindRef> logger)
    {
        _options = gitLabOptions.Value;
        _refApi = pipelineApi;
        _validator = validator;
        _logger = logger;
    }
    
    public Task<Ref?> Execute(Ref @ref, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}