using Elysio.Data;
using Elysio.Mappers;
using Elysio.Models.DTOs;
using Elysio.Models.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Messages.Query;

public class GetMessageQueryV1 : IRequest<MessageDTO>
{
    public Guid Id { get; set; }
}

public class GetMessageQueryV1Validator
    : AbstractValidator<GetMessageQueryV1>
{
    public GetMessageQueryV1Validator()
    {
    }
}

public class GetMessageQueryV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<GetMessageQueryV1, MessageDTO>
{
    async Task<MessageDTO> IRequestHandler<GetMessageQueryV1, MessageDTO>.Handle(
        GetMessageQueryV1 request, CancellationToken cancellationToken)
    {
        var message = await dbContext.Messages.FirstOrDefaultAsync(a => a.Id == request.Id);

        if (message == null)
            throw new NotFoundException("Message", request.Id);

        return message.ToDto();
    }
}