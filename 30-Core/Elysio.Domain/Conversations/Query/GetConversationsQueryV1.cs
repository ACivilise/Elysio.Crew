using Elysio.Core.Helpers;
using Elysio.Data;
using Elysio.Mappers;
using Elysio.Models.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Conversations.Query;

public class GetConversationsQueryV1 : IRequest<IReadOnlyList<ConversationDTO>>
{
    public string UserEmail { get; set; } = string.Empty;
}

public class GetConversationsQueryV1Validator
    : AbstractValidator<GetConversationsQueryV1>
{
    public GetConversationsQueryV1Validator()
    {
        RuleFor(a => a.UserEmail)
            .Matches(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$")
            .WithMessage("Email invalid par rapport à la regex @\"^[\\w-]+(\\.[\\w-]+)*@([\\w-]+\\.)+[a-zA-Z]{2,7}$\"");
    }
}

public class GetConversationsQueryV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<GetConversationsQueryV1, IReadOnlyList<ConversationDTO>>
{
    async Task<IReadOnlyList<ConversationDTO>> IRequestHandler<GetConversationsQueryV1, IReadOnlyList<ConversationDTO>>.Handle(
        GetConversationsQueryV1 request, CancellationToken cancellationToken)
    {
        var user = await CommandHelper.ValidateUser(dbContext, request.UserEmail);
        var conversations = await dbContext.Conversations.Where(c => c.CreatorId == user.Id).ToListAsync();
        return conversations.Select(a => a.ToDto()).ToList();
    }
}