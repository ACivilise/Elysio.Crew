using Elysio.Data;
using Elysio.Mappers;
using Elysio.Models.DTOs;
using Elysio.Models.Enums;
using Elysio.Models.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Messages.Command;

public class UpdateMessageCommandV1 : IRequest<MessageDTO>
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public RolesEnum Role { get; set; }
    public string Content { get; set; } = string.Empty;
}

public class UpdateMessageCommandV1Validator
    : AbstractValidator<UpdateMessageCommandV1>
{
    public UpdateMessageCommandV1Validator()
    {
        RuleFor(a => a.Name)
            .NotNull();
    }
}

public class UpdateMessageCommandV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<UpdateMessageCommandV1, MessageDTO>
{
    async Task<MessageDTO> IRequestHandler<UpdateMessageCommandV1, MessageDTO>.Handle(
        UpdateMessageCommandV1 request, CancellationToken cancellationToken)
    {
        var agent = await dbContext.Messages.FirstOrDefaultAsync(c => c.Id == request.Id);

        if (agent == null)
            throw new NotFoundException("Message", request.Id);

        agent.Role = request.Role;
        agent.Content = request.Content;
        agent.UpdatedAt = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return agent.ToDto();
    }
}