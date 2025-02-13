using Elysio.Data;
using Elysio.Mappers;
using Elysio.Models.DTOs;
using Elysio.Models.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Agents.Query;

public class GetAgentQueryV1 : IRequest<AgentDTO>
{
    public Guid Id { get; set; }
}

public class GetAgentQueryV1Validator
    : AbstractValidator<GetAgentQueryV1>
{
    public GetAgentQueryV1Validator()
    {
    }
}

public class GetAgentQueryV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<GetAgentQueryV1, AgentDTO>
{
    async Task<AgentDTO> IRequestHandler<GetAgentQueryV1, AgentDTO>.Handle(
        GetAgentQueryV1 request, CancellationToken cancellationToken)
    {
        var agent = await dbContext.Agents.FirstOrDefaultAsync(a => a.Id == request.Id);

        if (agent == null)
            throw new NotFoundException("Agent", request.Id);

        return agent.ToDto();
    }
}