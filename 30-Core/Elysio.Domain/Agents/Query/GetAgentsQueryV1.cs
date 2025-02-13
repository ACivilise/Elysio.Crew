using Elysio.Data;
using Elysio.Mappers;
using Elysio.Models.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Agents.Query;

public class GetAgentsQueryV1 : IRequest<IReadOnlyList<AgentDTO>>
{
}

public class GetAgentsQueryV1Validator
    : AbstractValidator<GetAgentsQueryV1>
{
    public GetAgentsQueryV1Validator()
    {
    }
}

public class GetAgentsQueryV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<GetAgentsQueryV1, IReadOnlyList<AgentDTO>>
{
    async Task<IReadOnlyList<AgentDTO>> IRequestHandler<GetAgentsQueryV1, IReadOnlyList<AgentDTO>>.Handle(
        GetAgentsQueryV1 request, CancellationToken cancellationToken)
    {
        var agents = dbContext.Agents;
        return await agents.Select(a => a.ToDto()).ToListAsync();
    }
}