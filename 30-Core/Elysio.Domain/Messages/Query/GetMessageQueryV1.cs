using Elysio.Core.Helpers;
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
    public string UserEmail { get; set; } = string.Empty;
    public Guid Id { get; set; }
}

public class GetMessageQueryV1Validator
    : AbstractValidator<GetMessageQueryV1>
{
    public GetMessageQueryV1Validator()
    {
        RuleFor(a => a.UserEmail)
            .Matches(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$")
            .WithMessage("Email invalid par rapport à la regex @\"^[\\w-]+(\\.[\\w-]+)*@([\\w-]+\\.)+[a-zA-Z]{2,7}$\"");
    }
}

public class GetMessageQueryV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<GetMessageQueryV1, MessageDTO>
{
    async Task<MessageDTO> IRequestHandler<GetMessageQueryV1, MessageDTO>.Handle(
        GetMessageQueryV1 request, CancellationToken cancellationToken)
    {
        var user = await CommandHelper.ValidateUser(dbContext, request.UserEmail);
        var message = await dbContext.Messages.FirstOrDefaultAsync(a => a.CreatorId == user.Id && a.Id == request.Id);

        if (message == null)
            throw new NotFoundException("Message", request.Id);

        return message.ToDto();
    }
}