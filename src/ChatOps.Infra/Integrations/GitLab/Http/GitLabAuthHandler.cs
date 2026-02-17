using Microsoft.Extensions.Options;

namespace ChatOps.Infra.Integrations.GitLab.Http;

internal sealed class GitLabAuthHandler : DelegatingHandler
{
    private readonly GitLabOptions _options;

    public GitLabAuthHandler(IOptions<GitLabOptions> gitLabOptions)
    {
        _options = gitLabOptions.Value;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken ct)
    {
        var token = _options.Token;
        request.Headers.Add("PRIVATE-TOKEN", token);
        return await base.SendAsync(request, ct);
    }
}