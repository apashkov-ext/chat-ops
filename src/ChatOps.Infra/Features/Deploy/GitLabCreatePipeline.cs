using System.Net;
using ChatOps.App.Core.Models;
using ChatOps.App.Features.Deploy;
using ChatOps.Infra.Integrations.GitLab;
using ChatOps.Infra.Integrations.GitLab.Http;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OneOf;
using Refit;

namespace ChatOps.Infra.Features.Deploy;

internal sealed class GitLabCreatePipeline : ICreatePipeline
{
    private readonly GitLabOptions _options;
    private readonly IPipelineApi _pipelineApi;
    // TODO
    private readonly IValidator<CreatedPipelineDto> _validator;
    private readonly ILogger<GitLabCreatePipeline> _logger;

    public GitLabCreatePipeline(IOptions<GitLabOptions> gitLabOptions,
        IPipelineApi pipelineApi,
        IValidator<CreatedPipelineDto> validator,
        ILogger<GitLabCreatePipeline> logger)
    {
        _options = gitLabOptions.Value;
        _pipelineApi = pipelineApi;
        _validator = validator;
        _logger = logger;
    }
    
    public async Task<OneOf<CreatePipelineSuccess, CreatePipelineFailure>> Execute(
        Resource resource, 
        Ref @ref, 
        IEnumerable<Variable> variables,
        CancellationToken ct = default)
    {
        ApiResponse<CreatedPipelineDto> response;

        try
        {
            response = await _pipelineApi.Create(_options.Project.Value, @ref.Value, ct);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex) when (IsTransientFailure(ex))
        {
            _logger.LogError(ex, "Error creating pipeline");
            return new CreatePipelineFailure(CreatePipelineFailureReason.Transient);
        }

        if (response.IsSuccessStatusCode)
        {
            if (response.Content is not null)
            {
                var dto = response.Content;
                var pipeline = new Pipeline(dto.Id, dto.WebUrl);
                return new CreatePipelineSuccess(pipeline);
            }
            
            _logger.LogWarning("GitLap API returned success status code {StatusCode} but empty content", response.StatusCode);
            return  new CreatePipelineFailure(CreatePipelineFailureReason.Unknown);
        }
        
        var message = response.Error?.Content ?? response.Error?.Message ?? $"HTTP status code {(int)response.StatusCode}";
        _logger.LogWarning("GitLab API error: {Message}", message);

        var reason = MapFailureReason(response.StatusCode);
        return new CreatePipelineFailure(reason);
    }

    private static bool IsTransientFailure(Exception ex) 
        => ex is HttpRequestException or TaskCanceledException;
    
    
    private static CreatePipelineFailureReason MapFailureReason(HttpStatusCode code)
    {
        switch (code)
        {
            case HttpStatusCode.TooManyRequests:
            case >= HttpStatusCode.InternalServerError:
            case HttpStatusCode.RequestTimeout:
                return CreatePipelineFailureReason.Transient;
            case HttpStatusCode.BadRequest 
                or HttpStatusCode.Unauthorized 
                or HttpStatusCode.Forbidden
                or HttpStatusCode.Conflict 
                or HttpStatusCode.UnprocessableEntity:
                return CreatePipelineFailureReason.Permanent;
            default:
                return CreatePipelineFailureReason.Unknown;
        }
    }
}