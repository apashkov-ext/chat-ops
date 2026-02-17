using ChatOps.App.Core.Models;
using ChatOps.App.Features.Deploy;
using ChatOps.Infra.Core;
using ChatOps.Infra.Integrations.GitLab;
using ChatOps.Infra.Integrations.GitLab.Http;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Refit;
using FindRefResult = OneOf.OneOf<
    ChatOps.App.Features.Deploy.FindRefSuccess,
    ChatOps.App.Features.Deploy.FindRefNotFound,
    ChatOps.App.Features.Deploy.FindRefFailure
>;

namespace ChatOps.Infra.Features.Deploy;

internal interface IFindTagByName
{
    Task<FindRefResult> Execute(RefName name, CancellationToken ct = default);
}

internal sealed class FindTagByName : IFindTagByName
{
    private readonly ITagsApi _tagsApi;
    private readonly IValidator<TagDto> _branchValidator;
    private readonly ILogger<FindTagByName> _logger;
    private readonly GitLabOptions _options;
    
    public FindTagByName(
        IOptions<GitLabOptions> gitLabOptions,
        ITagsApi tagsApi,
        IValidator<TagDto> branchValidator,
        ILogger<FindTagByName> logger)
    {
        _tagsApi = tagsApi;
        _branchValidator = branchValidator;
        _logger = logger;
        _options = gitLabOptions.Value;
    }
    
    public async Task<FindRefResult> Execute(RefName name, CancellationToken ct = default)
    {
        ApiResponse<TagDto> response;

        try
        {
            response = await _tagsApi.Single(
                _options.Project.Value, 
                name.Value, 
                ct);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex) when (IsTransientFailure(ex))
        {
            _logger.LogError(ex, "Error creating pipeline");
            return new FindRefFailure(FindRefFailureReason.Transient);
        }
        
        if (!response.IsSuccessStatusCode)
        {
            var message = response.Error?.Content ??
                          response.Error?.Message ?? $"HTTP status code {(int)response.StatusCode}";
            
            // вполне себе ожидаемая ситуация
            if (TagNotFound(message))
            {
                return new FindRefNotFound();
            }
            
            _logger.LogWarning("GitLab API error: {Message}", message);

            var reason = response.MapToFindRefFailureReason();
            return new FindRefFailure(reason);
        }
        
        if (response.Content is not null)
        {
            var dto = response.Content;
                
            var validation = await _branchValidator.ValidateAsync(dto, ct);
            if (!validation.IsValid)
            {
                var loggable = validation.AsLoggable();
                _logger.LogWarning("GitLab API returned invalid DTO: {@ValidationResult}", loggable);
                return  new FindRefFailure(FindRefFailureReason.Unknown);
            }
                
            var r = new Ref(dto.Name);
            return new FindRefSuccess(r);
        }
        
        _logger.LogWarning("GitLap API returned success status code {StatusCode} but empty content", response.StatusCode);
        return  new FindRefFailure(FindRefFailureReason.Unknown);
    }
    
    private static bool IsTransientFailure(Exception ex) 
        => ex is HttpRequestException or TaskCanceledException;
    
    private static bool TagNotFound(string message)
    {
        return message.Contains("404 Tag Not Found", StringComparison.OrdinalIgnoreCase);
    }
}