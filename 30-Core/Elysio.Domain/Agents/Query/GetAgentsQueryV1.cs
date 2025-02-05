using Elysio.Core.Helpers;
using Elysio.Data;
using Elysio.Mappers;
using Elysio.Models.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Agents.Query;

public class GetAgentsQueryV1 : IRequest<IReadOnlyList<AgentDTO>>
{
    public string UserEmail { get; set; } = string.Empty;
}

public class GetAgentsQueryV1Validator
    : AbstractValidator<GetAgentsQueryV1>
{
    public GetAgentsQueryV1Validator()
    {
        RuleFor(a => a.UserEmail)
            .Matches(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$")
            .WithMessage("Email invalid par rapport à la regex @\"^[\\w-]+(\\.[\\w-]+)*@([\\w-]+\\.)+[a-zA-Z]{2,7}$\"");
    }
}

public class GetAgentsQueryV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<GetAgentsQueryV1, IReadOnlyList<AgentDTO>>
{
    async Task<IReadOnlyList<AgentDTO>> IRequestHandler<GetAgentsQueryV1, IReadOnlyList<AgentDTO>>.Handle(
        GetAgentsQueryV1 request, CancellationToken cancellationToken)
    {
        var user = await CommandHelper.ValidateUser(dbContext, request.UserEmail);
        var agents = await dbContext.Agents.Where(c => c.CreatorId == user.Id).ToListAsync();
        return agents.Select(a => a.ToDto()).ToList();
    }
}