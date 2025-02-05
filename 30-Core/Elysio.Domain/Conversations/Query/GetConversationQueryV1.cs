using Elysio.Core.Helpers;
using Elysio.Data;
using Elysio.Mappers;
using Elysio.Models.DTOs;
using Elysio.Models.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Conversations.Query;

public class GetConversationQueryV1 : IRequest<ConversationDTO>
{
    public string UserEmail { get; set; } = string.Empty;
    public Guid Id { get; set; }
}

public class GetConversationQueryV1Validator
    : AbstractValidator<GetConversationQueryV1>
{
    public GetConversationQueryV1Validator()
    {
        RuleFor(a => a.UserEmail)
            .Matches(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$")
            .WithMessage("Email invalid par rapport à la regex @\"^[\\w-]+(\\.[\\w-]+)*@([\\w-]+\\.)+[a-zA-Z]{2,7}$\"");
    }
}

public class GetConversationQueryV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<GetConversationQueryV1, ConversationDTO>
{
    async Task<ConversationDTO> IRequestHandler<GetConversationQueryV1, ConversationDTO>.Handle(
        GetConversationQueryV1 request, CancellationToken cancellationToken)
    {
        var user = await CommandHelper.ValidateUser(dbContext, request.UserEmail);
        var conversation = await dbContext.Conversations.FirstOrDefaultAsync(a => a.CreatorId == user.Id && a.Id == request.Id);

        if (conversation == null)
            throw new NotFoundException("Conversation", request.Id);

        return conversation.ToDto();
    }
}