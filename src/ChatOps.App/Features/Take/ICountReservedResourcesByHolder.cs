using ChatOps.App.Core.Models;

namespace ChatOps.App.Features.Take;

public interface ICountReservedResourcesByHolder
{
    Task<int> Execute(
        HolderId holder,
        CancellationToken ct = default
        );
}