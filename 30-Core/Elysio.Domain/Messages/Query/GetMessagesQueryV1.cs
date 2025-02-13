using Elysio.Data;
using Elysio.Mappers;
using Elysio.Models.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Messages.Query;

public class GetMessagesQueryV1 : IRequest<IReadOnlyList<MessageDTO>>
{
}

public class GetMessagesQueryV1Validator
    : AbstractValidator<GetMessagesQueryV1>
{
    public GetMessagesQueryV1Validator()
    {
    }
}

public class GetMessagesQueryV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<GetMessagesQueryV1, IReadOnlyList<MessageDTO>>
{
    async Task<IReadOnlyList<MessageDTO>> IRequestHandler<GetMessagesQueryV1, IReadOnlyList<MessageDTO>>.Handle(
        GetMessagesQueryV1 request, CancellationToken cancellationToken)
    {
        var messages = dbContext.Messages;
        return await messages.Select(a => a.ToDto()).ToListAsync();
    }
}