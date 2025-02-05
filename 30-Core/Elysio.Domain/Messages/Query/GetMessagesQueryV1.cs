using Elysio.Core.Helpers;
using Elysio.Data;
using Elysio.Mappers;
using Elysio.Models.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Messages.Query;

public class GetMessagesQueryV1 : IRequest<IReadOnlyList<MessageDTO>>
{
    public string UserEmail { get; set; } = string.Empty;
}

public class GetMessagesQueryV1Validator
    : AbstractValidator<GetMessagesQueryV1>
{
    public GetMessagesQueryV1Validator()
    {
        RuleFor(a => a.UserEmail)
            .Matches(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$")
            .WithMessage("Email invalid par rapport à la regex @\"^[\\w-]+(\\.[\\w-]+)*@([\\w-]+\\.)+[a-zA-Z]{2,7}$\"");
    }
}

public class GetMessagesQueryV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<GetMessagesQueryV1, IReadOnlyList<MessageDTO>>
{
    async Task<IReadOnlyList<MessageDTO>> IRequestHandler<GetMessagesQueryV1, IReadOnlyList<MessageDTO>>.Handle(
        GetMessagesQueryV1 request, CancellationToken cancellationToken)
    {
        var user = await CommandHelper.ValidateUser(dbContext, request.UserEmail);
        var messages = await dbContext.Messages.Where(c => c.CreatorId == user.Id).ToListAsync();
        return messages.Select(a => a.ToDto()).ToList();
    }
}