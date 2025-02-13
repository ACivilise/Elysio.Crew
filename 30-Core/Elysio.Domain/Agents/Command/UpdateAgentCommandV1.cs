using Elysio.Data;
using Elysio.Mappers;
using Elysio.Models.DTOs;
using Elysio.Models.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Agents.Command;

public class UpdateAgentCommandV1 : IRequest<AgentDTO>
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Prompt { get; set; } = string.Empty;
    public double Temperature { get; set; }
    public string Model { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdateAgentCommandV1Validator
    : AbstractValidator<UpdateAgentCommandV1>
{
    public UpdateAgentCommandV1Validator()
    {
        RuleFor(a => a.Name)
            .NotNull();

        RuleFor(a => a.Prompt)
            .NotNull();
    }
}

public class UpdateAgentCommandV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<UpdateAgentCommandV1, AgentDTO>
{
    async Task<AgentDTO> IRequestHandler<UpdateAgentCommandV1, AgentDTO>.Handle(
        UpdateAgentCommandV1 request, CancellationToken cancellationToken)
    {
        var agent = await dbContext.Agents.FirstOrDefaultAsync(c => c.Id == request.Id);

        if (agent == null)
            throw new NotFoundException("Agent", request.Id);

        agent.Name = request.Name;
        agent.Prompt = request.Prompt;
        agent.Temperature = request.Temperature;
        agent.Model = request.Model;
        agent.Description = request.Description;
        agent.UpdatedAt = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return agent.ToDto();
    }
}