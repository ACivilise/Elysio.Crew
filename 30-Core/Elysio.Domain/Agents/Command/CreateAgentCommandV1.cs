using Elysio.Data;
using Elysio.Entities;
using Elysio.Mappers;
using Elysio.Models.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Agents.Command;

public class CreateAgentCommandV1 : IRequest<AgentDTO>
{
    public string Name { get; set; } = string.Empty;
    public string Prompt { get; set; } = string.Empty;
    public double Temperature { get; set; }
    public string Model { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class CreateAgentCommandV1Validator
    : AbstractValidator<CreateAgentCommandV1>
{
    public CreateAgentCommandV1Validator()
    {
        RuleFor(a => a.Name)
            .NotNull();

        RuleFor(a => a.Prompt)
            .NotNull();
    }
}

public class CreateAgentCommandV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<CreateAgentCommandV1, AgentDTO>
{
    async Task<AgentDTO> IRequestHandler<CreateAgentCommandV1, AgentDTO>.Handle(
        CreateAgentCommandV1 request, CancellationToken cancellationToken)
    {
        // on crée une conversation associé à cet utilisateur
        var newId = Guid.NewGuid();
        dbContext.Agents.Add(new Agent
        {
            Id = newId,
            Name = request.Name,
            Prompt = request.Prompt,
            Temperature = request.Temperature,
            Model = request.Model,
            Description = request.Description,
            CreatedAt = DateTimeOffset.UtcNow
        });

        await dbContext.SaveChangesAsync(cancellationToken);

        var newAgent = await dbContext.Agents.FirstAsync(c => c.Id == newId);

        return newAgent.ToDto();
    }
}