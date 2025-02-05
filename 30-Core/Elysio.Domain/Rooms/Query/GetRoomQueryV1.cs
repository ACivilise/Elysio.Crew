using Elysio.Core.Helpers;
using Elysio.Data;
using Elysio.Mappers;
using Elysio.Models.DTOs;
using Elysio.Models.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Rooms.Query;

public class GetRoomQueryV1 : IRequest<RoomDTO>
{
    public string UserEmail { get; set; } = string.Empty;
    public Guid Id { get; set; }
}

public class GetRoomQueryV1Validator
    : AbstractValidator<GetRoomQueryV1>
{
    public GetRoomQueryV1Validator()
    {
        RuleFor(a => a.UserEmail)
            .Matches(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$")
            .WithMessage("Email invalid par rapport à la regex @\"^[\\w-]+(\\.[\\w-]+)*@([\\w-]+\\.)+[a-zA-Z]{2,7}$\"");
    }
}

public class GetRoomQueryV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<GetRoomQueryV1, RoomDTO>
{
    async Task<RoomDTO> IRequestHandler<GetRoomQueryV1, RoomDTO>.Handle(
        GetRoomQueryV1 request, CancellationToken cancellationToken)
    {
        var user = await CommandHelper.ValidateUser(dbContext, request.UserEmail);
        var room = await dbContext.Rooms.FirstOrDefaultAsync(a => a.CreatorId == user.Id && a.Id == request.Id);

        if (room == null)
            throw new NotFoundException("Room", request.Id);

        return room.ToDto();
    }
}