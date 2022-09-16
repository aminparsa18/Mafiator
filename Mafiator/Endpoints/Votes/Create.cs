using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Votes;
using Mafiator.Entities;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Votes;

[Authorize]
[Produces("application/x-msgpack")]
public class Create : EndpointBaseAsync
    .WithRequest<VoteCreateRequest>
    .WithoutResult
{
    private readonly IUnitOfWork _unitOfWork;

    public Create(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/votes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("Votes.Create", "", "Creates a new vote.")]
    [OpenApiTag("Votes Endpoints")]
    public override async Task HandleAsync(VoteCreateRequest request, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.Vote.AddRangeFast(request.Targets.Select(s => new Vote()
        {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
            GameId = request.GameId,
            TargetId = s,
            VoterId = request.VoterId
        }));
    }
}