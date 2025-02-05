using Elysio.Core.Helpers;
using Elysio.Data;
using Elysio.Mappers;
using Elysio.Models.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Rooms.Query;

public class GetRoomsQueryV1 : IRequest<IReadOnlyList<RoomDTO>>
{
    public string UserEmail { get; set; } = string.Empty;
}

public class GetRoomsQueryV1Validator
    : AbstractValidator<GetRoomsQueryV1>
{
    public GetRoomsQueryV1Validator()
    {
        RuleFor(a => a.UserEmail)
            .Matches(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$")
            .WithMessage("Email invalid par rapport à la regex @\"^[\\w-]+(\\.[\\w-]+)*@([\\w-]+\\.)+[a-zA-Z]{2,7}$\"");
    }
}

public class GetRoomsQueryV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<GetRoomsQueryV1, IReadOnlyList<RoomDTO>>
{
    async Task<IReadOnlyList<RoomDTO>> IRequestHandler<GetRoomsQueryV1, IReadOnlyList<RoomDTO>>.Handle(
        GetRoomsQueryV1 request, CancellationToken cancellationToken)
    {
        var user = await CommandHelper.ValidateUser(dbContext, request.UserEmail);
        var rooms = await dbContext.Rooms.Where(c => c.CreatorId == user.Id).ToListAsync();
        return rooms.Select(r => r.ToDto()).ToList();
    }
}